using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication31
{
    class Program
    {
        static Color GetmountainBiom(int biomMap, Color origin)
        {

                if (biomMap > 118)
                    return Color.White;
                if (biomMap > 106)
                    return origin;

                if (biomMap < 82)
                    return origin;
                if (biomMap < 96)
                    return origin;
                return origin;
        }
        static Color GetBiom(int biomMap, Color origin, bool overRide)
        {
            if(!overRide)
            {
                if (biomMap > 118)
                    return Color.White;
                if (biomMap > 106)
                    return Color.DarkGreen;


                if (biomMap < 82)
                    return Color.Yellow;

                if (biomMap < 96)
                    return origin;
                return origin;
            }
            else
            {
                if (biomMap > 118)
                    return Color.White;
                if (biomMap > 106)
                    return Color.DarkGreen;

                if (biomMap < 82)
                    return Color.Yellow;
                if (biomMap < 96)
                    return Color.YellowGreen;
                return origin;
            }
            

        }
        static Color GetColor(int input, int biomMap)
        {
            if (input > 130)
                return Color.White;
            if (input > 125)
                return GetmountainBiom(biomMap, Color.Gray);
            if (input > 100)
                return GetBiom(biomMap, Color.Green, true);

            if (input > 97)
                return GetBiom(biomMap, Color.Yellow, false);
            else
            return Color.Blue;
        }
        static int[,] GenerateLayer(int dots, double averagePercentage)
        {
            Random R = new Random();
            int[,] calculated = new int[dots+1, dots + 1];
            for (int i = 0; i < dots + 1; i++)
                for (int j = 0; j < dots + 1; j++)
                    calculated[i, j] = R.Next(0, 256);

            calculated = Average(calculated, dots + 1, averagePercentage);

            int[,] result = new int[1000, 1000];
            //int a1 = R.Next(0,256);
            //int a2 = R.Next(0, 256);
            //int a3 = R.Next(0, 256);
            //int a4 = R.Next(0, 256);
            int t = (1000 / (dots));
            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                {

                    int x = (i / t) * t;
                    int y = (j / t) * t;
                    int a1 = calculated[i / t, j / t];
                    int a2 = calculated[(int)Math.Ceiling(i / (double)t), (int)Math.Ceiling(j / (double)t)];
                    int a3 = calculated[i / t, (int)Math.Ceiling(j / (double)t)];
                    int a4 = calculated[(int)Math.Ceiling(i / (double)t), j / t];


                    result[i, j] = (int)(a1 * (1 - (j - y) / (double)t) * (1 - (i - x) / (double)t) + a2 * ((j - y) / (double)t) * ((i - x) / (double)t) + a3 * ((j - y) / (double)t) * (1 - (i - x) / (double)t) + a4 * ((i - x) / (double)t) * (1 - (j - y) / (double)t));
                }
            return result;
        }
        static int[,] LinearBlur(int[,] input)
        {
            var output = input;

            return output;
        }
        static int[,] Average(int[,] input, int size, double percantage)
        {
            int sr=0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    sr += input[i, j] / input.Length;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    input[i, j] = (int)(input[i, j] - (input[i, j] - sr) * percantage);

            return input;
        }
        static void Main(string[] args)
        {
            Bitmap map = new Bitmap(1000, 1000);
            Random R = new Random();
            
            List<int[,]> list = new List<int[,]>();
            list.Add(GenerateLayer(2,0));
            list.Add(GenerateLayer(5,0));
            list.Add(GenerateLayer(5,0));
            list.Add(GenerateLayer(10,0));
            list.Add(GenerateLayer(50, 0.5d));
            list.Add(GenerateLayer(100,0.7d));

            int[,] result = new int[1000, 1000];
            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                    for (int k = 0; k < list.Count; k++)
                        result[i, j] = list[k][i,j] / list.Count+ result[i, j];


            
            int[,] biomMap = new int[1000, 1000];
            list.Clear();
            list.Add(GenerateLayer(2, 0));
            list.Add(GenerateLayer(5, 0));
            list.Add(GenerateLayer(5, 0));
            list.Add(GenerateLayer(10, 0));
            list.Add(GenerateLayer(50, 0.5d));
            list.Add(GenerateLayer(100, 0.7d));
            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                    for (int k = 0; k < list.Count; k++)
                        biomMap[i, j] = list[k][i, j] / list.Count + biomMap[i, j];

            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                    map.SetPixel(i, j, GetColor(result[i, j], biomMap[i,j]));

            map.Save("output.png");
            System.Diagnostics.Process.Start("output.png");
        }

    }
}
