namespace Crystal_Clinic_Mgm.Common.Constants
{
    public class Constants
    {
        /// <summary>
        /// To Check users Sign in pass in autorization
        /// </summary>
        public static bool CheckPassword = true;

        public static class ResponsCodes
        {
            public const int SAVED = 1;
            public const int UPDATED = 2;
            public const int DELETED = 3;
            public const int TOKENEX_PIRED = 4;
            public const int UNAUTHORIZED_ACCESS = 5;
            public const int INVALID_PARAMETER = 6;
            public const int PARAMETER_MISSING = 7;
            public const int Validation_Error = 8;
            public const int ReportDownloaded = 9;
        }
        public static class ProcessStatus
        {
            public const int ProcessStart = 10;
            public const int NotCompleted = 4;
            public const int Sent = 11;
            public const int Pending = 3;
            public const int InProcess = 2;
            public const int Completed = 1;
            public const int DocumentProcessEnd = 7;
            public const int ExecuteAndMessageSending = 9;
            public const int Draft = 14;
            // for itsms
            public const int Approve = 5;
            public const int Reject = 6;
            public const int ReplyToReceivedDocument = 8;
            //----Reception 
            public const int Schedule = 13;
        }
        public static class ErrorCodes
        {
            public const int NotFound = 404;
            public const int BadRequest = 400;
            public const int UnAuthorized = 401;
            public const int Forbidden = 403;
            public const int InternalServer = 500;
            public const int IdLessThenError = 409;
            public const int WebServerDown = 521;
            public const int SSLFailed = 525;
        }
        // ERP NAME
        public static class ERPSystemName
        {
            public const string Acronym = "Crystal_Clinic Management";
            public const string FullName = "";
        }
        public static class Language
        {
            public const string English = "en";
            public const string Pashto = "ps-AF";
            public const string Dari = "fa-IR";

        }
        public static class CultureCookies
        {
            public const string CookiesName = ".AspNetCore.Culture";
        }
        public static class LangCultureCookies
        {
            static string lang = "";

            public static string LangAbName
            {
                get { return lang; }
                set { lang = value; }
            }
            // public int MyProperty { get; set; }
        }
        public static class NotificationMessage
        {
            public const int VisitorInOut = 0;
            // ReceptionTrackingRecordApproved
            public const int ClientRequestApproved = 1;
            // NewReceptionTrackingRecord
            public const int CreatedNewRequest = 2;
            // ReceptionTrackingRecordRejected
            public const int ClientRequestRejected = 3;
            // NewInternalDocumentTrackingRecord
            public const int NewInternalDocument = 4;
            // NewExternalDocumentTrackingRecord
            public const int NewExternalDocument = 5;
            // InternalDocumentRecieved
            public const int InternalDocumentReceived = 6;
            // ExternalDocumentRecieved
            public const int ExternalDocumentReceived = 7;
            // ExternalDocumentRejected
            public const int ExternalDocumentRejected = 8;
            // InternalDocumentRejected
            public const int InternalDocumentRejected = 9;
            // NewITSMSApplicantRequestRecord
            public const int ITSMSNewRequest = 10;
            // ITSMSRequestRejected
            public const int ITSMSRequestRejected = 11;
            // ITSMSRequestProcessComplete
            public const int ITSMSRequestProcessComplete = 12;
            // ITSMSRequestRecived
            public const int ITSMSRequestRecived = 13;

            // Your Created Document Has Been Sent to You
            public const int CreatedDocumentRecivedBack = 14;

            public const int NewPMISProjectActivityRecieved = 15;
            public const int PMISActivityCompleted = 16;
            public const int PMISActivityHasRecomendation = 17;
        }
        public static class ApplicationModule
        {
            public const int HR = 3;
            public const int UMS = 6;
            public const int Stock = 5;
        }

        public static class CurrencyTypes
        {
            public const int USD = 1;
            public const int AFN = 2;
        }
        public static class BranchLevels
        {
            public const int Ministry = 1;
            public const int Deputy = 2;
            public const int GeneralDirectorate = 3;
            public const int Directorate = 4;
            public const int SubDirectorate = 5;
            public const int GeneralManagement = 6;
            public const int Management = 7;
        }
        public static class JobPosition
        {
            public const int Minister = 2;
            public const int deputy = 3;
            public const int GenralDirector = 4;
            public const int Director = 5;
        }
        public static class EmailSetting
        {
            public const int allowMinuts = 20;
        }
    }
}
