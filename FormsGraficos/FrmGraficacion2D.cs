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
    public partial class FrmGraficacion2D : Form
    {
        // Variables para el plano cartesiano
        private int origenX, origenY;
        private int escala = 20; 
        private Pen lapizEjes = new Pen(Color.Black, 2);
        private Pen lapizCuadricula = new Pen(Color.LightGray, 1);
        private Pen lapizFigura = new Pen(Color.Blue, 2);
        private Pen lapizPunto = new Pen(Color.Red, 5);
        private Font fuenteCoordenadas = new Font("Arial", 8);
        private SolidBrush brushTexto = new SolidBrush(Color.Black);

        // Variables para la figura actual
        private List<Point> puntosActuales = new List<Point>();
        private List<string> etiquetasPuntos = new List<string>();
        private char ultimaEtiqueta = 'A';

        // Almacenamiento de figuras
        private Dictionary<string, List<Point>> figurasGuardadas = new Dictionary<string, List<Point>>();
        private Dictionary<string, List<string>> etiquetasFiguras = new Dictionary<string, List<string>>();
        // Agregar un diccionario para almacenar los colores de las figuras
        private Dictionary<string, Color> coloresFiguras = new Dictionary<string, Color>();
        private int contadorFiguras = 1;

        // Variable para almacenar la figura actualmente seleccionada
        private string figuraSeleccionadaActual = "";

        public FrmGraficacion2D()
        {
            InitializeComponent();
        }

        private void FrmGraficacion2D_Load(object sender, EventArgs e)
        {
            // Inicializar componentes
            pictureBox1.BackColor = Color.White;

            // Calcular el origen (centro del PictureBox)
            origenX = pictureBox1.Width / 2;
            origenY = pictureBox1.Height / 2;

            // Dibujar el plano cartesiano inicialmente
            DibujarPlanoCartesiano();
        }

        private void DibujarPlanoCartesiano()
        {
            // Crear un bitmap del tamaño del PictureBox
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; 

                // Calcular límites para dibujar la cuadrícula
                int minX = -origenX / escala;
                int maxX = (pictureBox1.Width - origenX) / escala;
                int minY = -origenY / escala;
                int maxY = (pictureBox1.Height - origenY) / escala;

                // Dibujar cuadrícula
                for (int x = minX; x <= maxX; x++)
                {
                    g.DrawLine(lapizCuadricula,
                        origenX + x * escala, 0,
                        origenX + x * escala, pictureBox1.Height);
                }

                for (int y = minY; y <= maxY; y++)
                {
                    g.DrawLine(lapizCuadricula,
                        0, origenY + y * escala,
                        pictureBox1.Width, origenY + y * escala);
                }

                // Dibujar ejes principales
                g.DrawLine(lapizEjes, 0, origenY, pictureBox1.Width, origenY); // Eje X
                g.DrawLine(lapizEjes, origenX, 0, origenX, pictureBox1.Height); // Eje Y

                // Dibujar marcas en los ejes
                for (int x = minX; x <= maxX; x++)
                {
                    if (x != 0)
                    {
                        g.DrawString(x.ToString(), fuenteCoordenadas, brushTexto,
                            origenX + x * escala - 5, origenY + 5);
                    }
                }

                for (int y = minY; y <= maxY; y++)
                {
                    if (y != 0)
                    {
                        g.DrawString((-y).ToString(), fuenteCoordenadas, brushTexto,
                            origenX + 5, origenY + y * escala - 5);
                    }
                }

                // Marcar el origen (0,0)
                g.DrawString("0", fuenteCoordenadas, brushTexto, origenX + 5, origenY + 5);

                // Dibujar los puntos y líneas de la figura actual
                if (puntosActuales.Count > 0)
                {
                    // Determinar el color de la figura
                    Pen lapizFiguraActual = lapizFigura;

                    // Si la figura actual está en el diccionario de colores, usar ese color
                    if (!string.IsNullOrEmpty(figuraSeleccionadaActual) && coloresFiguras.ContainsKey(figuraSeleccionadaActual))
                    {
                        lapizFiguraActual = new Pen(coloresFiguras[figuraSeleccionadaActual], 2);
                    }

                    // Dibujar líneas entre puntos
                    for (int i = 0; i < puntosActuales.Count - 1; i++)
                    {
                        g.DrawLine(lapizFiguraActual, puntosActuales[i], puntosActuales[i + 1]);
                    }

                    // Si hay más de un punto, cerrar la figura conectando el último con el primero
                    if (puntosActuales.Count > 1)
                    {
                        g.DrawLine(lapizFiguraActual, puntosActuales[puntosActuales.Count - 1], puntosActuales[0]);
                    }

                    // Dibujar puntos
                    for (int i = 0; i < puntosActuales.Count; i++)
                    {
                        // Usar el mismo color para los puntos que para las líneas
                        SolidBrush brushPunto = new SolidBrush(lapizFiguraActual.Color);

                        // Dibujar el punto con mejor visualización
                        g.FillEllipse(brushPunto,
                            puntosActuales[i].X - 3, puntosActuales[i].Y - 3,
                            6, 6);
                        g.DrawEllipse(new Pen(lapizFiguraActual.Color, 1),
                            puntosActuales[i].X - 3, puntosActuales[i].Y - 3,
                            6, 6);

                        // Mostrar coordenadas junto al punto con 2 decimales para mayor precisión
                        double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                        double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);

                        g.DrawString(etiquetasPuntos[i] + "(" + pX + "," + pY + ")",
                            fuenteCoordenadas, brushTexto,
                            puntosActuales[i].X + 5, puntosActuales[i].Y - 15);
                    }
                }
            }

            // Asignar el bitmap al PictureBox
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Obtener posición del clic en coordenadas del picturebox
            MouseEventArgs me = (MouseEventArgs)e;
            Point puntoClick = me.Location;

            // Agregar el punto a la lista de puntos actuales
            puntosActuales.Add(puntoClick);

            // Agregar la etiqueta del punto (A, B, C, etc.)
            string etiqueta = ultimaEtiqueta.ToString();
            etiquetasPuntos.Add(etiqueta);
            ultimaEtiqueta++;

            // Convertir coordenadas de pantalla a plano cartesiano CON DECIMALES
            double pX = Math.Round((double)(puntoClick.X - origenX) / escala, 2);
            double pY = Math.Round((double)(origenY - puntoClick.Y) / escala, 2);

            // Actualizar listBoxCoordenadas con las coordenadas del punto
            listBoxCoordenadas.Items.Add($"Punto {etiqueta}: ({pX},{pY})");

            // Redibujar el plano con los puntos actualizados
            DibujarPlanoCartesiano();
        }

        private void btnGuardarFiguras_Click(object sender, EventArgs e)
        {
            if (puntosActuales.Count > 0)
            {
                // Crear nombre para la figura
                string nombreFigura = $"Figura {contadorFiguras}";
                contadorFiguras++;

                // Guardar la figura y sus etiquetas
                figurasGuardadas.Add(nombreFigura, new List<Point>(puntosActuales));
                etiquetasFiguras.Add(nombreFigura, new List<string>(etiquetasPuntos));

                // Guardar el color de la figura (por defecto, el color del lapizFigura)
                coloresFiguras.Add(nombreFigura, lapizFigura.Color);

                // Agregar la figura al historial
                listBoxHistorialFig.Items.Add(nombreFigura);

                // Mostrar coordenadas de la figura guardada CON DECIMALES
                listBoxCoordenadas.Items.Add($"--- {nombreFigura} ---");
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                    double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                    listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
                }

                // Mantener el plano con la figura pero reiniciar las variables para una nueva figura
                ultimaEtiqueta = 'A';
                puntosActuales.Clear();
                etiquetasPuntos.Clear();
                figuraSeleccionadaActual = "";
            }
            else
            {
                MessageBox.Show("No hay figura para guardar. Dibuje puntos primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLimpiarPlano_Click(object sender, EventArgs e)
        {
            // Limpiar los puntos actuales sin afectar el historial
            puntosActuales.Clear();
            etiquetasPuntos.Clear();
            ultimaEtiqueta = 'A';
            figuraSeleccionadaActual = "";

            // Limpiar información en listBoxCoordenadas que no sea de figuras guardadas
            listBoxCoordenadas.Items.Clear();

            // Redibuja el plano cartesiano limpio
            DibujarPlanoCartesiano();
        }

        private void btnBorrarTodo_Click(object sender, EventArgs e)
        {
            // Limpiar todos los datos
            puntosActuales.Clear();
            etiquetasPuntos.Clear();
            ultimaEtiqueta = 'A';
            figurasGuardadas.Clear();
            etiquetasFiguras.Clear();
            coloresFiguras.Clear();
            contadorFiguras = 1;
            figuraSeleccionadaActual = "";

            // Limpiar listas visuales
            listBoxHistorialFig.Items.Clear();
            listBoxCoordenadas.Items.Clear();

            // Redibuja el plano cartesiano limpio
            DibujarPlanoCartesiano();
        }

        private void listBoxHistorialFig_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxHistorialFig.SelectedIndex != -1)
            {
                string figuraSeleccionada = listBoxHistorialFig.SelectedItem.ToString();

                if (figurasGuardadas.ContainsKey(figuraSeleccionada))
                {
                    // Limpiar puntos actuales
                    puntosActuales.Clear();
                    etiquetasPuntos.Clear();

                    // Cargar los puntos de la figura seleccionada
                    puntosActuales.AddRange(figurasGuardadas[figuraSeleccionada]);
                    etiquetasPuntos.AddRange(etiquetasFiguras[figuraSeleccionada]);

                    // Actualizar la figura seleccionada actual
                    figuraSeleccionadaActual = figuraSeleccionada;

                    // Actualizar la última etiqueta para nuevos puntos
                    if (etiquetasPuntos.Count > 0)
                    {
                        ultimaEtiqueta = etiquetasPuntos.Last()[0];
                        ultimaEtiqueta++;
                    }
                    else
                    {
                        ultimaEtiqueta = 'A';
                    }

                    // Redibujar el plano con la figura seleccionada
                    DibujarPlanoCartesiano();

                    // Mostrar coordenadas de la figura seleccionada CON DECIMALES
                    listBoxCoordenadas.Items.Clear();
                    listBoxCoordenadas.Items.Add($"--- {figuraSeleccionada} ---");
                    for (int i = 0; i < puntosActuales.Count; i++)
                    {
                        double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                        double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                        listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
                    }
                }
            }
        }

        private void listBoxCoordenadas_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // Los métodos de los botones de transformación no se implementan ahora según la solicitud
        private void btnTraslacion_Click(object sender, EventArgs e)
        {
            // Verificar si hay una figura seleccionada
            if (string.IsNullOrEmpty(figuraSeleccionadaActual) || puntosActuales.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una figura del historial primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Crear un formulario más grande para la entrada de datos
            Form inputForm = new Form()
            {
                Width = 500,  // Aumentado para mejor visualización
                Height = 280,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Traslación de Figura",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Crear etiquetas y textbox para la traslación en X
            Label lblX = new Label() { Left = 30, Top = 30, Text = "Traslación en X:", Width = 120 };
            TextBox txtX = new TextBox() { Left = 150, Top = 30, Width = 300 };
            Label lblXInfo = new Label() { Left = 150, Top = 55, Width = 300, Text = "(+ Derecha, - Izquierda)" };

            // Crear etiquetas y textbox para la traslación en Y
            Label lblY = new Label() { Left = 30, Top = 90, Text = "Traslación en Y:", Width = 120 };
            TextBox txtY = new TextBox() { Left = 150, Top = 90, Width = 300 };
            Label lblYInfo = new Label() { Left = 150, Top = 115, Width = 300, Text = "(+ Arriba, - Abajo)" };

            // Botones para aceptar y cancelar
            Button btnAceptar = new Button() { Text = "Aceptar", Left = 260, Width = 100, Top = 180, DialogResult = DialogResult.OK };
            Button btnCancelar = new Button() { Text = "Cancelar", Left = 370, Width = 100, Top = 180, DialogResult = DialogResult.Cancel };

            btnAceptar.Click += (sender, e) => { inputForm.DialogResult = DialogResult.OK; };
            btnCancelar.Click += (sender, e) => { inputForm.DialogResult = DialogResult.Cancel; };

            // Agregar controles al formulario
            inputForm.Controls.Add(lblX);
            inputForm.Controls.Add(txtX);
            inputForm.Controls.Add(lblXInfo);
            inputForm.Controls.Add(lblY);
            inputForm.Controls.Add(txtY);
            inputForm.Controls.Add(lblYInfo);
            inputForm.Controls.Add(btnAceptar);
            inputForm.Controls.Add(btnCancelar);

            // Mostrar el formulario como diálogo
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                // Validar entradas - aceptar números decimales y enteros
                double tx, ty;
                bool xValido = double.TryParse(txtX.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out tx);
                bool yValido = double.TryParse(txtY.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out ty);

                if (!xValido || !yValido)
                {
                    MessageBox.Show("Por favor ingrese valores numéricos válidos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Guardar los puntos y etiquetas originales
                List<Point> puntosOriginales = new List<Point>(puntosActuales);
                List<string> etiquetasOriginales = new List<string>(etiquetasPuntos);
                string nombreFiguraOriginal = figuraSeleccionadaActual;
                Color colorOriginal = coloresFiguras.ContainsKey(figuraSeleccionadaActual) ?
                                     coloresFiguras[figuraSeleccionadaActual] : lapizFigura.Color;

                // Crear matriz con puntos originales para la traslación
                double[,] matrizPuntos = new double[puntosActuales.Count, 3];

                // Llenar la matriz con coordenadas convertidas a plano cartesiano
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    double pX = (double)(puntosActuales[i].X - origenX) / escala;
                    double pY = (double)(origenY - puntosActuales[i].Y) / escala;
                    matrizPuntos[i, 0] = pX;
                    matrizPuntos[i, 1] = pY;
                    matrizPuntos[i, 2] = 1; // Coordenadas homogéneas
                }

                // Crear la matriz de traslación
                double[,] matrizTraslacion = new double[3, 3] {
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { tx, ty, 1 }
                };

                // Crear una lista temporal para los nuevos puntos trasladados
                List<Point> nuevosPuntos = new List<Point>();

                // Aplicar la traslación a cada punto
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    // Aplicar la traslación mediante multiplicación de matrices
                    double[] resultado = MultiplicarMatrizPunto(matrizPuntos[i, 0], matrizPuntos[i, 1], matrizTraslacion);

                    // Convertir de coordenadas cartesianas a coordenadas de pantalla
                    int nuevaX = (int)Math.Round(origenX + resultado[0] * escala);
                    int nuevaY = (int)Math.Round(origenY - resultado[1] * escala);

                    nuevosPuntos.Add(new Point(nuevaX, nuevaY));
                }

                // Actualizar los puntos actuales con los nuevos puntos trasladados
                puntosActuales.Clear();
                puntosActuales.AddRange(nuevosPuntos);

                // Redibujar el plano con los puntos trasladados
                DibujarPlanoCartesiano();

                // Actualizar la visualización de coordenadas
                ActualizarListBoxCoordenadas("Traslación", tx, ty);

                // Preguntar si se quieren guardar las figuras
                DialogResult result = MessageBox.Show(
                    "¿Desea guardar la figura trasladada?",
                    "Guardar Figura",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Generar el nombre según la lógica requerida
                    string nombreNuevaFigura = GenerarNombreTraslacion(nombreFiguraOriginal);

                    // Guardar la figura trasladada
                    figurasGuardadas.Add(nombreNuevaFigura, new List<Point>(puntosActuales));
                    etiquetasFiguras.Add(nombreNuevaFigura, new List<string>(etiquetasPuntos));
                    coloresFiguras.Add(nombreNuevaFigura, colorOriginal); // Mantener el mismo color
                    listBoxHistorialFig.Items.Add(nombreNuevaFigura);

                    // Actualizar la figura seleccionada
                    figuraSeleccionadaActual = nombreNuevaFigura;
                }
            }
        }

        // Método para multiplicar matriz de traslación por un punto
        private double[] MultiplicarMatrizPunto(double x, double y, double[,] matriz)
        {
            double[] punto = new double[] { x, y, 1 };
            double[] resultado = new double[3];

            for (int i = 0; i < 3; i++)
            {
                resultado[i] = 0;
                for (int j = 0; j < 3; j++)
                {
                    resultado[i] += punto[j] * matriz[j, i];
                }
            }

            return resultado;
        }

        // Método para generar nombre de traslación según la lógica requerida
        private string GenerarNombreTraslacion(string nombreOriginal)
        {
            // Extraer la base del nombre original (sin el último "_TraslacionX")
            string nombreBase = nombreOriginal;
            int traslacionNumero = 1;

            // Si el nombre contiene "_Traslacion" seguido de dígitos al final
            if (nombreOriginal.Contains("_Traslacion"))
            {
                // Buscar la posición de la última ocurrencia de "_Traslacion"
                int ultimoIndice = nombreOriginal.LastIndexOf("_Traslacion");

                // Verificar si después de "_Traslacion" hay un número
                if (ultimoIndice != -1 && ultimoIndice + 11 <= nombreOriginal.Length) // "_Traslacion" tiene 11 caracteres
                {
                    string numStr = "";
                    int i = ultimoIndice + 11;

                    // Extraer todos los dígitos que siguen a "_Traslacion"
                    while (i < nombreOriginal.Length && char.IsDigit(nombreOriginal[i]))
                    {
                        numStr += nombreOriginal[i];
                        i++;
                    }

                    // Buscar figuras del mismo nombre base para encontrar el siguiente número
                    string prefijo = nombreOriginal + "_Traslacion";
                    traslacionNumero = 1; // Valor por defecto si no hay traslaciones previas

                    foreach (string nombreFigura in figurasGuardadas.Keys)
                    {
                        if (nombreFigura.StartsWith(prefijo))
                        {
                            // Extraer el número después del prefijo
                            string numParte = nombreFigura.Substring(prefijo.Length);
                            if (int.TryParse(numParte, out int num) && num >= traslacionNumero)
                            {
                                traslacionNumero = num + 1;
                            }
                        }
                    }

                    // Retornar el nombre original + "_Traslacion" + siguiente número
                    return nombreOriginal + "_Traslacion" + traslacionNumero;
                }
            }

            // Buscar figuras con el mismo nombre base para encontrar el siguiente número
            string prefijoBase = nombreOriginal + "_Traslacion";
            traslacionNumero = 1; // Valor por defecto

            foreach (string nombreFigura in figurasGuardadas.Keys)
            {
                if (nombreFigura.StartsWith(prefijoBase))
                {
                    // Extraer el número después del prefijo
                    string numParte = nombreFigura.Substring(prefijoBase.Length);
                    if (int.TryParse(numParte, out int num) && num >= traslacionNumero)
                    {
                        traslacionNumero = num + 1;
                    }
                }
            }

            // Retornar el nombre base + "_Traslacion" + siguiente número
            return nombreOriginal + "_Traslacion" + traslacionNumero;
        }

        // Método para actualizar el listbox de coordenadas
        private void ActualizarListBoxCoordenadas(string operacion, double tx, double ty)
        {
            listBoxCoordenadas.Items.Clear();
            listBoxCoordenadas.Items.Add($"--- {figuraSeleccionadaActual} ({operacion}: TX={tx}, TY={ty}) ---");

            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
            }
        }



        private void btnEscalacionRO_Click(object sender, EventArgs e)
        {
            // Verificar si hay una figura seleccionada
            if (string.IsNullOrEmpty(figuraSeleccionadaActual) || puntosActuales.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una figura del historial primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Crear un formulario para la entrada de datos
            Form inputForm = new Form()
            {
                Width = 550,
                Height = 380,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Escalación con Respecto al Origen",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Crear opciones para agrandar o reducir (RadioButtons)
            GroupBox grpTipoEscalacion = new GroupBox()
            {
                Text = "Tipo de Escalación",
                Left = 30,
                Top = 20,
                Width = 480,
                Height = 70
            };

            RadioButton radAgrandar = new RadioButton()
            {
                Text = "Agrandar",
                Left = 20,
                Top = 30,
                Width = 100,
                Checked = true
            };

            RadioButton radReducir = new RadioButton()
            {
                Text = "Reducir",
                Left = 180,
                Top = 30,
                Width = 100
            };

            // Agregar los radio buttons al grupo
            grpTipoEscalacion.Controls.Add(radAgrandar);
            grpTipoEscalacion.Controls.Add(radReducir);

            // Crear etiquetas y textbox para el factor de escalación
            Label lblFactor = new Label() { Left = 30, Top = 110, Text = "Factor:", Width = 120 };
            TextBox txtFactor = new TextBox() { Left = 150, Top = 110, Width = 300 };

            // Labels dinámicos que cambiarán según la opción seleccionada
            Label lblInfoTitulo = new Label() { Left = 30, Top = 150, Font = new Font("Arial", 9, FontStyle.Bold), Width = 400 };
            Label lblInfoDetalle = new Label() { Left = 30, Top = 175, Width = 480, Height = 120 };

            // Configuración inicial para "Agrandar"
            ActualizarInformacionEscalacion(lblInfoTitulo, lblInfoDetalle, true);

            // Evento para cambiar el texto de información cuando se cambie la selección
            radAgrandar.CheckedChanged += (s, ev) => {
                ActualizarInformacionEscalacion(lblInfoTitulo, lblInfoDetalle, radAgrandar.Checked);
            };

            radReducir.CheckedChanged += (s, ev) => {
                ActualizarInformacionEscalacion(lblInfoTitulo, lblInfoDetalle, !radReducir.Checked);
            };

            // Botones para aceptar y cancelar
            Button btnAceptar = new Button() { Text = "Aceptar", Left = 260, Width = 100, Top = 300, DialogResult = DialogResult.OK };
            Button btnCancelar = new Button() { Text = "Cancelar", Left = 370, Width = 100, Top = 300, DialogResult = DialogResult.Cancel };

            btnAceptar.Click += (s, ev) => { inputForm.DialogResult = DialogResult.OK; };
            btnCancelar.Click += (s, ev) => { inputForm.DialogResult = DialogResult.Cancel; };

            // Función local para actualizar la información según la opción seleccionada
            void ActualizarInformacionEscalacion(Label titulo, Label detalle, bool esAgrandar)
            {
                if (esAgrandar)
                {
                    titulo.Text = "Instrucciones para Agrandar:";
                    detalle.Text = "Ingrese el factor de aumento deseado:\n" +
                                  "• Use 2 para duplicar el tamaño\n" +
                                  "• Use 3 para triplicar el tamaño\n" +
                                  "• Puede usar decimales (ej: 1.5 para aumentar un 50%)";
                }
                else
                {
                    titulo.Text = "Instrucciones para Reducir:";
                    detalle.Text = "Hay dos formas de indicar la reducción:\n" +
                                  "• Ingrese una fracción directa: 0.5 (mitad), 0.25 (cuarto), etc.\n" +
                                  "• O ingrese el número de veces a reducir (ej: 2 para reducir a la mitad 2 veces).\n" +
                                  "  En este caso se aplicará la fórmula: 1-(valor*0.5)";
                }
            }

            // Agregar controles al formulario
            inputForm.Controls.Add(grpTipoEscalacion);
            inputForm.Controls.Add(lblFactor);
            inputForm.Controls.Add(txtFactor);
            inputForm.Controls.Add(lblInfoTitulo);
            inputForm.Controls.Add(lblInfoDetalle);
            inputForm.Controls.Add(btnAceptar);
            inputForm.Controls.Add(btnCancelar);

            // Mostrar el formulario como diálogo
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                // Validar entrada - aceptar números decimales y enteros
                double factor;
                bool factorValido = double.TryParse(txtFactor.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out factor);

                if (!factorValido || factor <= 0)
                {
                    MessageBox.Show("Por favor ingrese un valor numérico positivo válido.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Calcular el factor de escalación según lo ingresado y la opción seleccionada
                double sx, sy;
                string operacion;

                if (radAgrandar.Checked)
                {
                    // Para agrandar, usamos el factor directamente
                    sx = sy = factor;
                    operacion = "Agrandar";
                }
                else // Reducir
                {
                    // Verificar si el usuario ingresó una fracción directa (menor a 1)
                    if (factor < 1)
                    {
                        // Usar el valor directamente como factor de reducción
                        sx = sy = factor;
                        operacion = "Reducir (fracción directa)";
                    }
                    else
                    {
                        // Aplicar la fórmula para reducción por número de veces
                        sx = sy = 1 - (factor * 0.5);
                        operacion = "Reducir (por fórmula)";
                    }
                }

                // Guardar los puntos y etiquetas originales
                List<Point> puntosOriginales = new List<Point>(puntosActuales);
                List<string> etiquetasOriginales = new List<string>(etiquetasPuntos);
                string nombreFiguraOriginal = figuraSeleccionadaActual;
                Color colorOriginal = coloresFiguras.ContainsKey(figuraSeleccionadaActual) ?
                                     coloresFiguras[figuraSeleccionadaActual] : lapizFigura.Color;

                // Crear matriz con puntos originales para la escalación
                double[,] matrizPuntos = new double[puntosActuales.Count, 3];

                // Llenar la matriz con coordenadas convertidas a plano cartesiano
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    double pX = (double)(puntosActuales[i].X - origenX) / escala;
                    double pY = (double)(origenY - puntosActuales[i].Y) / escala;
                    matrizPuntos[i, 0] = pX;
                    matrizPuntos[i, 1] = pY;
                    matrizPuntos[i, 2] = 1; // Coordenadas homogéneas
                }

                // Crear la matriz de escalación
                double[,] matrizEscalacion = new double[3, 3] {
                    { sx, 0, 0 },
                    { 0, sy, 0 },
                    { 0, 0, 1 }
                };

                // Crear una lista temporal para los nuevos puntos escalados
                List<Point> nuevosPuntos = new List<Point>();

                // Aplicar la escalación a cada punto
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    // Aplicar la escalación mediante multiplicación de matrices
                    double[] resultado = MultiplicarMatrizPunto(matrizPuntos[i, 0], matrizPuntos[i, 1], matrizEscalacion);

                    // Convertir de coordenadas cartesianas a coordenadas de pantalla
                    int nuevaX = (int)Math.Round(origenX + resultado[0] * escala);
                    int nuevaY = (int)Math.Round(origenY - resultado[1] * escala);

                    nuevosPuntos.Add(new Point(nuevaX, nuevaY));
                }

                // Actualizar los puntos actuales con los nuevos puntos escalados
                puntosActuales.Clear();
                puntosActuales.AddRange(nuevosPuntos);

                // Redibujar el plano con los puntos escalados
                DibujarPlanoCartesiano();

                // Actualizar la visualización de coordenadas con detalles del factor usado
                string descripcionFactor = $"{(radAgrandar.Checked ? "Factor: " : "Factor: ")}{factor} → Sx=Sy={sx:F4}";
                ActualizarListBoxCoordenadasEscalacion($"Escalación RO ({operacion})", descripcionFactor);

                // Preguntar si se quieren guardar las figuras
                DialogResult result = MessageBox.Show(
                    "¿Desea guardar la figura escalada?",
                    "Guardar Figura",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Generar el nombre según la lógica requerida
                    string nombreNuevaFigura = GenerarNombreEscalacionRO(nombreFiguraOriginal);

                    // Guardar la figura escalada
                    figurasGuardadas.Add(nombreNuevaFigura, new List<Point>(puntosActuales));
                    etiquetasFiguras.Add(nombreNuevaFigura, new List<string>(etiquetasPuntos));
                    coloresFiguras.Add(nombreNuevaFigura, colorOriginal); // Mantener el mismo color
                    listBoxHistorialFig.Items.Add(nombreNuevaFigura);

                    // Actualizar la figura seleccionada
                    figuraSeleccionadaActual = nombreNuevaFigura;
                }
            }
        }

        // Método para actualizar el listbox de coordenadas para escalación
        private void ActualizarListBoxCoordenadasEscalacion(string operacion, string detallesFactor)
        {
            listBoxCoordenadas.Items.Clear();
            listBoxCoordenadas.Items.Add($"--- {figuraSeleccionadaActual} ({operacion}: {detallesFactor}) ---");

            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
            }
        }

        // Método para generar nombre de escalación según la lógica requerida
        private string GenerarNombreEscalacionRO(string nombreOriginal)
        {
            // Extraer la base del nombre original (sin el último "_EscalacionROX")
            string nombreBase = nombreOriginal;
            int escalacionNumero = 1;

            // Si el nombre contiene "_EscalacionRO" seguido de dígitos al final
            if (nombreOriginal.Contains("_EscalacionRO"))
            {
                // Buscar la posición de la última ocurrencia de "_EscalacionRO"
                int ultimoIndice = nombreOriginal.LastIndexOf("_EscalacionRO");

                // Verificar si después de "_EscalacionRO" hay un número
                if (ultimoIndice != -1 && ultimoIndice + 13 <= nombreOriginal.Length) // "_EscalacionRO" tiene 13 caracteres
                {
                    string numStr = "";
                    int i = ultimoIndice + 13;

                    // Extraer todos los dígitos que siguen a "_EscalacionRO"
                    while (i < nombreOriginal.Length && char.IsDigit(nombreOriginal[i]))
                    {
                        numStr += nombreOriginal[i];
                        i++;
                    }

                    // Buscar figuras del mismo nombre base para encontrar el siguiente número
                    string prefijo = nombreOriginal + "_EscalacionRO";
                    escalacionNumero = 1; // Valor por defecto si no hay escalaciones previas

                    foreach (string nombreFigura in figurasGuardadas.Keys)
                    {
                        if (nombreFigura.StartsWith(prefijo))
                        {
                            // Extraer el número después del prefijo
                            string numParte = nombreFigura.Substring(prefijo.Length);
                            if (int.TryParse(numParte, out int num) && num >= escalacionNumero)
                            {
                                escalacionNumero = num + 1;
                            }
                        }
                    }

                    // Retornar el nombre original + "_EscalacionRO" + siguiente número
                    return nombreOriginal + "_EscalacionRO" + escalacionNumero;
                }
            }

            // Buscar figuras con el mismo nombre base para encontrar el siguiente número
            string prefijoBase = nombreOriginal + "_EscalacionRO";
            escalacionNumero = 1; // Valor por defecto

            foreach (string nombreFigura in figurasGuardadas.Keys)
            {
                if (nombreFigura.StartsWith(prefijoBase))
                {
                    // Extraer el número después del prefijo
                    string numParte = nombreFigura.Substring(prefijoBase.Length);
                    if (int.TryParse(numParte, out int num) && num >= escalacionNumero)
                    {
                        escalacionNumero = num + 1;
                    }
                }
            }

            // Retornar el nombre base + "_EscalacionRO" + siguiente número
            return nombreOriginal + "_EscalacionRO" + escalacionNumero;
        }



        private void btnEscalacionRPF_Click(object sender, EventArgs e)
        {
            // Verificar si hay una figura seleccionada
            if (string.IsNullOrEmpty(figuraSeleccionadaActual) || puntosActuales.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una figura del historial primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Crear un formulario para la entrada de datos
            Form inputForm = new Form()
            {
                Width = 550,
                Height = 450,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Escalación con Respecto a Punto Fijo",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Crear opciones para agrandar o reducir (RadioButtons)
            GroupBox grpTipoEscalacion = new GroupBox()
            {
                Text = "Tipo de Escalación",
                Left = 30,
                Top = 20,
                Width = 480,
                Height = 70
            };

            RadioButton radAgrandar = new RadioButton()
            {
                Text = "Agrandar",
                Left = 20,
                Top = 30,
                Width = 100,
                Checked = true
            };

            RadioButton radReducir = new RadioButton()
            {
                Text = "Reducir",
                Left = 180,
                Top = 30,
                Width = 100
            };

            // Agregar los radio buttons al grupo
            grpTipoEscalacion.Controls.Add(radAgrandar);
            grpTipoEscalacion.Controls.Add(radReducir);

            // Crear etiquetas y textbox para el factor de escalación
            Label lblFactor = new Label() { Left = 30, Top = 110, Text = "Factor:", Width = 120 };
            TextBox txtFactor = new TextBox() { Left = 150, Top = 110, Width = 300 };

            // Crear etiquetas y ComboBox para seleccionar el punto fijo
            Label lblPuntoFijo = new Label() { Left = 30, Top = 150, Text = "Punto Fijo:", Width = 120 };
            ComboBox cmbPuntoFijo = new ComboBox() { Left = 150, Top = 150, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

            // Etiquetas para mostrar las coordenadas del punto seleccionado
            Label lblCoordenadasPunto = new Label() { Left = 150, Top = 180, Width = 300 };

            // Llenar el ComboBox con los puntos de la figura actual
            for (int i = 0; i < etiquetasPuntos.Count; i++)
            {
                cmbPuntoFijo.Items.Add(etiquetasPuntos[i]);
            }

            // Si hay puntos disponibles, seleccionar el primero por defecto
            if (cmbPuntoFijo.Items.Count > 0)
            {
                cmbPuntoFijo.SelectedIndex = 0;

                // Mostrar las coordenadas del punto seleccionado
                int indice = 0;
                double pX = Math.Round((double)(puntosActuales[indice].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[indice].Y) / escala, 2);
                lblCoordenadasPunto.Text = $"Coordenadas: ({pX}, {pY})";
            }

            // Actualizar las coordenadas cuando se cambia la selección del punto
            cmbPuntoFijo.SelectedIndexChanged += (s, ev) => {
                if (cmbPuntoFijo.SelectedIndex >= 0)
                {
                    int indice = cmbPuntoFijo.SelectedIndex;
                    double pX = Math.Round((double)(puntosActuales[indice].X - origenX) / escala, 2);
                    double pY = Math.Round((double)(origenY - puntosActuales[indice].Y) / escala, 2);
                    lblCoordenadasPunto.Text = $"Coordenadas: ({pX}, {pY})";
                }
            };

            // Labels dinámicos que cambiarán según la opción seleccionada
            Label lblInfoTitulo = new Label() { Left = 30, Top = 210, Font = new Font("Arial", 9, FontStyle.Bold), Width = 400 };
            Label lblInfoDetalle = new Label() { Left = 30, Top = 235, Width = 480, Height = 120 };

            // Configuración inicial para "Agrandar"
            ActualizarInformacionEscalacion(lblInfoTitulo, lblInfoDetalle, true);

            // Evento para cambiar el texto de información cuando se cambie la selección
            radAgrandar.CheckedChanged += (s, ev) => {
                ActualizarInformacionEscalacion(lblInfoTitulo, lblInfoDetalle, radAgrandar.Checked);
            };

            radReducir.CheckedChanged += (s, ev) => {
                ActualizarInformacionEscalacion(lblInfoTitulo, lblInfoDetalle, !radReducir.Checked);
            };

            // Función local para actualizar la información según la opción seleccionada
            void ActualizarInformacionEscalacion(Label titulo, Label detalle, bool esAgrandar)
            {
                if (esAgrandar)
                {
                    titulo.Text = "Instrucciones para Agrandar:";
                    detalle.Text = "Ingrese el factor de aumento deseado:\n" +
                                  "• Use 2 para duplicar el tamaño\n" +
                                  "• Use 3 para triplicar el tamaño\n" +
                                  "• Puede usar decimales (ej: 1.5 para aumentar un 50%)";
                }
                else
                {
                    titulo.Text = "Instrucciones para Reducir:";
                    detalle.Text = "Hay dos formas de indicar la reducción:\n" +
                                  "• Ingrese una fracción directa: 0.5 (mitad), 0.25 (cuarto), etc.\n" +
                                  "• O ingrese el número de veces a reducir (ej: 2 para reducir a la mitad 2 veces).\n" +
                                  "  En este caso se aplicará la fórmula: 1-(valor*0.5)";
                }
            }

            // Botones para aceptar y cancelar
            Button btnAceptar = new Button() { Text = "Aceptar", Left = 260, Width = 100, Top = 370, DialogResult = DialogResult.OK };
            Button btnCancelar = new Button() { Text = "Cancelar", Left = 370, Width = 100, Top = 370, DialogResult = DialogResult.Cancel };

            btnAceptar.Click += (s, ev) => { inputForm.DialogResult = DialogResult.OK; };
            btnCancelar.Click += (s, ev) => { inputForm.DialogResult = DialogResult.Cancel; };

            // Agregar controles al formulario
            inputForm.Controls.Add(grpTipoEscalacion);
            inputForm.Controls.Add(lblFactor);
            inputForm.Controls.Add(txtFactor);
            inputForm.Controls.Add(lblPuntoFijo);
            inputForm.Controls.Add(cmbPuntoFijo);
            inputForm.Controls.Add(lblCoordenadasPunto);
            inputForm.Controls.Add(lblInfoTitulo);
            inputForm.Controls.Add(lblInfoDetalle);
            inputForm.Controls.Add(btnAceptar);
            inputForm.Controls.Add(btnCancelar);

            // Mostrar el formulario como diálogo
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                // Validar entrada - aceptar números decimales y enteros
                double factor;
                bool factorValido = double.TryParse(txtFactor.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out factor);

                if (!factorValido || factor <= 0)
                {
                    MessageBox.Show("Por favor ingrese un valor numérico positivo válido.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verificar si se seleccionó un punto fijo
                if (cmbPuntoFijo.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe seleccionar un punto fijo para la escalación.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Obtener el índice del punto fijo seleccionado
                int indicePuntoFijo = cmbPuntoFijo.SelectedIndex;

                // Calcular el factor de escalación según lo ingresado y la opción seleccionada
                double sx, sy;
                string operacion;

                if (radAgrandar.Checked)
                {
                    // Para agrandar, usamos el factor directamente
                    sx = sy = factor;
                    operacion = "Agrandar";
                }
                else // Reducir
                {
                    // Verificar si el usuario ingresó una fracción directa (menor a 1)
                    if (factor < 1)
                    {
                        // Usar el valor directamente como factor de reducción
                        sx = sy = factor;
                        operacion = "Reducir (fracción directa)";
                    }
                    else
                    {
                        // Aplicar la fórmula para reducción por número de veces
                        sx = sy = 1 - (factor * 0.5);
                        operacion = "Reducir (por fórmula)";
                    }
                }

                // Guardar los puntos y etiquetas originales
                List<Point> puntosOriginales = new List<Point>(puntosActuales);
                List<string> etiquetasOriginales = new List<string>(etiquetasPuntos);
                string nombreFiguraOriginal = figuraSeleccionadaActual;
                Color colorOriginal = coloresFiguras.ContainsKey(figuraSeleccionadaActual) ?
                                     coloresFiguras[figuraSeleccionadaActual] : lapizFigura.Color;

                // Obtener las coordenadas del punto fijo en el plano cartesiano
                double psx = (double)(puntosActuales[indicePuntoFijo].X - origenX) / escala;
                double psy = (double)(origenY - puntosActuales[indicePuntoFijo].Y) / escala;

                // Crear matriz con puntos originales para la escalación
                double[,] matrizPuntos = new double[puntosActuales.Count, 3];

                // Llenar la matriz con coordenadas convertidas a plano cartesiano
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    double pX = (double)(puntosActuales[i].X - origenX) / escala;
                    double pY = (double)(origenY - puntosActuales[i].Y) / escala;
                    matrizPuntos[i, 0] = pX;
                    matrizPuntos[i, 1] = pY;
                    matrizPuntos[i, 2] = 1; // Coordenadas homogéneas
                }

                // Crear la matriz de escalación con respecto al punto fijo
                double[,] matrizEscalacion = new double[3, 3] {
                    { sx, 0, 0 },
                    { 0, sy, 0 },
                    { psx * (1 - sx), psy * (1 - sy), 1 }
                };

                // Crear una lista temporal para los nuevos puntos escalados
                List<Point> nuevosPuntos = new List<Point>();

                // Aplicar la escalación a cada punto
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    // Aplicar la escalación mediante multiplicación de matrices
                    double[] resultado = MultiplicarMatrizPunto(matrizPuntos[i, 0], matrizPuntos[i, 1], matrizEscalacion);

                    // Convertir de coordenadas cartesianas a coordenadas de pantalla
                    int nuevaX = (int)Math.Round(origenX + resultado[0] * escala);
                    int nuevaY = (int)Math.Round(origenY - resultado[1] * escala);

                    nuevosPuntos.Add(new Point(nuevaX, nuevaY));
                }

                // Actualizar los puntos actuales con los nuevos puntos escalados
                puntosActuales.Clear();
                puntosActuales.AddRange(nuevosPuntos);

                // Redibujar el plano con los puntos escalados
                DibujarPlanoCartesiano();

                // Actualizar la visualización de coordenadas con detalles del factor usado
                string descripcionFactor = $"{(radAgrandar.Checked ? "Factor: " : "Factor: ")}{factor} → Sx=Sy={sx:F4}, Punto Fijo: {etiquetasPuntos[indicePuntoFijo]}({psx:F2}, {psy:F2})";
                ActualizarListBoxCoordenadasEscalacionRPF($"Escalación RPF ({operacion})", descripcionFactor);

                // Preguntar si se quieren guardar las figuras
                DialogResult result = MessageBox.Show(
                    "¿Desea guardar la figura escalada?",
                    "Guardar Figura",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Generar el nombre según la lógica requerida
                    string nombreNuevaFigura = GenerarNombreEscalacionRPF(nombreFiguraOriginal);

                    // Guardar la figura escalada
                    figurasGuardadas.Add(nombreNuevaFigura, new List<Point>(puntosActuales));
                    etiquetasFiguras.Add(nombreNuevaFigura, new List<string>(etiquetasPuntos));
                    coloresFiguras.Add(nombreNuevaFigura, colorOriginal); // Mantener el mismo color
                    listBoxHistorialFig.Items.Add(nombreNuevaFigura);

                    // Actualizar la figura seleccionada
                    figuraSeleccionadaActual = nombreNuevaFigura;
                }
            }
        }

        // Método para actualizar el listbox de coordenadas para escalación con punto fijo
        private void ActualizarListBoxCoordenadasEscalacionRPF(string operacion, string detallesFactor)
        {
            listBoxCoordenadas.Items.Clear();
            listBoxCoordenadas.Items.Add($"--- {figuraSeleccionadaActual} ({operacion}: {detallesFactor}) ---");

            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
            }
        }

        // Método para generar nombre de escalación con punto fijo según la lógica requerida
        private string GenerarNombreEscalacionRPF(string nombreOriginal)
        {
            // Extraer la base del nombre original (sin el último "_EscalacionRPFX")
            string nombreBase = nombreOriginal;
            int escalacionNumero = 1;

            // Si el nombre contiene "_EscalacionRPF" seguido de dígitos al final
            if (nombreOriginal.Contains("_EscalacionRPF"))
            {
                // Buscar la posición de la última ocurrencia de "_EscalacionRPF"
                int ultimoIndice = nombreOriginal.LastIndexOf("_EscalacionRPF");

                // Verificar si después de "_EscalacionRPF" hay un número
                if (ultimoIndice != -1 && ultimoIndice + 14 <= nombreOriginal.Length) // "_EscalacionRPF" tiene 14 caracteres
                {
                    string numStr = "";
                    int i = ultimoIndice + 14;

                    // Extraer todos los dígitos que siguen a "_EscalacionRPF"
                    while (i < nombreOriginal.Length && char.IsDigit(nombreOriginal[i]))
                    {
                        numStr += nombreOriginal[i];
                        i++;
                    }

                    // Buscar figuras del mismo nombre base para encontrar el siguiente número
                    string prefijo = nombreOriginal + "_EscalacionRPF";
                    escalacionNumero = 1; // Valor por defecto si no hay escalaciones previas

                    foreach (string nombreFigura in figurasGuardadas.Keys)
                    {
                        if (nombreFigura.StartsWith(prefijo))
                        {
                            // Extraer el número después del prefijo
                            string numParte = nombreFigura.Substring(prefijo.Length);
                            if (int.TryParse(numParte, out int num) && num >= escalacionNumero)
                            {
                                escalacionNumero = num + 1;
                            }
                        }
                    }

                    // Retornar el nombre original + "_EscalacionRPF" + siguiente número
                    return nombreOriginal + "_EscalacionRPF" + escalacionNumero;
                }
            }

            // Buscar figuras con el mismo nombre base para encontrar el siguiente número
            string prefijoBase = nombreOriginal + "_EscalacionRPF";
            escalacionNumero = 1; // Valor por defecto

            foreach (string nombreFigura in figurasGuardadas.Keys)
            {
                if (nombreFigura.StartsWith(prefijoBase))
                {
                    // Extraer el número después del prefijo
                    string numParte = nombreFigura.Substring(prefijoBase.Length);
                    if (int.TryParse(numParte, out int num) && num >= escalacionNumero)
                    {
                        escalacionNumero = num + 1;
                    }
                }
            }

            // Retornar el nombre base + "_EscalacionRPF" + siguiente número
            return nombreOriginal + "_EscalacionRPF" + escalacionNumero;
        }





        private void btnRotacionRO_Click(object sender, EventArgs e)
        {
            // Verificar si hay una figura seleccionada
            if (string.IsNullOrEmpty(figuraSeleccionadaActual) || puntosActuales.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una figura del historial primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Crear un formulario para la entrada del ángulo de rotación
            Form inputForm = new Form()
            {
                Width = 600,
                Height = 280,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Rotación de Figura con Respecto al Origen",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Crear etiquetas y textbox para el ángulo de rotación
            Label lblAngulo = new Label() { Left = 30, Top = 30, Text = "Ángulo de rotación:", Width = 120 };
            TextBox txtAngulo = new TextBox() { Left = 150, Top = 30, Width = 300 };

            // Crear etiquetas con información sobre el sentido de rotación
            Label lblInfo = new Label()
            {
                Left = 150,
                Top = 55,
                Width = 400,
                Text = "(+ Sentido contrario a las manecillas, - Sentido horario)"
            };

            // Botones para aceptar y cancelar
            Button btnAceptar = new Button() { Text = "Aceptar", Left = 260, Width = 100, Top = 180, DialogResult = DialogResult.OK };
            Button btnCancelar = new Button() { Text = "Cancelar", Left = 370, Width = 100, Top = 180, DialogResult = DialogResult.Cancel };

            btnAceptar.Click += (sender, e) => { inputForm.DialogResult = DialogResult.OK; };
            btnCancelar.Click += (sender, e) => { inputForm.DialogResult = DialogResult.Cancel; };

            // Agregar controles al formulario
            inputForm.Controls.Add(lblAngulo);
            inputForm.Controls.Add(txtAngulo);
            inputForm.Controls.Add(lblInfo);
            inputForm.Controls.Add(btnAceptar);
            inputForm.Controls.Add(btnCancelar);

            // Mostrar el formulario como diálogo
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                // Validar entrada - aceptar números decimales y enteros
                double angulo;
                bool anguloValido = double.TryParse(txtAngulo.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out angulo);

                if (!anguloValido)
                {
                    MessageBox.Show("Por favor ingrese un valor numérico válido para el ángulo.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Convertir el ángulo a radianes
                double anguloRadianes = angulo * Math.PI / 180.0;

                // Guardar los puntos y etiquetas originales
                List<Point> puntosOriginales = new List<Point>(puntosActuales);
                List<string> etiquetasOriginales = new List<string>(etiquetasPuntos);
                string nombreFiguraOriginal = figuraSeleccionadaActual;
                Color colorOriginal = coloresFiguras.ContainsKey(figuraSeleccionadaActual) ?
                                     coloresFiguras[figuraSeleccionadaActual] : lapizFigura.Color;

                // Crear matriz con puntos originales para la rotación
                double[,] matrizPuntos = new double[puntosActuales.Count, 3];

                // Llenar la matriz con coordenadas convertidas a plano cartesiano
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    double pX = (double)(puntosActuales[i].X - origenX) / escala;
                    double pY = (double)(origenY - puntosActuales[i].Y) / escala;
                    matrizPuntos[i, 0] = pX;
                    matrizPuntos[i, 1] = pY;
                    matrizPuntos[i, 2] = 1; // Coordenadas homogéneas
                }

                // Crear la matriz de rotación
                double cosAngulo = Math.Cos(anguloRadianes);
                double sinAngulo = Math.Sin(anguloRadianes);
                double[,] matrizRotacion = new double[3, 3] {
                    { cosAngulo, sinAngulo, 0 },
                    { -sinAngulo, cosAngulo, 0 },
                    { 0, 0, 1 }
                };

                // Crear una lista temporal para los nuevos puntos rotados
                List<Point> nuevosPuntos = new List<Point>();

                // Aplicar la rotación a cada punto
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    // Aplicar la rotación mediante multiplicación de matrices
                    double[] resultado = MultiplicarMatrizPunto(matrizPuntos[i, 0], matrizPuntos[i, 1], matrizRotacion);

                    // Convertir de coordenadas cartesianas a coordenadas de pantalla
                    int nuevaX = (int)Math.Round(origenX + resultado[0] * escala);
                    int nuevaY = (int)Math.Round(origenY - resultado[1] * escala);

                    nuevosPuntos.Add(new Point(nuevaX, nuevaY));
                }

                // Actualizar los puntos actuales con los nuevos puntos rotados
                puntosActuales.Clear();
                puntosActuales.AddRange(nuevosPuntos);

                // Redibujar el plano con los puntos rotados
                DibujarPlanoCartesiano();

                // Actualizar la visualización de coordenadas
                ActualizarListBoxCoordenadasRotacionRO("Rotación RO", angulo, 0);

                // Preguntar si se quieren guardar las figuras
                DialogResult result = MessageBox.Show(
                    "¿Desea guardar la figura rotada?",
                    "Guardar Figura",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Generar el nombre según la lógica requerida
                    string nombreNuevaFigura = GenerarNombreRotacionRO(nombreFiguraOriginal);

                    // Guardar la figura rotada
                    figurasGuardadas.Add(nombreNuevaFigura, new List<Point>(puntosActuales));
                    etiquetasFiguras.Add(nombreNuevaFigura, new List<string>(etiquetasPuntos));
                    coloresFiguras.Add(nombreNuevaFigura, colorOriginal); // Mantener el mismo color
                    listBoxHistorialFig.Items.Add(nombreNuevaFigura);

                    // Actualizar la figura seleccionada
                    figuraSeleccionadaActual = nombreNuevaFigura;
                }
            }
        }

        // Método para generar nombre de rotación respecto al origen según la lógica requerida
        private string GenerarNombreRotacionRO(string nombreOriginal)
        {
            // Extraer la base del nombre original (sin el último "_RotacionROX")
            string nombreBase = nombreOriginal;
            int rotacionNumero = 1;

            // Si el nombre contiene "_RotacionRO" seguido de dígitos al final
            if (nombreOriginal.Contains("_RotacionRO"))
            {
                // Buscar la posición de la última ocurrencia de "_RotacionRO"
                int ultimoIndice = nombreOriginal.LastIndexOf("_RotacionRO");

                // Verificar si después de "_RotacionRO" hay un número
                if (ultimoIndice != -1 && ultimoIndice + 11 <= nombreOriginal.Length) // "_RotacionRO" tiene 11 caracteres
                {
                    string numStr = "";
                    int i = ultimoIndice + 11;

                    // Extraer todos los dígitos que siguen a "_RotacionRO"
                    while (i < nombreOriginal.Length && char.IsDigit(nombreOriginal[i]))
                    {
                        numStr += nombreOriginal[i];
                        i++;
                    }

                    // Buscar figuras del mismo nombre base para encontrar el siguiente número
                    string prefijo = nombreOriginal + "_RotacionRO";
                    rotacionNumero = 1; // Valor por defecto si no hay rotaciones previas

                    foreach (string nombreFigura in figurasGuardadas.Keys)
                    {
                        if (nombreFigura.StartsWith(prefijo))
                        {
                            // Extraer el número después del prefijo
                            string numParte = nombreFigura.Substring(prefijo.Length);
                            if (int.TryParse(numParte, out int num) && num >= rotacionNumero)
                            {
                                rotacionNumero = num + 1;
                            }
                        }
                    }

                    // Retornar el nombre original + "_RotacionRO" + siguiente número
                    return nombreOriginal + "_RotacionRO" + rotacionNumero;
                }
            }

            // Buscar figuras con el mismo nombre base para encontrar el siguiente número
            string prefijoBase = nombreOriginal + "_RotacionRO";
            rotacionNumero = 1; // Valor por defecto

            foreach (string nombreFigura in figurasGuardadas.Keys)
            {
                if (nombreFigura.StartsWith(prefijoBase))
                {
                    // Extraer el número después del prefijo
                    string numParte = nombreFigura.Substring(prefijoBase.Length);
                    if (int.TryParse(numParte, out int num) && num >= rotacionNumero)
                    {
                        rotacionNumero = num + 1;
                    }
                }
            }

            // Retornar el nombre base + "_RotacionRO" + siguiente número
            return nombreOriginal + "_RotacionRO" + rotacionNumero;
        }

        // Si aún no tienes este método, también debes agregarlo para actualizar las coordenadas
        // con la información de la rotación
        private void ActualizarListBoxCoordenadasRotacionRO(string operacion, double valor1, double valor2)
        {
            listBoxCoordenadas.Items.Clear();

            // Adaptar según el tipo de operación
            if (operacion == "Rotación RO")
            {
                listBoxCoordenadas.Items.Add($"--- {figuraSeleccionadaActual} ({operacion}: {valor1}°) ---");
            }
            else
            {
                listBoxCoordenadas.Items.Add($"--- {figuraSeleccionadaActual} ({operacion}: TX={valor1}, TY={valor2}) ---");
            }

            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
            }
        }


        private void btnRotacionRPF_Click(object sender, EventArgs e)
        {
            // Verificar si hay una figura seleccionada
            if (string.IsNullOrEmpty(figuraSeleccionadaActual) || puntosActuales.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una figura del historial primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Crear un formulario para la entrada de datos
            Form inputForm = new Form()
            {
                Width = 550,
                Height = 440,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Rotación con Respecto a un Punto Fijo",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Crear opciones para dirección de rotación (RadioButtons)
            GroupBox grpSentidoRotacion = new GroupBox()
            {
                Text = "Sentido de Rotación",
                Left = 30,
                Top = 20,
                Width = 480,
                Height = 70
            };

            RadioButton radSentidoAntihorario = new RadioButton()
            {
                Text = "Antihorario (Positivo)",
                Left = 20,
                Top = 30,
                Width = 180,
                Checked = true
            };

            RadioButton radSentidoHorario = new RadioButton()
            {
                Text = "Horario (Negativo)",
                Left = 220,
                Top = 30,
                Width = 180
            };

            // Agregar los radio buttons al grupo
            grpSentidoRotacion.Controls.Add(radSentidoAntihorario);
            grpSentidoRotacion.Controls.Add(radSentidoHorario);

            // Crear etiquetas y textbox para el ángulo de rotación
            Label lblAngulo = new Label() { Left = 30, Top = 110, Text = "Ángulo (grados):", Width = 120 };
            TextBox txtAngulo = new TextBox() { Left = 150, Top = 110, Width = 300 };

            // Crear panel para selección del punto fijo
            GroupBox grpPuntoFijo = new GroupBox()
            {
                Text = "Punto Fijo de Rotación",
                Left = 30,
                Top = 150,
                Width = 480,
                Height = 180
            };

            // Opción 1: Seleccionar un punto de la figura
            RadioButton radUsarPuntoExistente = new RadioButton()
            {
                Text = "Usar un punto existente de la figura",
                Left = 20,
                Top = 30,
                Width = 300,
                Checked = true
            };

            Label lblPuntoExistente = new Label() { Left = 40, Top = 60, Text = "Seleccione punto:", Width = 120 };
            ComboBox cmbPuntosExistentes = new ComboBox()
            {
                Left = 160,
                Top = 60,
                Width = 280,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Llenar el combo box con los puntos de la figura
            for (int i = 0; i < etiquetasPuntos.Count; i++)
            {
                double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                cmbPuntosExistentes.Items.Add($"{etiquetasPuntos[i]} ({pX}, {pY})");
            }

            if (cmbPuntosExistentes.Items.Count > 0)
            {
                cmbPuntosExistentes.SelectedIndex = 0;
            }

            // Opción 2: Ingresar coordenadas manualmente
            RadioButton radIngresarPunto = new RadioButton()
            {
                Text = "Ingresar coordenadas manualmente",
                Left = 20,
                Top = 95,
                Width = 300
            };

            Label lblPuntoX = new Label() { Left = 40, Top = 125, Text = "X:", Width = 30 };
            TextBox txtPuntoX = new TextBox() { Left = 70, Top = 125, Width = 100, Enabled = false };

            Label lblPuntoY = new Label() { Left = 220, Top = 125, Text = "Y:", Width = 30 };
            TextBox txtPuntoY = new TextBox() { Left = 250, Top = 125, Width = 100, Enabled = false };

            // Eventos para habilitar/deshabilitar controles según la opción seleccionada
            radUsarPuntoExistente.CheckedChanged += (s, ev) => {
                cmbPuntosExistentes.Enabled = radUsarPuntoExistente.Checked;
                txtPuntoX.Enabled = !radUsarPuntoExistente.Checked;
                txtPuntoY.Enabled = !radUsarPuntoExistente.Checked;
            };

            radIngresarPunto.CheckedChanged += (s, ev) => {
                cmbPuntosExistentes.Enabled = !radIngresarPunto.Checked;
                txtPuntoX.Enabled = radIngresarPunto.Checked;
                txtPuntoY.Enabled = radIngresarPunto.Checked;
            };

            // Agregar controles al grupo
            grpPuntoFijo.Controls.Add(radUsarPuntoExistente);
            grpPuntoFijo.Controls.Add(lblPuntoExistente);
            grpPuntoFijo.Controls.Add(cmbPuntosExistentes);
            grpPuntoFijo.Controls.Add(radIngresarPunto);
            grpPuntoFijo.Controls.Add(lblPuntoX);
            grpPuntoFijo.Controls.Add(txtPuntoX);
            grpPuntoFijo.Controls.Add(lblPuntoY);
            grpPuntoFijo.Controls.Add(txtPuntoY);

            // Botones para aceptar y cancelar
            Button btnAceptar = new Button() { Text = "Aceptar", Left = 260, Width = 100, Top = 350, DialogResult = DialogResult.OK };
            Button btnCancelar = new Button() { Text = "Cancelar", Left = 370, Width = 100, Top = 350, DialogResult = DialogResult.Cancel };

            btnAceptar.Click += (s, ev) => { inputForm.DialogResult = DialogResult.OK; };
            btnCancelar.Click += (s, ev) => { inputForm.DialogResult = DialogResult.Cancel; };

            // Agregar controles al formulario
            inputForm.Controls.Add(grpSentidoRotacion);
            inputForm.Controls.Add(lblAngulo);
            inputForm.Controls.Add(txtAngulo);
            inputForm.Controls.Add(grpPuntoFijo);
            inputForm.Controls.Add(btnAceptar);
            inputForm.Controls.Add(btnCancelar);

            // Mostrar el formulario como diálogo
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                // Validar entrada del ángulo - aceptar números decimales y enteros
                double angulo;
                bool anguloValido = double.TryParse(txtAngulo.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out angulo);

                if (!anguloValido)
                {
                    MessageBox.Show("Por favor ingrese un valor numérico válido para el ángulo.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Determinar el sentido de rotación (signo del ángulo)
                if (radSentidoHorario.Checked)
                {
                    angulo = -angulo; // Hacer el ángulo negativo para rotación en sentido horario
                }

                // Obtener el punto fijo de rotación
                double puntoFijoX, puntoFijoY;

                if (radUsarPuntoExistente.Checked)
                {
                    if (cmbPuntosExistentes.SelectedIndex == -1)
                    {
                        MessageBox.Show("Por favor seleccione un punto de la figura.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int indicePunto = cmbPuntosExistentes.SelectedIndex;
                    puntoFijoX = (double)(puntosActuales[indicePunto].X - origenX) / escala;
                    puntoFijoY = (double)(origenY - puntosActuales[indicePunto].Y) / escala;
                }
                else // radIngresarPunto.Checked
                {
                    bool xValido = double.TryParse(txtPuntoX.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out puntoFijoX);
                    bool yValido = double.TryParse(txtPuntoY.Text.Replace(',', '.'), System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out puntoFijoY);

                    if (!xValido || !yValido)
                    {
                        MessageBox.Show("Por favor ingrese valores numéricos válidos para las coordenadas del punto fijo.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Guardar los puntos y etiquetas originales
                List<Point> puntosOriginales = new List<Point>(puntosActuales);
                List<string> etiquetasOriginales = new List<string>(etiquetasPuntos);
                string nombreFiguraOriginal = figuraSeleccionadaActual;
                Color colorOriginal = coloresFiguras.ContainsKey(figuraSeleccionadaActual) ?
                                     coloresFiguras[figuraSeleccionadaActual] : lapizFigura.Color;

                // Crear matriz con puntos originales para la rotación
                double[,] matrizPuntos = new double[puntosActuales.Count, 3];

                // Llenar la matriz con coordenadas convertidas a plano cartesiano
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    double pX = (double)(puntosActuales[i].X - origenX) / escala;
                    double pY = (double)(origenY - puntosActuales[i].Y) / escala;
                    matrizPuntos[i, 0] = pX;
                    matrizPuntos[i, 1] = pY;
                    matrizPuntos[i, 2] = 1; // Coordenadas homogéneas
                }

                // Convertir ángulo a radianes
                double anguloRadianes = angulo * Math.PI / 180.0;
                double coseno = Math.Cos(anguloRadianes);
                double seno = Math.Sin(anguloRadianes);

                // Crear la matriz de rotación respecto a un punto fijo
                double[,] matrizRotacion = new double[3, 3] {
                    { coseno, seno, 0 },
                    { -seno, coseno, 0 },
                    { puntoFijoX*(1-coseno)+puntoFijoY*seno, puntoFijoY*(1-coseno)-puntoFijoX*seno, 1 }
                };

                // Crear una lista temporal para los nuevos puntos rotados
                List<Point> nuevosPuntos = new List<Point>();

                // Aplicar la rotación a cada punto
                for (int i = 0; i < puntosActuales.Count; i++)
                {
                    // Aplicar la rotación mediante multiplicación de matrices
                    double[] resultado = MultiplicarMatrizPunto(matrizPuntos[i, 0], matrizPuntos[i, 1], matrizRotacion);

                    // Convertir de coordenadas cartesianas a coordenadas de pantalla
                    int nuevaX = (int)Math.Round(origenX + resultado[0] * escala);
                    int nuevaY = (int)Math.Round(origenY - resultado[1] * escala);

                    nuevosPuntos.Add(new Point(nuevaX, nuevaY));
                }

                // Actualizar los puntos actuales con los nuevos puntos rotados
                puntosActuales.Clear();
                puntosActuales.AddRange(nuevosPuntos);

                // Redibujar el plano con los puntos rotados
                DibujarPlanoCartesiano();

                // Actualizar la visualización de coordenadas
                string operacion = radSentidoAntihorario.Checked ? "Antihorario" : "Horario";
                ActualizarListBoxCoordenadasRotacion($"Rotación RPF ({operacion})",
                    $"Ángulo: {Math.Abs(angulo)}°, Punto fijo: ({puntoFijoX:F2}, {puntoFijoY:F2})");

                // Preguntar si se quieren guardar las figuras
                DialogResult result = MessageBox.Show(
                    "¿Desea guardar la figura rotada?",
                    "Guardar Figura",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Generar el nombre según la lógica requerida
                    string nombreNuevaFigura = GenerarNombreRotacionRPF(nombreFiguraOriginal);

                    // Guardar la figura rotada
                    figurasGuardadas.Add(nombreNuevaFigura, new List<Point>(puntosActuales));
                    etiquetasFiguras.Add(nombreNuevaFigura, new List<string>(etiquetasPuntos));
                    coloresFiguras.Add(nombreNuevaFigura, colorOriginal); // Mantener el mismo color
                    listBoxHistorialFig.Items.Add(nombreNuevaFigura);

                    // Actualizar la figura seleccionada
                    figuraSeleccionadaActual = nombreNuevaFigura;
                }
            }
        }

        // Método para actualizar el listbox de coordenadas para rotación
        private void ActualizarListBoxCoordenadasRotacion(string operacion, string detallesRotacion)
        {
            listBoxCoordenadas.Items.Clear();
            listBoxCoordenadas.Items.Add($"--- {figuraSeleccionadaActual} ({operacion}: {detallesRotacion}) ---");

            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
            }
        }

        // Método para generar nombre de rotación según la lógica requerida
        private string GenerarNombreRotacionRPF(string nombreOriginal)
        {
            // Extraer la base del nombre original (sin el último "_RotacionRPFX")
            string nombreBase = nombreOriginal;
            int rotacionNumero = 1;

            // Si el nombre contiene "_RotacionRPF" seguido de dígitos al final
            if (nombreOriginal.Contains("_RotacionRPF"))
            {
                // Buscar la posición de la última ocurrencia de "_RotacionRPF"
                int ultimoIndice = nombreOriginal.LastIndexOf("_RotacionRPF");

                // Verificar si después de "_RotacionRPF" hay un número
                if (ultimoIndice != -1 && ultimoIndice + 12 <= nombreOriginal.Length) // "_RotacionRPF" tiene 12 caracteres
                {
                    string numStr = "";
                    int i = ultimoIndice + 12;

                    // Extraer todos los dígitos que siguen a "_RotacionRPF"
                    while (i < nombreOriginal.Length && char.IsDigit(nombreOriginal[i]))
                    {
                        numStr += nombreOriginal[i];
                        i++;
                    }

                    // Buscar figuras del mismo nombre base para encontrar el siguiente número
                    string prefijo = nombreOriginal + "_RotacionRPF";
                    rotacionNumero = 1; // Valor por defecto si no hay rotaciones previas

                    foreach (string nombreFigura in figurasGuardadas.Keys)
                    {
                        if (nombreFigura.StartsWith(prefijo))
                        {
                            // Extraer el número después del prefijo
                            string numParte = nombreFigura.Substring(prefijo.Length);
                            if (int.TryParse(numParte, out int num) && num >= rotacionNumero)
                            {
                                rotacionNumero = num + 1;
                            }
                        }
                    }

                    // Retornar el nombre original + "_RotacionRPF" + siguiente número
                    return nombreOriginal + "_RotacionRPF" + rotacionNumero;
                }
            }

            // Buscar figuras con el mismo nombre base para encontrar el siguiente número
            string prefijoBase = nombreOriginal + "_RotacionRPF";
            rotacionNumero = 1; // Valor por defecto

            foreach (string nombreFigura in figurasGuardadas.Keys)
            {
                if (nombreFigura.StartsWith(prefijoBase))
                {
                    // Extraer el número después del prefijo
                    string numParte = nombreFigura.Substring(prefijoBase.Length);
                    if (int.TryParse(numParte, out int num) && num >= rotacionNumero)
                    {
                        rotacionNumero = num + 1;
                    }
                }
            }

            // Retornar el nombre base + "_RotacionRPF" + siguiente número
            return nombreOriginal + "_RotacionRPF" + rotacionNumero;
        }




        private void btnReflexionRO_Click(object sender, EventArgs e)
        {
            // Verificar si hay una figura seleccionada
            if (string.IsNullOrEmpty(figuraSeleccionadaActual) || puntosActuales.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una figura del historial primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Guardar los puntos y etiquetas originales
            List<Point> puntosOriginales = new List<Point>(puntosActuales);
            List<string> etiquetasOriginales = new List<string>(etiquetasPuntos);
            string nombreFiguraOriginal = figuraSeleccionadaActual;
            Color colorOriginal = coloresFiguras.ContainsKey(figuraSeleccionadaActual) ?
                                 coloresFiguras[figuraSeleccionadaActual] : lapizFigura.Color;

            // Crear matriz con puntos originales para la reflexión
            double[,] matrizPuntos = new double[puntosActuales.Count, 3];

            // Llenar la matriz con coordenadas convertidas a plano cartesiano
            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = (double)(puntosActuales[i].X - origenX) / escala;
                double pY = (double)(origenY - puntosActuales[i].Y) / escala;
                matrizPuntos[i, 0] = pX;
                matrizPuntos[i, 1] = pY;
                matrizPuntos[i, 2] = 1; // Coordenadas homogéneas
            }

            // Crear la matriz de reflexión respecto al origen
            double[,] matrizReflexion = new double[3, 3] {
                { -1, 0, 0 },
                { 0, -1, 0 },
                { 0, 0, 1 }
            };

            // Crear una lista temporal para los nuevos puntos reflejados
            List<Point> nuevosPuntos = new List<Point>();

            // Aplicar la reflexión a cada punto
            for (int i = 0; i < puntosActuales.Count; i++)
            {
                // Aplicar la reflexión mediante multiplicación de matrices
                double[] resultado = MultiplicarMatrizPunto(matrizPuntos[i, 0], matrizPuntos[i, 1], matrizReflexion);

                // Convertir de coordenadas cartesianas a coordenadas de pantalla
                int nuevaX = (int)Math.Round(origenX + resultado[0] * escala);
                int nuevaY = (int)Math.Round(origenY - resultado[1] * escala);

                nuevosPuntos.Add(new Point(nuevaX, nuevaY));
            }

            // Actualizar los puntos actuales con los nuevos puntos reflejados
            puntosActuales.Clear();
            puntosActuales.AddRange(nuevosPuntos);

            // Redibujar el plano con los puntos reflejados
            DibujarPlanoCartesiano();

            // Actualizar la visualización de coordenadas
            ActualizarListBoxCoordenadasReflexion("Reflexión RO");

            // Preguntar si se quieren guardar las figuras
            DialogResult result = MessageBox.Show(
                "¿Desea guardar la figura reflejada?",
                "Guardar Figura",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Generar el nombre según la lógica requerida
                string nombreNuevaFigura = GenerarNombreReflexionRO(nombreFiguraOriginal);

                // Guardar la figura reflejada
                figurasGuardadas.Add(nombreNuevaFigura, new List<Point>(puntosActuales));
                etiquetasFiguras.Add(nombreNuevaFigura, new List<string>(etiquetasPuntos));
                coloresFiguras.Add(nombreNuevaFigura, colorOriginal); // Mantener el mismo color
                listBoxHistorialFig.Items.Add(nombreNuevaFigura);

                // Actualizar la figura seleccionada
                figuraSeleccionadaActual = nombreNuevaFigura;
            }
        }

        // Método para actualizar el listbox de coordenadas para reflexión
        private void ActualizarListBoxCoordenadasReflexion(string operacion)
        {
            listBoxCoordenadas.Items.Clear();
            listBoxCoordenadas.Items.Add($"--- {figuraSeleccionadaActual} ({operacion}) ---");

            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
            }
        }

        // Método para generar nombre de reflexión respecto al origen según la lógica requerida
        private string GenerarNombreReflexionRO(string nombreOriginal)
        {
            // Extraer la base del nombre original (sin el último "_ReflexionROX")
            string nombreBase = nombreOriginal;
            int reflexionNumero = 1;

            // Si el nombre contiene "_ReflexionRO" seguido de dígitos al final
            if (nombreOriginal.Contains("_ReflexionRO"))
            {
                // Buscar la posición de la última ocurrencia de "_ReflexionRO"
                int ultimoIndice = nombreOriginal.LastIndexOf("_ReflexionRO");

                // Verificar si después de "_ReflexionRO" hay un número
                if (ultimoIndice != -1 && ultimoIndice + 12 <= nombreOriginal.Length) // "_ReflexionRO" tiene 12 caracteres
                {
                    string numStr = "";
                    int i = ultimoIndice + 12;

                    // Extraer todos los dígitos que siguen a "_ReflexionRO"
                    while (i < nombreOriginal.Length && char.IsDigit(nombreOriginal[i]))
                    {
                        numStr += nombreOriginal[i];
                        i++;
                    }

                    // Buscar figuras del mismo nombre base para encontrar el siguiente número
                    string prefijo = nombreOriginal + "_ReflexionRO";
                    reflexionNumero = 1; // Valor por defecto si no hay reflexiones previas

                    foreach (string nombreFigura in figurasGuardadas.Keys)
                    {
                        if (nombreFigura.StartsWith(prefijo))
                        {
                            // Extraer el número después del prefijo
                            string numParte = nombreFigura.Substring(prefijo.Length);
                            if (int.TryParse(numParte, out int num) && num >= reflexionNumero)
                            {
                                reflexionNumero = num + 1;
                            }
                        }
                    }

                    // Retornar el nombre original + "_ReflexionRO" + siguiente número
                    return nombreOriginal + "_ReflexionRO" + reflexionNumero;
                }
            }

            // Buscar figuras con el mismo nombre base para encontrar el siguiente número
            string prefijoBase = nombreOriginal + "_ReflexionRO";
            reflexionNumero = 1; // Valor por defecto

            foreach (string nombreFigura in figurasGuardadas.Keys)
            {
                if (nombreFigura.StartsWith(prefijoBase))
                {
                    // Extraer el número después del prefijo
                    string numParte = nombreFigura.Substring(prefijoBase.Length);
                    if (int.TryParse(numParte, out int num) && num >= reflexionNumero)
                    {
                        reflexionNumero = num + 1;
                    }
                }
            }

            // Retornar el nombre base + "_ReflexionRO" + siguiente número
            return nombreOriginal + "_ReflexionRO" + reflexionNumero;
        }




        private void btnReflexionREjeX_Click(object sender, EventArgs e)
        {
            // Verificar si hay una figura seleccionada
            if (string.IsNullOrEmpty(figuraSeleccionadaActual) || puntosActuales.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una figura del historial primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Guardar los puntos y etiquetas originales
            List<Point> puntosOriginales = new List<Point>(puntosActuales);
            List<string> etiquetasOriginales = new List<string>(etiquetasPuntos);
            string nombreFiguraOriginal = figuraSeleccionadaActual;
            Color colorOriginal = coloresFiguras.ContainsKey(figuraSeleccionadaActual) ?
                                 coloresFiguras[figuraSeleccionadaActual] : lapizFigura.Color;

            // Crear matriz con puntos originales para la reflexión
            double[,] matrizPuntos = new double[puntosActuales.Count, 3];

            // Llenar la matriz con coordenadas convertidas a plano cartesiano
            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = (double)(puntosActuales[i].X - origenX) / escala;
                double pY = (double)(origenY - puntosActuales[i].Y) / escala;
                matrizPuntos[i, 0] = pX;
                matrizPuntos[i, 1] = pY;
                matrizPuntos[i, 2] = 1; // Coordenadas homogéneas
            }

            // Crear la matriz de reflexión respecto al eje X
            double[,] matrizReflexion = new double[3, 3] {
                { 1, 0, 0 },
                { 0, -1, 0 },
                { 0, 0, 1 }
            };

            // Crear una lista temporal para los nuevos puntos reflejados
            List<Point> nuevosPuntos = new List<Point>();

            // Aplicar la reflexión a cada punto
            for (int i = 0; i < puntosActuales.Count; i++)
            {
                // Aplicar la reflexión mediante multiplicación de matrices
                double[] resultado = MultiplicarMatrizPunto(matrizPuntos[i, 0], matrizPuntos[i, 1], matrizReflexion);

                // Convertir de coordenadas cartesianas a coordenadas de pantalla
                int nuevaX = (int)Math.Round(origenX + resultado[0] * escala);
                int nuevaY = (int)Math.Round(origenY - resultado[1] * escala);

                nuevosPuntos.Add(new Point(nuevaX, nuevaY));
            }

            // Actualizar los puntos actuales con los nuevos puntos reflejados
            puntosActuales.Clear();
            puntosActuales.AddRange(nuevosPuntos);

            // Redibujar el plano con los puntos reflejados
            DibujarPlanoCartesiano();

            // Actualizar la visualización de coordenadas
            ActualizarListBoxCoordenadasTransformacion("Reflexión Eje X");

            // Preguntar si se quieren guardar las figuras
            DialogResult result = MessageBox.Show(
                "¿Desea guardar la figura reflejada?",
                "Guardar Figura",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Generar el nombre según la lógica requerida
                string nombreNuevaFigura = GenerarNombreReflexionEjeX(nombreFiguraOriginal);

                // Guardar la figura reflejada
                figurasGuardadas.Add(nombreNuevaFigura, new List<Point>(puntosActuales));
                etiquetasFiguras.Add(nombreNuevaFigura, new List<string>(etiquetasPuntos));
                coloresFiguras.Add(nombreNuevaFigura, colorOriginal); // Mantener el mismo color
                listBoxHistorialFig.Items.Add(nombreNuevaFigura);

                // Actualizar la figura seleccionada
                figuraSeleccionadaActual = nombreNuevaFigura;
            }
        }

        // Método para actualizar el listbox de coordenadas para transformaciones
        private void ActualizarListBoxCoordenadasTransformacion(string operacion)
        {
            listBoxCoordenadas.Items.Clear();
            listBoxCoordenadas.Items.Add($"--- {figuraSeleccionadaActual} ({operacion}) ---");

            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
            }
        }

        // Método para generar nombre de reflexión según la lógica requerida
        private string GenerarNombreReflexionEjeX(string nombreOriginal)
        {
            // Extraer la base del nombre original (sin el último "_ReflexionEjeXX")
            string nombreBase = nombreOriginal;
            int reflexionNumero = 1;

            // Si el nombre contiene "_ReflexionEjeX" seguido de dígitos al final
            if (nombreOriginal.Contains("_ReflexionEjeX"))
            {
                // Buscar la posición de la última ocurrencia de "_ReflexionEjeX"
                int ultimoIndice = nombreOriginal.LastIndexOf("_ReflexionEjeX");

                // Verificar si después de "_ReflexionEjeX" hay un número
                if (ultimoIndice != -1 && ultimoIndice + 14 <= nombreOriginal.Length) // "_ReflexionEjeX" tiene 14 caracteres
                {
                    string numStr = "";
                    int i = ultimoIndice + 14;

                    // Extraer todos los dígitos que siguen a "_ReflexionEjeX"
                    while (i < nombreOriginal.Length && char.IsDigit(nombreOriginal[i]))
                    {
                        numStr += nombreOriginal[i];
                        i++;
                    }

                    // Buscar figuras del mismo nombre base para encontrar el siguiente número
                    string prefijo = nombreOriginal + "_ReflexionEjeX";
                    reflexionNumero = 1; // Valor por defecto si no hay reflexiones previas

                    foreach (string nombreFigura in figurasGuardadas.Keys)
                    {
                        if (nombreFigura.StartsWith(prefijo))
                        {
                            // Extraer el número después del prefijo
                            string numParte = nombreFigura.Substring(prefijo.Length);
                            if (int.TryParse(numParte, out int num) && num >= reflexionNumero)
                            {
                                reflexionNumero = num + 1;
                            }
                        }
                    }

                    // Retornar el nombre original + "_ReflexionEjeX" + siguiente número
                    return nombreOriginal + "_ReflexionEjeX" + reflexionNumero;
                }
            }

            // Buscar figuras con el mismo nombre base para encontrar el siguiente número
            string prefijoBase = nombreOriginal + "_ReflexionEjeX";
            reflexionNumero = 1; // Valor por defecto

            foreach (string nombreFigura in figurasGuardadas.Keys)
            {
                if (nombreFigura.StartsWith(prefijoBase))
                {
                    // Extraer el número después del prefijo
                    string numParte = nombreFigura.Substring(prefijoBase.Length);
                    if (int.TryParse(numParte, out int num) && num >= reflexionNumero)
                    {
                        reflexionNumero = num + 1;
                    }
                }
            }

            // Retornar el nombre base + "_ReflexionEjeX" + siguiente número
            return nombreOriginal + "_ReflexionEjeX" + reflexionNumero;
        }





        private void btnReflexionREjeY_Click(object sender, EventArgs e)
        {
            // Verificar si hay una figura seleccionada
            if (string.IsNullOrEmpty(figuraSeleccionadaActual) || puntosActuales.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una figura del historial primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Guardar los puntos y etiquetas originales
            List<Point> puntosOriginales = new List<Point>(puntosActuales);
            List<string> etiquetasOriginales = new List<string>(etiquetasPuntos);
            string nombreFiguraOriginal = figuraSeleccionadaActual;
            Color colorOriginal = coloresFiguras.ContainsKey(figuraSeleccionadaActual) ?
                                 coloresFiguras[figuraSeleccionadaActual] : lapizFigura.Color;

            // Crear matriz con puntos originales para la reflexión
            double[,] matrizPuntos = new double[puntosActuales.Count, 3];

            // Llenar la matriz con coordenadas convertidas a plano cartesiano
            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = (double)(puntosActuales[i].X - origenX) / escala;
                double pY = (double)(origenY - puntosActuales[i].Y) / escala;
                matrizPuntos[i, 0] = pX;
                matrizPuntos[i, 1] = pY;
                matrizPuntos[i, 2] = 1; // Coordenadas homogéneas
            }

            // Crear la matriz de reflexión respecto al eje Y
            double[,] matrizReflexion = new double[3, 3] {
        { -1, 0, 0 },
        { 0, 1, 0 },
        { 0, 0, 1 }
    };

            // Crear una lista temporal para los nuevos puntos reflejados
            List<Point> nuevosPuntos = new List<Point>();

            // Aplicar la reflexión a cada punto
            for (int i = 0; i < puntosActuales.Count; i++)
            {
                // Aplicar la reflexión mediante multiplicación de matrices
                double[] resultado = MultiplicarMatrizPunto(matrizPuntos[i, 0], matrizPuntos[i, 1], matrizReflexion);

                // Convertir de coordenadas cartesianas a coordenadas de pantalla
                int nuevaX = (int)Math.Round(origenX + resultado[0] * escala);
                int nuevaY = (int)Math.Round(origenY - resultado[1] * escala);

                nuevosPuntos.Add(new Point(nuevaX, nuevaY));
            }

            // Actualizar los puntos actuales con los nuevos puntos reflejados
            puntosActuales.Clear();
            puntosActuales.AddRange(nuevosPuntos);

            // Redibujar el plano con los puntos reflejados
            DibujarPlanoCartesiano();

            // Actualizar la visualización de coordenadas
            ActualizarListBoxCoordenadasReflexionEjeY("Reflexión Eje Y");

            // Preguntar si se quieren guardar las figuras
            DialogResult result = MessageBox.Show(
                "¿Desea guardar la figura reflejada?",
                "Guardar Figura",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Generar el nombre según la lógica requerida
                string nombreNuevaFigura = GenerarNombreReflexionEjeY(nombreFiguraOriginal);

                // Guardar la figura reflejada
                figurasGuardadas.Add(nombreNuevaFigura, new List<Point>(puntosActuales));
                etiquetasFiguras.Add(nombreNuevaFigura, new List<string>(etiquetasPuntos));
                coloresFiguras.Add(nombreNuevaFigura, colorOriginal); // Mantener el mismo color
                listBoxHistorialFig.Items.Add(nombreNuevaFigura);

                // Actualizar la figura seleccionada
                figuraSeleccionadaActual = nombreNuevaFigura;
            }
        }

        // Método para actualizar el listbox de coordenadas para reflexión
        private void ActualizarListBoxCoordenadasReflexionEjeY(string operacion)
        {
            listBoxCoordenadas.Items.Clear();
            listBoxCoordenadas.Items.Add($"--- {figuraSeleccionadaActual} ({operacion}) ---");

            for (int i = 0; i < puntosActuales.Count; i++)
            {
                double pX = Math.Round((double)(puntosActuales[i].X - origenX) / escala, 2);
                double pY = Math.Round((double)(origenY - puntosActuales[i].Y) / escala, 2);
                listBoxCoordenadas.Items.Add($"Punto {etiquetasPuntos[i]}: ({pX},{pY})");
            }
        }

        // Método para generar nombre de reflexión según la lógica requerida
        private string GenerarNombreReflexionEjeY(string nombreOriginal)
        {
            // Extraer la base del nombre original
            string nombreBase = nombreOriginal;
            int reflexionNumero = 1;

            // Si el nombre contiene "_ReflexionREjeY" seguido de dígitos al final
            if (nombreOriginal.Contains("_ReflexionREjeY"))
            {
                // Buscar la posición de la última ocurrencia de "_ReflexionREjeY"
                int ultimoIndice = nombreOriginal.LastIndexOf("_ReflexionREjeY");

                // Verificar si después de "_ReflexionREjeY" hay un número
                if (ultimoIndice != -1 && ultimoIndice + 15 <= nombreOriginal.Length) // "_ReflexionREjeY" tiene 15 caracteres
                {
                    string numStr = "";
                    int i = ultimoIndice + 15;

                    // Extraer todos los dígitos que siguen a "_ReflexionREjeY"
                    while (i < nombreOriginal.Length && char.IsDigit(nombreOriginal[i]))
                    {
                        numStr += nombreOriginal[i];
                        i++;
                    }

                    // Buscar figuras del mismo nombre base para encontrar el siguiente número
                    string prefijo = nombreOriginal + "_ReflexionREjeY";
                    reflexionNumero = 1; // Valor por defecto si no hay reflexiones previas

                    foreach (string nombreFigura in figurasGuardadas.Keys)
                    {
                        if (nombreFigura.StartsWith(prefijo))
                        {
                            // Extraer el número después del prefijo
                            string numParte = nombreFigura.Substring(prefijo.Length);
                            if (int.TryParse(numParte, out int num) && num >= reflexionNumero)
                            {
                                reflexionNumero = num + 1;
                            }
                        }
                    }

                    // Retornar el nombre original + "_ReflexionREjeY" + siguiente número
                    return nombreOriginal + "_ReflexionREjeY" + reflexionNumero;
                }
            }

            // Buscar figuras con el mismo nombre base para encontrar el siguiente número
            string prefijoBase = nombreOriginal + "_ReflexionREjeY";
            reflexionNumero = 1; // Valor por defecto

            foreach (string nombreFigura in figurasGuardadas.Keys)
            {
                if (nombreFigura.StartsWith(prefijoBase))
                {
                    // Extraer el número después del prefijo
                    string numParte = nombreFigura.Substring(prefijoBase.Length);
                    if (int.TryParse(numParte, out int num) && num >= reflexionNumero)
                    {
                        reflexionNumero = num + 1;
                    }
                }
            }

            // Retornar el nombre base + "_ReflexionREjeY" + siguiente número
            return nombreOriginal + "_ReflexionREjeY" + reflexionNumero;
        }






        private void BtnColorFigura_Click(object sender, EventArgs e)
        {
            // Si hay una figura seleccionada del historial
            if (!string.IsNullOrEmpty(figuraSeleccionadaActual))
            {
                // Crear un ColorDialog
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    // Configurar el diálogo de color
                    colorDialog.AllowFullOpen = true;
                    colorDialog.ShowHelp = true;

                    // Usar el color actual de la figura seleccionada
                    if (coloresFiguras.ContainsKey(figuraSeleccionadaActual))
                    {
                        colorDialog.Color = coloresFiguras[figuraSeleccionadaActual];
                    }
                    else
                    {
                        colorDialog.Color = lapizFigura.Color;
                    }

                    // Mostrar el diálogo de color
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Cambiar el color de la figura seleccionada
                        coloresFiguras[figuraSeleccionadaActual] = colorDialog.Color;

                        // Redibujar el plano con el nuevo color
                        DibujarPlanoCartesiano();
                    }
                }
            }
            // Si hay puntos actuales pero no es una figura guardada
            else if (puntosActuales.Count > 0)
            {
                // Crear un ColorDialog
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    // Configurar el diálogo de color
                    colorDialog.AllowFullOpen = true;
                    colorDialog.ShowHelp = true;
                    colorDialog.Color = lapizFigura.Color;

                    // Mostrar el diálogo de color
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Cambiar el color del lápiz para la figura actual
                        lapizFigura = new Pen(colorDialog.Color, 2);

                        // Redibujar el plano con el nuevo color
                        DibujarPlanoCartesiano();
                    }
                }
            }
            else
            {
                // Permitir cambiar el color de cualquier figura del historial si no hay ninguna seleccionada
                if (listBoxHistorialFig.Items.Count > 0)
                {
                    // Crear un formulario para seleccionar la figura
                    Form seleccionForm = new Form()
                    {
                        Width = 400,
                        Height = 300,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        Text = "Seleccionar Figura para Cambiar Color",
                        StartPosition = FormStartPosition.CenterParent,
                        MaximizeBox = false,
                        MinimizeBox = false
                    };

                    // Crear una lista de figuras guardadas
                    ListBox lstFiguras = new ListBox()
                    {
                        Left = 30,
                        Top = 30,
                        Width = 340,
                        Height = 180
                    };

                    foreach (string nombreFigura in listBoxHistorialFig.Items)
                    {
                        lstFiguras.Items.Add(nombreFigura);
                    }

                    // Botón para aceptar
                    Button btnAceptarSeleccion = new Button() { Text = "Aceptar", Left = 160, Width = 80, Top = 220, DialogResult = DialogResult.OK };
                    btnAceptarSeleccion.Click += (sender, e) => { seleccionForm.Close(); };

                    // Agregar controles al formulario
                    seleccionForm.Controls.Add(lstFiguras);
                    seleccionForm.Controls.Add(btnAceptarSeleccion);

                    // Mostrar el formulario como diálogo
                    if (seleccionForm.ShowDialog() == DialogResult.OK && lstFiguras.SelectedIndex != -1)
                    {
                        string figuraSeleccionada = lstFiguras.SelectedItem.ToString();

                        // Crear un ColorDialog
                        using (ColorDialog colorDialog = new ColorDialog())
                        {
                            // Configurar el diálogo de color
                            colorDialog.AllowFullOpen = true;
                            colorDialog.ShowHelp = true;

                            // Usar el color actual de la figura seleccionada
                            if (coloresFiguras.ContainsKey(figuraSeleccionada))
                            {
                                colorDialog.Color = coloresFiguras[figuraSeleccionada];
                            }
                            else
                            {
                                colorDialog.Color = lapizFigura.Color;
                            }

                            // Mostrar el diálogo de color
                            if (colorDialog.ShowDialog() == DialogResult.OK)
                            {
                                // Cambiar el color de la figura seleccionada
                                coloresFiguras[figuraSeleccionada] = colorDialog.Color;

                                // Si la figura seleccionada está actualmente en el plano, actualizar
                                if (figuraSeleccionada == figuraSeleccionadaActual)
                                {
                                    DibujarPlanoCartesiano();
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No hay figuras para cambiar el color. Dibuje puntos o guarde figuras primero.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}