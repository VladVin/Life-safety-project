using System;
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
        private Model model;

        public MainWindowManager(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

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
