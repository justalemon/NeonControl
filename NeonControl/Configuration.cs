using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using Control = GTA.Control;

namespace NeonControl
{
    /// <summary>
    ///     The main NeonControl configuration.
    /// </summary>
    public class Configuration
    {
        #region Properties

        /// <summary>
        /// When using a Gamepad: The first Control that will need to be pressed to toggle the neon on and off and change the effect. You need to press both to be picked up.
        /// </summary>
        [JsonProperty("control_gamepad_1")]
        public Control ControlGamepad1 { get; set; } = Control.FrontendAccept;
        /// <summary>
        /// When using a Gamepad: The second Control that will need to be pressed to toggle the neon on and off and change the effect. You need to press both to be picked up.
        /// </summary>
        [JsonProperty("control_gamepad_2")]
        public Control ControlGamepad2 { get; set; } = Control.FrontendAccept;
        /// <summary>
        /// When using a Gamepad: The time you will need to hold both Gamepad controls to change the neon mode in milliseconds. If you hold the buttons under the time, it will toggle neon on and off, but if you hold them over the time, they will change the mode. 
        /// </summary>
        [JsonProperty("control_gamepad_hold")]
        public int ControlGamepadHold { get; set; } = 150;

        /// <summary>
        /// When using a Keyboard: A shared key that will toggle on and off and change the mode (the same behavior as a Gamepad).
        /// </summary>
        [JsonProperty("control_keyboard_single")]
        public Keys ControlKeyboardSingle { get; set; } = Keys.N;
        /// <summary>
        /// When using a Keyboard: The dedicated key that will be used to toggle neon on and off.
        /// </summary>
        [JsonProperty("control_keyboard_toggle")]
        public Keys ControlKeyboardToggle { get; set; } = Keys.NumPad0;
        /// <summary>
        /// When using a Keyboard: The dedicated key that will be used to change the neon mode.
        /// </summary>
        [JsonProperty("control_keyboard_mode")]
        public Keys ControlKeyboardMode { get; set; } = Keys.Decimal;
        /// <summary>
        /// When using a Keyboard: The time you will need press the single key to change the neon mode in milliseconds. If you hold the buttons under the time, it will toggle neon on and off, but if you hold them over the time, they will change the mode.
        /// </summary>
        [JsonProperty("control_keyboard_hold")]
        public int ControlKeyboardHold { get; set; } = 150;
        
        #endregion
        
        #region Functions

        /// <summary>
        /// Loads up the current configuration file, or creates a new one if none exist.
        /// </summary>
        /// <returns>The loaded or new Configuration.</returns>
        public static Configuration Load()
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name;
            string path = $"scripts\\{name}\\Configuration.json";

            if (File.Exists(path))
            {
                string existingContents = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<Configuration>(existingContents);
            }

            Configuration newConfig = new Configuration();
            string newContents = JsonConvert.SerializeObject(newConfig);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, newContents);
            return newConfig;
        }

        #endregion
    }
}
