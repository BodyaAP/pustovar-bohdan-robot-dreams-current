using System.Collections;
using System.Collections.Generic;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    public interface IBasePlayerService : IService
    {
        BasePlayerController Base { get; }
        bool IsBase(Collider collider);
    }
}
