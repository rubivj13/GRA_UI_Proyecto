namespace FrmsFractales
{
    partial class FrmLineas
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
            ptbDibujo = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)ptbDibujo).BeginInit();
            SuspendLayout();
            // 
            // ptbDibujo
            // 
            ptbDibujo.Dock = DockStyle.Fill;
            ptbDibujo.Location = new Point(0, 0);
            ptbDibujo.Name = "ptbDibujo";
            ptbDibujo.Size = new Size(800, 450);
            ptbDibujo.TabIndex = 0;
            ptbDibujo.TabStop = false;
            ptbDibujo.Paint += ptbDibujo_Paint;
            // 
            // FrmLineas
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ptbDibujo);
            Name = "FrmLineas";
            Text = "FrmLineas";
            ((System.ComponentModel.ISupportInitialize)ptbDibujo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox ptbDibujo;
    }
}