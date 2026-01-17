using Microsoft.Extensions.Configuration;
namespace Crystal_Clinic_Mgm.Common.AppConfig
{
    public class AppConfig
    {
        private static IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile($"appsettings.json");
        private static IConfigurationRoot AppSettings
        {
            get
            {
                return builder.Build();
            }
        }
        public static string ERP_DbContext
        {
            get
            {
                return AppSettings["ERPDbConnection"]!;
            }
        }
        public static string UMS_DbContext
        {
            get
            {
                return AppSettings["UMSDbConnection"]!;
            }
        }
        public static string ITSMS_DbContext
        {
            get
            {
                return AppSettings["ITSMSDbConnection"]!;
            }
        }
        public static string SignalRIP
        {
            get
            {
                return AppSettings["Helper:SignalRIP"]!;
            }
        }

        public static string ProfilePhotoPath
        {
            get
            {
                return AppSettings["ProfilePhotoPath"]!;
            }
        }
        public static string AttachmentPhotoPath
        {
            get
            {
                return AppSettings["ArchiveAttachmentPath"]!;
            }
        }
        public static string WeatherToken
        {
            get
            {
                return AppSettings["WeatherToken"]!;
            }
        }
        public static string DMTS_InternalDocument
        {
            get
            {
                return AppSettings["AttachmentFilesPath:DMTS:InternalDocument"]!;
            }
        }
        public static string DMTS_InExternalDocument
        {
            get
            {
                return AppSettings["AttachmentFilesPath:DMTS:ExternalDocument:InDocuments"]!;
            }
        }
        public static string DMTS_OutExternalDocument
        {
            get
            {
                return AppSettings["AttachmentFilesPath:DMTS:ExternalDocument:OutDocuments"]!;
            }
        }
        public static string UMS_UserProfilePhoto
        {
            get
            {
                return AppSettings["AttachmentFilesPath:UMS:UserProfilePhoto"]!;
            }
        }
        public static string ITSMS_RequestForm
        {
            get
            {
                return AppSettings["AttachmentFilesPath:ITSMS:RequestForms"]!;
            }
        }
        public static string Archive_ArchivedDocuments
        {
            get
            {
                return AppSettings["AttachmentFilesPath:Archive:ArchivedDocuments"]!;
            }
        }
        public static string Archive_OutDocument
        {
            get
            {
                return AppSettings["AttachmentFilesPath:Archive:OutDocuments"]!;
            }
        }
        public static string Reception_RequestAttachment
        {
            get
            {
                return AppSettings["AttachmentFilesPath:Reception:RequestAttachment"]!;
            }
        }
        public static string PMIS_Attachments_Path
        {
            get
            {
                return AppSettings["AttachmentFilesPath:PMIS:PMISAttachments"]!;
            }
        }
        public static string HR_CardAttachments
        {
            get
            {
                return AppSettings["AttachmentFilesPath:HR:CardAttachments"]!;
            }
        }
        #region Email

        #endregion
        public static string MailAddress
        {
            get
            {
                return AppSettings["MailSettings:Mail"]!;
            }

        }
        public static string MailDisplayName
        {
            get
            {
                return AppSettings["MailSettings:DisplayName"]!;
            }

        }
        public static string MailPassword
        {
            get { return AppSettings["MailSettings:Password"]!; }
        }
        public static string NewsAttachments
        {
            get { return AppSettings["AttachmentFilesPath:General:NewsAttachments"]!; }
        }
        public static string TrainingAttachment
        {
            get { return AppSettings["AttachmentFilesPath:General:TrainingAttachment"]!; }
        }
        public static string AdministrativeFormsAttachments
        {
            get { return AppSettings["AttachmentFilesPath:GeneralForm:AdministrativeFormsAttachments"]!; }
        }
        public static string ClinicAttachment
        {
            get { return AppSettings["AttachmentFilesPath:ClinicAttachments"]!; }
        }
        public static string MailHost
        {
            get { return AppSettings["MailSettings:Host"]!; }
        }
        public static int MailPort
        {
            get
            {
                int p = 0;
                int.TryParse(AppSettings["MailSettings:Port"]!, out p);
                return p;
            }
        }
        public static string DownlaodReportFiles
        {
            get { return AppSettings["AttachmentFilesPath:General:DownloadReportFiles"]!; }
        }


        public static DateTimeOffset RedisExpirationTimeOffsetHours
        {
            get { return DateTimeOffset.Now.AddHours(Convert.ToInt32(AppSettings["Redis:ExpirationTimeOffsetHours"])); }
        }

        public static string RedisConnection
        {
            get { return AppSettings["Redis:ConnectionString"]!; }
        }
    }
}
