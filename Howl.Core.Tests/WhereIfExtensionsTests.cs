using System;
using System.Collections.Generic;
using System.Linq;
using Howl.Core.Extensions;
using Xunit;

namespace Howl.Core.Tests.Extensions
{
    public class WhereIfExtensionsTests
    {
        #region IQueryable WhereIf Tests

        [Fact]
        public void WhereIQueryable_WhenConditionTrue_ShouldApplyFilter()
        {
            // Arrange
            var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

            // Act
            var result = query.WhereIf(x => x > 2, true).ToList();

            // Assert
            Assert.Equal(new[] { 3, 4, 5 }, result);
        }

        [Fact]
        public void WhereIQueryable_WhenConditionFalse_ShouldNotApplyFilter()
        {
            // Arrange
            var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

            // Act
            var result = query.WhereIf(x => x > 2, false).ToList();

            // Assert
            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [Fact]
        public void WhereIQueryable_WithIndex_WhenConditionTrue_ShouldApplyFilter()
        {
            // Arrange
            var query = new[] { 10, 20, 30, 40, 50 }.AsQueryable();

            // Act
            var result = query.WhereIf((x, i) => x > 25 && i > 1, true).ToList();

            // Assert
            Assert.Equal(new[] { 30, 40, 50 }, result);
        }

        [Fact]
        public void WhereIQueryable_WithIndex_WhenConditionFalse_ShouldNotApplyFilter()
        {
            // Arrange
            var query = new[] { 10, 20, 30 }.AsQueryable();

            // Act
            var result = query.WhereIf((x, i) => x > 25, false).ToList();

            // Assert
            Assert.Equal(new[] { 10, 20, 30 }, result);
        }

        [Fact]
        public void WhereIQueryable_WithStringValue_WhenNotEmpty_ShouldApplyFilter()
        {
            // Arrange
            var query = new[] { "apple", "banana", "cherry" }.AsQueryable();

            // Act
            var result = query.WhereIf(x => x.StartsWith("b"), "has value").ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("banana", result[0]);
        }

        [Fact]
        public void WhereIQueryable_WithStringValue_WhenNull_ShouldNotApplyFilter()
        {
            // Arrange
            var query = new[] { "apple", "banana", "cherry" }.AsQueryable();
            string filterValue = null;

            // Act
            var result = query.WhereIf(x => x.StartsWith("b"), filterValue).ToList();

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void WhereIQueryable_WithStringValue_WhenEmpty_ShouldNotApplyFilter()
        {
            // Arrange
            var query = new[] { "apple", "banana", "cherry" }.AsQueryable();

            // Act
            var result = query.WhereIf(x => x.StartsWith("b"), "").ToList();

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void WhereIQueryable_WithStringValue_WhenWhitespace_ShouldNotApplyFilter()
        {
            // Arrange
            var query = new[] { "apple", "banana", "cherry" }.AsQueryable();

            // Act
            var result = query.WhereIf(x => x.StartsWith("b"), "   ").ToList();

            // Assert
            Assert.Equal(3, result.Count);
        }

        #endregion

        #region IEnumerable WhereIf Tests

        [Fact]
        public void WhereIEnumerable_WhenConditionTrue_ShouldApplyFilter()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = list.WhereIf(x => x > 2, true).ToList();

            // Assert
            Assert.Equal(new[] { 3, 4, 5 }, result);
        }

        [Fact]
        public void WhereIEnumerable_WhenConditionFalse_ShouldNotApplyFilter()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = list.WhereIf(x => x > 2, false).ToList();

            // Assert
            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [Fact]
        public void WhereIEnumerable_WithNullPredicate_ShouldNotApplyFilter()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            var result = list.WhereIf(null).ToList();

            // Assert
            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void WhereIEnumerable_WithPredicate_ShouldApplyFilter()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = list.WhereIf(x => x % 2 == 0).ToList();

            // Assert
            Assert.Equal(new[] { 2, 4 }, result);
        }

        [Fact]
        public void WhereIEnumerable_WithIndex_WhenConditionTrue_ShouldApplyFilter()
        {
            // Arrange
            var list = new List<int> { 10, 20, 30, 40, 50 };

            // Act
            var result = list.WhereIf((x, i) => i < 3, true).ToList();

            // Assert
            Assert.Equal(new[] { 10, 20, 30 }, result);
        }

        [Fact]
        public void WhereIEnumerable_WithIndex_WhenConditionFalse_ShouldNotApplyFilter()
        {
            // Arrange
            var list = new List<int> { 10, 20, 30 };

            // Act
            var result = list.WhereIf((x, i) => i < 1, false).ToList();

            // Assert
            Assert.Equal(new[] { 10, 20, 30 }, result);
        }

        [Fact]
        public void WhereIEnumerable_WithStringValue_WhenNotEmpty_ShouldApplyFilter()
        {
            // Arrange
            var list = new List<string> { "apple", "banana", "cherry" };

            // Act
            var result = list.WhereIf(x => x.Contains("a"), "has value").ToList();

            // Assert
            Assert.Equal(new[] { "apple", "banana" }, result);
        }

        [Fact]
        public void WhereIEnumerable_WithStringValue_WhenNull_ShouldNotApplyFilter()
        {
            // Arrange
            var list = new List<string> { "apple", "banana", "cherry" };
            string filterValue = null;

            // Act
            var result = list.WhereIf(x => x.Contains("z"), filterValue).ToList();

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void WhereIEnumerable_WithStringValue_WhenEmpty_ShouldNotApplyFilter()
        {
            // Arrange
            var list = new List<string> { "apple", "banana", "cherry" };

            // Act
            var result = list.WhereIf(x => x.Contains("z"), "").ToList();

            // Assert
            Assert.Equal(3, result.Count);
        }

        #endregion

        #region Chaining Tests

        [Fact]
        public void WhereIQueryable_ChainingMultipleConditions_ShouldApplyAllTrueConditions()
        {
            // Arrange
            var query = Enumerable.Range(1, 100).AsQueryable();

            // Act
            var result = query
                .WhereIf(x => x > 20, true)
                .WhereIf(x => x < 40, true)
                .WhereIf(x => x % 2 == 0, false)
                .ToList();

            // Assert
            Assert.Equal(Enumerable.Range(21, 19), result);
        }

        [Fact]
        public void WhereIEnumerable_ChainingMultipleConditions_ShouldApplyAllTrueConditions()
        {
            // Arrange
            var list = Enumerable.Range(1, 100).ToList();

            // Act
            var result = list
                .WhereIf(x => x > 20, true)
                .WhereIf(x => x < 40, true)
                .WhereIf(x => x % 2 == 0, false)
                .ToList();

            // Assert
            Assert.Equal(Enumerable.Range(21, 19), result);
        }

        [Fact]
        public void WhereIf_WithDynamicConditions_ShouldWorkCorrectly()
        {
            // Arrange
            var query = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.AsQueryable();
            var minFilter = 3;
            var maxFilter = 8;

            // Act
            var result = query
                .WhereIf(x => x >= minFilter, minFilter > 0)
                .WhereIf(x => x <= maxFilter, maxFilter > 0)
                .ToList();

            // Assert
            Assert.Equal(new[] { 3, 4, 5, 6, 7, 8 }, result);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void WhereIQueryable_EmptySource_ShouldReturnEmpty()
        {
            // Arrange
            var query = Array.Empty<int>().AsQueryable();

            // Act
            var result = query.WhereIf(x => x > 0, true).ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void WhereIEnumerable_EmptySource_ShouldReturnEmpty()
        {
            // Arrange
            var list = new List<int>();

            // Act
            var result = list.WhereIf(x => x > 0, true).ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void WhereIf_AllFilteredOut_ShouldReturnEmpty()
        {
            // Arrange
            var query = new[] { 1, 2, 3 }.AsQueryable();

            // Act
            var result = query.WhereIf(x => x > 100, true).ToList();

            // Assert
            Assert.Empty(result);
        }

        #endregion
    }
}
