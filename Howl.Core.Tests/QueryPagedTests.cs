using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Howl.Core.Contracts
{
    public class QueryTests
    {
        #region Query<T> Property Tests

        [Fact]
        public void Query_Take_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var query = new Query<string>();

            // Act
            query.Take = 20;

            // Assert
            Assert.Equal(20, query.Take);
        }

        [Fact]
        public void Query_Take_WhenNull_ShouldReturnNull()
        {
            // Arrange
            var query = new Query<string>();

            // Assert
            Assert.Null(query.Take);
        }

        [Fact]
        public void Query_Page_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var query = new Query<string>();

            // Act
            query.Page = 3;

            // Assert
            Assert.Equal(3, query.Page);
        }

        [Fact]
        public void Query_Page_WhenNull_ShouldReturnNull()
        {
            // Arrange
            var query = new Query<string>();

            // Assert
            Assert.Null(query.Page);
        }

        [Fact]
        public void Query_Includes_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var query = new Query<string>();

            // Act
            query.Includes = new[] { "Property1", "Property2" };

            // Assert
            Assert.Equal(2, query.Includes.Length);
            Assert.Equal("Property1", query.Includes[0]);
            Assert.Equal("Property2", query.Includes[1]);
        }

        [Fact]
        public void Query_SortString_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var query = new Query<string>();

            // Act
            query.SortString = "Name,asc;Date,desc";

            // Assert
            Assert.Equal("Name,asc;Date,desc", query.SortString);
        }

        #endregion

        #region Query<T>.Skip Calculation Tests

        [Fact]
        public void Query_Skip_ShouldCalculateCorrectly_WhenPageAndTakeSet()
        {
            // Arrange
            var query = new Query<string>
            {
                Page = 2,
                Take = 10
            };

            // Act & Assert
            // Skip = Page * Take = 2 * 10 = 20
            Assert.Equal(20, query.Skip);
        }

        [Fact]
        public void Query_Skip_WhenPageZero_ShouldReturnZero()
        {
            // Arrange
            var query = new Query<string>
            {
                Page = 0,
                Take = 10
            };

            // Assert
            Assert.Equal(0, query.Skip);
        }

        [Fact]
        public void Query_Skip_WhenPageNull_ShouldReturnNull()
        {
            // Arrange
            var query = new Query<string>
            {
                Page = null,
                Take = 10
            };

            // Assert
            Assert.Null(query.Skip);
        }

        [Fact]
        public void Query_Skip_WhenTakeNull_ShouldReturnNull()
        {
            // Arrange
            var query = new Query<string>
            {
                Page = 2,
                Take = null
            };

            // Assert
            Assert.Null(query.Skip);
        }

        [Fact]
        public void Query_Skip_WhenBothNull_ShouldReturnNull()
        {
            // Arrange
            var query = new Query<string>
            {
                Page = null,
                Take = null
            };

            // Assert
            Assert.Null(query.Skip);
        }

        [Fact]
        public void Query_Skip_FirstPage_ShouldReturnZero()
        {
            // Arrange
            var query = new Query<string>
            {
                Page = 0,
                Take = 20
            };

            // Assert
            Assert.Equal(0, query.Skip);
        }

        [Fact]
        public void Query_Skip_LargeValues_ShouldCalculateCorrectly()
        {
            // Arrange
            var query = new Query<string>
            {
                Page = 100,
                Take = 50
            };

            // Act & Assert
            Assert.Equal(5000, query.Skip);
        }

        #endregion

        #region IQuery Interface Tests

        [Fact]
        public void Query_ShouldImplementIQuery()
        {
            // Arrange
            var query = new Query<string>();

            // Assert
            Assert.IsAssignableFrom<IQuery>(query);
            Assert.IsAssignableFrom<IQuery<string>>(query);
        }

        [Fact]
        public void IQuery_ShouldHaveCorrectProperties()
        {
            // Arrange
            IQuery query = new Query<string>
            {
                Take = 10,
                Page = 1
            };

            // Assert
            Assert.Equal(10, query.Take);
            Assert.Equal(1, query.Page);
        }

        #endregion
    }

    public class PagedTests
    {
        #region Paged<T> Property Tests

        [Fact]
        public void Paged_Total_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var paged = new Paged<string>();

            // Act
            paged.Total = 1000;

            // Assert
            Assert.Equal(1000, paged.Total);
        }

        [Fact]
        public void Paged_Items_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var paged = new Paged<string>();
            var items = new[] { "a", "b", "c" };

            // Act
            paged.Items = items;

            // Assert
            Assert.Equal(3, paged.Items.Count());
            Assert.Equal(new[] { "a", "b", "c" }, paged.Items);
        }

        #endregion

        #region Paged<T>.Empty Tests

        [Fact]
        public void Paged_Empty_ShouldReturnEmptyPaged()
        {
            // Arrange & Act
            var empty = Paged<string>.Empty;

            // Assert
            Assert.Equal(0, empty.Total);
            Assert.Empty(empty.Items);
        }

        [Fact]
        public void Paged_Empty_ShouldBeSingleton()
        {
            // Arrange & Act
            var empty1 = Paged<string>.Empty;
            var empty2 = Paged<string>.Empty;

            // Assert
            Assert.Same(empty1, empty2);
        }

        #endregion

        #region Paged<T>.Create Tests

        [Fact]
        public void Paged_Create_ShouldReturnPagedWithCorrectValues()
        {
            // Arrange
            var items = new[] { "a", "b", "c" };
            long count = 100;

            // Act
            var paged = Paged<string>.Create(items, count);

            // Assert
            Assert.Equal(count, paged.Total);
            Assert.Equal(items, paged.Items);
        }

        [Fact]
        public void Paged_Create_WithEmptyItems_ShouldReturnPagedWithEmptyItems()
        {
            // Arrange
            var items = Array.Empty<int>();
            long count = 0;

            // Act
            var paged = Paged<int>.Create(items, count);

            // Assert
            Assert.Equal(0, paged.Total);
            Assert.Empty(paged.Items);
        }

        [Fact]
        public void Paged_Create_WithNullItems_ShouldReturnPagedWithNullItems()
        {
            // Arrange
            IEnumerable<string> items = null;

            // Act
            var paged = Paged<string>.Create(items, 0);

            // Assert
            Assert.Equal(0, paged.Total);
            Assert.Null(paged.Items);
        }

        [Fact]
        public void Paged_Create_WithDifferentTypes_ShouldWorkCorrectly()
        {
            // Arrange
            var intItems = new[] { 1, 2, 3 };
            var stringItems = new[] { "a", "b" };

            // Act
            var intPaged = Paged<int>.Create(intItems, 3);
            var stringPaged = Paged<string>.Create(stringItems, 2);

            // Assert
            Assert.Equal(3, intPaged.Total);
            Assert.Equal(3, intPaged.Items.Count());
            Assert.Equal(2, stringPaged.Total);
            Assert.Equal(2, stringPaged.Items.Count());
        }

        #endregion
    }
}
