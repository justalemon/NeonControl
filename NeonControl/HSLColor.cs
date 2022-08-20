using System;
using System.Drawing;

namespace NeonControl
{
    /// <summary>
    /// Represents an HSV Color.
    /// </summary>
    public struct HSLColor
    {
        #region Fields

        private float hue;
        private float saturation;
        private float lightness;

        #endregion

        #region Properties

        /// <summary>
        /// The Hue value.
        /// </summary>
        public float Hue
        {
            get => hue;
            set
            {
                if (value > 360 || value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                hue = value;
            }
        }
        /// <summary>
        /// The Saturation value.
        /// </summary>
        public float Saturation
        {
            get => saturation;
            set
            {
                if (value > 1 || value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                saturation = value;
            }
        }
        /// <summary>
        /// The Lightness value.
        /// </summary>
        public float Lightness
        {
            get => lightness;
            set
            {
                if (value > 1 || value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                lightness = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new HSL Color.
        /// </summary>
        /// <param name="h">The Hue.</param>
        /// <param name="s">The Saturation.</param>
        /// <param name="l">The Lightness.</param>
        public HSLColor(float h, float s, float l)
        {
            hue = 0;
            saturation = 0;
            lightness = 0;

            Hue = h;
            Saturation = s;
            Lightness = l;
        }

        #endregion

        #region Tools

        private static float HueToRgb(float v1, float v2, float vH)
        {
            if (vH < 0)
                vH += 1;

            if (vH > 1)
                vH -= 1;

            if ((6 * vH) < 1)
                return (v1 + (v2 - v1) * 6 * vH);

            if ((2 * vH) < 1)
                return v2;

            if ((3 * vH) < 2)
                return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);

            return v1;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the string representation of the HSL Color.
        /// </summary>
        /// <returns>The string representation of the Color.</returns>
        public override string ToString() => $"HSLColor [H={Hue}, S={Saturation}, L={Lightness}]";

        #endregion

        #region Operators

        public static bool operator !=(HSLColor one, HSLColor two)
        {
            return !(one == two);
        }
        public static bool operator ==(HSLColor one, HSLColor two)
        {
            return one.Hue == two.Hue && one.Saturation == two.Saturation && one.Lightness == two.Lightness;
        }

        #endregion

        #region Converters

        public static implicit operator HSLColor(Color color)
        {
            return new HSLColor(color.GetHue(), color.GetSaturation(), color.GetBrightness());
        }
        public static implicit operator Color(HSLColor color)
        {
            byte r, g, b;

            if (color.Saturation == 0)
            {
                r = g = b = (byte)(color.Lightness * 255);
            }
            else
            {
                float v1, v2;
                float hue = color.Hue / 360;

                v2 = (color.Lightness < 0.5) ? (color.Lightness * (1 + color.Saturation)) : ((color.Lightness + color.Saturation) - (color.Lightness * color.Saturation));
                v1 = 2 * color.Lightness - v2;

                r = (byte)(255 * HueToRgb(v1, v2, hue + (1.0f / 3)));
                g = (byte)(255 * HueToRgb(v1, v2, hue));
                b = (byte)(255 * HueToRgb(v1, v2, hue - (1.0f / 3)));
            }

            return Color.FromArgb(r, g, b);
        }

        #endregion
    }
}
