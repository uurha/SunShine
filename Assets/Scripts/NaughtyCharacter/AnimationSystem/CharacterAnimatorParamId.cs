using UnityEngine;

namespace NaughtyCharacter.AnimationSystem
{
    public static class CharacterAnimatorParamId
    {
        public static readonly int HorizontalSpeed = Animator.StringToHash("HorizontalSpeed");
        public static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public static readonly int IsSwimming = Animator.StringToHash("IsSwimming");
    }
}
