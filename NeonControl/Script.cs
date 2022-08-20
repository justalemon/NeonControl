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

        private static readonly List<Vehicle> knownVehicles = new List<Vehicle>();
        private static readonly Dictionary<string, DecoratorType> decorators = new Dictionary<string, DecoratorType>
        {
            { "neon_start", DecoratorType.Int },
            { "neon_controlled", DecoratorType.Bool },
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
            Tick += OnInit;
        }
        
        #endregion
        
        #region Events

        private void OnInit(object sender, EventArgs e)
        {
            foreach (Vehicle vehicle in World.GetAllVehicles())
            {
                if (vehicle.IsKnown())
                {
                    knownVehicles.Add(vehicle);
                }
            }
            
            Tick -= OnInit;
            Tick += OnTick;
        }
        private void OnTick(object sender, EventArgs e)
        {
            Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;

            if (currentVehicle != null && !knownVehicles.Contains(currentVehicle))
            {
                if (!currentVehicle.IsKnown())
                {
                    currentVehicle.MarkKnown();
                }

                knownVehicles.Add(currentVehicle);
            }
        }

        #endregion
    }
}
