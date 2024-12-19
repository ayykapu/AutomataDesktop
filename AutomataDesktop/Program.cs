using OpenTK.Graphics.ES20;
using System.Runtime.CompilerServices;

namespace AutomataDesktop
{
    public class ProgramStart
    {
        static void Main(string[] args)
        {
            List<byte[,]> generationList = new List<byte[,]>();

            byte[,] g1 = CreateGenerationCheckerboardPattern(true, 45, 80);
            byte[,] g2 = CreateGenerationCheckerboardPattern(false, 45, 80);

            generationList.Add(g1);
            generationList.Add(g2);
            
            Window window1 = new Window(1600, 900, "Cellualar Automata", generationList);
            window1.Run();
        }

        private static int[] TranslateBytesIntoInts(byte[,] generation)
        {
            int cellSize = 1;

            List<int> result = new List<int>();

            for (int i = 0; i < generation.GetLength(0) * cellSize; i++)
            {
                for (int j = 0; j < generation.GetLength(1) * cellSize; j++)
                {
                    if (generation[i, j] != 0)
                    {
                        result.Add(j);
                        result.Add(i);
                    }
                }
            }

            return result.ToArray();
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
            int height = generation.GetLength(0);
            int width = generation.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(generation[i, j] + " ");
                }
                Console.Write("\n");
            }

            Console.Write("\n");
        }
    }
}
