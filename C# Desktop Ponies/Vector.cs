namespace CSDesktopPonies
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Defines additional methods that operate on <see cref="T:System.Drawing.Point"/>, <see cref="T:System.Drawing.Size"/>,
    /// <see cref="T:System.Drawing.PointF"/> and <see cref="T:System.Drawing.SizeF"/> treating them as two-dimensional vectors.
    /// </summary>
    public static class Vector
    {
        #region Distance methods
        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="a">First point.</param>
        /// <param name="b">Second point.</param>
        /// <returns>The distance between the two points.</returns>
        public static double Distance(Point a, Point b)
        {
            return Math.Sqrt(DistanceSquared(a, b));
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="a">First point.</param>
        /// <param name="b">Second point.</param>
        /// <returns>The distance between the two points.</returns>
        public static double Distance(PointF a, PointF b)
        {
            return Math.Sqrt(DistanceSquared(a, b));
        }
        #endregion

        #region DistanceSquared methods
        /// <summary>
        /// Calculates the square of the distance between two points.
        /// </summary>
        /// <param name="a">First point.</param>
        /// <param name="b">Second point.</param>
        /// <returns>The square of the distance between the two points.</returns>
        public static int DistanceSquared(Point a, Point b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        /// <summary>
        /// Calculates the square of the distance between two points.
        /// </summary>
        /// <param name="a">First point.</param>
        /// <param name="b">Second point.</param>
        /// <returns>The square of the distance between the two points.</returns>
        public static double DistanceSquared(PointF a, PointF b)
        {
            return (double)(a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }
        #endregion

        #region Length methods
        /// <summary>
        /// Calculates the length of the vector this point represents.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>The distance of this point from the origin.</returns>
        public static double Length(this Point p)
        {
            return Distance(p, Point.Empty);
        }

        /// <summary>
        /// Calculates the length of the vector this point represents.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>The distance of this point from the origin.</returns>
        public static double Length(this PointF p)
        {
            return Distance(p, PointF.Empty);
        }

        /// <summary>
        /// Calculates the length of the vector this size represents.
        /// </summary>
        /// <param name="sz">The size.</param>
        /// <returns>The length of the vector.</returns>
        public static double Length(this Size sz)
        {
            return Distance((Point)sz, Point.Empty);
        }

        /// <summary>
        /// Calculates the length of the vector this size represents.
        /// </summary>
        /// <param name="sz">The size.</param>
        /// <returns>The length of the vector.</returns>
        public static double Length(this SizeF sz)
        {
            return Distance((PointF)sz, Point.Empty);
        }
        #endregion
    }
}
