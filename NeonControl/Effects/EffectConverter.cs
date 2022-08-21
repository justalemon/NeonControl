using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NeonControl.Effects
{
    /// <summary>
    /// Converts Effects.
    /// </summary>
    public class EffectConverter : JsonConverter<Effect>
    {
        /// <summary>
        /// Writes the Effect to a JSON Writer.
        /// </summary>
        public override void WriteJson(JsonWriter writer, Effect value, JsonSerializer serializer)
        {
            JObject core = new JObject();
            core["type"] = value.GetType().FullName;
            core.Merge(JObject.FromObject(value));
            core.WriteTo(writer);
        }
        /// <summary>
        /// Converts the Effect into it's correct class.
        /// </summary>
        public override Effect ReadJson(JsonReader reader, Type objectType, Effect existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject core = JObject.Load(reader);
            string typeName = (string)core["type"];
            core.Remove("type");

            if (typeName == null)
            {
                throw new InvalidOperationException($"The type cannot be set to null.");
            }

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                throw new InvalidOperationException($"The type '{typeName}' cannot be found.");
            }

            return (Effect)core.ToObject(type);
        }
    }
}
