using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Ex03.GarageLogic;
using static Ex03.GarageLogic.EnergySource;
using static Ex03.GarageLogic.EnergySource.GasBasedEnergy;

namespace Ex03.ConsoleUI
{
    public class ConsoleUI
    {
        private readonly Garage r_Garage;

        public ConsoleUI()
        {
            r_Garage = new Garage();
        }

        public enum MainMenuOption
        {
            AddVehicle = 1,
            DisplayLicenseNumbers = 2,
            ChangeVehicleStatus = 3,
            InflateWheels = 4,
            RefuelGasVehicle = 5,
            RechargeElectricVehicle = 6,
            DisplayVehicleDetails = 7,
            Exit = 8
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                printMainMenu();
                MainMenuOption selectedOption = getMainMenuOption();

                switch (selectedOption)
                {
                    case MainMenuOption.AddVehicle:
                        addVehicleToGarage();
                        break;
                    case MainMenuOption.DisplayLicenseNumbers:
                        displayVehiclesAccordingToFilter();
                        break;
                    case MainMenuOption.ChangeVehicleStatus:
                        changeVehicleStatus();
                        break;
                    case MainMenuOption.InflateWheels:
                        inflateVehicleWheels();
                        break;
                    case MainMenuOption.RefuelGasVehicle:
                        fuelGasVehicle();
                        break;
                    case MainMenuOption.RechargeElectricVehicle:
                        rechargeElectricVehicle();
                        break;
                    case MainMenuOption.DisplayVehicleDetails:
                        displayVehicleInformation();
                        break;
                    case MainMenuOption.Exit:
                        running = false;
                        break;
                    default:
                        Console.Write("Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void printMainMenu()
        {
            Console.WriteLine("Welcome to the Garage!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Add a vehicle to the garage");
            Console.WriteLine("2. Display list of vehicles in the garage");
            Console.WriteLine("3. Change a vehicle's status");
            Console.WriteLine("4. Inflate vehicle's wheels to maximum");
            Console.WriteLine("5. Refuel a gas-based vehicle");
            Console.WriteLine("6. Recharge an electric-based vehicle");
            Console.WriteLine("7. Display vehicle information");
            Console.WriteLine("8. Exit");
            Console.Write("Type here: ");
        }

        private MainMenuOption getMainMenuOption()
        {
            int option;

            while (!int.TryParse(Console.ReadLine(), out option) || !Enum.IsDefined(typeof(MainMenuOption), option))
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.Write("Type here: ");
            }

            return (MainMenuOption)option;
        }

        private void addVehicleToGarage()
        {
            string licenseNumber = getLicenseNumber();

            if (r_Garage.IsVehicleInGarage(licenseNumber))
            {
                Console.WriteLine($"Vehicle {licenseNumber} is already in the garage. Changing status to \"in repair\".");
                r_Garage.GetVehicleByLicenseNumber(licenseNumber).VehicleStatus = Vehicle.eVehicleStatus.InRepair;

                return;
            }

            VehicleFactory.eSupportedVehicles vehicleType = getVehicleTypeInput();
            Vehicle vehicle = VehicleFactory.CreateVehicle(licenseNumber, vehicleType);
            bool isValid = false; 

            while (!isValid)
            {
                try
                {
                    updateVehicleParameters(vehicle);
                    r_Garage.AddVehicle(vehicle);
                    isValid = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void updateVehicleParameters(Vehicle vehicle)
        {
            Console.WriteLine("Please provide the vehicle parameters:");
            Console.Write("Model Name: ");
            vehicle.ModelName = Console.ReadLine();
            Console.Write("Current Wheel Air Pressure: ");
            Garage.UpdateCurrentWheelAirPressure(vehicle ,float.Parse(Console.ReadLine())); 
            Console.Write("Wheel Manufacturer Name: ");
            string wheelManufacturerName = Console.ReadLine();

            if (!isCharacterString(wheelManufacturerName))
            {
                throw new FormatException("Error: Input must be composed of characters only.");
            }

            foreach (Wheel wheel in vehicle.Wheels)
            {
                wheel.ManufacturerName = wheelManufacturerName;
            }

            Console.Write("Current Energy Amount (Gas: Liters/ Electric: Hours): ");
            Garage.UpdateCurrentEnergyAmount(vehicle, float.Parse(Console.ReadLine()));
            Console.Write("Owner Name: ");
            string ownerName = Console.ReadLine();

            if (!isCharacterString(ownerName))
            {
                throw new FormatException("Error: Input must be composed of characters only.");
            }

            vehicle.OwnerName = ownerName;
            Console.Write("Owner Phone Number: ");
            string ownerPhoneNumber = Console.ReadLine();
            
            if (!isNumeric(ownerPhoneNumber))
            {
                throw new FormatException("Error: Input must be numeric.");
            }

            vehicle.OwnerPhoneNumber = ownerPhoneNumber;

            if (vehicle is Car car)
            {
                Console.WriteLine("Color (type the corresponding number):");
                Console.Write("White(1), Black(2), Yellow(3), Red(4): ");

                if (!int.TryParse(Console.ReadLine(), out int colorInput) || !Enum.IsDefined(typeof(Car.eColor), colorInput))
                {
                    throw new ValueOutOfRangeException(1, 4);
                }

                Car.eColor color = (Car.eColor)colorInput;
                car.Color = color;

                Console.Write("Number of Doors (2/3/4/5): ");

                if (!int.TryParse(Console.ReadLine(), out int numberOfDoorsInput) || !Enum.IsDefined(typeof(Car.eNumberOfDoors), numberOfDoorsInput))
                {
                    throw new ValueOutOfRangeException(2, 5);
                }

                Car.eNumberOfDoors numberOfDoors = (Car.eNumberOfDoors)numberOfDoorsInput;
                car.NumberOfDoors = numberOfDoors;
            }
            else if (vehicle is Motorcycle motorcycle)
            {
                Console.WriteLine("License Type (choose the corresponding number): ");
                Console.Write("A1 (1)/ A2 (2)/ AA (3) / B1 (4): ");

                if (!Enum.TryParse(Console.ReadLine(), out Motorcycle.eLicenseType licenseType) || !Enum.IsDefined(typeof(Motorcycle.eLicenseType), licenseType))
                {
                    throw new ValueOutOfRangeException(1, 4);
                }

                motorcycle.LicenseType = licenseType;

                Console.Write("Engine Capacity: ");

                if (!int.TryParse(Console.ReadLine(), out int engineCapacity))
                {
                    throw new FormatException("Error: Input must be an integer.");
                }

                motorcycle.EngineCapacity = engineCapacity;
            }
            else if (vehicle is Truck truck)
            {
                Console.Write("Is Carrying Hazardous Materials (true/false): ");

                if (!bool.TryParse(Console.ReadLine(), out bool isCarryingHazardous))
                {
                    throw new FormatException("Error: Input must be boolean.");
                }

                truck.IsCarryingHazardous = isCarryingHazardous;

                Console.Write("Trunk Volume: ");
                float trunkVolume = float.Parse(Console.ReadLine());

                truck.TrunkCapacity = trunkVolume;
            }

            vehicle.VehicleStatus = Vehicle.eVehicleStatus.InRepair;
            Console.WriteLine("All vehicle parameters updated successfully!");
        }

        private string getLicenseNumber()
        {
            string licenseNumber = null;

            while (string.IsNullOrEmpty(licenseNumber) || !isNumeric(licenseNumber))
            {
                Console.Write("Insert vehicle license number: ");
                licenseNumber = Console.ReadLine();

                if (!isNumeric(licenseNumber))
                {
                    Console.WriteLine("License number must be numeric. Try again.");
                }
            }

            return licenseNumber;
        }


        private bool isNumeric(string i_InputString)
        {
            bool isNumeric = true;

            foreach (char c in i_InputString)
            {
                if (!char.IsDigit(c))
                {
                    isNumeric = false;
                }
            }

            return isNumeric;
        }

        private bool isCharacterString(string i_InputString)
        {
            bool isCharacterString = true;

            foreach (char c in i_InputString)
            {
                if (!char.IsLetter(c))
                {
                    isCharacterString = false;
                }
            }

            return isCharacterString;
        }

        private VehicleFactory.eSupportedVehicles getVehicleTypeInput()
        {
            Console.WriteLine("Please select the type of vehicle:");
            Console.WriteLine("1. Regular Motorcycle");
            Console.WriteLine("2. Electric Motorcycle");
            Console.WriteLine("3. Regular Car");
            Console.WriteLine("4. Electric Car");
            Console.WriteLine("5. Regular Truck");
            Console.Write("Type here: ");

            int option;

            while (!int.TryParse(Console.ReadLine(), out option) || !Enum.IsDefined(typeof(VehicleFactory.eSupportedVehicles), option))
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.Write("Type here: ");
            }

            return (VehicleFactory.eSupportedVehicles)option;
        }

        private void displayVehiclesAccordingToFilter()
        {
            Vehicle.eVehicleStatus status = getVehicleStatusFilterInput();
            List<string> licenseNumbers = r_Garage.GetVehiclesLicenseNumbersByStatus(status);

            if (licenseNumbers.Count == 0)
            {
                Console.WriteLine("No vehicles found.");
            }
            else
            {
                Console.WriteLine("Vehicles in the garage:");

                foreach (string licenseNumber in licenseNumbers)
                {
                    Console.WriteLine(licenseNumber);
                }
            }
        }

        private Vehicle.eVehicleStatus getVehicleStatusFilterInput()
        {
            Console.WriteLine("Please select a vehicle status filter:");
            Console.WriteLine("1. In Repair");
            Console.WriteLine("2. Repaired");
            Console.WriteLine("3. Paid");
            Console.Write("Type here: ");

            int option;

            while (!int.TryParse(Console.ReadLine(), out option) || !Enum.IsDefined(typeof(Vehicle.eVehicleStatus), option))
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.Write("Type here: ");
            }

            return (Vehicle.eVehicleStatus)option;
        }

        private void changeVehicleStatus()
        {
            string licenseNumber = getLicenseNumber();

            if (r_Garage.IsVehicleInGarage(licenseNumber))
            {
                Vehicle.eVehicleStatus newStatus = getNewVehicleStatus();

                try
                {
                    r_Garage.ChangeVehicleStatus(licenseNumber, newStatus);
                    Console.WriteLine("Vehicle status changed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error changing vehicle status: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Vehicle not found in the garage.");
            }
        }

        private Vehicle.eVehicleStatus getNewVehicleStatus()
        {
            Console.WriteLine("Please select a new vehicle status:");
            Console.WriteLine("1. In Repair");
            Console.WriteLine("2. Repaired");
            Console.WriteLine("3. Paid");
            Console.Write("Type here: ");

            int option;

            while (!int.TryParse(Console.ReadLine(), out option) || !Enum.IsDefined(typeof(Vehicle.eVehicleStatus), option))
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.Write("Type here: ");
            }

            return (Vehicle.eVehicleStatus)option;
        }

        private void inflateVehicleWheels()
        {
            string licenseNumber = getLicenseNumber();

            if (r_Garage.IsVehicleInGarage(licenseNumber))
            {
                try
                {
                    r_Garage.InflateVehicleWheelsToMax(licenseNumber);
                    Console.WriteLine("Vehicle wheels inflated to maximum successfully.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error inflating vehicle wheels: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Vehicle not found in the garage.");
            }
        }

        private void fuelGasVehicle()
        {
            string licenseNumber = getLicenseNumber();

            if (r_Garage.IsVehicleInGarage(licenseNumber))
            {
                if (r_Garage.GetVehicleByLicenseNumber(licenseNumber).EnergySource is ElectricBasedEnergy)
                {
                    Console.WriteLine("Error: Vehicle does not support gas-based energy.");
                }
                else
                {
                    bool isValid = false;

                    while (!isValid)
                    {
                        try
                        {
                            eGasType gasType = getGasType();
                            Console.Write("Enter the amount of gas to add (in liters): ");
                            float fuelAmount = getEnergyAmount();
                            r_Garage.RefuelGasVehicle(licenseNumber, gasType, fuelAmount);
                            Console.WriteLine("Gas-based vehicle refueled successfully.");
                            isValid = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}, please try again.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Vehicle not found in the garage.");
            }
        }

        private eGasType getGasType()
        {
            Console.WriteLine("Please select a gas type:");
            Console.WriteLine("1. Octan98");
            Console.WriteLine("2. Octan96");
            Console.WriteLine("3. Octan95");
            Console.WriteLine("4. Soler");
            Console.Write("Type here: ");

            int option;

            while (!int.TryParse(Console.ReadLine(), out option) || !Enum.IsDefined(typeof(eGasType), option))
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.Write("Type here: ");
            }

            return (eGasType)option;
        }

        private float getEnergyAmount()
        {
            float energyAmount;

            while (!float.TryParse(Console.ReadLine(), out energyAmount))
            {
                Console.WriteLine("Invalid energy amount. Please try again.");
            }

            return energyAmount;
        }

        private void rechargeElectricVehicle()
        {
            string licenseNumber = getLicenseNumber();

            if (r_Garage.IsVehicleInGarage(licenseNumber))
            {
                if (r_Garage.GetVehicleByLicenseNumber(licenseNumber).EnergySource is GasBasedEnergy)
                {
                    Console.WriteLine("Error: Vehicle does not support electric-based energy.");
                }
                else
                {
                    bool isValid = false;

                    while (!isValid)
                    {
                        try
                        {
                            Console.Write("Enter the amount of battery charge to add (in hours): ");
                            float chargeAmount = getEnergyAmount();
                            r_Garage.RechargeElectricVehicle(licenseNumber, chargeAmount);
                            Console.WriteLine("Electric-based vehicle recharged successfully.");
                            isValid = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}, please try again.");
                        }
                    }
                }            
            }
            else
            {
                Console.WriteLine("Vehicle not found in the garage.");
            }

            Console.WriteLine();
        }

        private void displayVehicleInformation()
        {
            string licenseNumber = getLicenseNumber();

            if (!r_Garage.IsVehicleInGarage(licenseNumber))
            {
                Console.WriteLine("Vehicle not found in the garage.");
                return;
            }

            Vehicle vehicle = r_Garage.GetVehicleByLicenseNumber(licenseNumber);

            Console.WriteLine("Vehicle Information:");
            Console.WriteLine("--------------------");
            Console.WriteLine($"License Number: {vehicle.LicenseNumber}");
            Console.WriteLine($"Model Name: {vehicle.ModelName}");
            Console.WriteLine($"Owner Name: {vehicle.OwnerName}");
            Console.WriteLine($"Status in Garage: {vehicle.VehicleStatus}");
            Console.WriteLine($"");
            Console.WriteLine("Tire Specifications:");
            Console.WriteLine($"Number of wheels: {vehicle.Wheels.Count}");
            Console.WriteLine($"Manufacturer: {vehicle.Wheels[0].ManufacturerName}");
            Console.WriteLine($"Air Pressure: {vehicle.Wheels[0].CurrentAirPressure}/{vehicle.Wheels[0].MaxAirPressure}");
            Console.WriteLine($"");

            if (vehicle.EnergySource is GasBasedEnergy gasBasedEnergy)
            {
                Console.WriteLine($"Gas Status: {gasBasedEnergy.CurrentGasAmount}/{gasBasedEnergy.MaxGasAmount} liters");
                Console.WriteLine($"Gas Type: {gasBasedEnergy.GasType}");
            }
            else if (vehicle.EnergySource is ElectricBasedEnergy electricBasedEnergy)
            {
                Console.WriteLine($"Battery Status: {electricBasedEnergy.CurrentBatteryTime}/{electricBasedEnergy.MaxBatteryTime} hours");
            }

            if (vehicle is Car car)
            {
                Console.WriteLine($"Color: {car.Color}");
                Console.WriteLine($"Number of Doors: {car.NumberOfDoors}");
            }
            else if (vehicle is Motorcycle motorcycle)
            {
                Console.WriteLine($"License Type: {motorcycle.LicenseType}");
                Console.WriteLine($"Engine Capacity: {motorcycle.EngineCapacity} cc");
            }
            else if (vehicle is Truck truck)
            {
                Console.WriteLine($"Is Carrying Hazardous Materials: {truck.IsCarryingHazardous}");
                Console.WriteLine($"Trunk Capacity: {truck.TrunkCapacity} liters");
            }
        }
    }
}
