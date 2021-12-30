using System;

namespace GS.Animals
{
    public enum AnimalAnimatorState
    {
        IsWalking,
        IsRunning,
        IsDying
    }
    
    public static class AnimalAnimatorStateExtensions 
    {
        public static string AsString(this AnimalAnimatorState state)
        {
            return state switch {
                AnimalAnimatorState.IsWalking => "isJumping",
                AnimalAnimatorState.IsRunning => "isRunning",
                AnimalAnimatorState.IsDying => "isDead_1",
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}

