using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA230926
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            btnStart.Click += BtnStart_Click;
            nudSor.ValueChanged += NudSO_ValueChanged;
            nudOszlop.ValueChanged += NudSO_ValueChanged;
        }

        private void NudSO_ValueChanged(object sender, EventArgs e)
        {
            int optAknaSzam = (int)(nudSor.Value * nudOszlop.Value * .2m);
            nudAknaSzam.Value = optAknaSzam;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            this.Hide();
            _ = new FrmGame(
                o: (int)nudOszlop.Value,
                s: (int)nudSor.Value,
                a: (int)nudAknaSzam.Value)
                .ShowDialog();
            this.Show();
        }
    }
}
