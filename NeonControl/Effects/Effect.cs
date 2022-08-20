using System.Drawing;
using GTA;

namespace NeonControl.Effects
{
    /// <summary>
    /// Represents
    /// </summary>
    public abstract class Effect
    {
        #region Properties

        /// <summary>
        /// If the effect is enabled or not.
        /// </summary>
        public bool Enabled { get; set; }

        #endregion
        
        #region Functions

        /// <summary>
        /// Initializes the Effect requirements.
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Resets the effect of the vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to reset.</param>
        public abstract void Reset(Vehicle vehicle);
        /// <summary>
        /// Processes the status of the vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to process.</param>
        /// <returns>The color that was obtained from the effect.</returns>
        public abstract Color Process(Vehicle vehicle);

        #endregion
    }
}
