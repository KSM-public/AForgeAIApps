using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGeneticProject
{
    class IterationDetails
    {
        private int iterationCount, precision;
        private double x, y, fitness;

        public int IterationCount { get => iterationCount; }
        public double Fitness { get => fitness; }

        public double X { get => x; }
        public double Y { get => y; }

        public string FormattedFitness { get => fitness.ToString("N" + precision); }
        public string FormattedX { get => x.ToString("N" + precision); }
        public string FormattedY { get => y.ToString("N" + precision); }
        public int Precision { get => precision; set => precision = value; }

        public IterationDetails(int count, double x, double y, double fitness, int precision)
        {
            iterationCount = count;
            this.x = x;
            this.y = y;
            this.fitness = fitness;
            this.precision = precision;
        }

    }
}
