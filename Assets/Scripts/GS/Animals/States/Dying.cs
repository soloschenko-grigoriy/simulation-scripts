namespace GS.Animals.States
{
    public class Dying : State
    {
        public Dying(IAnimal animal) : base(animal) { }

        public override void OnEnter()
        {
            animal.Breed.Cancel();
            animal.Wander.Cancel();
            animal.Seek.Cancel();
            animal.Chase.Cancel();
            
            // when animation is completed it will call Animal.Die
            animal.Animator.SetBool(AnimalAnimatorState.IsDying.AsString(), true);
        }
    }
}
