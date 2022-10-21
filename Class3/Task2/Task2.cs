using System.Data;
using System.Text;
using OneVariableFunction = System.Func<double, double>;
using FunctionName = System.String;

namespace Task2
{
    public class Task2
    {

/*
 * В этом задании необходимо написать программу, способную табулировать сразу несколько
 * функций одной вещественной переменной на одном заданном отрезке.
 */


// Сформируйте набор как минимум из десяти вещественных функций одной переменной
internal static Dictionary<FunctionName, OneVariableFunction> availableFunctions =
            new Dictionary<FunctionName, OneVariableFunction>
            {
                { "square", x => x * x },
                { "sin", Math.Sin },
                { "cos", Math.Cos },
                { "tg", Math.Tan },
                { "exp", Math.Exp },
                { "log", Math.Log },
                { "sqrt", Math.Sqrt },
                { "round", Math.Round },
                { "atan", Math.Atan},
                { "acos", Math.Acos},
                { "arctg", Math.Atan},
            };

// Тип данных для представления входных данных
internal record InputData(double FromX, double ToX, int NumberOfPoints, List<string> FunctionNames);

// Чтение входных данных из параметров командной строки
        private static InputData? PrepareData(string[] args)
        {
            if (args.Length < 4) return null;
            double FromX, ToX;
            if (!double.TryParse(args[0], out FromX)) return null;
            if (!double.TryParse(args[1], out ToX)) return null;
            int numberOfPoints;
            if (!Int32.TryParse(args[2], out numberOfPoints)) return null;
            List<String> FunctionNames = new List<string>();
            for (int i = 4; i < args.Length; i++) FunctionNames.Add(args[i]);
            return new InputData(FromX, ToX, numberOfPoints, FunctionNames);
        }

// Тип данных для представления таблицы значений функций
// с заголовками столбцов и строками (первый столбец --- значение x,
// остальные столбцы --- значения функций). Одно из полей --- количество знаков
// после десятичной точки.
internal record FunctionTable
{
    private List<string> functionNames;
    private List<double> args;

    private int numberOfDigits;

    public FunctionTable(List<string> functions, List<double> arguments, int DigitsCount)
    {
        functionNames = functions;
        args = arguments;
        numberOfDigits = DigitsCount;
    }
        // Код, возвращающий строковое представление таблицы (с использованием StringBuilder)
        // Столбец x выравнивается по левому краю, все остальные столбцы по правому.
        // Для форматирования можно использовать функцию String.Format.
        public override string ToString()
        {
            List<StringBuilder> rows = new List<StringBuilder>();

            rows.Add(new StringBuilder("Argument"));
            for (int i = 0; i < args.Count; i++)
            {
                rows.Add(new StringBuilder(args[i].ToString("N" + numberOfDigits)));
            }

            for (int j = 0; j < functionNames.Count; j++)
            {
                List<string> adding = new List<string>();
                adding.Add(functionNames[j]);
                Func<double, double> function = availableFunctions[functionNames[j]];
                for (int i = 0; i < args.Count; i++)
                {
                    double res = function(args[i]);
                    adding.Add(res.ToString("N" + numberOfDigits));
                }

                int maxLenght = 0;

                for (int i = 0; i < adding.Count; i++)
                {
                    int len = rows[i].Length + adding[i].Length + 1;
                    if (len > maxLenght)
                    {
                        maxLenght = len;
                    }
                }

                for (int i = 0; i < adding.Count; i++)
                {
                    adding[i] = adding[i].PadLeft(maxLenght - rows[i].Length);
                    rows[i].Append(adding[i]);
                }
            }
            
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < rows.Count; i++)
            {
                if (i != rows.Count - 1)
                {
                    rows[i].Append(Environment.NewLine);
                }
                result.Append(rows[i]);
            }

            return result.ToString();
        }
    }

/*
 * Возвращает таблицу значений заданных функций на заданном отрезке [fromX, toX]
 * с заданным количеством точек.
 */
        internal static FunctionTable Tabulate(InputData input)
        {
            List<double> args = new List<double>();
            double step;
            if (input.NumberOfPoints < 2) step = 1;
            else step = (input.ToX - input.FromX) / (input.NumberOfPoints - 1);
            double from = input.FromX;
            for (int i = 0; i < input.NumberOfPoints; i++)
            {
                args.Add(from);
                from += step;
            }

            return new FunctionTable(input.FunctionNames, args, 2);
        }
        
        public static void Main(string[] args)
        {
            // Входные данные принимаются в аргументах командной строки
            // fromX fromY numberOfPoints function1 function2 function3 ...

            var input = PrepareData(args);

            // Собственно табулирование и печать результата (что надо поменять в этой строке?):
            Console.WriteLine(Tabulate(input).ToString());
        }
        
        private static T TODO<T>()
        {
            throw new NotImplementedException();
        }

    }
}