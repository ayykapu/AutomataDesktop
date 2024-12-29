using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataDesktop
{
    public class Automata
    {
        public int _generationWidth;
        public int _generationHeight;
        public RuleSet _ruleSet;
        public List<byte[,]> _sequence = new List<byte[,]>();

        public Automata(int width, int height, ValueTuple<int, int, byte>[] vectors, RuleSet ruleSet)
        {
            _generationWidth = width;
            _generationHeight = height;

            _ruleSet = ruleSet;

            _sequence.Add(SetGenerationZero(vectors));
        }

        byte[,] SetGenerationZero(ValueTuple<int, int, byte>[] vectors)
        {
            byte[,] result = new byte[_generationHeight, _generationWidth];

            foreach (var vector in vectors) 
            {
                result[vector.Item1, vector.Item2] = vector.Item3;
            }

            return result;
        }

        public void AddNextGeneration()
        {
            switch (_ruleSet)
            {
                case RuleSet.FALLINGSAND:
                    _sequence.Add(FallingSandRule(_sequence[_sequence.Count - 1]));
                    break;
                case RuleSet.GAMEOFLIFE:
                    //_sequence.Add(GameOfLifeRule(_sequence[_sequence.Count - 1]));
                    break;
            }
        }
        public void GetMoreGenerations(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.AddNextGeneration();
            }
        }
        byte[,] FallingSandRule(byte[,] generation)
        {
            byte[,] result = new byte[_generationHeight, _generationWidth];

            for (int i = 0; i < _generationHeight; i++)
            {
                for (int j = 0; j < _generationWidth; j++)
                {
                    byte thisCellState = generation[i, j];

                    byte topState, bottomState;

                    if (i == 0)
                    {
                        topState = generation[_generationHeight - 1, j];
                    } 
                    else
                    {
                        topState = generation[i - 1, j];
                    }

                    if (i == _generationHeight - 1)
                    {
                        bottomState = generation[0, j];
                    }
                    else
                    {
                        bottomState = generation[i + 1, j];
                    }

                    if (i == 0)
                    {
                        result[i, j] = 0;
                    }
                    else if (i > 0 && i < _generationHeight - 1)
                    {
                        if (topState == 1 && thisCellState == 0 || bottomState == 1 && thisCellState == 1)
                        {
                            result[i, j] = 1;
                        }
                        else
                        {
                            result[i, j] = 0;
                        }
                    }
                    else
                    {
                        if (topState == 1 || thisCellState == 1)
                        {
                            result[i, j] = 1;
                        }
                        else
                        {
                            result[i, j] = 0;
                        }
                    }
                }
            }

            return result;
        }

        public enum RuleSet
        {
            FALLINGSAND,
            GAMEOFLIFE,
        }
    }
}
