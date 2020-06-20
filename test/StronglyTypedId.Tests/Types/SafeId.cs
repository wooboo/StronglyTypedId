using System;

namespace StronglyTypedId.Tests.Types
{
    [SafeId("AA")]
    partial struct SafeIdWithType
    {
        static string NewId() => Guid.NewGuid().ToString();
    }
    [SafeId("AA", "1")]
    partial struct SafeIdWithTypeAndVersion
    {
        static string NewId() => Guid.NewGuid().ToString();
    }

    [SafeId(GeneratorFunction = "Generate")]
    partial struct SafeIdWithGenerator
    {
        static string Generate() => "HELLO";
    }
    [SafeId()]
    partial struct SafeId
    {
        static string NewId() => Guid.NewGuid().ToString();
    }

    [SafeId(generateJsonConverter: false)]
    public partial struct NoJsonSafeId
    {
        static string NewId() => Guid.NewGuid().ToString();
    }

    [SafeId(jsonConverter: StronglyTypedIdJsonConverter.NewtonsoftJson)]
    public partial struct NewtonsoftJsonSafeId
    {
        static string NewId() => Guid.NewGuid().ToString();
    }

    [SafeId(jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
    public partial struct SystemTextJsonSafeId
    {
        static string NewId() => Guid.NewGuid().ToString();
    }

    [SafeId(jsonConverter: StronglyTypedIdJsonConverter.NewtonsoftJson | StronglyTypedIdJsonConverter.SystemTextJson)]
    public partial struct BothJsonSafeId
    {
        static string NewId() => Guid.NewGuid().ToString();
    }
}