namespace FormsGraficos
{
    partial class FrmGraficacion2D
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
            pictureBox1 = new PictureBox();
            lblHistorial = new Label();
            listBoxHistorialFig = new ListBox();
            btnGuardarFiguras = new Button();
            btnLimpiarPlano = new Button();
            btnBorrarTodo = new Button();
            btnTraslacion = new Button();
            btnEscalacionRO = new Button();
            btnEscalacionRPF = new Button();
            btnRotacionRO = new Button();
            btnRotacionRPF = new Button();
            btnReflexionRO = new Button();
            btnReflexionREjeX = new Button();
            btnReflexionREjeY = new Button();
            lblMostrarCordenadasPF = new Label();
            listBoxCoordenadas = new ListBox();
            BtnColorFigura = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(0, -1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1014, 712);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // lblHistorial
            // 
            lblHistorial.AutoSize = true;
            lblHistorial.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblHistorial.Location = new Point(1144, 9);
            lblHistorial.Name = "lblHistorial";
            lblHistorial.Size = new Size(197, 28);
            lblHistorial.TabIndex = 1;
            lblHistorial.Text = "Historial de Figuras";
            // 
            // listBoxHistorialFig
            // 
            listBoxHistorialFig.FormattingEnabled = true;
            listBoxHistorialFig.Location = new Point(1027, 40);
            listBoxHistorialFig.Name = "listBoxHistorialFig";
            listBoxHistorialFig.Size = new Size(398, 164);
            listBoxHistorialFig.TabIndex = 2;
            listBoxHistorialFig.SelectedIndexChanged += listBoxHistorialFig_SelectedIndexChanged;
            // 
            // btnGuardarFiguras
            // 
            btnGuardarFiguras.Location = new Point(1158, 220);
            btnGuardarFiguras.Name = "btnGuardarFiguras";
            btnGuardarFiguras.Size = new Size(252, 29);
            btnGuardarFiguras.TabIndex = 3;
            btnGuardarFiguras.Text = "Guardar figuras o figura creadas";
            btnGuardarFiguras.UseVisualStyleBackColor = true;
            btnGuardarFiguras.Click += btnGuardarFiguras_Click;
            // 
            // btnLimpiarPlano
            // 
            btnLimpiarPlano.Location = new Point(1158, 268);
            btnLimpiarPlano.Name = "btnLimpiarPlano";
            btnLimpiarPlano.Size = new Size(127, 29);
            btnLimpiarPlano.TabIndex = 4;
            btnLimpiarPlano.Text = "Limpiar Plano";
            btnLimpiarPlano.UseVisualStyleBackColor = true;
            btnLimpiarPlano.Click += btnLimpiarPlano_Click;
            // 
            // btnBorrarTodo
            // 
            btnBorrarTodo.Location = new Point(1303, 268);
            btnBorrarTodo.Name = "btnBorrarTodo";
            btnBorrarTodo.Size = new Size(122, 29);
            btnBorrarTodo.TabIndex = 5;
            btnBorrarTodo.Text = "¡Borrar Todo!";
            btnBorrarTodo.UseVisualStyleBackColor = true;
            btnBorrarTodo.Click += btnBorrarTodo_Click;
            // 
            // btnTraslacion
            // 
            btnTraslacion.Location = new Point(1181, 462);
            btnTraslacion.Name = "btnTraslacion";
            btnTraslacion.Size = new Size(94, 29);
            btnTraslacion.TabIndex = 6;
            btnTraslacion.Text = "Traslación";
            btnTraslacion.UseVisualStyleBackColor = true;
            btnTraslacion.Click += btnTraslacion_Click;
            // 
            // btnEscalacionRO
            // 
            btnEscalacionRO.Location = new Point(1044, 506);
            btnEscalacionRO.Name = "btnEscalacionRO";
            btnEscalacionRO.Size = new Size(159, 48);
            btnEscalacionRO.TabIndex = 7;
            btnEscalacionRO.Text = "Escalación (Respecto al Origen)";
            btnEscalacionRO.UseVisualStyleBackColor = true;
            btnEscalacionRO.Click += btnEscalacionRO_Click;
            // 
            // btnEscalacionRPF
            // 
            btnEscalacionRPF.Location = new Point(1247, 497);
            btnEscalacionRPF.Name = "btnEscalacionRPF";
            btnEscalacionRPF.Size = new Size(158, 53);
            btnEscalacionRPF.TabIndex = 8;
            btnEscalacionRPF.Text = "Escalación (Respecto a un Punto Fijo)";
            btnEscalacionRPF.UseVisualStyleBackColor = true;
            btnEscalacionRPF.Click += btnEscalacionRPF_Click;
            // 
            // btnRotacionRO
            // 
            btnRotacionRO.Location = new Point(1044, 560);
            btnRotacionRO.Name = "btnRotacionRO";
            btnRotacionRO.Size = new Size(150, 51);
            btnRotacionRO.TabIndex = 9;
            btnRotacionRO.Text = "Rotación (Respecto al Origen)";
            btnRotacionRO.UseVisualStyleBackColor = true;
            btnRotacionRO.Click += btnRotacionRO_Click;
            // 
            // btnRotacionRPF
            // 
            btnRotacionRPF.Location = new Point(1257, 556);
            btnRotacionRPF.Name = "btnRotacionRPF";
            btnRotacionRPF.Size = new Size(148, 51);
            btnRotacionRPF.TabIndex = 10;
            btnRotacionRPF.Text = "Rotación (Respacto a un Punto Fijo)";
            btnRotacionRPF.UseVisualStyleBackColor = true;
            btnRotacionRPF.Click += btnRotacionRPF_Click;
            // 
            // btnReflexionRO
            // 
            btnReflexionRO.Location = new Point(1044, 617);
            btnReflexionRO.Name = "btnReflexionRO";
            btnReflexionRO.Size = new Size(156, 48);
            btnReflexionRO.TabIndex = 11;
            btnReflexionRO.Text = "Reflexión (Respecto al Origen)";
            btnReflexionRO.UseVisualStyleBackColor = true;
            btnReflexionRO.Click += btnReflexionRO_Click;
            // 
            // btnReflexionREjeX
            // 
            btnReflexionREjeX.Location = new Point(1255, 613);
            btnReflexionREjeX.Name = "btnReflexionREjeX";
            btnReflexionREjeX.Size = new Size(150, 52);
            btnReflexionREjeX.TabIndex = 12;
            btnReflexionREjeX.Text = "Reflexión (Respecto al eje X)";
            btnReflexionREjeX.UseVisualStyleBackColor = true;
            btnReflexionREjeX.Click += btnReflexionREjeX_Click;
            // 
            // btnReflexionREjeY
            // 
            btnReflexionREjeY.Location = new Point(1125, 671);
            btnReflexionREjeY.Name = "btnReflexionREjeY";
            btnReflexionREjeY.Size = new Size(216, 29);
            btnReflexionREjeY.TabIndex = 13;
            btnReflexionREjeY.Text = "Reflexión (Respecto al eje Y)";
            btnReflexionREjeY.UseVisualStyleBackColor = true;
            btnReflexionREjeY.Click += btnReflexionREjeY_Click;
            // 
            // lblMostrarCordenadasPF
            // 
            lblMostrarCordenadasPF.AutoSize = true;
            lblMostrarCordenadasPF.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblMostrarCordenadasPF.Location = new Point(1101, 300);
            lblMostrarCordenadasPF.Name = "lblMostrarCordenadasPF";
            lblMostrarCordenadasPF.Size = new Size(260, 28);
            lblMostrarCordenadasPF.TabIndex = 14;
            lblMostrarCordenadasPF.Text = "Cordenadas de las Figuras";
            // 
            // listBoxCoordenadas
            // 
            listBoxCoordenadas.FormattingEnabled = true;
            listBoxCoordenadas.Location = new Point(1027, 335);
            listBoxCoordenadas.Name = "listBoxCoordenadas";
            listBoxCoordenadas.Size = new Size(398, 124);
            listBoxCoordenadas.TabIndex = 15;
            listBoxCoordenadas.SelectedIndexChanged += listBoxCoordenadas_SelectedIndexChanged;
            // 
            // BtnColorFigura
            // 
            BtnColorFigura.Location = new Point(1027, 235);
            BtnColorFigura.Name = "BtnColorFigura";
            BtnColorFigura.Size = new Size(125, 52);
            BtnColorFigura.TabIndex = 16;
            BtnColorFigura.Text = "Elegir Color de Figura";
            BtnColorFigura.UseVisualStyleBackColor = true;
            BtnColorFigura.Click += BtnColorFigura_Click;
            // 
            // FrmGraficacion2D
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1437, 712);
            Controls.Add(BtnColorFigura);
            Controls.Add(listBoxCoordenadas);
            Controls.Add(lblMostrarCordenadasPF);
            Controls.Add(btnReflexionREjeY);
            Controls.Add(btnReflexionREjeX);
            Controls.Add(btnReflexionRO);
            Controls.Add(btnRotacionRPF);
            Controls.Add(btnRotacionRO);
            Controls.Add(btnEscalacionRPF);
            Controls.Add(btnEscalacionRO);
            Controls.Add(btnTraslacion);
            Controls.Add(btnBorrarTodo);
            Controls.Add(btnLimpiarPlano);
            Controls.Add(btnGuardarFiguras);
            Controls.Add(listBoxHistorialFig);
            Controls.Add(lblHistorial);
            Controls.Add(pictureBox1);
            Name = "FrmGraficacion2D";
            Text = "FrmGraficacion2D";
            Load += FrmGraficacion2D_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lblHistorial;
        private ListBox listBoxHistorialFig;
        private Button btnGuardarFiguras;
        private Button btnLimpiarPlano;
        private Button btnBorrarTodo;
        private Button btnTraslacion;
        private Button btnEscalacionRO;
        private Button btnEscalacionRPF;
        private Button btnRotacionRO;
        private Button btnRotacionRPF;
        private Button btnReflexionRO;
        private Button btnReflexionREjeX;
        private Button btnReflexionREjeY;
        private Label lblMostrarCordenadasPF;
        private ListBox listBoxCoordenadas;
        private Button BtnColorFigura;
    }
}