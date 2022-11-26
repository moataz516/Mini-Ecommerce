using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ECommerceProject.Models.Data
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser,IdentityRole>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser>userManager , RoleManager<IdentityRole>roleManager
            ,IOptions<IdentityOptions>options):base(userManager,roleManager,options)
        {
            _userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("LastName", user.LastName ?? ""));
            identity.AddClaim(new Claim("FullName", user.FirstName +' '+  user.LastName ?? ""));
            identity.AddClaim(new Claim("Photo", "~/image/user_photo/"+user.ProfileImageUrl ?? "~/image/user_photo/default.png"));
            identity.AddClaim(new Claim("UserName", user.UserName ?? ""));
            identity.AddClaim(new Claim("Email", user.Email ?? ""));
            identity.AddClaim(new Claim("UserId", user.Id ?? ""));


            //var userClaims = await _userManager.GetClaimsAsync(user);
            //var roles = await _userManager.GetRolesAsync(user);
            //var roleClaims = new List<Claim>();
            //foreach (var role in roles)
            //    roleClaims.Add(new Claim("roles", role));
            //var claims = new[]
            //{
            //    new Claim("UserName",user.UserName ?? ""),
            //    new Claim("FirstName",user.FirstName ?? ""),
            //    new Claim("LastName",user.LastName ?? ""),
            //    new Claim("Email",user.Email ?? ""),
            //    new Claim("UserId",user.Id),

            //}.Union(userClaims).Union(roleClaims);


            return identity;
        }


    }
}
