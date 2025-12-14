namespace WMS_WEBAPI.Interfaces
{
    public interface IResetPasswordEmailJob
    {
        void Send(string toEmail, string fullName, string token);
    }
}
