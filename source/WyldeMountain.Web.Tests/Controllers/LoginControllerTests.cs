using System;
using System.Linq.Expressions;
using WyldeMountain.Web.Controllers;
using WyldeMountain.Web.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using static WyldeMountain.Web.Controllers.LoginController;
using WyldeMountain.Web.Models.Authentication;

namespace WyldeMountain.Web.Tests.Controllers
{
    [TestFixture]
    public class LoginControllerTests
    {
        [Test]
        public void LoginReturnsOkWithUserIfCredentialsMatch()
        {
            // Arrange
            const string expectedEmail = "test@test.com";
            const string expectedPassword = "password";

            var request = new LoginRequest() { EmailAddress = expectedEmail, Password = expectedPassword };

            var existingUser = new User() { EmailAddress = expectedEmail, Id = new MongoDB.Bson.ObjectId() };
            var credentials = new Auth() { UserId = existingUser.Id, HashedPasswordWithSalt = expectedPassword };
            var repository = new Mock<IGenericRepository>();
            repository.Setup(u => u.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns(existingUser);
            repository.Setup(a => a.SingleOrDefault(It.IsAny<Expression<Func<Auth, bool>>>())).Returns(credentials);
            
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["Jwt:Key"]).Returns(Guid.NewGuid().ToString());

            var controller = new LoginController(new Mock<ILogger<LoginController>>().Object, configuration.Object, repository.Object);

            // Act
            var response = controller.Login(request).Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(OkObjectResult)));
            var obj = ((OkObjectResult)response).Value;
            Assert.That(obj.GetType().GetProperty("token").GetValue(obj), Is.Not.Null);

        }

        [Test]
        public void LoginReturnsBadRequestIfUserDoesntExistsInDatabase()
        {
            // Arrange
            const string expectedEmail = "test@test.com";

            var request = new LoginRequest() { EmailAddress = expectedEmail };

            var controller = new LoginController(new Mock<ILogger<LoginController>>().Object, new Mock<IConfiguration>().Object, new Mock<IGenericRepository>().Object);

            // Act
            var response = controller.Login(request).Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(UnauthorizedObjectResult)));
            var obj = ((UnauthorizedObjectResult)response).Value;
            Assert.That(obj, Is.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void LoginReturnsUnauthorizedIfPasswordDoesntMatchHash()
        {
            // Arrange
            const string expectedEmail = "test@test.com";
            const string expectedPassword = "password";

            var request = new LoginRequest() { EmailAddress = expectedEmail, Password = "wrong password!" };

            var existingUser = new User() { EmailAddress = expectedEmail, Id = new MongoDB.Bson.ObjectId() };
            var credentials = new Auth() { UserId = existingUser.Id, HashedPasswordWithSalt = expectedPassword };

            var repository = new Mock<IGenericRepository>();
            repository.Setup(u => u.SingleOrDefault<User>(It.IsAny<Expression<Func<User, bool>>>())).Returns(existingUser);
            repository.Setup(u => u.SingleOrDefault<Auth>(It.IsAny<Expression<Func<Auth, bool>>>())).Returns(credentials);
            
            var controller = new LoginController(new Mock<ILogger<LoginController>>().Object, new Mock<IConfiguration>().Object, repository.Object);

            // Act
            var response = controller.Login(request).Result;

            // Assert
            Assert.That(response, Is.TypeOf(typeof(UnauthorizedObjectResult)));
            var obj = ((UnauthorizedObjectResult)response).Value;
            Assert.That(obj, Is.TypeOf(typeof(ArgumentException)));
        }
    }
}