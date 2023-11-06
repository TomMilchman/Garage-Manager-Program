using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        protected readonly string r_LicenseNumber;
        protected string m_ModelName;
        protected readonly EnergySource r_EnergySource;
        protected readonly List<Wheel> r_Wheels;
        private string m_OwnerName;
        private string m_OwnerPhoneNumber;
        private eVehicleStatus m_VechicleStatus;

        public enum eVehicleStatus
        {
            InRepair = 1,
            Repaired = 2,
            PaidFor = 3
        }

        public Vehicle(string i_LicenseNumber ,List<Wheel> i_Wheels, EnergySource i_EnergySource)
        {
            r_LicenseNumber = i_LicenseNumber;
            r_Wheels = i_Wheels;
            r_EnergySource = i_EnergySource;
        }

        public string LicenseNumber
        {
            get { return r_LicenseNumber; }
        }

        public string ModelName
        {
            get { return m_ModelName; }
            set { m_ModelName = value; }
        }

        public EnergySource EnergySource
        {
            get { return r_EnergySource; }
        }

        public float EnergyLeftPercentage
        {
            get {
                float energyLeft;

                if (r_EnergySource is EnergySource.ElectricBasedEnergy electricEnergy)
                {
                    energyLeft = electricEnergy.CurrentBatteryTime / electricEnergy.MaxBatteryTime;
                }
                else if (r_EnergySource is EnergySource.GasBasedEnergy gasEnergy)
                {
                    energyLeft = gasEnergy.CurrentGasAmount / gasEnergy.MaxGasAmount;
                }
                else
                {
                    throw new ArgumentException("Energy source isn't either gas or electric.");
                }
                
                return energyLeft;
            }
        }

        public List<Wheel> Wheels
        {
            get { return r_Wheels; }
        }

        public string OwnerName
        {
            get { return m_OwnerName; }
            set { m_OwnerName = value; }
        }

        public string OwnerPhoneNumber
        {
            get { return m_OwnerPhoneNumber; }
            set { m_OwnerPhoneNumber = value; }
        }

        public eVehicleStatus VehicleStatus
        {
            get { return m_VechicleStatus; }
            set { m_VechicleStatus = value; }
        }
    }
}
