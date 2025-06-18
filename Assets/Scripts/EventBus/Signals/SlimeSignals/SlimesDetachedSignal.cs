using System.Collections.Generic;

namespace CustomEventBus.Signals
{
    public class SlimesDetachedSignal
    {
        public List<Slime> DetachedSlimes { get; }

        public SlimesDetachedSignal(List<Slime> slimes)
        {
            DetachedSlimes = slimes;
        }
    }

}
