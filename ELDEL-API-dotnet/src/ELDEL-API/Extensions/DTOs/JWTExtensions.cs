using System;
using System.Security.Claims;
using ELDEL_API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ELDEL_API.JWTExtensions
{
    public static class JWTExtensions
    {
        public static AccountDTO GetAccountFromAccessToken(this ControllerBase controller)
        {
            try
            {
                var account = new AccountDTO()
                {
                    Id = controller.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Email = controller.User.FindFirst(ClaimTypes.Email).Value,
                    FirstName = controller.User.FindFirst(ClaimTypes.GivenName).Value,
                    LastName = controller.User.FindFirst(ClaimTypes.Surname).Value,
                    FullName = controller.User.FindFirst(ClaimTypes.Name).Value,
                };

                return account;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
