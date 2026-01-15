using ES.Core.Entities;
using ES.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace ES.Web.Services
{
    public class TenderInquiryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TenderInquiryService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ---------------------------------------------------------
        // Save Inquiry + Send Email Notification
        // ---------------------------------------------------------
        public async Task<bool> SaveInquiryAsync(InquiryViewModel model)
        {
            string? filePath = await SaveAttachmentAsync(model.Attachment);

            var inquiry = new TenderInquiry
            {
                TenderId = model.TenderId,
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                InquiryText = model.Inquiry,
                AttachmentUrl = filePath,
                CreatedAt = DateTime.UtcNow
            };

            _context.TenderInquiries.Add(inquiry);
            await _context.SaveChangesAsync();

            // -------------------------------------------
            // Send Email Notification
            // -------------------------------------------
            var smtpSettings = await _context.SmtpSettings.FirstOrDefaultAsync();

            if (smtpSettings != null)
            {
                string tenderTitle = await _context.Tenders
                    .Where(t => t.Id == model.TenderId)
                    .Select(t => t.Title)
                    .FirstOrDefaultAsync() ?? "Tender Inquiry";

                await SendNotificationEmail(model, tenderTitle, smtpSettings);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No SMTP settings found for sending notification email.");
            }

            return true;
        }

        // ---------------------------------------------------------
        // Upload File
        // ---------------------------------------------------------
        private async Task<string?> SaveAttachmentAsync(IFormFile? file)
        {
            if (file == null)
                return null;

            const string folder = "CMS/documents/Tenders";
            string uploadPath = Path.Combine(_environment.WebRootPath, folder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string fullPath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/" + folder + "/" + fileName;
        }

        // ---------------------------------------------------------
        // Send Notification Email
        // ---------------------------------------------------------
        private async Task SendNotificationEmail(InquiryViewModel model, string tenderTitle, SmtpSettings smtpSettings)
        {
            try
            {
                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings.Email, "Email Solution Team"),
                    Subject = "New Tender Inquiry Submitted",
                    Body = $@"
                    <html>
                        <body style='font-family:Arial;color:#333;'>
                            <h2>New Tender Inquiry Received</h2>

                            <p><b>Tender:</b> {tenderTitle}</p>
                            <p><b>Name:</b> {model.Name}</p>
                            <p><b>Email:</b> {model.Email}</p>
                            <p><b>Phone:</b> {model.Phone}</p>
                            <p><b>Inquiry:</b> {System.Web.HttpUtility.HtmlEncode(model.Inquiry)}</p>

                            <p><b>Submitted At:</b> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>

                            <hr />
                            <p>This is an automated message from the CMS system.</p>
                        </body>
                    </html>",
                    IsBodyHtml = true
                };

                // Send to admin
                //  mailMessage.To.Add(smtpSettings.Email);
                mailMessage.To.Add(smtpSettings.Email);
                // Also send copy to user if they entered email
                //if (!string.IsNullOrEmpty(model.Email))
                //{
                //    mailMessage.CC.Add(model.Email);
                //}

                using var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(smtpSettings.Email, smtpSettings.Password),
                    EnableSsl = smtpSettings.EnableSsl
                };

                await smtpClient.SendMailAsync(mailMessage);
                System.Diagnostics.Debug.WriteLine("Inquiry notification email sent successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send inquiry email: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // Get Tender Slug
        // ---------------------------------------------------------
        public async Task<string?> GetTenderSlugAsync(int tenderId)
        {
            return await _context.Tenders
                .Where(t => t.Id == tenderId)
                .Select(t => t.Slug)
                .FirstOrDefaultAsync();
        }
    }
}
