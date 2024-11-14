using System;
using UnityEngine;

namespace EmptyCommon.Animations
{
    public interface IAnimationEventExecuter : IDisposable
    {
        void Execute(in AnimationEvent @event);
    }
}
