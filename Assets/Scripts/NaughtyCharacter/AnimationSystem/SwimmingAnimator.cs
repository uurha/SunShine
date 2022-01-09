using System;
using NaughtyCharacter.AnimationSystem.Interfaces;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.AnimationSystem
{
    [Serializable]
    public class SwimmingAnimator : ICharacterAnimator
    {
        private Animator _animator;

        public void UpdateState(IAnimatorStateProvider<int> stateProvider)
        {
            if (stateProvider.TryGetState(CharacterAnimatorParamId.HorizontalSpeed, out FloatState horizontalSpeed))
                _animator.SetFloat(CharacterAnimatorParamId.HorizontalSpeed, horizontalSpeed);

            if (stateProvider.TryGetState(CharacterAnimatorParamId.VerticalSpeed, out FloatState verticalSpeed))
                _animator.SetFloat(CharacterAnimatorParamId.VerticalSpeed, verticalSpeed);

            if (stateProvider.TryGetState(CharacterAnimatorParamId.IsSwimming, out BooleanState swimming))
                _animator.SetBool(CharacterAnimatorParamId.IsSwimming, swimming);
        }

        public void Exit()
        {
            _animator.SetBool(CharacterAnimatorParamId.IsSwimming, false);
        }

        public void Inject(Animator reference)
        {
            _animator = reference;
        }
    }
}
