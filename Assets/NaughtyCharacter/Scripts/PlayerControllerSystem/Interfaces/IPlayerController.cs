using Base;
using NaughtyCharacter.Scripts.CharacterSystem;

namespace NaughtyCharacter.Scripts.PlayerControllerSystem.Interfaces
{
    public interface IPlayerController : IInjectable<Character>, IInjectable<PlayerInputComponent>
    {
        public void OnCharacterUpdate();
    }
}
