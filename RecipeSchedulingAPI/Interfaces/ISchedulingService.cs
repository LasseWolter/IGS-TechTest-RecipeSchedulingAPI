using RecipeSchedulingAPI.Models;

namespace RecipeSchedulingAPI.Interfaces;

public interface ISchedulingService
{
   public Schedule? CreateScheduleForSingleRequest(RecipeRequest request, List<Recipe> recipes, bool ordered);
   public Schedule? CreateScheduleForListOfRequests(List<RecipeRequest> requests, List<Recipe> recipes, bool ordered);
}