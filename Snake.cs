using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace snake
{
  public enum Direction { Up, Down, Right, Left };

  public class Snake
  {
    double thickness = 1.0;

    const double Speed = 2.0;
    
    readonly Vector DRight = new Vector(Speed, 0);
    readonly Vector DLeft = new Vector(-Speed, 0);
    readonly Vector DUp = new Vector(0, -Speed);
    readonly Vector DDown = new Vector(0, Speed);
    
	readonly Polyline _snakeLine;
	
    Vector Dd;

    public Snake(double thickness)
    {
      this.thickness = thickness;
      Dd = DRight;

      _snakeLine = new Polyline();
      _snakeLine.Stroke = Brushes.Black;
      _snakeLine.StrokeThickness = thickness;

      _heading = Direction.Right;
      _snakeLine.Points.Add(new Point(100.0, 200.0));
      _snakeLine.Points.Add(new Point(250.0, 200.0));
    }
      
    public Polyline SnakeLine
    {
      get { return _snakeLine; }
    }

    Direction _heading;
    public Direction Heading
    {
      get { return _heading; }
    }

    public Point Head
    {
      get
      {
        int n = _snakeLine.Points.Count - 1;
        return _snakeLine.Points[n];
      }
    }

    public void NewHeading(Direction dir)
    {
      _heading = dir;

      int n = _snakeLine.Points.Count - 1;
      Point h = _snakeLine.Points[n];
      _snakeLine.Points.Add(h);

      if (dir == Direction.Up) { Dd = DUp; }
      if (dir == Direction.Down) { Dd = DDown; }
      if (dir == Direction.Left) { Dd = DLeft; }
      if (dir == Direction.Right) { Dd = DRight; }
    }

   public void MoveHead()
    {
      int n = _snakeLine.Points.Count - 1;
       _snakeLine.Points[n] += Dd;
    }

    void MoveTail()
    {
      if (_snakeLine.Points[0] == _snakeLine.Points[1])
        _snakeLine.Points.RemoveAt(0);

      double dxTail = Math.Sign(_snakeLine.Points[1].X - _snakeLine.Points[0].X) * Speed;
      double dyTail = Math.Sign(_snakeLine.Points[1].Y - _snakeLine.Points[0].Y) * Speed;
      _snakeLine.Points[0] += new Vector(dxTail, dyTail);
    }

    public void Move()
    {
      MoveHead();
      MoveTail();
    }

    void order(ref double a, ref double b)
    {
      if (a > b)
      {
        double h = a; a = b; b = h;
      }
    }

    private bool HasCollidedWithHorizontalLine(double x1, double x2, double y, double thickness)
    {
      order(ref x1, ref x2);
      if (x1 > x2) MessageBox.Show(x1 + " " + x2);
      return (x1 <= Head.X && Head.X <= x2 && Math.Abs(Head.Y - y) * 2 < this.thickness + thickness);
    }

    private bool HasCollidedWithVerticalLine(double x, double y1, double y2, double thickness)
    {
      order(ref y1, ref y2);
      if (y1 > y2) MessageBox.Show(y1 + " " + y2);
      return (y1 <= Head.Y && Head.Y <= y2 && Math.Abs(Head.X - x) * 2 < this.thickness + thickness);
    }

    public bool HasCollidedWithLine(Line line)
    {
      if (line.X1 == line.X2)
        return HasCollidedWithVerticalLine(line.X1, line.Y1, line.Y2, line.StrokeThickness);
      else
        if (line.Y1 == line.Y2)
          return HasCollidedWithHorizontalLine(line.X1, line.X2, line.Y1, line.StrokeThickness);
        else
            throw new Exception("Sloping line not permitted here");
    }

    bool HasCollidedBetween(Point p1, Point p2)
    {
      if (p1.X == p2.X)
        return HasCollidedWithVerticalLine(p1.X, p1.Y, p2.Y, thickness);
      else
        if (p1.Y == p2.Y)
          return HasCollidedWithHorizontalLine(p1.X, p2.X, p1.Y, thickness);
        else
          throw new Exception("Sloping line not permitted here");
    }

    public bool HasCollidedWithItself()
    {
      for (int n = 0; n < _snakeLine.Points.Count - 4; n++)
      {
        if (HasCollidedBetween(_snakeLine.Points[n], _snakeLine.Points[n + 1]))
        {
          return true;
        }
      }
      return false;
    }
  }
}
