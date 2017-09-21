using Enceladus.Api.Models.Bots;
using Enceladus.Api.Models.Prices.Intrinio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enceladus.Api.Helpers
{
    public class BotSimulator
    {
        private const string sellColor = "#f66733";
        private const string buyColor = "#522d80";
        private const string flatColor = "#d3d3d3";
        private const string blue = "#0075be";

        public async Task<BotModel> Simulate(BotModel bot)
        {
            bot.TradeBook = new TradeBook()
            {
                Id = 10,
                TradeEntries = new List<TradeEntry>(1000),
                Series = new List<SeriesItem>(1000)
            };
            // gather needed data to execute trades against
            var tickerDetails = await IntrinioService.GetTickerDetails(bot.TargetTicker.TickerSymbol);

            var prices = await IntrinioService.GetTickerPrices(bot.TargetTicker.TickerSymbol, bot.StartDate.AddDays(-1), DateTime.UtcNow.Date);

            var priceLookup = prices.ToDictionary(k => k.Date.ToString("yyyyMMddHHmmss"), m => new NormalizedPriceItem()
            {
                High = (int)(m.High * 100),
                Low = (int)(m.Low * 100),
                Open = (int)(m.Open * 100),
                Close = (int)(m.Close * 100),
                Date = DateTime.SpecifyKind(m.Date, DateTimeKind.Utc)
            });

            // register data with the model so it can perform its analysis

            bot.Model.RegisterInputSources(IntrinioService.GetTickerPrices, bot.StartDate, DateTime.UtcNow);

            // run get signal over a time series of the target equity
            var idxDate = bot.StartDate;
            var stopDate = DateTime.UtcNow;
            var idxDateString = idxDate.ToString("yyyyMMddHHmmss");
            // Take the first time step and use it to setup initial portfolio
            var currPrice = priceLookup[idxDateString];
            var prevItem = new SeriesItem()
            {
                Signal = Signal.Hold,
                Date = idxDate,
                Open = currPrice.Open,
                High = currPrice.High,
                Low = currPrice.Low,
                Close = currPrice.Close,
                PortfolioValue = currPrice.Close,
                Position = 0
            };
            bot.TradeBook.Series.Add(prevItem);
            idxDate = idxDate.AddDays(1);
            idxDateString = idxDate.ToString("yyyyMMddHHmmss");
            while (idxDate < stopDate)
            {
                // make sure the date is included in our data set coming from the data provider (ie intrinio)
                // (ie skip weekends)
                if (priceLookup.ContainsKey(idxDateString))
                {
                    // set up calculations for this time-step
                    var (newSignal, multiplier) = await bot.Model.GetSignal(idxDate);

                    var tradeEntry = new TradeEntry()
                    {
                        Type = "pin",
                        RollOverColor = blue
                    };
                    currPrice = priceLookup[idxDateString];

                    var seriesItem = new SeriesItem()
                    {
                        Date = idxDate,
                        Open = currPrice.Open,
                        High = currPrice.High,
                        Low = currPrice.Low,
                        Close = currPrice.Close,
                        Signal = newSignal,
                    };

                    ///
                    /// ====================================================================
                    /// Determine if we want to alter our position based on the model signal
                    /// ====================================================================
                    /// 
                    var positionChange = 0;
                    if (prevItem.Signal != newSignal)
                    {

                        switch (newSignal)
                        {
                            case Signal.Buy:

                                // if prev position is a short we want to reverse it
                                positionChange = (int)Math.Round(bot.OrderAmount * multiplier) - prevItem.Position;

                                tradeEntry.Position = TradeType.Buy;
                                tradeEntry.Text = "B";
                                tradeEntry.Date = idxDate;
                                tradeEntry.Price = seriesItem.Close;
                                seriesItem.Position = positionChange + prevItem.Position;
                                tradeEntry.Description = Math.Abs(positionChange) + " share(s) purchased @ $" + seriesItem.Close / 100.0;
                                tradeEntry.BackgroundColor = buyColor;
                                break;
                            case Signal.Sell:
                                // if long the prev position we want to reverse it
                                positionChange = -1 * (int)Math.Round(bot.OrderAmount * multiplier) - prevItem.Position;
                                tradeEntry.Position = TradeType.Sell;
                                tradeEntry.Text = "S";
                                tradeEntry.Date = idxDate;
                                tradeEntry.Price = seriesItem.Close;
                                seriesItem.Position = positionChange + prevItem.Position;
                                tradeEntry.Description = Math.Abs(positionChange) + " share(s) sold @ $" + seriesItem.Close / 100.0;
                                tradeEntry.BackgroundColor = sellColor;
                                break;
                            case Signal.Hold:
                                // if long the prev position we want to reverse it
                                positionChange = -1 * prevItem.Position;

                                tradeEntry.Text = "H";
                                tradeEntry.Date = idxDate;
                                tradeEntry.Price = seriesItem.Close;
                                seriesItem.Position = positionChange + prevItem.Position;
                                if (positionChange > 0)
                                {
                                    tradeEntry.Description = Math.Abs(positionChange) + " share(s) bought @ $" + seriesItem.Close / 100.0 + " to flatten position.";
                                    tradeEntry.Position = TradeType.Buy;
                                }
                                else
                                {
                                    tradeEntry.Description = Math.Abs(positionChange) + " share(s) sold @ $" + seriesItem.Close / 100.0 + " to flatten position.";
                                    tradeEntry.Position = TradeType.Sell;
                                }
                                tradeEntry.BackgroundColor = "#0075be";
                                break;
                            default:
                                throw new Exception("Unhandled signal '" + newSignal.ToString() + "'!!!");
                        }
                        bot.TradeBook.TradeEntries.Add(tradeEntry);
                    }
                    else
                    {
                        seriesItem.Position = prevItem.Position;
                    }
                    /// 
                    /// ====================================================================
                    /// 

                    ///                
                    /// ====================================================================
                    /// Calculate portfolio value
                    ///     Use the previous indexes signal value / position and the current closed value 
                    ///     to determine how the porfolio changed
                    /// ====================================================================
                    /// 
                    if (prevItem.Position != 0)
                    {
                        seriesItem.PortfolioValue = prevItem.Position * (seriesItem.Close - prevItem.Close) + prevItem.PortfolioValue;
                    }
                    else if (prevItem.Position < 0)
                    {
                        seriesItem.PortfolioValue = prevItem.Position * (seriesItem.Close - prevItem.Close) + prevItem.PortfolioValue;
                    }
                    else
                    {
                        seriesItem.PortfolioValue = prevItem.PortfolioValue;
                    }

                    /// 
                    /// ====================================================================
                    /// 

                    prevItem = seriesItem;
                    bot.TradeBook.Series.Add(seriesItem);
                }
                idxDate = idxDate.AddDays(1);
                idxDateString = idxDate.ToString("yyyyMMddHHmmss");
            }

            /// 
            /// If we have an open position then lets go ahead and close it out
            /// 

            if (prevItem.Position != 0)
            {
                var closeOutTradeEntry = new TradeEntry();
                var positionChange = -1 * prevItem.Position;

                closeOutTradeEntry.Text = "H";
                closeOutTradeEntry.Date = prevItem.Date;
                closeOutTradeEntry.Price = prevItem.Close;
                prevItem.Position = positionChange + prevItem.Position;
                if (positionChange > 0)
                    closeOutTradeEntry.Description = Math.Abs(positionChange) + " share(s) bought @ $" + prevItem.Close / 100.0 + " to flatten position.";
                else
                    closeOutTradeEntry.Description = Math.Abs(positionChange) + " share(s) sold @ $" + prevItem.Close / 100.0 + " to flatten position.";
                closeOutTradeEntry.BackgroundColor = "#0075be";
                bot.TradeBook.TradeEntries.Add(closeOutTradeEntry);
            }


            /// 
            /// ====================================================================
            /// Calculate stats
            /// ====================================================================
            /// 

            var stats = new TradingStats();

            var startingValue = bot.TradeBook.Series.First().PortfolioValue;
            var endingValue = bot.TradeBook.Series.Last().PortfolioValue;
            stats.TotalPnL = endingValue - startingValue;
            var tradeProfits = new List<int>();
            for (int idx = 0; idx < bot.TradeBook.TradeEntries.Count - 1; idx += 2)
            {
                var entryTrade = bot.TradeBook.TradeEntries[idx];
                var exitTrade = bot.TradeBook.TradeEntries[idx + 1];


                //think more about this need to get true profit
                if (entryTrade.Position == TradeType.Buy)
                    tradeProfits.Add(exitTrade.Price - entryTrade.Price);
                else
                    tradeProfits.Add(entryTrade.Price - exitTrade.Price);


            }
            stats.WinPct = 100 * tradeProfits.Count(k => k > 0) / tradeProfits.Count();

            bot.TradeBook.TradingStats = stats;
            bot.Model.Reset();
            return bot;
        }
    }
}
