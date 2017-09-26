using Enceladus.Api.Models.ModelConfigControls;
using Enceladus.Api.Models.Prices.Intrinio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enceladus.Api.Models.Bots
{
    public class BuyAndHoldModel : ITradingModel
    {
        public override int Id => 1;
        public override string Name => "Buy & Hold Model";
        public override string Description => "Simple model for a buy and hold strategy";
        public override string Author => "Joe Jordan";
        public override void Reset()
        {

        }
        public override ICollection<ConfigBase> ConfigurationQuestions => new List<ConfigBase>() {
            new Number()
            {
                Id = new Guid("{57d88e55-8c42-4cf3-bbfb-d1ef9d6cd33d}"),
                Label = "Test Number",
                Value = "200",
                Order = 1,
                Required = false
            },
            new Dropdown()
            {
                Id = new Guid("{81cf858d-8ff4-4f02-b48d-8b7f689bd5df}"),
                Label = "Test Number",
                Value = null,
                Order = 2,
                Required = true,
                Options = new List<ControlOption>()
                {
                    new ControlOption()
                    {
                        Key = new Guid("{4461705a-e152-47ec-879a-c070a93ebc3b}"),
                        Value = "Option 1"
                    },
                    new ControlOption()
                    {
                        Key = new Guid("{0d74ac47-145a-4bc8-be05-4c7d2881f334}"),
                        Value = "Option 2"
                    },
                    new ControlOption()
                    {
                        Key = new Guid("{f43ed952-8856-4429-8139-97f2ec872ff8}"),
                        Value = "Option 3"
                    },
                    new ControlOption()
                    {
                        Key = new Guid("{49a40635-04d3-4479-834b-dc811ac073b2}"),
                        Value = "Option 4"
                    }
                }
            },
            new Checkbox()
            {
                Id = new Guid("{fe8a0ccc-3deb-4aa3-b634-bd64b5764b98}"),
                Label = "Test Checkbox",
                Value = null,
                Order = 3,
                Required = false
            }
        };

        private Dictionary<string, List<PriceItem>> Data;
        public override async Task<(Signal Signal, double Multiplier)> GetSignal(DateTime currentDateTime)
        {
            // if this is first run then get data needed for the model
            // and process it accordingly
            if (Data == null)
            {
                this.Data = new Dictionary<string, List<PriceItem>>(this.TradingModelInputs.Count);
                foreach (var input in this.TradingModelInputs)
                {
                    var d = await _dataSource(input.TickerSymbol, this._startDate, this._endDate);
                    this.Data.Add(input.TickerSymbol, d.ToList());
                }
            }


            return (Signal.Buy, 1.0);
        }
    }
    public class RandomSignalModel : ITradingModel
    {
        public override int Id => 2;
        public override string Name => "Random Signal Model";
        public override string Description => "A model which generates a random Buy / Sell / Hold signal";
        public override string Author => "Joe Jordan";
        public override void Reset()
        {
            _signalCount = 0;
            _currentSignal = Signal.Hold;
        }
        private Dictionary<string, List<PriceItem>> Data;
        public override ICollection<ConfigBase> ConfigurationQuestions => new List<ConfigBase>() {
            new Number()
            {
                Id = new Guid("{a6b61827-0026-48bc-8d6a-b100d823c893}"),
                Label = "Test Number",
                Value = "300",
                Order = 1,
                Required = true,
                HelpText = "This is a helpful note about what this value is for."
            },
            new Dropdown()
            {
                Id = new Guid("{444bbc8c-8a03-4c47-8f4f-e2a353ad5a46}"),
                Label = "Test Number",
                Value = null,
                Order = 2,
                Required = false,
                Options = new List<ControlOption>()
                {
                    new ControlOption()
                    {
                        Key = new Guid("{9655ab13-fd6b-4b4f-b1bf-aed9b247d54b}"),
                        Value = "Option 1"
                    },
                    new ControlOption()
                    {
                        Key = new Guid("{16237600-c362-403e-816b-200ebf61af9c}"),
                        Value = "Option 2"
                    },
                    new ControlOption()
                    {
                        Key = new Guid("{f2c07201-93f8-45f9-92d5-d029cd4eb880}"),
                        Value = "Option 3"
                    },
                    new ControlOption()
                    {
                        Key = new Guid("{91c78d12-c9a8-43e9-9b45-74a688d22440}"),
                        Value = "Option 4"
                    }
                }
            },
            new Checkbox()
            {
                Id = new Guid("{b5f924f2-b7dd-454b-8b8d-70fc60cf88ae}"),
                Label = "Test Checkbox",
                Value = null,
                Order = 3,
                Required = false
            }
        };

        private int _signalCount;
        public Signal _currentSignal { get; set; }
        private Random rand;
        public override async Task<(Signal Signal, double Multiplier)> GetSignal(DateTime currentDateTime)
        {
            // if this is first run then get data needed for the model
            // and process it accordingly
            if (Data == null)
            {
                rand = new Random(DateTime.Now.Millisecond);
                Data = new Dictionary<string, List<PriceItem>>(this.TradingModelInputs.Count);
                foreach (var input in this.TradingModelInputs)
                {
                    var d = await _dataSource(input.TickerSymbol, this._startDate, this._endDate);
                    Data.Add(input.TickerSymbol, d.ToList());
                }
            }
            _signalCount++;
            if (_signalCount % 5 == 0)
            {
                var rn = rand.Next(-1, 2);
                _currentSignal = (Signal)rn;
            }
            return (_currentSignal, 1.0);
        }
    }
}
