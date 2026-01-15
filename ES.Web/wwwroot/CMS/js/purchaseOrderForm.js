// purchaseOrderForm.js — Fully rewritten, fixed, and enhanced

const baseUrl = window.appBaseUrl || '/';
const currentLanguage = window.currentLanguage || (navigator.language || 'en-US');

function $id(id) { return document.getElementById(id); }

// ------------------- TinyMCE -------------------
function initializeTinyMCEForPage() {
    const selectors = ['#tinyMCEDetails', '#tinyMCEPricesOffered'].join(',');
    if (typeof tinymce !== 'undefined') {
        tinymce.init({
            ...(window.tinyMCEInitSettings || {}),
            selector: selectors
        });
    } else console.warn("TinyMCE not loaded.");
}

// ------------------- Image Preview -------------------
function previewImage(event, previewElementId) {
    const input = event.target;
    const preview = $id(previewElementId);
    if (!preview) return;

    if (input.files && input.files[0]) {
        const reader = new FileReader();
        reader.onload = e => {
            preview.src = e.target.result;
            const hiddenInput = $id(input.id + 'Url');
            if (hiddenInput) hiddenInput.value = "true";
        };
        reader.readAsDataURL(input.files[0]);
    }
}

// ------------------- Image Validation -------------------
function validateImage(input, errorElementId) {
    const errorEl = $id(errorElementId);
    if (errorEl) errorEl.textContent = "";
    if (!input || !input.files.length) return true;

    const file = input.files[0];
    const validTypes = ["image/jpeg", "image/jpg", "image/png", "image/webp"];
    const maxSize = 20 * 1024 * 1024; // 20MB

    if (!validTypes.includes(file.type)) {
        if (errorEl) errorEl.textContent = currentLanguage === 'ar-JO' ?
            'نوع الملف غير صالح. مسموح JPG, PNG, WEBP فقط.' :
            'Invalid file type. Only JPG, PNG, WEBP allowed.';
        return false;
    }
    if (file.size > maxSize) {
        if (errorEl) errorEl.textContent = currentLanguage === 'ar-JO' ?
            'حجم الملف يجب أن يكون 20MB أو أقل.' :
            'File size must be 20MB or less.';
        return false;
    }
    return true;
}

// ------------------- Select2 -------------------
function initializeSelect2Safe(selector, placeholderText) {
    if (typeof $ === 'undefined' || !$(selector).length) return;
    $(selector).select2({
        theme: "bootstrap-5",
        placeholder: placeholderText || '',
        minimumResultsForSearch: 0
    });
}

// ------------------- Dropzone -------------------
Dropzone.autoDiscover = false;
let purchaseOrderFilesDropzoneInstance = null;
const removedPurchaseOrderFiles = [];

function initPurchaseOrderFilesDropzone() {
    const dzEl = document.querySelector("#purchaseOrderFilesDropzone");
    const previewsContainer = document.querySelector("#sortablePurchaseOrderFiles");
    if (!dzEl || !previewsContainer) return;

    purchaseOrderFilesDropzoneInstance = new Dropzone("#purchaseOrderFilesDropzone", {
        previewsContainer: "#sortablePurchaseOrderFiles",
        url: "#",
        autoProcessQueue: false,
        maxFiles: 20,
        maxFilesize: 20,
        acceptedFiles: ".pdf,.doc,.docx,.jpg,.jpeg,.png,.webp",
        addRemoveLinks: true,
        dictDefaultMessage: currentLanguage === 'ar-JO' ?
            'Drag and drop files هنا أو اضغط للتحميل' :
            'Drag and drop files here or click to upload',
        init: function () {
            const dz = this;

            // Load existing files
            if (window.existingPurchaseOrderFiles?.length) {
                window.existingPurchaseOrderFiles.forEach(f => {
                    const fileUrl = f.FileUrl.startsWith('http') ? f.FileUrl : baseUrl + "CMS/documents/PurchaseOrders/" + f.FileUrl;
                    const fileName = f.FileName || f.FileUrl.split('/').pop();
                    const mockFile = { name: fileName, size: f.FileSize || 12345, accepted: true, status: Dropzone.SUCCESS, existingId: f.Id };

                    dz.emit("addedfile", mockFile);

                    if (fileName.match(/\.(jpg|jpeg|png|webp)$/i)) dz.emit("thumbnail", mockFile, fileUrl);
                    else {
                        const previewEl = mockFile.previewElement;
                        if (previewEl) {
                            const thumb = previewEl.querySelector('.dz-image');
                            if (thumb) thumb.innerHTML = `<img src="${baseUrl}CMS/assets/images/icons/fileIcon.png" style="max-width:100%;max-height:100%;">`;
                        }
                    }

                    const previewEl = mockFile.previewElement;
                    if (previewEl) {
                        // Add download button
                        const downloadBtn = document.createElement("a");
                        downloadBtn.href = fileUrl;
                        downloadBtn.download = fileName;
                        downloadBtn.className = "btn btn-sm btn-primary mt-1";
                        downloadBtn.textContent = currentLanguage === 'ar-JO' ? "تحميل" : "Download";
                        previewEl.appendChild(downloadBtn);
                    }

                    if (mockFile.previewElement) mockFile.previewElement.classList.add("dz-success", "dz-complete");
                    dz.files.push(mockFile);
                });
            }

            dz.on("removedfile", file => {
                if (file.existingId) removedPurchaseOrderFiles.push(file.existingId);
                else removedPurchaseOrderFiles.push(file.name);
            });

            // Make files sortable
            try {
                Sortable.create(previewsContainer, {
                    animation: 200,
                    draggable: ".dz-preview",
                    onEnd: function () {
                        const sorted = previewsContainer.querySelectorAll(".dz-preview");
                        const newOrder = [];
                        sorted.forEach(previewEl => {
                            const nameNode = previewEl.querySelector("[data-dz-name]");
                            if (nameNode) newOrder.push(nameNode.innerText.trim());
                        });
                        const reordered = [];
                        newOrder.forEach(n => {
                            const matched = dz.files.find(f => f.name === n);
                            if (matched) reordered.push(matched);
                        });
                        dz.files.forEach(f => { if (!reordered.includes(f)) reordered.push(f); });
                        dz.files = reordered;
                    }
                });
            } catch (ex) { console.warn("Sortable init failed", ex); }
        }
    });
}

// ------------------- Form Submission -------------------
function initFormSubmission() {
    const form = $id("purchaseOrderForm");
    if (!form) return;

    form.addEventListener("submit", e => {
        e.preventDefault();
        if (typeof tinymce !== 'undefined') tinymce.triggerSave();

        let isValid = true;
        const purchaseOrderImgInput = $id("PurchaseOrderImage");
        if (purchaseOrderImgInput && !validateImage(purchaseOrderImgInput, "purchaseOrderImageError")) isValid = false;
        if (!isValid) return;

        const formData = new FormData(form);

        // Append new uploaded files
        purchaseOrderFilesDropzoneInstance?.files.forEach(file => {
            if (!removedPurchaseOrderFiles.includes(file.name) && (file instanceof File || file instanceof Blob)) {
                formData.append("PurchaseOrderFiles", file, file.name);
            }
        });

        // Append removed files (existing IDs)
        removedPurchaseOrderFiles.forEach((id, i) => formData.append(`RemovedFiles[${i}]`, id));

        fetch(form.action || window.location.href, { method: "POST", body: formData })
            .then(async res => {
                let data;
                try { data = await res.json(); } catch { data = null; }
                if (res.ok && data?.success) {
                    Swal.fire({ icon: 'success', title: currentLanguage === 'ar-JO' ? 'تمت العملية بنجاح!' : 'Done successfully!', showConfirmButton: false, timer: 1500 })
                        .then(() => { const base = baseUrl.endsWith("/") ? baseUrl : baseUrl + "/"; window.location.href = data.id ? `${base}EsAdmin/PurchaseOrdersTranslates?purchaseOrderId=${data.id}` : window.location.reload(); });
                } else {
                    Swal.fire({ icon: 'error', title: (data?.message) || (currentLanguage === 'ar-JO' ? 'حدث خطأ أثناء إرسال النموذج.' : 'An error occurred while submitting the form.'), showConfirmButton: true });
                }
            })
            .catch(err => { console.error(err); Swal.fire({ icon: 'error', title: currentLanguage === 'ar-JO' ? 'حدث خطأ أثناء إرسال النموذج.' : 'An error occurred while submitting the form.', showConfirmButton: true }); });
    });
}

// ------------------- DOMContentLoaded -------------------
document.addEventListener("DOMContentLoaded", () => {

  
    initializeSelect2Safe('#CategoryDropdown', currentLanguage === 'ar-JO' ? 'حدد القسم' : 'Select the category');
    initializeSelect2Safe('#relatedCategoriesDropdown', currentLanguage === 'ar-JO' ? 'حدد الأقسام ذات الصلة (اختياري)' : 'Select related categories (optional)');
    initializeSelect2Safe('#FormDropdown', currentLanguage === 'ar-JO' ? 'حدد النموذج' : 'Select the form');
    $('#purchaseOrderMaterialsSelect').select2({ theme: "bootstrap-5", placeholder: currentLanguage === 'ar-JO' ? "اختر المواد" : "Select materials" });

    initializeTinyMCEForPage();
    initPurchaseOrderFilesDropzone();
    initFormSubmission();

    const purchaseOrderInput = document.querySelector("input[name='PurchaseOrderImage']");
    if (purchaseOrderInput) {
        if (!purchaseOrderInput.id) purchaseOrderInput.id = 'PurchaseOrderImage';
        purchaseOrderInput.addEventListener('change', ev => previewImage(ev, 'PurchaseOrderImagePreview'));
    }
});
