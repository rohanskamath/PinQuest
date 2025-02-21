namespace dotnetcorebackend.Application.Repositories.EmailRepository
{
    public interface IEmailService
    {
        public Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
