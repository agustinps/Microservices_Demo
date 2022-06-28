namespace ms.shop.application.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendOrderCanceledEmail(string email)
        {
            Console.WriteLine("Su pedido ha sido cancelado por falta de stock");
            await Task.FromResult(true);
        }

        public async Task SendOrderPreparedEmail(string email)
        {
            Console.WriteLine("Su pedido está preparado y listo para enviar");
            await Task.FromResult(true);
        }

        public async Task SendOrderShippedEMail(string email)
        {
            Console.WriteLine("Su pedido ha sido enviado y lo recibirá en los proximos días");
            await Task.FromResult(true);
        }        
    }
}
