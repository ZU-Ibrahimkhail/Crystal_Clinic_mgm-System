using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Persistence.Initializers
{
    public class NotificationInitializer
    {
        /// <summary>
        /// /
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async static Task InitializeNotifications(UMS_DbContext context)
        {
            await SeedNotificationMessages(context);

        }
        #region Notification
        public static async Task SeedNotificationMessages(UMS_DbContext context)
        {
            var GUID = Guid.NewGuid();
            var Roles = context.NotificationMessages.ToList();
            if (Roles.Count == 14)
            {
                try
                {
                    ////ClientRequestApproved = 1
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "Client Request Approved!",
                    //    EnglishMessage = "Your request is approved from user ",
                    //    ApplicationName = "Reception",
                    //    EnglishDescription = "This message is created informing the client user that his or her request is approved.",
                    //    ControllerLink = "ReceptionTrackingController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////CreatedNewRequest = 2
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "New Client Request!",
                    //    EnglishMessage = "There is a new visit request from user ",
                    //    ApplicationName = "Reception",
                    //    EnglishDescription = "This message is created informing the client user that request a visit.",
                    //    ControllerLink = "ReceptionTrackingController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////ClientRequestRejected = 3
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "Client Request Rejected!",
                    //    EnglishMessage = "Your request is reject from user ",
                    //    ApplicationName = "Reception",
                    //    EnglishDescription = "This message is created informing the client user that request is reject from the user.",
                    //    ControllerLink = "ReceptionTrackingController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////NewInternalDocument = 4
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "New Internal Document",
                    //    EnglishMessage = "Your have a New Internal Document from user ",
                    //    ApplicationName = "DMTS",
                    //    EnglishDescription = "This message is created informing the client user that he/she has new internal document from the user.",
                    //    ControllerLink = "InternalDocumentController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////NewExternalDocument = 5
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "New External Document",
                    //    EnglishMessage = "Your have a New External Document from user ",
                    //    ApplicationName = "DMTS",
                    //    EnglishDescription = "This message is created informing the client user that he/she has new External document from the user.",
                    //    ControllerLink = "ExternalDocumentController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////InternalDocumentReceived = 6
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "Internal Document Received!",
                    //    EnglishMessage = "Your Internal Document is Received by user ",
                    //    ApplicationName = "DMTS",
                    //    EnglishDescription = "This message is created informing the client user that Internal Document is Received by user.",
                    //    ControllerLink = "InternalDocumentController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////ExternalDocumentReceived = 7
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "External Document Received!",
                    //    EnglishMessage = "Your External Document is Received by user ",
                    //    ApplicationName = "DMTS",
                    //    EnglishDescription = "This message is created informing the client user that External Document is Received by user.",
                    //    ControllerLink = "ExternalDocumentController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////ExternalDocumentRejected = 8
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "External Document Rejected!",
                    //    EnglishMessage = "Your External Document is Rejected by user ",
                    //    ApplicationName = "DMTS",
                    //    EnglishDescription = "This message is created informing the client user that External Document is Rejected by user.",
                    //    ControllerLink = "ExternalDocumentController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////InternalDocumentRejected = 9
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "Internal Document Rejected!",
                    //    EnglishMessage = "Your Internal Document is Rejected by user ",
                    //    ApplicationName = "DMTS",
                    //    EnglishDescription = "This message is created informing the client user that Internal Document is Rejected by user.",
                    //    ControllerLink = "InternalDocumentController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////NewITSMSApplicantRequestRecord = 10
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "New ITSMS Applicant Request Record!",
                    //    EnglishMessage = "You have a New ITSMS Applicant Request Record ",
                    //    ApplicationName = "TISMS",
                    //    EnglishDescription = "This message is created informing the client user from New ITSMS Applicant Request Record.",
                    //    ControllerLink = "ApplicantRequestController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////ITSMSRequestRejected = 11
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "ITSMS Applicant Request Rejected!",
                    //    EnglishMessage = "Your ITSMS Applicant Request is Rejected ",
                    //    ApplicationName = "TISMS",
                    //    EnglishDescription = "This message is created informing the client user that his / her  ITSMS Applicant Request Rejected by user.",
                    //    ControllerLink = "ApplicantRequestController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////ITSMSRequestProcessComplete = 12
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "ITSMS Applicant Request Process Completed!",
                    //    EnglishMessage = "Your ITSMS Applicant Request Process is Completed ",
                    //    ApplicationName = "TISMS",
                    //    EnglishDescription = "This message is created informing the client user that his / her  ITSMS Applicant Request Process is Completed.",
                    //    ControllerLink = "ApplicantRequestController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});
                    //await context.SaveChangesAsync();

                    ////ITSMSRequestRecived = 13
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "ITSMS Applicant Request Recived!",
                    //    EnglishMessage = "Your ITSMS Applicant Request is Recived by user ",
                    //    ApplicationName = "TISMS",
                    //    EnglishDescription = "This message is created informing the client user that his / her  ITSMS Applicant Request Recived by user.",
                    //    ControllerLink = "ApplicantRequestController",
                    //    ActionLink = "GetList",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});

                    ////Your Created Document Has Been Sent to You = 14
                    //context.NotificationMessages.Add(new NotificationMessage
                    //{
                    //    EnglishTitle = "Your Created Document Has Been Sent to You!",
                    //    PashtoTitle = "ستاسو لخوا جوړ شوی سند تاسو ته رالیږل شوی!",
                    //    DariTitle = "سند ایجاد شده شما برای شما ارسال شده است!",

                    //    EnglishMessage = "Your Created Internal Document has been sent to you",
                    //    DariMessage = "سند داخلی ایجاد شده شما برای شما ارسال شده است!",
                    //    PashtoMessage = "ستاسو لخوا جوړ شوی داخلي سند تاسو ته رالیږل شوی!",

                    //    ApplicationName = "DMTS",
                    //    EnglishDescription = "This message is created informing the client user that Internal Document has been sent back to creator.",
                    //    PashtoDescription = "دا پیغام د پیرودونکي کارونکي ته خبر ورکولو لپاره رامینځته شوی چې داخلي سند بیرته جوړونکي ته لیږل شوی.",
                    //    DariDescription = "این پیام برای اطلاع کاربر مشتری ایجاد می شود که سند داخلی به سازنده بازگردانده شده است.",

                    //    ControllerLink = "InternalDocumentController",
                    //    ActionLink = "GetList(True)",
                    //    IsDeleted = false,
                    //    CreatedBy = GUID,
                    //    ModifiedBy = GUID,
                    //    CreatedOn = DateTime.Now,
                    //    ModifiedOn = DateTime.Now

                    //});


                    //New PMIS Activity Received = 15
                    context.NotificationMessages.Add(new NotificationMessage
                    {
                        EnglishTitle = "New PMIS Activity Received",
                        PashtoTitle = "فعالیت جدید دریافت شد!",
                        DariTitle = "نوی فعالیت ترلاسه شو!",

                        EnglishMessage = "You Have recieved a new activity access in a project of PMIS",
                        DariMessage = "شما دسترسی فعالیت جدیدی در سیستم پروژه ها دریافت کرده اید!",
                        PashtoMessage = "تاسو د پروژې سیسټم کې نوي فعالیت ته لاسرسی ترلاسه کړی!",

                        ApplicationName = "PMIS",
                        EnglishDescription = "This message is created informing the client user that a new activity access in a project of PMIS is recieved.",
                        PashtoDescription = "دا پیغام د دې لپاره رامینځته شوی چې پیرودونکي کارونکي ته خبر ورکړي چې د پروژې سیسټم پروژې کې نوي فعالیت ته لاسرسی ترلاسه شوی.",
                        DariDescription = "این پیام برای اطلاع کاربر مشتری ایجاد می شود که دسترسی فعالیت جدیدی در پروژه سیستم پروژه ها دریافت شده است.",

                        ControllerLink = "ActivityAssignmentController",
                        ActionLink = "GetDropDownList/{ProjectId:int}",
                        IsDeleted = false,
                        CreatedBy = GUID,
                        ModifiedBy = GUID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now

                    });
                    await context.SaveChangesAsync();
                    //PMIS Activity Completed = 16
                    context.NotificationMessages.Add(new NotificationMessage
                    {
                        EnglishTitle = "PMIS Activity Completed!",
                        PashtoTitle = "فعالیت در سیستم پروژه ها تکمیل شد!",
                        DariTitle = "د پروژې په سیستم کې فعالیت بشپړ شوی دی!",

                        EnglishMessage = "Your activity in a project of PMIS is completed and ready for review",
                        DariMessage = "فعالیت شما در پروژه در سیستم پروژه ها تکمیل شده و آماده بررسی است!",
                        PashtoMessage = "ستاسو فعالیت د پروژې په سیسټم کې په یو پروژه کې بشپړ شوی او بیاکتنې ته چمتو دی!",

                        ApplicationName = "PMIS",
                        EnglishDescription = "This message is created informing the client user that Your activity in a project of PMIS is completed and ready for review.",
                        PashtoDescription = "دا پیغام د دې لپاره رامینځته شوی چې پیرودونکي کارونکي ته خبر ورکړي چې ستاسو فعالیت د پروژې په سیسټم کې په یو پروژه کې بشپړ شوی او بیاکتنې ته چمتو دی.",
                        DariDescription = "این پیام برای اطلاع کاربر مشتری ایجاد می شود که فعالیت شما در پروژه در سیستم پروژه ها تکمیل شده و آماده بررسی است.",

                        ControllerLink = "ActivityAssignmentController",
                        ActionLink = "GetDropDownList/{ProjectId:int}",
                        IsDeleted = false,
                        CreatedBy = GUID,
                        ModifiedBy = GUID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now

                    });
                    await context.SaveChangesAsync();

                    //PMIS Activity Has Recomendation = 17
                    context.NotificationMessages.Add(new NotificationMessage
                    {
                        EnglishTitle = "PMIS Activity Has Recomendation!",
                        PashtoTitle = "فعالیت اجرا شده دارای توصیه است!",
                        DariTitle = "پلي شوي فعالیت وړاندیز لري!",

                        EnglishMessage = "Your executed activity in a project of PMIS has a recomendation",
                        DariMessage = "فعالیت انجام شده شما در پروژه سیستم پروژه ها دارای یک توصیه است!",
                        PashtoMessage = "د پروژې سیسټم په پروژه کې ستاسو فعالیت سپارښتنه لري!",

                        ApplicationName = "PMIS",
                        EnglishDescription = "This message is created informing the client user that Your executed activity in a project of PMIS has a recomendation.",
                        PashtoDescription = "دا پیغام د دې لپاره رامینځته شوی چې پیرودونکي کارونکي ته خبر ورکړي چې د پروژې سیسټم په یو پروژه کې ستاسو فعالیت سپارښتنه لري.",
                        DariDescription = "این پیام برای اطلاع کاربر مشتری ایجاد می شود که فعالیت انجام شده شما در پروژه سیستم پروژه ها دارای یک توصیه است.",

                        ControllerLink = "ActivityAssignmentController",
                        ActionLink = "GetDropDownList/{ProjectId:int}",
                        IsDeleted = false,
                        CreatedBy = GUID,
                        ModifiedBy = GUID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now

                    });
                    await context.SaveChangesAsync();




                }
                catch (Exception)
                {
                    throw;
                }

            }
        }

        #endregion

    }
}
