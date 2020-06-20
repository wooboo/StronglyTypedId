using System;
using System.Diagnostics;
using CodeGeneration.Roslyn;

[AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
[CodeGenerationAttribute("StronglyTypedId.Generator.SafeIdGenerator, StronglyTypedId.Generator")]
[Conditional("CodeGeneration")]
public class SafeIdAttribute : Attribute
{
    public string Type { get; }
    public string Version { get; }
    public bool GenerateJsonConverter { get; }
    public StronglyTypedIdJsonConverter JsonConverter { get; }
    public string GeneratorFunction { get; set; } = "NewId";
    public string Separator { get; set; } = "-";

    public SafeIdAttribute(
        string type = "",
        string version = "",
        bool generateJsonConverter = true,
        StronglyTypedIdJsonConverter jsonConverter = StronglyTypedIdJsonConverter.NewtonsoftJson)
    {
        Type = type;
        Version = version;
        GenerateJsonConverter = generateJsonConverter;
        JsonConverter = jsonConverter;
    }
}