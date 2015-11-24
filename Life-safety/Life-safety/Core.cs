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
            public enum SubstanceType { Ammonia }
            public enum SubstanceStateType { Gas, Fluid }

            public enum AirType { Convection, Isotermia, Inversion }
            public enum Clouds
            {
                Clean, Cloudy
            }
            public enum OverflowType { Free, VPoddon }
            public enum TemperatureType { Freezy, Cold, Norm, Warm, Hot }

            // Substance params
            private SubstanceType substance;
            private SubstanceStateType substanceState;
            private float mass;
            private float thickness;
            private Point position;

            // Air params
            private Vector windVector;
            private AirType airType;
            private OverflowType overflow;
            private TemperatureType temperature;

            public SubstanceType Substance
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
