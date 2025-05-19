using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsGraficos
{
    public partial class FrmGraficacion3D : Form
    {
        private List<Point3D> puntos = new List<Point3D>();
        private List<Line3D> lineas = new List<Line3D>();
        private Point3D puntoSeleccionado = null;
        private char siguienteLetra = 'A';
        private Color colorActual = Color.Red;

        private float anguloX = 0, anguloY = 0, anguloZ = 0;
        private Point ultimaPosicionMouse;
        private bool arrastrando = false;
        private float escala = 30;

        private const float D = 500;

        public FrmGraficacion3D()
        {
            InitializeComponent();
            typeof(PictureBox).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, pictureBox1, new object[] { true });
        }

        private void FrmGraficacion3D_Load(object sender, EventArgs e)
        {
            txtDibujarEnX.Text = "0";
            txtDibujarEnY.Text = "0";
            txtDibujarEnZ.Text = "0";

            colorActual = Color.Red;
            btnColorFigura.BackColor = colorActual;

            anguloX = -0.5f;
            anguloY = 0.5f;

            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseUp += PictureBox1_MouseUp;
            pictureBox1.Paint += PictureBox1_Paint;

            pictureBox1.Invalidate();
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            int centroX = pictureBox1.Width / 2;
            int centroY = pictureBox1.Height / 2;

            DibujarEjes(g, centroX, centroY);

            DibujarCuadricula(g, centroX, centroY);

            DibujarPuntos(g, centroX, centroY);

            DibujarLineas(g, centroX, centroY);
        }

        private void DibujarEjes(Graphics g, int centroX, int centroY)
        {
            Point3D origen = new Point3D(0, 0, 0);
            Point3D ejeX = new Point3D(15, 0, 0);
            Point3D ejeY = new Point3D(0, 15, 0);
            Point3D ejeZ = new Point3D(0, 0, 15);

            Point puntoOrigen = Proyectar(origen, centroX, centroY);
            Point puntoEjeX = Proyectar(ejeX, centroX, centroY);
            Point puntoEjeY = Proyectar(ejeY, centroX, centroY);
            Point puntoEjeZ = Proyectar(ejeZ, centroX, centroY);

            using (Pen penX = new Pen(Color.Red, 2))
            using (Pen penY = new Pen(Color.Green, 2))
            using (Pen penZ = new Pen(Color.Blue, 2))
            {
                g.DrawLine(penX, puntoOrigen, puntoEjeX);
                g.DrawLine(penY, puntoOrigen, puntoEjeY);
                g.DrawLine(penZ, puntoOrigen, puntoEjeZ);

                g.DrawString("X", new Font("Arial", 10, FontStyle.Bold), Brushes.Red, puntoEjeX.X + 10, puntoEjeX.Y);
                g.DrawString("Y", new Font("Arial", 10, FontStyle.Bold), Brushes.Green, puntoEjeY.X - 20, puntoEjeY.Y);
                g.DrawString("Z", new Font("Arial", 10, FontStyle.Bold), Brushes.Blue, puntoEjeZ.X + 10, puntoEjeZ.Y - 10);
            }
        }

        private void DibujarCuadricula(Graphics g, int centroX, int centroY)
        {
            int tamano = 15;
            using (Pen penGrid = new Pen(Color.LightGray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
            {
                for (int i = -tamano; i <= tamano; i++)
                {
                    if (i == 0) continue;

                    Point3D p1 = new Point3D(-tamano, i, 0);
                    Point3D p2 = new Point3D(tamano, i, 0);
                    Point proy1 = Proyectar(p1, centroX, centroY);
                    Point proy2 = Proyectar(p2, centroX, centroY);
                    g.DrawLine(penGrid, proy1, proy2);

                    Point3D p3 = new Point3D(-tamano, 0, i);
                    Point3D p4 = new Point3D(tamano, 0, i);
                    Point proy3 = Proyectar(p3, centroX, centroY);
                    Point proy4 = Proyectar(p4, centroX, centroY);
                    g.DrawLine(penGrid, proy3, proy4);
                }

                for (int i = -tamano; i <= tamano; i++)
                {
                    if (i == 0) continue;

                    Point3D p1 = new Point3D(i, -tamano, 0);
                    Point3D p2 = new Point3D(i, tamano, 0);
                    Point proy1 = Proyectar(p1, centroX, centroY);
                    Point proy2 = Proyectar(p2, centroX, centroY);
                    g.DrawLine(penGrid, proy1, proy2);

                    Point3D p3 = new Point3D(0, -tamano, i);
                    Point3D p4 = new Point3D(0, tamano, i);
                    Point proy3 = Proyectar(p3, centroX, centroY);
                    Point proy4 = Proyectar(p4, centroX, centroY);
                    g.DrawLine(penGrid, proy3, proy4);
                }

                for (int i = -tamano; i <= tamano; i++)
                {
                    if (i == 0) continue;

                    Point3D p1 = new Point3D(i, 0, -tamano);
                    Point3D p2 = new Point3D(i, 0, tamano);
                    Point proy1 = Proyectar(p1, centroX, centroY);
                    Point proy2 = Proyectar(p2, centroX, centroY);
                    g.DrawLine(penGrid, proy1, proy2);

                    Point3D p3 = new Point3D(0, i, -tamano);
                    Point3D p4 = new Point3D(0, i, tamano);
                    Point proy3 = Proyectar(p3, centroX, centroY);
                    Point proy4 = Proyectar(p4, centroX, centroY);
                    g.DrawLine(penGrid, proy3, proy4);
                }
            }

            using (Font font = new Font("Arial", 8))
            {
                for (int i = -tamano; i <= tamano; i += 2)
                {
                    if (i == 0) continue;

                    Point3D numX = new Point3D(i, 0, 0);
                    Point proyNumX = Proyectar(numX, centroX, centroY);
                    g.DrawString(i.ToString(), font, Brushes.Black, proyNumX.X, proyNumX.Y + 10);

                    Point3D numY = new Point3D(0, i, 0);
                    Point proyNumY = Proyectar(numY, centroX, centroY);
                    g.DrawString(i.ToString(), font, Brushes.Black, proyNumY.X - 15, proyNumY.Y);

                    Point3D numZ = new Point3D(0, 0, i);
                    Point proyNumZ = Proyectar(numZ, centroX, centroY);
                    g.DrawString(i.ToString(), font, Brushes.Black, proyNumZ.X + 10, proyNumZ.Y - 5);
                }
            }
        }

        private void DibujarPuntos(Graphics g, int centroX, int centroY)
        {
            using (Font font = new Font("Arial", 10))
            {
                for (int i = 0; i < puntos.Count; i++)
                {
                    Point3D punto = puntos[i];
                    Point proyeccion = Proyectar(punto, centroX, centroY);

                    using (SolidBrush brush = new SolidBrush(punto.Color))
                    {
                        g.FillEllipse(brush, proyeccion.X - 5, proyeccion.Y - 5, 10, 10);
                    }

                    string etiqueta = $"{punto.Etiqueta}({punto.X},{punto.Y},{punto.Z})";
                    SizeF tamanioTexto = g.MeasureString(etiqueta, font);

                    using (SolidBrush brushFondo = new SolidBrush(Color.FromArgb(180, Color.White)))
                    {
                        g.FillRectangle(brushFondo,
                            proyeccion.X + 5,
                            proyeccion.Y - 20,
                            tamanioTexto.Width,
                            tamanioTexto.Height);
                    }

                    g.DrawString(etiqueta, font, Brushes.Black, proyeccion.X + 5, proyeccion.Y - 20);
                }
            }
        }

        private void DibujarLineas(Graphics g, int centroX, int centroY)
        {
            foreach (var linea in lineas)
            {
                Point punto1 = Proyectar(linea.Punto1, centroX, centroY);
                Point punto2 = Proyectar(linea.Punto2, centroX, centroY);

                using (Pen pen = new Pen(linea.Color, 2))
                {
                    g.DrawLine(pen, punto1, punto2);
                }
            }
        }

        private Point Proyectar(Point3D punto3D, int centroX, int centroY)
        {
            double cosX = Math.Cos(anguloX);
            double sinX = Math.Sin(anguloX);
            double cosY = Math.Cos(anguloY);
            double sinY = Math.Sin(anguloY);
            double cosZ = Math.Cos(anguloZ);
            double sinZ = Math.Sin(anguloZ);

            double y1 = punto3D.Y * cosX - punto3D.Z * sinX;
            double z1 = punto3D.Y * sinX + punto3D.Z * cosX;

            double x2 = punto3D.X * cosY + z1 * sinY;
            double z2 = -punto3D.X * sinY + z1 * cosY;

            double x3 = x2 * cosZ - y1 * sinZ;
            double y3 = x2 * sinZ + y1 * cosZ;

            double factor = D / (D + z2);
            int proyX = (int)(x3 * factor * escala) + centroX;
            int proyY = (int)(y3 * factor * escala) + centroY;

            return new Point(proyX, proyY);
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ultimaPosicionMouse = e.Location;
            arrastrando = true;

            int centroX = pictureBox1.Width / 2;
            int centroY = pictureBox1.Height / 2;

            foreach (var punto in puntos)
            {
                Point proyeccion = Proyectar(punto, centroX, centroY);
                Rectangle areaClick = new Rectangle(proyeccion.X - 5, proyeccion.Y - 5, 10, 10);

                if (areaClick.Contains(e.Location))
                {
                    if (puntoSeleccionado == null)
                    {
                        puntoSeleccionado = punto;
                        punto.Seleccionado = true;
                    }
                    else if (puntoSeleccionado != punto)
                    {
                        lineas.Add(new Line3D(puntoSeleccionado, punto, colorActual));
                        puntoSeleccionado.Seleccionado = false;
                        puntoSeleccionado = null;
                        pictureBox1.Invalidate();
                    }
                    break;
                }
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (arrastrando)
            {
                float deltaX = (e.X - ultimaPosicionMouse.X) * 0.01f;
                float deltaY = (e.Y - ultimaPosicionMouse.Y) * 0.01f;

                anguloY += deltaX;
                anguloX += deltaY;

                ultimaPosicionMouse = e.Location;

                pictureBox1.Invalidate();
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            arrastrando = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void txtDibujarEnX_TextChanged(object sender, EventArgs e)
        {
        }
        private void txtDibujarEnY_TextChanged(object sender, EventArgs e)
        {
        }
        private void txtDibujarEnZ_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnDibujarPunto_Click(object sender, EventArgs e)
        {
            if (!float.TryParse(txtDibujarEnX.Text, out float x) ||
                !float.TryParse(txtDibujarEnY.Text, out float y) ||
                !float.TryParse(txtDibujarEnZ.Text, out float z))
            {
                MessageBox.Show("Por favor, ingrese valores numéricos válidos para X, Y y Z",
                                "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Point3D nuevoPunto = new Point3D(x, y, z)
            {
                Etiqueta = siguienteLetra.ToString(),
                Color = colorActual
            };

            puntos.Add(nuevoPunto);

            siguienteLetra = (char)(siguienteLetra + 1);
            if (siguienteLetra > 'Z') siguienteLetra = 'A';

            ActualizarListaCoordenadas();

            pictureBox1.Invalidate();
        }

        private void btnColorFigura_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = colorActual;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    colorActual = colorDialog.Color;

                    btnColorFigura.BackColor = colorActual;

                    if (colorActual.R + colorActual.G + colorActual.B < 384)
                    {
                        btnColorFigura.ForeColor = Color.White;
                    }
                    else
                    {
                        btnColorFigura.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void ActualizarListaCoordenadas()
        {
            listBoxCoordenadas.Items.Clear();
            foreach (var punto in puntos)
            {
                listBoxCoordenadas.Items.Add($"{punto.Etiqueta}({punto.X},{punto.Y},{punto.Z})");
            }
        }

        private class Point3D
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public string Etiqueta { get; set; }
            public Color Color { get; set; }
            public bool Seleccionado { get; set; }

            public Point3D(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
                Color = Color.Black;
                Seleccionado = false;
            }
        }

        private class Line3D
        {
            public Point3D Punto1 { get; set; }
            public Point3D Punto2 { get; set; }
            public Color Color { get; set; }

            public Line3D(Point3D punto1, Point3D punto2, Color color)
            {
                Punto1 = punto1;
                Punto2 = punto2;
                Color = color;
            }
        }

        private void listBoxHistorialFiguras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
                return;

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraSeleccionada = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraSeleccionada == null)
                return;

            puntos.Clear();
            lineas.Clear();
            puntoSeleccionado = null;

            siguienteLetra = 'A';

            foreach (var punto in figuraSeleccionada.Puntos)
            {
                Point3D nuevoPunto = new Point3D(punto.X, punto.Y, punto.Z)
                {
                    Etiqueta = punto.Etiqueta,
                    Color = punto.Color,
                    Seleccionado = punto.Seleccionado
                };
                puntos.Add(nuevoPunto);

                if (punto.Etiqueta.Length == 1 && punto.Etiqueta[0] >= siguienteLetra)
                {
                    siguienteLetra = (char)(punto.Etiqueta[0] + 1);
                    if (siguienteLetra > 'Z') siguienteLetra = 'A';
                }
            }

            foreach (var linea in figuraSeleccionada.Lineas)
            {
                Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                if (punto1 != null && punto2 != null)
                {
                    lineas.Add(new Line3D(punto1, punto2, linea.Color));
                }
            }

            ActualizarListaCoordenadas(nombreFiguraSeleccionada);

            pictureBox1.Invalidate();
        }

        private void ActualizarListaCoordenadas(string nombreFigura = "")
        {
            listBoxCoordenadas.Items.Clear();

            if (!string.IsNullOrEmpty(nombreFigura))
            {
                listBoxCoordenadas.Items.Add($"--- {nombreFigura} ---");
            }

            foreach (var punto in puntos)
            {
                listBoxCoordenadas.Items.Add($"{punto.Etiqueta}({punto.X},{punto.Y},{punto.Z})");
            }
        }

        private class Figura3D
        {
            public string Nombre { get; set; }
            public List<Point3D> Puntos { get; set; }
            public List<Line3D> Lineas { get; set; }

            public Figura3D(string nombre, List<Point3D> puntos, List<Line3D> lineas)
            {
                Nombre = nombre;
                Puntos = puntos.Select(p => new Point3D(p.X, p.Y, p.Z)
                {
                    Etiqueta = p.Etiqueta,
                    Color = p.Color,
                    Seleccionado = p.Seleccionado
                }).ToList();

                Lineas = new List<Line3D>();
                foreach (var linea in lineas)
                {
                    Point3D punto1 = Puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                    Point3D punto2 = Puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                    if (punto1 != null && punto2 != null)
                    {
                        Lineas.Add(new Line3D(punto1, punto2, linea.Color));
                    }
                }
            }
        }

        private List<Figura3D> figuras = new List<Figura3D>();
        private int contadorFiguras = 1;

        private void btnGuardarFiguras_Click(object sender, EventArgs e)
        {
            if (puntos.Count == 0)
            {
                MessageBox.Show("No hay puntos para guardar como figura.",
                               "Figura vacía", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFigura = $"Figura {contadorFiguras}";

            Figura3D nuevaFigura = new Figura3D(nombreFigura, puntos, lineas);
            figuras.Add(nuevaFigura);

            contadorFiguras++;

            listBoxHistorialFiguras.Items.Add(nombreFigura);

            MessageBox.Show($"La figura '{nombreFigura}' ha sido guardada correctamente.",
                           "Figura guardada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLimpiarPlano_Click(object sender, EventArgs e)
        {
            puntos.Clear();
            lineas.Clear();
            puntoSeleccionado = null;

            listBoxCoordenadas.Items.Clear();

            siguienteLetra = 'A';

            pictureBox1.Invalidate();

            MessageBox.Show("El plano ha sido limpiado. El historial de figuras se mantiene.",
                           "Plano limpiado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBorrarTodo_Click(object sender, EventArgs e)
        {
            puntos.Clear();
            lineas.Clear();
            puntoSeleccionado = null;

            figuras.Clear();
            listBoxHistorialFiguras.Items.Clear();

            listBoxCoordenadas.Items.Clear();

            siguienteLetra = 'A';
            contadorFiguras = 1;

            pictureBox1.Invalidate();

            MessageBox.Show("El plano y el historial de figuras han sido borrados completamente.",
                           "Todo borrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listBoxCoordenadas_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnTraslacion_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar la traslación.",
                               "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoTraslacion = new Form())
            {
                dialogoTraslacion.Text = "Traslación de Figura";
                dialogoTraslacion.Size = new Size(400, 300);
                dialogoTraslacion.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoTraslacion.StartPosition = FormStartPosition.CenterParent;
                dialogoTraslacion.MaximizeBox = false;
                dialogoTraslacion.MinimizeBox = false;

                Label lblTrasX = new Label { Text = "Traslación en X:", Location = new Point(20, 20), Width = 120 };
                Label lblTrasY = new Label { Text = "Traslación en Y:", Location = new Point(20, 60), Width = 120 };
                Label lblTrasZ = new Label { Text = "Traslación en Z:", Location = new Point(20, 100), Width = 120 };
                Label lblColorNuevaFigura = new Label { Text = "Color:", Location = new Point(20, 140), Width = 120 };

                NumericUpDown numTrasX = new NumericUpDown { Location = new Point(150, 20), Width = 120, Minimum = -100, Maximum = 100, DecimalPlaces = 1, Value = 0 };
                NumericUpDown numTrasY = new NumericUpDown { Location = new Point(150, 60), Width = 120, Minimum = -100, Maximum = 100, DecimalPlaces = 1, Value = 0 };
                NumericUpDown numTrasZ = new NumericUpDown { Location = new Point(150, 100), Width = 120, Minimum = -100, Maximum = 100, DecimalPlaces = 1, Value = 0 };

                Button btnColor = new Button { Text = "Seleccionar Color", Location = new Point(130, 140), Width = 200, BackColor = colorActual };
                Button btnAceptar = new Button { Text = "Aceptar", DialogResult = DialogResult.OK, Location = new Point(50, 180) };
                Button btnCancelar = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(150, 180) };

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) =>
                {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                dialogoTraslacion.Controls.Add(lblTrasX);
                dialogoTraslacion.Controls.Add(lblTrasY);
                dialogoTraslacion.Controls.Add(lblTrasZ);
                dialogoTraslacion.Controls.Add(lblColorNuevaFigura);
                dialogoTraslacion.Controls.Add(numTrasX);
                dialogoTraslacion.Controls.Add(numTrasY);
                dialogoTraslacion.Controls.Add(numTrasZ);
                dialogoTraslacion.Controls.Add(btnColor);
                dialogoTraslacion.Controls.Add(btnAceptar);
                dialogoTraslacion.Controls.Add(btnCancelar);

                if (dialogoTraslacion.ShowDialog() == DialogResult.OK)
                {
                    float trasX = (float)numTrasX.Value;
                    float trasY = (float)numTrasY.Value;
                    float trasZ = (float)numTrasZ.Value;

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    float[,] matrizTraslacion = new float[4, 4] {
                        { 1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 0, 1, 0 },
                        { trasX, trasY, trasZ, 1 }
                    };

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                        { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                    };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizTraslacion[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                                resultado[0],
                                resultado[1],
                                resultado[2]
                            )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_Traslación";

                    if (nombreBase.Contains("_Traslación"))
                    {
                        sufijo = "_Traslación";
                    }

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Traslación aplicada. Se ha creado la figura '{nuevoNombre}'.",
                                   "Traslación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnEscalacionRO_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar la escalación.",
                               "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoEscalacion = new Form())
            {
                dialogoEscalacion.Text = "Escalación Respecto al Origen";
                dialogoEscalacion.Size = new Size(500, 250);
                dialogoEscalacion.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoEscalacion.StartPosition = FormStartPosition.CenterParent;
                dialogoEscalacion.MaximizeBox = false;
                dialogoEscalacion.MinimizeBox = false;

                Label lblFactorEscala = new Label { Text = "Factor de escalación:", Location = new Point(20, 20), Width = 120 };
                Label lblDescripcion = new Label
                {
                    Text = "- Valores > 1 aumentan el tamaño y se reduce el tamaño cuando es negativo -1 \n- Valores entre 0 y 1 reducen el tamaño",
                    Location = new Point(20, 50),
                    Width = 400,
                    Height = 80
                };
                Label lblColorNuevaFigura = new Label { Text = "Color:", Location = new Point(20, 130), Width = 100 };

                NumericUpDown numFactorEscala = new NumericUpDown
                {
                    Location = new Point(150, 20),
                    Width = 120,
                    Minimum = 0.1m,
                    Maximum = 10m,
                    DecimalPlaces = 2,
                    Value = 1.5m,
                    Increment = 0.1m
                };

                Button btnColor = new Button { Text = "Seleccionar Color", Location = new Point(150, 130), Width = 120, BackColor = colorActual };
                Button btnAceptar = new Button { Text = "Aceptar", DialogResult = DialogResult.OK, Location = new Point(70, 160) };
                Button btnCancelar = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(190, 160) };

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) =>
                {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                dialogoEscalacion.Controls.Add(lblFactorEscala);
                dialogoEscalacion.Controls.Add(lblDescripcion);
                dialogoEscalacion.Controls.Add(lblColorNuevaFigura);
                dialogoEscalacion.Controls.Add(numFactorEscala);
                dialogoEscalacion.Controls.Add(btnColor);
                dialogoEscalacion.Controls.Add(btnAceptar);
                dialogoEscalacion.Controls.Add(btnCancelar);

                if (dialogoEscalacion.ShowDialog() == DialogResult.OK)
                {
                    float factorEscala = (float)numFactorEscala.Value;

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    float[,] matrizEscalacion = new float[4, 4] {
                        { factorEscala, 0, 0, 0 },
                        { 0, factorEscala, 0, 0 },
                        { 0, 0, factorEscala, 0 },
                        { 0, 0, 0, 1 }
                    };

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                            { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                        };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizEscalacion[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                                resultado[0],
                                resultado[1],
                                resultado[2]
                            )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_EscalaciónRO";

                    if (!nombreBase.Contains("_EscalaciónRO"))
                    {
                        sufijo = "_EscalaciónRO";
                    }

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Escalación aplicada. Se ha creado la figura '{nuevoNombre}'.",
                                   "Escalación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnEscalacionRPF_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar la escalación.",
                               "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoEscalacionRPF = new Form())
            {
                dialogoEscalacionRPF.Text = "Escalación Respecto a un Punto Fijo";
                dialogoEscalacionRPF.Size = new Size(500, 400);
                dialogoEscalacionRPF.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoEscalacionRPF.StartPosition = FormStartPosition.CenterParent;
                dialogoEscalacionRPF.MaximizeBox = false;
                dialogoEscalacionRPF.MinimizeBox = false;

                Label lblFactorEscala = new Label
                {
                    Text = "Factor de escalación:",
                    Location = new Point(20, 20),
                    Width = 120
                };

                NumericUpDown numFactorEscala = new NumericUpDown
                {
                    Location = new Point(150, 20),
                    Width = 120,
                    Minimum = 0.1m,
                    Maximum = 10m,
                    DecimalPlaces = 2,
                    Value = 1.5m,
                    Increment = 0.1m
                };

                Label lblDescripcion = new Label
                {
                    Text = "- Valores > 1 aumentan el tamaño o números negativos reducen el tamaño\n- Valores entre 0 y 1 reducen el tamaño",
                    Location = new Point(20, 50),
                    Width = 400,
                    Height = 70
                };

                Label lblPuntoRef = new Label
                {
                    Text = "Punto de referencia:",
                    Location = new Point(20, 100),
                    Width = 160
                };

                RadioButton rbPuntoExistente = new RadioButton
                {
                    Text = "Seleccionar punto existente:",
                    Location = new Point(20, 130),
                    Width = 180,
                    Checked = true
                };

                ComboBox cmbPuntosExistentes = new ComboBox
                {
                    Location = new Point(210, 130),
                    Width = 200,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                foreach (var punto in figuraOriginal.Puntos)
                {
                    cmbPuntosExistentes.Items.Add($"{punto.Etiqueta}({punto.X},{punto.Y},{punto.Z})");
                }
                if (cmbPuntosExistentes.Items.Count > 0)
                    cmbPuntosExistentes.SelectedIndex = 0;

                RadioButton rbPuntoPersonalizado = new RadioButton
                {
                    Text = "Coordenadas personalizadas:",
                    Location = new Point(20, 170),
                    Width = 180
                };

                Label lblX = new Label { Text = "X:", Location = new Point(210, 170), Width = 20 };
                Label lblY = new Label { Text = "Y:", Location = new Point(290, 170), Width = 20 };
                Label lblZ = new Label { Text = "Z:", Location = new Point(370, 170), Width = 20 };

                NumericUpDown numX = new NumericUpDown
                {
                    Location = new Point(230, 170),
                    Width = 50,
                    Minimum = -100,
                    Maximum = 100,
                    DecimalPlaces = 1,
                    Value = 0
                };

                NumericUpDown numY = new NumericUpDown
                {
                    Location = new Point(310, 170),
                    Width = 50,
                    Minimum = -100,
                    Maximum = 100,
                    DecimalPlaces = 1,
                    Value = 0
                };

                NumericUpDown numZ = new NumericUpDown
                {
                    Location = new Point(390, 170),
                    Width = 50,
                    Minimum = -100,
                    Maximum = 100,
                    DecimalPlaces = 1,
                    Value = 0
                };

                rbPuntoExistente.CheckedChanged += (s, ev) =>
                {
                    cmbPuntosExistentes.Enabled = rbPuntoExistente.Checked;
                    numX.Enabled = !rbPuntoExistente.Checked;
                    numY.Enabled = !rbPuntoExistente.Checked;
                    numZ.Enabled = !rbPuntoExistente.Checked;
                };

                rbPuntoPersonalizado.CheckedChanged += (s, ev) =>
                {
                    cmbPuntosExistentes.Enabled = !rbPuntoPersonalizado.Checked;
                    numX.Enabled = rbPuntoPersonalizado.Checked;
                    numY.Enabled = rbPuntoPersonalizado.Checked;
                    numZ.Enabled = rbPuntoPersonalizado.Checked;
                };

                Label lblColorNuevaFigura = new Label
                {
                    Text = "Color:",
                    Location = new Point(20, 220),
                    Width = 100
                };

                Button btnColor = new Button
                {
                    Text = "Seleccionar Color",
                    Location = new Point(150, 220),
                    Width = 180,
                    BackColor = colorActual
                };

                numX.Enabled = false;
                numY.Enabled = false;
                numZ.Enabled = false;

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) =>
                {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                Button btnAceptar = new Button
                {
                    Text = "Aceptar",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 280),
                    Width = 100
                };

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(250, 280),
                    Width = 100
                };

                dialogoEscalacionRPF.Controls.AddRange(new Control[] {
                    lblFactorEscala, numFactorEscala, lblDescripcion,
                    lblPuntoRef, rbPuntoExistente, cmbPuntosExistentes,
                    rbPuntoPersonalizado, lblX, lblY, lblZ, numX, numY, numZ,
                    lblColorNuevaFigura, btnColor,
                    btnAceptar, btnCancelar
                });

                if (dialogoEscalacionRPF.ShowDialog() == DialogResult.OK)
                {
                    float factorEscala = (float)numFactorEscala.Value;

                    float x0, y0, z0;

                    if (rbPuntoExistente.Checked && cmbPuntosExistentes.SelectedIndex >= 0)
                    {
                        Point3D puntoRef = figuraOriginal.Puntos[cmbPuntosExistentes.SelectedIndex];
                        x0 = puntoRef.X;
                        y0 = puntoRef.Y;
                        z0 = puntoRef.Z;
                    }
                    else
                    {
                        x0 = (float)numX.Value;
                        y0 = (float)numY.Value;
                        z0 = (float)numZ.Value;
                    }

                    float[,] matrizEscalacion = new float[4, 4] {
                        { factorEscala, 0, 0, 0 },
                        { 0, factorEscala, 0, 0 },
                        { 0, 0, factorEscala, 0 },
                        { x0 * (1 - factorEscala), y0 * (1 - factorEscala), z0 * (1 - factorEscala), 1 }
                    };

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                        { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                    };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizEscalacion[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                            resultado[0],
                            resultado[1],
                            resultado[2]
                        )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_EscalaciónRPF";

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Escalación respecto a punto fijo aplicada. Se ha creado la figura '{nuevoNombre}'.",
                                   "Escalación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnRotacionREjeX_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar la rotación.",
                               "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoRotacion = new Form())
            {
                dialogoRotacion.Text = "Rotación Respecto al Eje X";
                dialogoRotacion.Size = new Size(450, 300);
                dialogoRotacion.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoRotacion.StartPosition = FormStartPosition.CenterParent;
                dialogoRotacion.MaximizeBox = false;
                dialogoRotacion.MinimizeBox = false;

                Label lblAngulo = new Label
                {
                    Text = "Ángulo de rotación (en grados):",
                    Location = new Point(20, 20),
                    Width = 250
                };

                NumericUpDown numAngulo = new NumericUpDown
                {
                    Location = new Point(270, 20),
                    Width = 120,
                    Minimum = -360,
                    Maximum = 360,
                    DecimalPlaces = 1,
                    Value = 0,
                    Increment = 5
                };

                Label lblDescripcion = new Label
                {
                    Text = "- Valores positivos: rotación en sentido antihorario o sentido a la mano derecha\n- Valores negativos: rotación en sentido horario o sentido a la mano izquierda",
                    Location = new Point(20, 50),
                    Width = 400,
                    Height = 80
                };

                Label lblColorNuevaFigura = new Label
                {
                    Text = "Color:",
                    Location = new Point(20, 150),
                    Width = 100
                };

                Button btnColor = new Button
                {
                    Text = "Seleccionar Color",
                    Location = new Point(200, 150),
                    Width = 180,
                    BackColor = colorActual
                };

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) =>
                {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                Button btnAceptar = new Button
                {
                    Text = "Aceptar",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 180),
                    Width = 100
                };

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(250, 180),
                    Width = 100
                };

                dialogoRotacion.Controls.AddRange(new Control[] {
                    lblAngulo, numAngulo, lblDescripcion,
                    lblColorNuevaFigura, btnColor,
                    btnAceptar, btnCancelar
                });

                if (dialogoRotacion.ShowDialog() == DialogResult.OK)
                {
                    float angulo = (float)numAngulo.Value;

                    float anguloRadianes = angulo * (float)Math.PI / 180.0f;

                    float coseno = (float)Math.Cos(anguloRadianes);
                    float seno = (float)Math.Sin(anguloRadianes);

                    float[,] matrizRotacionX = new float[4, 4] {
                        { 1, 0, 0, 0 },
                        { 0, coseno, seno, 0 },
                        { 0, -seno, coseno, 0 },
                        { 0, 0, 0, 1 }
                    };

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                    { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizRotacionX[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                            resultado[0],
                            resultado[1],
                            resultado[2]
                        )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_RotacionREjeX";

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Rotación respecto al eje X aplicada. Se ha creado la figura '{nuevoNombre}'.",
                                   "Rotación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnRotacionREjeY_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar la rotación.",
                               "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoRotacion = new Form())
            {
                dialogoRotacion.Text = "Rotación Respecto al Eje Y";
                dialogoRotacion.Size = new Size(450, 300);
                dialogoRotacion.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoRotacion.StartPosition = FormStartPosition.CenterParent;
                dialogoRotacion.MaximizeBox = false;
                dialogoRotacion.MinimizeBox = false;

                Label lblAngulo = new Label
                {
                    Text = "Ángulo de rotación (en grados):",
                    Location = new Point(20, 20),
                    Width = 250
                };

                NumericUpDown numAngulo = new NumericUpDown
                {
                    Location = new Point(270, 20),
                    Width = 120,
                    Minimum = -360,
                    Maximum = 360,
                    DecimalPlaces = 1,
                    Value = 0,
                    Increment = 5
                };

                Label lblDescripcion = new Label
                {
                    Text = "- Valores positivos: rotación en sentido antihorario o sentido a la mano derecha\n- Valores negativos: rotación en sentido horario o sentido a la mano izquierda",
                    Location = new Point(20, 50),
                    Width = 400,
                    Height = 80
                };

                Label lblColorNuevaFigura = new Label
                {
                    Text = "Color:",
                    Location = new Point(20, 150),
                    Width = 100
                };

                Button btnColor = new Button
                {
                    Text = "Seleccionar Color",
                    Location = new Point(200, 150),
                    Width = 180,
                    BackColor = colorActual
                };

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) =>
                {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                Button btnAceptar = new Button
                {
                    Text = "Aceptar",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 180),
                    Width = 100
                };

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(250, 180),
                    Width = 100
                };

                dialogoRotacion.Controls.AddRange(new Control[] {
                    lblAngulo, numAngulo, lblDescripcion,
                    lblColorNuevaFigura, btnColor,
                    btnAceptar, btnCancelar
                });

                if (dialogoRotacion.ShowDialog() == DialogResult.OK)
                {
                    float angulo = (float)numAngulo.Value;

                    float anguloRadianes = angulo * (float)Math.PI / 180.0f;

                    float coseno = (float)Math.Cos(anguloRadianes);
                    float seno = (float)Math.Sin(anguloRadianes);

                    float[,] matrizRotacionY = new float[4, 4] {
                    { coseno, 0, -seno, 0 },
                    { 0, 1, 0, 0 },
                    { seno, 0, coseno, 0 },
                    { 0, 0, 0, 1 }
                };

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                        { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                    };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizRotacionY[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                            resultado[0],
                            resultado[1],
                            resultado[2]
                        )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_RotacionREjeY";

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Rotación respecto al eje Y aplicada. Se ha creado la figura '{nuevoNombre}'.",
                                   "Rotación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnRotacionREjeZ_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar la rotación.",
                               "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoRotacion = new Form())
            {
                dialogoRotacion.Text = "Rotación Respecto al Eje Z";
                dialogoRotacion.Size = new Size(450, 300);
                dialogoRotacion.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoRotacion.StartPosition = FormStartPosition.CenterParent;
                dialogoRotacion.MaximizeBox = false;
                dialogoRotacion.MinimizeBox = false;

                Label lblAngulo = new Label
                {
                    Text = "Ángulo de rotación (en grados):",
                    Location = new Point(20, 20),
                    Width = 250
                };

                NumericUpDown numAngulo = new NumericUpDown
                {
                    Location = new Point(270, 20),
                    Width = 120,
                    Minimum = -360,
                    Maximum = 360,
                    DecimalPlaces = 1,
                    Value = 0,
                    Increment = 5
                };

                Label lblDescripcion = new Label
                {
                    Text = "- Valores positivos: rotación en sentido antihorario o sentido a la mano derecha\n- Valores negativos: rotación en sentido horario o sentido a la mano izquierda",
                    Location = new Point(20, 50),
                    Width = 400,
                    Height = 80
                };

                Label lblColorNuevaFigura = new Label
                {
                    Text = "Color:",
                    Location = new Point(20, 150),
                    Width = 100
                };

                Button btnColor = new Button
                {
                    Text = "Seleccionar Color",
                    Location = new Point(200, 150),
                    Width = 180,
                    BackColor = colorActual
                };

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) =>
                {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                Button btnAceptar = new Button
                {
                    Text = "Aceptar",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 200),
                    Width = 100
                };

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(250, 200),
                    Width = 100
                };

                dialogoRotacion.Controls.AddRange(new Control[] {
                    lblAngulo, numAngulo, lblDescripcion,
                    lblColorNuevaFigura, btnColor,
                    btnAceptar, btnCancelar
                });

                if (dialogoRotacion.ShowDialog() == DialogResult.OK)
                {
                    float angulo = (float)numAngulo.Value;

                    float anguloRadianes = angulo * (float)Math.PI / 180.0f;

                    float coseno = (float)Math.Cos(anguloRadianes);
                    float seno = (float)Math.Sin(anguloRadianes);

                    float[,] matrizRotacionZ = new float[4, 4] {
                        { coseno, seno, 0, 0 },
                        { -seno, coseno, 0, 0 },
                        { 0, 0, 1, 0 },
                        { 0, 0, 0, 1 }
                    };

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                            { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                        };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizRotacionZ[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                            resultado[0],
                            resultado[1],
                            resultado[2]
                        )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_RotacionREjeZ";

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Rotación respecto al eje Z aplicada. Se ha creado la figura '{nuevoNombre}'.",
                                   "Rotación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnReflexionREjeX_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar la reflexión.",
                               "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoReflexion = new Form())
            {
                dialogoReflexion.Text = "Reflexión Respecto al Eje X";
                dialogoReflexion.Size = new Size(450, 250);
                dialogoReflexion.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoReflexion.StartPosition = FormStartPosition.CenterParent;
                dialogoReflexion.MaximizeBox = false;
                dialogoReflexion.MinimizeBox = false;

                Label lblColorNuevaFigura = new Label
                {
                    Text = "Color:",
                    Location = new Point(20, 30),
                    Width = 100
                };

                Button btnColor = new Button
                {
                    Text = "Seleccionar Color",
                    Location = new Point(130, 30),
                    Width = 200,
                    BackColor = colorActual
                };

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) => {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                Label lblDescripcion = new Label
                {
                    Text = "Esta operación reflejará la figura con respecto al eje X,\ninvirtiendo las coordenadas Y y Z.",
                    Location = new Point(20, 70),
                    Width = 400,
                    Height = 60
                };

                Button btnAceptar = new Button
                {
                    Text = "Aceptar",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 120),
                    Width = 80
                };

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(220, 120),
                    Width = 80
                };

                dialogoReflexion.Controls.AddRange(new Control[] {
                    lblColorNuevaFigura, btnColor, lblDescripcion,
                    btnAceptar, btnCancelar
                });

                if (dialogoReflexion.ShowDialog() == DialogResult.OK)
                {

                    float[,] matrizReflexionX = new float[4, 4] {
                        { 1, 0, 0, 0 },
                        { 0, -1, 0, 0 },
                        { 0, 0, -1, 0 },
                        { 0, 0, 0, 1 }
                    };

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                        { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                    };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizReflexionX[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                            resultado[0],
                            resultado[1],
                            resultado[2]
                        )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_ReflexionREjeX";

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Reflexión respecto al eje X aplicada. Se ha creado la figura '{nuevoNombre}'.",
                                   "Reflexión completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnReflexionREjeY_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar la reflexión.",
                            "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoReflexion = new Form())
            {
                dialogoReflexion.Text = "Reflexión Respecto al Eje Y";
                dialogoReflexion.Size = new Size(450, 250);
                dialogoReflexion.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoReflexion.StartPosition = FormStartPosition.CenterParent;
                dialogoReflexion.MaximizeBox = false;
                dialogoReflexion.MinimizeBox = false;

                Label lblColorNuevaFigura = new Label
                {
                    Text = "Color:",
                    Location = new Point(20, 30),
                    Width = 100
                };

                Button btnColor = new Button
                {
                    Text = "Seleccionar Color",
                    Location = new Point(130, 30),
                    Width = 200,
                    BackColor = colorActual
                };

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) => {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                Label lblDescripcion = new Label
                {
                    Text = "Esta operación reflejará la figura con respecto al eje Y,\ninvirtiendo las coordenadas X y Z.",
                    Location = new Point(20, 70),
                    Width = 400,
                    Height = 60
                };

                Button btnAceptar = new Button
                {
                    Text = "Aceptar",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 120),
                    Width = 80
                };

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(220, 120),
                    Width = 80
                };

                dialogoReflexion.Controls.AddRange(new Control[] {
                    lblColorNuevaFigura, btnColor, lblDescripcion,
                    btnAceptar, btnCancelar
                });

                if (dialogoReflexion.ShowDialog() == DialogResult.OK)
                {
                    
                    float[,] matrizReflexionY = new float[4, 4] {
                        { -1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { 0, 0, -1, 0 },
                        { 0, 0, 0, 1 }
                    };

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                    { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizReflexionY[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                            resultado[0],
                            resultado[1],
                            resultado[2]
                        )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_ReflexionREjeY";

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Reflexión respecto al eje Y aplicada. Se ha creado la figura '{nuevoNombre}'.",
                                "Reflexión completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnReflexionREjeZ_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar la reflexión.",
                            "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoReflexion = new Form())
            {
                dialogoReflexion.Text = "Reflexión Respecto al Eje Z";
                dialogoReflexion.Size = new Size(450, 250);
                dialogoReflexion.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoReflexion.StartPosition = FormStartPosition.CenterParent;
                dialogoReflexion.MaximizeBox = false;
                dialogoReflexion.MinimizeBox = false;

                Label lblColorNuevaFigura = new Label
                {
                    Text = "Color:",
                    Location = new Point(20, 30),
                    Width = 100
                };

                Button btnColor = new Button
                {
                    Text = "Seleccionar Color",
                    Location = new Point(130, 30),
                    Width = 200,
                    BackColor = colorActual
                };

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) => {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                Label lblDescripcion = new Label
                {
                    Text = "Esta operación reflejará la figura con respecto al eje Z,\ninvirtiendo las coordenadas X e Y.",
                    Location = new Point(20, 70),
                    Width = 400,
                    Height = 60
                };

                Button btnAceptar = new Button
                {
                    Text = "Aceptar",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 120),
                    Width = 80
                };

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(220, 120),
                    Width = 80
                };

                dialogoReflexion.Controls.AddRange(new Control[] {
                    lblColorNuevaFigura, btnColor, lblDescripcion,
                    btnAceptar, btnCancelar
                });

                if (dialogoReflexion.ShowDialog() == DialogResult.OK)
                {
                    float[,] matrizReflexionZ = new float[4, 4] {
                        { -1, 0, 0, 0 },
                        { 0, -1, 0, 0 },
                        { 0, 0, 1, 0 },
                        { 0, 0, 0, 1 }
                    };

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                    { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizReflexionZ[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                            resultado[0],
                            resultado[1],
                            resultado[2]
                        )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_ReflexionREjeZ";

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Reflexión respecto al eje Z aplicada. Se ha creado la figura '{nuevoNombre}'.",
                                "Reflexión completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnAfilamiento_Click(object sender, EventArgs e)
        {
            if (listBoxHistorialFiguras.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una figura del historial para aplicar el afilamiento.",
                               "Ninguna figura seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreFiguraSeleccionada = listBoxHistorialFiguras.SelectedItem.ToString();
            Figura3D figuraOriginal = figuras.FirstOrDefault(f => f.Nombre == nombreFiguraSeleccionada);

            if (figuraOriginal == null)
            {
                MessageBox.Show("No se pudo encontrar la figura seleccionada.",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialogoAfilamiento = new Form())
            {
                dialogoAfilamiento.Text = "Afilamiento de Figura";
                dialogoAfilamiento.Size = new Size(450, 300);
                dialogoAfilamiento.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogoAfilamiento.StartPosition = FormStartPosition.CenterParent;
                dialogoAfilamiento.MaximizeBox = false;
                dialogoAfilamiento.MinimizeBox = false;

                Label lblParamA = new Label
                {
                    Text = "Parámetro a (factor de afilamiento en X):",
                    Location = new Point(20, 20),
                    Width = 250
                };

                NumericUpDown numParamA = new NumericUpDown
                {
                    Location = new Point(280, 20),
                    Width = 120,
                    Minimum = -10,
                    Maximum = 10,
                    DecimalPlaces = 2,
                    Increment = 0.1m,
                    Value = 0
                };

                Label lblParamB = new Label
                {
                    Text = "Parámetro b (factor de afilamiento en Y):",
                    Location = new Point(20, 60),
                    Width = 250
                };

                NumericUpDown numParamB = new NumericUpDown
                {
                    Location = new Point(280, 60),
                    Width = 120,
                    Minimum = -10,
                    Maximum = 10,
                    DecimalPlaces = 2,
                    Increment = 0.1m,
                    Value = 0
                };

                Label lblDescripcion = new Label
                {
                    Text = "El afilamiento modifica la forma de la figura inclinándola respecto al eje Z.\n",
                    Location = new Point(20, 100),
                    Width = 400,
                    Height = 60
                };

                Label lblColorNuevaFigura = new Label
                {
                    Text = "Color:",
                    Location = new Point(20, 170),
                    Width = 100
                };

                Button btnColor = new Button
                {
                    Text = "Seleccionar Color",
                    Location = new Point(280, 170),
                    Width = 120,
                    BackColor = colorActual
                };

                Color colorNuevaFigura = colorActual;
                btnColor.Click += (s, ev) => {
                    using (ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.Color = colorNuevaFigura;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorNuevaFigura = colorDialog.Color;
                            btnColor.BackColor = colorNuevaFigura;

                            if (colorNuevaFigura.R + colorNuevaFigura.G + colorNuevaFigura.B < 384)
                            {
                                btnColor.ForeColor = Color.White;
                            }
                            else
                            {
                                btnColor.ForeColor = Color.Black;
                            }
                        }
                    }
                };

                Button btnAceptar = new Button
                {
                    Text = "Aceptar",
                    DialogResult = DialogResult.OK,
                    Location = new Point(100, 220),
                    Width = 100
                };

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(250, 220),
                    Width = 100
                };

                dialogoAfilamiento.Controls.AddRange(new Control[] {
                    lblParamA, numParamA,
                    lblParamB, numParamB,
                    lblDescripcion,
                    lblColorNuevaFigura, btnColor,
                    btnAceptar, btnCancelar
                });

                if (dialogoAfilamiento.ShowDialog() == DialogResult.OK)
                {
                    float paramA = (float)numParamA.Value;
                    float paramB = (float)numParamB.Value;

                    float[,] matrizAfilamiento = new float[4, 4] {
                        { 1, 0, 0, 0 },
                        { 0, 1, 0, 0 },
                        { -paramA, -paramB, 1, 0 },
                        { 0, 0, 0, 1 }
                    };

                    List<Point3D> nuevosPuntos = new List<Point3D>();
                    char letraActual = 'A';

                    foreach (var puntoOriginal in figuraOriginal.Puntos)
                    {
                        float[,] matrizPunto = new float[1, 4] {
                    { puntoOriginal.X, puntoOriginal.Y, puntoOriginal.Z, 1 }
                };

                        float[] resultado = new float[4];
                        for (int i = 0; i < 4; i++)
                        {
                            resultado[i] = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                resultado[i] += matrizPunto[0, j] * matrizAfilamiento[j, i];
                            }
                        }

                        Point3D nuevoPunto = new Point3D(
                            resultado[0],
                            resultado[1],
                            resultado[2]
                        )
                        {
                            Etiqueta = letraActual.ToString(),
                            Color = colorNuevaFigura,
                            Seleccionado = false
                        };

                        nuevosPuntos.Add(nuevoPunto);
                        letraActual++;
                        if (letraActual > 'Z') letraActual = 'A';
                    }

                    List<Line3D> nuevasLineas = new List<Line3D>();
                    foreach (var lineaOriginal in figuraOriginal.Lineas)
                    {
                        int indice1 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto1);
                        int indice2 = figuraOriginal.Puntos.IndexOf(lineaOriginal.Punto2);

                        if (indice1 >= 0 && indice2 >= 0 && indice1 < nuevosPuntos.Count && indice2 < nuevosPuntos.Count)
                        {
                            nuevasLineas.Add(new Line3D(nuevosPuntos[indice1], nuevosPuntos[indice2], colorNuevaFigura));
                        }
                    }

                    string nombreBase = nombreFiguraSeleccionada;
                    string sufijo = "_Afilamiento";

                    int contador = 1;
                    string nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    while (figuras.Any(f => f.Nombre == nuevoNombre))
                    {
                        contador++;
                        nuevoNombre = $"{nombreBase}{sufijo} {contador}";
                    }

                    Figura3D nuevaFigura = new Figura3D(nuevoNombre, nuevosPuntos, nuevasLineas);

                    figuras.Add(nuevaFigura);
                    listBoxHistorialFiguras.Items.Add(nuevoNombre);

                    listBoxHistorialFiguras.SelectedItem = nuevoNombre;

                    puntos.Clear();
                    lineas.Clear();

                    foreach (var punto in figuraOriginal.Puntos)
                    {
                        puntos.Add(new Point3D(punto.X, punto.Y, punto.Z)
                        {
                            Etiqueta = punto.Etiqueta,
                            Color = punto.Color,
                            Seleccionado = punto.Seleccionado
                        });
                    }

                    foreach (var linea in figuraOriginal.Lineas)
                    {
                        Point3D punto1 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto1.Etiqueta);
                        Point3D punto2 = puntos.FirstOrDefault(p => p.Etiqueta == linea.Punto2.Etiqueta);

                        if (punto1 != null && punto2 != null)
                        {
                            lineas.Add(new Line3D(punto1, punto2, linea.Color));
                        }
                    }

                    foreach (var punto in nuevosPuntos)
                    {
                        puntos.Add(punto);
                    }

                    foreach (var linea in nuevasLineas)
                    {
                        lineas.Add(linea);
                    }

                    ActualizarListaCoordenadas();

                    pictureBox1.Invalidate();

                    MessageBox.Show($"Afilamiento aplicado. Se ha creado la figura '{nuevoNombre}'.",
                                   "Afilamiento completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}