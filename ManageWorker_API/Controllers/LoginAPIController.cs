using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ManageWorker_API.Data;
using ManageWorker_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ManageWorker_API.Controllers
{
    [Route("[controller]")]
    public class LoginAPIController : ControllerBase
    {
        [HttpGet("{login}", Name = "Auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Auth(string login)
        {
            if (!UserStore.LoginList.Contains(login)) return Unauthorized();

            var claims = new List<Claim> { new(ClaimTypes.Name, login) };

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.Add(TimeSpan.FromSeconds(5)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new
            {
                access_token = encodedJwt,
                jwt_life = jwt.ValidTo - jwt.ValidFrom,
            });
        }

        [HttpPost("{id:int}")]
        [AuthorizeExpiry]
        public IActionResult GetSecretData(int id)
        {
            var objResult = new
            {
                secret_data = "secret is 123",
                id_ = id,
            };

            return Ok(objResult);
        }
    }
}