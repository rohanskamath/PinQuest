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
    <div style='font-family: Arial, sans-serif; padding: 20px;'>
        <h2 style='color: #4CAF50;'>Your OTP Code</h2>
        <p>Hello,</p>
        <p>Your OTP for password reset is:</p>
        <h3 style='background-color: #f3f4f6; padding: 10px; border-radius: 5px;'>{otp}</h3>
        <p>This OTP is valid for 2 minutes.</p>
        <p style='font-size: 12px; color: #888;'>If you didn't request this, please ignore this email.</p>
    </div>
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
