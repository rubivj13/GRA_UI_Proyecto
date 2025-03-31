namespace FrmsFractales
{
    partial class FrmFractal02
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
            pictureBoxJulia = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxJulia).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxJulia
            // 
            pictureBoxJulia.Location = new Point(30, 28);
            pictureBoxJulia.Name = "pictureBoxJulia";
            pictureBoxJulia.Size = new Size(748, 394);
            pictureBoxJulia.TabIndex = 0;
            pictureBoxJulia.TabStop = false;
            // 
            // FrmFractal02
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBoxJulia);
            Name = "FrmFractal02";
            Text = "FractalJuliaSet";
            Load += FrmFractal02_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxJulia).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxJulia;
    }
}