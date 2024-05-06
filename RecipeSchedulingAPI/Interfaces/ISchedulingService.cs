using RecipeSchedulingAPI.Models;

namespace RecipeSchedulingAPI.Interfaces;

public interface ISchedulingService
{
    public Schedule? CreateScheduleForSingleRequest(ScheduleRequest request, List<Recipe> recipes, bool ordered);
    public Schedule? CreateScheduleForListOfRequests(List<ScheduleRequest> requests, List<Recipe> recipes, bool ordered);
}