using UnityEngine.PlayerLoop;

namespace Abg
{
    public static class Loops
    {
        public static readonly ILoopSystem PreUpdate = new LoopSystem<PreUpdate>();
        public static readonly ILoopSystem Update = new LoopSystem<Update>();
        public static readonly ILoopSystem FixedUpdate = new LoopSystem<FixedUpdate>();
        public static readonly ILoopSystem PreLateUpdate = new LoopSystem<PreLateUpdate>();
        public static readonly ILoopSystem PostLateUpdate = new LoopSystem<PostLateUpdate>();
    }
}