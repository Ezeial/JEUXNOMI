using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace YIKES.Class
{
    static class RectangleUtils
    {
        public static Vector2 GetIntersectionDepth(Rectangle rectangleA, Rectangle rectangleB)
        {
            float halfWidthA = rectangleA.Width / 2.0f;
            float halfHeightA = rectangleA.Height / 2.0f;
            float halfWidthB = rectangleB.Width / 2.0f;
            float halfHeightB = rectangleB.Height / 2.0f;

            Vector2 centerA = rectangleA.Center.ToVector2();
            Vector2 centerB = rectangleB.Center.ToVector2();

            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }
    }
}