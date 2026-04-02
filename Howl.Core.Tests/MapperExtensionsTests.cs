using System;
using Howl.Core.Extensions;
using Xunit;

namespace Howl.Core.Tests.Extensions
{
    public class MapperExtensionsTests
    {
        #region Map Tests

        [Fact]
        public void Map_SimpleObject_ShouldMapCorrectly()
        {
            // Arrange
            var source = new SourceObject { Id = 1, Name = "John" };

            // Act
            var result = source.Map<DestinationObject>();

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.Name);
        }

        [Fact]
        public void Map_WithExistingTarget_ShouldMapToTarget()
        {
            // Arrange
            var source = new SourceObject { Id = 1, Name = "John" };
            var target = new DestinationObject { Id = 99, Name = "Existing" };

            // Act
            var result = source.Map(target);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.Name);
            Assert.Same(target, result);
        }

        [Fact]
        public void Map_WithAfterMap_ShouldInvokeAfterMap()
        {
            // Arrange
            var source = new SourceObject { Id = 1, Name = "John" };

            // Act
            var result = source.Map<DestinationObject>(afterMap: dest => dest.Name = "Modified");

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Modified", result.Name);
        }

        [Fact]
        public void Map_NullSource_ShouldReturnDefault()
        {
            // Arrange
            SourceObject source = null;

            // Act
            var result = source.Map<DestinationObject>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Map_DifferentPropertyNames_ShouldMapMatchingProperties()
        {
            // Arrange
            var source = new SourceObject { Id = 1, Name = "John", ExtraProperty = "Extra" };

            // Act
            var result = source.Map<PartialDestination>();

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.Name);
            Assert.Null(result.DifferentProperty);
        }

        [Fact]
        public void Map_NullSourceProperty_ShouldNotOverwriteDestination()
        {
            // Arrange
            var source = new SourceObject { Id = 1, Name = null };
            var target = new DestinationObject { Id = 99, Name = "Existing" };

            // Act
            var result = source.Map(target);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Existing", result.Name); // Name not overwritten because source is null
        }

        [Fact]
        public void Map_SourceWithMoreProperties_ShouldMapOnlyCommon()
        {
            // Arrange
            var source = new ExtendedSource { Id = 1, Name = "John", Extra = "Extra" };

            // Act
            var result = source.Map<DestinationObject>();

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.Name);
        }

        [Fact]
        public void Map_DestinationWithMoreProperties_ShouldMapOnlyCommon()
        {
            // Arrange
            var source = new SourceObject { Id = 1, Name = "John" };

            // Act
            var result = source.Map<ExtendedDestination>();

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.Name);
            Assert.Null(result.Extra);
        }

        [Fact]
        public void Map_ValueTypes_ShouldMapCorrectly()
        {
            // Arrange
            var source = new ValueTypeSource { Count = 42, Price = 19.99m };

            // Act
            var result = source.Map<ValueTypeDestination>();

            // Assert
            Assert.Equal(42, result.Count);
            Assert.Equal(19.99m, result.Price);
        }

        [Fact]
        public void Map_MultipleTimes_ShouldCacheMapper()
        {
            // Arrange
            var source1 = new SourceObject { Id = 1, Name = "First" };
            var source2 = new SourceObject { Id = 2, Name = "Second" };

            // Act
            var result1 = source1.Map<DestinationObject>();
            var result2 = source2.Map<DestinationObject>();

            // Assert
            Assert.Equal(1, result1.Id);
            Assert.Equal("First", result1.Name);
            Assert.Equal(2, result2.Id);
            Assert.Equal("Second", result2.Name);
        }

        [Fact]
        public void Map_Collections_ShouldBeHandledCorrectly()
        {
            // Arrange
            var source = new CollectionSource
            {
                Items = new System.Collections.Generic.List<string> { "a", "b", "c" }
            };

            // Act
            var result = source.Map<CollectionDestination>();

            // Assert
            Assert.NotNull(result.Items);
            Assert.Equal(3, result.Items.Count);
        }

        #endregion

        #region Inherit Tests

        [Fact]
        public void Inherit_SameType_ShouldInheritNonNullProperties()
        {
            // Arrange
            var target = new DestinationObject { Id = 1, Name = null };
            var source = new DestinationObject { Id = 99, Name = "Inherited" };

            // Act
            var result = target.Inherit(source);

            // Assert
            Assert.Equal(1, result.Id); // Original value preserved
            Assert.Equal("Inherited", result.Name); // Inherited from source
            Assert.Same(target, result);
        }

        [Fact]
        public void Inherit_FromNullSource_ShouldReturnOriginal()
        {
            // Arrange
            var target = new DestinationObject { Id = 1, Name = "Original" };
            DestinationObject source = null;

            // Act
            var result = target.Inherit(source);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Original", result.Name);
        }

        [Fact]
        public void Inherit_WhenTargetHasValue_ShouldNotOverride()
        {
            // Arrange
            var target = new DestinationObject { Id = 1, Name = "Original" };
            var source = new DestinationObject { Id = 99, Name = "Source" };

            // Act
            var result = target.Inherit(source);

            // Assert
            Assert.Equal(1, result.Id); // Not overridden
            Assert.Equal("Original", result.Name); // Not overridden
        }

        [Fact]
        public void Inherit_WhenTargetPropertyNullAndSourceHasValue_ShouldInherit()
        {
            // Arrange
            var target = new DestinationObject { Id = 1, Name = null };
            var source = new DestinationObject { Id = 99, Name = "Source" };

            // Act
            var result = target.Inherit(source);

            // Assert
            Assert.Equal(1, result.Id); // Original preserved
            Assert.Equal("Source", result.Name); // Inherited
        }

        [Fact]
        public void Inherit_AllPropertiesNull_ShouldRemainNull()
        {
            // Arrange
            var target = new DestinationObject { Id = 1 };
            var source = new DestinationObject { Id = 2, Name = null };

            // Act
            var result = target.Inherit(source);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Null(result.Name);
        }

        [Fact]
        public void Inherit_DifferentTypes_ShouldWorkWithMatchingProperties()
        {
            // Arrange
            var target = new DestinationObject { Id = 1, Name = null };
            var source = new SourceObject { Id = 99, Name = "Source", ExtraProperty = "Extra" };

            // Act
            var result = target.Inherit(source);

            // Assert
            Assert.Equal(1, result.Id); // Original preserved (not null)
            Assert.Equal("Source", result.Name); // Inherited from source
        }

        [Fact]
        public void Inherit_DifferentTypes_ExtraPropertiesIgnored()
        {
            // Arrange
            var target = new PartialDestination { Id = 1, Name = "Original" };
            var source = new SourceObject { Id = 99, Name = "Source", ExtraProperty = "Extra" };

            // Act
            var result = target.Inherit(source);

            // Assert - only matching properties are considered
            Assert.Equal(1, result.Id); // Original preserved
            Assert.Equal("Original", result.Name); // Original preserved (not null)
        }

        #endregion

        #region Test Classes

        public class SourceObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ExtraProperty { get; set; }
        }

        public class DestinationObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class PartialDestination
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string DifferentProperty { get; set; }
        }

        public class ExtendedSource : SourceObject
        {
            public string Extra { get; set; }
        }

        public class ExtendedDestination : DestinationObject
        {
            public string Extra { get; set; }
        }

        public class ValueTypeSource
        {
            public int Count { get; set; }
            public decimal Price { get; set; }
        }

        public class ValueTypeDestination
        {
            public int Count { get; set; }
            public decimal Price { get; set; }
        }

        public class CollectionSource
        {
            public System.Collections.Generic.List<string> Items { get; set; }
        }

        public class CollectionDestination
        {
            public System.Collections.Generic.List<string> Items { get; set; }
        }

        #endregion
    }
}
