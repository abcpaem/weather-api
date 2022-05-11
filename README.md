## The Weather API

### Introduction

Let's imagine a ship operating company having vessels traveling across the globe. The operators located in one of the headquarters are responsible for communication with vessel captains during their journey.  
To make their work easier they need to understand what is the local time at the ports where vessels stay as well as local weather conditions.

With this exercise, you will build an ASP.NET Core Web Api allowing the system to provide that information.

The produced solution should:
* build with `dotnet build` command,
* run with `dotnet run` command and start the local, console instance of the webapi,
* contain all the necessary tests (unit tests and integration tests) runnable with `dotnet test` command,
* have a documented api showing available operation and the requests/response models.

During writing the service, please include all the good practices that would be normally used during the development of the service.

For obtaining the weather/time information, please integrate with https://www.weatherapi.com/ using a free account.

For the implementation, please follow the tickets written below.  
For every ticket, please prepare a separate commit/pull-request showing incremental work.  
Assuming every ticket is implemented and released independently, the changes should be implemented in a non-breaking manner.

During the interview session, all of the tickets will be demoed, reviewed and discussed.

## Ticket-1

As an operator,  
I want to know what is the local time and temperature in the specified city,  
So that I can efficiently coordinate the vessel crew staying in the port.

Please design and create an endpoint returning the current weather conditions for the `city name` specified in the request, like: `Liverpool`, `Rotterdam` or `Busan, South Korea`.

The endpoint should use http://api.weatherapi.com/v1/current.json for obtaining the details and return response including the city name, region, country, local time and temperature in Celsius:

```
{
  "city": "Rotterdam",
  "region": "South Holland",
  "country": "Netherlands",
  "localTime": "2020-04-05 21:54",
  "temperature": 15.0
}
```

## **Implementation notes for Ticket-1**
- I left my weatherapi.com key in the appsettings file for 3 reasons: 
    1) I wanted to deliver a working solution.
    2) On the free account there is a limit of 1 million calls for month (more than enough).
    3) They won't charge me at all if my key runs out of service calls.
- I could have used AutoMapper for mapping the object received from weatherapi.com but I decided to do it manually to avoid overengineering at this stage, however I left a TODO comment because this approach is open for discussion, sometimes using AutoMapper gives you more headaches or adds more complexity, going against the KISS principle.
- In the Integration Tests I could have followed 2 approaches, either mocking the weatherapi.com call or use the live service for the tests, I decided to use the latter for a few reasons (also is open for discussion):
    1) Testing against an external API (either a live or sandbox endpoint) gives us more confidence that the whole integration is working fine.
    2) Testing against the real external API is very fast and having 1 million calls per month for free is more than enough for the tests.
    3) In real life we could have created a testing account only for the tests or ask if they can provide a sanbox that we could use to run the tests.
    4) If this becomes an issue at some point (for example if the machine running the tests doesn't have connection to internet), mocking the weatherapi.com call is not a big deal.
- The structure of the project is also open for discussion, I used the traditional approach due to the size and purpose of this project but in the last project that I worked, we structured the project in feature slices because that fits better in the microservices world.


## Ticket-2

As an operator,  
I want to be able to specify Celsius or Fahrenheit for temperature measurement,  
So that I can communicate more efficiently with the vessel crew.

Assuming that Ticket-1 is implemented and released to production, please extend the `existing endpoint` with an option to return the temperature in Celsius or Fahrenheit.

## **Implementation notes for Ticket-2**
- In order to test if the api is returning the temperature in both scales properly, an integration test was added where we request both temperatures for the same city, then we calculate the conversion from one scale to the other and if both values are the same this means that both temperatures are in the right scale.
- Some refactoring could be done in the WeatherControllerTests in order to avoid code duplication, but it is still manegeable at this point, ast there are only 2 tests.
    

## Ticket-3

As an operator,  
I want to know what are the times of sunrise and sunset,  
So that I can better plan and coordinate the operations on the vessel.

Assuming previous changes are implemented and released to production, please extend the `existing endpoint` with information of sunrise and sunset using the http://api.weatherapi.com/v1/astronomy.json api call.

## **Implementation notes for Ticket-3**
- During testing I just realised that there are many cities with duplicated names in the world, for example Guadalajara, there is one in México and other in Spain, the api always returns weather data for the city in Mexico, so as a user there is no way for me to know the weather conditions of Guadalajara in Spain. This would be something to think about for a future requirement. As you just noticed, when I'm developing and testing I see things that others cannot see or imagine, hehe :P
- I made private the method for getting the astronomy for now because this is only used internally by the WeatherService.
- This requirement adds 2 more properties to the currentWeather object, but we are still mapping values to the same object, so no need to use AutoMapper yet.


## Ticket-4 (optional to verify the front-end skills)

As a operator,
I want to see the current weather details for typed in the city name,
So that I can better handle the various vessel crews.

As a part of this ticket, please create a simple react page which allows to display the weather details for the typed in city. Using typescript is highly appreciated.

## API Documentation
You can use Swagger to see the api documentation, just run the api and then go to https://localhost:5001/swagger
