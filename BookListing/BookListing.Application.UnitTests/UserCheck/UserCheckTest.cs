using BookListing.Application.Books.Commands.CreateBook;
using BookListing.Application.Common.Behaviours;
using BookListing.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
namespace BookListing.Application.UnitTests
{


    public class UserCheckTest
    {
        // Declare mock variables
        private Mock<ILogger<CreateBookCommand>> _mockLogger;
        private Mock<ICurrentUserService> _mockCurrentUserService;
        private Mock<IIdentityService> _mockIdentityService;

        // This method is run before each test
        [SetUp]
        public void InitializeTest()
        {
            // Initialize mock objects
            _mockLogger = new Mock<ILogger<CreateBookCommand>>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockIdentityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            // Arrange
            // Set up the user ID to simulate an authenticated user
            _mockCurrentUserService.Setup(x => x.UserId).Returns(Guid.NewGuid().ToString());

            var requestLogger = new LoggingBehaviour<CreateBookCommand>(
                _mockLogger.Object,
                _mockCurrentUserService.Object,
                _mockIdentityService.Object
            );

            var command = new CreateBookCommand
            {
                Author = "Test Author",
                Description = "Test Description",
                Title = "Test title"
            };

            // Act
            // Process the request and log
            await requestLogger.Process(command, new CancellationToken());

            // Assert
            // Ensure that GetUserNameAsync was called once
            _mockIdentityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncIfUnauthenticated()
        {
            // Arrange
            var requestLogger = new LoggingBehaviour<CreateBookCommand>(
                _mockLogger.Object,
                _mockCurrentUserService.Object,
                _mockIdentityService.Object
            );

            var command = new CreateBookCommand
            {
                Author = "Test Author",
                Description = "Test Description",
                Title = "Test title"
            };

            // Act
            // Process the request and log
            await requestLogger.Process(command, new CancellationToken());

            // Assert
            // Ensure that GetUserNameAsync was never called
            _mockIdentityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
        }
    }


}