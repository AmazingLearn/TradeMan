using System.Text.Json;
using System.Text.Json.Serialization;
using TradingManBackend.PresentationLayer.Dtos;

namespace TradingManBackend.Util.Json
{
    /// <summary>
    /// Class extending JsonConverter for INotificationDto. Needed to convert containers containing derived objects of INotificationDTO.
    /// This is needed to avoid slicing of data when converting individual objects to Json in response from controller i.e. NotificationConrolloer.
    /// </summary>
    public class NotificationJsonConverter : JsonConverter<INotificationDto>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(INotificationDto).IsAssignableFrom(typeToConvert);
        }

        public override INotificationDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, INotificationDto value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();

            foreach (var property in value.GetType().GetProperties())
            {
                if (!property.CanRead)
                {
                    continue;
                }

                var propertyValue = property.GetValue(value);
                writer.WritePropertyName(property.Name);
                JsonSerializer.Serialize(writer, propertyValue, options);
            }

            writer.WriteEndObject();
        }
    }
}
