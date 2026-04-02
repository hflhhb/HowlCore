using System;
using System.Collections.Generic;
using Howl.Core.Extensions;
using Xunit;

namespace Howl.Core.Tests.Extensions
{
    public class JsonExtensionsTests
    {
        #region TrySerialize Tests

        [Fact]
        public void TrySerialize_SimpleObject_ShouldReturnValidJson()
        {
            // Arrange
            var obj = new { Name = "John", Age = 30 };

            // Act
            var result = obj.TrySerialize();

            // Assert
            Assert.Contains("\"name\"", result);
            Assert.Contains("\"age\"", result);
            Assert.Contains("John", result);
            Assert.Contains("30", result);
        }

        [Fact]
        public void TrySerialize_ObjectWithNullProperty_ShouldIgnoreNull()
        {
            // Arrange
            var obj = new TestObject { Name = "John", Value = null };

            // Act
            var result = obj.TrySerialize();

            // Assert
            Assert.Contains("name", result);
            Assert.DoesNotContain("value", result);
        }

        [Fact]
        public void TrySerialize_Enum_ShouldSerializeAsString()
        {
            // Arrange
            var obj = new { Day = DayOfWeek.Monday };

            // Act
            var result = obj.TrySerialize();

            // Assert
            Assert.Contains("Monday", result);
        }

        [Fact]
        public void TrySerialize_Collection_ShouldReturnJsonArray()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            var result = list.TrySerialize();

            // Assert
            Assert.Equal("[1,2,3]", result);
        }

        [Fact]
        public void TrySerialize_EmptyCollection_ShouldReturnEmptyArray()
        {
            // Arrange
            var list = new List<int>();

            // Act
            var result = list.TrySerialize();

            // Assert
            Assert.Equal("[]", result);
        }

        [Fact]
        public void TrySerialize_NullObject_ShouldReturnNullJson()
        {
            // Arrange
            object obj = null;

            // Act
            var result = obj.TrySerialize();

            // Assert
            Assert.Equal("null", result);
        }

        [Fact]
        public void TrySerialize_ComplexObject_ShouldSerializeCorrectly()
        {
            // Arrange
            var obj = new ComplexObject
            {
                Id = 1,
                Name = "Test",
                Nested = new NestedObject { Value = "NestedValue" }
            };

            // Act
            var result = obj.TrySerialize();

            // Assert
            Assert.Contains("id", result);
            Assert.Contains("name", result);
            Assert.Contains("nested", result);
            Assert.Contains("NestedValue", result);
        }

        [Fact]
        public void TrySerialize_CamelCasePropertyNames_ShouldUseCamelCase()
        {
            // Arrange
            var obj = new { PropertyName = "value" };

            // Act
            var result = obj.TrySerialize();

            // Assert
            Assert.Contains("propertyName", result);
            Assert.DoesNotContain("PropertyName", result);
        }

        #endregion

        #region TryDeserialize Tests

        [Fact]
        public void TryDeserialize_ValidJson_ShouldReturnObject()
        {
            // Arrange
            var json = "{\"name\":\"John\",\"age\":30}";

            // Act
            var result = json.TryDeserialize<TestObject>();

            // Assert
            Assert.Equal("John", result.Name);
            Assert.Equal(30, result.Age);
        }

        [Fact]
        public void TryDeserialize_JsonArray_ShouldReturnCollection()
        {
            // Arrange
            var json = "[1,2,3]";

            // Act
            var result = json.TryDeserialize<List<int>>();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void TryDeserialize_EmptyArray_ShouldReturnEmptyList()
        {
            // Arrange
            var json = "[]";

            // Act
            var result = json.TryDeserialize<List<int>>();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void TryDeserialize_InvalidJson_ShouldReturnDefault()
        {
            // Arrange
            var json = "not valid json";

            // Act
            var result = json.TryDeserialize<TestObject>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryDeserialize_NullString_ShouldReturnDefault()
        {
            // Arrange
            string json = null;

            // Act
            var result = json.TryDeserialize<TestObject>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryDeserialize_EmptyString_ShouldReturnDefault()
        {
            // Arrange
            var json = "";

            // Act
            var result = json.TryDeserialize<TestObject>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryDeserialize_NullJson_ShouldReturnNull()
        {
            // Arrange
            var json = "null";

            // Act
            var result = json.TryDeserialize<TestObject>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryDeserialize_MissingProperties_ShouldUseDefaults()
        {
            // Arrange
            var json = "{\"name\":\"John\"}";

            // Act
            var result = json.TryDeserialize<TestObject>();

            // Assert
            Assert.Equal("John", result.Name);
            Assert.Equal(0, result.Age); // default
        }

        [Fact]
        public void TryDeserialize_ComplexObject_ShouldDeserializeCorrectly()
        {
            // Arrange
            var json = "{\"id\":1,\"name\":\"Test\",\"nested\":{\"value\":\"NestedValue\"}}";

            // Act
            var result = json.TryDeserialize<ComplexObject>();

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
            Assert.NotNull(result.Nested);
            Assert.Equal("NestedValue", result.Nested.Value);
        }

        [Fact]
        public void TryDeserialize_EnumAsString_ShouldDeserializeCorrectly()
        {
            // Arrange
            var json = "{\"day\":\"Monday\"}";

            // Act
            var result = json.TryDeserialize<DayHolder>();

            // Assert
            Assert.Equal(DayOfWeek.Monday, result.Day);
        }

        [Fact]
        public void TryDeserialize_ReferenceLoop_ShouldNotFail()
        {
            // Arrange - object with reference loop
            var original = new SelfReferencingObject { Id = 1 };
            original.Self = original;

            // Act
            var serialized = original.TrySerialize();

            // Assert - should not throw and should handle the loop
            Assert.NotNull(serialized);
        }

        #endregion

        #region Round-trip Tests

        [Fact]
        public void SerializeDeserialize_RoundTrip_ShouldPreserveData()
        {
            // Arrange
            var original = new TestObject { Name = "John", Age = 30 };

            // Act
            var json = original.TrySerialize();
            var result = json.TryDeserialize<TestObject>();

            // Assert
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.Age, result.Age);
        }

        [Fact]
        public void SerializeDeserialize_CollectionRoundTrip_ShouldPreserveData()
        {
            // Arrange
            var original = new List<TestObject>
            {
                new TestObject { Name = "John", Age = 30 },
                new TestObject { Name = "Jane", Age = 25 }
            };

            // Act
            var json = original.TrySerialize();
            var result = json.TryDeserialize<List<TestObject>>();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("John", result[0].Name);
            Assert.Equal("Jane", result[1].Name);
        }

        #endregion

        #region Test Classes

        public class TestObject
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int? Value { get; set; }
        }

        public class ComplexObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public NestedObject Nested { get; set; }
        }

        public class NestedObject
        {
            public string Value { get; set; }
        }

        public class DayHolder
        {
            public DayOfWeek Day { get; set; }
        }

        public class SelfReferencingObject
        {
            public int Id { get; set; }
            public SelfReferencingObject Self { get; set; }
        }

        #endregion
    }
}
