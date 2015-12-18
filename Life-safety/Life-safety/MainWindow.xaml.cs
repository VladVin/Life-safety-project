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

        Point oldMapMousePosition;
        bool movingMode = false;
        bool startPointMode = false;
        bool endPointMode = false;
        bool windVectorMode = false;

        Line windVectorLine;
        Point startWindPoint;
        Point endWindPoint;
        bool startWindPointGiven = false;
        bool endWindPointGiven = false;

        private MapConverter mapConverter;

        public MainWindow()
        {
            InitializeComponent();

            mapConverter = new MapConverter(40000, (int)mapField.ActualWidth, (int)mapField.ActualHeight);
            initSubstanceLoader = new InitSubstanceLoader();
            InitializeWindow();
            this.windowManager = new MainWindowManager(this);

            //initValues();

            var mat = realDangerZoneEllipse.RenderTransform.Value;
            double angle = Math.Atan2(-0.5, 0.5) / Math.PI * 180.0;
            Console.WriteLine("Angle: {0}", angle);
            mat.RotateAtPrepend(angle, realDangerZoneEllipse.ActualWidth / 2, realDangerZoneEllipse.ActualHeight / 2);
            realDangerZoneEllipse.RenderTransform = new MatrixTransform(mat);
        }

        public void RefreshAll(Core.PossibleDangerZone possibleDangerZone, Core.RealDangerZone realDangerZone,
            float timeOfComing, float timeOfSteam)
        {
            depthField.Text = possibleDangerZone.Depth.ToString();
            areaField.Text = possibleDangerZone.Area.ToString();
            timeField.Text = timeOfComing.ToString();
            timeOfSteamField.Text = timeOfSteam.ToString();
            //realDangerZone.Direction.
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

            updatePointSelectionState();
        }

        private void initValues()
        {
            substanceBox.SelectedIndex = 10;
            substanceStateBox.SelectedIndex = 0;
            substanceMassBox.SelectedIndex = 2;
            windSpeedBox.SelectedIndex = 1;
            airTypeBox.SelectedIndex = 0;
            overflowTypeBox.SelectedIndex = 0;
            temperatureTypeBox.SelectedIndex = 2;
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

        private void timeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            windowManager.UpdateTime(Convert.ToSingle(timeSlider.Value));
        }

        private void mapFieldArea_MouseMove(object sender, MouseEventArgs e)
        {
            int diff = 0;
            Point pos = e.GetPosition(mapFieldCanvas);

            if (movingMode && (Math.Abs(pos.X - oldMapMousePosition.X) > diff ||
                Math.Abs(pos.Y - oldMapMousePosition.Y) > diff))
            {
                // TODO:
                //movingMode = false;
                //return;
                //Console.WriteLine(Mouse.GetPosition(mapFieldArea));
                var mat = mapFieldCanvas.RenderTransform.Value;
                mat.TranslatePrepend(pos.X - oldMapMousePosition.X, pos.Y - oldMapMousePosition.Y);
                MatrixTransform matTrans = new MatrixTransform(mat);
                mapFieldCanvas.RenderTransform = matTrans;
            }

            if (windVectorMode)
            {
                if (startWindPointGiven && !endWindPointGiven)
                {
                    windVectorLine.X2 = pos.X;
                    windVectorLine.Y2 = pos.Y;
                }
            }
        }

        private void mapFieldArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(mapFieldCanvas);
            oldMapMousePosition = new Point(pos.X, pos.Y);

            if (canMove())
            {
                movingMode = true;
            }

            if (startPointMode)
            {
                windowManager.UpdatePosition(mapConverter.TranslatePointToMeters(pos));
                Console.WriteLine("Start point: {0}", pos);
            }

            if (endPointMode)
            {
                windowManager.UpdateEndPosition(mapConverter.TranslatePointToMeters(pos));
                Console.WriteLine("End point: {0}", pos);
            }

            if (windVectorMode)
            {
                if (startWindPointGiven && endWindPointGiven)
                {
                    removeWindVectorLine();
                }

                if (!startWindPointGiven)
                {
                    createWindVectorLine();
                    startWindPoint = pos;
                    startWindPointGiven = true;
                    windVectorLine.X1 = startWindPoint.X;
                    windVectorLine.Y1 = startWindPoint.Y;
                }
                else if (startWindPointGiven && !endWindPointGiven)
                {
                    endWindPoint = pos;
                    endWindPointGiven = true;
                    windVectorLine.X2 = endWindPoint.X;
                    windVectorLine.Y2 = endWindPoint.Y;
                    Vector windVector = new Vector(endWindPoint.X - startWindPoint.X, endWindPoint.Y - startWindPoint.Y);
                    windowManager.UpdateWindVector(windVector);
                }
                else
                {
                    startWindPointGiven = false;
                    endWindPointGiven = false;
                    windowManager.UpdateWindVector(new Vector(0.0, 0.0));
                }
            }
        }

        private void mapFieldArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            movingMode = false;
        }

        private void mapFieldArea_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scaleFactor = 1.0;
            if (e.Delta > 0)
            {
                scaleFactor = 1.2;
            }
            else
            {
                scaleFactor = 1.0 / 1.2;
            }
            var mat = mapFieldCanvas.RenderTransform.Value;
            mat.ScaleAt(scaleFactor, scaleFactor, ((Canvas)sender).ActualWidth / 2, ((Canvas)sender).ActualHeight / 2);
            mapFieldCanvas.RenderTransform = new MatrixTransform(mat);
        }

        private void startPointBtn_Click(object sender, RoutedEventArgs e)
        {
            startPointMode = !startPointMode;
            endPointMode = false;
            windVectorMode = false;
            updatePointSelectionState();
        }

        private void endPointBtn_Click(object sender, RoutedEventArgs e)
        {
            endPointMode = !endPointMode;
            windVectorMode = false;
            startPointMode = false;
            updatePointSelectionState();
        }

        private void speedVectorBtn_Click(object sender, RoutedEventArgs e)
        {
            windVectorMode = !windVectorMode;
            startPointMode = false;
            endPointMode = false;
            updatePointSelectionState();
        }

        private bool canMove()
        {
            if (startPointMode || endPointMode || windVectorMode)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void updatePointSelectionState()
        {
            if (startPointMode)
            {
                startPointBtn.Background = Brushes.CornflowerBlue;
                mapFieldArea.Cursor = Cursors.Cross;
            }
            else
            {
                startPointBtn.Background = null;
            }

            if (endPointMode)
            {
                endPointBtn.Background = Brushes.Coral;
                mapFieldArea.Cursor = Cursors.Cross;
            }
            else
            {
                endPointBtn.Background = null;
            }

            if (windVectorMode)
            {
                windVectorBtn.Background = Brushes.LightGreen;
                mapFieldArea.Cursor = Cursors.Pen;
            }
            else
            {
                windVectorBtn.Background = null;
            }

            if (!startPointMode && !endPointMode && !windVectorMode)
            {
                mapFieldArea.Cursor = Cursors.Arrow;
            }
        }

        private void createWindVectorLine()
        {
            windVectorLine = new Line();
            windVectorLine.Stroke = Brushes.Brown;
            windVectorLine.StrokeThickness = 4.0;
            mapFieldCanvas.Children.Add(windVectorLine);
        }

        private void removeWindVectorLine()
        {
            mapFieldCanvas.Children.Remove(windVectorLine);
            windVectorLine = null;
        }
    }
}
