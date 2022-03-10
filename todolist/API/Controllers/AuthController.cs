using System;
using System.Threading.Tasks;
using API.Auth;
using Microsoft.AspNetCore.Mvc;
using API.Users;

namespace API.Controllers
{
    [Route("users")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthenticator authenticator;

        public AuthController(IUserRepository userRepository, IAuthenticator authenticator)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }
    }
}