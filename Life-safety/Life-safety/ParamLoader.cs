using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

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
        
        public Model.Coeff[] loadCoeffs()
        {
            DataTable table = dataLoader.GetTable(DataLoader.Table.SUBSTANCES);
            foreach (DataRow row in table.Rows)
            {
                if ((string)row["substance"] == damageParams.Substance)
                {

                    Model.Coeff[] result = new Model.Coeff[8];

                    result[0] = float.Parse((string)row["k1"]);
                    result[1] = float.Parse((string)row["k2"]);
                    result[2] = float.Parse((string)row["k3"]);
                    string temperature;
                    switch(damageParams.Temperature)
                    {
                        case Core.DamageParams.TemperatureType.Cold:
                            temperature = "k7m40";
                            break;

                        case Core.DamageParams.TemperatureType.Freezy:
                            temperature = "k7m20";
                            break;

                        case Core.DamageParams.TemperatureType.Norm:
                            temperature = "k7p0";
                            break;

                        case Core.DamageParams.TemperatureType.Warm:
                            temperature = "k7p20"; 
                            break;

                        case Core.DamageParams.TemperatureType.Hot:
                            temperature = "k7p40";
                            break;
                        default:
                            throw new ArgumentException("Wrong temperature type in loadCoeffs()");
                    }

                    string[] parts = ((string)row[temperature]).Split('/');
                    if (parts.Length == 1)
                    {
                        result[6] = float.Parse((string)row[temperature]);
                        result[7] = float.Parse((string)row[temperature]);
                    }
                    else if (parts.Length == 2)
                    {
                        result[6] = float.Parse(parts[0]);
                        result[7] = float.Parse(parts[1]);
                    }
                    else
                    {
                        throw new Exception("Data type in loadCoeffs()");
                    }

                    Core.DamageParams.AirType air = damageParams.Air;
                    if (air == Core.DamageParams.AirType.Inversion)
                    {
                        result[4] = 1f;
                    }
                    else if (air == Core.DamageParams.AirType.Isotermia)
                    {
                        result[4] = 0.23f;
                    }
                    else
                    {
                        result[4] = 0.08f;
                    }

                    DataTable wind = dataLoader.GetTable(DataLoader.Table.WIND_COEF);
                    result[3] = float.Parse((string)wind.Rows[0]["v" + damageParams.WindSpeed.ToString()]);
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
                if (damageParams.airFromString((string)row["state"]) == damageParams.Air)
                {
                    float result = float.Parse((string)row["v" + ((int)(damageParams.WindSpeed)).ToString()]);
                    return result;
                }
            }
            return 0.0f;
        }
        
        public float loadDepth(float equivalentMass)
        {
            DataTable table = dataLoader.GetTable(DataLoader.Table.ZONE_DEPTH);
            float[] elems = new float[] { 0.01F, 0.05F, 0.1F, 0.5F, 1, 3, 5, 10, 20, 30, 50, 70, 100, 300, 500, 700, 1000, 2000 };
            int n = -1;
            for (int i = 0; i < elems.Length - 1; i++)
            {
                if (equivalentMass - (elems[i + 1] - elems[i]) / 2.0f <= elems[i])
                {
                    n = i;
                    break;
                }
            }
            if (n == -1)
                n = elems.Length - 1;
            if (n == elems.Length - 1)
                n = elems.Length - 2;
            foreach (DataRow row in table.Rows)
            {       
                if ((string)row["velocity"] == ((int)(damageParams.WindSpeed)).ToString())
                {
                    string sN = "t" + elems[n].ToString(CultureInfo.InvariantCulture).Replace(".", "");
                    string sN1 = "t" + elems[n + 1].ToString(CultureInfo.InvariantCulture).Replace(".", "");
                    float fa = float.Parse((string)row[sN], CultureInfo.InvariantCulture);
                    float fb = float.Parse((string)row[sN1], CultureInfo.InvariantCulture);
                    //float fa = float.Parse((string)row["t" + elems[n].ToString(CultureInfo.InvariantCulture).Replace(".", "")]);
                    //float fb = float.Parse((string)row["t" + elems[n+1].ToString(CultureInfo.InvariantCulture).Replace(".", "")]);
                    float a = elems[n];
                    float b = elems[n + 1];
                    float x = equivalentMass;
                    float fx = fa + (fb - fa) / (b - a) * (x - a);
                    return fx;
                }
            }
            return 0.0f;
        }
        
        public float loadDensity()
        {
            DataTable table = dataLoader.GetTable(DataLoader.Table.SUBSTANCES);
            foreach (DataRow row in table.Rows)
            {
                string field;
                if (damageParams.SubstanceState == Core.DamageParams.SubstanceStateType.Fluid)
                {
                    field = "liquid_density";
                }
                else
                {
                    field = "gas_density";
                }
                if ((string)row["substance"] == damageParams.Substance.ToString())
                {
                    float result = float.Parse((string)row[field]);
                    return result;
                }
            }
            return 0.0f;
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
                if (column.ColumnName == "velocity" || column.ColumnName == "id")
                    continue;
                string massStr = column.ColumnName.Substring(1);
                if (massStr[0] == '0')
                {
                    massStr = "0," + massStr.Substring(1);
                }
                result.Add(float.Parse(massStr));
            }
            return result.ToArray();
        }
        
        public float[] getWindSpeedVariants()
        {
            List<float> result = new List<float>();
            DataTable table = dataLoader.GetTable(DataLoader.Table.WIND_VELOCITY);
            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName == "state" || column.ColumnName == "id")
                    continue;
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
