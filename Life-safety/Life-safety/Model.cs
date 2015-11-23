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
        private ParamLoader paramLoader;
        private float[] coeffs;
        private float density;

        Model(Core.DamageParams damageParams)
        {
            this.damageParams = damageParams;
            
            paramLoader = new ParamLoader(damageParams);
            coeffs = paramLoader.loadCoeffs();
            density = paramLoader.loadDensity();
        }

        public Core.DangerZone calcDangerZone()
        {

        }

        private float depth()
        {
            float mass_first_cloud = coeffs[1] * coeffs[3] *
                                     coeffs[5] * coeffs[7] *
                                     damageParams.mass;

            float mass_second_cloud = (1.0f - coeffs[1]) * coeffs[2] *
                                      coeffs[3] * coeffs[4] *
                                      coeffs[5] * coeffs[6] *
                                      coeffs[7] * damageParams.mass /
                                      damageParams.thickness / density;


            return 0.0f;
        }

        private float timeOfSteam()
        {
            return (damageParams.thickness * density) / 
                   (coeffs[2] * coeffs[4] * coeffs[7]);
        }
        // Init DangerZone by params
        // Get DangerZone by time
        // Get death count
    }
}
