using System;
using Xunit;

namespace Howl.Core.Tests
{
    public class ExceptionTests
    {
        #region PlatformException Tests

        [Fact]
        public void PlatformException_DefaultConstructor_ShouldSetCode400()
        {
            // Arrange & Act
            var exception = new PlatformException();

            // Assert
            Assert.Equal(400, exception.Code);
            Assert.Null(exception.Message);
            Assert.Null(exception.ExData);
        }

        [Fact]
        public void PlatformException_WithMessage_ShouldSetMessage()
        {
            // Arrange
            var message = "Test error message";

            // Act
            var exception = new PlatformException(message);

            // Assert
            Assert.Equal(400, exception.Code);
            Assert.Equal(message, exception.Message);
            Assert.Null(exception.ExData);
        }

        [Fact]
        public void PlatformException_WithMessageAndData_ShouldSetProperties()
        {
            // Arrange
            var message = "Test error";
            var data = new { Key = "Value" };

            // Act
            var exception = new PlatformException(message, data);

            // Assert
            Assert.Equal(400, exception.Code);
            Assert.Equal(message, exception.Message);
            Assert.Equal(data, exception.ExData);
        }

        [Fact]
        public void PlatformException_WithMessageDataAndCode_ShouldSetAllProperties()
        {
            // Arrange
            var message = "Not found";
            var data = new { Id = 123 };
            var code = 404;

            // Act
            var exception = new PlatformException(message, data, code);

            // Assert
            Assert.Equal(404, exception.Code);
            Assert.Equal(message, exception.Message);
            Assert.Equal(data, exception.ExData);
        }

        [Fact]
        public void PlatformException_WithCodeAndMessage_ShouldSetProperties()
        {
            // Arrange
            var code = 500;
            var message = "Internal error";

            // Act
            var exception = new PlatformException(code, message);

            // Assert
            Assert.Equal(500, exception.Code);
            Assert.Equal(message, exception.Message);
            Assert.Null(exception.ExData);
        }

        [Fact]
        public void PlatformException_WithAllParameters_ShouldSetAllProperties()
        {
            // Arrange
            var code = 503;
            var message = "Service unavailable";
            var data = new { Service = "API" };

            // Act
            var exception = new PlatformException(code, message, data);

            // Assert
            Assert.Equal(503, exception.Code);
            Assert.Equal(message, exception.Message);
            Assert.Equal(data, exception.ExData);
        }

        [Fact]
        public void PlatformException_ShouldInheritFromException()
        {
            // Arrange & Act
            var exception = new PlatformException("Test");

            // Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }

        #endregion

        #region HttpStatusCodeException Tests

        [Fact]
        public void HttpStatusCodeException_WithStatusCode_ShouldSetStatusCode()
        {
            // Arrange
            var statusCode = 404;

            // Act
            var exception = new HttpStatusCodeException(statusCode);

            // Assert
            Assert.Equal(404, exception.StatusCode);
            Assert.Equal(@"text/plain", exception.ContentType);
        }

        [Fact]
        public void HttpStatusCodeException_WithStatusCodeAndMessage_ShouldSetBoth()
        {
            // Arrange
            var statusCode = 500;
            var message = "Internal Server Error";

            // Act
            var exception = new HttpStatusCodeException(statusCode, message);

            // Assert
            Assert.Equal(500, exception.StatusCode);
            Assert.Equal(message, exception.Message);
            Assert.Equal(@"text/plain", exception.ContentType);
        }

        [Fact]
        public void HttpStatusCodeException_WithStatusCodeAndInnerException_ShouldSetMessageFromInner()
        {
            // Arrange
            var innerException = new InvalidOperationException("Inner error");
            var statusCode = 500;

            // Act
            var exception = new HttpStatusCodeException(statusCode, innerException);

            // Assert
            Assert.Equal(500, exception.StatusCode);
            Assert.Contains("Inner error", exception.Message);
            Assert.Equal(@"text/plain", exception.ContentType);
        }

        [Fact]
        public void HttpStatusCodeException_ShouldInheritFromException()
        {
            // Arrange & Act
            var exception = new HttpStatusCodeException(400);

            // Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void HttpStatusCodeException_CustomContentType_CanBeModified()
        {
            // Arrange
            var exception = new HttpStatusCodeException(400);

            // Act
            exception.ContentType = "application/json";

            // Assert
            Assert.Equal("application/json", exception.ContentType);
        }

        #endregion

        #region BadRequestException Tests

        [Fact]
        public void BadRequestException_DefaultConstructor_ShouldSetStatusCode400()
        {
            // Arrange & Act
            var exception = new BadRequestException();

            // Assert
            Assert.Equal(400, exception.StatusCode);
            Assert.Equal(@"text/plain", exception.ContentType);
        }

        [Fact]
        public void BadRequestException_WithMessage_ShouldSetMessage()
        {
            // Arrange
            var message = "Invalid request";

            // Act
            var exception = new BadRequestException(message);

            // Assert
            Assert.Equal(400, exception.StatusCode);
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void BadRequestException_ShouldInheritFromHttpStatusCodeException()
        {
            // Arrange & Act
            var exception = new BadRequestException();

            // Assert
            Assert.IsAssignableFrom<HttpStatusCodeException>(exception);
        }

        [Fact]
        public void BadRequestException_ShouldBeUsedForValidationErrors()
        {
            // Arrange
            var validationError = "The Name field is required.";

            // Act
            var exception = new BadRequestException(validationError);

            // Assert
            Assert.Equal(400, exception.StatusCode);
            Assert.Equal(validationError, exception.Message);
        }

        #endregion

        #region Exception Hierarchy Tests

        [Fact]
        public void ExceptionHierarchy_BadRequestIsHttpStatusCode_IsException()
        {
            // Arrange & Act
            var exception = new BadRequestException("Test");

            // Assert
            Assert.IsAssignableFrom<HttpStatusCodeException>(exception);
            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void ExceptionHierarchy_PlatformExceptionIsException()
        {
            // Arrange & Act
            var exception = new PlatformException("Test");

            // Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void Exceptions_CanBeThrownAndCaught()
        {
            // Arrange & Act & Assert
            Assert.Throws<PlatformException>(new Action(() => throw new PlatformException("Test")));
            Assert.Throws<HttpStatusCodeException>(new Action(() => throw new HttpStatusCodeException(404)));
            Assert.Throws<BadRequestException>(new Action(() => throw new BadRequestException("Bad request")));
        }

        [Fact]
        public void Exceptions_CanBeCaughtByBaseType()
        {
            // Arrange
            Exception caughtException = null;

            // Act
            try
            {
                throw new BadRequestException("Bad request");
            }
            catch (HttpStatusCodeException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.NotNull(caughtException);
            Assert.IsType<BadRequestException>(caughtException);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void PlatformException_WithNullMessage_ShouldAcceptNull()
        {
            // Arrange & Act
            var exception = new PlatformException((string)null);

            // Assert
            Assert.Equal(400, exception.Code);
            Assert.Null(exception.Message);
        }

        [Fact]
        public void PlatformException_WithNullData_ShouldAcceptNull()
        {
            // Arrange & Act
            var exception = new PlatformException("message", null);

            // Assert
            Assert.Null(exception.ExData);
        }

        [Fact]
        public void PlatformException_WithEmptyMessage_ShouldAcceptEmpty()
        {
            // Arrange & Act
            var exception = new PlatformException("");

            // Assert
            Assert.Equal("", exception.Message);
        }

        [Fact]
        public void HttpStatusCodeException_WithNullMessage_ShouldAcceptNull()
        {
            // Arrange & Act
            var exception = new HttpStatusCodeException(404, (string)null);

            // Assert
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void BadRequestException_WithNullMessage_ShouldAcceptNull()
        {
            // Arrange & Act
            var exception = new BadRequestException(null);

            // Assert
            Assert.Equal(400, exception.StatusCode);
        }

        #endregion
    }
}
