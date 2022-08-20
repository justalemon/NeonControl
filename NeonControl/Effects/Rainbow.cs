using System.Drawing;
using GTA;
using Newtonsoft.Json;

namespace NeonControl.Effects
{
    /// <summary>
    /// An effect that scrolls thru the entire set of colors.
    /// </summary>
    public class Rainbow : Effect
    {
        #region Properties

        /// <summary>
        /// The duration of the rainbow effect.
        /// </summary>
        [JsonProperty("duration")]
        public int Duration { get; set; } = 10000;

        #endregion
        
        #region Functions

        /// <inheritdoc/>
        public override void Initialize()
        {
        }
        /// <inheritdoc/>
        public override void Reset(Vehicle vehicle)
        {
            HSLColor colorStock = vehicle.GetBaseColor();
            float current = 1f - (colorStock.Hue / 360);
            int since = (int)(Game.GameTime - (Duration * current));
            vehicle.SetStart(since);
        }
        /// <inheritdoc/>
        public override Color Process(Vehicle vehicle)
        {
            int since = vehicle.GetStart();
            HSLColor color = vehicle.GetBaseColor();

            float progress = (since + Duration - Game.GameTime) / (float)Duration;

            if (progress < 0 || progress >= 1)
            {
                progress = 0;
                since = Game.GameTime;
                vehicle.SetStart(since);
            }

            color.Hue = 360 * progress;
            return color;
        }
        
        #endregion
    }
}
