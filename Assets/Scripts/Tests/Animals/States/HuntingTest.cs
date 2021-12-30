using GS.Animals;
using GS.Animals.Behaviors.Chase;
using GS.Animals.Behaviors.Hunger;
using GS.Animals.Behaviors.Seek;
using GS.Animals.Behaviors.Stamina;
using GS.Animals.Behaviors.Thirst;
using GS.Animals.Behaviors.Wander;
using GS.Animals.States;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Tests.Animals.States
{
    public class HuntingTest
    {
        private IAnimal animal;
        private Hunting hunting;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            animal = Substitute.For<IAnimal>();

            animal.Seek.Returns(Substitute.For<IAnimalSeekBehavior>());
            animal.Wander.Returns(Substitute.For<IAnimalWanderBehavior>());
            animal.Chase.Returns(Substitute.For<IAnimalChaseBehavior>());
            animal.Hunger.Returns(Substitute.For<IAnimalHungerBehavior>());
            animal.Thirst.Returns(Substitute.For<IAnimalThirstBehavior>());
            animal.Stamina.Returns(Substitute.For<IAnimalStaminaBehavior>());
        }

        [SetUp]
        public void Setup()
        {
            hunting = new Hunting(animal);
            
            animal.Wander.InProgress.Returns(false);
            animal.Wander.IsScheduled.Returns(false);
            animal.Chase.InProgress.Returns(false);
            animal.NearbyPray.ReturnsNull();
        }

        [TearDown]
        public void TearDown()
        {
            animal.ClearReceivedCalls();
            animal.Seek.ClearReceivedCalls();
            animal.Wander.ClearReceivedCalls();
            animal.Chase.ClearReceivedCalls();
            animal.Hunger.ClearReceivedCalls();
            animal.Thirst.ClearReceivedCalls();
            animal.Stamina.ClearReceivedCalls();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            animal = null;
            hunting = null;
        }

        [Test]
        public void CancelsWanderChaseAndSeekWhenEnters()
        {
            hunting.OnEnter();
            
            animal.Wander.Received().Cancel();
            animal.Seek.Received().Cancel();
            animal.Chase.Received().Cancel();
        }
        
        [Test]
        public void IncreasesHungerAndThirstDrainsStaminaWhenUpdates()
        {
            const float delta = 10f;
            hunting.OnUpdate(delta);

            animal.Thirst.Received(1).Increase(AnimalStatChangeRate.Regular, delta);
            animal.Stamina.Received(1).Drain(AnimalStatChangeRate.Regular, delta);
        }
        
        [Test]
        public void WhenUpdatesStartsWanderingIfNotChasingNorPrayFound()
        {
            hunting.OnUpdate(10f);
            
            animal.Wander.Received(1).Begin();
        }
        
        [Test]
        public void WhenUpdatesDoesNothingIfCurrentlyWandering()
        {
            animal.Wander.InProgress.Returns(true);
            hunting.OnUpdate(10f);
            
            animal.Wander.DidNotReceive().Begin();
            animal.Wander.DidNotReceive().Cancel();
            animal.Chase.DidNotReceive().Begin(default, default);
        }

        [Test]
        public void WhenUpdatesDoesNothingIfWanderingAtLeastScheduled()
        {
            animal.Wander.IsScheduled.Returns(true);
            hunting.OnUpdate(10f);

            animal.Wander.DidNotReceive().Begin();
        }
        
        [Test]
        public void WhenUpdatesStartsChasingIfPrayIsNearby()
        {
            var pray = Substitute.For<IAnimal>();
            animal.NearbyPray.Returns(pray);
            hunting.OnUpdate(10f);
            
            animal.Wander.Received().Cancel();
            animal.Chase.Received().Begin(pray, 0f);
            animal.Wander.DidNotReceive().Begin();
        }
        
        [Test]
        public void WhenUpdatesDoesNothingIfAlreadyChasing()
        {
            var pray = Substitute.For<IAnimal>();
            animal.NearbyPray.Returns(pray);
            animal.Chase.InProgress.Returns(true);
            hunting.OnUpdate(10f);
            
            animal.Wander.DidNotReceive().Cancel();
            animal.Chase.DidNotReceive().Begin(default, default);
            animal.Wander.DidNotReceive().Begin();
        }
        
        [Test]
        public void CancelsWanderAndChaseWhenExits()
        {
            hunting.OnEnter();
            
            animal.Wander.Received().Cancel();
            animal.Chase.Received().Cancel();
        }
    }
}
