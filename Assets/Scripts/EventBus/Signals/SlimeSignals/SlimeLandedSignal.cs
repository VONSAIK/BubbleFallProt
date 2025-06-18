using UnityEngine;

namespace CustomEventBus.Signals
{
    public class SlimeLandedSignal
    {
        public Slime Slime;
        public Vector3 Direction;

        public SlimeLandedSignal(Slime slime, Vector3 direction)
        {
            Slime = slime;
            Direction = direction;
        }
    }
}