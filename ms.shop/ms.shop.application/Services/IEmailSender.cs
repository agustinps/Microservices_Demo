namespace ms.shop.application.Services
{
    public interface IEmailSender
    {
        Task SendOrderCanceledEmail(string email);
        Task SendOrderPreparedEmail(string email);
        Task SendOrderShippedEMail(string email);
    }
}
