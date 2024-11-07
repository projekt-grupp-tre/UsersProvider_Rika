

using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UsersProvider_Rika.Functions;

namespace UsersProvider.Tests;

public class DeleteUserTests
{
    private readonly Mock<ILogger<DeleteUser>> _mockLogger;
    private readonly DbContextOptions<DataContext> _dbContextOptions;

    public DeleteUserTests()
    {
        _mockLogger = new Mock<ILogger<DeleteUser>>();

        // Configure In-Memory Database
        _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task Run_ReturnsOkResult_WhenUserIsDeleted()
    {
        // Arrange
        var userId = "test-user-id";

        using (var context = new DataContext(_dbContextOptions))
        {
            // Seed the in-memory database with a user with all required fields
            var userEntity = new UserEntity
            {
                Id = userId,
                FirstName = "Jonnyee",   //!!!UNIK varje gång!!
                LastName = "Doeenee",     //!!!UNIK varje gång!!
                Email = "johnee.doen@example.com" //!!!UNIK varje gång!!
            };
            context.Users.Add(userEntity);
            await context.SaveChangesAsync();

            var function = new DeleteUser(_mockLogger.Object, context);
            var httpRequest = new Mock<HttpRequest>();

            // Act
            var result = await function.Run(httpRequest.Object, userId);

            // Assert
            Assert.IsType<OkResult>(result);

            // Verify the user was deleted
            var deletedUser = await context.Users.FindAsync(userId);
            Assert.Null(deletedUser);
        }
    }

    [Fact]
    public async Task Run_ReturnsNotFoundResult_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "non-existent-user-id";

        using (var context = new DataContext(_dbContextOptions))
        {
            var function = new DeleteUser(_mockLogger.Object, context);
            var httpRequest = new Mock<HttpRequest>();

            // Act
            var result = await function.Run(httpRequest.Object, userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
