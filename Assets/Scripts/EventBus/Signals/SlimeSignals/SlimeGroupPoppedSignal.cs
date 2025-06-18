using System.Collections.Generic;
using UnityEngine;

namespace CustomEventBus.Signals
{
    public class SlimeGroupPoppedSignal
    {
        public HashSet<Vector2Int> Group { get; }

        public SlimeGroupPoppedSignal(HashSet<Vector2Int> group)
        {
            Group = group;
        }
    }
}
