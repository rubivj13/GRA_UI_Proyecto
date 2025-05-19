namespace FormsGraficos
{
    partial class FrmGraficacion3D
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Button btnGuardarFiguras;
            pictureBox1 = new PictureBox();
            lblDibujarPuntosFigura = new Label();
            lblDibujarEnX = new Label();
            txtDibujarEnX = new TextBox();
            lblDibujarEnY = new Label();
            txtDibujarEnY = new TextBox();
            lblDibujarEnZ = new Label();
            txtDibujarEnZ = new TextBox();
            btnDibujarPunto = new Button();
            btnColorFigura = new Button();
            listBoxHistorialFiguras = new ListBox();
            lblHistorialFiguras = new Label();
            btnLimpiarPlano = new Button();
            btnBorrarTodo = new Button();
            listBoxCoordenadas = new ListBox();
            lblCoordenadasFiguras = new Label();
            btnEscalacionRO = new Button();
            btnTraslacion = new Button();
            btnEscalacionRPF = new Button();
            btnRotacionREjeX = new Button();
            btnRotacionREjeY = new Button();
            btnRotacionREjeZ = new Button();
            btnReflexionREjeX = new Button();
            btnReflexionREjeY = new Button();
            btnReflexionREjeZ = new Button();
            btnAfilamiento = new Button();
            btnGuardarFiguras = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnGuardarFiguras
            // 
            btnGuardarFiguras.Location = new Point(879, 353);
            btnGuardarFiguras.Name = "btnGuardarFiguras";
            btnGuardarFiguras.Size = new Size(252, 29);
            btnGuardarFiguras.TabIndex = 15;
            btnGuardarFiguras.Text = "Guardar figuras o figura creadas";
            btnGuardarFiguras.UseVisualStyleBackColor = true;
            btnGuardarFiguras.Click += btnGuardarFiguras_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(2, 1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(848, 750);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // lblDibujarPuntosFigura
            // 
            lblDibujarPuntosFigura.AutoSize = true;
            lblDibujarPuntosFigura.Location = new Point(886, 21);
            lblDibujarPuntosFigura.Name = "lblDibujarPuntosFigura";
            lblDibujarPuntosFigura.Size = new Size(187, 20);
            lblDibujarPuntosFigura.TabIndex = 1;
            lblDibujarPuntosFigura.Text = "Dibujar puntos de la figura";
            // 
            // lblDibujarEnX
            // 
            lblDibujarEnX.AutoSize = true;
            lblDibujarEnX.Location = new Point(885, 70);
            lblDibujarEnX.Name = "lblDibujarEnX";
            lblDibujarEnX.Size = new Size(21, 20);
            lblDibujarEnX.TabIndex = 2;
            lblDibujarEnX.Text = "X:";
            // 
            // txtDibujarEnX
            // 
            txtDibujarEnX.Location = new Point(912, 67);
            txtDibujarEnX.Name = "txtDibujarEnX";
            txtDibujarEnX.Size = new Size(125, 27);
            txtDibujarEnX.TabIndex = 3;
            txtDibujarEnX.TextChanged += txtDibujarEnX_TextChanged;
            // 
            // lblDibujarEnY
            // 
            lblDibujarEnY.AutoSize = true;
            lblDibujarEnY.Location = new Point(1074, 70);
            lblDibujarEnY.Name = "lblDibujarEnY";
            lblDibujarEnY.Size = new Size(20, 20);
            lblDibujarEnY.TabIndex = 4;
            lblDibujarEnY.Text = "Y:";
            // 
            // txtDibujarEnY
            // 
            txtDibujarEnY.Location = new Point(1100, 67);
            txtDibujarEnY.Name = "txtDibujarEnY";
            txtDibujarEnY.Size = new Size(125, 27);
            txtDibujarEnY.TabIndex = 5;
            txtDibujarEnY.TextChanged += txtDibujarEnY_TextChanged;
            // 
            // lblDibujarEnZ
            // 
            lblDibujarEnZ.AutoSize = true;
            lblDibujarEnZ.Location = new Point(1246, 70);
            lblDibujarEnZ.Name = "lblDibujarEnZ";
            lblDibujarEnZ.Size = new Size(21, 20);
            lblDibujarEnZ.TabIndex = 6;
            lblDibujarEnZ.Text = "Z:";
            // 
            // txtDibujarEnZ
            // 
            txtDibujarEnZ.Location = new Point(1274, 67);
            txtDibujarEnZ.Name = "txtDibujarEnZ";
            txtDibujarEnZ.Size = new Size(125, 27);
            txtDibujarEnZ.TabIndex = 7;
            txtDibujarEnZ.TextChanged += txtDibujarEnZ_TextChanged;
            // 
            // btnDibujarPunto
            // 
            btnDibujarPunto.Location = new Point(1145, 109);
            btnDibujarPunto.Name = "btnDibujarPunto";
            btnDibujarPunto.Size = new Size(254, 29);
            btnDibujarPunto.TabIndex = 8;
            btnDibujarPunto.Text = "Dibujar punto en el plano";
            btnDibujarPunto.UseVisualStyleBackColor = true;
            btnDibujarPunto.Click += btnDibujarPunto_Click;
            // 
            // btnColorFigura
            // 
            btnColorFigura.Location = new Point(912, 109);
            btnColorFigura.Name = "btnColorFigura";
            btnColorFigura.Size = new Size(201, 29);
            btnColorFigura.TabIndex = 9;
            btnColorFigura.Text = "Elegir color de la figura ";
            btnColorFigura.UseVisualStyleBackColor = true;
            btnColorFigura.Click += btnColorFigura_Click;
            // 
            // listBoxHistorialFiguras
            // 
            listBoxHistorialFiguras.FormattingEnabled = true;
            listBoxHistorialFiguras.Location = new Point(872, 192);
            listBoxHistorialFiguras.Name = "listBoxHistorialFiguras";
            listBoxHistorialFiguras.Size = new Size(549, 144);
            listBoxHistorialFiguras.TabIndex = 10;
            listBoxHistorialFiguras.SelectedIndexChanged += listBoxHistorialFiguras_SelectedIndexChanged;
            // 
            // lblHistorialFiguras
            // 
            lblHistorialFiguras.AutoSize = true;
            lblHistorialFiguras.Location = new Point(879, 158);
            lblHistorialFiguras.Name = "lblHistorialFiguras";
            lblHistorialFiguras.Size = new Size(135, 20);
            lblHistorialFiguras.TabIndex = 11;
            lblHistorialFiguras.Text = "Historial de figuras";
            // 
            // btnLimpiarPlano
            // 
            btnLimpiarPlano.Location = new Point(1156, 353);
            btnLimpiarPlano.Name = "btnLimpiarPlano";
            btnLimpiarPlano.Size = new Size(127, 29);
            btnLimpiarPlano.TabIndex = 13;
            btnLimpiarPlano.Text = "Limpiar Plano";
            btnLimpiarPlano.UseVisualStyleBackColor = true;
            btnLimpiarPlano.Click += btnLimpiarPlano_Click;
            // 
            // btnBorrarTodo
            // 
            btnBorrarTodo.Location = new Point(1289, 353);
            btnBorrarTodo.Name = "btnBorrarTodo";
            btnBorrarTodo.Size = new Size(122, 29);
            btnBorrarTodo.TabIndex = 14;
            btnBorrarTodo.Text = "¡Borrar Todo!";
            btnBorrarTodo.UseVisualStyleBackColor = true;
            btnBorrarTodo.Click += btnBorrarTodo_Click;
            // 
            // listBoxCoordenadas
            // 
            listBoxCoordenadas.FormattingEnabled = true;
            listBoxCoordenadas.Location = new Point(872, 426);
            listBoxCoordenadas.Name = "listBoxCoordenadas";
            listBoxCoordenadas.Size = new Size(549, 124);
            listBoxCoordenadas.TabIndex = 16;
            listBoxCoordenadas.SelectedIndexChanged += listBoxCoordenadas_SelectedIndexChanged;
            // 
            // lblCoordenadasFiguras
            // 
            lblCoordenadasFiguras.AutoSize = true;
            lblCoordenadasFiguras.Location = new Point(879, 394);
            lblCoordenadasFiguras.Name = "lblCoordenadasFiguras";
            lblCoordenadasFiguras.Size = new Size(180, 20);
            lblCoordenadasFiguras.TabIndex = 17;
            lblCoordenadasFiguras.Text = "Cordenadas de las figuras";
            // 
            // btnEscalacionRO
            // 
            btnEscalacionRO.Location = new Point(954, 573);
            btnEscalacionRO.Name = "btnEscalacionRO";
            btnEscalacionRO.Size = new Size(159, 48);
            btnEscalacionRO.TabIndex = 18;
            btnEscalacionRO.Text = "Escalación (Respecto al Origen)";
            btnEscalacionRO.UseVisualStyleBackColor = true;
            btnEscalacionRO.Click += btnEscalacionRO_Click;
            // 
            // btnTraslacion
            // 
            btnTraslacion.Location = new Point(856, 585);
            btnTraslacion.Name = "btnTraslacion";
            btnTraslacion.Size = new Size(94, 29);
            btnTraslacion.TabIndex = 19;
            btnTraslacion.Text = "Traslación";
            btnTraslacion.UseVisualStyleBackColor = true;
            btnTraslacion.Click += btnTraslacion_Click;
            // 
            // btnEscalacionRPF
            // 
            btnEscalacionRPF.Location = new Point(1119, 571);
            btnEscalacionRPF.Name = "btnEscalacionRPF";
            btnEscalacionRPF.Size = new Size(158, 53);
            btnEscalacionRPF.TabIndex = 20;
            btnEscalacionRPF.Text = "Escalación (Respecto a un Punto Fijo)";
            btnEscalacionRPF.UseVisualStyleBackColor = true;
            btnEscalacionRPF.Click += btnEscalacionRPF_Click;
            // 
            // btnRotacionREjeX
            // 
            btnRotacionREjeX.Location = new Point(1283, 570);
            btnRotacionREjeX.Name = "btnRotacionREjeX";
            btnRotacionREjeX.Size = new Size(147, 54);
            btnRotacionREjeX.TabIndex = 22;
            btnRotacionREjeX.Text = "Rotación (Respecto al eje X)";
            btnRotacionREjeX.UseVisualStyleBackColor = true;
            btnRotacionREjeX.Click += btnRotacionREjeX_Click;
            // 
            // btnRotacionREjeY
            // 
            btnRotacionREjeY.Location = new Point(907, 627);
            btnRotacionREjeY.Name = "btnRotacionREjeY";
            btnRotacionREjeY.Size = new Size(152, 52);
            btnRotacionREjeY.TabIndex = 23;
            btnRotacionREjeY.Text = "Rotación (Respecto al eje Y)";
            btnRotacionREjeY.UseVisualStyleBackColor = true;
            btnRotacionREjeY.Click += btnRotacionREjeY_Click;
            // 
            // btnRotacionREjeZ
            // 
            btnRotacionREjeZ.Location = new Point(1072, 630);
            btnRotacionREjeZ.Name = "btnRotacionREjeZ";
            btnRotacionREjeZ.Size = new Size(153, 49);
            btnRotacionREjeZ.TabIndex = 24;
            btnRotacionREjeZ.Text = "Rotación (Respecto al eje Z)";
            btnRotacionREjeZ.UseVisualStyleBackColor = true;
            btnRotacionREjeZ.Click += btnRotacionREjeZ_Click;
            // 
            // btnReflexionREjeX
            // 
            btnReflexionREjeX.Location = new Point(1234, 630);
            btnReflexionREjeX.Name = "btnReflexionREjeX";
            btnReflexionREjeX.Size = new Size(150, 52);
            btnReflexionREjeX.TabIndex = 25;
            btnReflexionREjeX.Text = "Reflexión (Respecto al eje X)";
            btnReflexionREjeX.UseVisualStyleBackColor = true;
            btnReflexionREjeX.Click += btnReflexionREjeX_Click;
            // 
            // btnReflexionREjeY
            // 
            btnReflexionREjeY.Location = new Point(904, 686);
            btnReflexionREjeY.Name = "btnReflexionREjeY";
            btnReflexionREjeY.Size = new Size(155, 60);
            btnReflexionREjeY.TabIndex = 26;
            btnReflexionREjeY.Text = "Reflexión (Respecto al eje Y)";
            btnReflexionREjeY.UseVisualStyleBackColor = true;
            btnReflexionREjeY.Click += btnReflexionREjeY_Click;
            // 
            // btnReflexionREjeZ
            // 
            btnReflexionREjeZ.Location = new Point(1074, 688);
            btnReflexionREjeZ.Name = "btnReflexionREjeZ";
            btnReflexionREjeZ.Size = new Size(159, 57);
            btnReflexionREjeZ.TabIndex = 27;
            btnReflexionREjeZ.Text = "Reflexión (Respecto al eje Z)";
            btnReflexionREjeZ.UseVisualStyleBackColor = true;
            btnReflexionREjeZ.Click += btnReflexionREjeZ_Click;
            // 
            // btnAfilamiento
            // 
            btnAfilamiento.Location = new Point(1263, 703);
            btnAfilamiento.Name = "btnAfilamiento";
            btnAfilamiento.Size = new Size(121, 29);
            btnAfilamiento.TabIndex = 28;
            btnAfilamiento.Text = "Afilamiento";
            btnAfilamiento.UseVisualStyleBackColor = true;
            btnAfilamiento.Click += btnAfilamiento_Click;
            // 
            // FrmGraficacion3D
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1442, 753);
            Controls.Add(btnAfilamiento);
            Controls.Add(btnReflexionREjeZ);
            Controls.Add(btnReflexionREjeY);
            Controls.Add(btnReflexionREjeX);
            Controls.Add(btnRotacionREjeZ);
            Controls.Add(btnRotacionREjeY);
            Controls.Add(btnRotacionREjeX);
            Controls.Add(btnEscalacionRPF);
            Controls.Add(btnTraslacion);
            Controls.Add(btnEscalacionRO);
            Controls.Add(lblCoordenadasFiguras);
            Controls.Add(listBoxCoordenadas);
            Controls.Add(btnGuardarFiguras);
            Controls.Add(btnBorrarTodo);
            Controls.Add(btnLimpiarPlano);
            Controls.Add(lblHistorialFiguras);
            Controls.Add(listBoxHistorialFiguras);
            Controls.Add(btnColorFigura);
            Controls.Add(btnDibujarPunto);
            Controls.Add(txtDibujarEnZ);
            Controls.Add(lblDibujarEnZ);
            Controls.Add(txtDibujarEnY);
            Controls.Add(lblDibujarEnY);
            Controls.Add(txtDibujarEnX);
            Controls.Add(lblDibujarEnX);
            Controls.Add(lblDibujarPuntosFigura);
            Controls.Add(pictureBox1);
            Name = "FrmGraficacion3D";
            Text = "FrmGraficacion3D";
            Load += FrmGraficacion3D_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lblDibujarPuntosFigura;
        private Label lblDibujarEnX;
        private TextBox txtDibujarEnX;
        private Label lblDibujarEnY;
        private TextBox txtDibujarEnY;
        private Label lblDibujarEnZ;
        private TextBox txtDibujarEnZ;
        private Button btnDibujarPunto;
        private Button btnColorFigura;
        private ListBox listBoxHistorialFiguras;
        private Label lblHistorialFiguras;
        private Button btnLimpiarPlano;
        private Button btnBorrarTodo;
        private Button btnGuardarFiguras;
        private ListBox listBoxCoordenadas;
        private Label lblCoordenadasFiguras;
        private Button btnEscalacionRO;
        private Button btnTraslacion;
        private Button btnEscalacionRPF;
        private Button btnRotacionREjeX;
        private Button btnRotacionREjeY;
        private Button btnRotacionREjeZ;
        private Button btnReflexionREjeX;
        private Button btnReflexionREjeY;
        private Button btnReflexionREjeZ;
        private Button btnAfilamiento;
    }
}