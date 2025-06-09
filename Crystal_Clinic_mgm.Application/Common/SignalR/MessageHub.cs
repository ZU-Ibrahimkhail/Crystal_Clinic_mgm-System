using Microsoft.AspNetCore.SignalR;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.Application.Common.SignalR
{
    public class MessageHub : Hub<ISignalTypes>, IMessageHubClient
    {
        private static Dictionary<string, Guid> _UserId = new();
        private readonly IHubContext<MessageHub, ISignalTypes> _hubContext;
        private readonly ILoggedInUser _loggedInUser;
        public MessageHub(IHubContext<MessageHub, ISignalTypes> hubContext, ILoggedInUser loggedInUser)
        {
            _hubContext = hubContext;
            _loggedInUser = loggedInUser;
        }



        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"] ?? string.Empty;
            _UserId.Add(Context.ConnectionId, Guid.Parse(userId!));
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _UserId.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task PushAsync(Guid? userId, int NotificationType)
        {
            string? UserName = _loggedInUser.UserName;
            string? PhotoPath = _loggedInUser.PhotoPath;
            IReadOnlyList<string> connectionId = _UserId.Where(p => p.Value == userId).Select(p => p.Key).ToList();
            switch (NotificationType)
            {

                #region Reception Signals 
                case Constants.NotificationMessage.VisitorInOut:
                    await _hubContext.Clients.Clients(connectionId).VisitorInOut(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.CreatedNewRequest:
                    await _hubContext.Clients.Clients(connectionId).NewReceptionTrackingRecord(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.ClientRequestApproved:
                    await _hubContext.Clients.Clients(connectionId).ReceptionTrackingRecordApproved(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.ClientRequestRejected:
                    await _hubContext.Clients.Clients(connectionId).ReceptionTrackingRecordRejected(UserName, PhotoPath);
                    break;
                #endregion

                #region DMTS Signals
                #region DMTS Internal Document Signals
                case Constants.NotificationMessage.NewInternalDocument:
                    await _hubContext.Clients.Clients(connectionId).NewInternalDocumentTrackingRecord(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.InternalDocumentRejected:
                    await _hubContext.Clients.Clients(connectionId).InternalDocumentRejected(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.InternalDocumentReceived:
                    await _hubContext.Clients.Clients(connectionId).InternalDocumentRecieved(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.CreatedDocumentRecivedBack:
                    await _hubContext.Clients.Clients(connectionId).CreatedDocumentRecivedBack(UserName, PhotoPath);
                    break;
                #endregion

                #region DMTS External Document Signals
                case Constants.NotificationMessage.NewExternalDocument:
                    await _hubContext.Clients.Clients(connectionId).NewExternalDocumentTrackingRecord(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.ExternalDocumentRejected:
                    await _hubContext.Clients.Clients(connectionId).ExternalDocumentRejected(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.ExternalDocumentReceived:
                    await _hubContext.Clients.Clients(connectionId).ExternalDocumentRecieved(UserName, PhotoPath);
                    break;
                #endregion
                #endregion

                #region ITSMS Signals
                case Constants.NotificationMessage.ITSMSNewRequest:
                    await _hubContext.Clients.Clients(connectionId).NewITSMSApplicantRequestRecord(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.ITSMSRequestProcessComplete:
                    await _hubContext.Clients.Clients(connectionId).ITSMSRequestProcessComplete(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.ITSMSRequestRejected:
                    await _hubContext.Clients.Clients(connectionId).ITSMSRequestRejected(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.ITSMSRequestRecived:
                    await _hubContext.Clients.Clients(connectionId).ITSMSRequestRecived(UserName, PhotoPath);
                    break;
                #endregion

                #region PMIS Signals
                case Constants.NotificationMessage.NewPMISProjectActivityRecieved:
                    await _hubContext.Clients.Clients(connectionId).NewPMISProjectActivityRecieved(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.PMISActivityCompleted:
                    await _hubContext.Clients.Clients(connectionId).PMISActivityCompleted(UserName, PhotoPath);
                    break;
                case Constants.NotificationMessage.PMISActivityHasRecomendation:
                    await _hubContext.Clients.Clients(connectionId).PMISActivityHasRecomendation(UserName, PhotoPath);
                    break;
                    #endregion
            }
        }

        public async Task NewsBroadCost(int newsId, string? Message = null, List<Guid>? UserId = null)
        {
            IReadOnlyList<string> sender = _UserId.Where(x => x.Value == _loggedInUser.Id).Select(x => x.Key).ToList();
            if (UserId != null)
            {
                IReadOnlyList<string> connectionId = _UserId.Where(p => UserId.Contains(p.Value)).Select(p => p.Key).ToList();
                await _hubContext.Clients.Clients(connectionId).News(newsId, Message);
            }
            else
            {
                await _hubContext.Clients.AllExcept(sender).News(newsId, Message);
            }
        }
    }
}
