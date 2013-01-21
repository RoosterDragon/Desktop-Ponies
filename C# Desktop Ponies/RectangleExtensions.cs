namespace CSDesktopPonies
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Defines extensions methods for <see cref="T:System.Drawing.Rectangle"/> and <see cref="T:System.Drawing.RectangleF"/>.
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// Gets the minimal <see cref="T:System.Drawing.Rectangle"/> which covers the same area as a
        /// <see cref="T:System.Drawing.RectangleF"/> structure.
        /// </summary>
        /// <param name="r">The <see cref="T:System.Drawing.RectangleF"/> whose bounds should be found.</param>
        /// <returns>A <see cref="T:System.Drawing.Rectangle"/> whose top-left coordinates are the truncation of the top-left coordinates
        /// of the <see cref="T:System.Drawing.RectangleF"/> structure and whose bottom-right coordinates are the ceiling of the
        /// bottom-right coordinates of the <see cref="T:System.Drawing.RectangleF"/> structure.</returns>
        public static Rectangle BoundingRectangle(this RectangleF r)
        {
            return Rectangle.FromLTRB((int)r.Left, (int)r.Top, (int)Math.Ceiling(r.Right), (int)Math.Ceiling(r.Bottom));
        }

        /// <summary>
        /// Gets the coordinates of the center of a <see cref="T:System.Drawing.Rectangle"/> structure.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <returns>The coordinates of the center of a <see cref="T:System.Drawing.Rectangle"/> structure.</returns>
        public static PointF Center(this Rectangle r)
        {
            return new PointF(r.Left + r.Width / 2f, r.Top + r.Height / 2f);
        }

        /// <summary>
        /// Gets the coordinates of the center of a <see cref="T:System.Drawing.RectangleF"/> structure.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <returns>The coordinates of the center of a <see cref="T:System.Drawing.RectangleF"/> structure.</returns>
        public static PointF Center(this RectangleF r)
        {
            return new PointF(r.Left + r.Width / 2f, r.Top + r.Height / 2f);
        }
    }
}
