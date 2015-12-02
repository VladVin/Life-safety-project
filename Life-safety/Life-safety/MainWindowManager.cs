﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_safety
{
    class MainWindowManager
    {
        private MainWindow mainWindow;
        private Core.DamageParams damageParams;
        private Vector normWindVector;
        private Model model;

        public MainWindowManager(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            this.normWindVector = new Vector(0.0, 0.0);
            this.damageParams = new Core.DamageParams();
            this.damageParams.Substance = "";
            this.damageParams.SubstanceState = Core.DamageParams.SubstanceStateType.None;
            this.damageParams.Mass = -1.0f;
            this.damageParams.WindVector = new Vector(0.0f, 0.0f);
            this.damageParams.Position = new Point(0.0, 0.0);
            this.damageParams.Air = Core.DamageParams.AirType.None;
            this.damageParams.Overflow = Core.DamageParams.OverflowType.None;
            this.damageParams.Temperature = Core.DamageParams.TemperatureType.None;

            this.model = new Model();
        }

        public void UpdateSubstanceName(string substanceName)
        {
            damageParams.Substance = substanceName;
            UpdateAll();
        }

        public void UpdateSubstanceState(Core.DamageParams.SubstanceStateType substanceState)
        {
            damageParams.SubstanceState = substanceState;
            UpdateAll();
        }

        public void UpdateSubstanceMass(float mass)
        {
            damageParams.Mass = mass;
            UpdateAll();
        }

        public void UpdateWindVector(Vector windVector)
        {
            if (windVector.Length != 0.0)
            {
                normWindVector = windVector / windVector.Length;
            }
            else
            {
                normWindVector.X = 0.0;
                normWindVector.Y = 0.0;
            }
            damageParams.WindVector = normWindVector * damageParams.WindSpeed;
            UpdateAll();
        }

        public void UpdateWindSpeed(float windSpeed)
        {
            if (damageParams.WindVector.Length != 0.0)
            {
                normWindVector = damageParams.WindVector / damageParams.WindVector.Length;
            }
            else
            {
                normWindVector.X = 0.0;
                normWindVector.Y = 0.0;
            }
            damageParams.WindVector = normWindVector * windSpeed;
            UpdateAll();
        }

        public void UpdatePosition(Point position)
        {
            damageParams.Position = position;
            UpdateAll();
        }

        public void UpdateAirType(Core.DamageParams.AirType airType)
        {
            damageParams.Air = airType;
            UpdateAll();
        }

        public void UpdateOverflow(Core.DamageParams.OverflowType overflowType)
        {
            damageParams.Overflow = overflowType;
            UpdateAll();
        }

        public void UpdateTemperature(Core.DamageParams.TemperatureType temperatureType)
        {
            damageParams.Temperature = temperatureType;
            UpdateAll();
        }

        private void UpdateAll()
        {
            if (!isReadyToCalculate()) return;
            model.updateDamageParams(damageParams);
            Core.PossibleDangerZone possibleDangerZone = model.getPossibleDangerZone(damageParams.Time);
            Core.RealDangerZone realDangerZone = model.getRealDangerZone(damageParams.Time);
            mainWindow.RefreshAll(possibleDangerZone, realDangerZone);
        }

        private bool isReadyToCalculate()
        {
            if (!damageParams.Substance.Equals("") &&
                damageParams.SubstanceState != Core.DamageParams.SubstanceStateType.None &&
                damageParams.Mass > 0.0f && damageParams.Air != Core.DamageParams.AirType.None &&
                damageParams.Overflow != Core.DamageParams.OverflowType.None &&
                damageParams.Temperature != Core.DamageParams.TemperatureType.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}