using Crayon.ExchangeRates.Api.Controllers;
using Crayon.ExchangeRates.BusinessLogic.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Crayon.ExchangeRates.Api.Tests
{
    public class HistoricalExchangeRatesControllerTest {
        [Fact]
        public async Task Get_ReturnsOkResult_TestDataPassed()
        {
            // Arrange
            var baseCurr = "SEK";
            var targetCurr = "NOK";
            var dates = new DateTime[] { new DateTime(2020, 01, 10), new DateTime(2020, 01, 01) };

            var mockBL = new Mock<IHistoricalExchangeRatesBusinessLogic>();
            mockBL.Setup(bl => bl.CalculateHistoricalExchangeRatesAsync(baseCurr, targetCurr, dates))
                .ReturnsAsync(GetTestData());

            var mockLogger = new Mock<ILogger<HistoricalExchangeRatesController>>();

            var controller = new HistoricalExchangeRatesController(mockLogger.Object, mockBL.Object);

            // Act
            var result = await controller.Get(baseCurr, targetCurr, dates);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Get_ReturnsBadResult_GivenInvalidModel()
        {
            // Arrange
            var mockBL = new Mock<IHistoricalExchangeRatesBusinessLogic>();
            mockBL.Setup(bl => bl.CalculateHistoricalExchangeRatesAsync(string.Empty, string.Empty, null))
                .ReturnsAsync(GetTestData());

            var mockLogger = new Mock<ILogger<HistoricalExchangeRatesController>>();

            var controller = new HistoricalExchangeRatesController(mockLogger.Object, mockBL.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Get(string.Empty, string.Empty, null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        private static string GetTestData()
        {
            return "Test response";
        }
    }
}
