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
            public enum OverflowType { Free, VPoddon, VObvalovku }
            public enum TemperatureType { Freezy, Cold, Norm, Warm, Hot }

            // Substance params
            private SubstanceType substance;
            private SubstanceStateType substanceState;
            private float mass;
            private float thickness;

            // Air params
            private float windSpeed;
            private AirType airType;
            private OverflowType overflowType;
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
                    return thickness;
                }
                set
                {
                    thickness = value;
                }
            }

            public float WindSpeed
            {
                get
                {
                    return windSpeed;
                }
                set
                {
                    windSpeed = value;
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
                    return overflowType;
                }
                set
                {
                    overflowType = value;
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

        public class DangerZone
        {
            private float width;
            private float depth;
            private Point position;
            private float azimuth;
            private float area;

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

            public float Azimuth
            {
                get
                {
                    return azimuth;
                }
                set
                {
                    azimuth = value;
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
