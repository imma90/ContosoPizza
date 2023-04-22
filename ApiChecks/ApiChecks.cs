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

        [Test, TestCaseSource(typeof(TestDataClass), "PostTestData")]
        public async Task<HttpStatusCode> VerifyPostPizzaStatusCode(Pizza pizza)
        {
            // Arrange
            Pizza expectedPizza = Helpers.CreatePizza();
            
            // Act
            RestResponse response = await _client.ExecutePostAsync(Helpers.PostPizzaRequest(expectedPizza));

            return response.StatusCode;
        }

        [Test]
        public async Task VerifyGetPizzaWithId1ReturnsPizza()
        {
            // Arrange
            int expectedId = 1;
            Pizza expectedPizza = Helpers.CreatePizza(name:"Classic Italian",isGlutenFree:false);

            // Act
            RestResponse<Pizza> response = await _client.ExecuteGetAsync<Pizza>(Helpers.GetSinglePizzaRequest(expectedId));

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Get pizzas with id {expectedId} did not return a success status code; it returned {response.StatusCode}");
            Assert.AreEqual(expectedId, response.Data.Id,$"Get pizzas with id {expectedId} did not return pizza with this id; it returned {response.Data.Id}");
            Assert.AreEqual(expectedPizza.Name, response.Data.Name,$"Actual name should have been {expectedPizza.Name} but it was {response.Data.Name}");
            Assert.AreEqual(expectedPizza.IsGlutenFree, response.Data.IsGlutenFree,$"Actual GlutenFree should have been {expectedPizza.IsGlutenFree} but it was {response.Data.IsGlutenFree}");
        }

        

        [Test]
        public async Task VerifyPostWithAllValidValuesReturns201()
        {
            // Arrange
            Pizza pizza = Helpers.CreatePizza(name:"verify valid POST");
    

            // Act
            RestResponse response = await _client.ExecutePostAsync(Helpers.PostPizzaRequest(pizza));

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode,$"Create pizza with Name {pizza.Name} and GlutenFree {pizza.IsGlutenFree} did not return a success status code; it returned {response.StatusCode}");
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
                yield return new TestCaseData(Helpers.CreatePizza(name:"POST valid pizza",isGlutenFree:false))
                    .Returns(HttpStatusCode.Created)
                    .SetName("Post valid pizza");
                yield return new TestCaseData(Helpers.CreatePizza(name:"POST glutenfree pizza",isGlutenFree:true))
                    .Returns(HttpStatusCode.Created)
                    .SetName("Post glutenfree pizza");
                yield return new TestCaseData(Helpers.CreatePizza(name:"POST contaminated glutenfree pizza",isGlutenFree:false))
                    .Returns(HttpStatusCode.Created)
                    .SetName("Post contaminated glutenfree pizza");
            }
        }
    }
}
