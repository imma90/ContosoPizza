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
        private static RestClient _client;
        
        [OneTimeSetUp]
        public void TestClassInitialize()
        {
            var options = new RestClientOptions("https://localhost:7292/pizza"){
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            _client = new RestClient(options);
        }

        [Test]
        public async Task VerifyGetAllPizzasReturns200()
        {
            //Act
            RestResponse response = await _client.ExecuteGetAsync(Helpers.GetAllPizzasRequest());

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Get all pizzas did not return a success status code; it returned {response.StatusCode}");
        }

        [Test, TestCaseSource(typeof(TestDataClass), "GetByIdTestData")]
        public async Task<HttpStatusCode> VerifyGetPizzaByIdStatusCode(int id)
        {
            // Act
            RestResponse response = await _client.ExecuteGetAsync(Helpers.GetSinglePizzaRequest(id));

            // Assert
            return response.StatusCode;
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

            // Act
            RestResponse<Pizza> response = await _client.ExecuteGetAsync<Pizza>(Helpers.GetSinglePizzaRequest(expectedPizza.Id));

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Get pizzas with id {expectedPizza.Id} did not return a success status code; it returned {response.StatusCode}");
            Assert.AreEqual(expectedPizza.Id, response.Data.Id,$"Get pizzas with id {expectedPizza.Id} did not return pizza with this id; it returned {response.Data.Id}");
            Assert.AreEqual(expectedPizza.Name, response.Data.Name,$"Actual name should have been {expectedPizza.Name} but it was {response.Data.Name}");
            Assert.AreEqual(expectedPizza.IsGlutenFree, response.Data.IsGlutenFree,$"Actual GlutenFree should have been {expectedPizza.IsGlutenFree} but it was {response.Data.IsGlutenFree}");
        }

        [Test]
        public async Task VerifyPostWithAllValidValuesReturns201()
        {
            // Arrange
            Pizza expectedPizza = new Pizza
            {
                Name = "verify valid POST",
                IsGlutenFree = false
            };

            // Act
            RestResponse response = await _client.ExecutePostAsync(Helpers.PostPizzaRequest(expectedPizza));

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode,$"Create pizza with Name {expectedPizza.Name} and GlutenFree {expectedPizza.IsGlutenFree} did not return a success status code; it returned {response.StatusCode}");
        }
    }

    public class TestDataClass
    {
        public static IEnumerable GetByIdTestData
        {
            get
            {
                yield return new TestCaseData(1).Returns(HttpStatusCode.OK).SetName("id 1");
                yield return new TestCaseData(39393).Returns(HttpStatusCode.NotFound).SetName("nonexistent id");
                yield return new TestCaseData(0).Returns(HttpStatusCode.NotFound).SetName("id 0");
            }
        }

        public static IEnumerable PostTestData
        {
            get
            {
                yield return new TestCaseData(new Pizza {
                    Name = "POST valid pizza",
                    IsGlutenFree = false
                }).Returns(HttpStatusCode.Created).SetName("Post valid pizza");
                yield return new TestCaseData(new Pizza {
                    Name = "POST glutenfree pizza",
                    IsGlutenFree = true
                }).Returns(HttpStatusCode.Created).SetName("Post glutenfree pizza");
                yield return new TestCaseData(new Pizza {
                    Name = "POST contaminated glutenfree pizza",
                    IsGlutenFree = false
                }).Returns(HttpStatusCode.Created).SetName("Post contaminated glutenfree pizza");
            }
        }
    }
}
