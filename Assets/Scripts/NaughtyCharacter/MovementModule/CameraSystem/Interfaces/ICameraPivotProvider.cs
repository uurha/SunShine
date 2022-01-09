using Base;
using CorePlugin.ReferenceDistribution.Interface;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.CameraSystem.Interfaces
{
    public interface ICameraPivotProvider : IProvider<Transform>, IDistributingReference
    {
        
    }
}
