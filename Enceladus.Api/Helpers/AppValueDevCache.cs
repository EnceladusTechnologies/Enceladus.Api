using Enceladus.Api.Models.Bots;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enceladus.Api.Helpers
{

    public static class AppValueDevCache
    {
        private static MemoryCache memCache = new MemoryCache(new MemoryCacheOptions());
        private static int CacheLifetimeMins = 600;

        internal static ICollection<BotModel> GetBots(string userId)
        {
            var key = userId + ":bots";
            var x = memCache.Get(key);
            if (x != null)
            {
                return (ICollection<BotModel>)x;
            }
            else
            {
                ICollection<BotModel> items;
                items = GetBotsInMemory();
                memCache.Set(key, items, DateTime.Now.AddMinutes(CacheLifetimeMins));
                return items;
            }
        }

        private static ICollection<BotModel> GetBotsInMemory()
        {
            return new List<BotModel>() {
                new BotModel()
                {
                    Id = 1,
                    Name = "Buy and Hold Bot",
                    Description = "A test bot to implement the buy and hold strategy",
                    Author = "Joe Jordan",
                    OrderAmount = 1,
                    OrderType = OrderType.MarketOrder,
                    StartDate = DateTime.UtcNow.Date.AddDays(-365),
                    TrailingStopExit = new TrailingStopExit()
                    {
                        Id = 88,
                        Amount = 5
                    },
                    TakeProfitExit = new TakeProfitExit()
                    {
                        Id = 32,
                        Amount = 3
                    },
                    InitialStopExit = new InitialStopExit()
                    {
                        Id = 82,
                        Amount = 1
                    },
                    TargetTicker = new TargetTicker()
                    {
                        TickerSymbol = "AAPL",
                        ExchangeSymbol = "NASDAQ"
                    },
                    StartingBank = 1000,
                    Model = new BuyAndHoldModel()
                    {
                        TradingModelInputs = new List<TradingModelInput>()
                        {
                            new TradingModelInput()
                            {
                                Id = 1,
                                TickerSymbol = "AAPL",
                                ExchangeSymbol = "NASDAQ",
                                IntervalType = IntervalType.Daily
                            }
                        }
                    }
                },
                new BotModel()
                {
                    Id = 2,
                    Name = "Random Signal Bot",
                    Description = "A bot using a model which creates a random Buy / Sell / Hold signal",
                    Author = "Joe Jordan",
                    OrderAmount = 1,
                    OrderType = OrderType.MarketOrder,
                    StartDate = DateTime.UtcNow.Date.AddDays(-365),
                    TargetTicker = new TargetTicker()
                    {
                        TickerSymbol = "IBM",
                        ExchangeSymbol = "NASDAQ"
                    },
                    StartingBank = 1000,
                    Model = new SellAndHoldModel()
                    {
                        TradingModelInputs = new List<TradingModelInput>()
                        {
                            new TradingModelInput()
                            {
                                Id = 1,
                                TickerSymbol = "IBM",
                                ExchangeSymbol = "NASDAQ",
                                IntervalType = IntervalType.Daily
                            }
                        }
                    }
                }
            };
        }
    }
}
