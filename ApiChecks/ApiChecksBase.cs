using NUnit.Framework;
using RestSharp;

namespace ApiChecks
{
    [SetUpFixture]
    public class ApiChecksBase
    {
        public static string _baseUrl;
        public static RestClient _client;

        [OneTimeSetUp]
        public void TestFictureInitalize()
        {
            _baseUrl = "https://localhost:7292/pizza";

            var options = new RestClientOptions(_baseUrl){
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            _client = new RestClient(options);
        }
    }  
}
