namespace Crystal_Clinic_Mgm.Application.Common.SignalR
{
    public interface IMessageHubClient
    {
        Task PushAsync(Guid? userId, int NotificationType);
        Task NewsBroadCost(int newsId, string? message = null, List<Guid>? userId = null);
    }
}
