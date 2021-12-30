using GS.Animals;
using GS.Animals.Behaviors.Flee;
using GS.Animals.Behaviors.Movement;
using GS.Helpers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Animals.Behaviors
{
    public class AnimalFleeBehaviorTest
    {
        private IAnimal animal;
        private IAnimalFleeBehavior flee;
        private IPositionable target;
        private float threshold;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            animal = Substitute.For<IAnimal>();
            animal.Characteristics.Returns(Substitute.For<AnimalCharacteristics>());
            animal.Movement.Returns(Substitute.For<IAnimalMovementBehavior>());
            target = Substitute.For<IPositionable>();
        }

        [SetUp]
        public void Setup()
        {
            target.Position.Returns(new Vector3(10, 0, 10));
            flee = new AnimalFleeBehavior(animal);
            animal.Movement.ClearReceivedCalls();
            animal.NearbyPredator.Returns(Substitute.For<IAnimal>());
            threshold = 10f;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            animal = null;
            flee = null;
        }

        [Test]
        public void StartsMovingAwayFromTargetWhenBegins()
        {
            flee.Begin(target, 0f, threshold);
            animal.Movement.Received().BeginTowards(Destination.GetOppositeDirection(animal, target, animal.Characteristics.awareness));
            Assert.IsFalse(flee.GotCaught);
            Assert.IsFalse(flee.GotAway);
        }

        [Test]
        public void KeepsMovingFromTargetWhenReachesMovementDestButStillIsChased()
        {
            // should update "from destination" constantly with the update cut
            flee.Begin(target, 20f, threshold);
            animal.Movement.ClearReceivedCalls();
            
            // threshold not yet met, skipping
            target.Position.Returns(new Vector3(20, 0, 20));
            flee.Update(10f);
            animal.Movement.DidNotReceive().Stop();
            animal.Movement.DidNotReceive().BeginTowards(Arg.Any<Vector3>());
            Assert.IsTrue(flee.InProgress);
            Assert.IsFalse(flee.GotAway);
            Assert.IsFalse(flee.GotCaught);
            animal.Movement.ClearReceivedCalls();
            
            // now should update
            target.Position.Returns(new Vector3(30, 0, 0));
            flee.Update(20f);
            animal.Movement.Received().Stop();
            animal.Movement.Received().BeginTowards(Destination.GetOppositeDirection(animal, target, animal.Characteristics.awareness));
            Assert.IsFalse(flee.GotAway);
            Assert.IsFalse(flee.GotCaught);
            animal.Movement.ClearReceivedCalls();
            
            // skips again since not enough time has passed
            target.Position.Returns(new Vector3(20, 0, 20));
            flee.Update(30f);
            animal.Movement.DidNotReceive().Stop();
            animal.Movement.DidNotReceive().BeginTowards(Destination.GetOppositeDirection(animal, target, animal.Characteristics.awareness));
            Assert.IsTrue(flee.InProgress);
            Assert.IsFalse(flee.GotAway);
            Assert.IsFalse(flee.GotCaught);
            animal.Movement.ClearReceivedCalls();
            
            // but now should fo it
            target.Position.Returns(new Vector3(20, 0, 20));
            flee.Update(40f);
            animal.Movement.Received().Stop();
            animal.Movement.Received().BeginTowards(Destination.GetOppositeDirection(animal, target, animal.Characteristics.awareness));
            Assert.IsTrue(flee.InProgress);
            Assert.IsFalse(flee.GotAway);
            Assert.IsFalse(flee.GotCaught);
            
            // and again after the next threshold
            target.Position.Returns(new Vector3(20, 0, 20));
            flee.Update(61f);
            animal.Movement.Received().Stop();
            animal.Movement.Received().BeginTowards(Destination.GetOppositeDirection(animal, target, animal.Characteristics.awareness));
            Assert.IsTrue(flee.InProgress);
            Assert.IsFalse(flee.GotAway);
            Assert.IsFalse(flee.GotCaught);
        }
        
        [Test]
        public void StopsWhenCanceled()
        {
            flee.Begin(target, 0f, threshold);
            flee.Cancel();
            animal.Movement.Received().Stop();
            Assert.IsFalse(flee.InProgress);
            Assert.IsFalse(flee.GotCaught);
            Assert.IsFalse(flee.GotAway);
        }

        [Test]
        public void StopsWhenGotAwayFarEnoughFromTarget()
        {
            flee.Begin(target, 0f, threshold);
            animal.NearbyPredator.ReturnsNull();
            
            flee.Update(1f);
            Assert.IsFalse(animal.Movement.InProgress);
            Assert.IsFalse(flee.InProgress);
            Assert.IsFalse(flee.GotCaught);
            Assert.IsTrue(flee.GotAway);
        }

        [Test]
        public void StopsWhenGotCaughtByTarget()
        {
            flee.Begin(target, 0f, threshold);
            animal.Position.Returns(new Vector3(0, 0, 0));
            target.Position.Returns(new Vector3(0, 0, 0));
            
            flee.Update(1f);
            Assert.IsFalse(animal.Movement.InProgress);
            Assert.IsFalse(flee.InProgress);
            Assert.IsTrue(flee.GotCaught);
            Assert.IsFalse(flee.GotAway);
        }
    }
}
 
