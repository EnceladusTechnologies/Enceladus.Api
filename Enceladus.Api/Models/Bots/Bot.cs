using System;
using System.Collections.Generic;
using System.Linq;

namespace Enceladus.Api.Models.Bots
{
    public class BotModel
    {
        public BotModel()
        {
            this.TradeBook = new TradeBook()
            {
                Id = 10,
                TradeEntries = new List<TradeEntry>(),
                PortfolioSeries = new List<PortfolioValueEntry>(),
                TargetPriceSeries = new List<PriceEntry>()
            };
        }
        public int Id { get; set; }
        public ITradingModel Model { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public TargetTicker TargetTicker { get; set; }
        public OrderType OrderType { get; set; }
        public long OrderAmount { get; set; }
        public double StartingBank { get; set; }

        public int TimeSlippageSecs { get; set; }
        public float PriceSlippagePct { get; set; }
        public double CommissionPenaltyPerTrade { get; set; }


        public DateTime StartDate { get; set; }
        public TradeBook TradeBook { get; set; }
        public BotListItemViewModel ToListItemViewModel()
        {
            var vm = new BotListItemViewModel()
            {
                Id = this.Id,
                Name = this.Name,
                Author = this.Author,
                Description = this.Description,
                ModelName = this.Model.Name,
                OrderAmount = this.OrderAmount,
                
                OrderType = this.OrderType.ToString(),
                ModelInputs = string.Join(", ", this.Model.TradingModelInputs.Select(k => k.TickerSymbol))
            };
            return vm;
        }
    }
    public class BotListItemViewModel
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string OrderType { get; set; }
        public double OrderAmount { get; set; }
        public string ModelInputs { get; set; }
    }

    public class TargetTicker
    {
        public string ExchangeSymbol { get; set; }
        public string TickerSymbol { get; set; }
    }
    public class SignalHistoryItem
    {
        public DateTime DateTime { get; set; }
        public bool Signal { get; set; }
    }
    public class TradeBook
    {
        public int Id { get; set; }
        public ICollection<TradeEntry> TradeEntries { get; set; }
        public ICollection<PortfolioValueEntry> PortfolioSeries { get; set; }
        public ICollection<PriceEntry> TargetPriceSeries { get; set; }
    }
    public class TradeEntry
    {
        public DateTime TimeStamp { get; set; }
        public Signal Signal { get; set; }
        public string Label { get; set; }

    }
    public class PortfolioValueEntry
    {
        public DateTime TimeStamp { get; set; }
        public Signal Signal { get; set; }
        public long HoldingsChange { get; set; }
    }
    public class PriceEntry
    {
        public DateTime TimeStamp { get; set; }
        public double Price { get; set; }
    }
    public enum OrderType
    {
        MarketOrder = 0,
        LimitOrder = 1,
        StopLoss = 2,
        StopLimit = 3,
        TrailingStopLossPct = 4,
        TrailingStopLossAmt = 5,
        TrailingStopLimitPct = 6,
        TrailingStopLimitAmt = 7
    }
}
