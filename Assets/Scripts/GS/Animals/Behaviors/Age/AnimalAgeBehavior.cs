namespace GS.Animals.Behaviors.Age
{
    public class AnimalAgeBehavior : IAnimalAgeBehavior
    {
        private readonly IAnimal animal;
        private float age;
        private readonly float rate;

        public AnimalAgeBehavior(IAnimal animal, float rate)
        {
            this.rate = rate;
            this.animal = animal;
        }

        public AnimalAge Current
        {
            get {
                if (age >= 0.9)
                {
                    return AnimalAge.Elderly;
                }

                if (age >= 0.5)
                {
                    return AnimalAge.Adult;
                }

                if (age >= 0.3)
                {
                    return AnimalAge.YoungAdult;
                }

                if (age >= 0.2)
                {
                    return AnimalAge.Teen;
                }

                if (age >= 0.1)
                {
                    return AnimalAge.Child;
                }

                return AnimalAge.Infant;
            }
        }

        public void Increase(float deltaTime)
        {
            age += rate * deltaTime;
        }

        public void Reset()
        {
            age = 0f;
        }
    }
}
