using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private bool m_IsCarryingHazardous;
        private float m_TrunkCapacity;

        public Truck(string i_LicenseNumber, List<Wheel> i_Wheels, EnergySource i_EnergySource)
            : base(i_LicenseNumber, i_Wheels, i_EnergySource)
        {
        }

        public bool IsCarryingHazardous
        {
            get { return m_IsCarryingHazardous; }
            set { m_IsCarryingHazardous = value; }
        }

        public float TrunkCapacity
        {
            get { return m_TrunkCapacity; }
            set { m_TrunkCapacity = value; }
        }
    }
}
