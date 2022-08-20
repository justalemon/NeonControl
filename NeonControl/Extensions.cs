using System.Drawing;
using GTA;
using GTA.Native;

namespace NeonControl
{
    /// <summary>
    /// Some extensions to make working with vehicles easier.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the original color of the neon.
        /// </summary>
        /// <param name="vehicle">The vehicle to get the neon conor from.</param>
        /// <returns>The original neon color.</returns>
        public static Color GetBaseColor(this Vehicle vehicle)
        {
            int r = Function.Call<int>(Hash.DECOR_GET_INT, vehicle, "neon_base_r");
            int g = Function.Call<int>(Hash.DECOR_GET_INT, vehicle, "neon_base_g");
            int b = Function.Call<int>(Hash.DECOR_GET_INT, vehicle, "neon_base_b");
            return Color.FromArgb(r, g, b);
        }
        /// <summary>
        /// Sets the original neon color.
        /// </summary>
        /// <param name="vehicle">The vehicle to set the neon color.</param>
        /// <param name="color">The color to set.</param>
        public static void SetBaseColor(this Vehicle vehicle, Color color)
        {
            Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_base_r", color.R);
            Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_base_g", color.G);
            Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_base_b", color.B);
        }
        
        /// <summary>
        /// Gets the last color used for the Neon effect.
        /// </summary>
        /// <param name="vehicle">The vehicle to set.</param>
        /// <returns>The last color used.</returns>
        public static Color GetLastColor(this Vehicle vehicle)
        {
            int r = Function.Call<int>(Hash.DECOR_GET_INT, vehicle, "neon_last_r");
            int g = Function.Call<int>(Hash.DECOR_GET_INT, vehicle, "neon_last_g");
            int b = Function.Call<int>(Hash.DECOR_GET_INT, vehicle, "neon_last_b");
            return Color.FromArgb(r, g, b);
        }
        /// <summary>
        /// Sets the last color used.
        /// </summary>
        /// <param name="vehicle">The vehicle to set.</param>
        /// <param name="color">The last color to set.</param>
        public static void SetLastColor(this Vehicle vehicle, Color color)
        {
            Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_last_r", color.R);
            Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_last_g", color.G);
            Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_last_b", color.B);
        }
        
        /// <summary>
        /// Gets time the current neon effect started..
        /// </summary>
        /// <param name="vehicle">The vehicle to get.</param>
        /// <returns>The time neon started..</returns>
        public static int GetStart(this Vehicle vehicle) => Function.Call<int>(Hash.DECOR_GET_INT, vehicle, "neon_start");
        /// <summary>
        /// Sets the start time of the neon effect.
        /// </summary>
        /// <param name="vehicle">The vehicle to set.</param>
        /// <param name="start">The </param>
        public static void SetStart(this Vehicle vehicle, int start) => Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_start", start);
        
        /// <summary>
        /// If the vehicle is known by the mod.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <returns>True if the vehicle is known by the script, false otherwise.</returns>
        public static bool IsKnown(this Vehicle vehicle) => Function.Call<bool>(Hash.DECOR_GET_BOOL, vehicle, "neon_known");
        /// <summary>
        /// Marks a specific vehicle as known.
        /// </summary>
        /// <param name="vehicle">The vehicle to mark.</param>
        public static void MarkKnown(this Vehicle vehicle) => Function.Call<bool>(Hash.DECOR_SET_BOOL, vehicle, $"neon_known", true);
        
        /// <summary>
        /// Gets the index of the currently set effect.
        /// </summary>
        /// <param name="vehicle">The vehicle to get.</param>
        /// <returns>The index of the vehicle effect.</returns>
        public static int GetEffect(this Vehicle vehicle) => Function.Call<int>(Hash.DECOR_GET_INT, vehicle, "neon_effect");
        /// <summary>
        /// Sets the index of a specific vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to set.</param>
        /// <param name="effect">The effect to set.</param>
        public static void SetEffect(this Vehicle vehicle, int effect) => Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_effect", (int)effect);
        
        /// <summary>
        /// Checks if the vehicle has neon enabled or not.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <returns>true if the neon is enabled, false otherwise.</returns>
        public static bool IsEnabled(this Vehicle vehicle) => Function.Call<bool>(Hash.DECOR_GET_BOOL, vehicle, "neon_enabled");
        /// <summary>
        /// Sets the activation of the neon effect
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <param name="activation">The activation to set.</param>
        public static void SetActivation(this Vehicle vehicle, bool activation) => Function.Call<bool>(Hash.DECOR_SET_BOOL, vehicle, $"neon_enabled", activation);
        
        /// <summary>
        /// Gets the time when the engine was turned on.
        /// </summary>
        /// <param name="vehicle">The vehicle to check.</param>
        /// <returns>The last time the engine was turned on.</returns>
        public static int GetEngineTime(this Vehicle vehicle) => Function.Call<int>(Hash.DECOR_GET_INT, vehicle, "neon_engine_time");
        /// <summary>
        /// Invalidates the last time the engine was on.
        /// </summary>
        /// <param name="vehicle">The vehicle to invalidate.</param>
        public static void InvalidateEngineTime(this Vehicle vehicle) => Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_engine_time", -1);
        /// <summary>
        /// Updates the engine time.
        /// </summary>
        /// <param name="vehicle">The vehicle to update.</param>
        public static void UpdateEngineTime(this Vehicle vehicle) => Function.Call<bool>(Hash.DECOR_SET_INT, vehicle, "neon_engine_time", Game.GameTime);
    }
}
