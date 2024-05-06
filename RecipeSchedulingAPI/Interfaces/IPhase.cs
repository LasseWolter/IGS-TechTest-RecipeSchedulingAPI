namespace RecipeSchedulingAPI.Interfaces;

// REMARK: Composition vs inheritance...the good old debate.
// I opt for composition because I prefer having all logic/properties defined in one class instead of having
// to jump up the inheritance chain to find the properties. However, I'm always happy to convinced otherwise.
// One of my fav concepts is 'Disagree and Commit' - let's discuss it and then decide what's best for the situation.
public interface IPhase
{
    public string Name { get; set; }
    public int Order { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Repetitions { get; set; }
}