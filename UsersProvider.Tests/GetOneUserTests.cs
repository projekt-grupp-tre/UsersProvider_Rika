using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using UsersProvider_Rika.Functions;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace UsersProvider.Tests;

public class GetOneUserTests
{
    private readonly Mock<ILogger<GetOneUser>> _mockLogger;
    private readonly DbContextOptions<DataContext> _dbContextOptions;

    public GetOneUserTests()
    {
        _mockLogger = new Mock<ILogger<GetOneUser>>();

        // Configure the in-memory database
        _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task Run_ReturnsOkObjectResult_WhenUserExists()
    {
        // Arrange
        var userId = "test-user-id";
        var userEntity = new UserEntity { Id = userId, FirstName = "John", LastName = "Doe" };

        using (var context = new DataContext(_dbContextOptions))
        {
            // Seed the in-memory database with a user
            context.Users.Add(userEntity);
            await context.SaveChangesAsync();

            var function = new GetOneUser(_mockLogger.Object, context);
            var httpRequest = new Mock<HttpRequest>();

            // Act
            var result = await function.Run(httpRequest.Object, userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserEntity>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
        }
    }

    [Fact]
    public async Task Run_ReturnsNotFoundResult_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "non-existent-user-id";

        using (var context = new DataContext(_dbContextOptions))
        {
            var function = new GetOneUser(_mockLogger.Object, context);
            var httpRequest = new Mock<HttpRequest>();

            // Act
            var result = await function.Run(httpRequest.Object, userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
