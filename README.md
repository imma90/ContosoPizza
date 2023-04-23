# ContosoPizza

This project follows tutorial [Create a web API with ASP.NET Core controllers](https://learn.microsoft.com/en-us/training/modules/build-web-api-aspnet-core/). After completing the tutorial, I added:

- API Checks, in subfolders of `ApiChecks`. My code is based on the (IMO excellent) course "Automating API Checks With RestShahrp" by Hilary Weaver-Robb on the Ministry of Testing platform. Writig the API test code for this API was not very challenging - the course's example API and the Contoso Pizza API are very similar - but it helped me consolidate what I learned in the course. 

## Run

To run the API
```
dotnet run --project ContosoPizza/ContosoPizza.csproj 
```

## Testing

To run the tests:

```
dotnet test
```

I like more verbosity so that I can also see all passing tests:

```
dotnet test --logger:"console;verbosity=normal"
```

The test code is located in the `ApiChecks` directory. It covers the response code and output of all CRUD operations. 