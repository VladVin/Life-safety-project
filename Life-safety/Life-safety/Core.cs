using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Life_safety
{
    public class Core
    {
        public class DamageParams
        {
            public enum SubstanceStateType { None, Gas, Fluid }
            public enum OverflowType { None, Free, VPoddon }
            public enum TemperatureType { None, Freezy, Cold, Norm, Warm, Hot }
            public enum AirType { None, Convection, Isotermia, Inversion }
            public enum WherePeopleType { None, OpenAir, Building }

            // Substance params
            private string substance;
            private SubstanceStateType substanceState;
            private float mass;
            private float thickness;
            private Point position;
            private float percentGasMask;
            private int numHuman;
            private WherePeopleType wherePeople;

            public class Loss
            {
                public Loss(int h, int m, int l)
                {
                    hard = h;
                    medium = m;
                    lite = l;
                }
                public int hard;
                public int medium;
                public int lite;
            }

            // Air params
            private Vector windVector;
            private AirType airType;
            private OverflowType overflow;
            private TemperatureType temperature;

            private float time;

            public string Substance
            {
                get
                {
                    return substance;
                }
                set
                {
                    substance = value;
                }
            }

            public SubstanceStateType SubstanceState
            {
                get
                {
                    return substanceState;
                }
                set
                {
                    substanceState = value;
                }
            }

            public float Mass
            {
                get
                {
                    return mass;
                }
                set
                {
                    mass = value;
                }
            }

            public float Thickness
            {
                get
                {
                    if (overflow == OverflowType.VPoddon)
                    {
                        return thickness - 0.2f;
                    }
                    return thickness;
                }
                set
                {
                    thickness = value;
                }
            }

            public Point Position
            {
                get
                {
                    return position;
                }
                set
                {
                    position = value;
                }
            }

            public float WindSpeed
            {
                get
                {
                    return (float)windVector.Length;
                }
            }

            public Vector WindVector
            {
                get
                {
                    return windVector;
                }
                set
                {
                    windVector = value;
                }
            }

            public AirType Air
            {
                get
                {
                    return airType;
                }
                set
                {
                    airType = value;
                }
            }

            public OverflowType Overflow
            {
                get
                {
                    return overflow;
                }
                set
                {
                    overflow = value;
                }
            }

            public TemperatureType Temperature
            {
                get
                {
                    return temperature;
                }
                set
                {
                    temperature = value;
                }
            }

            public float Time
            {
                get
                {
                    return time;
                }
                set
                {
                    time = value;
                }
            }

            public WherePeopleType WherePeople
            {
                get { return wherePeople; }
                set { wherePeople = value; }
            }

            public float PercentGasMask
            {
                get { return percentGasMask; }
                set { percentGasMask = value; }
            }

            public int NumHuman
            {
                get { return numHuman; }
                set { numHuman = value; }
            }
        }

        public class PossibleDangerZone
        {
            private float angle;
            private float depth;
            private Point position;
            private Vector direction;
            private float area;

            public float Angle
            {
                get
                {
                    return angle;
                }
                set
                {
                    angle = value;
                }
            }

            public float Depth
            {
                get
                {
                    return depth;
                }
                set
                {
                    depth = value;
                }
            }

            public Point Position
            {
                get
                {
                    return position;
                }
                set
                {
                    position = value;
                }
            }

            public Vector Direction
            {
                get
                {
                    return direction;
                }
                set
                {
                    direction = value;
                }
            }

            public float Area
            {
                get
                {
                    return area;
                }
                set
                {
                    area = value;
                }
            }
        }

        public class RealDangerZone
        {
            private float width;
            private float depth;
            private Point position;
            private Point shiftedCenter;
            private Vector direction;
            private float area;

            public Point ShiftedCenter
            {
                get
                {
                    return shiftedCenter;
                }
                set
                {
                    shiftedCenter = value;
                }
            }
            public float Width
            {
                get
                {
                    return width;
                }
                set
                {
                    width = value;
                }
            }

            public float Depth
            {
                get
                {
                    return depth;
                }
                set
                {
                    depth = value;
                }
            }

            public Point Position
            {
                get
                {
                    return position;
                }
                set
                {
                    position = value;
                }
            }

            public Vector Direction
            {
                get
                {
                    return direction;
                }
                set
                {
                    direction = value;
                }
            }

            public float Area
            {
                get
                {
                    return area;
                }
                set
                {
                    area = value;
                }
            }
        }
    }
}
