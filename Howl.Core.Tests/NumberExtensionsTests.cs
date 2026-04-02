using System;
using Howl.Core.Extensions;
using Xunit;

namespace Howl.Core.Tests.Extensions
{
    public class NumberExtensionsTests
    {
        #region Between Tests (Non-nullable)

        [Fact]
        public void Between_ValueWithinRange_ShouldReturnTrue()
        {
            // Arrange
            int value = 5;

            // Act
            var result = value.Between(1, 10);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Between_ValueEqualsLowerBound_ShouldReturnTrue()
        {
            // Arrange
            int value = 1;

            // Act
            var result = value.Between(1, 10);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Between_ValueEqualsUpperBound_ShouldReturnTrue()
        {
            // Arrange
            int value = 10;

            // Act
            var result = value.Between(1, 10);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Between_ValueBelowRange_ShouldReturnFalse()
        {
            // Arrange
            int value = 0;

            // Act
            var result = value.Between(1, 10);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Between_ValueAboveRange_ShouldReturnFalse()
        {
            // Arrange
            int value = 11;

            // Act
            var result = value.Between(1, 10);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Between_DoubleValue_ShouldWorkCorrectly()
        {
            // Arrange
            double value = 3.14;

            // Act
            var result = value.Between(3.0, 4.0);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Between_DateTime_ShouldWorkCorrectly()
        {
            // Arrange
            var value = new DateTime(2020, 6, 15);
            var from = new DateTime(2020, 1, 1);
            var to = new DateTime(2020, 12, 31);

            // Act
            var result = value.Between(from, to);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Between Tests (Nullable)

        [Fact]
        public void Between_NullableValueWithinRange_ShouldReturnTrue()
        {
            // Arrange
            int? value = 5;

            // Act
            var result = value.Between(1, 10);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Between_NullableValueNull_ShouldReturnFalse()
        {
            // Arrange
            int? value = null;

            // Act
            var result = value.Between(1, 10);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Between_NullableFrom_ShouldUseDefaultBehavior()
        {
            // Arrange
            int? value = 5;
            int? from = null;
            int? to = 10;

            // Act
            var result = value.Between(from, to);

            // Assert
            // When from is null and blfromIfnull is true (default), the from condition is satisfied
            Assert.True(result);
        }

        [Fact]
        public void Between_NullableTo_ShouldUseDefaultBehavior()
        {
            // Arrange
            int? value = 5;
            int? from = 1;
            int? to = null;

            // Act
            var result = value.Between(from, to);

            // Assert
            // When to is null and blToIfnull is true (default), the to condition is satisfied
            Assert.True(result);
        }

        [Fact]
        public void Between_BothBoundsNull_ShouldReturnTrue()
        {
            // Arrange
            int? value = 5;
            int? from = null;
            int? to = null;

            // Act
            var result = value.Between(from, to);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Between_WithExplicitNullBehavior_ShouldWorkCorrectly()
        {
            // Arrange
            int? value = 5;
            int? from = null;
            int? to = null;

            // Act - both null behaviors are true, so bounds are considered satisfied
            var result = value.Between(from, to, true, true);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Between_WithFalseFromNullBehavior_ShouldReturnFalseWhenFromNull()
        {
            // Arrange
            int? value = 5;
            int? from = null;
            int? to = 10;

            // Act - blfromIfnull is false, so when from is null, the from condition fails
            var result = value.Between(from, to, false, true);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Between_WithFalseToNullBehavior_ShouldReturnFalseWhenToNull()
        {
            // Arrange
            int? value = 5;
            int? from = 1;
            int? to = null;

            // Act - blToIfnull is false, so when to is null, the to condition fails
            var result = value.Between(from, to, true, false);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Max Tests (Non-nullable)

        [Fact]
        public void Max_FirstGreater_ShouldReturnFirst()
        {
            // Arrange
            int a = 10;
            int b = 5;

            // Act
            var result = a.Max(b);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void Max_SecondGreater_ShouldReturnSecond()
        {
            // Arrange
            int a = 5;
            int b = 10;

            // Act
            var result = a.Max(b);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void Max_EqualValues_ShouldReturnFirst()
        {
            // Arrange
            int a = 10;
            int b = 10;

            // Act
            var result = a.Max(b);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void Max_NegativeValues_ShouldWorkCorrectly()
        {
            // Arrange
            int a = -5;
            int b = -10;

            // Act
            var result = a.Max(b);

            // Assert
            Assert.Equal(-5, result);
        }

        #endregion

        #region Max Tests (Nullable)

        [Fact]
        public void Max_NullableBothHaveValues_ShouldReturnMax()
        {
            // Arrange
            int? a = 10;
            int? b = 5;

            // Act
            var result = a.Max(b);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void Max_NullableFirstNull_ShouldReturnSecond()
        {
            // Arrange
            int? a = null;
            int? b = 5;

            // Act
            var result = a.Max(b);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Max_NullableSecondNull_ShouldReturnFirst()
        {
            // Arrange
            int? a = 10;
            int? b = null;

            // Act
            var result = a.Max(b);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void Max_NullableBothNull_ShouldReturnNull()
        {
            // Arrange
            int? a = null;
            int? b = null;

            // Act
            var result = a.Max(b);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Min Tests (Non-nullable)

        [Fact]
        public void Min_FirstSmaller_ShouldReturnFirst()
        {
            // Arrange
            int a = 5;
            int b = 10;

            // Act
            var result = a.Min(b);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Min_SecondSmaller_ShouldReturnSecond()
        {
            // Arrange
            int a = 10;
            int b = 5;

            // Act
            var result = a.Min(b);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Min_EqualValues_ShouldReturnFirst()
        {
            // Arrange
            int a = 10;
            int b = 10;

            // Act
            var result = a.Min(b);

            // Assert
            Assert.Equal(10, result);
        }

        #endregion

        #region Min Tests (Nullable)

        [Fact]
        public void Min_NullableBothHaveValues_ShouldReturnMin()
        {
            // Arrange
            int? a = 5;
            int? b = 10;

            // Act
            var result = a.Min(b);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Min_NullableFirstNull_ShouldReturnSecond()
        {
            // Arrange
            int? a = null;
            int? b = 5;

            // Act
            var result = a.Min(b);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Min_NullableSecondNull_ShouldReturnFirst()
        {
            // Arrange
            int? a = 10;
            int? b = null;

            // Act
            var result = a.Min(b);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void Min_NullableBothNull_ShouldReturnNull()
        {
            // Arrange
            int? a = null;
            int? b = null;

            // Act
            var result = a.Min(b);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Value Tests

        [Fact]
        public void Value_NullableWithValue_ShouldReturnValue()
        {
            // Arrange
            int? value = 42;

            // Act
            var result = value.Value();

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Value_NullableWithNull_ShouldReturnDefault()
        {
            // Arrange
            int? value = null;

            // Act
            var result = value.Value();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Value_NullableWithNullAndCustomDefault_ShouldReturnCustomDefault()
        {
            // Arrange
            int? value = null;

            // Act
            var result = value.Value(99);

            // Assert
            Assert.Equal(99, result);
        }

        [Fact]
        public void Value_NullableWithValueAndDefault_ShouldReturnValue()
        {
            // Arrange
            int? value = 42;

            // Act
            var result = value.Value(99);

            // Assert
            Assert.Equal(42, result);
        }

        #endregion

        #region ToFixed Tests

        [Fact]
        public void ToFixed_PositiveNumber_ShouldTruncate()
        {
            // Arrange
            decimal value = 9.969m;

            // Act
            var result = value.ToFixed(2);

            // Assert
            Assert.Equal(9.96m, result);
        }

        [Fact]
        public void ToFixed_ZeroPrecision_ShouldReturnIntegerPart()
        {
            // Arrange
            decimal value = 9.969m;

            // Act
            var result = value.ToFixed(0);

            // Assert
            Assert.Equal(9m, result);
        }

        [Fact]
        public void ToFixed_NegativePrecision_ShouldRoundUp()
        {
            // Arrange
            decimal value = 9.969m;

            // Act
            var result = value.ToFixed(-2);

            // Assert
            // Negative precision uses ceiling for positive numbers
            Assert.Equal(9.97m, result);
        }

        [Fact]
        public void ToFixed_NegativeNumber_ShouldHandleCorrectly()
        {
            // Arrange
            decimal value = -9.969m;

            // Act
            var result = value.ToFixed(2);

            // Assert
            // For negative numbers with positive precision, uses ceiling
            Assert.Equal(-9.96m, result);
        }

        [Fact]
        public void ToFixed_NullableWithValue_ShouldProcess()
        {
            // Arrange
            decimal? value = 9.969m;

            // Act
            var result = value.ToFixed(2);

            // Assert
            Assert.Equal(9.96m, result);
        }

        [Fact]
        public void ToFixed_NullableWithNull_ShouldReturnZero()
        {
            // Arrange
            decimal? value = null;

            // Act
            var result = value.ToFixed(2);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void ToFixed_LargePrecision_ShouldWork()
        {
            // Arrange
            decimal value = 1.123456789m;

            // Act
            var result = value.ToFixed(5);

            // Assert
            Assert.Equal(1.12345m, result);
        }

        #endregion

        #region ToFixedAsString Tests

        [Fact]
        public void ToFixedAsString_DefaultParameters_ShouldReturnFormatted()
        {
            // Arrange
            decimal value = 1234.5678m;

            // Act
            var result = value.ToFixedAsString();

            // Assert
            Assert.Equal("1234.56", result);
        }

        [Fact]
        public void ToFixedAsString_WithSharpFormat_ShouldReturnTrimmedFormat()
        {
            // Arrange
            decimal value = 1234.5m;

            // Act
            var result = value.ToFixedAsString(2, "#");

            // Assert
            Assert.Equal("1234.5", result);
        }

        [Fact]
        public void ToFixedAsString_NullableWithValue_ShouldReturnFormatted()
        {
            // Arrange
            decimal? value = 1234.5678m;

            // Act
            var result = value.ToFixedAsString();

            // Assert
            Assert.Equal("1234.56", result);
        }

        [Fact]
        public void ToFixedAsString_NullableWithNull_ShouldReturnDefaultFormatted()
        {
            // Arrange
            decimal? value = null;

            // Act
            var result = value.ToFixedAsString();

            // Assert
            Assert.Equal("0.00", result);
        }

        [Fact]
        public void ToFixedAsString_NullableWithDefaultValue_ShouldUseDefault()
        {
            // Arrange
            decimal? value = null;

            // Act
            var result = value.ToFixedAsString(2, 999.99m);

            // Assert
            Assert.Equal("999.99", result);
        }

        [Fact]
        public void ToFixedAsString_NullableWithNullString_ShouldReturnNullString()
        {
            // Arrange
            decimal? value = null;

            // Act
            var result = value.ToFixedAsString("N/A");

            // Assert
            Assert.Equal("N/A", result);
        }

        [Fact]
        public void ToFixedAsString_NullableWithValueAndNullString_ShouldReturnFormatted()
        {
            // Arrange
            decimal? value = 123.45m;

            // Act
            var result = value.ToFixedAsString("N/A");

            // Assert
            Assert.Equal("123.45", result);
        }

        #endregion

        #region ToFixedFormat Tests

        [Fact]
        public void ToFixedFormat_DefaultParameters_ShouldReturnFormattedWithThousands()
        {
            // Arrange
            decimal value = 123456.789m;

            // Act
            var result = value.ToFixedFormat();

            // Assert
            // Uses "n2" format which includes thousands separator
            Assert.Equal("123,456.78", result);
        }

        [Fact]
        public void ToFixedFormat_WithSharpFormat_ShouldReturnTrimmedFormat()
        {
            // Arrange
            decimal value = 123456.7m;

            // Act
            var result = value.ToFixedFormat(2, "#");

            // Assert
            Assert.Equal("123,456.7", result);
        }

        [Fact]
        public void ToFixedFormat_NullableWithValue_ShouldReturnFormatted()
        {
            // Arrange
            decimal? value = 123456.789m;

            // Act
            var result = value.ToFixedFormat();

            // Assert
            Assert.Equal("123,456.78", result);
        }

        [Fact]
        public void ToFixedFormat_NullableWithNull_ShouldReturnDefaultFormatted()
        {
            // Arrange
            decimal? value = null;

            // Act
            var result = value.ToFixedFormat();

            // Assert
            Assert.Equal("0.00", result);
        }

        [Fact]
        public void ToFixedFormat_NullableWithDefaultValue_ShouldUseDefault()
        {
            // Arrange
            decimal? value = null;

            // Act
            var result = value.ToFixedFormat(2, 999999.99m);

            // Assert
            Assert.Equal("999,999.99", result);
        }

        [Fact]
        public void ToFixedFormat_NullableWithNullString_ShouldReturnNullString()
        {
            // Arrange
            decimal? value = null;

            // Act
            var result = value.ToFixedFormat("N/A");

            // Assert
            Assert.Equal("N/A", result);
        }

        [Fact]
        public void ToFixedFormat_NullableWithValueAndNullString_ShouldReturnFormatted()
        {
            // Arrange
            decimal? value = 123456.78m;

            // Act
            var result = value.ToFixedFormat("N/A");

            // Assert
            Assert.Equal("123,456.78", result);
        }

        #endregion
    }
}
