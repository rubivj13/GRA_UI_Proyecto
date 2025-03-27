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
    public partial class FrmLineas : Form
    {
        public FrmLineas()
        {
            InitializeComponent();
        }

        Graphics graphics;
        private void ptDibujo_Paint(object sender, PaintEventArgs e)
        {




        }
        private void FrmLineas_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics;
            Pen pen = new Pen(Color.Blue, 3);
            graphics.DrawLine(pen, new Point(50, 50), new Point(100, 50));
            graphics.DrawLine(pen, new Point(100, 50), new Point(100, 100));
            graphics.DrawLine(pen, new Point(100, 100), new Point(50, 100));
            graphics.DrawLine(pen, new Point(50, 100), new Point(50, 50));
        }
    }
}
