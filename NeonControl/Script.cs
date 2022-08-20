using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Native;
using NeonControl.Effects;

namespace NeonControl
{
    /// <summary>
    /// The Script that controls the Neon effects.
    /// </summary>
    public class NeonControl : Script
    {
        #region Fields

        private readonly List<int> losSantosCustoms = new List<int>()
        {
            2044753180,
            -122296439,
            1204347848
        };
        private static readonly List<Vehicle> knownVehicles = new List<Vehicle>();
        private static readonly Dictionary<string, DecoratorType> decorators = new Dictionary<string, DecoratorType>()
        {
            { "neon_enabled", DecoratorType.Bool },
            { "neon_start", DecoratorType.Int },
            { "neon_known", DecoratorType.Bool },
            { "neon_effect", DecoratorType.Int },
            { "neon_base_r", DecoratorType.Int },
            { "neon_base_g", DecoratorType.Int },
            { "neon_base_b", DecoratorType.Int },
            { "neon_last_r", DecoratorType.Int },
            { "neon_last_g", DecoratorType.Int },
            { "neon_last_b", DecoratorType.Int }
        };
        private static readonly List<Effect> effects = new List<Effect>
        {
            new On(),
            new Blink(),
            new Fade(),
            new Rainbow()
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
                    currentVehicle.SetActivation(true);
                    currentVehicle.MarkKnown();
                }

                knownVehicles.Add(currentVehicle);
            }
            
            foreach (Vehicle vehicle in knownVehicles)
            {
                if (!vehicle.IsEnabled())
                {
                    vehicle.Mods.NeonLightsColor = Color.Black;
                    continue;
                }

                if (vehicle.Mods.NeonLightsColor != vehicle.GetLastColor())
                {
                    vehicle.SetBaseColor(vehicle.Mods.NeonLightsColor);
                }

                int room = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, vehicle);
                if (losSantosCustoms.Contains(room))
                {
                    Color color = vehicle.GetBaseColor();
                    vehicle.Mods.NeonLightsColor = color;
                    vehicle.SetLastColor(color);
                    continue;
                }

                int effectIndex = vehicle.GetEffect();

                if (effectIndex >= effects.Count)
                {
                    effectIndex = 0;
                    vehicle.SetEffect(effectIndex);
                }

                Effect effect = effects[effectIndex];
                Color currentColor = effect.Process(vehicle);

                vehicle.Mods.NeonLightsColor = currentColor;
                vehicle.SetLastColor(currentColor);
            }
        }

        #endregion
    }
}
