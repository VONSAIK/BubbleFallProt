using UnityEngine;

namespace CustomEventBus.Signals
{
    public class SlimeLaunchedSignal
    {
        public Slime Slime;
        public Vector3 Direction;

        public SlimeLaunchedSignal(Slime slime, Vector3 direction)
        {
            Slime = slime;
            Direction = direction;
        }
    }
}