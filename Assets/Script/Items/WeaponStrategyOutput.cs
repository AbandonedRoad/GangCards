using Enum;
using Interfaces;

namespace Items
{
    public class WeaponStrategyOutput : IItemStrategyOutput
    {
        public string Message { get; private set; }

        public int Value { get; private set; }

        public DamageType DamageType { get; private set; }

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="message"></param>
        /// <param name="damageType"></param>
        /// <param name="value"></param>
        public WeaponStrategyOutput(string message, DamageType damageType, int value)
        {
            Message = message;
            Value = value;
            DamageType = damageType;
        }
    }
}
