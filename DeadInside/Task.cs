namespace DeadInside
{
    public class Task
    {
        public static int Counter = 0;

        public readonly int Id;
        public readonly int TimeToSolve;
        public readonly int TimeStamp;

        public Task(int timeToSolve, int timeStamp)
        {
            Id = Counter++;
            TimeToSolve = timeToSolve;
            TimeStamp = timeStamp;
        }

        public Task(int id, int timeToSolve, int timeStamp)
        {
            Id = id;
            TimeToSolve = timeToSolve;
            TimeStamp = timeStamp;
        }
    }
}