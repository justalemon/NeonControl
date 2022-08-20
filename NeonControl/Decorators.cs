using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace NeonControl
{
    /// <summary>
    /// The type of decorator to create.
    /// </summary>
    public enum DecoratorType
    {
        /// <summary>
        /// <see cref="System.Single"/>.
        /// </summary>
        Float = 1,
        /// <summary>
        /// <see cref="System.Boolean"/>.
        /// </summary>
        Bool = 2,
        /// <summary>
        /// <see cref="System.Int32"/>.
        /// </summary>
        Int = 3,
        /// <summary>
        /// <see cref="System.TimeSpan"/>.
        /// </summary>
        Time = 5
    }

    /// <summary>
    /// Class used to manage the decorators.
    /// </summary>
    public static unsafe class Decorators
    {
        #region Fields

        private static bool ready = false;
        private static byte* pointer;
        
        #endregion
        
        #region Functions

        private static void EnsureReady()
        {
            if (!ready)
            {
                throw new InvalidOperationException("Decorator system is not initialized.");
            }
        }
        /// <summary>
        /// Initializes the decorator system.
        /// </summary>
        public static void Initialize()
        {
            IntPtr addr = Game.FindPattern("\x40\x53\x48\x83\xEC\x20\x80\x3D\x00\x00\x00\x00\x00\x8B\xDA\x75\x29", "xxxxxxxx????xxxxx");
            if (addr == IntPtr.Zero)
            {
                throw new DataMisalignedException("Memory pattern was not found.");
            }
            
            pointer = (byte*)(addr + *(int*)(addr + 8) + 13);
            ready = true;
        }
        /// <summary>
        /// Registers a new decorator.
        /// </summary>
        /// <param name="decorator">The name of the decorator to register.</param>
        /// <param name="type">The type of decorator.</param>
        public static void Register(string decorator, DecoratorType type)
        {
            EnsureReady();

            *pointer = 0;
            Function.Call(Hash.DECOR_REGISTER, decorator, (int) type);
            *pointer = 1;
        }
        /// <summary>
        /// Registers a set of decorators.
        /// </summary>
        /// <param name="decorators"></param>
        public static void Register(Dictionary<string, DecoratorType> decorators)
        {
            EnsureReady();

            *pointer = 0;
            foreach (KeyValuePair<string,DecoratorType> decorator in decorators)
            {
                Function.Call(Hash.DECOR_REGISTER, decorator.Key, (int)decorator.Value);
            }
            *pointer = 1;
        }
        
        #endregion
    }
}
