using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enceladus.Api.Models.Bots
{
    public interface IExitStrategy
    {
        int Id { get; set; }

    }

    public class TrailingStopExit : IExitStrategy
    {
        public int Id { get; set; }
        /// <summary>
        /// The absolute amount below the max gain for which the trade should be exited
        /// </summary>
        public int Amount { get; set; }
    }

    public class TakeProfitExit: IExitStrategy
    {
        public int Id { get; set; }
        /// <summary>
        /// The absolute amount of profit to take before exiting
        /// </summary>
        public int Amount { get; set; }
    }

    public class InitialStopExit: IExitStrategy
    {
        public int Id { get; set; }
        /// <summary>
        /// The initial amount willing to be lost relative to position before a trade is entered
        /// </summary>
        public int Amount { get; set; }
    }
    /// <summary>
    /// This exit strategy simply uses the model signal to determine whether a position should be
    /// closed or reversed
    /// </summary>
    public class ModelBasedExit: IExitStrategy
    {
        public int Id { get; set; }

    }
}
