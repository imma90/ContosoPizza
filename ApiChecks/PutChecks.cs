using NUnit.Framework;
using RestSharp;
using ContosoPizza.Models;
using System.Net;
using System.Collections;

namespace ApiChecks
{
    [TestFixture]
    public class PutChecksClass: ApiChecksBase
    {
        private Pizza testPizza;

        [SetUp]
        public async Task TestDataSetup()
        {
            Pizza pizza = Helpers.CreatePizza(name:$"Check PUT pizza {DateTimeOffset.UtcNow.Ticks}");
            Console.WriteLine($"Created pizza {pizza.Name} and id {pizza.Id}");
            // Act
            RestResponse<Pizza> response = await _client.ExecutePostAsync<Pizza>(Helpers.PostPizzaRequest(pizza));

            testPizza = response.Data;
        }

        [TearDown]
        public async Task TestDataCleanup()
        {
            RestResponse response = await _client.DeleteAsync(Helpers.DeletePizzaRequest(testPizza.Id));
        }
        
        [Test]
        public async Task VerifyPutWithValidIdReturns204()
        {
            // Arrange
            var pizzaData = Helpers.CreatePizza("PUT valid pizza");
            pizzaData.Id = testPizza.Id;

            // Act
            RestResponse response = await _client.ExecutePutAsync(Helpers.PutPizzaRequest(testPizza.Id, pizzaData));
            
            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, $"PUT pizza {testPizza.Id} should have returned a No Content response; instead it returned {response.StatusCode}");

        }

        [Test]
        public async Task VerifyPutChangesPizza()
        {
            // Arrange
            var expectedName = "PUT valid and glutenfree pizza";
            var pizzaData = Helpers.CreatePizza(expectedName, true);
            pizzaData.Id = testPizza.Id;

            // Act
            RestResponse responsePUT = await _client.ExecutePutAsync(Helpers.PutPizzaRequest(testPizza.Id, pizzaData));
            RestResponse<Pizza> responseGET = await _client.ExecuteGetAsync<Pizza>(Helpers.GetSinglePizzaRequest(testPizza.Id));
            
            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, responsePUT.StatusCode, $"PUT pizza {testPizza.Id} should have returned a No Content response; instead it returned {responsePUT.StatusCode}");
            Assert.AreEqual(HttpStatusCode.OK, responseGET.StatusCode, $"PUT pizza {testPizza.Id} should have returned an OK response; instead it returned {responsePUT.StatusCode}");
            StringAssert.AreEqualIgnoringCase(expectedName, responseGET.Data.Name,$"Name of the pizza should have been '{expectedName}' but it was {responseGET.Data.Name}");
            Assert.AreEqual(true, responseGET.Data.IsGlutenFree,$"IsGlutenFree of the pizza should have been true but it was {responseGET.Data.IsGlutenFree}");
        }

        [Test]
        public async Task VerifyPutWithInvalidIdReturns400()
        {
            // Arrange
            var pizzaData = Helpers.CreatePizza("PUT pizza with invalid ID in body");
            pizzaData.Id = 3098204;
            
            // Act
            RestResponse responsePUT = await _client.ExecutePutAsync(Helpers.PutPizzaRequest(testPizza.Id, pizzaData));
            RestResponse<Pizza> responseGET = await _client.ExecuteGetAsync<Pizza>(Helpers.GetSinglePizzaRequest(testPizza.Id));

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, responsePUT.StatusCode, $"PUT pizza {testPizza.Id} should have returned a Bad Request response; instead it returned {responsePUT.StatusCode}");
            StringAssert.AreEqualIgnoringCase(testPizza.Name, responseGET.Data.Name,$"Name of the pizza should have stayed '{testPizza.Name}' but it was {responseGET.Data.Name}");
        }

        [Test]
        public async Task VerifyPutWithNonexistentIdReturns404()
        {
            // Arrange
            var expectedName = "PUT with invalid endpoint id";
            var pizzaData = Helpers.CreatePizza(expectedName, true);
            pizzaData.Id = testPizza.Id;

            // Act
            RestResponse responseDelete = await _client.DeleteAsync(Helpers.DeletePizzaRequest(testPizza.Id));
            RestResponse responsePUT = await _client.ExecutePutAsync(Helpers.PutPizzaRequest(testPizza.Id, pizzaData));

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, responsePUT.StatusCode, $"PUT pizza {testPizza.Id} should have returned a NotFound response; instead it returned {responsePUT.StatusCode}");

        }
    }

}