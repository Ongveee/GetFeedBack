using GetFeedBack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GetFeedBack.Controllers
{
    public class BaseController : Controller
    {
        public async Task CreateAuthenticationTicket(Admins user)
        {
            var key = Encoding.ASCII.GetBytes(SiteKeys.Token);// Jwt secret Key
            var JWToken = new JwtSecurityToken(
            issuer: SiteKeys.WebSiteDomain,//your website URL
            audience: SiteKeys.WebSiteDomain,
            claims: GetUserClaims(user),
            notBefore: new DateTimeOffset(DateTime.Now).DateTime,
            expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));

            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            HttpContext.Session.SetString("JWToken", token);
        }
        private IEnumerable<Claim> GetUserClaims(Admins user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
            claims.Add(_claim);
            _claim = new Claim(ClaimTypes.Name, user.Account);
            claims.Add(_claim);

            _claim = new Claim("Role", Role.Admin);
            claims.Add(_claim);

            return claims.AsEnumerable<Claim>();
        }
        public async Task CreateAuthenticationTicketUserLogin(Users user)
        {
            var key = Encoding.ASCII.GetBytes(SiteKeys.Token);// Jwt secret Key
            var JWToken = new JwtSecurityToken(
            issuer: SiteKeys.WebSiteDomain,//your website URL
            audience: SiteKeys.WebSiteDomain,
            claims: GetUsersClaims(user),
            notBefore: new DateTimeOffset(DateTime.Now).DateTime,
            expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));

            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            HttpContext.Session.SetString("JWToken", token);
        }
        private IEnumerable<Claim> GetUsersClaims(Users user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
            claims.Add(_claim);
            _claim = new Claim(ClaimTypes.Name, user.Email);
            claims.Add(_claim);

            _claim = new Claim("Role", Role.User);
            claims.Add(_claim);

            return claims.AsEnumerable<Claim>();
        }

        private IEnumerable<Claim> GetUsersClaims()
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim("Role", Role.Guest);
            claims.Add(_claim);

            return claims.AsEnumerable<Claim>();
        }
        public struct Role
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string Guest = "Guest";
        }
    }
}
