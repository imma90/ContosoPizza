using RestSharp;
using ContosoPizza.Models;

namespace ApiChecks
{
    public static class Helpers
    {
        public static RestRequest GetAllPizzasRequest()
        {
            return new RestRequest();
        }

        public static RestRequest GetSinglePizzaRequest(int id)
        {
            var request = new RestRequest($"{id}");
            request.AddUrlSegment("id", id);
            return request;
        }

        public static RestRequest PostPizzaRequest(Pizza item)
        {
            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(item);
            return request;
        }

        public static RestRequest PutPizzaRequest(int id, Pizza item)
        {
            var request = new RestRequest($"{id}");
            request.AddUrlSegment("id", 1);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(item);
            return request;
        }

        public static RestRequest DeletePizzaRequest(int id)
        {
            var request = new RestRequest($"{id}");
            request.AddUrlSegment("id",id);
            return request;
        }

        public static Pizza CreatePizza(string name = "veggie pizza", bool isGlutenFree = false)
        {            
            return new Pizza
            {
                Name = name,
                IsGlutenFree = isGlutenFree
            };
        }
    }
}