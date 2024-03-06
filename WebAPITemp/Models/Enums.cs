namespace WebAPITemp.Models
{
    public class Enums
    {
        /// <summary>
        /// 使用者語系
        /// </summary>
        public enum UsingLanguage
        {
            CNAndEN = 1,
            EN = 2
        }
        /// <summary>
        /// 使用者帳號狀態
        /// </summary>
        public enum UserStatus
        {
            In_service = 1,
            Resigned = 2,
            Remain  = 3
        }
        /// <summary>
        /// 使用者性別
        /// </summary>
        public enum Gender
        {
            Male = 1,
            Female = 2
        }
        /// <summary>
        /// 使用者個人資料審核狀態
        /// </summary>
        public enum ReviewStatus
        {
            Reviewing = 1,
            Approved = 2,
            Rejected = 3
        }

        /// <summary>
        /// Action權限
        /// </summary>
        public enum ActionRole
        {
            Management = 1,
            Admin = 2,
            LeadTeacher = 3,
            SeniorTeacher = 4,
            FullTimeTeacher = 5,
            FreelanceTeacher = 6,
            DemoTeacher = 7,
            FilipinoTeacher = 8,
            OutsourcedManagement = 9,
            OutsourcedTeacher = 10,
            AcademicCounselor = 11,
            None = 12,
            Developer = 13,
        }
    }
}