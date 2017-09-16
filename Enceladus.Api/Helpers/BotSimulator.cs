using Enceladus.Api.Models.Bots;
using Enceladus.Api.Models.Prices.Intrinio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enceladus.Api.Helpers
{
    public static class BotSimulator
    {
        public static async Task<TradeBook> Simulate(BotModel bot)
        {
            bot.TradeBook = new TradeBook()
            {
                Id = 10,
                TradeEntries = new List<TradeEntry>(1000),
                PortfolioSeries = new List<PortfolioValueEntry>(1000),
                TargetPriceSeries = new List<PriceEntry>(1000)
            };
            // gather needed data to execute trades against
            var tickerDetails = await IntrinioService.GetTickerDetails(bot.TargetTicker.TickerSymbol);

            var prices = await IntrinioService.GetTickerPrices(bot.TargetTicker.TickerSymbol, bot.StartDate.AddDays(-1), DateTime.UtcNow.Date);

            var priceLookup = prices.ToDictionary(k => k.Date.ToString("yyyyMMddHHmmss"), m => m);

            // register data with the model so it can perform its analysis

            bot.Model.RegisterInputSources(IntrinioService.GetTickerPrices, bot.StartDate, DateTime.UtcNow);


            // run get signal over a time series of the target equity
            var idxDate = bot.StartDate;
            var stopDate = DateTime.UtcNow;
            Signal prevSignal = Signal.Hold;
            while (idxDate < stopDate)
            {
                var idxDateString = idxDate.ToString("yyyyMMddHHmmss");
                var (newSignal, multiplier) = await bot.Model.GetSignal(idxDate);

                var tradeEntry = new TradeEntry();
                var priceEntry = new PriceEntry();
                var portfolioValueEntry = new PortfolioValueEntry();


                if (priceLookup.ContainsKey(idxDateString) && prevSignal != newSignal)
                {
                    switch (newSignal)
                    {
                        case Signal.Buy:
                            entry.HoldingsChange = (long)Math.Round(bot.OrderAmount * multiplier);
                            entry.Price = priceLookup[idxDateString].Open;
                            entry.PriceTimeStamp = idxDate;
                            break;
                        case Signal.Sell:
                            entry.HoldingsChange = -1 * (long)Math.Round(bot.OrderAmount * multiplier);
                            entry.Price = priceLookup[idxDateString].Open;
                            entry.PriceTimeStamp = idxDate;
                            break;
                        case Signal.Hold:
                            entry.HoldingsChange = 0;
                            entry.Price = 0;
                            entry.PriceTimeStamp = idxDate;
                            break;
                        default:
                            throw new Exception("Unhandled signal '" + newSignal.ToString() + "'!!!");
                    }
                    prevSignal = newSignal;
                }
                else
                {
                    entry.HoldingsChange = 0;
                    entry.Price = 0;
                    entry.PriceTimeStamp = idxDate;
                }
                bot.TradeBook.Entries.Add(entry);
                idxDate = idxDate.AddDays(1);
            }
            return bot.TradeBook;
        }
    }
}
