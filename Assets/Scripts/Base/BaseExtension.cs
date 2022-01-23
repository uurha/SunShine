using NaughtyCharacter.CharacterSystem;
using NaughtyCharacter.MovementModule.EnvironmentSystem;

namespace Base
{
    public static class BaseExtension
    {
        public static void Set(this Character character, EnvironmentPreset preset)
        {
            character.Set(preset.Settings);
            character.Set(preset.Environment);
            character.Set(preset.CharacterAnimator);
        }
    }
}
