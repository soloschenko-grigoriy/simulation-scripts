namespace GS.Animals
{
    public enum AnimalSex
    {
        Male,
        Female
    }
    
    public static class AnimalSexExtensions 
    {
        public static AnimalSex GetOpposite(this AnimalSex sex)
        {
            return sex == AnimalSex.Female ? AnimalSex.Male : AnimalSex.Female;
        }
    }
}
