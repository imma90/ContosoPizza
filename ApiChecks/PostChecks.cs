using NUnit.Framework;
using System.Net;
using System.Collections;
using RestSharp;
using ContosoPizza.Models;

namespace ApiChecks
{
    [TestFixture]
    public class PostChecksClass : ApiChecksBase
    {
        private Pizza testPizza;

        [TearDown]
        public async Task TestDataCleanup()
        {
            RestResponse response = await _client.DeleteAsync(Helpers.DeletePizzaRequest(testPizza.Id));
            if(response.StatusCode != HttpStatusCode.NoContent)
            {
                Console.Write($"Unable to delete {testPizza.Id} - {response.StatusCode}");
            }
        }        

        
        [Test, TestCaseSource(typeof(PostTestDataClass), "PostTestData")]
        public async Task<HttpStatusCode> VerifyPostPizzaStatusCode(Pizza pizza)
        {            
            // Act
            RestResponse<Pizza> response = await _client.ExecutePostAsync<Pizza>(Helpers.PostPizzaRequest(pizza));
            testPizza = response.Data;

            return response.StatusCode;
        }
    }

    public class PostTestDataClass
    {

        public static IEnumerable PostTestData
        {
            get
            {
                yield return new TestCaseData(Helpers.CreatePizza("POST valid pizza"))
                    .Returns(HttpStatusCode.Created)
                    .SetName("Post valid pizza");
                yield return new TestCaseData(Helpers.CreatePizza("POST glutenfree pizza",true))
                    .Returns(HttpStatusCode.Created)
                    .SetName("Post glutenfree pizza");
                yield return new TestCaseData(Helpers.CreatePizza("POST contaminated glutenfree pizza",false))
                    .Returns(HttpStatusCode.Created)
                    .SetName("Post contaminated glutenfree pizza");
            }
        }
    }
}
