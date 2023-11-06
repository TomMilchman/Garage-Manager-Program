using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Motorcycle : Vehicle
    {
        private eLicenseType m_LicenseType;
        private int m_EngineCapacity;

        public enum eLicenseType
        {
            A1 = 1,
            A2 = 2,
            AA = 3,
            B1 = 4
        }

        public Motorcycle(string i_LicenseNumber, List<Wheel> i_Wheels, EnergySource i_EnergySource)
            : base(i_LicenseNumber, i_Wheels, i_EnergySource)
        {
        }

        public eLicenseType LicenseType
        {
            get { return m_LicenseType; }
            set { m_LicenseType = value; }
        }

        public int EngineCapacity
        {
            get { return m_EngineCapacity; }
            set { m_EngineCapacity = value; }
        }
    }
}
