using System;
using System.Collections.Generic;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces
{
    public class AnimatorStateProvider : IAnimatorStateProvider<int>
    {
        private readonly Dictionary<int, IState> _states = new Dictionary<int, IState>();
        
        public bool TryGetState<T>(int stateName, out T data) where T : IState
        {
            var tryGetValue = _states.TryGetValue(stateName, out var value);

            data = (T)value;
            return tryGetValue;
        }

        public void SetState<T>(int stateName, T data) where T : IState
        {
            if (_states.ContainsKey(stateName))
            {
                _states[stateName] = data;
            }
            else
            {
                _states.Add(stateName, data);
            }
        }
    }

    public interface IAnimatorStateProvider<in TKey>
    {
        public bool TryGetState<T>(TKey stateName, out T data) where T : IState;

        public void SetState<T>(TKey stateName, T data) where T : IState;
    }
    
    [Serializable]
    public struct BooleanState : IState
    {
        public BooleanState(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public static implicit operator bool(BooleanState state)
        {
            return state.Value;
        }
        
        public static implicit operator BooleanState(bool state)
        {
            return new BooleanState(state);
        }
    }
    
    [Serializable]
    public struct FloatState : IState
    {
        public FloatState(float value)
        {
            Value = value;
        }

        public float Value { get; }

        public static implicit operator float(FloatState state)
        {
            return state.Value;
        }
        
        public static implicit operator FloatState(float state)
        {
            return new FloatState(state);
        }
    }

    public interface IState
    {
    }
}
