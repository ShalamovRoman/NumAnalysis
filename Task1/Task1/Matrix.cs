using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Task1
{
    class Matrix
    {
        [Required]
        private double[,] _matrix;
        [Required]
        public int Dimension
        {
            get
            {
                return _matrix.GetLength(0);

            }
        } //Размерность матрицы
        public double Determinant
        {
            get
            {
                return GetDeterminant(_matrix);
            }
        } //Определитель матрицы
        public double Norm {
            get
            {
                return GetNorm(_matrix);
            }
        } //Норма один

        public double this[int i, int j]
        {
            get
            {
                return _matrix[i, j];
            }
            set
            {
                _matrix[i, j] = value;
            }
        }

        /// <summary>
        /// Converts array in Matrix
        /// </summary>

        public Matrix(double[,] arr)
        {
            _matrix = arr;
            
        }

        /// <summary>
        /// Creates square matrix
        /// </summary>
        public Matrix(int n)
        {
            _matrix = new double[n, n];
        }

        double GetDeterminant(double[,] _matrix)
        {
            if (_matrix.GetLength(0) == 2)
            {
                return _matrix[0, 0] * _matrix[1, 1] - _matrix[0, 1] * _matrix[1, 0];
            }
            else if (_matrix.GetLength(0) == 1)
            {
                return _matrix[0, 0];
            }
            double sign = 1, result = 0;
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                double[,] minor = GetMinor(_matrix, i);
                result += sign * _matrix[0, i] * GetDeterminant(minor);
                sign = -sign;
            }
            return result;
        }

        static double[,] GetMinor(double[,] _matrix, int n)
        {
            double[,] result = new double[_matrix.GetLength(0) - 1, _matrix.GetLength(0) - 1];

            for (int i = 1; i < _matrix.GetLength(0); i++)
            {
                for (int j = 0; j < n; j++)
                    result[i - 1, j] = _matrix[i, j];

                for (int j = n + 1; j < _matrix.GetLength(0); j++)
                    result[i - 1, j - 1] = _matrix[i, j];
            }
            return result;
        }

        double GetNorm(double[,] _matrix)
        {
            double res = 0,sum = 0;
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int j = 0; j < _matrix.GetLength(0); j++)
                    sum += Math.Abs(_matrix[i, j]);

                if (sum >= res) res = sum;
                sum = 0;
            }

            return res;
        }

        public static Matrix GetInvertibleMatrix(double[,] _matrix)
        {
            Matrix minorMatrix = new Matrix(_matrix.GetLength(0));
            double sign = 1;
            //Вычисление элементов
            for (int i = 0; i < _matrix.GetLength(0); i++)
            for (int j = 0; j < _matrix.GetLength(0); j++)
            {
                double[,] minor = GetMinor(_matrix, j);
                Matrix minorMatr = new Matrix(minor);
                PrintMatrix(minorMatr);
                minorMatrix[i, j] = minorMatr.Determinant*sign;
                sign = -sign;
            }

            minorMatrix = TransposeMatrix(minorMatrix);
            Matrix m = new Matrix(_matrix);
            for (int i = 0; i < minorMatrix.Dimension; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    minorMatrix[i, j] = minorMatrix[i, j] / m.Determinant;
                }
            }

            return minorMatrix;
        }

        public static Matrix TransposeMatrix(Matrix matrix)
        {
            double tmp = 0;
            for (int i = 0; i < matrix.Dimension; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    tmp = matrix[i, j];
                    matrix[i, j] = matrix[j, i];
                    matrix[j, i] = tmp;
                }
            }

            return matrix;
        }

        public static Matrix CreateHilbertMatrix(int n)
        {
            Matrix matrixA = new Matrix(n);
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    matrixA[i - 1, j - 1] = 1 / (double)(i + j - 1);
                }
            }
        return matrixA;
        }

        public static double[,] ToArray(Matrix _matrix)
        {
            double[,] arr = new double[_matrix.Dimension, _matrix.Dimension];
            for (int i = 0; i < _matrix.Dimension; i++)
            for (int j = 0; j < _matrix.Dimension; j++)
                arr[i, j] = _matrix[i, j];

            return arr;
        }

        public static void PrintMatrix(Matrix matrix)
        {
            for (int i = 0; i < matrix.Dimension; i++)
            {
                for (int j = 0; j < matrix.Dimension; j++)
                {
                    Console.Write("{0:f2}  ", matrix[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
