using Base.Inject;
using NaughtyCharacter.MovementModule.EnvironmentSystem.Interfaces;
using UnityEngine;

namespace NaughtyCharacter.AnimationSystem.Interfaces
{
    public interface ICharacterAnimator: IInjectReceiver<Animator>
    {
        public void UpdateState(IAnimatorStateProvider<int> stateProvider);

        public void Exit();
    }
}
