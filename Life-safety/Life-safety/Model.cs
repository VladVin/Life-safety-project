using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_safety
{
    public class Model
    {
        private Core.DamageParams damageParams;
        private ParametersLoader paramLoader;
        private float[] coeffs;
        private float density;

        public Model()
        {
            this.paramLoader = new ParametersLoader();
        }

        public void updateDamageParams(Core.DamageParams damageParams)
        {
            this.damageParams = damageParams;
            paramLoader.updateDamageParams(damageParams);
        }

        public Core.PossibleDangerZone getPossibleDangerZone(float time)
        {
            coeffs = paramLoader.loadCoeffs();
            paramLoader = new ParametersLoader();
            density = paramLoader.loadDensity();
            Core.PossibleDangerZone pDangerZone = new Core.PossibleDangerZone();
            pDangerZone.Angle = Angle();
            pDangerZone.Depth = Depth(time);
            pDangerZone.Area = PossibleZoneArea(time);
            pDangerZone.Position = damageParams.Position;
            pDangerZone.Direction = damageParams.WindVector;
            return pDangerZone;
        }

        public Core.RealDangerZone getRealDangerZone(float time)
        {
            coeffs = paramLoader.loadCoeffs();
            paramLoader = new ParametersLoader();
            density = paramLoader.loadDensity();
            Core.RealDangerZone rDangerZone = new Core.RealDangerZone();
            rDangerZone.Depth = Depth(time);
            rDangerZone.Area = RealZoneArea(time);
            rDangerZone.Width = Width(rDangerZone.Depth);
            rDangerZone.Position = damageParams.Position;
            rDangerZone.Direction = damageParams.WindVector;
            rDangerZone.ShiftedCenter = ShiftedCenter(rDangerZone.Depth);
            return rDangerZone;
        }

        public float TimeOfComing(System.Windows.Point point)
        {
            float trans_speed = paramLoader.loadTranslationSpeed();
            System.Windows.Vector v = new System.Windows.Vector(
                point.X - damageParams.Position.X, 
                point.Y - damageParams.Position.Y);
            float dist = (float)v.Length;
            return dist / trans_speed;
        }

        private float Depth(float time)
        {
            float mass_first_cloud = coeffs[1] * coeffs[3] *
                                     coeffs[5] * coeffs[7] *
                                     damageParams.Mass;

            float time_steam = TimeOfSteam();
            if (time_steam < 1.0f) time_steam = 1.0f;
            if (time < time_steam)
            {
                coeffs[6] = (float)Math.Pow(time, 0.8);
            }
            else
            {
                coeffs[6] = (float)Math.Pow(time_steam, 0.8);
            }

            float mass_second_cloud = (1.0f - coeffs[1]) * coeffs[2] *
                                      coeffs[3] * coeffs[4] *
                                      coeffs[5] * coeffs[6] *
                                      coeffs[7] * damageParams.Mass /
                                      damageParams.Thickness / density;

            float trans_speed = paramLoader.loadTranslationSpeed();
            float max_depth = time * trans_speed;

            float depth_first = paramLoader.loadDepth(mass_first_cloud);
            float depth_second = paramLoader.loadDepth(mass_second_cloud);

            float depth = Math.Max(depth_first, depth_second) + 0.5f * Math.Min(depth_first, depth_second);

            return Math.Min(max_depth, depth);
        }

        public float TimeOfSteam()
        {
            return (damageParams.Thickness * density) /
                   (coeffs[2] * coeffs[4] * coeffs[7]);
        }

        private float Angle()
        {
            float phi;
            float windSpeed = damageParams.WindSpeed;
            if (windSpeed < 0.5f)
            {
                phi = 360f;
            }
            else if (windSpeed >= 0.5f && windSpeed < 1f)
            {
                phi = 180f;
            }
            else if (windSpeed >= 1f && windSpeed < 2f)
            {
                phi = 90f;
            }
            else
            {
                phi = 45f;
            }

            return phi;
        }

        private float Width(float depth)
        {
            float width;
            float windSpeed = damageParams.WindSpeed;
            if (windSpeed < 0.5f)
            {
                width = depth;
            }
            else if (windSpeed >= 0.5f && windSpeed < 1f)
            {
                width = depth / 4f;
            }
            else if (windSpeed >= 1f && windSpeed < 2f)
            {
                width = depth / 8f;
            }
            else
            {
                width = depth / 16f;
            }

            return width;
        }

        private System.Windows.Point ShiftedCenter(float depth)
        {
            float windSpeed = damageParams.WindSpeed;
            System.Windows.Point position = damageParams.Position;
            System.Windows.Point center;
            System.Windows.Vector shift = damageParams.WindVector;
            shift.Normalize();

            if (windSpeed < 0.5f)
            {
                center = position;
            }
            else if (windSpeed >= 0.5f && windSpeed < 1f)
            {
                center = System.Windows.Point.Add(position, System.Windows.Vector.Multiply(depth / 8f, shift));
            }
            else if (windSpeed >= 1f && windSpeed < 2f)
            {
                center = System.Windows.Point.Add(position, System.Windows.Vector.Multiply(depth / 4f, shift));
            }
            else
            {
                center = System.Windows.Point.Add(position, System.Windows.Vector.Multiply(depth / 2f, shift));
            }

            return center;
        }

        private float PossibleZoneArea(float time)
        {
            float phi = Angle();
            float depth = Depth(time);
            return 8.72f * 0.001f * depth * depth * phi;
        }

        private float RealZoneArea(float time)
        {
            float coeff8;
            Core.DamageParams.AirType air = damageParams.Air;
            if (air == Core.DamageParams.AirType.Inversion)
            {
                coeff8 = 0.081f;
            }
            else if (air == Core.DamageParams.AirType.Isotermia)
            {
                coeff8 = 0.133f;
            }
            else
            {
                coeff8 = 0.235f;
            }

            float depth = Depth(time);

            return coeff8 * depth * depth * (float)Math.Pow(time, 0.2);
        }
    }
}
