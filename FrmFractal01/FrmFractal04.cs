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
    public partial class FrmFractal04 : Form
    {
        public FrmFractal04()
        {
            InitializeComponent();
        }

        private void FrmFractal04_Load(object sender, EventArgs e)
        {
            BurningShipFractal();
        }

        private void BurningShipFractal()
        {
            int width = pictureBoxBurningShip.Width;
            int height = pictureBoxBurningShip.Height;
            Bitmap bmp = new Bitmap(width, height);

            // Parámetros para visualización del fractal
            double xMin = -2.0;
            double xMax = 1.0;
            double yMin = -2.0;
            double yMax = 1.0;

            xMin = -1.8;
            xMax = -1.7;
            yMin = -0.1;
            yMax = 0.02;

            int maxIterations = 500;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    // Mapear las coordenadas de pixel a las coordenadas del plano complejo
                    double x0 = xMin + (xMax - xMin) * col / width;
                    double y0 = yMin + (yMax - yMin) * row / height;

                    double x = 0;
                    double y = 0;

                    int iteration = 0;

                    // Iteración principal del algoritmo Burning Ship
                    while (x * x + y * y < 4 && iteration < maxIterations)
                    {
                        // La diferencia clave con Mandelbrot: tomamos el valor absoluto de x e y
                        double xTemp = x * x - y * y + x0;
                        y = Math.Abs(2 * x * y) + y0;
                        x = xTemp;

                        iteration++;
                    }

                    if (iteration < maxIterations)
                    {
                        // Aplicar un esquema de color más detallado para mostrar la estructura
                        // Usamos una variación logarítmica para mejorar el contraste
                        double smooth = iteration + 1 - Math.Log(Math.Log(Math.Sqrt(x * x + y * y))) / Math.Log(2);

                        // Crear un esquema de color tipo "fuego"
                        int colorValue = (int)(smooth * 10) % 256;

                        int r = Math.Min(255, colorValue * 2);
                        int g = Math.Min(255, (int)(colorValue * 1.3));
                        int b = Math.Min(255, colorValue / 2);

                        if (iteration % 5 == 0)
                        {
                            r = b;
                            b = Math.Min(255, colorValue * 2);
                        }

                        //Bordes
                        if (iteration > maxIterations / 2)
                        {
                            int temp = r;
                            r = b;
                            b = temp;
                        }

                        bmp.SetPixel(col, row, Color.FromArgb(r, g, b));
                    }
                    else
                    {
                        // Para puntos dentro del conjunto
                        bmp.SetPixel(col, row, Color.Black);
                    }
                }
            }

            pictureBoxBurningShip.Image = bmp;
        }
    }
}
