using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;
using AForge.Math;
using AForge.Genetic;
using info.lundin.math;

namespace AIGeneticProject
{
    class FitnessFunction : IFitnessFunction
    {
        private string expression;

        public FitnessFunction(string expression)
        {
            this.expression = expression;
        }

        public double Evaluate(IChromosome chromosome)
        {
            DoubleArrayChromosome doubleArrayChromosome = new DoubleArrayChromosome((DoubleArrayChromosome)chromosome);
            ExpressionParser expressionParser = new ExpressionParser();
            double x = doubleArrayChromosome.Value[0];
            double y = doubleArrayChromosome.Value[1];

            string valueX = x.ToString().Replace(",", ".");
            string valueY = y.ToString().Replace(",", ".");

            expressionParser.Values.Add("x", valueX);
            expressionParser.Values.Add("y", valueY);

            double result = expressionParser.Parse(expression);

            return 100 / (Math.Abs(result) + 1);
        }
    }
}
