// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using TripolisInc.EcsCore.Interfaces;
using UnityEngine;

namespace TripolisInc.EcsCore.GameComponent
{
    public abstract class EcsMonoBehavior : MonoBehaviour, IMonoBehavior
    {
        public virtual bool HasInstance() => this != null;
    }
}