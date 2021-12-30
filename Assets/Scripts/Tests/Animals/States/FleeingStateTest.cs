using GS.Animals;
using GS.Animals.Behaviors.Hunger;
using GS.Animals.Behaviors.Stamina;
using GS.Animals.Behaviors.Thirst;
using GS.Animals.States;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Animals.States
{
    public class FleeingStateTest
    {
        private IAnimal animal;
        private IAnimal target;
        private Fleeing fleeing;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            animal = Substitute.For<IAnimal>();
            target = Substitute.For<IAnimal>();
            animal.Characteristics.Returns(Substitute.For<AnimalCharacteristics>());
            animal.Hunger.Returns(Substitute.For<IAnimalHungerBehavior>());
            animal.Thirst.Returns(Substitute.For<IAnimalThirstBehavior>());
            animal.Stamina.Returns(Substitute.For<IAnimalStaminaBehavior>());
        }

        [SetUp]
        public void Setup()
        {
            fleeing = new Fleeing(animal);
        }

        [TearDown]
        public void TearDown()
        {
            animal.ClearReceivedCalls();
            animal.Hunger.ClearReceivedCalls();
            animal.Thirst.ClearReceivedCalls();
            animal.Stamina.ClearReceivedCalls();
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            animal = null;
            fleeing = null;
        }

        [Test]
        public void StartsFleeingWhenEnters()
        {
            animal.NearbyPredator.Returns(target);
            
            fleeing.OnEnter();
            
            animal.Flee.Received(1).Begin(target, 0f, 1f);
        }

        [Test]
        public void DrainsStaminaWhenUpdates()
        {
            const float delta = 10f;
            fleeing.OnUpdate(delta);
            
            animal.Stamina.Received(1).Drain(AnimalStatChangeRate.Regular, delta);
        }
        
        [Test]
        public void CancelsFleeWhenExits()
        {
            fleeing.OnExit();
            
            animal.Flee.Received().Cancel();
        }
    }
}
