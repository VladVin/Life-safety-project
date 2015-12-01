using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_safety
{
    class Model
    {
        private Core.DamageParams damageParams;
        private Core.DangerZone dangerZone;
        //private ParamLoader paramLoader;
        private float[] coeffs;
        private float density;

        Model(Core.DamageParams damageParams)
        {
            this.damageParams = damageParams;
            
            // FIXME
            //paramLoader = new ParamLoader(damageParams);
            //coeffs = paramLoader.loadCoeffs();
            //density = paramLoader.loadDensity();
        }

        private float depth(float time)
        {
            float mass_first_cloud = coeffs[1] * coeffs[3] *
                                     coeffs[5] * coeffs[7] *
                                     damageParams.Mass;
            
            float time_steam = timeOfSteam();
            if (time_steam < 1.0f) time_steam = 1.0f;
            if (time < time_steam) {
                coeffs[6] = (float)Math.Pow(time, 0.8);
            } else {
                coeffs[6] = (float)Math.Pow(time_steam, 0.8);
            }

            float mass_second_cloud = (1.0f - coeffs[1]) * coeffs[2] *
                                      coeffs[3] * coeffs[4] *
                                      coeffs[5] * coeffs[6] *
                                      coeffs[7] * damageParams.Mass /
                                      damageParams.Thickness / density;

            // FIXME

            //float trans_speed = paramLoader.loadTranslationSpeed();
            //float max_depth = time * trans_speed;

            //float depth_first = paramLoader.loadDepth(mass_first_cloud);
            //float depth_second = paramLoader.loadDepth(mass_second_cloud);

            //float depth = Math.Max(depth_first, depth_second) + 0.5f * Math.Min(depth_first, depth_second);

            //return Math.Min(max_depth, depth);
            return 0.0f;
        }

        private float timeOfSteam()
        {
            return (damageParams.Thickness * density) / 
                   (coeffs[2] * coeffs[4] * coeffs[7]);
        }

        // Init DangerZone by params
        // Get DangerZone by time
        // Get death count
    }
}
