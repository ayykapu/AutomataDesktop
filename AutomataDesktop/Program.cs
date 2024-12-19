using OpenTK.Graphics.ES20;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace AutomataDesktop
{
    public class ProgramStart
    {
        static void Main(string[] args)
        {
            List<byte[,]> generationList = new List<byte[,]>();

            int height = 45 * 2;
            int width = 80 * 2;

            byte[,] generation = CreateGeneration(height, width, new int[] { 11, 50, 12, 49, 12, 50, 13, 50, 13, 51 });


            for (int i = 0; i < 350; i++)
            {
                generationList.Add(generation);

                generation = GameOfLife(generation);
            }



            Window window1 = new Window(1600, 900, "Cellualar Automata", generationList);
            window1.Run();
        }
        static byte[,] CreateGeneration(int height, int width, int[] vectorsToFill)
        {
            byte[,] result = new byte[height, width];

            for (int i = 0; i < height; i++) 
            {
                for (int j = 0; j < width; j++)
                {
                    result[i, j] = 0;
                }
            }

            for (int i = 0; i < vectorsToFill.Length; i = i + 2)
            {
                result[vectorsToFill[i], vectorsToFill[i + 1]] = 1;
            }

            return result;
        }
        static byte[,] CreateGenerationCheckerboardPattern(bool startWithEmpty, int height, int width)
        {
            byte[,] result = new byte[height, width];

            byte nextByte;

            if (startWithEmpty) 
            {
                nextByte = 0; 
            }
            else
            {
                nextByte = 1;
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[i, j] = nextByte;

                    if (width % 2 != 0)
                    {
                        if (nextByte == 0)
                        {
                            nextByte = 1;
                        }
                        else
                        {
                            nextByte = 0;
                        }
                    }
                    else
                    {
                        if (j != width - 1)
                        {
                            if (nextByte == 0)
                            {
                                nextByte = 1;
                            }
                            else
                            {
                                nextByte = 0;
                            }
                        }
                        else
                        {
                            if (nextByte == 0)
                            {
                                nextByte = 0;
                            }
                            else
                            {
                                nextByte = 1;
                            }
                        }
                    }
                }
            }

            return result;
        }
        static void DisplayGeneration(byte[,] generation)
        {
            string resultString = "";

            for (int i = 0; i < generation.GetLength(0); i++)
            {
                for (int j = 0; j < generation.GetLength(1); j++)
                {

                    if (generation[i, j] == 0)
                    {
                        resultString += "(0)";
                    }

                    if (generation[i, j] == 1)
                    {
                        resultString += "(1)";
                    }

                    if (generation[i, j] == 2)
                    {
                        resultString += "?";
                    }

                    if (j == generation.GetLength(1) - 1)
                    {
                        resultString += "\n";
                    }
                    else
                    {
                        resultString += " ";
                    }
                }
            }

            Console.WriteLine(resultString);
        }



        //////////////////////////////
      


        static byte[,] SierpinskiRule(byte[,] generation)
        {
            int hight = generation.GetLength(0);
            int width = generation.GetLength(1);

            byte[,] result = CreateGeneration(hight, width, new int[] {} );

            for (int i = 0; i < hight; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byte thisCellState = generation[i, j];

                    ValueTuple<int, int> topRight = GetRight(GetTop((i, j), generation), generation);
                    ValueTuple<int, int> topLeft = GetLeft(GetTop((i, j), generation), generation);

                    byte topRightState = generation[topRight.Item1, topRight.Item2];
                    byte topLeftState = generation[topLeft.Item1, topLeft.Item2];

                    if ((thisCellState == 1) || (topRightState == 1 && topLeftState == 0) || (topRightState == 0 && topLeftState == 1))
                    {
                        result[i, j] = 1;
                    }
                    else
                    {
                        result[i, j] = 0;
                    }
                }
            }

            return result;
        }
        static byte[,] GameOfLife(byte[,] generation)
        {
            int hight = generation.GetLength(0);
            int width = generation.GetLength(1);

            byte[,] result = CreateGeneration(hight, width, new int[] { } );

            for (int i = 0; i < hight; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byte thisCellState = generation[i, j];

                    int stateOneMooreNeighbours = NeighboursStateCount((i, j), 1, false, generation);

                    if
                        ((thisCellState == 1 && stateOneMooreNeighbours == 2) ||
                               (thisCellState == 1 && stateOneMooreNeighbours == 3) ||
                               (thisCellState == 0 && stateOneMooreNeighbours == 3))
                    {
                        result[i, j] = 1;
                    }
                    else
                    {
                        result[i, j] = 0;
                    }
                }
            }

            return result;
        }





        static int NeighboursStateCount(ValueTuple<int, int> vector, byte stateToCheck, bool isVonNeumann, byte[,] generation)
        {
            int result = 0;

            ValueTuple<int, int>[] neighbours = GetNeighbours(vector, isVonNeumann, generation);

            for (int i = 0; i < neighbours.Length; i++)
            {
                byte thisCellState = generation[neighbours[i].Item1, neighbours[i].Item2];

                if (thisCellState == stateToCheck)
                {
                    result++;
                }
            }

            return result;
        }
        static ValueTuple<int, int>[] GetNeighbours(ValueTuple<int, int> vector, bool isVonNeumann, byte[,] generation)
        {
            List<ValueTuple<int, int>> neighbourVectors = new List<ValueTuple<int, int>>();

            ValueTuple<int, int> top, right, left, down, topRight, downRight, downLeft, topLeft;

            top = GetTop(vector, generation);
            neighbourVectors.Add(top);
            right = GetRight(vector, generation);
            neighbourVectors.Add(right);
            left = GetLeft(vector, generation);
            neighbourVectors.Add(left);
            down = GetDown(vector, generation);
            neighbourVectors.Add(down);

            if (!isVonNeumann)
            {
                topRight = GetRight(top, generation);
                neighbourVectors.Add(topRight);
                downRight = GetRight(down, generation);
                neighbourVectors.Add(downRight);
                downLeft = GetLeft(down, generation);
                neighbourVectors.Add(downLeft);
                topLeft = GetLeft(top, generation);
                neighbourVectors.Add(topLeft);
            }

            ValueTuple<int, int>[] result = neighbourVectors.ToArray();

            return result;
        }
        static ValueTuple<int, int> GetRight(ValueTuple<int, int> vector, byte[,] generation)
        {
            int width = generation.GetLength(1);

            if (vector.Item2 == width - 1)
            {
                return (vector.Item1, 0);
            }
            else
            {
                return (vector.Item1, vector.Item2 + 1);
            }

        }
        static ValueTuple<int, int> GetLeft(ValueTuple<int, int> vector, byte[,] generation)
        {
            int width = generation.GetLength(1);

            if (vector.Item2 == 0)
            {
                return (vector.Item1, width - 1);
            }
            else
            {
                return (vector.Item1, vector.Item2 - 1);
            }

        }
        static ValueTuple<int, int> GetTop(ValueTuple<int, int> vector, byte[,] generation)
        {
            int hight = generation.GetLength(0);

            if (vector.Item1 == 0)
            {
                return (hight - 1, vector.Item2);
            }
            else
            {
                return (vector.Item1 - 1, vector.Item2);
            }

        }
        static ValueTuple<int, int> GetDown(ValueTuple<int, int> vector, byte[,] generation)
        {
            int hight = generation.GetLength(0);

            if (vector.Item1 == hight - 1)
            {
                return (0, vector.Item2);
            }
            else
            {
                return (vector.Item1 + 1, vector.Item2);
            }

        }
    }
}
