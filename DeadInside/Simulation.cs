#nullable enable
using System;
using System.Collections.Generic;

namespace DeadInside
{
    internal class SimulationResult
    {
        public float FirstDowntimeProbability { get; set; }
        public float SecondDowntimeProbability { get; set; }
        public float AverageTimeInQueue { get; set; }

        public float AverageTimeInSystem { get; set; }
    }

    internal class Simulation
    {
        private readonly Random _random;
        private int _time;

        private int _nextGenerateTime;
        private int _solvedTaskAmount;
        private int _generatedTaskAmount;

        private readonly Queue<Task> _tasks = new Queue<Task>();

        private Task? _firstSolver;
        private Task? _secondSolver;

        private int _firstSolverDowntime;
        private int _secondSolverDowntime;

        private float _queueTime;

        private float _inSystemTime;

        public Simulation(Random random)
        {
            Task.Counter = 0;

            _time = 0;

            _nextGenerateTime = 0;
            _solvedTaskAmount = 0;
            _generatedTaskAmount = 0;

            _random = random;
        }

        public SimulationResult Run()
        {
            while (_solvedTaskAmount < 1000)
            {
                Tick();
            }

            var result = new SimulationResult
            {
                AverageTimeInQueue = _queueTime / _solvedTaskAmount,
                FirstDowntimeProbability = (float) _firstSolverDowntime / _time,
                SecondDowntimeProbability = (float) _secondSolverDowntime / _time,
                AverageTimeInSystem = _inSystemTime / _solvedTaskAmount
            };


            return result;
        }

        private void Tick()
        {
            if (_nextGenerateTime == _time)
            {
                GenerateTask();
                EnqueueTaskGeneration();
            }

            if (_firstSolver == null)
            {
                _firstSolverDowntime++;
            }

            if (_secondSolver == null)
            {
                _secondSolverDowntime++;
            }


            if (_firstSolver?.TimeToSolve == _time)
            {
                Console.WriteLine($"{_time}:: Task — {_firstSolver.Id} solved by 1");
                _solvedTaskAmount++;
                _firstSolver = null;
            }

            if (_secondSolver?.TimeToSolve == _time)
            {
                Console.WriteLine($"{_time}:: Task — {_secondSolver.Id} solved by 2");
                _solvedTaskAmount++;
                _secondSolver = null;
            }

            // Если есть задачи
            if (_tasks.Count > 0)
            {
                // Если меньше двух
                if (_tasks.Count < 2)
                {
                    //То ждем пока освободится первый
                    if (_firstSolver == null)
                    {
                        SolveOnFirst();
                    }
                }
                // Иначе тот кто быстрее освободиться
                else
                {
                    if (_firstSolver == null)
                    {
                        SolveOnFirst();
                    }
                    else if (_secondSolver == null)
                    {
                        SolveOnSecond();
                    }
                }
            }

            _time++;
        }

        private void EnqueueTaskGeneration()
        {
            var timeToGenerateTask = _random.Next(1, 12);
            _nextGenerateTime = _time + timeToGenerateTask;
        }

        private void GenerateTask()
        {
            if (_generatedTaskAmount > 1000)
            {
                _nextGenerateTime = 0;
                return;
            }

            var timeToSovleTask = _random.Next(1, 21);
            var newTask = new Task(timeToSovleTask, _time);

            Console.WriteLine($"{_time}:: Task — {newTask.Id} generated");

            _tasks.Enqueue(newTask);
            _generatedTaskAmount++;
        }

        private void SolveOnFirst()
        {
            var dequeued = _tasks.Dequeue();
            Console.WriteLine($"{_time}:: Task — {dequeued.Id} directed to 1");
            _firstSolver = new Task(dequeued.Id, _time + dequeued.TimeToSolve, dequeued.TimeStamp);

            _queueTime += _time - dequeued.TimeStamp;
            _inSystemTime += _time - dequeued.TimeStamp + dequeued.TimeToSolve;
        }

        private void SolveOnSecond()
        {
            var dequeued = _tasks.Dequeue();
            Console.WriteLine($"{_time}:: Task — {dequeued.Id} directed to 2");
            _secondSolver = new Task(dequeued.Id, _time + dequeued.TimeToSolve, dequeued.TimeStamp);

            _queueTime += _time - dequeued.TimeStamp;
            _inSystemTime += _time - dequeued.TimeStamp + dequeued.TimeToSolve;
        }
    }
}