using System;
using System.Collections.Generic;
using static Ex03.GarageLogic.EnergySource.GasBasedEnergy;
using static Ex03.GarageLogic.EnergySource;

namespace Ex03.GarageLogic
{
    public static class VehicleFactory
    {
        public enum eSupportedVehicles
        {
            RegularMotorcycle = 1,
            ElectricMotorcycle = 2,
            RegularCar = 3,
            ElectricCar = 4,
            RegularTruck = 5
        }

        public static Vehicle CreateVehicle(string i_LicenseNumber, eSupportedVehicles i_VehicleType)
        {
            Vehicle vehicle;

            switch (i_VehicleType)
            {
                case eSupportedVehicles.RegularMotorcycle:
                    vehicle = new Motorcycle(i_LicenseNumber, istantiateWheels(2, 31f),
                        istantiateEnergySource(eEnergyType.Gas, 6.4f, eGasType.Octan98));
                    break;
                case eSupportedVehicles.ElectricMotorcycle:
                    vehicle = new Motorcycle(i_LicenseNumber, istantiateWheels(2, 31f), 
                        istantiateEnergySource(eEnergyType.Electric, 2.6f));
                    break;
                case eSupportedVehicles.RegularCar:
                    vehicle = new Car(i_LicenseNumber, istantiateWheels(5, 33f), 
                        istantiateEnergySource(eEnergyType.Gas, 46f, eGasType.Octan95));
                    break;
                case eSupportedVehicles.ElectricCar:
                    vehicle = new Car(i_LicenseNumber, istantiateWheels(5, 33f), 
                        istantiateEnergySource(eEnergyType.Electric, 5.2f));
                    break;
                case eSupportedVehicles.RegularTruck:
                    vehicle = new Truck(i_LicenseNumber, istantiateWheels(14, 26f), 
                        istantiateEnergySource(eEnergyType.Gas, 135f, eGasType.Soler));
                    break;
                default:
                    throw new ArgumentException("Vehicle type not in supported vehicles.");
            }

            return vehicle;
        }

        private static List<Wheel> istantiateWheels(int i_NumberOfWheels, float i_MaxAirPressure)
        {
            List<Wheel> wheels = new List<Wheel>(i_NumberOfWheels);

            for (int i = 0; i < i_NumberOfWheels; i++)
            {
                wheels.Add(new Wheel(i_MaxAirPressure));
            }

            return wheels;
        }

        private static EnergySource istantiateEnergySource(eEnergyType i_EnergyType,
            float i_MaxEnergyAmount, eGasType i_GasType = eGasType.None)
        {
            EnergySource energySource;

            if (i_EnergyType == eEnergyType.Gas)
            {
                if (i_GasType == eGasType.None)
                {
                    throw new ArgumentException("Gas based vehicles cannot have 'None' as a gas type.");
                }

                energySource = new GasBasedEnergy(i_GasType, i_MaxEnergyAmount);
            }
            else
            {
                energySource = new ElectricBasedEnergy(i_MaxEnergyAmount);
            }

            return energySource;
        }
    }
}
