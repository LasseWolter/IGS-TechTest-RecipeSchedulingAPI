using FluentAssertions;
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
        // REMARK: I could also new up the SchedulingService here. I opted for not doing that to keep the tests as self-contained as possible. 
        // If the service doesn't have any state which could affect subsequent tests, newing it up here would be fine. 
        _mockLogger = new Mock<ILogger<SchedulingService>>();
    }


    // REMARK: This test is too complex and should be broken down into smaller tests that test a single functionality (see other tests in this class). 
    // I created this to check the correctness of my logic. For production code this test wouldn't be great because it tests too many things at once.
    [Test]
    public void CreateScheduleForSingleRequest_SingleLightingPhaseWithSingleOperation_ReturnsCorrectSchedule()
    {
        // Arrange
        var schedulingService = new SchedulingService(_mockLogger.Object);

        DateTime startDate = new DateTime(2022, 1, 1);
        var scheduleRequest = new ScheduleRequest
        {
            TrayNumber = 1,
            RecipeName = "Fruit1",
            StartDate = startDate
        };

        List<Recipe> recipeList =
        [
            new Recipe
            {
                Name = "Fruit1",
                WateringPhases = [],
                LightingPhases =
                [
                    new LightingPhase
                    {
                        Name = "Test Phase 1",
                        Hours = 1,
                        Minutes = 0,
                        Repetitions = 1,
                        Operations =
                        [
                            new Operation
                            {
                                OffsetHours = 0,
                                OffsetMinutes = 0,
                                LightIntensity = LightIntensity.High,
                            }
                        ]
                    }
                ]
            }

        ];

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

    [Test]
    public void ExtractCommandsForWateringPhases_ExtractedWateringCommands_ShouldHaveNullForLightIntensity()
    {
        // Arrange 
        var schedule = new Schedule();
        var schedulingService = new SchedulingService(_mockLogger.Object);
        List<WateringPhase> wateringPhases =
        [
            new WateringPhase
            {
                Amount = 100, Repetitions = 1, Minutes = 0, Hours = 1, Name = "Watering Phase Test", Order = 1
            }
        ];

        // Act
        schedulingService.ExtractCommandsForWateringPhases(ref schedule, wateringPhases, It.IsAny<DateTime>(), It.IsAny<int>());

        // Assert
        schedule.Commands.ForEach(c => c.LightIntensity.Should().BeNull());
    }

    [Test]
    public void ExtractCommandsForLightingPhases_ExtractedLightingCommands_ShouldHaveNullForWaterAmount()
    {
        // Arrange
        var schedule = new Schedule();
        var schedulingService = new SchedulingService(_mockLogger.Object);
        List<LightingPhase> lightingPhases =
        [
            new LightingPhase
            {
                Name = "Test Light Phase 1",
                Hours = 1,
                Minutes = 0,
                Repetitions = 1,
                Operations =
                [
                    new Operation
                    {
                        OffsetHours = 0,
                        OffsetMinutes = 0,
                        LightIntensity = LightIntensity.High,
                    }
                ]
            }
        ];

        // Act  
        schedulingService.ExtractCommandsForLightingPhases(ref schedule, lightingPhases, It.IsAny<DateTime>(), It.IsAny<int>());

        // Assert
        schedule.Commands.ForEach(c => c.WaterAmount.Should().BeNull());
    }
}