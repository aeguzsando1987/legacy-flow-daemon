using System;

namespace HypertermFlow.Core.Geometry
{
    /// <summary>
    /// Traduce una region logica  + dimensiones reales de pantalla
    /// a una coordenada absoluta. PAra esta prueba esta en 25% / 75% 
    /// en las esquinas, 50% al centro.
    /// Coordenadas absolutas para que en phoenix quede fijo y bloqueado,
    /// con resolucion fija.
    /// </summary>
    public static class CoordinateResolver
    {
        public static ScreenPoint Resolve(ScreenRegion region, ScreenSize size)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentException("Tamaño invalido | altura y ancho deben ser positivos");

            double fx;
            double fy;

            switch (region)
            {
                case ScreenRegion.TopLeft: fx = 0.25; fy = 0.25; break;
                case ScreenRegion.TopRight: fx = 0.75; fy = 0.25; break;
                case ScreenRegion.BottomLeft: fx = 0.25; fy = 0.75; break;
                case ScreenRegion.BottomRight: fx = 0.75; fy = 0.75; break;
                case ScreenRegion.Center: fx = 0.5; fy = 0.5; break;
                default:
                    throw new ArgumentOutOfRangeException("region", region, "Region invalida. No es soportada");
            }

            int x = (int)((size.Width - 1) * fx);
            int y = (int)((size.Height - 1) * fy);
            return new ScreenPoint(x, y);
        }       
    }
}