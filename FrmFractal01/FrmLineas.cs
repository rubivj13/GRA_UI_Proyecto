using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmsFractales
{
    public partial class FrmLineas : Form
    {
        public FrmLineas()
        {
            InitializeComponent();
        }

        Graphics graphics;

        private void ptbDibujo_Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics; //e: significa el objeto osea el picturebox
        }
    }
}
