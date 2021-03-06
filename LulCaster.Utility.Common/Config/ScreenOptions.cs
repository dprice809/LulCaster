﻿using System.Drawing;

namespace LulCaster.Utility.Common.Config
{
  public class ScreenOptions
  {
    public ScreenSelection ScreenSelection { get; set; }
    public int ScreenHeight { get; set; }
    public int ScreenWidth { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Rectangle GetBoundsAsRectangle()
    {
      return new Rectangle
      {
        X = this.X,
        Y = this.Y,
        Width = this.ScreenWidth,
        Height = this.ScreenHeight
      };
    }
  }
}