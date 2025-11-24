namespace ES.Web.Models
{
    public class SilosJobRequestDto
    {
        public string DECLERATION_NO { get; set; } = string.Empty;
        public int JOB_CODE { get; set; }
        public string NATIONAL_NO { get; set; } = string.Empty;
        public string NAM_A1 { get; set; } = string.Empty;
        public string NAM_A2 { get; set; } = string.Empty;
        public string NAM_A3 { get; set; } = string.Empty;
        public string NAM_A4 { get; set; } = string.Empty;
        public string NAME_E1 { get; set; } = string.Empty;
        public string NAME_E2 { get; set; } = string.Empty;
        public string NAME_E3 { get; set; } = string.Empty;
        public string NAME_E4 { get; set; } = string.Empty;
        public int CODE_NATIONALITY { get; set; }
        public string PLACE_OF_BIRTH { get; set; } = string.Empty;
        public int COUNTRY_CODE_BIRTH_PLACE { get; set; }
        public int STATE_CODE_BIRTH_PLACE { get; set; }
        public int CITY_CODE_BIRTH_PLACE { get; set; }
        public int RELIGION_ID { get; set; }
        public int GENDER { get; set; }
        public string DATE_OF_BIRTH { get; set; } = string.Empty;
        public string PASSPORT_NO { get; set; } = string.Empty;
        public string PASSPORT_ISSUE_DATE { get; set; } = string.Empty;
        public string PASSPORT_EXPIRE_DATE { get; set; } = string.Empty;
        public string DATE_AVAILABLE_TO_WORK { get; set; } = string.Empty;
        public int MARITAL_STATUS { get; set; }
        public int BLOOD_TYPE { get; set; }
        public string COUNTRY_CODE_ADDRESS { get; set; } = string.Empty;
        public string STATE_CODE_ADDRESS { get; set; } = string.Empty;
        public string CITY_CODE_ADDRESS { get; set; } = string.Empty;
        public string ADDRESS { get; set; } = string.Empty;
        public string EMAIL { get; set; } = string.Empty;
        public long TELEPHONE_NO { get; set; }
        public string PO_BOX { get; set; } = string.Empty;
        public long MOBILE { get; set; }
        public int NO_OF_CHILDREN { get; set; }
        public decimal EXPECTED_SALARY { get; set; }
        public long SOCIAL_SECURITY_NO { get; set; }
        public List<EducationInfoDto> EDUCATION_INFO { get; set; } = new List<EducationInfoDto>();
        public List<LanguageInfoDto> EMPLOYEE_LANGUAGES { get; set; } = new List<LanguageInfoDto>();
        public List<ExperienceInfoDto> EXPERIENCE_INFO { get; set; } = new List<ExperienceInfoDto>();
        public List<SkillInfoDto> SKILLS_INFO { get; set; } = new List<SkillInfoDto>();
    }

    public class EducationInfoDto
    {
        public string UNIV_COUNTRY_CODE { get; set; } = string.Empty;
        public string UNV_CODE { get; set; } = string.Empty;
        public string SPECIAL_CODE { get; set; } = string.Empty;
        public string YEARS_OF_STUDY { get; set; } = string.Empty;
        public string EDU_DEGREE { get; set; } = string.Empty;
        public string START_DATE { get; set; } = string.Empty;
        public string END_DATE { get; set; } = string.Empty;
        public string GPA { get; set; } = string.Empty;
    }

    public class LanguageInfoDto
    {
        public string LANGUAGE_ID { get; set; } = string.Empty;
        public string READING_LEVEL { get; set; } = string.Empty;
        public string WRITING_LEVEL { get; set; } = string.Empty;
        public string SPEAKING_LEVEL { get; set; } = string.Empty;
    }

    public class ExperienceInfoDto
    {
        public string COMPANY_NAME { get; set; } = string.Empty;
        public string COMPANY_ADDRESS { get; set; } = string.Empty;
        public string START_DATE { get; set; } = string.Empty;
        public string END_DATE { get; set; } = string.Empty;
        public decimal LAST_SALARY { get; set; }
        public string MAIN_JOB_DESCRIPTION { get; set; } = string.Empty;
        public string DIRECT_MANAGER_NAME { get; set; } = string.Empty;
        public string LEAVE_REASON { get; set; } = string.Empty;
    }

    public class SkillInfoDto
    {
        public string SKILL_DESCRIPTION { get; set; } = string.Empty;
        public string SKILL_LEVEL { get; set; } = string.Empty;
        public string SKILL_MAIN_TYPE { get; set; } = string.Empty;
        public string SKILL_SUB_TYPE { get; set; } = string.Empty;
    }
}
