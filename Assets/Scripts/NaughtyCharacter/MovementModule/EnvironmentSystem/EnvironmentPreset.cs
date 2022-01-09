using CorePlugin.Attributes.EditorAddons.SelectAttributes;
using NaughtyCharacter.AnimationSystem.Interfaces;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem
{
    [CreateAssetMenu(fileName = "EnvironmentPreset", menuName = "Environment/Preset", order = 0)]
    public class EnvironmentPreset : ScriptableObject
    {
        [SerializeReference] [SelectImplementation]
        private IMovementEnvironment environment;
        
        [SerializeReference] [SelectImplementation]
        private ICharacterAnimator characterAnimator;

        public IMovementEnvironment Environment => environment;

        public ICharacterAnimator CharacterAnimator => characterAnimator;
    }
}
