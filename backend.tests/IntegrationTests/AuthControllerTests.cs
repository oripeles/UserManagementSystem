using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using backend.Models;

namespace backend.tests.IntegrationTests
{
    [TestFixture]
    public class AuthIntegrationTests
    {
        private WebApplicationFactory<Program> _factory; 
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Register_Login()
        {
            var registerDto = new UserRegisterDto
            {
                Email = "ori@example.com",
                Password = "Secret",
                FullName = "ori"
            };
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            if (registerResponse.StatusCode == HttpStatusCode.BadRequest)
            {  
                Console.WriteLine("User already exists");
             }
            else
           {
               Assert.That(registerResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
           }

            
        
            var loginDto = new UserLoginDto
            {
                Email = "ori@example.com",
                Password = "Secret"
            };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            Assert.That(loginResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Login should succeed");

            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            
            var json = JObject.Parse(loginContent);
            var tokenFromServer = json["token"];
            string token;
            if (tokenFromServer != null)
            {
             token = tokenFromServer.ToString();
            }
            else
            {
             token = "";
            }

            var meRequest = new HttpRequestMessage(HttpMethod.Get, "/api/auth/me");
            meRequest.Headers.Add("Authorization", $"Bearer {token}");

            var meResponse = await _client.SendAsync(meRequest);
            Assert.That(meResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "With a valid token, /me should return 200 OK");

            var meJsonText = await meResponse.Content.ReadAsStringAsync();
            var meJson = JObject.Parse(meJsonText);
            var returnedEmail = meJson["email"]?.ToString();

            Assert.That(returnedEmail, Is.EqualTo("ori@example.com"));
        }
    }
}
