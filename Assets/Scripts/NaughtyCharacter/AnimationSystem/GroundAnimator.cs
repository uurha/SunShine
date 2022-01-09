using System;
using NaughtyCharacter.AnimationSystem.Interfaces;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.AnimationSystem
{
    [Serializable]
	public class GroundAnimator : ICharacterAnimator
	{
		private Animator _animator;

        public void UpdateState(IAnimatorStateProvider<int> stateProvider)
		{
            if (stateProvider.TryGetState(CharacterAnimatorParamId.HorizontalSpeed, out FloatState horizontalSpeed))
                _animator.SetFloat(CharacterAnimatorParamId.HorizontalSpeed, horizontalSpeed);

            if (stateProvider.TryGetState(CharacterAnimatorParamId.VerticalSpeed, out FloatState verticalSpeed))
                _animator.SetFloat(CharacterAnimatorParamId.VerticalSpeed, verticalSpeed);
            
            if (stateProvider.TryGetState(CharacterAnimatorParamId.IsGrounded, out BooleanState grounded))
                _animator.SetBool(CharacterAnimatorParamId.IsGrounded, grounded);
		}

        public void Exit()
        {
            _animator.SetBool(CharacterAnimatorParamId.IsGrounded, false);
        }

        public void Inject(Animator reference)
        {
            _animator = reference;
        }
    }
}
