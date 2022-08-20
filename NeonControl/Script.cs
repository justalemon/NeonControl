using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace NeonControl
{
    /// <summary>
    /// The Script that controls the Neon effects.
    /// </summary>
    public class NeonControl : Script
    {
        #region Fields

        private static readonly Dictionary<string, DecoratorType> decorators = new Dictionary<string, DecoratorType>()
        {
            { "neon_start", DecoratorType.Int },
            { "neon_base_r", DecoratorType.Int },
            { "neon_base_g", DecoratorType.Int },
            { "neon_base_b", DecoratorType.Int },
        }; 

        #endregion
        
        #region Constructors

        /// <summary>
        /// Creates a new Neon control script.
        /// </summary>
        public NeonControl()
        {
            Decorators.Initialize();
            Decorators.Register(decorators);
            Tick += OnTick;
        }
        
        #endregion
        
        #region Events

        private void OnTick(object sender, EventArgs e)
        {
        }

        #endregion
    }
}
