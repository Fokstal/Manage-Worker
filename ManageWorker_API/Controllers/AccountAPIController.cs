using ManageWorker_API.Data;
using ManageWorker_API.Models;
using ManageWorker_API.Models.Dto;
using ManageWorker_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageWorker_API.Controllers
{
    [Route("account/")]
    public class AccountAPIController : ControllerBase
    {
        private static readonly string keyToAddUser = "KeyToAdd99Key";


        [HttpPost("sign-up/{key}", Name = "SignUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignUpAsync(string key, [FromBody] UserDTO userDTO)
        {
            if (key != keyToAddUser)
            {
                ModelState.AddModelError("Account Error", "Key to add is not correct!");

                return BadRequest(ModelState);
            }

            using (AppDbContext db = new())
            {

                if (await db.User.FirstOrDefaultAsync(user => user.Login.ToLower() == userDTO.Login.ToLower()) is not null)
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

                user.PasswordHash = HashWorker.GenerateSpecialHashBySHA512(user, userDTO.Password);


                await db.User.AddAsync(user);

                // db.RefreshToken.Add(GenerateRefreshToken());

                await db.SaveChangesAsync();
            }

            // string? jwt = GenerateJWTToken(userDTO.Login);

            // if (jwt is null) return Unauthorized();

            // return Ok(jwt);
            
            return Ok();
        }

        [HttpPost("login/", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromBody] UserDTO userDTO)
        {
            using (AppDbContext db = new())
            {
                User? user = await db.User.FirstOrDefaultAsync(user => user.Login == userDTO.Login);

                if (user is null) return NotFound();

                string passwordHash = HashWorker.GenerateSpecialHashBySHA512(user, userDTO.Password);

                if (passwordHash != user.PasswordHash) return Unauthorized();
            }

            RefreshToken refreshToken = TokenWorker.GenerateRefreshToken();
            string? jwt = await TokenWorker.GenerateJWTTokenAsync(userDTO.Login);

            if (jwt is null) return Unauthorized();

            return Ok(new
            {
                access_token = jwt,
                refresh_token = refreshToken.Value
            });
        }

        [HttpPost("auth-refresh/{refreshTokenValue}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AuthByRefreshTokenAsync(string? refreshTokenValue)
        {
            if (refreshTokenValue is null) return BadRequest();

            using (AppDbContext db = new())
            {
                RefreshToken? refreshToken = await db.RefreshToken.FirstOrDefaultAsync(token => token.Value == refreshTokenValue);

                if (refreshToken is null) return NotFound();

                if (refreshToken.ExpiryTime > DateTime.Now)
                {
                    ModelState.AddModelError("Custom Error", "Refresh token expiry time is up!");

                    return Unauthorized(ModelState);
                }

                User? user = await db.User.FirstOrDefaultAsync(user => user.Id == refreshToken.Id);

                if (user is null) return NotFound();

                RefreshToken newRefreshToken = TokenWorker.GenerateRefreshToken();

                refreshToken = newRefreshToken;

                await db.SaveChangesAsync();

                return Ok(new
                {
                    access_token = TokenWorker.GenerateJWTTokenAsync(user.Login),
                    refresh_token = newRefreshToken,
                });
            }
        }
    }
}