using GS.Animals;
using GS.Animals.Behaviors.Hunger;
using GS.Animals.Behaviors.Stamina;
using GS.Animals.Behaviors.Thirst;
using GS.Animals.Behaviors.Wander;
using GS.Animals.States;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Animals.States
{
    public class RestingTest
    {
        private IAnimal animal;
        private Resting resting;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            animal = Substitute.For<IAnimal>();
            resting = new Resting(animal);

            animal.Wander.Returns(Substitute.For<IAnimalWanderBehavior>());
            animal.Hunger.Returns(Substitute.For<IAnimalHungerBehavior>());
            animal.Thirst.Returns(Substitute.For<IAnimalThirstBehavior>());
            animal.Stamina.Returns(Substitute.For<IAnimalStaminaBehavior>());
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            animal = null;
        }


        [Test]
        public void OnEnter()
        {
            resting.OnEnter();
            animal.Wander.Received().Begin();
        }

        [Test]
        public void OnUpdate()
        {
            resting.OnUpdate(0);
            animal.Hunger.Received().Increase(AnimalStatChangeRate.Regular, 0f);
            animal.Thirst.Received().Increase(AnimalStatChangeRate.Regular, 0f);
            animal.Stamina.Received().Replenish(AnimalStatChangeRate.Intense, 0f);
        }

        [Test]
        public void OnExit()
        {
            resting.OnExit();
            animal.Wander.Received().Cancel();
        }
    }
}
