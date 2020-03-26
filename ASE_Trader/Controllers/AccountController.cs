using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASE_Trader.Data;
using ASE_Trader.Models.BindingModels;
using ASE_Trader.Models.Dto;
using ASE_Trader.Models.EntityModels;
using ASE_Trader.Models.Roles;
using ASE_Trader.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static BCrypt.Net.BCrypt;

namespace ASE_Trader.Controllers
{
    //[ApiController, Route("api/[controller]"),Authorize]
    public class AccountController : Controller
    {
        readonly ApplicationDbContext _context;
        readonly IMapper _mapper;
        readonly IConfiguration _configuration;
        const int BcryptWorkfactor = 11;

        public AccountController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        // Registere en ny bruger i databasen
        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Register(RegisterViewModel reguser)
        {
            reguser.Email = reguser.Email.ToLower();
            var emailExist = await _context.Users.Where(u => u.Email == reguser.Email).FirstOrDefaultAsync();
            if (emailExist != null)
                return BadRequest(new { errorMessage = "Email Already in use" });
            User user = new User();
            user.FirstName = reguser.FirstName;
            user.LastName = reguser.LastName;
            user.Email = reguser.Email;
            
            //  = _mapper.Map<User>(reguser);
            if (reguser.Email == "davidbaekhoj@hotmail.com")
            {
                user.AccountType = Role.Admin;
            }
            else
            {
                user.AccountType = Role.Stockstrader;
            }
            user.PwHash = BCrypt.Net.BCrypt.HashPassword(reguser.Password, BcryptWorkfactor);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var jwtToken = new TokenDto();
            jwtToken.Token = GenerateToken(user);
            return CreatedAtAction("Get", new { id = user.UserId }, jwtToken);
            //return View();
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Login(LoginViewModel login)
        {
            login.Email = login.Email.ToLower();
            var user = await _context.Users.Where(u => u.Email == login.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                var validPwd = Verify(login.Password, user.PwHash);
                if (validPwd)
                {
                    return new TokenDto { Token = GenerateToken(user) };
                }
            }
            ModelState.AddModelError(string.Empty, "Forkert brugernavn eller password");
            return BadRequest(ModelState);
        }



        

        private string GenerateToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.AccountType.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(10)).ToUnixTimeSeconds().ToString()),
            };
            var secret = _configuration["JwtSecret"];
            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}