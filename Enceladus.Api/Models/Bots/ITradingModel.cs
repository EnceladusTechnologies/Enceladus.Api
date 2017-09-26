using Enceladus.Api.Models.Prices.Intrinio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enceladus.Api.Models.Bots
{
    public class TradingModelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string TradingModelInputs { get; set; }
        public ICollection<ConfigBase> ConfigurationQuestions { get; set; }

    }

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
        public TradingModelViewModel ToViewModel()
        {
            var vm = new TradingModelViewModel()
            {
                Id = this.Id,
                Name = this.Name,
                Author = this.Author,
                Description = this.Description,
                TradingModelInputs = string.Join(", ", this.TradingModelInputs.Select(k => k.TickerSymbol + " (" + k.IntervalType.ToString() + ")")),
                ConfigurationQuestions = this.ConfigurationQuestions
            };
            return vm;
        }
        public ModelListItemViewModel ToListItemViewModel()
        {
            var vm = new ModelListItemViewModel()
            {
                Id = this.Id,
                Name = this.Name,
                Author = this.Author,
                Description = this.Description,
                ModelInputs = string.Join(", ", this.TradingModelInputs.Select(k => k.TickerSymbol))
            };
            return vm;
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

    public class ModelListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ModelInputs { get; set; }
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
