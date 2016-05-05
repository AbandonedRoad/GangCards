using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    public interface IItemStrategyOutput
    {
        /// <summary>
        /// Message e.g. shown in the combat log.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// A value, e.g. damage value
        /// </summary>
        int Value { get; }
    }
}
