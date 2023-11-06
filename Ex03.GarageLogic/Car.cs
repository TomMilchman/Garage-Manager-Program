using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        private eColor m_Color;
        private eNumberOfDoors m_NumberOfDoors;

        public enum eColor
        {
            White = 1,
            Black = 2,
            Yellow = 3,
            Red = 4
        }
        public enum eNumberOfDoors
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        public Car(string i_LicenseNumber ,List<Wheel> i_Wheels, EnergySource i_EnergySource)
            : base(i_LicenseNumber ,i_Wheels, i_EnergySource)
        {
        }

        public eColor Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        public eNumberOfDoors NumberOfDoors
        {
            get { return m_NumberOfDoors; }
            set { m_NumberOfDoors = value; }
        }
    }
}
