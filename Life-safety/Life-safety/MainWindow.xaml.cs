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

            //float[] masses = initSubstanceLoader.getSubstancesMasses();
            //foreach (float mass in masses)
            //{
            //    substanceMassBox.Items.Add(Convert.ToString(mass));
            //}

            //float[] windSpeedVariants = initSubstanceLoader.getWindSpeedVariants();
            //foreach (float windSpeedVariant in windSpeedVariants)
            //{
            //    windSpeedBox.Items.Add(Convert.ToString(windSpeedVariant));
            //}

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
    }
}
