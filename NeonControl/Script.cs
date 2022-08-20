using System;
using GTA;

namespace NeonControl
{
    public class NeonControl : Script
    {
        #region Constructors

        public NeonControl()
        {
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
