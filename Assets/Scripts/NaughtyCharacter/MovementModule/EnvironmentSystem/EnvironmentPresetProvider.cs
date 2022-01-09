using Base;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem
{
    public class EnvironmentPresetProvider : MonoBehaviour, IProvider<EnvironmentPreset>
    {
        [SerializeField] private EnvironmentPreset environmentPreset;
        
        public EnvironmentPreset Provided => environmentPreset;
    }
}
