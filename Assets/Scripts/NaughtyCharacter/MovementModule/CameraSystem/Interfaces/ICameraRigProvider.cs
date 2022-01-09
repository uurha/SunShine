using Base;
using CorePlugin.ReferenceDistribution.Interface;
using UnityEngine;

namespace NaughtyCharacter.MovementModule.CameraSystem.Interfaces
{
    public interface ICameraRigProvider : IProvider<Transform>, IDistributingReference
    {
        
    }
}
