using NUnit.Framework;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace backend.tests.UnitTests
{
    [TestFixture]
    public class BCryptTests
    {
        [Test]
        public void Hash_Password()
        {
            string plainPassword = "ori1234";
            string hash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(plainPassword, hash));
        }
        [Test]
       public void Hash_Password_Wrong()
       {
        string plainPassword = "password";
        string wrongPassword = "wrongpassword";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        Assert.IsFalse(BCrypt.Net.BCrypt.Verify(wrongPassword, hashedPassword));
       }
    }
}
