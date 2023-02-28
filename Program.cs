// Задача 62. Напишите программу, которая заполнит спирально массив 4 на 4.
// Например, на выходе получается вот такой массив:
// 01 02 03 04
// 12 13 14 05
// 11 16 15 06
// 10 09 08 07

namespace HW62
{
    class ConsoleApp
    {
        static void Main()
        {
            Console.WriteLine("Welcome to 3d array generator!");
            Console.WriteLine("Here comes your array:");
            var arr = new MatrixBuilder(6);
            arr.SetSnakeValues();
            Console.WriteLine(arr.ToString());
        }
    }

    public class MatrixBuilder
    {
        private double[,] _arr;
        private bool _isInitialized = false;
        private Random _rnd;
        private int _count = 0;
        private int _filledTopRows = 0;
        private int _filledBottomRows = 0;
        private int _filledLeftCols = 0;
        private int _filledRightCols = 0;
        private bool _lastRightCol = false;
        private bool _lastTopRow = false;

        private HashSet<double> _values = new HashSet<double>();
        public int Size { get; private set; }

        public MatrixBuilder(int size)
        {
            _arr = new double[size, size];
            Size = size;
            _rnd = new Random();
            _isInitialized = true;
        }

        public override string ToString()
        {
            return _arr.ToArrString();
        }

        public void SetRandomValues()
        {
            var rnd = new Random();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _arr[i, j] = GetNextRandom();
                }
            }
        }

        public void SetSnakeValues()
        {
            SetLine(1, -1, 0, Size, true);
        }

        private void SetLine(int row, int col, int startIndex, int endIndex, bool isRow)
        {
            if (startIndex == endIndex) return;
            if (isRow)
            {
                for (var i = startIndex; _lastRightCol ? i > endIndex : i < endIndex; )
                {
                    _arr[row - 1, i] = GetNext();
                    i = _lastRightCol ? i - 1 : i + 1;
                }


                var nextStartIndex = !_lastTopRow ? row : row - 2;
                var nextEndIndex = !_lastTopRow ? Size - _filledBottomRows : _filledTopRows - 1;
                var nextCol = _lastTopRow ? endIndex + 2 : endIndex;
                if (_lastTopRow) 
                {
                    _filledBottomRows++;
                }
                else
                {
                    _filledTopRows++;
                }

                _lastTopRow = !_lastTopRow;
                SetLine(-1, nextCol, nextStartIndex, nextEndIndex, false);
            }
            else
            {
                for (var i = startIndex; _lastTopRow ? i < endIndex : i > endIndex; )
                {
                    _arr[i, col - 1] = GetNext();
                    i = _lastTopRow ? i + 1 : i - 1;
                }

                var nextStartIndex = !_lastRightCol ? col - 2 : col;
                var nextEndIndex = !_lastRightCol ? _filledLeftCols - 1 : Size - _filledRightCols;
                var nextRow = _lastRightCol ? endIndex + 2 : endIndex;
                if (_lastRightCol)
                {
                    _filledLeftCols++;
                }
                else
                {
                    _filledRightCols++;
                }

                _lastRightCol = !_lastRightCol;
                SetLine(nextRow, -1, nextStartIndex, nextEndIndex, true);
            }
        }

        private int GetNext()
        {
            _count++;
            return _count;
        }

        private double GetNextRandom()
        {
            var signPow = _rnd.Next(1, 3);
            var tenPow = _rnd.Next(0, 3);
            var doubleValue = _rnd.NextDouble();
            var sign = ((double)Math.Pow(-1, signPow));
            var tens = ((double)Math.Pow(10, tenPow));
            var roundCount = _rnd.Next(0, 3);
            var newValue = Math.Round(doubleValue * sign * tens, roundCount);
            if (_values.Contains(newValue))
            {
                return GetNextRandom();
            }
            else
            {
                _values.Add(newValue);
                return newValue;
            }
        }
    }

    public static class ArrExtension
    {
        public static string ToArrString(this double[,] arr)
        {
            var result = string.Empty;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    result += arr[i, j] +"\t";
                }

                result += "\n";
            }

            return result;
        }
    }
}