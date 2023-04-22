using NUnit.Framework;
using System.Net;
using System.Collections;
using RestSharp;
using ContosoPizza.Models;

namespace ApiChecks
{
    [TestFixture]
    public class PostChecksClass
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

        
        [Test, TestCaseSource(typeof(PostTestDataClass), "PostTestData")]
        public async Task<HttpStatusCode> VerifyPostPizzaStatusCode(Pizza pizza)
        {
            // Arrange
            Pizza expectedPizza = Helpers.CreatePizza();
            
            // Act
            RestResponse response = await _client.ExecutePostAsync(Helpers.PostPizzaRequest(expectedPizza));

            return response.StatusCode;
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

    public class PostTestDataClass
    {

        public static IEnumerable PostTestData
        {
            get
            {
                yield return new TestCaseData(Helpers.CreatePizza(name:"POST valid pizza"))
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
