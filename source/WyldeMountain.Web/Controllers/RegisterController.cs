using System;
using WyldeMountain.Web.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WyldeMountain.Web.Models.Authentication;

namespace WyldeMountain.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> logger;
        private readonly IGenericRepository genericRepository;

        public RegisterController(ILogger<RegisterController> logger, IGenericRepository genericRepository)
        {
            this.logger = logger;
            this.genericRepository = genericRepository;
        }
        
        /// <summary>
        /// Registers a new user. Returns the user's ID if successful.
        /// </summary>
        [HttpPost]
        public ActionResult<User> Register(RegistrationRequest request)
        {
            // TODO: validate email address
            // TODO: validate password is sufficiently long

            var emailAddress = request.EmailAddress;
            var plainTextPassword = request.Password;

            var newUser = new User() { EmailAddress = emailAddress };
            var existingUser = this.genericRepository.SingleOrDefault<User>(u => u.EmailAddress == emailAddress);
            if (existingUser != null)
            {
                return BadRequest(new ArgumentException(nameof(emailAddress)));
            }

            this.genericRepository.Insert<User>(newUser);
            newUser = this.genericRepository.SingleOrDefault<User>(u => u.EmailAddress == emailAddress); // Load back with ID

            var auth = new Auth() { UserId = newUser.Id, HashedPassword = plainTextPassword, Salt = "TODO" };
            this.genericRepository.Insert<Auth>(auth);

            return Ok(newUser);
        }

        public class RegistrationRequest
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }
    }
}