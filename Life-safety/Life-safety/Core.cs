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
        public class DamageParams
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
            public Substance substance;
            public SubstanceState substanceState;
            public float mass;
            public float thickness;

            // Air params
            public float windSpeed;
            public AirType airType;
            public OverflowType overflowType;
            public TemperatureType temperature;
        }

        public class DangerZone
        {
            public float width;
            public float depth;
            public Point position;
            public float azimuth;
            public float area;
        }
    }
}
