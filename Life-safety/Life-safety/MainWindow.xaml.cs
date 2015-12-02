using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Life_safety
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InitSubstanceLoader initSubstanceLoader;
        MainWindowManager windowManager;

        public MainWindow()
        {
            InitializeComponent();

            initSubstanceLoader = new InitSubstanceLoader();

            this.windowManager = new MainWindowManager(this);
            InitializeWindow();
        }

        public void RefreshAll(Core.PossibleDangerZone possibleDangerZone, Core.RealDangerZone realDangerZone)
        {

        }

        private void InitializeWindow()
        {
            string[] substanceNames = initSubstanceLoader.getSubstancesNames();
            foreach(string substanceName in substanceNames)
            {
                substanceBox.Items.Add(substanceName);
            }

            string[] substanceStates = initSubstanceLoader.getSubstancesStates();
            foreach(string substanceState in substanceStates)
            {
                substanceStateBox.Items.Add(substanceState);
            }

            float[] masses = initSubstanceLoader.getSubstancesMasses();
            foreach (float mass in masses)
            {
                substanceMassBox.Items.Add(Convert.ToString(mass));
            }

            float[] windspeedvariants = initSubstanceLoader.getWindSpeedVariants();
            foreach (float windspeedvariant in windspeedvariants)
            {
                windSpeedBox.Items.Add(Convert.ToString(windspeedvariant));
            }

            string[] airTypes = initSubstanceLoader.getAirTypes();
            foreach (string airType in airTypes)
            {
                airTypeBox.Items.Add(airType);
            }

            string[] overflowTypes = initSubstanceLoader.getOverflowTypes();
            foreach(string overflowType in overflowTypes)
            {
                overflowTypeBox.Items.Add(overflowType);
            }

            int[] temperatureVars = initSubstanceLoader.getTemperatureVariants();
            foreach(int temperatureVar in temperatureVars)
            {
                temperatureTypeBox.Items.Add(Convert.ToString(temperatureVar));
            }
        }

        private void substanceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            windowManager.UpdateSubstanceName(e.AddedItems[0].ToString());
        }

        private void substanceStateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Core.DamageParams.SubstanceStateType state = Core.DamageParams.SubstanceStateType.None;
            if (substanceStateBox.SelectedIndex == 0) state = Core.DamageParams.SubstanceStateType.Gas;
            if (substanceStateBox.SelectedIndex == 1) state = Core.DamageParams.SubstanceStateType.Fluid;
            if (state == Core.DamageParams.SubstanceStateType.None) return;
            windowManager.UpdateSubstanceState(state);
        }

        private void substanceMassBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            windowManager.UpdateSubstanceMass(Convert.ToSingle((string)e.AddedItems[0]));
        }

        private void windSpeedBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            windowManager.UpdateWindSpeed(Convert.ToSingle((string)e.AddedItems[0]));
        }

        private void airTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Core.DamageParams.AirType airType = Core.DamageParams.AirType.None;
            if (airTypeBox.SelectedIndex == 0) airType = Core.DamageParams.AirType.Inversion;
            if (airTypeBox.SelectedIndex == 1) airType = Core.DamageParams.AirType.Isotermia;
            if (airTypeBox.SelectedIndex == 2) airType = Core.DamageParams.AirType.Convection;
            if (airType == Core.DamageParams.AirType.None) return;
            windowManager.UpdateAirType(airType);
            //windowManager.UpdateAirType()
        }

        private void overflowTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Core.DamageParams.OverflowType overflowType = Core.DamageParams.OverflowType.None;
            if (overflowTypeBox.SelectedIndex == 0) overflowType = Core.DamageParams.OverflowType.VPoddon;
            if (overflowTypeBox.SelectedIndex == 1) overflowType = Core.DamageParams.OverflowType.Free;
            if (overflowType == Core.DamageParams.OverflowType.None) return;
            windowManager.UpdateOverflow(overflowType);
        }

        private void temperatureTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Core.DamageParams.TemperatureType temperatureType = Core.DamageParams.TemperatureType.None;
            if (temperatureTypeBox.SelectedIndex == 0) temperatureType = Core.DamageParams.TemperatureType.Freezy;
            if (temperatureTypeBox.SelectedIndex == 1) temperatureType = Core.DamageParams.TemperatureType.Cold;
            if (temperatureTypeBox.SelectedIndex == 2) temperatureType = Core.DamageParams.TemperatureType.Norm;
            if (temperatureTypeBox.SelectedIndex == 3) temperatureType = Core.DamageParams.TemperatureType.Warm;
            if (temperatureTypeBox.SelectedIndex == 4) temperatureType = Core.DamageParams.TemperatureType.Hot;
            if (temperatureType == Core.DamageParams.TemperatureType.None) return;
            windowManager.UpdateTemperature(temperatureType);
        }
    }
}
