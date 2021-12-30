namespace GS.Animals
{
    public enum AnimalSpecimen
    {
        Rabbit,
        Wolf
    }
    
    public static class AnimalSpecimenExtensions 
    {
        public static bool IsEqual(this AnimalSpecimen specimen, AnimalSpecimen other)
        {
            return specimen == other;
        }
    }
}
