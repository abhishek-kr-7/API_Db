using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DbAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DbAPI.Controllers;



public class userController : ControllerBase
{
    private readonly StudentContext _context;
    private readonly jwtSettings _jwtsettings;
    public userController(StudentContext context,IOptions<jwtSettings> options)
    {
        _context = context;
        this._jwtsettings = options.Value;
    }

    [HttpPost("Authenticate")]
    public async Task<ActionResult> Authenticate([FromBody]userCred userCred)
    {
        var user = await this._context.userCreds.FirstOrDefaultAsync(item => item.username == userCred.username && item.passwrd == userCred.passwrd);
        if(user == null)
            return Unauthorized();

        //Generate Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenkey = Encoding.UTF8.GetBytes(this._jwtsettings.securitykey);
        var tokendesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[] {new Claim(ClaimTypes.Name,user.username)}
            ),
            Expires = DateTime.Now.AddSeconds(20),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey),SecurityAlgorithms.HmacSha256)

        };
        var token = tokenHandler.CreateToken(tokendesc);
        string finalToken = tokenHandler.WriteToken(token);
        
        return Ok(finalToken);
    }
}