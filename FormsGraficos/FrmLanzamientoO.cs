using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsGraficos
{
    public partial class FrmLanzamientoO : Form
    {
        private double x, y;
        private double velocidadX, velocidadY;
        private double gravedad = 9.81;
        private double radio = 10;
        private System.Windows.Forms.Timer animationTimer;
        private bool mostrarTrayectoria = true;
        private List<Point> puntosTrayectoria = new List<Point>();
        private List<DataPoint> datosMovimiento = new List<DataPoint>();
        private bool enColision = false;
        private double escalaX = 1.0, escalaY = 1.0;
        private int contadorDeformacion = 0;
        private int maxDeformacion = 10;
        private int nivelSuelo;

        private int numeroBotes = 0;
        private double alcance = 0;
        private double alturaMaxima = 0;
        private double distanciaReal = 0;
        private double tiempoTranscurrido = 0;
        private double alturaInicial = 0;
        private double velocidadInicial = 0;
        private double factorTiempo = 1.0; // Factor para acelerar simulaciones lentas
        private double ultimoRegistroTiempo = 0; // Para controlar la frecuencia de registro

        // Factor de escala para la representación visual
        private double factorEscalaX = 1.0;
        private double factorEscalaY = 1.0;

        // Controles que agregaremos dinámicamente
        private TextBox txtAltura;
        private TextBox txtVelocidad;
        private TextBox txtAngulo;
        private Button btnIniciar;
        private Label lblAltura;
        private Label lblVelocidad;
        private Label lblAngulo;
        private GroupBox groupBoxControles;
        private GroupBox groupBoxDatos;
        private DataGridView dgvDatos;
        private Panel panelGrafico;

        // Estructura para almacenar datos de movimiento
        private struct DataPoint
        {
            public double Tiempo { get; set; }
            public double PosicionX { get; set; }
            public double PosicionY { get; set; }
            public double VelocidadX { get; set; }
            public double VelocidadY { get; set; }
            public double Altura { get; set; }
            public double Distancia { get; set; }
        }

        public FrmLanzamientoO()
        {
            InitializeComponent();

            // Configurar propiedades del formulario
            ConfigurarFormulario();

            // Configurar panel para el gráfico
            ConfigurarPanelGrafico();

            // Configurar la superficie de dibujo
            ConfigurarPictureBox();

            // Agregar controles de usuario
            AgregarControles();

            // Configurar tabla de datos
            ConfigurarTablaDatos();

            // Configurar timer para la animación
            ConfigurarTimer();
        }
        private void FrmLanzamientoO_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ConfigurarFormulario()
        {
            this.Width = 1200;
            this.Height = 750;
            this.Text = "Simulador de Lanzamiento Horizontal";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 700);
        }

        private void ConfigurarPanelGrafico()
        {
            panelGrafico = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(this.ClientSize.Width - 420, this.ClientSize.Height - 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelGrafico);
        }

        private void ConfigurarPictureBox()
        {
            // Configuramos el pictureBox que ya existe en el diseñador
            pictureBox1.BackColor = Color.White;
            pictureBox1.Paint += DibujarEnPictureBox;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Dock = DockStyle.Fill;
            panelGrafico.Controls.Add(pictureBox1);
            pictureBox1.SizeChanged += PictureBox1_SizeChanged;
        }

        private void PictureBox1_SizeChanged(object sender, EventArgs e)
        {
            // Recalcular el nivel del suelo y factores de escala cuando cambia el tamaño
            nivelSuelo = pictureBox1.Height - 50;

            // Recalcular la escala para adaptarse al nuevo tamaño
            AjustarFactoresEscala();

            pictureBox1.Invalidate();
        }

        private void AjustarFactoresEscala()
        {
            // Ajustar factores de escala según el tamaño del pictureBox
            factorEscalaX = (pictureBox1.Width - 100) / 500.0; // 500 es la distancia de referencia en metros
            factorEscalaY = (pictureBox1.Height - 100) / 200.0; // 200 es la altura de referencia en metros
        }

        private void AgregarControles()
        {
            // Crear grupo de controles
            groupBoxControles = new GroupBox
            {
                Text = "Parámetros de Lanzamiento",
                Location = new Point(this.ClientSize.Width - 400, 10),
                Size = new Size(380, 150),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            this.Controls.Add(groupBoxControles);

            // Crear etiqueta y campo de entrada para altura
            lblAltura = new Label
            {
                Text = "Altura Inicial (m):",
                Location = new Point(20, 30),
                AutoSize = true
            };
            groupBoxControles.Controls.Add(lblAltura);

            txtAltura = new TextBox
            {
                Location = new Point(150, 26),
                Width = 80,
                Text = "100"
            };
            groupBoxControles.Controls.Add(txtAltura);

            // Crear etiqueta y campo de entrada para velocidad
            lblVelocidad = new Label
            {
                Text = "Velocidad Inicial (m/s):",
                Location = new Point(20, 60),
                Size = new Size(125, 20),
                AutoSize = false
            };
            groupBoxControles.Controls.Add(lblVelocidad);

            txtVelocidad = new TextBox
            {
                Location = new Point(150, 56),
                Width = 80,
                Text = "30"
            };
            groupBoxControles.Controls.Add(txtVelocidad);

            // Crear etiqueta y campo de entrada para ángulo (siempre será 0 para lanzamiento horizontal)
            lblAngulo = new Label
            {
                Text = "Ángulo (grados):",
                Location = new Point(20, 90),
                AutoSize = true
            };
            groupBoxControles.Controls.Add(lblAngulo);

            txtAngulo = new TextBox
            {
                Location = new Point(150, 86),
                Width = 80,
                Text = "0",
                Enabled = false // Bloqueado en 0 para lanzamiento horizontal
            };
            groupBoxControles.Controls.Add(txtAngulo);

            // Crear botón de inicio
            btnIniciar = new Button
            {
                Text = "Iniciar Simulación",
                Location = new Point(240, 55),
                Width = 120,
                Height = 40
            };
            btnIniciar.Click += BtnIniciar_Click;
            groupBoxControles.Controls.Add(btnIniciar);
        }

        private void ConfigurarTablaDatos()
        {
            // Crear grupo para tabla de datos
            groupBoxDatos = new GroupBox
            {
                Text = "Datos de la Simulación",
                Location = new Point(this.ClientSize.Width - 400, 170),
                Size = new Size(380, this.ClientSize.Height - 180),
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom
            };
            this.Controls.Add(groupBoxDatos);

            // Configurar DataGridView
            dgvDatos = new DataGridView
            {
                Location = new Point(10, 20),
                Size = new Size(360, groupBoxDatos.Height - 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Agregar columnas
            dgvDatos.Columns.Add("Tiempo", "Tiempo (s)");
            dgvDatos.Columns.Add("Altura", "Altura (m)");
            dgvDatos.Columns.Add("Distancia", "Distancia (m)");
            dgvDatos.Columns.Add("VelocidadX", "Vel. X (m/s)");
            dgvDatos.Columns.Add("VelocidadY", "Vel. Y (m/s)");

            groupBoxDatos.Controls.Add(dgvDatos);
        }

        private void ConfigurarTimer()
        {
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 20; // 20ms para una animación fluida
            animationTimer.Tick += ActualizarAnimacion;
        }

        private void BtnIniciar_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtAltura.Text, out double alturaInicial) && alturaInicial >= 0 &&
                double.TryParse(txtVelocidad.Text, out double velocidadInicial) && velocidadInicial >= 0)
            {
                IniciarSimulacion(alturaInicial, velocidadInicial);
            }
            else
            {
                MessageBox.Show("Introduce valores numéricos válidos y positivos.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IniciarSimulacion(double alturaInicial, double velocidadInicial)
        {
            animationTimer.Stop();

            // Limpiar datos anteriores
            dgvDatos.Rows.Clear();
            datosMovimiento.Clear();

            // Guardar valores iniciales
            this.alturaInicial = alturaInicial;
            this.velocidadInicial = velocidadInicial;

            // Ajustar factor de tiempo según velocidad
            factorTiempo = velocidadInicial < 10 ? 3.0 : (velocidadInicial < 30 ? 2.0 : 1.0);

            // Ajustar intervalo del timer según velocidad
            animationTimer.Interval = velocidadInicial < 30 ? 40 : 20;

            // Recalcular nivel del suelo y factores de escala
            nivelSuelo = pictureBox1.Height - 50;
            AjustarFactoresEscala();

            // Configurar estado inicial
            tiempoTranscurrido = 0;
            ultimoRegistroTiempo = 0;
            x = 50; // Posición inicial en eje X (margen izquierdo)
            y = nivelSuelo - (alturaInicial * factorEscalaY); // Convertir altura real a píxeles
            velocidadX = velocidadInicial; // Velocidad horizontal constante
            velocidadY = 0; // Inicialmente no hay velocidad vertical en lanzamiento horizontal
            numeroBotes = 0;
            alcance = 0;
            alturaMaxima = alturaInicial;
            distanciaReal = 0;
            puntosTrayectoria.Clear();
            enColision = false;
            escalaX = 1.0;
            escalaY = 1.0;
            contadorDeformacion = 0;

            // Registrar punto inicial
            RegistrarDatoPunto();

            // Iniciar animación
            animationTimer.Start();
        }

        private void RegistrarDatoPunto()
        {
            // Crear punto de datos
            DataPoint punto = new DataPoint
            {
                Tiempo = Math.Round(tiempoTranscurrido, 1),
                PosicionX = x,
                PosicionY = y,
                VelocidadX = velocidadX,
                VelocidadY = velocidadY,
                Altura = Math.Round((nivelSuelo - y) / factorEscalaY, 2), // Convertir a metros reales
                Distancia = Math.Round((x - 50) / factorEscalaX, 2) // Convertir a metros reales
            };

            // Añadir a la lista
            datosMovimiento.Add(punto);

            // Ajustar el intervalo de registro según la velocidad
            double intervaloRegistro = Math.Max(0.1, 0.1 * (30 / Math.Max(1, velocidadInicial)));

            // Añadir a la tabla si corresponde a un intervalo apropiado
            if (tiempoTranscurrido - ultimoRegistroTiempo >= intervaloRegistro)
            {
                dgvDatos.Rows.Add(
                    punto.Tiempo.ToString("F1"),
                    punto.Altura.ToString("F2"),
                    punto.Distancia.ToString("F2"),
                    punto.VelocidadX.ToString("F2"),
                    punto.VelocidadY.ToString("F2")
                );

                // Desplazar al último registro - optimizado para no hacerlo siempre
                if (dgvDatos.Rows.Count % 5 == 0 && dgvDatos.Rows.Count > 0)
                    dgvDatos.FirstDisplayedScrollingRowIndex = dgvDatos.Rows.Count - 1;

                ultimoRegistroTiempo = tiempoTranscurrido;
            }
        }

        private void ActualizarAnimacion(object sender, EventArgs e)
        {
            // Incrementar tiempo con factor de aceleración
            double deltaT = (animationTimer.Interval / 1000.0) * factorTiempo; // Convertir ms a segundos
            tiempoTranscurrido += deltaT;

            // Registrar punto para la trayectoria con frecuencia adaptativa
            if (mostrarTrayectoria)
            {
                // Reducir la frecuencia de puntos para velocidades bajas
                double intervaloTrayectoria = 0.03 / Math.Max(0.5, velocidadInicial / 30.0);
                if (tiempoTranscurrido % intervaloTrayectoria < deltaT)
                {
                    puntosTrayectoria.Add(new Point((int)x, (int)y));
                    // Limitar cantidad de puntos basado en la velocidad
                    int maxPuntos = Math.Min(500, 150 + (int)(velocidadInicial * 2));
                    if (puntosTrayectoria.Count > maxPuntos)
                        puntosTrayectoria.RemoveAt(0);
                }
            }

            // Aplicar física del movimiento (fórmulas de lanzamiento horizontal)
            // Calcular nuevo x usando fórmula de movimiento uniforme
            x += velocidadX * deltaT * factorEscalaX;

            // Calcular nuevo y usando fórmula de caída libre
            velocidadY += gravedad * deltaT; // Aceleración por gravedad
            y += velocidadY * deltaT * factorEscalaY;

            // Calcular estadísticas
            alcance = (x - 50) / factorEscalaX; // Convertir a metros reales
            double alturaActual = (nivelSuelo - y) / factorEscalaY; // Convertir a metros reales
            if (alturaActual > alturaMaxima)
                alturaMaxima = alturaActual;

            double dx = velocidadX * deltaT * factorEscalaX;
            double dy = velocidadY * deltaT * factorEscalaY;
            distanciaReal += Math.Sqrt(dx * dx + dy * dy) / Math.Max(factorEscalaX, factorEscalaY); // Convertir a metros reales

            // Registrar datos del movimiento - optimizado para no registrar en cada frame
            if (tiempoTranscurrido % 0.05 < deltaT)
            {
                RegistrarDatoPunto();
            }

            // Detectar colisión con el suelo
            if (y + radio * escalaY > nivelSuelo)
            {
                y = nivelSuelo - radio * escalaY;
                velocidadY = -velocidadY * 0.7;  // Rebote con pérdida de energía
                velocidadX *= 0.95;             // Fricción

                numeroBotes++;

                // Verificar si la simulación debe terminar - condición mejorada
                if (Math.Abs(velocidadY) < 2 && Math.Abs(velocidadX) < 2 || numeroBotes >= 10 || tiempoTranscurrido > 60)
                {
                    velocidadX = 0;
                    velocidadY = 0;
                    animationTimer.Stop();

                    // Mostrar resumen de la simulación
                    MessageBox.Show(
                        $"Simulación completada:\n\n" +
                        $"Altura máxima: {Math.Round(alturaMaxima, 2)} m\n" +
                        $"Alcance horizontal: {Math.Round(alcance, 2)} m\n" +
                        $"Tiempo total: {Math.Round(tiempoTranscurrido, 2)} s\n" +
                        $"Número de rebotes: {numeroBotes}\n\n" +
                        $"¿Deseas lanzar otra vez?",
                        "Resumen de Simulación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);
                }

                // Aplicar deformación al colisionar
                if (!enColision)
                {
                    enColision = true;
                    escalaX = 1.3;
                    escalaY = 0.7;
                    contadorDeformacion = 0;
                }
            }

            // Detectar colisión con los bordes laterales
            if (x - radio * escalaX < 50)
            {
                x = 50 + radio * escalaX;
                velocidadX = -velocidadX * 0.7;
            }
            else if (x + radio * escalaX > pictureBox1.Width - 50)
            {
                x = pictureBox1.Width - 50 - radio * escalaX;
                velocidadX = -velocidadX * 0.7;
            }

            // Manejar animación de deformación
            if (enColision)
            {
                contadorDeformacion++;
                if (contadorDeformacion >= maxDeformacion)
                {
                    escalaX = 1.0 + (escalaX - 1.0) * 0.7;
                    escalaY = 1.0 + (escalaY - 1.0) * 0.7;
                    if (Math.Abs(escalaX - 1.0) < 0.05 && Math.Abs(escalaY - 1.0) < 0.05)
                    {
                        escalaX = 1.0;
                        escalaY = 1.0;
                        enColision = false;
                    }
                }
            }

            // Forzar repintado del pictureBox
            pictureBox1.Invalidate();
        }

        private void DibujarEnPictureBox(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            // Configurar dimensiones para los ejes
            int margenIzquierdo = 50;
            int margenDerecho = 50;
            int margenSuperior = 50;

            // Dibujar ejes
            Pen lapizEjes = new Pen(Color.Black, 2);
            g.DrawLine(lapizEjes, margenIzquierdo, nivelSuelo, pictureBox1.Width - margenDerecho, nivelSuelo); // Eje X
            g.DrawLine(lapizEjes, margenIzquierdo, margenSuperior, margenIzquierdo, nivelSuelo); // Eje Y

            // Ajustar intervalos de cuadrícula según la velocidad
            int intervalX = velocidadInicial < 50 ? 100 : 50;
            int intervalY = velocidadInicial < 50 ? 100 : 50;

            // Dibujar escalas en los ejes
            Font fuenteEscala = new Font("Arial", 8);

            // Escala en eje X - optimizada para mostrar menos marcas en velocidades bajas
            int saltosX = velocidadInicial < 20 ? 2 : 1;
            for (int i = margenIzquierdo + intervalX; i < pictureBox1.Width - margenDerecho; i += intervalX * saltosX)
            {
                g.DrawLine(lapizEjes, i, nivelSuelo - 5, i, nivelSuelo + 5);
                double valorReal = Math.Round((i - margenIzquierdo) / factorEscalaX, 1);
                g.DrawString(valorReal.ToString() + " m", fuenteEscala, Brushes.Black, i - 15, nivelSuelo + 7);
            }

            // Escala en eje Y - optimizada para mostrar menos marcas en velocidades bajas
            int saltosY = velocidadInicial < 20 ? 2 : 1;
            for (int i = nivelSuelo - intervalY; i > margenSuperior; i -= intervalY * saltosY)
            {
                g.DrawLine(lapizEjes, margenIzquierdo - 5, i, margenIzquierdo + 5, i);
                double valorReal = Math.Round((nivelSuelo - i) / factorEscalaY, 1);
                g.DrawString(valorReal.ToString() + " m", fuenteEscala, Brushes.Black, 5, i - 7);
            }

            // Dibujar cuadrícula - optimizada para velocidades bajas
            Pen lapizCuadricula = new Pen(Color.LightGray, 1);
            for (int i = nivelSuelo - intervalY; i > margenSuperior; i -= intervalY * saltosY)
                g.DrawLine(lapizCuadricula, margenIzquierdo, i, pictureBox1.Width - margenDerecho, i);

            for (int i = margenIzquierdo + intervalX; i < pictureBox1.Width - margenDerecho; i += intervalX * saltosX)
                g.DrawLine(lapizCuadricula, i, margenSuperior, i, nivelSuelo);

            // Etiquetas de los ejes
            Font fuenteEtiqueta = new Font("Arial", 10, FontStyle.Bold);
            g.DrawString("Distancia (m)", fuenteEtiqueta, Brushes.Black, (pictureBox1.Width - margenDerecho - margenIzquierdo) / 2 + margenIzquierdo - 40, nivelSuelo + 25);
            g.DrawString("Altura (m)", fuenteEtiqueta, Brushes.Black, 5, margenSuperior - 20);

            // Dibujar trayectoria - optimizada para mejorar rendimiento
            if (mostrarTrayectoria && puntosTrayectoria.Count > 1)
            {
                Pen lapizTrayectoria = new Pen(Color.Blue, 1.5f) { DashStyle = DashStyle.Dot };
                if (puntosTrayectoria.Count > 200)
                {
                    // Para trayectorias largas, reducir puntos para dibujo
                    List<Point> puntosReducidos = new List<Point>();
                    int paso = Math.Max(1, puntosTrayectoria.Count / 150); // Máximo 150 puntos a dibujar
                    for (int i = 0; i < puntosTrayectoria.Count; i += paso)
                        puntosReducidos.Add(puntosTrayectoria[i]);

                    // Asegurar que agregamos el último punto
                    if (puntosReducidos.Count > 0 && puntosTrayectoria.Count > 0 &&
                        puntosReducidos[puntosReducidos.Count - 1] != puntosTrayectoria[puntosTrayectoria.Count - 1])
                        puntosReducidos.Add(puntosTrayectoria[puntosTrayectoria.Count - 1]);

                    if (puntosReducidos.Count > 1)
                        g.DrawCurve(lapizTrayectoria, puntosReducidos.ToArray(), 0.1f);
                }
                else
                {
                    g.DrawCurve(lapizTrayectoria, puntosTrayectoria.ToArray(), 0.2f);
                }
                lapizTrayectoria.Dispose();
            }

            // Guardar estado para aplicar transformación solo al proyectil
            GraphicsState estadoOriginal = g.Save();
            g.TranslateTransform((float)x, (float)y);
            g.ScaleTransform((float)escalaX, (float)escalaY);
            g.TranslateTransform(-(float)x, -(float)y);

            // Dibujar proyectil
            g.FillEllipse(Brushes.Red, (float)(x - radio), (float)(y - radio), (float)(radio * 2), (float)(radio * 2));

            // Dibujar detalles del proyectil
            g.DrawEllipse(Pens.Black, (float)(x - radio), (float)(y - radio), (float)(radio * 2), (float)(radio * 2));
            g.DrawLine(Pens.Black, (float)(x - radio), (float)y, (float)(x + radio), (float)y);
            g.DrawLine(Pens.Black, (float)x, (float)(y - radio), (float)x, (float)(y + radio));

            // Restaurar estado gráfico
            g.Restore(estadoOriginal);

            // Mostrar información de la simulación
            Font fuente = new Font("Arial", 9);
            double velocidadReal = Math.Sqrt(velocidadX * velocidadX + velocidadY * velocidadY);

            // Crear recuadro de información
            Rectangle rectInfo = new Rectangle(pictureBox1.Width - 230, 10, 220, 150);
            g.FillRectangle(new SolidBrush(Color.FromArgb(230, 255, 255, 255)), rectInfo);
            g.DrawRectangle(Pens.Gray, rectInfo);

            // Mostrar información dentro del recuadro
            g.DrawString("INFORMACIÓN DE LA SIMULACIÓN", new Font("Arial", 9, FontStyle.Bold), Brushes.Black, rectInfo.X + 10, rectInfo.Y + 5);
            g.DrawString($"Tiempo: {Math.Round(tiempoTranscurrido, 2)} s", fuente, Brushes.Black, rectInfo.X + 10, rectInfo.Y + 25);
            g.DrawString($"Altura: {Math.Round((nivelSuelo - y) / factorEscalaY, 2)} m", fuente, Brushes.Black, rectInfo.X + 10, rectInfo.Y + 45);
            g.DrawString($"Distancia: {Math.Round((x - margenIzquierdo) / factorEscalaX, 2)} m", fuente, Brushes.Black, rectInfo.X + 10, rectInfo.Y + 65);
            g.DrawString($"Velocidad X: {Math.Round(velocidadX, 2)} m/s", fuente, Brushes.Black, rectInfo.X + 10, rectInfo.Y + 85);
            g.DrawString($"Velocidad Y: {Math.Round(velocidadY, 2)} m/s", fuente, Brushes.Black, rectInfo.X + 10, rectInfo.Y + 105);
            g.DrawString($"Velocidad total: {Math.Round(velocidadReal, 2)} m/s", fuente, Brushes.Black, rectInfo.X + 10, rectInfo.Y + 125);

            // Mostrar valores iniciales
            g.DrawString($"Altura inicial: {alturaInicial} m", fuente, Brushes.DarkBlue, margenIzquierdo + 10, margenSuperior + 5);
            g.DrawString($"Velocidad inicial: {velocidadInicial} m/s", fuente, Brushes.DarkBlue, margenIzquierdo + 10, margenSuperior + 25);

            // Mostrar factor de aceleración si está activo
            if (factorTiempo > 1.0)
            {
                g.DrawString($"Simulación acelerada: {factorTiempo}x", new Font("Arial", 8, FontStyle.Italic),
                    Brushes.Red, margenIzquierdo + 10, margenSuperior + 45);
            }
        }
    }
}