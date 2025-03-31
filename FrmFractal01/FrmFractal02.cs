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
    public partial class FrmFractal02 : Form
    {
        public FrmFractal02()
        {
            InitializeComponent();
        }

        private void FrmFractal02_Load(object sender, EventArgs e)
        {
            JuliaSet();
        }

        private void JuliaSet()
        {
            int width = pictureBoxJulia.Width;
            int height = pictureBoxJulia.Height;
            Bitmap bmp = new Bitmap(width, height);

            // Parámetros para el conjunto de Julia
            double cReal = -0.7;  // Parte real del parámetro c
            double cImag = 0.27;  // Parte imaginaria del parámetro c

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    // Mapear coordenadas al plano complejo
                    double zReal = (col - width / 2.0) * 4.0 / width;
                    double zImag = (row - height / 2.0) * 4.0 / height;

                    int iteraciones = 0;

                    // Aplicar la fórmula del conjunto de Julia: z = z² + c
                    while (iteraciones < 500 && ((zReal * zReal) + (zImag * zImag)) < 4)
                    {
                        double zRealTemp = (zReal * zReal) - (zImag * zImag) + cReal;
                        zImag = 2 * zReal * zImag + cImag;
                        zReal = zRealTemp;
                        iteraciones++;
                    }

                    if (iteraciones < 500)
                    {
                        int r = (int)(Math.Sin(0.1 * iteraciones) * 127 + 128);
                        int g = (int)(Math.Sin(0.1 * iteraciones + 2) * 127 + 128);
                        int b = (int)(Math.Sin(0.1 * iteraciones + 4) * 127 + 128);
                        bmp.SetPixel(col, row, Color.FromArgb(r, g, b));
                    }
                    else
                    {
                        bmp.SetPixel(col, row, Color.Black);
                    }
                }
            }

            pictureBoxJulia.Image = bmp;
        }
    }
}
