﻿using System;
using System.Collections.Generic;

namespace Rogue
{
  internal class Point
  {
    public Point(int r, int c)
    {
      Row = r;
      Col = c;
    }

    public int Row { get; set; }
    public int Col { get; set; }
  }

  internal class Line
  {
    public static double DistanceBetweenPoints(Point p1, Point p2)
    {
      double a = (p1.Row - p2.Row);
      double b = (p1.Col - p2.Col);
      return Math.Sqrt(a * a + b * b);
    }

    public static IEnumerable<Point> GetPointsOnLine(Point p1, Point p2)
    {
      int x0 = p1.Row;
      int y0 = p1.Col;
      int x1 = p2.Row;
      int y1 = p2.Col;

      bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
      if (steep)
      {
        int t = x0;
        x0 = y0;
        y0 = t;
        t = x1; // swap x1 and y1
        x1 = y1;
        y1 = t;
      }
      if (x0 > x1)
      {
        int t = x0;
        x0 = x1;
        x1 = t;
        t = y0; // swap y0 and y1
        y0 = y1;
        y1 = t;
      }
      int dx = x1 - x0;
      int dy = Math.Abs(y1 - y0);
      int error = dx / 2;
      int ystep = (y0 < y1) ? 1 : -1;
      int y = y0;
      for (int x = x0; x <= x1; x++)
      {
        yield return new Point((steep ? y : x), (steep ? x : y));
        error = error - dy;
        if (error < 0)
        {
          y += ystep;
          error += dx;
        }
      }
    }
  }
}
