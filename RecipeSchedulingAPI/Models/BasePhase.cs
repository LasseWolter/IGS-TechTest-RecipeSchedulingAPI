namespace RecipeSchedulingAPI.Models;

// REMARK: Composition vs inheritance...the good old debate. I'm gonna go with inheritance here following
// the 'is a' logic, i.e. if something 'is a/an X' inheritance makes sense. If it's more something 'can do X' using an interface
// to factor out that trait would be more suitable. 
// I'm against dogmatic standpoints. I'm using this one more as a rule of thumb here but happy to be convinced otherwise. 
// One of my fav things is 'Disagree and Commit' - let's discuss it and then decide what's best for the situation.
public class BasePhase
{
    public string Name { get; set; }
    public int Order { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Repetitions { get; set; }
}