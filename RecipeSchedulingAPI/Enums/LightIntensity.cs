namespace RecipeSchedulingAPI.Enums;

// REMARK: Always better to define specific values in case you have to add another enum value in the future
// Imagine you add a DISFUNC value as the 'first' value. That would shift the integer value of all other enums if you hadn't specified them 
// explicitly in the first place.
// This could become an issue when you send enum values as integers (like we do for our examples) or store the enum value as integers in your DB. 
public enum LightIntensity
{
    Off = 0,
    Low = 1,
    Medium = 2,
    High = 3
}