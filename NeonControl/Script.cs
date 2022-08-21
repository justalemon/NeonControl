using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Native;
using LemonUI;
using NeonControl.Effects;

namespace NeonControl
{
    /// <summary>
    /// The Script that controls the Neon effects.
    /// </summary>
    public class NeonControl : Script
    {
        #region Fields

        private readonly ObjectPool pool = new ObjectPool();
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
        private Configuration config = Configuration.Load();
        private int pressedSince = -1;
        private bool clearOnceLifted = false;

        #endregion
        
        #region Constructors

        /// <summary>
        /// Creates a new Neon control script.
        /// </summary>
        public NeonControl()
        {
            Decorators.Initialize();
            
            Tick += OnInit;
            Aborted += OnAborted;
        }

        #endregion
        
        #region Events

        private void OnInit(object sender, EventArgs e)
        {
            Decorators.Register(decorators);

            foreach (Effect effect in effects)
            {
                effect.Initialize();
            }
            
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
            pool.Process();
            
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

            InputMethod inputMethod = Game.LastInputMethod;
            
            foreach (Vehicle vehicle in knownVehicles)
            {
                int room = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, vehicle);
                if (losSantosCustoms.Contains(room))
                {
                    Color color = vehicle.GetBaseColor();
                    vehicle.Mods.NeonLightsColor = color;
                    vehicle.SetLastColor(color);
                    continue;
                }
                
                if (currentVehicle == vehicle &&
                    !(pool.AreAnyVisible && inputMethod == InputMethod.GamePad) &&
                    Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) != 0)
                {
                    bool changeActivation = false;
                    bool changeEffect = false;

                    if (inputMethod == InputMethod.GamePad)
                    {
                        Game.DisableControlThisFrame(Control.Duck);
                        Game.DisableControlThisFrame(Control.VehicleDuck);
                        Game.DisableControlThisFrame(Control.VehicleFlyDuck);
                        Game.EnableControlThisFrame(config.ControlGamepad1);
                        Game.EnableControlThisFrame(config.ControlGamepad2);
                        Function.Call(Hash.SET_INPUT_EXCLUSIVE, 0, (int)config.ControlGamepad1);
                        Function.Call(Hash.SET_INPUT_EXCLUSIVE, 0, (int)config.ControlGamepad2);

                        if (pressedSince == -1 && Game.IsControlPressed(config.ControlGamepad1) &&
                            Game.IsControlJustPressed(config.ControlGamepad2))
                        {
                            pressedSince = Game.GameTime;
                        }
                        else if (pressedSince != -1)
                        {
                            if (!Game.IsControlPressed(config.ControlGamepad1) &&
                                !Game.IsControlPressed(config.ControlGamepad2))
                            {
                                changeActivation = true;
                                pressedSince = -1;
                            }
                            else if (Game.GameTime - pressedSince > config.ControlGamepadHold)
                            {
                                changeEffect = true;
                                pressedSince = -1;
                            }
                        }
                    }
                    else if (inputMethod == InputMethod.MouseAndKeyboard)
                    {
                        bool single = Game.IsKeyPressed(config.ControlKeyboardSingle);
                        bool mode = Game.IsKeyPressed(config.ControlKeyboardMode);
                        bool toggle = Game.IsKeyPressed(config.ControlKeyboardToggle);

                        if (clearOnceLifted && !single && !mode && !toggle)
                        {
                            clearOnceLifted = false;
                            pressedSince = -1;
                        }

                        if (!clearOnceLifted)
                        {
                            if (pressedSince == -1)
                            {
                                if (single)
                                {
                                    pressedSince = Game.GameTime;
                                }
                                else if (mode)
                                {
                                    changeEffect = true;
                                    pressedSince = Game.GameTime;
                                    clearOnceLifted = true;
                                }
                                else if (toggle)
                                {
                                    changeActivation = true;
                                    pressedSince = Game.GameTime;
                                    clearOnceLifted = true;
                                }
                            }
                            else
                            {
                                if (!single)
                                {
                                    changeActivation = true;
                                    clearOnceLifted = true;
                                }
                                else if (Game.GameTime - pressedSince > config.ControlKeyboardHold)
                                {
                                    changeEffect = true;
                                    clearOnceLifted = true;
                                }
                            }
                        }
                    }

                    if (changeActivation)
                    {
                        bool newActivation = !vehicle.IsEnabled();
                        vehicle.SetActivation(newActivation);
                        GTA.UI.Screen.ShowSubtitle($"Neon has been set {(newActivation ? "~g~On" : "~r~Off")}~s~!");
                    }
                    else if (changeEffect && vehicle.IsEnabled())
                    {
                        int currentIndex = vehicle.GetEffect();
                        int newIndex = currentIndex >= effects.Count - 1 ? 0 : currentIndex + 1;
                        Effect newEffect = effects[newIndex];
                        vehicle.SetEffect(newIndex);
                        GTA.UI.Screen.ShowSubtitle($"Neon mode was set to ~q~{newEffect.GetType().Name} ({newIndex})~s~!");
                    }
                }

                if (config.ResetOnEngineOff)
                {
                    int engineTime = vehicle.GetEngineTime();
                    
                    if (vehicle.IsEngineRunning)
                    {
                        if (engineTime == -1)
                        {
                            vehicle.SetEffect(config.DefaultEffect);

                            if (!vehicle.IsEnabled() && config.ToggleOnWhenEngineIsOn)
                            {
                                vehicle.SetActivation(true);
                            }
                        }
                        
                        vehicle.UpdateEngineTime();
                    }
                    else if (!vehicle.IsEngineRunning && Game.GameTime - engineTime > 1500)
                    {
                        vehicle.InvalidateEngineTime();
                        continue;
                    }
                }
                
                if (!vehicle.IsEnabled())
                {
                    vehicle.Mods.NeonLightsColor = Color.Black;
                    continue;
                }

                if (vehicle.Mods.NeonLightsColor != vehicle.GetLastColor())
                {
                    vehicle.SetBaseColor(vehicle.Mods.NeonLightsColor);
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
        private void OnAborted(object sender, EventArgs e)
        {
            foreach (Vehicle vehicle in knownVehicles)
            {
                Color color = vehicle.GetBaseColor();
                vehicle.Mods.NeonLightsColor = color;
                vehicle.SetLastColor(color);
            }
        }

        #endregion
    }
}
