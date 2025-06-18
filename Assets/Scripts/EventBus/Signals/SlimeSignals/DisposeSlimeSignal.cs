namespace CustomEventBus.Signals
{
    public class DisposeSlimeSignal
    {
        public readonly Slime Slime;

        public DisposeSlimeSignal(Slime slime)
        {
            Slime = slime;
        }
    }
}