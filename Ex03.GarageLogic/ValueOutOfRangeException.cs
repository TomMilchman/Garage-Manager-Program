using System;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        public float MaxValue { get; }
        public float MinValue { get; }

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue)
        {
            MinValue = i_MinValue;
            MaxValue = i_MaxValue;
        }

        public override string Message
        {
            get
            {
                return $"Error: Inserted value is out of range: allowed range: {MinValue}-{MaxValue}";
            }
        }
    }
}
