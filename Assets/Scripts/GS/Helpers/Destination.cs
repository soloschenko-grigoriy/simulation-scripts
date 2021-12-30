using GS.Animals;
using UnityEngine;

namespace GS.Helpers
{
    public static class Destination
    {
        public static Vector3 GetOppositeDirection(IPositionable animal, IPositionable target, float radius)
        {
            var targetPosition = new Vector2(target.Position.x, target.Position.z);
            var currentPosition = new Vector2(animal.Position.x, animal.Position.z);
            
            var dir = (currentPosition - targetPosition).normalized;
            var pos = currentPosition + dir * (radius + 2f);
            var dest = new Vector3(pos.x, animal.Position.y, pos.y);
            
            return dest;
        }
    }
}
