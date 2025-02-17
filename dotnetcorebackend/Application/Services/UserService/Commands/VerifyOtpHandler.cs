using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class VerifyOtpHandler : IRequestHandler<VerifyOtpCommand, object>
    {
        private readonly IMemoryCache _memoryCache;
        public VerifyOtpHandler(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<object> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(request.Email, out string? cachedOtp))
                {
                    if (cachedOtp == request.Otp)
                    {
                        _memoryCache.Remove(request.Email);
                        return Task.FromResult<object>(new { success = true, message = "OTP verified successfully." });
                    }
                    return Task.FromResult<object>(new { success = false, message = "Invalid OTP!" });
                }
                return Task.FromResult<object>(new { success = false, message = "OTP expired or not found." });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
    }
}
