namespace CustomEventBus.Signals
{
    public class ScoreUpdatedSignal
    {
        public int Score;

        public ScoreUpdatedSignal(int score)
        {
            Score = score;
        }
    }
}