using System;
using Xunit;

namespace Howl.Core.Tests
{
    public class FactoryTests
    {
        #region IFactory Interface Tests

        [Fact]
        public void IFactory_IsAssignableFromIFactoryT()
        {
            // Arrange
            var factoryInterface = typeof(IFactory);
            var genericInterface = typeof(IFactory<>);

            // Assert
            Assert.True(factoryInterface.IsAssignableFrom(genericInterface.MakeGenericType(typeof(int))));
        }

        #endregion

        #region DelegateFactory Tests

        [Fact]
        public void DelegateFactory_Create_ShouldInvokeHandler()
        {
            // Arrange
            var expected = new TestObject { Id = 1, Name = "Test" };
            var factory = new DelegateFactory<TestObject>(() => expected);

            // Act
            var result = ((IFactory<TestObject>)factory).Create();

            // Assert
            Assert.Same(expected, result);
        }

        [Fact]
        public void DelegateFactory_CreateWithValue_ShouldReturnValue()
        {
            // Arrange
            var factory = new DelegateFactory<int>(() => 42);

            // Act
            var result = ((IFactory<int>)factory).Create();

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void DelegateFactory_CreateWithNull_ShouldReturnNull()
        {
            // Arrange
            var factory = new DelegateFactory<string>(() => null);

            // Act
            var result = ((IFactory<string>)factory).Create();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DelegateFactory_CreateEachTime_ShouldInvokeHandlerEachTime()
        {
            // Arrange
            var counter = 0;
            var factory = new DelegateFactory<int>(() => ++counter);

            // Act
            var result1 = ((IFactory<int>)factory).Create();
            var result2 = ((IFactory<int>)factory).Create();
            var result3 = ((IFactory<int>)factory).Create();

            // Assert
            Assert.Equal(1, result1);
            Assert.Equal(2, result2);
            Assert.Equal(3, result3);
        }

        [Fact]
        public void DelegateFactory_WithComplexObject_ShouldWork()
        {
            // Arrange
            var factory = new DelegateFactory<TestObject>(() => new TestObject
            {
                Id = 1,
                Name = "Created",
                CreatedAt = DateTime.Now
            });

            // Act
            var result = ((IFactory<TestObject>)factory).Create();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Created", result.Name);
        }

        [Fact]
        public void DelegateFactory_WithException_ShouldPropagate()
        {
            // Arrange
            var factory = new DelegateFactory<int>(() => throw new InvalidOperationException("Test exception"));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => ((IFactory<int>)factory).Create());
        }

        [Fact]
        public void DelegateFactory_AsIFactory_ShouldBeAssignable()
        {
            // Arrange
            var factory = new DelegateFactory<string>(() => "test");

            // Assert
            Assert.IsAssignableFrom<IFactory<string>>(factory);
            Assert.IsAssignableFrom<IFactory>(factory);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void Factory_Pattern_Usage_Example()
        {
            // Arrange
            IFactory<TestObject> factory = new DelegateFactory<TestObject>(() =>
                new TestObject { Id = 100, Name = "Factory Created" });

            // Act
            var result = factory.Create();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.Id);
            Assert.Equal("Factory Created", result.Name);
        }

        [Fact]
        public void Factory_WithDependency_ShouldWork()
        {
            // Arrange
            var dependency = new Dependency { Value = "DepValue" };
            var factory = new DelegateFactory<string>(() => dependency.Value);

            // Act
            var result = ((IFactory<string>)factory).Create();

            // Assert
            Assert.Equal("DepValue", result);
        }

        [Fact]
        public void Factory_WithClosures_ShouldCaptureVariables()
        {
            // Arrange
            var prefix = "PRE_";
            var counter = 0;
            var factory = new DelegateFactory<string>(() => $"{prefix}{++counter}");

            // Act
            var result1 = ((IFactory<string>)factory).Create();
            var result2 = ((IFactory<string>)factory).Create();

            // Assert
            Assert.Equal("PRE_1", result1);
            Assert.Equal("PRE_2", result2);
        }

        #endregion

        #region Test Classes

        public class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class Dependency
        {
            public string Value { get; set; }
        }

        #endregion
    }
}
