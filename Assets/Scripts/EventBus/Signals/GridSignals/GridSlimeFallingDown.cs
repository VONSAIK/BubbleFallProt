using System.Collections.Generic;

namespace CustomEventBus.Signals
{
    public class GridSlimeFallingDown
    {
        public List<Slime> DetachedSlimes { get; }

        public GridSlimeFallingDown(List<Slime> slimes)
        {
            DetachedSlimes = slimes;
        }
    }

}
