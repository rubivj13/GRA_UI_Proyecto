namespace FrmsFractales
{
    partial class FrmFractal03
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
            pictureBoxNewton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNewton).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxNewton
            // 
            pictureBoxNewton.Location = new Point(27, 29);
            pictureBoxNewton.Name = "pictureBoxNewton";
            pictureBoxNewton.Size = new Size(736, 562);
            pictureBoxNewton.TabIndex = 0;
            pictureBoxNewton.TabStop = false;
            // 
            // FrmFractal03
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(790, 620);
            Controls.Add(pictureBoxNewton);
            Name = "FrmFractal03";
            Text = "FractalNewton";
            Load += FrmFractal03_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxNewton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxNewton;
    }
}