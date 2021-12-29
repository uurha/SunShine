using System;
using Base;
using NaughtyCharacter.Scripts.CharacterSettingsModel;
using NaughtyCharacter.Scripts.Utility;
using UnityEngine;

namespace NaughtyCharacter.Scripts.CharacterSystem
{
	public static class CharacterAnimatorParamId
	{
		public static readonly int HorizontalSpeed = Animator.StringToHash("HorizontalSpeed");
		public static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
		public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
	}

    public interface ICharacterAnimator: IInjectable<MovementSettings>, IInjectable<Animator>
    {
        public void UpdateState(Character.Data data);
    }

    [Serializable]
	public class CharacterAnimator : ICharacterAnimator
	{
		private Animator _animator;
        private MovementSettings _movementSettings;

        public void UpdateState(Character.Data characterData)
		{
			var normHorizontalSpeed = characterData.HorizontalVelocity.magnitude / _movementSettings.MaxHorizontalSpeed;
			_animator.SetFloat(CharacterAnimatorParamId.HorizontalSpeed, normHorizontalSpeed);

			var jumpSpeed = _movementSettings.JumpSpeed;
			var normVerticalSpeed = characterData.VerticalVelocity.y.Remap(-jumpSpeed, jumpSpeed, -1.0f, 1.0f);
			_animator.SetFloat(CharacterAnimatorParamId.VerticalSpeed, normVerticalSpeed);

			_animator.SetBool(CharacterAnimatorParamId.IsGrounded, characterData.IsGrounded);
		}

        public void Inject(MovementSettings reference)
        {
            _movementSettings = reference;
        }

        public void Inject(Animator reference)
        {
            _animator = reference;
        }
    }
}
