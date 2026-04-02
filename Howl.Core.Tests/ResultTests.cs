using System;
using System.Threading.Tasks;
using Xunit;

namespace Howl.Core.Tests
{
    public class ResultTests
    {
        #region Result.Bridge Tests

        [Fact]
        public void Result_Succeeded_ShouldReturnSuccessWithCode200()
        {
            // Arrange & Act
            var result = Result.Succeeded;

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.Code);
        }

        [Fact]
        public async Task Result_SucceededTask_ShouldReturnCompletedTaskWithSuccess()
        {
            // Arrange & Act
            var task = Result.SucceededTask;
            var taskResult = await task;    
            // Assert
            Assert.True(task.IsCompleted);
            Assert.True(taskResult.Success);
            Assert.Equal(200, taskResult.Code);
        }

        [Fact]
        public void Result_Bridge_Erase_ShouldReturnBridgeWithSameProperties()
        {
            // Arrange
            var result = new Result
            {
                Success = true,
                Code = 200,
                Message = "Test message",
                InnerCode = "INNER_001",
                ElapsedMilliseconds = 100.5
            };

            // Act
            var bridge = result.Erase();

            // Assert
            Assert.Equal(result.Success, bridge.Success);
            Assert.Equal(result.Code, bridge.Code);
            Assert.Equal(result.Message, bridge.Message);
            Assert.Equal(result.InnerCode, bridge.InnerCode);
            Assert.Equal(result.ElapsedMilliseconds, bridge.ElapsedMilliseconds);
        }

        [Fact]
        public void Result_Datalize_ShouldConvertToResultT()
        {
            // Arrange
            var result = new Result
            {
                Success = true,
                Code = 200,
                Message = "Test message"
            };

            // Act
            var typedResult = result.Datalize(42);

            // Assert
            Assert.Equal(42, typedResult.Data);
            Assert.Equal(result.Success, typedResult.Success);
            Assert.Equal(result.Code, typedResult.Code);
            Assert.Equal(result.Message, typedResult.Message);
        }

        [Fact]
        public void Result_Datalize_WhenAlreadyTyped_ShouldReturnSameInstance()
        {
            // Arrange
            var typedResult = new Result<int>
            {
                Success = true,
                Code = 200,
                Data = 100
            };

            // Act
            var result = typedResult.Datalize(200);

            // Assert
            Assert.Same(typedResult, result);
            Assert.Equal(100, result.Data); // Original data preserved
        }

        #endregion

        #region Result<T> Tests

        [Fact]
        public void ResultT_ImplicitOperator_FromT_ShouldCreateSucceedResult()
        {
            // Arrange
            string data = "test data";

            // Act
            Result<string> result = data;

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.Code);
            Assert.Equal(data, result.Data);
        }

        [Fact]
        public void ResultT_ImplicitOperator_FromBridge_ShouldCreateResultFromBridge()
        {
            // Arrange
            var bridge = ResultBuilder.Fail("Error message");

            // Act
            Result<int> result = bridge;

            // Assert
            Assert.False(result.Success);
            Assert.Equal(400, result.Code);
            Assert.Equal("Error message", result.Message);
            Assert.Equal(default(int), result.Data);
        }

        [Fact]
        public async Task ResultT_ImplicitOperator_ToTask_ShouldReturnCompletedTask()
        {
            // Arrange
            var result = new Result<int> { Success = true, Data = 42 };

            // Act
            Task<Result<int>> task = result;

            // Assert
            Assert.True(task.IsCompleted);
            Assert.Equal(result, (await task));
        }

        #endregion

        #region ResultBuilder.Succeed Tests

        [Fact]
        public void ResultBuilder_Succeed_ShouldReturnSuccessResult()
        {
            // Arrange
            var data = new { Name = "Test", Value = 123 };

            // Act
            var result = ResultBuilder.Succeed(data);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.Code);
            Assert.Equal(data, result.Data);
            Assert.Null(result.Message);
        }

        [Fact]
        public void ResultBuilder_Succeed_WithMessage_ShouldReturnSuccessWithMessage()
        {
            // Arrange
            var data = "test data";
            var message = "Operation completed successfully";

            // Act
            var result = ResultBuilder.Succeed(data, message);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.Code);
            Assert.Equal(data, result.Data);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void ResultBuilder_Succeed_WithNullData_ShouldReturnSuccessWithNullData()
        {
            // Act
            var result = ResultBuilder.Succeed<string>(null);

            // Assert
            Assert.True(result.Success);
            Assert.Null(result.Data);
        }

        #endregion

        #region ResultBuilder.Fail Tests

        [Fact]
        public void ResultBuilder_Fail_ShouldReturnBridgeWithFailure()
        {
            // Act
            var bridge = ResultBuilder.Fail("Error occurred");

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(400, bridge.Code);
            Assert.Equal("Error occurred", bridge.Message);
        }

        [Fact]
        public void ResultBuilder_Fail_WithStatusCode_ShouldReturnBridgeWithStatusCode()
        {
            // Act
            var bridge = ResultBuilder.Fail(404, "Not found", "NOT_FOUND");

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(404, bridge.Code);
            Assert.Equal("Not found", bridge.Message);
            Assert.Equal("NOT_FOUND", bridge.InnerCode);
        }

        [Fact]
        public void ResultBuilder_Fail_WithNullMessage_ShouldReturnBridgeWithNullMessage()
        {
            // Act
            var bridge = ResultBuilder.Fail();

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(400, bridge.Code);
            Assert.Null(bridge.Message);
        }

        [Fact]
        public void ResultBuilder_FailT_ShouldReturnResultWithFailure()
        {
            // Act
            var result = ResultBuilder.Fail("default data", 400, "Bad request", "BAD_REQUEST");

            // Assert
            Assert.False(result.Success);
            Assert.Equal(400, result.Code);
            Assert.Equal("Bad request", result.Message);
            Assert.Equal("BAD_REQUEST", result.InnerCode);
            Assert.Equal("default data", result.Data);
        }

        #endregion

        #region ResultBuilder Null/NotFound/Disabled/Error Tests

        [Fact]
        public void ResultBuilder_Null_ShouldReturnFailWithNullMessage()
        {
            // Act
            var bridge = ResultBuilder.Null("UserName");

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(400, bridge.Code);
            Assert.Equal("UserName can not be null.", bridge.Message);
        }

        [Fact]
        public void ResultBuilder_NotFound_ShouldReturnFailWithNotFoundMessage()
        {
            // Act
            var bridge = ResultBuilder.NotFound("User");

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(404, bridge.Code);
            Assert.Equal("User can not be found.", bridge.Message);
        }

        [Fact]
        public void ResultBuilder_Disabled_ShouldReturnFailWithDisabledMessage()
        {
            // Act
            var bridge = ResultBuilder.Disabled("Account");

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(400, bridge.Code);
            Assert.Equal("Account is disabled.", bridge.Message);
        }

        [Fact]
        public void ResultBuilder_Error_ShouldReturnFailWithCode500()
        {
            // Act
            var bridge = ResultBuilder.Error("Internal server error");

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(500, bridge.Code);
            Assert.Equal("Internal server error", bridge.Message);
        }

        [Fact]
        public void ResultBuilder_Error_WithNullMessage_ShouldReturnFailWithNullMessage()
        {
            // Act
            var bridge = ResultBuilder.Error();

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(500, bridge.Code);
            Assert.Null(bridge.Message);
        }

        #endregion

        #region ResultBuilder.Handle Tests

        [Fact]
        public void ResultBuilder_Handle_WithException_ShouldReturnFailWithCode500()
        {
            // Arrange
            var exception = new InvalidOperationException("Test exception");

            // Act
            var bridge = ResultBuilder.Handle(exception);

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(500, bridge.Code);
        }

        [Fact]
        public void ResultBuilder_Handle_WithNullException_ShouldReturnFailWithCode500()
        {
            // Act
            var bridge = ResultBuilder.Handle(null);

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(500, bridge.Code);
        }

        [Fact]
        public void ResultBuilder_Handle_WithAggregateException_ShouldUnwrapInnerException()
        {
            // Arrange
            var innerException = new InvalidOperationException("Inner exception");
            var aggregateException = new AggregateException(innerException);

            // Act
            var bridge = ResultBuilder.Handle(aggregateException);

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(500, bridge.Code);
        }

        [Fact]
        public void ResultBuilder_Handle_WithNestedAggregateException_ShouldUnwrapAllLevels()
        {
            // Arrange
            var innermostException = new ArgumentException("Innermost");
            var innerAggregate = new AggregateException(innermostException);
            var outerAggregate = new AggregateException(innerAggregate);

            // Act
            var bridge = ResultBuilder.Handle(outerAggregate);

            // Assert
            Assert.False(bridge.Success);
            Assert.Equal(500, bridge.Code);
        }

        #endregion

        #region Result Children Tests

        [Fact]
        public void Result_WithChildren_ShouldPreserveChildren()
        {
            // Arrange
            var children = new[]
            {
                new Result { Success = true, Code = 200 },
                new Result { Success = false, Code = 400 }
            };

            // Act
            var result = new Result
            {
                Success = true,
                Children = children
            };

            // Assert
            Assert.Equal(2, result.Children.Length);
            Assert.True(result.Children[0].Success);
            Assert.False(result.Children[1].Success);
        }

        #endregion
    }
}
