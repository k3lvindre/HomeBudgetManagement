using Microsoft.AspNetCore.Identity;

namespace HomeBudgetManagement.Api.Core.Services
{
    public interface IAuthService
    {
        public string GenerateToken(IdentityUser userAccount);
    }
}
