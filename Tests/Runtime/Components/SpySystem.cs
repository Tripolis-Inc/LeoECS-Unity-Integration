// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using Leopotam.EcsLite;

namespace Tests.Runtime.Components
{
    public class SpySystem : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem, IEcsPreInitSystem, IEcsPostDestroySystem, IEcsPostRunSystem
    {
        public bool IsInited = false;
        public bool IsPreInited = false;
        public bool IsDestroyed = false;
        public bool IsPostDestroyed = false;

        public int RunCount = 0;
        public int PostRunCount = 0;
        
        public void Init(IEcsSystems systems)
        {
            IsInited = true;
        }

        public void Destroy(IEcsSystems systems)
        {
            IsDestroyed = true;
        }

        public void Run(IEcsSystems systems)
        {
            RunCount++;
        }

        public void PreInit(IEcsSystems systems)
        {
            IsPreInited = true;
        }

        public void PostDestroy(IEcsSystems systems)
        {
            IsPostDestroyed = true;
        }

        public void PostRun(IEcsSystems systems)
        {
            PostRunCount++;
        }
    }
}