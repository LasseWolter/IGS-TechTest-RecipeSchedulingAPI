using Microsoft.Extensions.Logging;
using Moq;
using RecipeSchedulingAPI.Enums;
using RecipeSchedulingAPI.Models;
using RecipeSchedulingAPI.Services;

namespace RecipeSchedulingAPI.Tests;

public class SchedulingServiceTests
{
    private Mock<ILogger<SchedulingService>> _mockLogger;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<SchedulingService>>();
    }

    [Test]
    public void CreateScheduleForSingleRequest_SingleLightingPhaseWithSingleOperation_ReturnsCorrectSchedule()
    {
        // Arrange
        var schedulingService = new SchedulingService(_mockLogger.Object);

        DateTime startDate = new DateTime(2022, 1, 1);
        var scheduleRequest = new ScheduleRequest()
        {
            TrayNumber = 1,
            RecipeName = "Fruit1",
            StartDate = startDate
        };

        var recipeList = new List<Recipe>()
        {
            new Recipe()
            {
                Name = "Fruit1",
                WateringPhases = new List<WateringPhase>(),
                LightingPhases = new List<LightingPhase>()
                {
                    new LightingPhase()
                    {
                        Name = "Test Phase 1",
                        Hours = 1,
                        Minutes = 0,
                        Repetitions = 1,
                        Operations = new List<Operation>()
                        {
                            new Operation()
                            {
                                OffsetHours = 0,
                                OffsetMinutes = 0,
                                LightIntensity = LightIntensity.High,
                            }
                        }
                    }
                }
            },
        };

        // Act
        var schedule = schedulingService.CreateScheduleForSingleRequest(scheduleRequest, recipeList);

        // Assert
        Assert.That(schedule, Is.Not.Null);
        Assert.That(schedule.Commands.Count, Is.EqualTo(1));
        Assert.That(schedule.Commands[0].CommandType, Is.EqualTo(CommandType.Light));
        Assert.That(schedule.Commands[0].LightIntensity, Is.EqualTo(LightIntensity.High));
        Assert.That(schedule.Commands[0].WaterAmount, Is.Null);
        Assert.That(schedule.Commands[0].DateTimeUtc, Is.EqualTo(startDate));
    }
}