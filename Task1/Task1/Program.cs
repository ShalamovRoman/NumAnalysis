using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Task1
{
    class Program
    {
        //Создание вектора b из случайных чисел
        static double[] CreateRandomVector(int n)
        {
            double[] Vector = new double[n];
            Random Rnd = new Random();
            for (int i = 0; i < Vector.Length; i++)
            {
                Vector[i] = Rnd.NextDouble() + Rnd.Next(-5, 5);
            }
            return Vector;
        }

        //Метод крамера
        static double[] CramersRule(Matrix matrix, double[] vector)
        {
            double[] res = new double[vector.Length];

            Matrix tmpMatrix = new Matrix(vector.Length);
            for (int i = 0; i < vector.Length; i++)
            {
                for (int j = 0; j < tmpMatrix.Dimension; j++)
                for (int k = 0; k < tmpMatrix.Dimension; k++)
                    tmpMatrix[j, k] = matrix[j, k];
                tmpMatrix = ChangeVector(tmpMatrix, vector, i);
                res[i] = tmpMatrix.Determinant/matrix.Determinant;
                
                //Matrix.PrintMatrix(tmpMatrix)
            }
            return res;
        }

        //Метод замены одного столбца матрицы на заданный вектор
        static Matrix ChangeVector(Matrix matrix, double[] vector, int column)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                matrix[i, column] = vector[i];
            }
            return matrix;

        }

        static double VectorNorm(double[] vector)
        {
            double res = 0;
            for (int i = 0; i < vector.Length; i++)
                res += Math.Abs(vector[i]);
            return res;
        }

        static void Main(string[] args)
        {
            Console.Write("Введите размерность матрицы: ");
            int n = int.Parse(Console.ReadLine());

            Console.WriteLine("\nМатрица А: \n");
            var A = Matrix.CreateHilbertMatrix(n);
            Matrix.PrintMatrix(A);

            Console.WriteLine("Вектор b: \n");
            double[] b = CreateRandomVector(n);
            foreach (double j in b)
                Console.Write("{0:0.00}  ", j);

            Console.WriteLine("\n\nОпределитель матрицы A = {0}", A.Determinant);

            Console.WriteLine("\nРешения уравнения Ax = b:\n");
            double[] x = new double[b.Length];
            x = CramersRule(A, b);
            for (int i = 0; i < x.Length; i++)
                Console.WriteLine("x{0} = {1}", i, x[i]);

            Console.WriteLine("\nНовая матрица A: \n");
            Matrix newA = new Matrix(A.Dimension);
            Random rnd = new Random();
            for (int i = 0; i < A.Dimension; i++)
            for (int j = 0; j < A.Dimension; j++)
                    newA[i,j] = A[i,j] + rnd.NextDouble();
            Matrix.PrintMatrix(newA);

            Console.WriteLine("\nПриращение A: \n");
            Matrix deltaA = new Matrix(A.Dimension);
            for (int i = 0; i < A.Dimension; i++)
            for (int j = 0; j < A.Dimension; j++)
                deltaA[i, j] = Math.Abs(A[i, j] - newA[i,j]);
            Matrix.PrintMatrix(deltaA);

            Console.WriteLine("\nНовый вектор b: \n");
            double[] newb = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
                newb[i] = b[i] + rnd.NextDouble(); ;

            foreach (double j in newb)
                Console.Write("{0:0.00}  ", j);

            Console.WriteLine("\n\nПриращение b: \n");
            double[] deltab = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
                deltab[i] = Math.Abs(b[i] - newb[i]); ;

            foreach (double j in deltab)
                Console.Write("{0:0.00}  ", j);

            Console.WriteLine("\n\nРешение нового уравнения: \n");
            double[] x1 = new double[b.Length];
            x1 = CramersRule(newA,newb);

            for (int i = 0; i < x.Length; i++)
                Console.WriteLine("x*{0} = {1}", i, x1[i]);

            Console.WriteLine("\n\nДельта x: \n");
            double[] newx = new double[b.Length];
            for (int i = 0; i < x.Length; i++)
            {
                newx[i] = Math.Abs(x[i] - x1[i]);
                Console.WriteLine("x*{0} = {1:0.00}", i, newx[i]);
            }

            Console.WriteLine("\nОбратная матрица к А: \n");

            Matrix<double> a = Matrix<double>.Build.DenseOfArray(Matrix.ToArray(A)); 
            a = a.Inverse();
            //invertA = Matrix.GetInvertibleMatrix(Matrix.ToArray(A));
            var invertA = new Matrix(a.ToArray()); 
            Matrix.PrintMatrix(invertA);

            Console.Write("||A|| = {0:0.00}\n ", A.Norm);
            Console.Write("\n||A^-1|| = {0:0.00}\n ", invertA.Norm);
            Console.Write("\ncond(A) = {0}\n", A.Norm*invertA.Norm);

            Console.Write("\n||x|| = {0}\n ", VectorNorm(x));
            Console.Write("\n||x*|| = {0}\n ", VectorNorm(newx));
            Console.Write("\n||x|| / ||x*|| = {0}", VectorNorm(newx) / VectorNorm(x));

            double d = (A.Norm * invertA.Norm) / (1 - invertA.Norm * deltaA.Norm) *
                       (VectorNorm(deltab) / VectorNorm(b) + deltaA.Norm / A.Norm);
            Console.WriteLine("\n\nОценка = {0}", d);

            Console.WriteLine(invertA.Norm * deltaA.Norm);
            Console.ReadLine();
        
        }


    }
}
