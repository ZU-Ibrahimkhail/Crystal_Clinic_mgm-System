using Microsoft.AspNetCore.SignalR;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Constants;
using Microsoft.Extensions.Logging;
namespace Crystal_Clinic_Mgm.Application.Common.SignalR
{
    public class MessageHub : Hub<ISignalTypes>, IMessageHubClient
    {
        private static Dictionary<string, Guid> _UserId = new();
        private ILogger<MessageHub> _logger;
        private readonly IHubContext<MessageHub, ISignalTypes> _hubContext;
        private readonly ILoggedInUser _loggedInUser;
        public MessageHub(IHubContext<MessageHub, ISignalTypes> hubContext, ILoggedInUser loggedInUser, ILogger<MessageHub> logger)
        {
            _logger = logger;
            _hubContext = hubContext;
            _loggedInUser = loggedInUser;
        }
   


        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"] ?? string.Empty;
            _UserId.Add(Context.ConnectionId, Guid.Parse(userId!));
            _logger.LogInformation($"Connection Id: {Context.ConnectionId}");
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
            _logger.LogInformation($"Push Notification to User Id: {userId}");
            IReadOnlyList<string> connectionId = _UserId.Where(p => p.Value == userId).Select(p => p.Key).ToList();
            switch (NotificationType)
            {

                #region New Service Session Record Signals 
                case Constants.NotificationMessage.NewServiceSessionRecord:
                    await _hubContext.Clients.Clients(connectionId).NewServiceSessionRecord(UserName, PhotoPath);
                    _logger.LogInformation($"New Service Session Record Signal Sent to User Id: {userId}");
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
