namespace CustomEventBus.Signals
{
    public class SlimeLandedSignal
    {
        public readonly Slime Slime;

        public SlimeLandedSignal(Slime slime)
        {
            Slime = slime;
        }
    }
}