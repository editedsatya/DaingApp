using System;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Model;
using DatingApp.API.dtos;
using System.Threading.Tasks;
using DatingApp.API.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo,IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegister userForRegisterDto )
        {   

                //validate request;
               
               // if(!ModelState.IsValid)
               //return BadRequest(ModelState);

                userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
                if(await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("User already exists");

                var UserToCreate = new User
                {
                    Username = userForRegisterDto.Username
                };


                var createUserName = await _repo.Register(UserToCreate,userForRegisterDto.Password);

                return StatusCode(201);
                    
        }

        
        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserForLogin userForLogin)
        {
            var userfromrepo= await _repo.Login(userForLogin.UserName.ToLower(),userForLogin.Password);
            if(userfromrepo==null)
            return Unauthorized();

            var claims=new []
            {
                new Claim(ClaimTypes.NameIdentifier,userfromrepo.id.ToString()),
                new Claim(ClaimTypes.Name,userfromrepo.Username)
            };

            var key=new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.
            GetSection("AppSetting:Token").Value));

            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var TokenDecotor=new SecurityTokenDescriptor{
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials=creds
            };

            var tokenHanlder=new JwtSecurityTokenHandler();
            var Token=tokenHanlder.CreateToken(TokenDecotor);
            return Ok(new {
                Token= tokenHanlder.WriteToken(Token)
            });

        }

    } 
}