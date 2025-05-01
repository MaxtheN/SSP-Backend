using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Core
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter()
        {
            base.DateTimeFormat = Constants.DATE_TIME_FORMAT;
        }
    }

    public class HDateTimeConverter : IsoDateTimeConverter
    {
        public HDateTimeConverter()
        {
            base.DateTimeFormat = Constants.H_DATE_TIME_FORMAT;
        }
    }

    public class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            base.DateTimeFormat = Constants.DATE_FORMAT;
        }
    }

    public class DateTimeMinuteConverter : IsoDateTimeConverter
    {
        public DateTimeMinuteConverter()
        {
            base.DateTimeFormat = Constants.DATE_TIME_MINUTE_FORMAT;
        }
    }

    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public TimeOnlyConverter()
        {
        }

        public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return TimeOnly.ParseExact(reader.Value as string, Constants.TIME_MINUTE_FORMAT, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(Constants.TIME_MINUTE_FORMAT, CultureInfo.InvariantCulture));
        }
    }


    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string DateFormat = Constants.DATE_FORMAT;

        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value.GetType() == typeof(DateTime))
            {
                return DateOnly.FromDateTime((DateTime)reader.Value);
            }
            return DateOnly.ParseExact((string)reader.Value, DateFormat, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }

    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string TimeFormat = Constants.TIME_MINUTE_FORMAT;

        public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return TimeOnly.ParseExact((string)reader.Value, TimeFormat, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(TimeFormat, CultureInfo.InvariantCulture));
        }
    }

    public class DateOnlyNullableJsonConverter : JsonConverter<DateOnly?>
    {
        private const string DateFormat = Constants.DATE_FORMAT;

        public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString()))
                return null;

            if (reader.Value.GetType() == typeof(DateTime))
                return DateOnly.FromDateTime((DateTime)reader.Value);

            return DateOnly.ParseExact((string)reader.Value, DateFormat, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
        {
            writer.WriteValue(value == null ? "null" : value.Value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }

    public class TimeOnlyNullableJsonConverter : JsonConverter<TimeOnly?>
    {
        private const string TimeFormat = Constants.TIME_MINUTE_FORMAT;

        public override TimeOnly? ReadJson(JsonReader reader, Type objectType, TimeOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString()))
                return null;

            return TimeOnly.ParseExact((string)reader.Value, TimeFormat, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, TimeOnly? value, JsonSerializer serializer)
        {
            writer.WriteValue(value == null ? "null" : value.Value.ToString(TimeFormat, CultureInfo.InvariantCulture));
        }
    }

}
