namespace Crystal_Clinic_Mgm.Application.Common.SignalR
{
    public interface ISignalTypes
    {
        #region Reception Signals
        Task NewServiceSessionRecord(string? UserName = null, string? PhotoPath = null);
        Task VisitorInOut(string? UserName = null, string? PhotoPath = null);
        #endregion
        #region DMTS Internal and external document signals
        Task NewInternalDocumentTrackingRecord(string? UserName = null, string? PhotoPath = null);
        Task InternalDocumentRejected(string? UserName = null, string? PhotoPath = null);
        Task InternalDocumentRecieved(string? UserName = null, string? PhotoPath = null);
        Task NewExternalDocumentTrackingRecord(string? UserName = null, string? PhotoPath = null);
        Task ExternalDocumentRejected(string? UserName = null, string? PhotoPath = null);
        Task ExternalDocumentRecieved(string? UserName = null, string? PhotoPath = null);
        Task CreatedDocumentRecivedBack(string? UserName = null, string? PhotoPath = null);
        #endregion
        #region ITSMS Signals
        Task NewITSMSApplicantRequestRecord(string? UserName = null, string? PhotoPath = null);
        Task ITSMSRequestProcessComplete(string? UserName = null, string? PhotoPath = null);
        Task ITSMSRequestRejected(string? UserName = null, string? PhotoPath = null);
        Task ITSMSRequestRecived(string? UserName = null, string? PhotoPath = null);
        #endregion
        #region PMIS Signals
        Task NewPMISProjectActivityRecieved(string? UserName = null, string? PhotoPath = null);
        Task PMISActivityHasRecomendation(string? UserName = null, string? PhotoPath = null);
        Task PMISActivityCompleted(string? UserName = null, string? PhotoPath = null);
        #endregion


        Task News(int newsId, string? Message = null, string? UserName = null, string? PhotoPath = null);
    }
}
