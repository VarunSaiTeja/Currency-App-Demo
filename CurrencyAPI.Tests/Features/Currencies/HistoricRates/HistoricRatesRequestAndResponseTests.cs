using System;
using System.Collections.Generic;
using CurrencyApp.Application.Features.Currencies.HistoricRates;
using Xunit;

namespace CurrencyAPI.Tests.Features.Currencies.HistoricRates
{
    public class HistoricRatesRequestAndResponseTests
    {
        [Fact]
        public void GetFormattedStartDate_Returns_Correct_Format()
        {
            var request = new HistoricRatesRequest
            {
                StartDate = new DateTime(2024, 5, 5)
            };
            Assert.Equal("2024-05-05", request.GetFormattedStartDate());
        }

        [Fact]
        public void GetFormattedEndDate_Returns_Correct_Format()
        {
            var request = new HistoricRatesRequest
            {
                EndDate = new DateTime(2024, 12, 31)
            };
            Assert.Equal("2024-12-31", request.GetFormattedEndDate());
        }
    }
}
