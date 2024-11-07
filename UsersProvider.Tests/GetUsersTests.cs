

using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UsersProvider_Rika.Functions;

namespace UsersProvider.Tests;

public class GetUsersTests
{
    private readonly Mock<ILogger<GetUsers>> _mockLogger;
    private readonly DbContextOptions<DataContext> _dbContextOptions;

    public GetUsersTests()
    {
        _mockLogger = new Mock<ILogger<GetUsers>>();

        // Configure In-Memory Database
        _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task Run_ReturnsOkResultWithUsers()
    {
        // Arrange
        using (var context = new DataContext(_dbContextOptions))
        {
            context.Users.AddRange(new List<UserEntity>
            {
                new UserEntity { Id = "1", FirstName = "John", LastName = "Doe" },
                new UserEntity { Id = "2", FirstName = "Jane", LastName = "Doe" }
            });
            await context.SaveChangesAsync();

            var function = new GetUsers(_mockLogger.Object, context);
            var httpRequest = new Mock<HttpRequest>();

            // Act
            var result = await function.Run(httpRequest.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserEntity>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }
    }

    [Fact]
    public async Task Run_ReturnsOkResultWithEmptyList_WhenNoUsersExist()
    {
        // Arrange
        using (var context = new DataContext(_dbContextOptions))
        {
            var function = new GetUsers(_mockLogger.Object, context);
            var httpRequest = new Mock<HttpRequest>();

            // Act
            var result = await function.Run(httpRequest.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserEntity>>(okResult.Value);
            Assert.Empty(returnedUsers);
        }
    }
}
