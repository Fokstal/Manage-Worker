using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ManageWorker_API.Data;
using ManageWorker_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ManageWorker_API.Service
{
    public static class TokenWorker
    {
        private static readonly int timeLifeJWTinSecond = 3600;
        private static readonly int timeLifeRefreshInDay = 15;

        public static async Task<string?> GenerateJWTTokenAsync(string login)
        {
            if (await new AppDbContext().User.FirstOrDefaultAsync(user => user.Login == login) is null) return null;

            List<Claim> claims = [new(ClaimTypes.Name, login)];

            JwtSecurityToken jwt = new(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.Add(TimeSpan.FromSeconds(timeLifeJWTinSecond)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public static RefreshToken GenerateRefreshToken(User user)
        {
            RefreshToken refreshToken = new();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            refreshToken.Value = Convert.ToBase64String(randomNumber).Replace('/', '#');
            refreshToken.ExpiryTime = DateTime.Now.Add(TimeSpan.FromDays(timeLifeRefreshInDay));
            refreshToken.UserId = user.Id;
            refreshToken.User = user;

            return refreshToken;
        }
    }
}