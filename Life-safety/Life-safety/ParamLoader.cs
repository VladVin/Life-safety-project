using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_safety
{
    public class ParamLoader
    {
        private Core.DamageParams damageParams;
        public ParamLoader(Core.DamageParams damageParams)
        {
            this.damageParams = damageParams;
        }
        public float loadDepth(float windSpeed, float mass);
        public float[] loadCoeffs();
        public float loadDensity();
    }
}
