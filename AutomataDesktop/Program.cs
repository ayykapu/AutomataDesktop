using OpenTK.Graphics.ES20;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using static AutomataDesktop.Automata;

namespace AutomataDesktop
{
    public class ProgramStart
    {
        static void Main(string[] args)
        {
            ValueTuple<int, int, byte>[] randomVectorsToFill = GetRandomVectors(160, 90, 500);

            Automata automata = new Automata(160, 90, randomVectorsToFill, RuleSet.FALLINGSAND);

            Window window1 = new Window(1600, 900, "Cellualar Automata", automata);
            window1.Run();
        }

        static ValueTuple<int, int, byte>[] GetRandomVectors(int width, int height, int filledCellsCount)
        {
            List<(int, int, byte)> randomVectorList = new List<(int, int, byte)>();
            Random random = new Random();

            if (filledCellsCount < 1)
            {
                filledCellsCount = random.Next(0, width * height + 1);
            }

            for (int i = 0; i < filledCellsCount; i++)
            {
                randomVectorList.Add((random.Next(0, height), random.Next(0, width), 1));
            }

            randomVectorList.Distinct();

            ValueTuple<int, int, byte>[] randomVectors = randomVectorList.ToArray();

            return randomVectors;
        }
    }
}
