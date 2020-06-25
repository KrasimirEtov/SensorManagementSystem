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

            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            return userId;
        }
    }
}
