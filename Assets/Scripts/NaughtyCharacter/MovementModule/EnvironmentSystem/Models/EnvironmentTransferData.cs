using System;
using Base.Inject;
using NaughtyCharacter.CharacterSystem.Models;
using NaughtyCharacter.MovementModule.PlayerSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.EnvironmentSystem.Models
{
    public readonly struct EnvironmentTransferState
    {
        public float VerticalSpeed { get; }

        public float HorizontalSpeed { get; }

        public IInputAnalyzer.InputAnalyzedData AnalyzedInputData { get; }
        
        public EnvironmentTransferState(float verticalSpeed, float horizontalSpeed, IInputAnalyzer.InputAnalyzedData analyzedInputData)
        {
            VerticalSpeed = verticalSpeed;
            HorizontalSpeed = horizontalSpeed;
            AnalyzedInputData = analyzedInputData;
        }
    }
}
