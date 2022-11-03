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
    class Genetic
    {
        private DoubleArrayChromosome doubleArrayChromosome;
        private Population population;
        private int iterations, precision;
        private List<IterationDetails> iterationsDetailsList;

        public Genetic(string expression, int populationCount, int iterations, double mutationRate, double crossoverRate,  int minRange, int maxRange, int precision, 
            List<IterationDetails> iterationsDetailsList, int selectionMethod)
        {
            doubleArrayChromosome = new DoubleArrayChromosome(new AForge.Math.Random.UniformGenerator(new Range(minRange, maxRange)), 
                new AForge.Math.Random.UniformGenerator(new Range(0.1f, 0.5f)), 
                new AForge.Math.Random.UniformGenerator(new Range(0.1f, 0.5f)), 2);
            
            switch (selectionMethod)
            {
                case 0:
                    population = new Population(populationCount, doubleArrayChromosome, new FitnessFunction(expression), new EliteSelection());
                    break;

                case 1:
                    population = new Population(populationCount, doubleArrayChromosome, new FitnessFunction(expression), new RankSelection());
                    break;

                case 2:
                    population = new Population(populationCount, doubleArrayChromosome, new FitnessFunction(expression), new RouletteWheelSelection());
                    break;

                default:
                    population = new Population(populationCount, doubleArrayChromosome, new FitnessFunction(expression), new EliteSelection());
                    break;
            }

            population.MutationRate = mutationRate;
            population.CrossoverRate = crossoverRate;

            this.iterations = iterations;
            this.precision = precision;

            this.iterationsDetailsList = iterationsDetailsList;
        }

        public void Run()
        {
            for (int i = 0; i < iterations; i++)
            {
                population.RunEpoch();

                DoubleArrayChromosome bestChromosome = new DoubleArrayChromosome((DoubleArrayChromosome)population.BestChromosome);

                IterationDetails iterationDetails = new IterationDetails(i + 1, bestChromosome.Value[0], bestChromosome.Value[1], bestChromosome.Fitness, precision);

                iterationsDetailsList.Add(iterationDetails);
            }
        }
    }
}
