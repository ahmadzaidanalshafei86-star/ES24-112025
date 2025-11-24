using Microsoft.AspNetCore.Mvc.Rendering;

namespace ES.Core.Consts
{
    public static class JobFormLookUps
    {
        public static List<(int Id, string NameEn, string NameAr)> Countries { get; } = new List<(int, string, string)>
        {
            (1, "Jordan", "الاردن"),
            (2, "Egypt", "مصر"),
            (3, "Iraq", "العراق"),
            (4, "Syria", "سوريا"),
            (5, "Lebanon", "لبنان"),
            (6, "USA", "امريكا"),
            (7, "UK", "بريطانيا"),
            (8, "Germany", "المانيا"),
            (9, "India", "الهند"),
            (10, "Pakistan", "الباكستان"),
            (11, "Turkey", "تركيا"),
            (12, "Russia", "روسيا"),
            (13, "Yugoslavia", "يوغسلافيا"),
            (14, "Spain", "اسبانيا"),
            (15, "Italy", "ايطاليا"),
            (16, "France", "فرنسا"),
            (17, "Sudan", "السودان"),
            (18, "Algeria", "الجزائر"),
            (19, "Libya", "ليبيا"),
            (20, "Tunisia", "تونس"),
            (21, "Saudi Arabia", "السعودية"),
            (22, "Kuwait", "الكويت"),
            (23, "UAE", "الامارات"),
            (24, "Oman", "عمان"),
            (25, "Qatar", "قطر"),
            (26, "Palestine", "فلسطين"),
            (27, "Ukraine", "اوكرانيا"),
            (28, "Romania", "رومانيا"),
            (29, "Brazil", "البرازيل"),
            (30, "Argentina", "الارجنتين"),
            (31, "Netherlands", "هولندا"),
            (32, "Belgium", "بلجيكا"),
            (33, "Canada", "كندا"),
            (34, "Bulgaria", "بلغاريا"),
            (35, "Greece", "اليونان"),
            (36, "Poland", "بولندا"),
            (37, "Australia", "استراليا"),
            (38, "Ireland", "ايرلندا"),
            (39, "China", "الصين"),
            (40, "Venezuela", "فنزويلا"),
            (41, "Cuba", "كوبا"),
            (42, "Malaysia", "ماليزيا"),
            (43, "Norway", "النرويج"),
            (44, "Denmark", "الدنمارك"),
            (45, "Finland", "فلندا"),
            (46, "Japan", "اليابان")
        };

        public static IEnumerable<SelectListItem> GetCountrySelectList(int? languageId)
        {
            return Countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = languageId == 1 ? c.NameEn : c.NameAr
            }).ToList();
        }

        // States by CountryId
        public static List<(int Id, int CountryId, string NameEn, string NameAr)> States { get; } = new List<(int, int, string, string)>
        {
            (1, 1, "Amman", "محافظة العاصمة"),
            (2, 1, "Zarqa", "محافظة الزرقاء"),
            (3, 1, "Irbid", "محافظة اربد"),
            (4, 1, "Jarash", "محافظة جرش"),
            (5, 1, "Balqa", "محافظة البلقاء"),
            (6, 1, "Karak", "محافظة الكرك"),
            (8, 1, "M'aan", "محافظة معان"),
            (9, 1, "Mafraq", "محافظة المفرق"),
            (10, 1, "Tafila", "محافظة الطفيلة"),
            (12, 1, "Aqaba", "محافظة العقبة"),
            (14, 1, "Ajloon", "محافظة عجلون"),
            (15, 1, "Madaba", "محافظة مادبا")
        };

        public static IEnumerable<SelectListItem> GetStatesSelectList(int countryId, int? languageId)
        {
            return States
                .Where(c => c.CountryId == countryId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = languageId == 1 ? c.NameEn : c.NameAr
                })
                .ToList();
        }

        // Cities list with unique IDs indexed from 1
        //public static List<(int Id, int StateId, string NameEn, string NameAr)> Cities { get; } = new List<(int, int, string, string)>
        //{
        //    (1, 8, "Ma'an", "معان"),
        //    (2, 10, "Tafielh", "الطفيلة"),
        //    (3, 12, "Aqaba", "العقبة"),
        //    (4, 3, "North Gor", "الاغوار الشمالية"),
        //    (5, 9, "Mafraq", "المفرق"),
        //    (6, 15, "Madaba", "مادبا"),
        //    (7, 5, "Karak", "الكرك"),
        //    (8, 1, "Amman", "عمان"),
        //    (9, 4, "Jarash", "جرش"),
        //    (10, 3, "Irbid", "اربد"),
        //    (11, 2, "Zarqa", "الزرقاء"),
        //    (12, 14, "Ajloon", "عجلون"),

        //    (13, 1, "Sweleh", "صويلح"),
        //    (14, 3, "Bny Knanah", "بني كنانة"),
        //    (15, 5, "Der Alaa", "دير علا"),
        //    (16, 6, "Gor Al Safi", "غور الصافي"),

        //    (17, 1, "South Amman", "جنوب عمان"),
        //    (18, 5, "South Shonh", "الشونة الجنوبية"),
        //    (19, 6, "Qaser", "القصر"),
        //    (20, 3, "Alkorah", "لواء الكورة"),

        //    (21, 1, "West Amman", "غرب عمان"),
        //    (22, 3, "Ramth", "الرمثا"),
        //    (23, 6, "Mazaar Aljanoby", "المزار الجنوبي"),

        //    (24, 3, "North Gor", "الاغوار الشمالية")
        //};
        public static List<(int Id, int StateId, string NameEn, string NameAr)> Cities { get; } = new List<(int, int, string, string)>
{
    (1, 8, "Ma'an", "معان"),
    (1, 10, "Tafielh", "الطفيلة"),
    (1, 12, "Aqaba", "العقبة"),
    (5, 3, "North Gor", "الاغوار الشمالية"),
    (2, 3, "Bny Knanah", "بني كنانة"),
    (3, 3, "Alkorah", "لواء الكورة"),
    (1, 9, "Mafraq", "المفرق"),
    (2, 6, "Gor Al Safi", "غور الصافي"),
    (4, 6, "Mazaar Aljanoby", "المزار الجنوبي"),
    (3, 6, "Qaser", "القصر"),
    (1, 15, "Madaba", "مادبا"),
    (4, 3, "Ramth", "الرمثا"),
    (1, 5, "Salt", "السلط"),
    (2, 5, "Der Alaa", "دير علا"),
    (3, 5, "South Shonh", "الشونة الجنوبية"),
    (1, 6, "Karak", "الكرك"),
    (1, 1, "Amman", "عمان"),
    (1, 4, "Jarash", "جرش"),
    (2, 1, "Sweleh", "صويلح"),
    (1, 3, "Irbid", "اربد"),
    (1, 2, "Zarqa", "الزرقاء"),
    (3, 1, "South Amman", "جنوب عمان"),
    (4, 1, "West Amman", "غرب عمان"),
    (1, 14, "Ajloon", "عجلون")
};

        public static IEnumerable<SelectListItem> GetCitiesByCountryAndState(int stateId, int? languageId)
        {
            return Cities
                .Where(c => c.StateId == stateId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = languageId == 1 ? c.NameEn : c.NameAr
                })
                .ToList();
        }

        // Marital Statuses
        public static List<(int Id, string NameEn, string NameAr)> MaritalStatuses { get; } = new List<(int, string, string)>
        {
            (1, "Single", "أعزب"),
            (2, "Married", "متزوج"),
            (6, "Provider", "معيل"),
            (3, "Divorced", "مطلق"),
            (4, "Widowed", "ارمل"),
            (5, "Single Female", "عزباء")
        };

        public static IEnumerable<SelectListItem> GetMaritalStatusSelectList(int? languageId)
        {
            return MaritalStatuses
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = languageId == 1 ? m.NameEn : m.NameAr
                })
                .ToList();
        }

        public static List<(int Id, string NameEn, string NameAr)> BloodTypes { get; } = new List<(int, string, string)>
        {
            (1, "A-", "A-"),
            (2, "A+", "A+"),
            (3, "B-", "B-"),
            (4, "B+", "B+"),
            (5, "O-", "O-"),
            (6, "O+", "O+"),
            (7, "AB-", "AB-"),
            (8, "AB+", "AB+")
        };


        public static IEnumerable<SelectListItem> GetBloodTypesSelectList(int? languageId)
        {
            return BloodTypes
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = languageId == 1 ? b.NameEn : b.NameAr
                })
                .ToList();
        }

        public static List<(int Id, string NameEn, string NameAr)> EducationDegrees { get; } = new List<(int, string, string)>
    {
        (1, "PhD", "دكتوراه"),
        (2, "Master's", "ماجستير"),
        (3, "Higher Diploma", "دبلوم عالي"),
        (4, "Bachelor's", "بكالوريوس"),
        (5, "College Diploma", "دبلوم-كلية متوسطة"),
        (6, "Two-Year Diploma", "دبلوم-سنتين"),
        (7, "Hospitality", "فندقة"),
        (8, "High School", "ثانوية عامة"),
        (9, "Industrial High School", "ثانوية عامة صناعية"),
        (10, "Vocational Training Center", "مركز تدريب مهني"),
        (30, "Comprehensive Diploma", "دبلوم شامل"),
        (11, "Below High School", "دون الثانوية")
    };

        public static IEnumerable<SelectListItem> GetEducationDegreeSelectList(int? languageId)
        {
            return EducationDegrees
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = languageId == 1 ? e.NameEn : e.NameAr
                })
                .ToList();
        }

        // Universities list
        public static List<(int CountryId, int Code, string NameAr, string NameEn)> Universities { get; } = new List<(int, int, string, string)>
        {
            (1, 1, "جامعة الطفيله", "Tafila University"),
            (1, 3, "الامير فيصل الفنيه", "Prince Faisal Technical College"),
            (1, 4, "الجامعه الاردنيه", "University of Jordan"),
            (1, 5, "الجامعيه المتوسطه", "Intermediate University College"),
            (1, 6, "الكفرين", "Al-Kofrein College"),
            (1, 7, "الكليه الجامعيه للهندسه التطبيقيه", "University College of Applied Engineering"),
            (1, 8, "الكليه العربيه", "Arab College"),
            (1, 9, "الكليه الفنيه العسكريه", "Military Technical College"),
            (1, 10, "المجتمع العربي", "Arab Community College"),
            (1, 11, "المركز الثقافي البريطاني", "British Council"),
            (1, 12, "المعهد الفني الهندسي (البوليتكنيك)", "Engineering Technical Institute (Polytechnic)"),
            (1, 13, "المهاجرين", "Al-Mohajireen College"),
            (1, 14, "جامعة اربد الاهليه", "Irbid National University"),
            (1, 15, "جامعة البلقاء التطبيقيه", "Al-Balqa Applied University"),
            (1, 16, "جامعة العلوم التطبيقية", "Applied Science University"),
            (1, 17, "جامعة العلوم والتكنولوجيا", "Jordan University of Science and Technology"),
            (1, 18, "جامعة النيلين", "Al-Neelain University"),
            (1, 19, "جامعة اليرموك", "Yarmouk University"),
            (1, 20, "جامعة جرش", "Jerash University"),
            (1, 21, "جامعة عمان الاهليه", "Amman National University"),
            (1, 22, "جامعة موته", "Mutah University"),
            (1, 23, "جامعة الاسراء", "Isra University"),
            (1, 24, "جامعة فيلادلفيا", "Philadelphia University"),
            (1, 25, "عمان", "Amman"),
            (1, 26, "غرفة تجارة عمان", "Amman Chamber of Commerce"),
            (1, 27, "كلية الأميرة سمية", "Princess Sumaya College"),
            (1, 28, "كلية الأندلس", "Al-Andalus College"),
            (1, 29, "كلية الحصن للمهن الهندسية", "Al-Huson Engineering Professions College"),
            (1, 30, "كلية الخوارزمي", "Al-Khawarizmi College"),
            (1, 31, "كلية الزرقاء", "Zarqa College"),
            (1, 32, "كلية القدس", "Al-Quds College"),
            (1, 33, "كلية المجتمع العربي", "Arab Community College"),
            (1, 34, "كلية حواره", "Howara College"),
            (1, 35, "كلية عمان للمهن الهندسية", "Amman Engineering Professions College"),
            (1, 36, "كلية مجتمع", "Community College"),
            (1, 37, "كلية مجتمع معان", "Ma'an Community College"),
            (1, 38, "للب", "LIOB"),
            (1, 39, "مركز ابو غزاله", "Abu Ghazaleh Center"),
            (1, 40, "مركز تدريب المهندسين", "Engineers Training Center"),
            (1, 41, "معهد البوليتكنك", "Polytechnic Institute"),
            (1, 42, "مكتب المسعود لتدقيق الحسابات والاستشارات", "Al-Masoud Office for Auditing and Consultations"),
            (1, 43, "مكتب المسعود/ للتدقيق والاستشارات الماليه والمصرفي", "Al-Masoud Office for Auditing, Financial and Banking Consultations"),
            (1, 44, "مكتب المسعود/لتدقيق الحسابات الماليه", "Al-Masoud Office for Financial Auditing"),
            (1, 45, "مؤسسة التدريب المهني", "Vocational Training Corporation"),
            (1, 46, "وادي السير", "Wadi Al-Seer College"),
            (1, 47, "وكالة الأمم المتحدة لإغاثة اللاجئين", "United Nations Relief Works Agency"),
            (1, 48, "ى", "N/A"),
            (1, 49, "جامعة الحسين بن طلال", "Al-Hussein Bin Talal University"),
            (1, 50, "كلية البتراء", "Petra College"),
            (1, 51, "كلية القادسية", "Al-Qadisiyah College"),
            (1, 52, "كلية غرناطة", "Granada College"),
            (1, 53, "كلية الطفيلة", "Tafila College"),
            (1, 54, "كلية التدريب التكنولوجي", "Technological Training College"),
            (1, 55, "جامعة الزرقاء", "Zarqa University"),
            (1, 56, "مركز الاستشارات الفنية", "Technical Consultation Center"),
            (1, 57, "صندوق الملكة عليا للعمل الاجتماعي", "Queen Alia Fund for Social Development"),
            (1, 58, "معهد الإدارة الأردني", "Jordan Management Institute"),
            (1, 59, "الجامعة العربية المفتوحة", "Arab Open University"),
            (1, 60, "جامعة آل البيت", "Al al-Bayt University"),
            (1, 61, "الجامعة الهاشمية", "Hashemite University"),
            (1, 62, "جامعة الزيتونة", "Al-Zaytoonah University"),
            (1, 63, "جامعة عدن اليمن", "Aden University - Yemen"),
            (1, 64, "جامعة العقبة للتكنولوجيا", "Aqaba University of Technology"),
            (1, 65, "دون الثانوية", "Below Secondary"),
            (1, 66, "الثانوية العامة", "General Secondary"),
            (1, 67, "التجارة الإلكترونية", "E-Commerce"),
            (1, 68, "جامعة البتراء", "Petra University"),
            (1, 69, "جامعة العلوم الإسلامية العالمية", "World Islamic Sciences University"),
            (1, 70, "كلية اربد الجامعية", "Irbid University College"),
            (1, 71, "كلية الكرك الجامعية", "Karak University College"),
            (1, 72, "كلية العقبة الجامعية", "Aqaba University College"),
            (1, 73, "جامعة عمان العربية", "Amman Arab University"),
            (1, 74, "أكاديمية الأمير الحسين بن عبدالله الثاني للحماية المدنية / جامعة البلقاء", "Prince Hussein Academy for Civil Protection / Al-Balqa University"),
            (1, 75, "جامعة عجلون الوطنية", "Ajloun National University"),

            // Other countries
            (2, 1, "المعهد الفني / مصر", "Technical Institute / Egypt"),
            (3, 1, "جامعة الموصل", "University of Mosul"),
            (3, 2, "جامعة بغداد", "University of Baghdad"),
            (4, 1, "المعهد المتوسط / سوريا", "Intermediate Institute / Syria"),
            (4, 2, "جامعة حلب", "University of Aleppo"),
            (4, 3, "جامعة دمشق", "University of Damascus"),
            (5, 1, "جامعة بيروت", "University of Beirut"),
            (8, 1, "جامعة برلين الغربية", "University of West Berlin"),
            (9, 1, "جامعة بنقالور", "University of Bangalore"),
            (10, 1, "جامعة بيشاور", "University of Peshawar"),
            (11, 1, "أكاديمية اسطنبول", "Istanbul Academy"),
            (11, 2, "جامعة تركيا", "University of Turkey"),
            (12, 1, "موسكو", "Moscow"),
            (13, 1, "جامعة كوتسية", "University of Kotseya"),
            (17, 1, "جامعة البحر الأحمر", "University of the Red Sea"),
            (17, 2, "جامعة النيلين", "University of Nileen"),
            (18, 1, "المعهد الوطني للصناعات الخفيفه", "National Institute for Light Industries"),
            (27, 1, "جامعة كيوفوغراد", "Kyivograd University"),
            (27, 2, "جامعة أوكرانيا", "University of Ukraine"),
            (28, 1, "جامعة رومانيا", "University of Romania"),
            (35, 1, "معهد التطبيقات التكنولوجية - مؤسسة التعليم الفني سالوتيك", "Institute of Technological Applications - Salotech")
        };

        public static IEnumerable<SelectListItem> GetUniversitiesByCountry(int countryId, int? languageId)
        {
            return Universities
                .Where(u => u.CountryId == countryId)
                .Select(u => new SelectListItem
                {
                    Value = u.Code.ToString(),
                    Text = languageId == 1 ? u.NameEn : u.NameAr
                })
                .ToList();
        }

        public static List<(int Id, string NameEn, string NameAr)> YearsOfStudy { get; } =
               Enumerable.Range(1, 10)
               .Select(i => (i, $"{i} Year{(i > 1 ? "s" : "")}", $"{i} سنة"))
               .ToList();

        public static IEnumerable<SelectListItem> GetYearsOfStudySelectList(int? languageId)
        {
            return YearsOfStudy
                .Select(y => new SelectListItem
                {
                    Value = y.Id.ToString(),
                    Text = languageId == 1 ? y.NameEn : y.NameAr
                })
                .ToList();
        }


        public static List<(int Id, string NameEn, string NameAr)> Specializations { get; } = new List<(int, string, string)>
    {
        (1, "Engineering", "هندسة"),
        (2, "Commerce", "تجارة"),
        (3, "Laboratories", "مختبرات"),
        (4, "Pharmacy", "صيدلة"),
        (5, "Computer", "حاسوب"),
        (6, "Arts", "آداب"),
        (7, "Agriculture", "زراعة"),
        (8, "Industry", "صناعة"),
        (9, "Vocational Training Centers", "مراكز تدريب مهني"),
        (10, "Scientific", "علمي"),
        (11, "Literary", "ادبي"),
        (12, "Commercial", "تجاري"),
        (13, "Industrial", "صناعي"),
        (14, "Agricultural", "زراعي"),
        (15, "Hospitality", "فندقة"),
        (16, "None", "بلا"),
        (17, "Accounting", "محاسبة"),
        (18, "Business Administration", "ادارة اعمال"),
        (19, "Public Administration", "ادارة عامة"),
        (20, "Fashion Design", "تصميم أزياء"),
        (21, "Programming", "برمجة"),
        (22, "Political Science", "علوم سياسية"),
        (23, "Food Industries", "صناعات غذائية"),
        (28, "No Specialization", "بدون تخصص"),
        (30, "Law", "قانون"),
        (31, "Management", "ادارة"),
        (32, "Law", "قانون"),
        (33, "Mechanics", "ميكانيك"),
        (40, "Sociology", "اجتماع"),
        (41, "Electricity", "كهرباء"),
        (50, "Air Conditioning and Refrigeration", "تكييف وتبريد"),
        (51, "Child Education", "تربية طفل"),
        (52, "Automotive", "سيارات"),
        (53, "Arabic Language", "لغة عربية"),
        (54, "Social Studies", "اجتماعيات"),
        (55, "Blacksmithing and Welding", "حدادة ولحام"),
        (56, "Science and Biology", "علوم وأحياء"),
        (57, "Economics", "اقتصاد"),
        (58, "Computer Programming", "برمجة كمبيوتر"),
        (60, "Financial and Banking Sciences", "علوم مالية ومصرفية"),
        (80, "English Language", "لغة إنجليزية"),
        (91, "Internal Auditing", "التدقيق الداخلي"),
        (92, "No Specialization", "بدون تخصص"),
        (93, "Information Technology", "تكنولوجيا معلومات"),
        (94, "Civil Engineering", "هندسة مدنية"),
        (95, "Civil Engineering", "هنسة مدنيه"),
        (96, "Mechanical Engineering", "هندسة ميكانيكية"),
        (97, "Chemical Engineering", "هندسة كيميائية"),
        (98, "Electrical Engineering", "هندسة كهربائية"),
        (99, "Agricultural Engineering", "هندسة زراعية"),
        (100, "Electronic Engineering", "هندسة إلكترونية"),
        (101, "Production Technology", "تكنولوجيا الإنتاج"),
        (102, "Special Education", "تربية خاصة"),
        (103, "Interior Design", "تصميم داخلي"),
        (104, "Management Information Systems", "نظم معلومات إدارية"),
        (105, "Industrial Electronics", "الإلكترونيات الصناعية"),
        (106, "Supply Management", "إدارة التزويد"),
        (107, "Warehouse Management", "إدارة المستودعات"),
        (108, "Journalism and Media", "صحافة وإعلام"),
        (109, "Primary Education", "تربية ابتدائية"),
        (110, "Accounting Information Systems", "نظم محاسبية"),
        (111, "Mechatronics Engineering", "هندسة ميكاترونيكس"),
        (112, "Food Technology", "تكنولوجيا الأغذية"),
        (113, "Police and Administrative Sciences", "العلوم الشرطية والإدارية"),
        (114, "-----", "----"),
        (115, "Mechanical Engineering / Air Conditioning and Refrigeration", "هندسة ميكانيك/تكييف وتبريد"),
        (116, "Electrical Engineering / Communications", "هندسة كهرباء/اتصالات"),
        (117, "Mechanical Engineering / Heavy Machinery", "هندسة ميكانيك/آلات ثقيلة"),
        (118, "Mechanical Engineering / Thermal and Hydraulic Machinery", "هندسة ميكانيك/آلات حرارية وهيدروليكية"),
        (119, "Mechanical Engineering / General", "هندسة ميكانيك/عامة"),
        (120, "Mechanical Engineering / Air Conditioning and Refrigeration", "هندسة ميكانيك/تكييف وتبريد"),
        (121, "Below Secondary", "دون الثانوية"),
        (122, "General Secondary", "الثانوية العامة"),
        (123, "Business Administration / Accounting Track", "ادارة اعمال/ مسار محاسبة"),
        (124, "Accounting and Commercial Law", "المحاسبة والقانون التجاري"),
        (125, "Computer Information Systems", "أنظمة المعلومات الحاسوبية"),
        (126, "Plant Production", "الانتاج النباتي"),
        (127, "Electromechanical Maintenance", "الصيانة الكهروميكانيكية"),
        (128, "Mechanical Design", "التصميم الميكانيكي"),
        (129, "Computer Engineering", "هندسة حاسوب")
    };

        public static IEnumerable<SelectListItem> GetSpecializationsSelectList(int? languageId)
        {
            return Specializations
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = languageId == 1 ? s.NameEn : s.NameAr
                })
                .ToList();
        }
    }

}
