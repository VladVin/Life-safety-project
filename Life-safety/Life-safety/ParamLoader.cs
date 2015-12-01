using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_safety
{
    public class ParametersLoader
    {
        private Core.DamageParams damageParams;
        private DataLoader dataLoader;
        
        public ParametersLoader()
        {
            this.dataLoader = new DataLoader();
        }

        public void updateDamageParams(Core.DamageParams damageParams)
        {
            this.damageParams = damageParams;
        }
        
        public float loadDepth(float windSpeed, float mass)
        {
            DataTable table = dataLoader.GetTable(DataLoader.Table.SUBSTANCES);
            foreach (DataRow row in table.Rows)
            {
                if ((string)row["speed"] == windSpeed.ToString())
                {
                    return float.Parse((string)row[mass.ToString()]);
                }
            }
            //return null;

            throw new Exception("Incorrect depth data");
        }
        
        public float[] loadCoeffs()
        {
            DataTable table = dataLoader.GetTable(DataLoader.Table.SUBSTANCES);
            foreach (DataRow row in table.Rows)
            {
                if ((string)row["substance"] == damageParams.Substance.ToString())
                {
                    float[] result = new float[8];
                    result[0] = float.Parse((string)row["k1"]);
                    result[1] = float.Parse((string)row["k2"]);
                    result[2] = float.Parse((string)row["k3"]);

                    switch(damageParams.Temperature)
                    {
                        case Core.DamageParams.TemperatureType.Cold:
                            result[7] = float.Parse((string)row["k7m40"]);
                            break;

                        case Core.DamageParams.TemperatureType.Freezy:
                            result[7] = float.Parse((string)row["k7m20"]);
                            break;

                        case Core.DamageParams.TemperatureType.Norm:
                            result[7] = float.Parse((string)row["k7p0"]);
                            break;

                        case Core.DamageParams.TemperatureType.Warm:
                            result[7] = float.Parse((string)row["k7p20"]); 
                            break;

                        case Core.DamageParams.TemperatureType.Hot:
                            result[7] = float.Parse((string)row["k7p40"]);
                            break;
                    }

                    Core.DamageParams.AirType air = damageParams.Air;
                    if (air == Core.DamageParams.AirType.Inversion)
                    {
                        result[5] = 1f;
                    }
                    else if (air == Core.DamageParams.AirType.Isotermia)
                    {
                        result[5] = 0.23f;
                    }
                    else
                    {
                        result[5] = 0.08f;
                    }

                    DataTable wind = dataLoader.GetTable(DataLoader.Table.WIND_COEF);
                    result[4] = float.Parse((string)wind.Rows[0][damageParams.WindSpeed.ToString()]);

                    return result;
                }
            }
            return null;
        }
        
        public float loadTranslationSpeed()
        {
            DataTable table = dataLoader.GetTable(DataLoader.Table.WIND_VELOCITY);
            foreach (DataRow row in table.Rows)
            {
                if ((string)row["state"] == damageParams.Air.ToString())
                {
                    float result = float.Parse((string)row[damageParams.WindSpeed.ToString()]);
                    return result;
                }
            }
            return 0;
        }
        
        public float loadDepth(float equivalentMass)
        {
            DataTable table = dataLoader.GetTable(DataLoader.Table.ZONE_DEPTH);
            foreach (DataRow row in table.Rows)
            {
                if ((string)row["velocity"] == damageParams.WindSpeed.ToString())
                {
                    float result = float.Parse((string)row[equivalentMass.ToString()]);
                    return result;
                }
            }
            return 0;
        }
        
        public float loadDensity()
        {
            // Stub
            return 0;
        }
    }

    public class DataLoader
    {
        private const string DATA_PATH = "../../../../data/xml/";    // Bin path

        public enum Table
        {
            ATMOSPHERE,
            ZONE_DEPTH,
            SUBSTANCES,
            WIND_COEF,
            WIND_VELOCITY
        }

        private readonly string[] tablesName = { "", "depth.xml", "toxic_substances.xml", "k4.xml" , "wind_velocity.xml" };
        private DataSet[] tables = new DataSet[5];
        
        public DataLoader()
        {
            for (int i = 0; i < tablesName.Length; i++)
            {
                if (tablesName[i] == "")
                    continue;
                tables[i] = new DataSet();
                string file = DATA_PATH + tablesName[i];
                tables[i].ReadXml(file);
                if (tables[i].Tables.Count != 1)
                    throw new Exception("XML parsing error");
            }
        }

        public DataTable GetTable(Table table)
        {
            switch (table)
            {
                case Table.ATMOSPHERE:
                    return tables[0].Tables[0];
                case Table.ZONE_DEPTH:
                    return tables[1].Tables[0];
                case Table.SUBSTANCES:
                    return tables[2].Tables[0];
                case Table.WIND_COEF:
                    return tables[3].Tables[0];
                case Table.WIND_VELOCITY:
                    return tables[4].Tables[0];
            }
            throw new Exception("Incorrect table ID");
        }
    }

    public class InitSubstanceLoader
    {
        private DataLoader dataLoader;
        
        public InitSubstanceLoader()
        {
            this.dataLoader = new DataLoader();
        }
        
        public string[] getSubstancesNames()
        {
            List<string> result = new List<string>();
            DataTable table = dataLoader.GetTable(DataLoader.Table.SUBSTANCES);
            foreach (DataRow row in table.Rows)
            {
                result.Add((string)row["substance"]);
            }
            return result.ToArray();
        }

        public string[] getSubstancesStates()
        {
            return new string[] { "Газ", "Жидкость"};
        }
        
        public float[] getSubstancesMasses()
        {
            List<float> result = new List<float>();
            DataTable table = dataLoader.GetTable(DataLoader.Table.ZONE_DEPTH);
            foreach (DataColumn column in table.Columns)
            {
                result.Add(float.Parse(column.ColumnName.Substring(1)));
            }
            return result.ToArray();
        }
        
        public float[] getWindSpeedVariants()
        {
            List<float> result = new List<float>();
            DataTable table = dataLoader.GetTable(DataLoader.Table.WIND_VELOCITY);
            foreach (DataColumn column in table.Columns)
            {
                result.Add(float.Parse(column.ColumnName.Substring(1)));
            }
            return result.ToArray();
        }
        
        public string[] getAirTypes()
        {
            List<string> result = new List<string>();
            DataTable table = dataLoader.GetTable(DataLoader.Table.WIND_VELOCITY);
            foreach (DataRow row in table.Rows)
            {
                result.Add((string)row["state"]);
            }
            return result.ToArray();
        }
        
        public string[] getOverflowTypes()
        {
            return new string[] { "В поддон", "Открыто" };
        }
        
        public int[] getTemperatureVariants()
        {
            return new int[] { -40, -20, 0, 20, 40 };
        }
    }
} 
