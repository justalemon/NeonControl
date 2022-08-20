using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace NeonControl
{
    /// <summary>
    ///     The main NeonControl configuration.
    /// </summary>
    public class Configuration
    {
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
