using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmsFractales
{
    public partial class FrmFractal03 : Form
    {
        public FrmFractal03()
        {
            InitializeComponent();
        }

        private void FrmFractal03_Load(object sender, EventArgs e)
        {
            NewtonFractal();
        }

        private void NewtonFractal()
        {
            int width = pictureBoxNewton.Width;
            int height = pictureBoxNewton.Height;
            Bitmap bmp = new Bitmap(width, height);

            // Definir las raíces del polinomio z³ - 1 = 0
            Complex[] roots = new Complex[3];
            roots[0] = new Complex(1, 0);                                // 1
            roots[1] = new Complex(-0.5, Math.Sqrt(3) / 2);              // -1/2 + (√3/2)i
            roots[2] = new Complex(-0.5, -Math.Sqrt(3) / 2);             // -1/2 - (√3/2)i

            
            Color[] rootColors = new Color[3];
            rootColors[0] = Color.FromArgb(255, 50, 50);                
            rootColors[1] = Color.FromArgb(50, 255, 50);                 
            rootColors[2] = Color.FromArgb(50, 50, 255);                 

            // Parámetros para el fractal
            double zoom = 1.5;
            int maxIter = 15;
            double tolerance = 0.000001;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    double real = (col - width / 2.0) / (width / 4.0) / zoom;
                    double imag = (row - height / 2.0) / (height / 4.0) / zoom;

                    Complex z = new Complex(real, imag);
                    int iterations = 0;
                    int rootIndex = -1;

                    // Iterar usando el método de Newton para z³ - 1 = 0
                    while (iterations < maxIter)
                    {
                        for (int i = 0; i < roots.Length; i++)
                        {
                            if (Complex.Abs(z - roots[i]) < tolerance)
                            {
                                rootIndex = i;
                                break;
                            }
                        }

                        if (rootIndex != -1)
                            break;

                        // Aplicar la fórmula del método de Newton: z = z - f(z)/f'(z)
                        // Para el polinomio z³ - 1, f(z) = z³ - 1 y f'(z) = 3z²
                        // Por lo tanto: z = z - (z³ - 1)/(3z²) = z - (z - 1/z²)/3 = (2z + 1/z²)/3
                        Complex z3 = Complex.Pow(z, 3);
                        Complex z2 = Complex.Pow(z, 2);

                        // Nueva z según la fórmula de Newton
                        z = z - (z3 - new Complex(1, 0)) / (3 * z2);

                        iterations++;
                    }

                    // Colorear según la raíz a la que converge
                    if (rootIndex >= 0)
                    {
                        Color baseColor = rootColors[rootIndex];

                        int intensity = 255 - (iterations * 10);
                        if (intensity < 30) intensity = 30;  

                        float factor = (float)intensity / 255;
                        int r = (int)(baseColor.R * factor);
                        int g = (int)(baseColor.G * factor);
                        int b = (int)(baseColor.B * factor);

                        bmp.SetPixel(col, row, Color.FromArgb(r, g, b));
                    }
                    else
                    {
                        bmp.SetPixel(col, row, Color.Black);
                    }
                }
            }

            pictureBoxNewton.Image = bmp;
        }

        private class Complex
        {
            public double Real { get; set; }
            public double Imaginary { get; set; }

            public Complex(double real, double imaginary)
            {
                Real = real;
                Imaginary = imaginary;
            }

            // Resta de dos números complejos
            public static Complex operator -(Complex a, Complex b)
            {
                return new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);
            }

            // División de dos números complejos
            public static Complex operator /(Complex a, Complex b)
            {
                double denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
                double real = (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator;
                double imaginary = (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator;
                return new Complex(real, imaginary);
            }

            // Multiplicación de dos números complejos
            public static Complex operator *(Complex a, Complex b)
            {
                double real = a.Real * b.Real - a.Imaginary * b.Imaginary;
                double imaginary = a.Real * b.Imaginary + a.Imaginary * b.Real;
                return new Complex(real, imaginary);
            }

            // Multiplicación de un número complejo por un escalar
            public static Complex operator *(double a, Complex b)
            {
                return new Complex(a * b.Real, a * b.Imaginary);
            }

            // Potencia de un número complejo
            public static Complex Pow(Complex a, int power)
            {
                if (power == 0)
                    return new Complex(1, 0);

                Complex result = new Complex(a.Real, a.Imaginary);
                for (int i = 1; i < power; i++)
                {
                    result = result * a;
                }
                return result;
            }

            // Valor absoluto (módulo) de un número complejo
            public static double Abs(Complex a)
            {
                return Math.Sqrt(a.Real * a.Real + a.Imaginary * a.Imaginary);
            }
        }
    }
}
