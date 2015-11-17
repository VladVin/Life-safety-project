using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Life_safety
{
    class Core
    {
        class DamageParams
        {
            public enum Substance { Ammonia }
            public enum SubstanceState { Gas, Fluid }

            public enum AirType { Convection, Isotermia, Inversion }
            public enum Clouds
            {
                Clean, Cloudy
            }
            public enum OverflowType { Free, VPoddon, VObvalovku }
            public enum TemperatureType { Freezy, Cold, Norm, Warm, Hot }

            // Substance params
            private Substance substance;
            private SubstanceState substanceState;
            private float mass;

            // Air params
            private float windSpeed;
            private AirType airType;
            private OverflowType overflowType;
            private TemperatureType temperature;
        }

        class DangerZone
        {
            private float width;
            private float depth;
            private Point position;
            private float azimuth;
            private float area;

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
