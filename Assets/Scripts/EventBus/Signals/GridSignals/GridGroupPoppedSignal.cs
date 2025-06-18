using System.Collections.Generic;
using UnityEngine;

namespace CustomEventBus.Signals
{
    public class GridGroupPoppedSignal
    {
        public HashSet<Vector2Int> Group { get; }

        public GridGroupPoppedSignal(HashSet<Vector2Int> group)
        {
            Group = group;
        }
    }
}
