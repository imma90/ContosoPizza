﻿using NUnit.Framework;
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
        public async Task VeryfyGetAllPizzasReturns200()
        {
            // Arrange
            var request = new RestRequest();

            //Act
            RestResponse response = await _client.ExecuteGetAsync(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Get all pizzas did not return a success status code; it returned {response.StatusCode}");
        }

        [Test, TestCaseSource(typeof(TestDataClass), "GetByIdTestData")]
        public async Task<HttpStatusCode> VerifyGetPizzaByIdStatusCode(int id)
        {
            // Arrange
            var request = new RestRequest($"{id}");
            request.AddUrlSegment("id",id);

            // Act
            RestResponse response = await _client.ExecuteGetAsync(request);

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

            var request = new RestRequest($"{expectedPizza.Id}");
            request.AddUrlSegment("id",expectedPizza.Id);

            // Act
            RestResponse<Pizza> response = await _client.ExecuteGetAsync<Pizza>(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Get pizzas with id {expectedPizza.Id} did not return a success status code; it returned {response.StatusCode}");
            Assert.AreEqual(expectedPizza.Id, response.Data.Id,$"Get pizzas with id {expectedPizza.Id} did not return pizza with this id; it returned {response.Data.Id}");
            Assert.AreEqual(expectedPizza.Name, response.Data.Name,$"Actual name should have been {expectedPizza.Name} but it was {response.Data.Name}");
            Assert.AreEqual(expectedPizza.IsGlutenFree, response.Data.IsGlutenFree,$"Actual GlutenFree should have been {expectedPizza.IsGlutenFree} but it was {response.Data.IsGlutenFree}");
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
    }
}
