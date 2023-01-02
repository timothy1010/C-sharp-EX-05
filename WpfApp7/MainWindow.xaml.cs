using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp7
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        Point start, dest;
        Color strokeColor = Colors.Red;
        Color fillColor = Colors.Yellow;
        Brush currentStrokBrush;
        Brush currentFillBrush;
        int strokeThickness = 3;
        string shapeType = "Line";
        string actionType = "Draw";
        public MainWindow()
        {
            InitializeComponent();
            strokeColorPicker.SelectedColor = strokeColor;
            fillColorPicker.SelectedColor = fillColor;
        }
        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(myCanvas);

            if (actionType == "Draw")
            {
                myCanvas.Cursor = Cursors.Cross;
                switch (shapeType)
                {
                    case "Line":
                        DrawLine(Brushes.Gray, 1);
                        break;
                    case "Rectangle":
                        DrawRectangle(Brushes.Gray, Brushes.LightGray, 1);
                        break;
                    case "Ellipse":
                        DrawEllipase(Brushes.Gray, Brushes.LightGray, 1);
                        break;
                    case "Polyline":
                        DrawPolyline(Brushes.Gray, Brushes.LightGray, 1);
                        break;
                }
            }
            DisplayStatus();
        }
        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            dest = e.GetPosition(myCanvas);
            if (actionType == "Erase")
            {
                Shape eraseShape = e.OriginalSource as Shape;
                myCanvas.Children.Remove(eraseShape);
                if(myCanvas.Children.Count == 0)myCanvas.Cursor = Cursors.Arrow;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                switch (shapeType)
                {
                    case "Line":
                        var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                        line.X2 = dest.X;
                        line.Y2 = dest.Y;
                        break;
                    case "Rectangle":
                        var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                        UpdateShape(rect);
                        break;
                    case "Ellipse":
                        var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                        UpdateShape(ellipse);
                        break;
                    case "Polyline":
                        var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault(); 
                        PointCollection pointCollection = polyline.Points;
                        pointCollection.Add(dest);
                        break;
                }
            }
            DisplayStatus();
        }
        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (actionType == "Draw")
            {
                switch (shapeType)
                {
                    case "Line":
                        var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                        UpdateShape(line, currentStrokBrush, strokeThickness);
                        break;
                    case "Rectangle":
                        var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                        UpdateShape(rect, currentStrokBrush, currentStrokBrush, strokeThickness);
                        break;
                    case "Ellipse":
                        var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                        UpdateShape(ellipse, currentStrokBrush, currentStrokBrush, strokeThickness);
                        break;
                    case "Polyline":
                        var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                        UpdateShape(polyline, currentStrokBrush, currentStrokBrush, strokeThickness);
                        break;
                }
                myCanvas.Cursor = Cursors.Arrow;
            }
            DisplayStatus();
        }
        private void UpdateShape(Shape shape)
        {
            //Point origin;
            Point origin = new Point();

            origin.X = Math.Min(start.X, dest.X);
            origin.Y = Math.Min(start.Y, dest.Y);
            double width = Math.Abs(dest.X - start.X);
            double height = Math.Abs(dest.Y - start.Y);

            shape.Width = width;
            shape.Height = height;
            shape.SetValue(Canvas.LeftProperty, origin.X);
            shape.SetValue(Canvas.TopProperty, origin.Y);
        }
        private void UpdateShape(Shape shape, Brush stroke, int thickckness)
        {
            shape.Stroke = stroke;
            shape.StrokeThickness = thickckness;
        }
        private void UpdateShape(Shape shape, Brush stroke, Brush fill, int thickness)
        {
            shape.Stroke = stroke;
            shape.Fill = fill;
            shape.StrokeThickness = thickness;
        }
        private void DrawPolyline(Brush stroke, Brush fill, int thickness)
        {
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(start);
            Polyline polyline = new Polyline()
            {
                Stroke = stroke,
                Fill = fill,
                StrokeThickness = thickness,
                Points = myPointCollection
            };
            myCanvas.Children.Add(polyline);
        }
        private void DrawEllipase(Brush stroke, Brush fill, int thickness)
        {
            Ellipse ellipase = new Ellipse()
            {
                Stroke = stroke,
                Fill = fill,
                StrokeThickness = thickness
            };
            UpdateShape(ellipase);
            myCanvas.Children.Add(ellipase);
        }
        private void DrawRectangle(Brush stroke, Brush fill, int thickness)
        {
            Rectangle rect = new Rectangle()
            {
                Stroke = stroke,
                Fill = fill,
                StrokeThickness = thickness,
            };
            UpdateShape(rect);
            myCanvas.Children.Add(rect);
        }
        private void DrawLine(Brush stroke, int thickness)
        {
            //Line myLine = new Line();
            //myLine.Stroke = curentStrokBrush;
            //myLine.X1 = start.X;
            //myLine.Y1 = start.Y;
            //myLine.X2 = dest.X;
            //myLine.Y2 = dest.Y;
            //myLine.StrokeThickness = 3.0;
            Line line = new Line()
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = dest.X,
                Y2 = dest.Y,
                StrokeThickness = thickness,
                Stroke = stroke,
            };
            myCanvas.Children.Add(line);

        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton targetReadioButton = sender as RadioButton;
            shapeType = targetReadioButton.Tag.ToString();
            actionType = "Draw";
        }
        private void StrokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = (Color)e.NewValue;
            currentStrokBrush = new SolidColorBrush(strokeColor);
        }
        private void FillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = (Color)e.NewValue;
            currentStrokBrush = new SolidColorBrush(fillColor);
        }
        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = (int)e.NewValue;
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            actionType = "Draw";
        }
        private void EraserButton_Click(object sender, RoutedEventArgs e)
        {
                actionType = "Erase";
                if (myCanvas.Children.Count != 0)
                {
                    myCanvas.Cursor = Cursors.Hand;
                }
        }
        private void DisplayStatus()
        {
            int lineCount = myCanvas.Children.OfType<Line>().Count();
            int rectangleCount = myCanvas.Children.OfType<Rectangle>().Count();
            int ellipseCount = myCanvas.Children.OfType<Ellipse>().Count();
            int polylineCount = myCanvas.Children.OfType<Polyline>().Count();

            coordinteLabel.Content = $"座標點:({Math.Round(start.X)},{Math.Round(start.Y)}):({Math.Round(dest.X)},{Math.Round(dest.Y)})";
            shapeLabel.Content = $"Line: {lineCount},Rectangle:{rectangleCount},Ellipse:{ellipseCount},PolyLine:{polylineCount}";
        }
        /*private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton targetReadioButton = sender as RadioButton;
            shapeType = targetReadioButton.Content.ToString();
        }*/
    }
}
