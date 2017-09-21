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
                Series = new List<SeriesItem>()
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

        public TrailingStopExit TrailingStopExit { get; set; }
        public TakeProfitExit TakeProfitExit { get; set; }
        public InitialStopExit InitialStopExit { get; set; }

        public DateTime StartDate { get; set; }
        public TradeBook TradeBook { get; set; }
        public BotResultViewModel ToViewModel()
        {
            var vm = new BotResultViewModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Author = this.Author,
                OrderAmount = this.OrderAmount,
                OrderType = this.OrderType.ToString(),
                TargetTicker = this.TargetTicker.TickerSymbol,
                StartDate = this.StartDate,
                StartingBank = this.StartingBank,
                PriceSlippagePct = this.PriceSlippagePct,
                CommissionPenaltyPerTrade = this.CommissionPenaltyPerTrade,
                ModelAuthor = this.Model.Author,
                ModelDescription = this.Model.Description,
                ModelName = this.Model.Name,
                ModelInputs = string.Join(", ", this.Model.TradingModelInputs.Select(k=>k.TickerSymbol)),
                TimeSlippageSecs = this.TimeSlippageSecs,
                TradeBook = this.TradeBook
            };
            return vm;
        }
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
    public class BotResultViewModel
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
        public string ModelAuthor { get; set; }
        public string ModelInputs { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string TargetTicker { get; set; }
        public string OrderType { get; set; }
        public long OrderAmount { get; set; }
        public double StartingBank { get; set; }

        public int TimeSlippageSecs { get; set; }
        public float PriceSlippagePct { get; set; }
        public double CommissionPenaltyPerTrade { get; set; }


        public DateTime StartDate { get; set; }
        public TradeBook TradeBook { get; set; }
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
        public List<TradeEntry> TradeEntries { get; set; }
        public List<SeriesItem> Series { get; set; }
        public TradingStats TradingStats { get; set; }
    }

    public class TradingStats
    {
        public int Return { get; set; }
        public int Drawdown { get; set; }
        public double SharpeRatio { get; set; }
        public double SterlingRatio { get; set; }
        public double TotalPnL { get; set; }
        public double WinPct { get; set; }

    }
    public class TradeEntry
    {
        public TradeEntry()
        {
            BackgroundAlpha = 1.0;
            BorderAlpha = 1.0;
            Color = "#FFFFFF";
            ShowOnAxis = false;
        }
        public double BackgroundAlpha { get; set; }
        public string BackgroundColor { get; set; }
        public double BorderAlpha { get; set; }
        public string BorderColor { get; set; }
        public string Color { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int Shares { get; set; }
        public TradeType Position { get; set; }
        public string Graph { get; set; }
        public string RollOverColor { get; set; }
        public bool ShowOnAxis { get; set; }    
        public string Text { get; set; }
        /// <summary>
        /// Type of bullet. Possible values are: 
        /// "flag", "sign", "pin", "triangleUp", "triangleDown", "triangleLeft", "triangleRight", "text", "arrowUp", "arrowDown"
        /// </summary>
        public string Type { get; set; }
        
        
        public string Description { get; set; }
    }
    public enum TradeType
    {
        Buy = 1,
        Sell = -1
    }
    public class SeriesItem
    {
        public DateTime Date { get; set; }
        public int Open { get; set; }
        public int High { get; set; }
        public int Low { get; set; }
        public int Close { get; set; }
        public Signal Signal { get; set; }
        public int PortfolioValue { get; set; }
        public int Position { get; set; }
    }
    public class NormalizedPriceItem
    {
        public DateTime Date { get; set; }
        public int Open { get; set; }
        public int High { get; set; }
        public int Low { get; set; }
        public int Close { get; set; }
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
