using NUnit.Framework;
using System.Net;
using System.Collections;
using RestSharp;

namespace ApiChecks
{
    [TestFixture]
    public class ApiChecksClass
    {
        [Test]
        public async Task VeryfyGetAllPizzasReturns200()
        {
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
    }
}
