using System;
using System.Collections.Generic;
using System.Linq;
using Howl.Core.Extensions;
using Xunit;

namespace Howl.Core.Tests.Extensions
{
    public class ExtensionsTests
    {
        #region To Tests

        [Fact]
        public void To_StringToInt_ShouldConvertCorrectly()
        {
            // Arrange
            var value = "123";

            // Act
            var result = value.To<int>();

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void To_StringToDouble_ShouldConvertCorrectly()
        {
            // Arrange
            var value = "123.45";

            // Act
            var result = value.To<double>();

            // Assert
            Assert.Equal(123.45, result);
        }

        [Fact]
        public void To_StringToBool_ShouldConvertCorrectly()
        {
            // Arrange
            var value = "true";

            // Act
            var result = value.To<bool>();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void To_StringToEnum_ShouldConvertCorrectly()
        {
            // Arrange
            var value = "Monday";

            // Act
            var result = value.To<DayOfWeek>();

            // Assert
            Assert.Equal(DayOfWeek.Monday, result);
        }

        [Fact]
        public void To_StringToEnum_IgnoreCase_ShouldConvertCorrectly()
        {
            // Arrange
            var value = "MONDAY";

            // Act
            var result = value.To<DayOfWeek>();

            // Assert
            Assert.Equal(DayOfWeek.Monday, result);
        }

        [Fact]
        public void To_InvalidStringToEnum_ShouldReturnDefault()
        {
            // Arrange
            var value = "InvalidDay";

            // Act
            var result = value.To<DayOfWeek>();

            // Assert
            Assert.Equal(default(DayOfWeek), result);
        }

        [Fact]
        public void To_InvalidStringToInt_ShouldReturnDefault()
        {
            // Arrange
            var value = "abc";

            // Act
            var result = value.To<int>();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void To_NullToInt_ShouldReturnDefault()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.To<int>();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void To_NullToIntWithDefault_ShouldReturnDefault()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.To(42);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void To_NullableToTarget_ShouldConvertCorrectly()
        {
            // Arrange
            int? value = 100;

            // Act
            var result = value.To<int, long>();

            // Assert
            Assert.Equal(100L, result);
        }

        [Fact]
        public void To_NullableNullToTarget_ShouldReturnDefault()
        {
            // Arrange
            int? value = null;

            // Act
            var result = value.To<int, int>(999);

            // Assert
            Assert.Equal(999, result);
        }

        #endregion

        #region Nullable Tests

        [Fact]
        public void Nullable_ObjectToInt_ShouldConvertCorrectly()
        {
            // Arrange
            object value = "123";

            // Act
            var result = value.Nullable<int>();

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void Nullable_NullToInt_ShouldReturnNull()
        {
            // Arrange
            object value = null;

            // Act
            var result = value.Nullable<int>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Nullable_NullWithDefault_ShouldReturnDefault()
        {
            // Arrange
            object value = null;

            // Act
            var result = value.Nullable<int>(42);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Nullable_ObjectToEnum_ShouldConvertCorrectly()
        {
            // Arrange
            object value = "Friday";

            // Act
            var result = value.Nullable<DayOfWeek>();

            // Assert
            Assert.Equal(DayOfWeek.Friday, result);
        }

        #endregion

        #region As Tests

        [Fact]
        public void As_ObjectToSameType_ShouldReturnConverted()
        {
            // Arrange
            object value = "test";

            // Act
            var result = value.As<string>();

            // Assert
            Assert.Equal("test", result);
        }

        [Fact]
        public void As_ObjectToDifferentType_ShouldReturnDefault()
        {
            // Arrange
            object value = "test";

            // Act
            var result = value.As<int>();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void As_NullToObject_ShouldReturnDefault()
        {
            // Arrange
            object value = null;

            // Act
            var result = value.As<string>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void As_DerivedToBase_ShouldReturnConverted()
        {
            // Arrange
            object value = new List<int>();

            // Act
            var result = value.As<IList<int>>();

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region In Tests

        [Fact]
        public void In_NullableInValues_ShouldReturnTrueWhenFound()
        {
            // Arrange
            int? value = 5;

            // Act
            var result = value.In(1, 2, 3, 4, 5);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void In_NullableInValues_ShouldReturnFalseWhenNotFound()
        {
            // Arrange
            int? value = 10;

            // Act
            var result = value.In(1, 2, 3, 4, 5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void In_NullInValues_ShouldReturnFalse()
        {
            // Arrange
            int? value = null;

            // Act
            var result = value.In(1, 2, 3);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void In_EmptyValues_ShouldReturnFalse()
        {
            // Arrange
            int? value = 1;

            // Act
            var result = value.In();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void In_NullValues_ShouldReturnFalse()
        {
            // Arrange
            int? value = 1;
            int?[] values = null;

            // Act
            var result = value.In(values);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void In_StringInValues_ShouldReturnTrueWhenFound()
        {
            // Arrange
            var value = "apple";

            // Act
            var result = value.In("apple", "banana", "orange");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void In_StringInValues_ShouldReturnFalseWhenNotFound()
        {
            // Arrange
            var value = "grape";

            // Act
            var result = value.In("apple", "banana", "orange");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void In_StringInValues_ShouldBeCaseSensitive()
        {
            // Arrange
            var value = "Apple";

            // Act
            var result = value.In("apple", "banana");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void In_NullStringInValues_ShouldReturnFalse()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.In("a", "b");

            // Assert
            Assert.False(result);
        }

        #endregion

        #region EqualsIgnoreCase Tests

        [Fact]
        public void EqualsIgnoreCase_SameCase_ShouldReturnTrue()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = value.EqualsIgnoreCase("hello");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsIgnoreCase_DifferentCase_ShouldReturnTrue()
        {
            // Arrange
            var value = "Hello";

            // Act
            var result = value.EqualsIgnoreCase("HELLO");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsIgnoreCase_DifferentValues_ShouldReturnFalse()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = value.EqualsIgnoreCase("world");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsIgnoreCase_NullValue_ShouldReturnFalse()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.EqualsIgnoreCase("hello");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsIgnoreCase_NullOther_ShouldReturnFalse()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = value.EqualsIgnoreCase(null);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region EqualsFully Tests

        [Fact]
        public void EqualsFully_SameValues_ShouldReturnTrue()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = value.EqualsFully("hello");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsFully_DifferentCase_ShouldReturnFalse()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = value.EqualsFully("HELLO");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsFully_NullValue_ShouldReturnFalse()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.EqualsFully("hello");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsFully_EmptyValue_ShouldReturnFalse()
        {
            // Arrange
            var value = "";

            // Act
            var result = value.EqualsFully("hello");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsFully_WhitespaceValue_ShouldReturnFalse()
        {
            // Arrange
            var value = "   ";

            // Act
            var result = value.EqualsFully("hello");

            // Assert
            Assert.False(result);
        }

        #endregion

        #region ToQueryString Tests

        [Fact]
        public void ToQueryString_Dictionary_ShouldReturnQueryString()
        {
            // Arrange
            var dict = new Dictionary<string, object>
            {
                { "name", "John" },
                { "age", 30 }
            };

            // Act
            var result = dict.ToQueryString();

            // Assert
            Assert.Contains("name=John", result);
            Assert.Contains("age=30", result);
        }

        [Fact]
        public void ToQueryString_NullDictionary_ShouldReturnEmpty()
        {
            // Arrange
            Dictionary<string, object> dict = null;

            // Act
            var result = dict.ToQueryString();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToQueryString_EmptyDictionary_ShouldReturnEmpty()
        {
            // Arrange
            var dict = new Dictionary<string, object>();

            // Act
            var result = dict.ToQueryString();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToQueryString_WithNullValue_ShouldHandleGracefully()
        {
            // Arrange
            var dict = new Dictionary<string, object>
            {
                { "name", null }
            };

            // Act
            var result = dict.ToQueryString();

            // Assert
            Assert.Contains("name=", result);
        }

        [Fact]
        public void ToQueryString_ShouldUrlEncode()
        {
            // Arrange
            var dict = new Dictionary<string, object>
            {
                { "query", "hello world" }
            };

            // Act
            var result = dict.ToQueryString();

            // Assert
            Assert.Contains("query=hello+world", result);
        }

        #endregion

        #region AppendQueries Tests

        [Fact]
        public void AppendQueries_ShouldAppendQueryString()
        {
            // Arrange
            var url = "http://example.com/api";
            var parameters = new { name = "John", age = 30 };

            // Act
            var result = url.AppendQueries(parameters);

            // Assert
            Assert.Contains("name=John", result);
            Assert.Contains("age=30", result);
        }

        [Fact]
        public void AppendQueries_WithExistingQuery_ShouldAppendWithAmpersand()
        {
            // Arrange
            var url = "http://example.com/api?id=1";
            var parameters = new { name = "John" };

            // Act
            var result = url.AppendQueries(parameters);

            // Assert
            Assert.StartsWith("http://example.com/api?id=1&", result);
            Assert.Contains("name=John", result);
        }

        [Fact]
        public void AppendQueries_NullUrl_ShouldReturnNull()
        {
            // Arrange
            string url = null;
            var parameters = new { name = "John" };

            // Act
            var result = url.AppendQueries(parameters);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void AppendQueries_EmptyUrl_ShouldReturnEmpty()
        {
            // Arrange
            var url = "";
            var parameters = new { name = "John" };

            // Act
            var result = url.AppendQueries(parameters);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void AppendQueries_NullParameters_ShouldReturnOriginalUrl()
        {
            // Arrange
            var url = "http://example.com";

            // Act
            var result = url.AppendQueries(null);

            // Assert
            Assert.Equal("http://example.com", result);
        }

        [Fact]
        public void AppendQueries_WithHash_ShouldAppendBeforeHash()
        {
            // Arrange
            var url = "http://example.com/api#section";
            var parameters = new { id = 1 };

            // Act
            var result = url.AppendQueries(parameters);

            // Assert
            Assert.Contains("id=1", result);
            Assert.EndsWith("#section", result);
        }

        #endregion

        #region ToUrlEncode Tests

        [Fact]
        public void ToUrlEncode_PlainString_ShouldReturnSameString()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = value.ToUrlEncode();

            // Assert
            Assert.Equal("hello", result);
        }

        [Fact]
        public void ToUrlEncode_StringWithSpaces_ShouldEncode()
        {
            // Arrange
            var value = "hello world";

            // Act
            var result = value.ToUrlEncode();

            // Assert
            Assert.Equal("hello+world", result);
        }

        #endregion

        #region IsAny Tests

        [Fact]
        public void IsAny_NonEmptyCollection_ShouldReturnTrue()
        {
            // Arrange
            var list = new[] { 1, 2, 3 };

            // Act
            var result = list.IsAny();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAny_EmptyCollection_ShouldReturnFalse()
        {
            // Arrange
            var list = Array.Empty<int>();

            // Act
            var result = list.IsAny();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAny_NullCollection_ShouldReturnFalse()
        {
            // Arrange
            IEnumerable<int> list = null;

            // Act
            var result = list.IsAny();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAny_WithPredicate_ShouldFilter()
        {
            // Arrange
            var list = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = list.IsAny(x => x > 3);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAny_WithPredicateNoMatch_ShouldReturnFalse()
        {
            // Arrange
            var list = new[] { 1, 2, 3 };

            // Act
            var result = list.IsAny(x => x > 10);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsEmpty Tests

        [Fact]
        public void IsEmpty_EmptyCollection_ShouldReturnTrue()
        {
            // Arrange
            var list = Array.Empty<int>();

            // Act
            var result = list.IsEmpty();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEmpty_NonEmptyCollection_ShouldReturnFalse()
        {
            // Arrange
            var list = new[] { 1, 2, 3 };

            // Act
            var result = list.IsEmpty();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsEmpty_NullCollection_ShouldReturnTrue()
        {
            // Arrange
            IEnumerable<int> list = null;

            // Act
            var result = list.IsEmpty();

            // Assert
            Assert.True(result);
        }

        #endregion

        #region HasValue / IsNullOrEmpty Tests

        [Fact]
        public void HasValue_NonEmptyString_ShouldReturnTrue()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = value.HasValue();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasValue_EmptyString_ShouldReturnFalse()
        {
            // Arrange
            var value = "";

            // Act
            var result = value.HasValue();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasValue_NullString_ShouldReturnFalse()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.HasValue();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNullOrEmpty_EmptyString_ShouldReturnTrue()
        {
            // Arrange
            var value = "";

            // Act
            var result = value.IsNullOrEmpty();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNullOrEmpty_NullString_ShouldReturnTrue()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.IsNullOrEmpty();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNullOrEmpty_NonEmptyString_ShouldReturnFalse()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = value.IsNullOrEmpty();

            // Assert
            Assert.False(result);
        }

        #endregion

        #region StringJoin Tests

        [Fact]
        public void StringJoin_WithSelector_ShouldJoinWithSelector()
        {
            // Arrange
            var list = new[] { 1, 2, 3 };

            // Act
            var result = list.StringJoin(x => x.ToString("000"));

            // Assert
            Assert.Equal("001,002,003", result);
        }

        [Fact]
        public void StringJoin_WithSeparator_ShouldJoinWithSeparator()
        {
            // Arrange
            var list = new[] { "a", "b", "c" };

            // Act
            var result = list.StringJoin("-");

            // Assert
            Assert.Equal("a-b-c", result);
        }

        [Fact]
        public void StringJoin_EmptyCollection_ShouldReturnEmpty()
        {
            // Arrange
            var list = Array.Empty<int>();

            // Act
            var result = list.StringJoin();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void StringJoin_NullCollection_ShouldReturnEmpty()
        {
            // Arrange
            IEnumerable<int> list = null;

            // Act
            var result = list.StringJoin();

            // Assert
            Assert.Equal("", result);
        }

        #endregion

        #region Coalesce Tests

        [Fact]
        public void Coalesce_NonEmptyString_ShouldReturnOriginal()
        {
            // Arrange
            var value = "hello";

            // Act
            var result = value.Coalesce("default1", "default2");

            // Assert
            Assert.Equal("hello", result);
        }

        [Fact]
        public void Coalesce_EmptyString_ShouldReturnFirstNonEmptyDefault()
        {
            // Arrange
            var value = "";

            // Act
            var result = value.Coalesce("", "first", "second");

            // Assert
            Assert.Equal("first", result);
        }

        [Fact]
        public void Coalesce_NullString_ShouldReturnFirstNonEmptyDefault()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.Coalesce(null, "first", "second");

            // Assert
            Assert.Equal("first", result);
        }

        [Fact]
        public void Coalesce_AllEmpty_ShouldReturnNull()
        {
            // Arrange
            var value = "";

            // Act
            var result = value.Coalesce("", "", "");

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Supplant Tests

        [Fact]
        public void Supplant_ShouldFormatString()
        {
            // Arrange
            var template = "Hello, {0}! You have {1} messages.";

            // Act
            var result = template.Supplant("John", 5);

            // Assert
            Assert.Equal("Hello, John! You have 5 messages.", result);
        }

        #endregion

        #region OfDescription Tests

        [Fact]
        public void OfDescription_EnumWithDescription_ShouldReturnDescription()
        {
            // Arrange
            var value = TestEnum.WithDescription;

            // Act
            var result = value.OfDescription();

            // Assert
            Assert.Equal("Test Description", result);
        }

        [Fact]
        public void OfDescription_EnumWithoutDescription_ShouldReturnEmpty()
        {
            // Arrange
            var value = TestEnum.NoDescription;

            // Act
            var result = value.OfDescription();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void OfDescription_NullEnum_ShouldReturnEmpty()
        {
            // Arrange
            TestEnum? value = null;

            // Act
            var result = value.OfDescription();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        #endregion

        #region SplitString Tests

        [Fact]
        public void SplitString_DefaultSeparator_ShouldSplitByComma()
        {
            // Arrange
            var value = "a,b,c";

            // Act
            var result = value.SplitString();

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Equal(new[] { "a", "b", "c" }, result);
        }

        [Fact]
        public void SplitString_CustomSeparator_ShouldSplitBySeparator()
        {
            // Arrange
            var value = "a|b|c";

            // Act
            var result = value.SplitString('|');

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Equal(new[] { "a", "b", "c" }, result);
        }

        [Fact]
        public void SplitString_EmptyParts_ShouldRemoveEmptyEntries()
        {
            // Arrange
            var value = "a,,b,,,c";

            // Act
            var result = value.SplitString();

            // Assert
            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void SplitString_NullString_ShouldReturnNull()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.SplitString();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SplitString_EmptyString_ShouldReturnNull()
        {
            // Arrange
            var value = "";

            // Act
            var result = value.SplitString();

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region SplitTo Tests

        [Fact]
        public void SplitTo_Integers_ShouldConvertToIntArray()
        {
            // Arrange
            var value = "1,2,3";

            // Act
            var result = value.SplitTo<int>();

            // Assert
            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void SplitTo_WithInvalidValues_ShouldUseDefault()
        {
            // Arrange
            var value = "1,abc,3";

            // Act
            var result = value.SplitTo<int>().ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(0, result[1]); // default for invalid
            Assert.Equal(3, result[2]);
        }

        [Fact]
        public void SplitTo_NullString_ShouldReturnNull()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.SplitTo<int>();

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region SplitToNullable Tests

        [Fact]
        public void SplitToNullable_Integers_ShouldConvertToNullableArray()
        {
            // Arrange
            var value = "1,2,3";

            // Act
            var result = value.SplitToNullable<int>();

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
            Assert.Equal(3, result[2]);
        }

        [Fact]
        public void SplitToNullable_NullString_ShouldReturnNull()
        {
            // Arrange
            string value = null;

            // Act
            var result = value.SplitToNullable<int>();

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region AsList Tests

        [Fact]
        public void AsList_FromArray_ShouldReturnList()
        {
            // Arrange
            var array = new[] { 1, 2, 3 };

            // Act
            var result = array.AsList();

            // Assert
            Assert.IsType<List<int>>(result);
            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void AsList_FromList_ShouldReturnSameList()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            var result = list.AsList();

            // Assert
            Assert.Same(list, result);
        }

        [Fact]
        public void AsList_FromIEnumerable_ShouldReturnList()
        {
            // Arrange
            IEnumerable<int> enumerable = Enumerable.Range(1, 3);

            // Act
            var result = enumerable.AsList();

            // Assert
            Assert.IsType<List<int>>(result);
        }

        #endregion

        #region Dictionary Value Tests

        [Fact]
        public void Value_KeyExists_ShouldReturnValue()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "key", 42 } };

            // Act
            var result = dict.Value("key");

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Value_KeyNotExists_ShouldReturnDefault()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act
            var result = dict.Value("missing");

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Value_KeyNotExistsWithDefault_ShouldReturnDefault()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act
            var result = dict.Value("missing", 99);

            // Assert - Now correctly returns the provided default value
            Assert.Equal(99, result);
        }

        [Fact]
        public void Value_NullDictionary_ShouldReturnDefault()
        {
            // Arrange
            Dictionary<string, int> dict = null;

            // Act
            var result = dict.Value("key", 42);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Value_NullKey_ShouldReturnDefault()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "key", 42 } };

            // Act
            var result = dict.Value(null, 99);

            // Assert
            Assert.Equal(99, result);
        }

        #endregion
    }

    // Test enum for OfDescription tests
    public enum TestEnum
    {
        [System.ComponentModel.Description("Test Description")]
        WithDescription = 1,
        NoDescription = 2
    }
}
