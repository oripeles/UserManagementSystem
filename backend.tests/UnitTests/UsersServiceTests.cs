using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using backend.Services;
using backend.Models;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;


namespace backend.tests.UnitTests
{
    [TestFixture]
    public class UsersServiceTests
    {
        [Test]
        public async Task Valid_User_Password()
        {
         var mockConfig = new Mock<IConfiguration>();
         mockConfig.Setup(c => c["ConnectionStrings:MongoDB"]).Returns("mongodb://localhost:27017");
         mockConfig.Setup(c => c["DatabaseName"]).Returns("TestDb");

         var mockService = new Mock<UsersService>(mockConfig.Object) { CallBase = true };;//for fake to GetUserByEmailAsync , but using in real fun ValidateUserPasswordAsync.


            string plainPassword = "MyTest123";
            var user = new User
            {
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword),
                FullName = "Test User",
                
            };
            mockService.Setup(s => s.GetUserByEmailAsync("test@example.com"))
                       .ReturnsAsync(user);

            var result = await mockService.Object.ValidateUserPasswordAsync("test@example.com", plainPassword);
            Assert.NotNull(result, "User should not be null if password is correct");
            Assert.That(result!.Email, Is.EqualTo("test@example.com"));//if result =! null , EqualTo Email

        }

        [Test]
        public async Task Valid_User_Password_Wrong()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ConnectionStrings:MongoDB"]).Returns("mongodb://localhost:27017");
            mockConfig.Setup(c => c["DatabaseName"]).Returns("TestDb");

         var mockService = new Mock<UsersService>(mockConfig.Object) { CallBase = true };;
            string plainPassword = "MyTest123";
            var user = new User
            {
                FullName = "Test User",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword)
            };
            mockService.Setup(s => s.GetUserByEmailAsync("test@example.com"))
                       .ReturnsAsync(user);

            var result = await mockService.Object.ValidateUserPasswordAsync("test@example.com", "WrongPassword");

            Assert.IsNull(result, "Should return null if password is incorrect");
        }
    }
}
