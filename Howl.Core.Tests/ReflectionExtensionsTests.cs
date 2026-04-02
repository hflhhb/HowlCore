using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Howl.Core.Reflection;
using Xunit;

namespace Howl.Core.Tests.Reflection
{
    public class ReflectionExtensionsTests
    {
        #region Get by MemberInfo Tests

        [Fact]
        public void Get_Property_ShouldReturnValue()
        {
            // Arrange
            var obj = new TestClass { Name = "Test" };
            var prop = typeof(TestClass).GetProperty(nameof(TestClass.Name));

            // Act
            var result = obj.Get(prop);

            // Assert
            Assert.Equal("Test", result);
        }

        [Fact]
        public void Get_Field_ShouldReturnValue()
        {
            // Arrange
            var obj = new TestClass();
            obj.SetField("FieldValue");
            var field = typeof(TestClass).GetField("_field");

            // Act
            var result = obj.Get(field);

            // Assert
            Assert.Equal("FieldValue", result);
        }

        [Fact]
        public void Get_Generic_ShouldReturnTypedValue()
        {
            // Arrange
            var obj = new TestClass { Count = 42 };
            var prop = typeof(TestClass).GetProperty(nameof(TestClass.Count));

            // Act
            var result = obj.Get<int>(prop);

            // Assert
            Assert.Equal(42, result);
        }

        #endregion

        #region Get by String Tests

        [Fact]
        public void Get_ByPropertyName_ShouldReturnValue()
        {
            // Arrange
            var obj = new TestClass { Name = "Test" };

            // Act
            var result = obj.Get("Name");

            // Assert
            Assert.Equal("Test", result);
        }

        [Fact]
        public void Get_ByPropertyNameGeneric_ShouldReturnTypedValue()
        {
            // Arrange
            var obj = new TestClass { Count = 42 };

            // Act
            var result = obj.Get<int>("Count");

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Get_NonExistentProperty_ShouldReturnNull()
        {
            // Arrange
            var obj = new TestClass();

            // Act
            var result = obj.Get("NonExistent");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Get_NullObject_ShouldReturnNull()
        {
            // Arrange
            TestClass obj = null;

            // Act
            var result = obj.Get("Name");

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Get by LambdaExpression Tests

        [Fact]
        public void Get_ByLambdaExpression_ShouldReturnValue()
        {
            // Arrange
            var obj = new TestClass { Name = "Test" };
            Expression<Func<TestClass, object>> expr = x => x.Name;

            // Act
            var result = obj.Get(expr);

            // Assert
            Assert.Equal("Test", result);
        }

        [Fact]
        public void Get_ByLambdaExpressionGeneric_ShouldReturnTypedValue()
        {
            // Arrange
            var obj = new TestClass { Count = 42 };
            Expression<Func<TestClass, int>> expr = x => x.Count;

            // Act
            var result = obj.Get<int>(expr);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Get_NestedProperty_ShouldReturnValue()
        {
            // Arrange
            var obj = new TestClass
            {
                Nested = new NestedClass { Value = "NestedValue" }
            };
            Expression<Func<TestClass, object>> expr = x => x.Nested.Value;

            // Act
            var result = obj.Get(expr);

            // Assert
            Assert.Equal("NestedValue", result);
        }

        [Fact]
        public void Get_NestedPropertyWithNull_ShouldReturnNull()
        {
            // Arrange
            var obj = new TestClass { Nested = null };
            Expression<Func<TestClass, object>> expr = x => x.Nested.Value;

            // Act
            var result = obj.Get(expr);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Set Tests

        [Fact]
        public void Set_ByPropertyName_ShouldSetValue()
        {
            // Arrange
            var obj = new TestClass();

            // Act
            obj.Set("Name", "NewValue");

            // Assert
            Assert.Equal("NewValue", obj.Name);
        }

        [Fact]
        public void Set_ByMemberInfo_ShouldSetValue()
        {
            // Arrange
            var obj = new TestClass();
            var prop = typeof(TestClass).GetProperty(nameof(TestClass.Name));

            // Act
            obj.Set(prop, "NewValue");

            // Assert
            Assert.Equal("NewValue", obj.Name);
        }

        [Fact]
        public void Set_Field_ShouldSetValue()
        {
            // Arrange
            var obj = new TestClass();
            var field = typeof(TestClass).GetField("_field");

            // Act
            obj.Set(field, "NewFieldValue");

            // Assert
            Assert.Equal("NewFieldValue", obj.GetField());
        }

        [Fact]
        public void Set_ByLambdaExpression_ShouldSetValue()
        {
            // Arrange
            var obj = new TestClass();
            Expression<Func<TestClass, object>> expr = x => x.Name;

            // Act
            obj.Set(expr, "NewValue");

            // Assert
            Assert.Equal("NewValue", obj.Name);
        }

        [Fact]
        public void Set_NullObject_ShouldReturnNull()
        {
            // Arrange
            TestClass obj = null;

            // Act - when obj is null, Set returns null (the object itself)
            var result = obj.Set("Name", "Value");

            // Assert - returns null because me is null
            Assert.Null(result);
        }

        #endregion

        #region ToDictionary Tests

        [Fact]
        public void ToDictionary_Object_ShouldReturnDictionary()
        {
            // Arrange
            var obj = new TestClass { Name = "Test", Count = 42 };

            // Act
            var result = obj.ToDictionary();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result["Name"]);
            Assert.Equal(42, result["Count"]);
        }

        [Fact]
        public void ToDictionary_NullObject_ShouldReturnNull()
        {
            // Arrange
            TestClass obj = null;

            // Act
            var result = obj.ToDictionary();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ToDictionary_DictionaryInput_ShouldReturnSameDictionary()
        {
            // Arrange
            var dict = new Dictionary<string, object> { { "key", "value" } };

            // Act
            var result = dict.ToDictionary();

            // Assert
            Assert.Same(dict, result);
        }

        [Fact]
        public void ToDictionary_CaseInsensitiveLookup_ShouldWork()
        {
            // Arrange
            var obj = new TestClass { Name = "Test" };

            // Act
            var result = obj.ToDictionary();

            // Assert
            Assert.Equal("Test", result["name"]); // lowercase key
            Assert.Equal("Test", result["NAME"]); // uppercase key
        }

        #endregion

        #region Path Tests

        [Fact]
        public void Path_SingleProperty_ShouldReturnPropertyName()
        {
            // Arrange
            Expression<Func<TestClass, object>> expr = x => x.Name;

            // Act
            var result = expr.Path();

            // Assert
            Assert.Equal("Name", result);
        }

        [Fact]
        public void Path_NestedProperty_ShouldReturnDotPath()
        {
            // Arrange
            Expression<Func<TestClass, object>> expr = x => x.Nested.Value;

            // Act
            var result = expr.Path();

            // Assert
            Assert.Equal("Nested.Value", result);
        }

        [Fact]
        public void Path_DeepNestedProperty_ShouldReturnFullDotPath()
        {
            // Arrange
            Expression<Func<TestClass, object>> expr = x => x.Nested.DeepNested.DeepValue;

            // Act
            var result = expr.Path();

            // Assert
            Assert.Equal("Nested.DeepNested.DeepValue", result);
        }

        [Fact]
        public void Path_NoMemberAccess_ShouldThrow()
        {
            // Arrange
            Expression<Func<TestClass, object>> expr = x => x;

            // Act & Assert - Now throws ArgumentException as expected
            Assert.Throws<ArgumentException>(() => expr.Path());
        }

        #endregion

        #region Members Tests

        [Fact]
        public void Members_SingleProperty_ShouldReturnSingleMember()
        {
            // Arrange
            Expression<Func<TestClass, object>> expr = x => x.Name;

            // Act
            var result = expr.Members();

            // Assert
            Assert.Single(result);
            Assert.Equal("Name", result[0].Name);
        }

        [Fact]
        public void Members_NestedProperties_ShouldReturnAllMembers()
        {
            // Arrange
            Expression<Func<TestClass, object>> expr = x => x.Nested.Value;

            // Act
            var result = expr.Members();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Equal("Nested", result[0].Name);
            Assert.Equal("Value", result[1].Name);
        }

        #endregion

        #region Properties Tests

        [Fact]
        public void Properties_SingleProperty_ShouldReturnSingleProperty()
        {
            // Arrange
            Expression<Func<TestClass, object>> expr = x => x.Name;

            // Act
            var result = expr.Properties();

            // Assert
            Assert.Single(result);
            Assert.Equal("Name", result[0].Name);
        }

        [Fact]
        public void Properties_NestedProperties_ShouldReturnAllProperties()
        {
            // Arrange
            Expression<Func<TestClass, object>> expr = x => x.Nested.Value;

            // Act
            var result = expr.Properties();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Equal("Nested", result[0].Name);
            Assert.Equal("Value", result[1].Name);
        }

        #endregion

        #region IsNullableType Tests

        [Fact]
        public void IsNullableType_NullableType_ShouldReturnTrue()
        {
            // Arrange
            var type = typeof(int?);

            // Act
            var result = type.IsNullableType();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNullableType_NonNullableType_ShouldReturnFalse()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var result = type.IsNullableType();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNullableType_ReferenceType_ShouldReturnFalse()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var result = type.IsNullableType();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNullableType_DateTimeNullable_ShouldReturnTrue()
        {
            // Arrange
            var type = typeof(DateTime?);

            // Act
            var result = type.IsNullableType();

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Test Classes

        public class TestClass
        {
            public string _field;
            public string Name { get; set; }
            public int Count { get; set; }
            public NestedClass Nested { get; set; }

            public void SetField(string value) => _field = value;
            public string GetField() => _field;
        }

        public class NestedClass
        {
            public string Value { get; set; }
            public DeepNestedClass DeepNested { get; set; }
        }

        public class DeepNestedClass
        {
            public string DeepValue { get; set; }
        }

        #endregion
    }
}
