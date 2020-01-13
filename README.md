# Exchange Rates service

#### The service will use a free API provided by the ECB as its data source.
This API is documented at https://exchangeratesapi.io/ .
The service you implement should expose an endpoint that accepts this input:
- A set of Dates
- A base currency
- A target currency

#### The service should return this information:
- The maximum exchange rate during the period
- The minimum exchange rate during the period
- The average exchange rate during the period

#### Example usage and return values
Given this input
- Dates: 2018-02-01, 2018-02-15, 2018-03-01
- Currency SEK->NOK
The service should return this information:
- A min rate of 0.9546869595 on 2018-03-01
- A max rate of 0.9815486993 on 2018-02-15
- An average rate of 0.970839476467

#### Instructions how to build and run the service:
- Install the latest available .NET Core 2.2 SDK/Runtime from https://dotnet.microsoft.com/download/dotnet-core/2.2.
- Get the source code from this repository.
- Open solution file - Crayon.ExchangeRates.sln.
- Restore NuGet Packages on the solution level.
- Do Clean/Build operations.
- Hit F5 (Run).
- In Swagger, select the endpoint GET /api/HistoricalExchangeRates.
- Press Try it out with values e.g. SEK NOK 2020-01-01 2019-01-01.
- Hit Execute.
- The result will output the following: *"A min rate of 0.9527362445 on 2018-02-28\r\nA max rate of 0.9852686831 on 2018-02-09\r\nAn average rate of 0.972288664514286"*
