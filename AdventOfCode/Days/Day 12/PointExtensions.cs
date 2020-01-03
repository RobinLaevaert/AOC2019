using System.Collections.Generic;
using System.Drawing;

namespace Day_12
{
    public static class PointExtensions
    {
        public static Point CalculateVelocity(this Point point, List<Point> otherPoints)
        {
            otherPoints.ForEach(y =>
            {
                if (y.X > point.X) point.Y++;
                if (y.X < point.X) point.Y--;
            });
            return point;
        }

        public static Point ApplyVelocity(this Point point)
        {
            point.X += point.Y;
            return point;
        }
    }
}
