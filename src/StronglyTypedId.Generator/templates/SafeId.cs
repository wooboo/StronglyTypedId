using System;

[System.ComponentModel.TypeConverter(typeof(SafeIdTypeConverter))]
[Newtonsoft.Json.JsonConverter(typeof(SafeIdNewtonsoftJsonConverter))]
[System.Text.Json.Serialization.JsonConverter(typeof(SafeIdSystemTextJsonConverter))]
readonly partial struct SafeId : System.IComparable<SafeId>, System.IEquatable<SafeId>
{
    public const string Type = "";
    public const string Version = "";
    public const string GeneratorFunction = "";
    public string Value { get; }

    public SafeId(string value)
    {
        if (!value.StartsWith(Type))
        {
            throw new System.InvalidOperationException("Invalid Type prefix");
        }
        Value = value;
    }
    static string NewId() => "";
    public static SafeId New() => Create(NewId());
    public static SafeId Create(string newId) => new SafeId(string.Join(Separator, Type, Version, newId));
    public static readonly SafeId Empty = Create(string.Empty);

    public bool Equals(SafeId other) => this.Value.Equals(other.Value);
    public int CompareTo(SafeId other) => Value.CompareTo(other.Value);

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is SafeId other && Equals(other);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
    public static bool operator ==(SafeId a, SafeId b) => a.CompareTo(b) == 0;
    public static bool operator !=(SafeId a, SafeId b) => !(a == b);

    class SafeIdTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var stringValue = value as string;
            if (!string.IsNullOrEmpty(stringValue))
            {
                return new SafeId(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    class SafeIdNewtonsoftJsonConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(SafeId);
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var id = (SafeId)value;
            serializer.Serialize(writer, id.Value);
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, System.Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return new SafeId(serializer.Deserialize<string>(reader));
        }
    }

    class SafeIdSystemTextJsonConverter : System.Text.Json.Serialization.JsonConverter<SafeId>
    {
        public override SafeId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            return new SafeId(reader.GetString());
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, SafeId value, System.Text.Json.JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}