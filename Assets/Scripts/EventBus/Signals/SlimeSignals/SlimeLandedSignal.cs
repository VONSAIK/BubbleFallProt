using UnityEngine;

namespace CustomEventBus.Signals
{
    public class SlimeLandedSignal
    {
        public Slime Slime { get; }
        public Vector3 Direction { get; }

        public SlimeLandedSignal(Slime slime, Vector3 direction)
        {
            Slime = slime;
            Direction = direction;
        }
    }
}