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
    public partial class FrmLineasEscalar : Form
    {
        public FrmLineasEscalar()
        {
            InitializeComponent();
        }

        Graphics graphics;
        int ex = 0, ey = 0;

        private void FrmLineasEscalar_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics;
            Pen pen = new Pen(Color.Blue, 3);
            Rectangle rectangle = new Rectangle(50, 50, 50 + ex, 50 + ey);
            graphics.DrawRectangle(pen, rectangle);
        }

        private void FrmLineasEscalar_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    //incremento
                    ex++;
                    ey++;
                    pictureBox1.Refresh();
                    //rectangle (ex, ey,50,50);
                    break;
                case Keys.Down:
                    //decrementar
                    if (ex > 1)
                    {
                        ex--;
                        ey--;
                        pictureBox1.Refresh();
                    }
                    break;

            }
        }
    }
}
