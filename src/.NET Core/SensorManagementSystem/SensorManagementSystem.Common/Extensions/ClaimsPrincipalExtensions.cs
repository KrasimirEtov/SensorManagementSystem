using System;
using System.Security.Claims;

namespace SensorManagementSystem.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            int userId = 0;
            
            if (int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int parsedUserId))
			{
                userId = parsedUserId;
			}

            return userId;
        }
    }
}
