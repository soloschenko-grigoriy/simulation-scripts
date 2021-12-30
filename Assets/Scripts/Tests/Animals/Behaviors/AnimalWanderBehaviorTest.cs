using GS.Animals;
using GS.Animals.Behaviors.Movement;
using GS.Animals.Behaviors.Wander;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Animals.Behaviors
{
    public class AnimalWanderBehaviorTest
    {
        private IAnimal animal;
        private AnimalWanderBehavior wander;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            animal = Substitute.For<IAnimal>();
            animal.Movement.Returns(Substitute.For<IAnimalMovementBehavior>());
        }
        
        [SetUp]
        public void Setup()
        {
            wander = new AnimalWanderBehavior(animal);
            animal.Movement.ClearReceivedCalls();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            animal = null;
            wander = null;
        }


        [Test]
        public void StopsWhenCanceled()
        {
            Assert.IsFalse(wander.IsScheduled);
            animal.Movement.DidNotReceiveWithAnyArgs().BeginTowards(default);

            wander.Begin();
            Assert.IsTrue(wander.IsScheduled);

            wander.Cancel();
            Assert.IsFalse(wander.IsScheduled);
            animal.Movement.Received().Stop();
        }
    }
}
