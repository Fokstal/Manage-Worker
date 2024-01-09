using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ManageWorker_API.Data;
using ManageWorker_API.Models;
using ManageWorker_API.Models.Dto;
using ManageWorker_API.Service;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ManageWorker_API.Controllers
{
    [Route("[controller]")]
    public class AccountAPIController : ControllerBase
    {
        private static readonly int timeLifeJWTinSecond = 3600;
        private static readonly string keyToAddUser = "KeyToAdd99Key";

        [HttpPost("{key}", Name = "SignUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult SignUp(string key, [FromBody] UserDTO userDTO)
        {
            if (key != keyToAddUser)
            {
                ModelState.AddModelError("Account Error", "Key to add is not correct!");

                return BadRequest(ModelState);
            }

            using (AppDbContext db = new())
            {

                if (db.User.FirstOrDefault(user => user.Login.ToLower() == userDTO.Login.ToLower()) is not null)
                {
                    ModelState.AddModelError("CustomError", "User already Exists!");

                    return BadRequest(ModelState);
                }

                User user = new()
                {
                    Login = userDTO.Login,
                    DateCreated = DateTime.Now
                };

                if (userDTO.Email is not null) user.Email = userDTO.Email;

                user.PasswordHash = GenerateSpecialHashBySHA512(user, userDTO.Password);


                db.User.Add(user);

                db.SaveChanges();
            }

            string? jwt = Authentication(userDTO.Login);

            if (jwt is null) return Unauthorized();

            return Ok(jwt);
        }

        [HttpPost(Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Login([FromBody] UserDTO userDTO)
        {
            using (AppDbContext db = new())
            {
                User? user = db.User.FirstOrDefault(user => user.Login == userDTO.Login);

                if (user is null) return NotFound();

                string passwordHash = GenerateSpecialHashBySHA512(user, userDTO.Password);

                if (passwordHash != user.PasswordHash) return Unauthorized();
            }

            string? jwt = Authentication(userDTO.Login);

            if (jwt is null) return Unauthorized();

            return Ok(jwt);
        }


        private static string GenerateSHA512SaltedHash(string password, string salt = "")
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hash = SHA512.HashData(bytes);

            return Convert.ToBase64String(hash);
        }

        private static string GenerateSpecialHashBySHA512(User user, string password)
        {
            return GenerateSHA512SaltedHash
            (
                GenerateSHA512SaltedHash(user.Login + password),
                GenerateSHA512SaltedHash(user.DateCreated.ToString())
            );
        }

        private static string? Authentication(string login)
        {
            if (new AppDbContext().User.FirstOrDefault(user => user.Login == login) is null) return null;

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
    }
}