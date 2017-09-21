using Enceladus.Api.Models.Prices.Intrinio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enceladus.Api.Models.Bots
{
    public abstract class ITradingModel
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Author { get; }
        public abstract Task<(Signal Signal, double Multiplier)> GetSignal(DateTime currentDateTime);
        public abstract ICollection<ConfigBase> ConfigurationQuestions { get; }
        public string ConfigurationQuestionAnswers { get; set; }
        public ICollection<TradingModelInput> TradingModelInputs { get; set; }
        internal Func<string, DateTime?, DateTime?, Task<ICollection<PriceItem>>> _dataSource;
        internal DateTime _startDate;
        internal DateTime _endDate;
        public abstract void Reset();
        public void RegisterInputSources(Func<string, DateTime?, DateTime?, Task<ICollection<PriceItem>>> dataSource, DateTime startDate, DateTime endDate)
        {
            this._startDate = startDate;
            this._endDate = endDate;
            this._dataSource = dataSource;
        }
    }
    public enum Signal
    {
        Buy = 1,
        Sell = -1,
        Hold = 0
    }
    public class TradingModelInput
    {
        public int Id { get; set; }
        public string TickerSymbol { get; set; }
        public string ExchangeSymbol { get; set; }
        public IntervalType IntervalType { get; set; }

    }


    public enum IntervalType
    {
        OneMinute = 1,
        FiveMinute = 2,
        ThirtyMinute = 3,
        OneHour = 4,
        Daily = 5,
        Weekly = 6,
        Monthly = 7,
        Quarterly = 8,
        Yearly = 9
    }
}
