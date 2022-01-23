using Base.Inject;
using NaughtyCharacter.CharacterSystem;
using NaughtyCharacter.CharacterSystem.Models;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Models;
using NaughtyCharacter.MovementModule.PlayerSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces
{
    public interface IMovementEnvironment : IInjectReceiver<EnvironmentSettings>,
                                            IInjectReceiver<RotationSettings>, IInjectReceiver<GravitySettings>,
                                            IInjectReceiver<MovementSettings>
    {
        public IAnimatorStateProvider<int> StateProvider { get; }
        
        public void OnEnvironmentChanged(EnvironmentTransferState transferData);

        public EnvironmentTransferState GetTransferState();
        
        public void Initialize();

        public void OnPreMove(float deltaTime);

        public void OnUpdate(IInputAnalyzer.InputAnalyzedData data);

        public Vector3 CalculateMove();

        public void OnPostMove(Character.CharacterData position);

        public Quaternion CalculateRotation(Quaternion transformRotation);

        public Vector2 ClampRotationFunc(Vector2 rotationInput);

        public Vector3 ClampMovementFunc(Vector3 movementInput);
    }
}
