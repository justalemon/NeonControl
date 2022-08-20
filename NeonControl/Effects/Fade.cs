using System.Drawing;
using GTA;
using GTA.Native;
using GTA.UI;
using Newtonsoft.Json;

namespace NeonControl.Effects
{
    /// <summary>
    /// Fades the color up and down.
    /// </summary>
    public class Fade : Effect
    {
        #region Properties

        /// <summary>
        /// The time it takes to go from dark to light in the fade effect, in milliseconds.
        /// </summary>
        [JsonProperty("time_process_in")]
        public int TimeProcessIn { get; set; } = 1000;
        /// <summary>
        /// The time it takes to go from light to dark in the fade effect, in milliseconds.
        /// </summary>
        [JsonProperty("time_process_out")]
        public int TimeProcessOut { get; set; } = 1000;
        /// <summary>
        /// The time the fade effect will wait once it reaches the maximum configured lightness, in milliseconds.
        /// </summary>
        [JsonProperty("time_hold_in")]
        public int TimeHoldIn { get; set; } = 0;
        /// <summary>
        /// The time the fade effect will wait once it reaches the maximum configured darkness, in milliseconds.
        /// </summary>
        [JsonProperty("time_hold_out")]
        public int TimeHoldOut { get; set; } = 0;
        /// <summary>
        /// The maximum amount of lightness during the fade, between 0 and 1 and can't be over 1 (for example, 0.5 is 50% of lightness and 1.0 is 100%). Please note that the combined values of the Maximum and Minimum can't be over 1.
        /// </summary>
        [JsonProperty("light_max")]
        public float LightMax { get; set; } = 0.5f;
        /// <summary>
        /// The minimum amount of lightness during the fade, between 0 and 1 and can't be over 1 (for example, 0.5 is 50% of lightness and 1.0 is 100%). Please note that the combined values of the Maximum and Minimum can't be over 1.
        /// </summary>
        [JsonProperty("light_min")]
        public float LightMin { get; set; } = 0.3f;

        #endregion
        
        #region Functions
        
        /// <inheritdoc/>
        public override void Initialize() => Decorators.Register("neon_fade_toggle", DecoratorType.Bool);
        /// <inheritdoc/>
        public override void Reset(Vehicle vehicle)
        {
        }
        /// <inheritdoc/>
        public override Color Process(Vehicle vehicle)
        {
            int since = vehicle.GetStart();

            HSLColor color = vehicle.GetBaseColor();
            float calc = LightMax - LightMin;

            if (calc is > 1 or < 0)
            {
                LightMax = 1f;
                LightMin = 0.5f;
                Notification.Show("~o~Warning~s~: The upper and/or lower fade bounds are out of range. They have been reset to avoid crashes.");
            }

            bool goingUp = Function.Call<bool>(Hash.DECOR_GET_BOOL, vehicle, "neon_fade_toggle");

            int timeProcess = goingUp ? TimeProcessIn : TimeProcessOut;
            int timeHold = goingUp ? TimeHoldIn : TimeHoldOut;

            int durationTotalMaximum = timeProcess + timeHold;
            int durationTotalCurrent = Game.GameTime - since;
            int durationColorMaximum = durationTotalMaximum - timeHold;
            int durationColorCurrent = durationTotalCurrent - timeHold;

            float progress = durationTotalCurrent <= timeHold ? 0 : durationColorCurrent / (float) durationColorMaximum;

            if (durationTotalCurrent > durationTotalMaximum)
            {
                goingUp = !goingUp;
                progress = 0;
                Function.Call(Hash.DECOR_SET_BOOL, vehicle, "neon_fade_toggle", goingUp);
                vehicle.SetStart(Game.GameTime);
            }

            float difference = LightMax - LightMin;
            float step = difference * progress;

            if (goingUp)
            {
                color.Lightness = LightMin + step;
            }
            else
            {
                color.Lightness = LightMin + (difference - step);
            }

            return color;
        }

        #endregion
    }
}
