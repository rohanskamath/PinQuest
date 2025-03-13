using dotnetcorebackend.Application.Repositories.EmailRepository;
using dotnetcorebackend.Application.Repositories.UserRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class SendOtpHandler : IRequestHandler<SendOtpCommnd, object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        public SendOtpHandler(IUserRepository userRepository, IMemoryCache cache, IEmailService emailService)
        {
            _userRepository = userRepository;
            _cache = cache;
            _emailService = emailService;
        }

        public async Task<object> Handle(SendOtpCommnd request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser == null)
                {
                    return new { success = false, message = "User not found!.. Kindly register!" };

                }
                // Generate OTP
                var otp = GenerateOtp();

                // Store OTP in cahche with 1-min expiry
                _cache.Set(request.Email, otp, TimeSpan.FromMinutes(1));

                // Send OTP to Users provided email
                var htmlContent = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <title>OTP Email Template</title>
</head>
<body>
    <div style='font-family: Helvetica, Arial, sans-serif; min-width: 1000px; overflow: auto; line-height: 2;'>
        <div style='margin: 50px auto; width: 70%; padding: 20px 0;'>
            <div style='border-bottom: 1px solid #eee;'>
                <a href='' style='font-size: 1.4em; color: #660033; text-decoration: none; font-weight: 600;'>PinQuest</a>
            </div>
            <p style='font-size: 1.1em;'>Hi,</p>
            <p>Thank you for using PinQuest. <br/> Use the following OTP to complete your Password Recovery Procedure.<br /> OTP is valid for 2 minutes.</p>
            <h2 style='background: #660033; margin: 0 auto; width: max-content; padding: 0 10px; color: #fff; border-radius: 4px;'>{otp}</h2>
            <p style='font-size: 0.9em;'>Thanks & Regards,<br />PinQuest Team</p>
            <hr style='border: none; border-top: 1px solid #eee;' />
            <p style='font-size: 12px; color: #888;'>If you didn't request this, please ignore this email.</p>
        </div>
    </div>
</body>
</html>
";

                var result = await _emailService.SendEmailAsync(request.Email, "Your one-time OTP verifiation", htmlContent);
                return new { success = result, message = "OTP sent successfully." };

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }

        public static string GenerateOtp(int length = 4)
        {
            var randomNumber = new Random();
            var otp = "";
            for (int i = 0; i < length; i++)
            {
                otp = otp + randomNumber.Next(0, 10).ToString();
            }
            return otp;
        }
    }
}
