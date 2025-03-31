namespace FrmsFractales
{
    partial class FrmFractal04
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
            pictureBoxBurningShip = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBurningShip).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxBurningShip
            // 
            pictureBoxBurningShip.Location = new Point(31, 33);
            pictureBoxBurningShip.Name = "pictureBoxBurningShip";
            pictureBoxBurningShip.Size = new Size(545, 478);
            pictureBoxBurningShip.TabIndex = 0;
            pictureBoxBurningShip.TabStop = false;
            // 
            // FrmFractal04
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(608, 549);
            Controls.Add(pictureBoxBurningShip);
            Name = "FrmFractal04";
            Text = "FractalBurningShip";
            Load += FrmFractal04_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxBurningShip).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxBurningShip;
    }
}