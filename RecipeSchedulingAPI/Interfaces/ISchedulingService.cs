using RecipeSchedulingAPI.Models;

namespace RecipeSchedulingAPI.Interfaces;

public interface ISchedulingService
{
   public Schedule CreateSchedule(Recipe recipe);
}