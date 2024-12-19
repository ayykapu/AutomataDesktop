using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace AutomataDesktop
{
    public class ProgramStart
    {
        static void Main(string[] args)
        {
            List<byte[,]> generationList = new List<byte[,]>();

            int height = 1080 / 2;
            int width = 1920 / 2;
            byte[,] generation1 = CreateGenerationCheckerboardPattern(false, height, width);
            byte[,] generation2 = CreateGenerationCheckerboardPattern(true, height, width);

            generationList.Add(generation1);
            generationList.Add(generation2);

            Window window1 = new Window(1920, 1080, "TEST", generationList);
            window1.Run();

            DisplayGeneration(generation1);
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
