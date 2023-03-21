using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using DbAPI.Models;
using System.Security.Claims;
using System.Net.Http.Headers;

 namespace DbAPI.Handler
 {
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>{
        
        private readonly StudentContext _stContext;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,StudentContext stContext) : base(options,logger,encoder,clock)
        {
            _stContext = stContext;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync(){
            if(!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("");
            
            var _headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(_headerValue.Parameter);
            string credentials = System.Text.Encoding.UTF8.GetString(bytes);
            if(!string.IsNullOrEmpty(credentials)){
                string[] arr = credentials.Split(":");
                string username = arr[0];
                string password = arr[1];
                var user = this._stContext.Students.FirstOrDefault(items => items.userid == username && items.Password == password);
                if(user == null)
                    return AuthenticateResult.Fail("unauthorized");

                var claim = new[]{new Claim(ClaimTypes.Name,username)};
                var identity = new ClaimsIdentity(claim,Scheme.Name);
                var prinicpal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(prinicpal,Scheme.Name);
                    return AuthenticateResult.Success(ticket);
            }
            else{
                return AuthenticateResult.Fail("unauthorized");
            }
        }
    }
 }