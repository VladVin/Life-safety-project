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
using Microsoft.Expression.Controls;

namespace Life_safety
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InitSubstanceLoader initSubstanceLoader;
        private MainWindowManager windowManager;
        private MapConverter mapConverter;

        private Point oldMapMousePosition;
        private bool movingMode = false;
        private bool startPointMode = false;
        private bool endPointMode = false;
        private bool windVectorMode = false;

        private Point realDangerZonePos;
        private Point experimentZonePos;
        private Line cross1Line, cross2Line;
        private Ellipse experimentCircle, experimentCircle2;
        private LineArrow windArrow;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void RefreshAll(Core.PossibleDangerZone possibleDangerZone, Core.RealDangerZone realDangerZone,
            float timeOfComing, float timeOfSteam)
        {
            depthField.Text = realDangerZone.Depth.ToString();
            widthField.Text = realDangerZone.Width.ToString();
            areaField.Text = realDangerZone.Area.ToString();
            timeField.Text = timeOfComing.ToString();
            timeOfSteamField.Text = timeOfSteam.ToString();
            realDangerZoneEllipse.Width = mapConverter.ConvertWidthToPixels(realDangerZone.Width);
            realDangerZoneEllipse.Height = mapConverter.ConvertHeightToPixels(realDangerZone.Depth);
            Point position = mapConverter.TranslatePointToPixels(realDangerZone.Position);
            Point center = mapConverter.TranslatePointToPixels(realDangerZone.ShiftedCenter);
            Vector dir = realDangerZone.Direction;
            realDangerZoneEllipse.Margin = new Thickness(position.X, position.Y, 0.0, 0.0);
            var mat = realDangerZoneEllipse.RenderTransform.Value;
            double angle = Math.Atan2(dir.Y, dir.X) / Math.PI * 180.0;
            mat.RotateAtPrepend(angle, realDangerZoneEllipse.ActualWidth / 2.0, realDangerZoneEllipse.ActualHeight / 2.0);
            realDangerZoneEllipse.RenderTransform = new MatrixTransform(mat);
            realDangerZoneEllipse.Visibility = Visibility.Visible;
        }

        private void initializeWindow()
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

            realDangerZonePos = new Point(300.0, 400.0);
            experimentZonePos = new Point(800.0, 250.0);

            buildingPeopleCountField.Text = "1000";
            openAirPeopleCountField.Text = "5000";
            buildingSafetyPercent.Value = 70.0;
            openAirSafetyPercent.Value = 30.0;

            substanceBox.SelectedIndex = 10;
            substanceStateBox.SelectedIndex = 0;
            substanceMassBox.SelectedIndex = 2;
            windSpeedBox.SelectedIndex = 1;
            airTypeBox.SelectedIndex = 0;
            overflowTypeBox.SelectedIndex = 0;
            temperatureTypeBox.SelectedIndex = 2;
            arrowE_MouseDown(arrowE, null);

            drawCross(realDangerZonePos);
            drawExperimentCircle(experimentZonePos);
            windowManager.UpdatePosition(mapConverter.TranslatePointToMeters(realDangerZonePos));
            windowManager.UpdateEndPosition(mapConverter.TranslatePointToMeters(experimentZonePos));

            updatePointSelectionState();
        }

        private void drawCross(Point pos)
        {
            if (cross1Line != null)
            {
                mapFieldCanvas.Children.Remove(cross1Line);
            }

            if (cross2Line != null)
            {
                mapFieldCanvas.Children.Remove(cross2Line);
            }

            cross1Line = new Line();
            cross1Line.StrokeThickness = 5.0;
            cross1Line.Stroke = Brushes.Red;
            cross1Line.Opacity = 0.7;
            cross1Line.X1 = pos.X - 10.0;
            cross1Line.Y1 = pos.Y - 10.0;
            cross1Line.X2 = pos.X + 10.0;
            cross1Line.Y2 = pos.Y + 10.0;

            cross2Line = new Line();
            cross2Line.StrokeThickness = 5.0;
            cross2Line.Stroke = Brushes.Red;
            cross2Line.Opacity = 0.7;
            cross2Line.X1 = pos.X - 10.0;
            cross2Line.Y1 = pos.Y + 10.0;
            cross2Line.X2 = pos.X + 10.0;
            cross2Line.Y2 = pos.Y - 10.0;

            mapFieldCanvas.Children.Add(cross1Line);
            mapFieldCanvas.Children.Add(cross2Line);
        }

        private void drawExperimentCircle(Point pos)
        {
            if (experimentCircle != null)
            {
                mapFieldCanvas.Children.Remove(experimentCircle);
            }

            if (experimentCircle2 != null)
            {
                mapFieldCanvas.Children.Remove(experimentCircle2);
            }

            experimentCircle = new Ellipse();
            double r = 20.0;
            experimentCircle.Width = 2 * r;
            experimentCircle.Height = 2 * r;
            experimentCircle.Margin = new Thickness(pos.X - r, pos.Y - r, 0, 0);
            experimentCircle.Stroke = Brushes.Black;
            experimentCircle.Fill = Brushes.BlueViolet;
            experimentCircle.Opacity = 0.7;
            mapFieldCanvas.Children.Add(experimentCircle);

            experimentCircle2 = new Ellipse();
            double r2 = 5.0;
            experimentCircle2.Width = 2 * r2;
            experimentCircle2.Height = 2 * r2;
            experimentCircle2.Margin = new Thickness(pos.X - r2, pos.Y - r2, 0, 0);
            experimentCircle2.Stroke = Brushes.Black;
            experimentCircle2.Fill = Brushes.Black;
            experimentCircle2.Opacity = 0.7;
            mapFieldCanvas.Children.Add(experimentCircle2);
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
                drawCross(pos);
            }

            if (endPointMode)
            {
                windowManager.UpdateEndPosition(mapConverter.TranslatePointToMeters(pos));
                drawExperimentCircle(pos);
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

            if (!startPointMode && !endPointMode && !windVectorMode)
            {
                mapFieldArea.Cursor = Cursors.Arrow;
            }
        }

        private void createWindVectorLine()
        {
            windArrow = new LineArrow();
            windArrow.Height = 0;
            windArrow.Stroke = Brushes.PaleVioletRed;
            windArrow.StrokeThickness = 4.0;
            mapFieldCanvas.Children.Add(windArrow);
        }

        private void updateAllArrows(Image except)
        {
            arrowN.Source = (ImageSource)FindResource("arrow");
            arrowNW.Source = (ImageSource)FindResource("arrow");
            arrowNE.Source = (ImageSource)FindResource("arrow");
            arrowW.Source = (ImageSource)FindResource("arrow");
            arrowE.Source = (ImageSource)FindResource("arrow");
            arrowS.Source = (ImageSource)FindResource("arrow");
            arrowSW.Source = (ImageSource)FindResource("arrow");
            arrowSE.Source = (ImageSource)FindResource("arrow");

            except.Source = (ImageSource)FindResource("arrow-red");
        }

        private void arrowNW_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateAllArrows((Image)sender);
            Vector windVector = new Vector(-1.0, 1.0);
            windowManager.UpdateWindVector(windVector);
        }

        private void arrowN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateAllArrows((Image)sender);
            Vector windVector = new Vector(0.0, 1.0);
            windowManager.UpdateWindVector(windVector);
        }

        private void arrowNE_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateAllArrows((Image)sender);
            Vector windVector = new Vector(1.0, 1.0);
            windowManager.UpdateWindVector(windVector);
        }

        private void arrowW_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateAllArrows((Image)sender);
            Vector windVector = new Vector(-1.0, 0.0);
            windowManager.UpdateWindVector(windVector);
        }

        private void arrowE_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateAllArrows((Image)sender);
            Vector windVector = new Vector(1.0, 0.0);
            windowManager.UpdateWindVector(windVector);
        }

        private void arrowS_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateAllArrows((Image)sender);
            Vector windVector = new Vector(0.0, -1.0);
            windowManager.UpdateWindVector(windVector);
        }

        private void arrowSW_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateAllArrows((Image)sender);
            Vector windVector = new Vector(-1.0, -1.0);
            windowManager.UpdateWindVector(windVector);
        }

        private void arrowSE_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateAllArrows((Image)sender);
            Vector windVector = new Vector(1.0, -1.0);
            windowManager.UpdateWindVector(windVector);
        }

        private void buildingPeopleCountField_TextChanged(object sender, TextChangedEventArgs e)
        {
            int count = Convert.ToInt32(((TextBox)sender).Text);
            windowManager.UpdateBuildingPeopleCount(count);
        }

        private void openAirPeopleCountField_TextChanged(object sender, TextChangedEventArgs e)
        {
            int count = Convert.ToInt32(((TextBox)sender).Text);
            windowManager.UpdateOpenAirPeopleCount(count);
        }

        private void buildingSafetyPercent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double percent = ((Slider)sender).Value;
            windowManager.UpdateBuildingSafetyPeoplePercent(percent);
        }

        private void openAirSafetyPercent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double percent = ((Slider)sender).Value;
            windowManager.UpdateOpenAirSafetyPeoplePercent(percent);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initSubstanceLoader = new InitSubstanceLoader();
            mapConverter = new MapConverter(40, mapField.ActualWidth, mapField.ActualHeight);
            windowManager = new MainWindowManager(this);
            initializeWindow();
        }
    }
}
