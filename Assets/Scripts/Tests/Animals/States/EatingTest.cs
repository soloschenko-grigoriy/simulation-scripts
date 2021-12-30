using GS.Animals;
using GS.Animals.Behaviors.Hunger;
using GS.Animals.Behaviors.Stamina;
using GS.Animals.Behaviors.Thirst;
using GS.Animals.States;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Tests.Animals.States
{
    public class EatingTest
    {
        private IAnimal animal;
        private Eating eating;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            animal = Substitute.For<IAnimal>();
            animal.Hunger.Returns(Substitute.For<IAnimalHungerBehavior>());
            animal.Thirst.Returns(Substitute.For<IAnimalThirstBehavior>());
            animal.Stamina.Returns(Substitute.For<IAnimalStaminaBehavior>());
        }

        [SetUp]
        public void Setup()
        {
            eating = new Eating(animal);
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
            eating = null;
        }

        [Test]
        public void CancelsWanderChaseAndSeekWhenEnters()
        {
            const float deltaTime = 10f;
            eating.OnUpdate(deltaTime);
            
            animal.Hunger.Received().Decrease(AnimalStatChangeRate.Intense, deltaTime);
            animal.Thirst.Received().Decrease(AnimalStatChangeRate.Regular, deltaTime);
            animal.Stamina.Received().Replenish(AnimalStatChangeRate.Medium, deltaTime);
        }

        [Test]
        public void DevoursPrayWhenEnters()
        {
            var pray = Substitute.For<IAnimal>();
            animal.NearbyPray.Returns(pray);
            
            Assert.IsFalse(pray.BeingDevoured);
            eating.OnEnter();
            Assert.IsTrue(pray.BeingDevoured);
        }
    }
}
