using System;
using System.Collections.Generic;
using static Ex03.GarageLogic.EnergySource;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        private readonly Dictionary<string, Vehicle> r_VehiclesInGarage;

        public Garage()
        {
            r_VehiclesInGarage = new Dictionary<string, Vehicle>();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            r_VehiclesInGarage[vehicle.LicenseNumber] = vehicle;
        }

        public bool IsVehicleInGarage(string i_LicenseNumber)
        {
            return r_VehiclesInGarage.ContainsKey(i_LicenseNumber);
        }

        public List<string> GetVehiclesLicenseNumbersByStatus(Vehicle.eVehicleStatus i_Status)
        {
            List<string> licenseNumbers = new List<string>();

            foreach (KeyValuePair<string, Vehicle> vehicle in r_VehiclesInGarage)
            {
                if (vehicle.Value.VehicleStatus == i_Status)
                {
                    licenseNumbers.Add(vehicle.Key);
                }
            }

            return licenseNumbers;
        }

        public void ChangeVehicleStatus(string i_LicenseNumber, Vehicle.eVehicleStatus i_NewStatus)
        {
            if (IsVehicleInGarage(i_LicenseNumber))
            {
                r_VehiclesInGarage[i_LicenseNumber].VehicleStatus = i_NewStatus;
            }
            else
            {
                throw new ArgumentException("Vehicle not found in the garage.");
            }
        }

        public void InflateVehicleWheelsToMax(string i_LicenseNumber)
        {
            if (IsVehicleInGarage(i_LicenseNumber))
            {
                List<Wheel> wheels = r_VehiclesInGarage[i_LicenseNumber].Wheels;

                foreach (Wheel wheel in wheels)
                {
                    wheel.InflateWheel(wheel.MaxAirPressure - wheel.CurrentAirPressure);
                }
            }
            else
            {
                throw new ArgumentException("Vehicle not found in the garage.");
            }
        }

        public void RefuelGasVehicle(string i_LicenseNumber, GasBasedEnergy.eGasType i_GasType, float i_AmountToAdd)
        {
            if (IsVehicleInGarage(i_LicenseNumber))
            {
                Vehicle vehicle = r_VehiclesInGarage[i_LicenseNumber];

                if (vehicle.EnergySource is GasBasedEnergy vehicleGasEnergy)
                {
                    vehicleGasEnergy.AddGas(i_AmountToAdd, i_GasType);
                }
                else
                {
                    throw new ArgumentException("Vehicle does not support gas-based energy");
                }
            }
            else
            {
                throw new ArgumentException("Vehicle not found in the garage");
            }
        }

        public void RechargeElectricVehicle(string i_LicenseNumber, float i_HoursToAdd)
        {
            if (IsVehicleInGarage(i_LicenseNumber))
            {
                Vehicle vehicle = r_VehiclesInGarage[i_LicenseNumber];

                if (vehicle.EnergySource is ElectricBasedEnergy vehicleElectricEnergy)
                {
                    vehicleElectricEnergy.AddCharge(i_HoursToAdd);
                }
                else
                {
                    throw new ArgumentException("Vehicle does not support electric-based energy.");
                }
            }
            else
            {
                throw new ArgumentException("Vehicle not found in the garage.");
            }
        }

        public Vehicle GetVehicleByLicenseNumber(string i_LicenseNumber)
        {
            if (IsVehicleInGarage(i_LicenseNumber))
            {
                return r_VehiclesInGarage[i_LicenseNumber];
            }
            else
            {
                throw new ArgumentException("Vehicle not found in the garage.");
            }
        }

        public static void UpdateCurrentWheelAirPressure(Vehicle vehicle, float i_CurrentAirPressure)
        {
            foreach (Wheel wheel in vehicle.Wheels)
            {
                if (i_CurrentAirPressure > wheel.MaxAirPressure
                || i_CurrentAirPressure < 0)
                {
                    throw new ValueOutOfRangeException(0, wheel.MaxAirPressure);
                }

                wheel.CurrentAirPressure = i_CurrentAirPressure;
            }
        }

        public static void UpdateCurrentEnergyAmount(Vehicle vehicle, float i_CurrentEnergyAmount)
        {
            if (vehicle.EnergySource is GasBasedEnergy gasEnergy)
            {
                if (i_CurrentEnergyAmount > gasEnergy.MaxGasAmount
                    || i_CurrentEnergyAmount < 0)
                {
                    throw new ValueOutOfRangeException(0, gasEnergy.MaxGasAmount);
                }

                gasEnergy.CurrentGasAmount = i_CurrentEnergyAmount;
            }
            else if (vehicle.EnergySource is ElectricBasedEnergy electricEnergy)
            {
                if (i_CurrentEnergyAmount > electricEnergy.MaxBatteryTime
                    || i_CurrentEnergyAmount < 0)
                {
                    throw new ValueOutOfRangeException(0, electricEnergy.MaxBatteryTime);
                }

                electricEnergy.CurrentBatteryTime = i_CurrentEnergyAmount;
            }
        }
    }
}
