using System;

namespace Ex03.GarageLogic
{
    public abstract class EnergySource
    {
        public enum eEnergyType
        {
            Gas,
            Electric
        }

        public class GasBasedEnergy : EnergySource
        {
            private readonly eGasType r_GasType;
            private readonly float r_MaxGasAmount;
            private float m_CurrentGaslAmount;
            
            public enum eGasType
            {
                Octan98 = 1,
                Octan96 = 2,
                Octan95 = 3,
                Soler = 4,
                None
            }

            public GasBasedEnergy(eGasType i_FuelType, float i_MaxFuelCapacity)
            {
                r_GasType = i_FuelType;
                r_MaxGasAmount = i_MaxFuelCapacity;
            }

            public eGasType GasType
            {
                get { return r_GasType; }
            }

            public float MaxGasAmount
            {
                get { return r_MaxGasAmount; }
            }

            public float CurrentGasAmount
            {
                get { return m_CurrentGaslAmount; }
                set { m_CurrentGaslAmount = value; }
            }

            internal void AddGas(float i_LitersToAdd, eGasType i_GasType) 
            {
                if (i_GasType != r_GasType)
                {
                    throw new ArgumentException("Error: Gas type is incorrect");
                }

                if (m_CurrentGaslAmount + i_LitersToAdd > r_MaxGasAmount || i_LitersToAdd < 0)
                {
                    throw new ValueOutOfRangeException(0, r_MaxGasAmount - m_CurrentGaslAmount);
                }

                m_CurrentGaslAmount += i_LitersToAdd;
            }
        }

        public class ElectricBasedEnergy : EnergySource
        {
            private readonly float r_MaximumBatteryTime;
            private float m_CurrentBatteryTime;
            
            public ElectricBasedEnergy(float i_MaximumBatteryTime)
            {
                r_MaximumBatteryTime = i_MaximumBatteryTime;
            }

            public float MaxBatteryTime
            {
                get { return r_MaximumBatteryTime; }
            }

            public float CurrentBatteryTime
            {
                get { return m_CurrentBatteryTime; }
                set { m_CurrentBatteryTime = value; }
            }

            internal void AddCharge(float i_HoursToAdd)
            {
                if (m_CurrentBatteryTime + i_HoursToAdd > r_MaximumBatteryTime || i_HoursToAdd < 0)
                {
                    throw new ValueOutOfRangeException(0, r_MaximumBatteryTime - m_CurrentBatteryTime);
                }

                m_CurrentBatteryTime += i_HoursToAdd;
            }
        }
    }
}
