using System;
using StronglyTypedId.Tests.Types;
using Xunit;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonConvert;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

namespace StronglyTypedId.Tests
{

    public class SafeIdTests
    {
        [Fact]
        public void MatchingType_Accepts()
        {
            var id = "AA-some-value";
            var foo1 = new SafeIdWithType(id);

            Assert.Equal(foo1.Value, id);
        }

        [Fact]
        public void Generated_Accepts()
        {
            var foo1 = SafeIdWithType.New();
            var foo2 = new SafeIdWithType(foo1.Value);

            Assert.Equal(foo1, foo2);
        }


        [Fact]
        public void GeneratedWithVersion_Accepts()
        {
            var foo1 = SafeIdWithTypeAndVersion.New();
            var foo2 = new SafeIdWithTypeAndVersion(foo1.Value);

            Assert.Equal(foo1, foo2);
        }
        [Fact]
        public void NotMatchingType_Throws()
        {
            var id = "some-value";

            Assert.Throws<InvalidOperationException>(() => new SafeIdWithType(id));
        }

        [Fact]
        public void New_GeneratesWithProvidedFunction()
        {
            var foo1 = SafeIdWithGenerator.New();

            Assert.Equal("HELLO", foo1.Value);
        }


        [Fact]
        public void SameValuesAreEqual()
        {
            var id = "some-value";
            var foo1 = new SafeId(id);
            var foo2 = new SafeId(id);

            Assert.Equal(foo1, foo2);
        }

        [Fact]
        public void EmptyValueIsEmpty()
        {
            Assert.Equal(SafeId.Empty.Value, string.Empty);
        }


        [Fact]
        public void DifferentValuesAreUnequal()
        {
            var foo1 = new SafeId("value1");
            var foo2 = new SafeId("value2");

            Assert.NotEqual(foo1, foo2);
        }

        [Fact]
        public void OverloadsWorkCorrectly()
        {
            var id = "some-value";
            var same1 = new SafeId(id);
            var same2 = new SafeId(id);
            var different = new SafeId("other value");

            Assert.True(same1 == same2);
            Assert.False(same1 == different);
            Assert.False(same1 != same2);
            Assert.True(same1 != different);
        }

        [Fact]
        public void DifferentTypesAreUnequal()
        {
            var bar = GuidId2.New();
            var foo = new SafeId("Value");

            //Assert.NotEqual(bar, foo); // does not compile
            Assert.NotEqual((object)bar, (object)foo);
        }

        [Fact]
        public void CanSerializeToString_WithNewtonsoftJsonProvider()
        {
            var foo = new NewtonsoftJsonSafeId("123");

            var serializedFoo = NewtonsoftJsonSerializer.SerializeObject(foo);
            var serializedString = NewtonsoftJsonSerializer.SerializeObject(foo.Value);

            Assert.Equal(serializedFoo, serializedString);
        }

        [Fact]
        public void CanSerializeToString_WithSystemTextJsonProvider()
        {
            var foo = new SystemTextJsonSafeId("123");

            var serializedFoo = SystemTextJsonSerializer.Serialize(foo);
            var serializedString = SystemTextJsonSerializer.Serialize(foo.Value);

            Assert.Equal(serializedFoo, serializedString);
        }

        [Fact]
        public void CanDeserializeFromString_WithNewtonsoftJsonProvider()
        {
            var value = "123";
            var foo = new NewtonsoftJsonSafeId(value);
            var serializedString = NewtonsoftJsonSerializer.SerializeObject(value);

            var deserializedFoo = NewtonsoftJsonSerializer.DeserializeObject<NewtonsoftJsonSafeId>(serializedString);

            Assert.Equal(foo, deserializedFoo);
        }

        [Fact]
        public void CanDeserializeFromString_WithSystemTextJsonProvider()
        {
            var value = "123";
            var foo = new SystemTextJsonSafeId(value);
            var serializedString = SystemTextJsonSerializer.Serialize(value);

            var deserializedFoo = SystemTextJsonSerializer.Deserialize<SystemTextJsonSafeId>(serializedString);

            Assert.Equal(foo, deserializedFoo);
        }

        [Fact]
        public void CanSerializeToString_WithBothJsonConverters()
        {
            var foo = new BothJsonSafeId("123");

            var serializedFoo1 = NewtonsoftJsonSerializer.SerializeObject(foo);
            var serializedString1 = NewtonsoftJsonSerializer.SerializeObject(foo.Value);

            var serializedFoo2 = SystemTextJsonSerializer.Serialize(foo);
            var serializedString2 = SystemTextJsonSerializer.Serialize(foo.Value);

            Assert.Equal(serializedFoo1, serializedString1);
            Assert.Equal(serializedFoo2, serializedString2);
        }

        [Fact]
        public void WhenNoJsonConverter_SerializesWithValueProperty()
        {
            var foo = new NoJsonSafeId("123");

            var serialized1 = NewtonsoftJsonSerializer.SerializeObject(foo);
            var serialized2 = SystemTextJsonSerializer.Serialize(foo);

            var expected = "{\"Value\":\"" + foo.Value + "\"}";

            Assert.Equal(expected, serialized1);
            Assert.Equal(expected, serialized2);
        }
    }
}