using NUnit.Framework;
using RestSharp;
using System.Net;
using ContosoPizza.Models;

namespace ApiChecks
{
    [TestFixture]
    public class PostChecks
    {
        private static string _baseUrl;
        private static RestClient _client;

        [OneTimeSetUp]
        public void TestClassInitalize()
        {
            var options = new RestClientOptions("https://localhost:7292/pizza"){
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            _client = new RestClient(options);
        }

        private Pizza testPizza;
        [SetUp]
        public async Task TestDataSetup()
        {
            Pizza pizza = Helpers.CreatePizza(name:$"Check delete pizza {new DateTime().Ticks}");

            // Act
            RestResponse<Pizza> response = await _client.ExecutePostAsync<Pizza>(Helpers.PostPizzaRequest(pizza));

            testPizza = response.Data;
        }
        

        [Test]
        public async Task VerifyDeleteWithValidIdReturn204()
        {
            // Arrange
            var id = testPizza.Id;

            // Act
            RestResponse response = await _client.DeleteAsync(Helpers.DeletePizzaRequest(id));

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, $"Delete pizza {testPizza.Id} should have returned a NoContent response; instead it returned {response.StatusCode}");
        
        }

        [Test]
        public async Task VerifyDeleteWithValidIdDeletesPizza()
        {
            // Arrange
            var id = testPizza.Id;

            // Act
            RestResponse deleteResponse = await _client.DeleteAsync(Helpers.DeletePizzaRequest(id));
            RestResponse getResponse = await _client.GetAsync(Helpers.GetSinglePizzaRequest(id));

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode, $"Delete pizza {testPizza.Id} should have returned a NoContent response; instead it returned {deleteResponse.StatusCode}");
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode, $"Get deleted pizza {testPizza.Id} should have returned a NotFound respones; instead it returned {getResponse.StatusCode}");
        }

        [Test]
        public async Task VerifyDeleteNonexistentPizzaReturn404()
        {
            // Arrange
            int id = 482034;

            // Act
            RestResponse response = await _client.DeleteAsync(Helpers.DeletePizzaRequest(id));

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, $"Delete pizza {testPizza.Id} should have returned a NotFoun response; instead it returned {response.StatusCode}");
        }
    }

    
}
