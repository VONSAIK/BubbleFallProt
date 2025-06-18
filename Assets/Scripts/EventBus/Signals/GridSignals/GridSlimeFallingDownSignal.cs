using System.Collections.Generic;

namespace CustomEventBus.Signals
{
    public class GridSlimeFallingDownSignal
    {
        public List<Slime> Slimes;

        public GridSlimeFallingDownSignal(List<Slime> slimes)
        {
            Slimes = slimes;
        }
    }

}
