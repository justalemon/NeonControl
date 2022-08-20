using System.Drawing;
using GTA;

namespace NeonControl.Effects
{
    /// <summary>
    /// Keeps the neon always on.
    /// </summary>
    public class On : Effect
    {
        /// <inheritdoc/>
        public override void Initialize()
        {
        }
        /// <inheritdoc/>
        public override void Reset(Vehicle vehicle)
        {
        }
        /// <inheritdoc/>
        public override Color Process(Vehicle vehicle) => vehicle.GetBaseColor();
    }
}
