using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_safety
{
    public class Model
    {
        public class Coeff
        {
            private static float eps = 1e-4f;
            private float value;

            public Coeff(float value)
            {
                this.value = value;
            }


            public float Value
            {
                get { return value; }
                set { this.value = value; }
            }

            public static Coeff operator + (Coeff c1, Coeff c2)
            {
                return new Coeff(c1.Value + c2.Value);
            }
            public static Coeff operator - (Coeff c1, Coeff c2)
            {
                return new Coeff(c1.Value - c2.Value);
            }
            public static Coeff operator * (Coeff c1, Coeff c2)
            {
                if (c1.value < eps) return new Coeff(c2.value);
                if (c2.value < eps) return new Coeff(c1.value);
                return new Coeff(c1.Value * c2.Value);
            }
            public static Coeff operator / (Coeff c1, Coeff c2)
            {
                if (c1.value < eps && c2.value > eps) return new Coeff(1.0f / c2.value);
                if (c2.value < eps) return new Coeff(c1.value);
                return new Coeff(c1.Value / c2.Value);
            }

            public static explicit operator float(Coeff c)
            {
                return c.value;
            }
            public static implicit operator Coeff(float f)
            {
                return new Coeff(f);
            }
        }

        private Core.DamageParams damageParams;
        private ParametersLoader paramLoader;
        private Coeff[] coeffs;
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

        public float TimeOfSteam()
        {
            return (float)((damageParams.Thickness * density) /
                   (coeffs[1] * coeffs[3] * coeffs[6]));
        }

        public Core.DamageParams.Loss Loss()
        {
            float perLoss = 0.0f;
            if (damageParams.WherePeople == Core.DamageParams.WherePeopleType.OpenAir)
            {
                float perGM = damageParams.PercentGasMask;
                if (perGM < 0.18f)
                {
                    perLoss = 0.9f;
                }
                else if (perGM >= 0.18f && perGM < 0.3f)
                {
                    perLoss = 0.75f;
                }
                else if (perGM >= 0.3f && perGM < 0.4f)
                {
                    perLoss = 0.65f;
                }
                else if (perGM >= 0.4f && perGM < 0.5f)
                {
                    perLoss = 0.58f;
                }
                else if (perGM >= 0.5f && perGM < 0.6f)
                {
                    perLoss = 0.5f;
                }
                else if (perGM >= 0.6f && perGM < 0.7f)
                {
                    perLoss = 0.4f;
                }
                else if (perGM >= 0.7f && perGM < 0.8f)
                {
                    perLoss = 0.35f;
                }
                else if (perGM >= 0.8f && perGM < 0.9f)
                {
                    perLoss = 0.25f;
                }
                else if (perGM >= 0.9f && perGM < 0.98f)
                {
                    perLoss = 0.18f;
                }
                else if (perGM >= 0.98f)
                {
                    perLoss = 0.1f;
                }
            }
            else if (damageParams.WherePeople == Core.DamageParams.WherePeopleType.Building)
            {
                float perGM = damageParams.PercentGasMask;
                if (perGM < 0.18f)
                {
                    perLoss = 0.5f;
                }
                else if (perGM >= 0.18f && perGM < 0.3f)
                {
                    perLoss = 0.4f;
                }
                else if (perGM >= 0.3f && perGM < 0.4f)
                {
                    perLoss = 0.35f;
                }
                else if (perGM >= 0.4f && perGM < 0.5f)
                {
                    perLoss = 0.3f;
                }
                else if (perGM >= 0.5f && perGM < 0.6f)
                {
                    perLoss = 0.27f;
                }
                else if (perGM >= 0.6f && perGM < 0.7f)
                {
                    perLoss = 0.22f;
                }
                else if (perGM >= 0.7f && perGM < 0.8f)
                {
                    perLoss = 0.18f;
                }
                else if (perGM >= 0.8f && perGM < 0.9f)
                {
                    perLoss = 0.14f;
                }
                else if (perGM >= 0.9f && perGM < 0.98f)
                {
                    perLoss = 0.09f;
                }
                else if (perGM >= 0.98f)
                {
                    perLoss = 0.04f;
                }
            }

            int countLoss = (int)(damageParams.NumHuman * perLoss);
            Core.DamageParams.Loss loss =
                new Core.DamageParams.Loss((int)(countLoss * 0.25f), 
                                           (int)(countLoss * 0.4f), 
                                           (int)(countLoss * 0.35f));
            return loss;
        }

        private float Depth(float time)
        {
            float mass_first_cloud = (float)(coeffs[0] * coeffs[2] *
                                     coeffs[4] * coeffs[6] *
                                     damageParams.Mass);

            float time_steam = TimeOfSteam();
            if (time_steam < 1.0f) time_steam = 1.0f;
            if (time < time_steam)
            {
                coeffs[5] = (float)Math.Pow(time, 0.8);
            }
            else
            {
                coeffs[5] = (float)Math.Pow(time_steam, 0.8);
            }

            float mass_second_cloud = (float)((1.0f - coeffs[0]) * coeffs[1] *
                                      coeffs[2] * coeffs[3] *
                                      coeffs[4] * coeffs[5] *
                                      coeffs[7] * damageParams.Mass /
                                      damageParams.Thickness / density);

            float trans_speed = paramLoader.loadTranslationSpeed();
            float max_depth = time * trans_speed;

            float depth_first = paramLoader.loadDepth(mass_first_cloud);
            float depth_second = paramLoader.loadDepth(mass_second_cloud);

            float depth = Math.Max(depth_first, depth_second) + 0.5f * Math.Min(depth_first, depth_second);

            return Math.Min(max_depth, depth);
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
