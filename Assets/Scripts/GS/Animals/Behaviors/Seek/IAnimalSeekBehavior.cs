using UnityEngine;

namespace GS.Animals.Behaviors.Seek
{
    public interface IAnimalSeekBehavior
    {
        bool InProgress { get; }
        bool IsCompleted { get; }
        
        void Begin(Vector3 at);
        void Cancel();
    }
}
