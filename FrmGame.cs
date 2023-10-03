﻿using System;
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
    public partial class FrmGame : Form
    {
        static Random rnd = new Random();
        public int Oszlopok { get; set; }
        public int Sorok { get; set; }
        public int Aknak { get; set; }
        public int UiScale { get; set; }
        public Button[,] Aknamezo { get; set; }
        public bool[,] AknaPos { get; set; }

        public FrmGame(int o, int s, int a, int u)
        {
            Oszlopok = o;
            Sorok = s;
            Aknak = a;
            UiScale = u;
            InitializeComponent();
            InitAknamezo();
            InitAknaPos();
        }

        private void InitAknaPos()
        {
            AknaPos = new bool[Sorok, Oszlopok];

            for (int i = 0; i < Aknak; )
            {
                int rs = rnd.Next(Sorok);
                int ro = rnd.Next(Oszlopok);

                if (!AknaPos[rs, ro])
                {
                    AknaPos[rs, ro] = true;
                    i++;
                }
            }
        }

        private void InitAknamezo()
        {
            Aknamezo = new Button[Sorok, Oszlopok];
            for (int s = 0; s < Aknamezo.GetLength(0); s++)
            {
                for (int o = 0; o < Aknamezo.GetLength(1); o++)
                {
                    Aknamezo[s, o] = new Button()
                    {
                        Bounds = new Rectangle(
                            x: o * UiScale,
                            y: s * UiScale,
                            width: UiScale,
                            height: UiScale),
                        BackColor = Color.LightGray,
                        Font = new Font("Arial", (UiScale / 2) * .8f, FontStyle.Bold),
                        Margin = Padding.Empty,
                        TextAlign = ContentAlignment.MiddleCenter,
                    };
                    Aknamezo[s, o].MouseUp += AknamezoBtn_MouseUp;
                    this.Controls.Add(Aknamezo[s, o]);
                }
            }
        }

        private void AknamezoBtn_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right 
                && (sender as Button).Text.Length == 0)
            {
                if ((sender as Button).BackColor == Color.Black)
                {
                    (sender as Button).BackColor = Color.LightGray;
                }
                else (sender as Button).BackColor = Color.Black;
            }
            else if (e.Button == MouseButtons.Left)
            {
                var crds = MatrixIndexOf(sender as Button);
                if (AknaPos[crds.s, crds.o])
                {
                    //theend
                    (sender as Button).BackColor = Color.Red;
                }
                else
                {
                    int kasz = KornyezoAknakSzam(crds);
                    (sender as Button).BackColor = Color.White;

                    switch (kasz)
                    {
                        case 0: (sender as Button).ForeColor = Color.LightGray; break;
                        case 1: (sender as Button).ForeColor = Color.Blue; break;
                        case 2: (sender as Button).ForeColor = Color.DarkGreen; break;
                        case 3: (sender as Button).ForeColor = Color.Red; break;
                        case 4: (sender as Button).ForeColor = Color.DarkBlue; break;
                        case 5: (sender as Button).ForeColor = Color.DarkRed; break;
                        case 6: (sender as Button).ForeColor = Color.Cyan; break;
                        case 7: (sender as Button).ForeColor = Color.Black; break;
                        case 8: (sender as Button).ForeColor = Color.Gray; break;
                        default: throw new Exception("hiba");
                    }

                    (sender as Button).Text = $"{kasz}";

                    if (kasz == 0)
                    {
                        var kornyezoGombok = GetKornyezoGombok(sender as Button);
                        foreach (var btn in kornyezoGombok)
                        {
                            AknamezoBtn_MouseUp(btn, e);
                        }
                    }

                }
            }
        }

        private List<Button> GetKornyezoGombok(Button button)
        {
            var crds = MatrixIndexOf(button);
            var kornyezoGombok = new List<Button>();
            for (int s = crds.s - 1; s <= crds.s + 1; s++)
            {
                for (int o = crds.o - 1; o <= crds.o + 1; o++)
                {
                    if ((s != crds.s || o != crds.o)
                        && s >= 0
                        && s < Aknamezo.GetLength(0)
                        && o >= 0
                        && o < Aknamezo.GetLength(1)
                        && Aknamezo[s, o].Text.Length == 0)
                        kornyezoGombok.Add(Aknamezo[s, o]);
                }
            }
            return kornyezoGombok;
        }

        private int KornyezoAknakSzam((int s, int o) crds)
        {
            int aknakSzama = 0;
            for (int s = crds.s-1; s <= crds.s+1; s++)
            {
                for (int o = crds.o-1; o <= crds.o+1; o++)
                {
                    if ((s != crds.s || o != crds.o)
                        && s >= 0
                        && s < AknaPos.GetLength(0)
                        && o >= 0
                        && o < AknaPos.GetLength(1)
                        && AknaPos[s, o]) aknakSzama++;
                }
            }
            return aknakSzama;
        }

        private (int s, int o) MatrixIndexOf(Button button)
        {
            for (int s = 0; s < Aknamezo.GetLength(0); s++)
            {
                for (int o = 0; o < Aknamezo.GetLength(1); o++)
                {
                    if (Aknamezo[s, o].Equals(button)) return (s, o);
                }
            }
            throw new Exception("error");
        }
    }
}
