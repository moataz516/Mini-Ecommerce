﻿using System.Security.Claims;

namespace ECommerceProject
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(GetUserId(user))?.Value;
        }
    }
}
