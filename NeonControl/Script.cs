using System;
using GTA;
using GTA.Native;

namespace NeonControl
{
    /// <summary>
    /// The Script that controls the Neon effects.
    /// </summary>
    public class NeonControl : Script
    {
        #region Constructors

        /// <summary>
        /// Creates a new Neon control script.
        /// </summary>
        public unsafe NeonControl()
        {
            IntPtr addr = Game.FindPattern("\x40\x53\x48\x83\xEC\x20\x80\x3D\x00\x00\x00\x00\x00\x8B\xDA\x75\x29", "xxxxxxxx????xxxxx");
            if (addr == IntPtr.Zero)
            {
                throw new DataMisalignedException("Memory pattern was not found.");
            }
            
            byte* decoratorLock = (byte*)(addr + *(int*)(addr + 8) + 13);
            *decoratorLock = 0;
            
            Function.Call(Hash.DECOR_REGISTER, "neon_start", 3);

            Function.Call(Hash.DECOR_REGISTER, "neon_base_r", 3);
            Function.Call(Hash.DECOR_REGISTER, "neon_base_g", 3);
            Function.Call(Hash.DECOR_REGISTER, "neon_base_b", 3);
            
            *decoratorLock = 1;
            
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
