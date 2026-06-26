using System;

namespace HypertermFlow.Core.Geometry
{
    /// <summary>
    /// Regiones objetivo de la secuencia (para probar: 4 esquinas y centro)
    /// </summary>
    public enum ScreenRegion { TopLeft, TopRight, BottomLeft, BottomRight, Center }

    /// <summary>
    /// Dimensiones de pantalla en pixeles
    /// </summary>
    public struct ScreenSize
    {
        public readonly int Width;
        public readonly int Height;
        public ScreenSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public override string ToString()
        {
            return Width + "x" + Height;
        }
    }

    /// <summary>
    /// Coordenada absoluta de pantalla
    /// </summary>
    public struct ScreenPoint
    {
        public readonly int X;
        public readonly int Y;
        public ScreenPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
}
