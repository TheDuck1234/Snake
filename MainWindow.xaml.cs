using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace snake{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow{
        public MainWindow(){
            InitializeComponent();
        }

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private Snake _snake;
        private const double Thickness = 3.0;
        private readonly List<Line> _lines = new List<Line>();
        private readonly Random _random = new Random();
        private readonly Rectangle _currentFood = new Rectangle();
        private List<Line> _foods;

        private int _point;
        private int _gSpeed;
        private int _screenWidth;
        private int _screenHeight;

        private void Window_Loaded_1(object sender, RoutedEventArgs e){
            _snake = new Snake(Thickness);
            canvas.Children.Add(_snake.SnakeLine);

            _screenHeight = 293; 
            _screenWidth = 509;
            _currentFood.Height = 10;
            _currentFood.Width = 10;
            _currentFood.Fill = Brushes.LimeGreen;
            _gSpeed = 0;
            _point = 1;

            GenerateBorder();
            FindFood();

            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _timer.Tick += timer_Tick;
            _timer.Start();

        }

        void GenerateBorder(){

            var l1 = new Line();
            l1.X1 = 0.0; l1.Y1 = 0.0; l1.X2 = 0.0; l1.Y2 = 305;
            l1.Stroke = Brushes.MediumBlue;
            l1.StrokeThickness = 2.0;

            var l2 = new Line();
            l2.X1 = 0.0; l2.Y1 = 0.0; l2.X2 = 505; l2.Y2 = 0.0;
            l2.Stroke = Brushes.MediumBlue;
            l2.StrokeThickness = 2.0;

            var l3 = new Line();
            l3.X1 = 505; l3.Y1 = 0.0; l3.X2 = 505; l3.Y2 = 305;
            l3.Stroke = Brushes.MediumBlue;
            l3.StrokeThickness = 2.0;

            var l4 = new Line();
            l4.X1 = 0.0; l4.Y1 = 305; l4.X2 = 505; l4.Y2 = 305;
            l4.Stroke = Brushes.MediumBlue;
            l4.StrokeThickness = 2.0;

            canvas.Children.Add(l1);
            canvas.Children.Add(l2);
            canvas.Children.Add(l3);
            canvas.Children.Add(l4);

            _lines.Add(l1); _lines.Add(l2); _lines.Add(l3); _lines.Add(l4);

        }

        void FindFood(){

            int maxXPos = Convert.ToInt32(_screenWidth - (_currentFood.Width + 1));
            int maxYPos = Convert.ToInt32(_screenHeight - (_currentFood.Height + 1));

            double xPos = _random.Next(1, maxXPos);
            double yPos = _random.Next(1, maxYPos);

            _foods = new List<Line>();
            var line1 = new Line();
            line1.X1 = xPos; line1.Y1 = yPos; line1.X2 = xPos; line1.Y2 = yPos + _currentFood.Height;

            var line2 = new Line();
            line2.X1 = xPos; line2.Y1 = yPos; line2.X2 = xPos + _currentFood.Width; line2.Y2 = yPos;

            var line3 = new Line();
            line3.X1 = xPos; line3.Y1 = yPos + _currentFood.Height; line3.X2 = xPos + _currentFood.Width; line3.Y2 = yPos + _currentFood.Height;

            var line4 = new Line();
            line4.X1 = xPos + _currentFood.Width; line4.Y1 = yPos; line4.X2 = xPos + _currentFood.Width; line4.Y2 = yPos + _currentFood.Height;

            Canvas.SetLeft(_currentFood, xPos);
            Canvas.SetTop(_currentFood, yPos);
            canvas.Children.Add(_currentFood);
           
            _foods.Add(line1);
            _foods.Add(line2);
            _foods.Add(line3);
            _foods.Add(line4);
        }

        public void SnakeMove(){
            if (_gSpeed == 0){
                _snake.Move();
            }
            else{
                _snake.MoveHead();
                _gSpeed--;
            }
        }

        public void CollidedWithFood()
        {
            
            canvas.Children.Remove(_currentFood);
            _foods = new List<Line>();
            _gSpeed = 50;
            _point = _point + _point;
            FindFood();
        }

        public void Dead(){
            _timer.Stop();

            var result = (DialogResult) MessageBox.Show("Wanna try again ?", "Again ?", MessageBoxButton.YesNo);
            switch (result){
                case System.Windows.Forms.DialogResult.Yes:
                    canvas.Children.Remove(_snake.SnakeLine);
                    _snake = new Snake(Thickness);
                    canvas.Children.Add(_snake.SnakeLine);
                    _timer.Start();
                    break;
                case System.Windows.Forms.DialogResult.No:
                    Close();
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e){
            SnakeMove();
            foreach (var line in _foods)
            {
             if (_snake.HasCollidedWithLine(line)){
                 CollidedWithFood();
             }   
            }

            if (_snake.HasCollidedWithItself()){
                Dead();
            }
            foreach (var line in _lines ){
                if (_snake.HasCollidedWithLine(line)){
                    Dead();
                }
            }
            statusTBlck.Text = "Head: (" + _snake.Head.X + "," + _snake.Head.Y + ")";
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e) {
            if (e.Key == Key.Up && _snake.Heading != Direction.Down) {
                _snake.NewHeading(Direction.Up);
            }
            else if (e.Key == Key.Down && _snake.Heading != Direction.Up) {
                _snake.NewHeading(Direction.Down);
            }
            else if (e.Key == Key.Left && _snake.Heading != Direction.Right) {
                _snake.NewHeading(Direction.Left);
            }
            else if (e.Key == Key.Right && _snake.Heading != Direction.Left) {
                _snake.NewHeading(Direction.Right);
            }
        }
    }
}