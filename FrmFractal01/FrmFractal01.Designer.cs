namespace FrmFractal01
{
    partial class FrmFractal01
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBoxMandelbrot = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMandelbrot).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxMandelbrot
            // 
            pictureBoxMandelbrot.Location = new Point(17, 20);
            pictureBoxMandelbrot.Name = "pictureBoxMandelbrot";
            pictureBoxMandelbrot.Size = new Size(550, 550);
            pictureBoxMandelbrot.TabIndex = 0;
            pictureBoxMandelbrot.TabStop = false;
            // 
            // FrmFractal01
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(579, 582);
            Controls.Add(pictureBoxMandelbrot);
            Name = "FrmFractal01";
            Text = "FractalMandelbrot";
            Load += FrmFractal01_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxMandelbrot).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxMandelbrot;
    }
}
