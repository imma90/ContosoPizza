using NUnit.Framework;
using System.Net;
using System.Collections;
using RestSharp;
using ContosoPizza.Models;

namespace ApiChecks
{
    [TestFixture]
    public class ApiChecksClass
    {
        [Test]
        public async Task VeryfyGetAllPizzasReturns200()
        {
            // Arrange
            var options = new RestClientOptions("https://localhost:7292/pizza"){
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            var client = new RestClient(options);
            var request = new RestRequest();

            //Act
            RestResponse response = await client.ExecuteGetAsync(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Get all pizzas did not return a success status code; it returned {response.StatusCode}");
        }

        [Test]
        public async Task VerifyGetPizzaById1Returns200()
        {
            // Arrange
            int id = 1;
            var options = new RestClientOptions($"https://localhost:7292/pizza/{id}"){
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            var client = new RestClient(options);
            var request = new RestRequest();

            // Act
            RestResponse response = await client.ExecuteGetAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Get pizzas with id {id} did not return a success status code; it returned {response.StatusCode}");

        }

        [Test]
        public async Task VerifyGetPizzaWithId1ReturnsPizza()
        {
            // Arrange
            Pizza expectedPizza = new Pizza
            {
                Id = 1,
                Name = "Classic Italian",
                IsGlutenFree = false
            };

            var options = new RestClientOptions($"https://localhost:7292/pizza/{expectedPizza.Id}"){
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            var client = new RestClient(options);
            var request = new RestRequest();

            // Act
            RestResponse<Pizza> response = await client.ExecuteGetAsync<Pizza>(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Get pizzas with id {expectedPizza.Id} did not return a success status code; it returned {response.StatusCode}");
            Assert.AreEqual(expectedPizza.Id, response.Data.Id,$"Get pizzas with id {expectedPizza.Id} did not return pizza with this id; it returned {response.Data.Id}");
            Assert.AreEqual(expectedPizza.Name, response.Data.Name,$"Actual name should have been {expectedPizza.Name} but it was {response.Data.Name}");
            Assert.AreEqual(expectedPizza.IsGlutenFree, response.Data.IsGlutenFree,$"Actual GlutenFree should have been {expectedPizza.IsGlutenFree} but it was {response.Data.IsGlutenFree}");
        }
    }
}
