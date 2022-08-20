using System.Drawing;
using GTA;
using GTA.Native;
using Newtonsoft.Json;

namespace NeonControl.Effects
{
    /// <summary>
    /// Blinks the Neon, on and off.
    /// </summary>
    public class Blink : Effect
    {
        #region Properties
        
        /// <summary>
        /// The time between the neon turns on and off when using the On/Off effect, in milliseconds.
        /// </summary>
        [JsonProperty("time_between")]
        public int TimeBetween { get; set; } = 1000;
        
        #endregion
        
        #region Functions
        
        /// <inheritdoc/>
        public override void Initialize() => Decorators.Register("neon_onoff_toggle", DecoratorType.Bool);

        /// <inheritdoc/>
        public override void Reset(Vehicle vehicle)
        {
        }
        /// <inheritdoc/>
        public override Color Process(Vehicle vehicle)
        {
            HSLColor color = vehicle.GetBaseColor();
            int start = vehicle.GetStart();
            bool activation = Function.Call<bool>(Hash.DECOR_GET_BOOL, vehicle, "neon_onoff_toggle");

            if (start + TimeBetween < Game.GameTime)
            {
                activation = !activation;
                Function.Call<bool>(Hash.DECOR_SET_BOOL, vehicle, "neon_onoff_toggle", activation);
                vehicle.SetStart(Game.GameTime);
            }

            color.Saturation = activation ? 1 : 0;
            color.Lightness = activation ? color.Lightness : 0;

            return color;
        }
        
        #endregion
    }
}
