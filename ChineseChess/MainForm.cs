using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChineseChess
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.panelChessman.Controls.Count == 0)
            {
                for (int j = 0; j < 10; ++j)
                {
                    for (int i = 0; i < 9; ++i)
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Size = new Size(70, 70);
                        if (j < 5)
                            pictureBox.Location = new Point(i * 83, 10 + j * 83);
                        else if (j >= 5)
                            pictureBox.Location = new Point(i * 83, 15 + j * 83);
                        pictureBox.Parent = panelChessman;
                        pictureBox.BackColor = Color.Transparent;
                        panelChessman.Controls.Add(pictureBox);
                    }
                }
            }
            int index = 0;
            foreach (Control control in this.panelChessman.Controls)
            {
                PictureBox pictureBox = (PictureBox)control;
                switch (index)
                {
                    case 0: pictureBox.Image = Properties.Resources.enemy6; break;
                    case 1: pictureBox.Image = Properties.Resources.enemy7; break;
                    case 2: pictureBox.Image = Properties.Resources.enemy5; break;
                    case 3: pictureBox.Image = Properties.Resources.enemy3; break;
                    case 4: pictureBox.Image = Properties.Resources.enemy1; break;
                    case 5: pictureBox.Image = Properties.Resources.enemy3; break;
                    case 6: pictureBox.Image = Properties.Resources.enemy5; break;
                    case 7: pictureBox.Image = Properties.Resources.enemy7; break;
                    case 8: pictureBox.Image = Properties.Resources.enemy6; break;
                    case 89: pictureBox.Image = Properties.Resources.friend6; break;
                    case 88: pictureBox.Image = Properties.Resources.friend7; break;
                    case 87: pictureBox.Image = Properties.Resources.friend5; break;
                    case 86: pictureBox.Image = Properties.Resources.friend3; break;
                    case 85: pictureBox.Image = Properties.Resources.friend1; break;
                    case 84: pictureBox.Image = Properties.Resources.friend3; break;
                    case 83: pictureBox.Image = Properties.Resources.friend5; break;
                    case 82: pictureBox.Image = Properties.Resources.friend7; break;
                    case 81: pictureBox.Image = Properties.Resources.friend6; break;
                    case 19: pictureBox.Image = Properties.Resources.enemy4; break;
                    case 25: pictureBox.Image = Properties.Resources.enemy4; break;
                    case 27: pictureBox.Image = Properties.Resources.enemy2; break;
                    case 29: pictureBox.Image = Properties.Resources.enemy2; break;
                    case 31: pictureBox.Image = Properties.Resources.enemy2; break;
                    case 33: pictureBox.Image = Properties.Resources.enemy2; break;
                    case 35: pictureBox.Image = Properties.Resources.enemy2; break;
                    case 70: pictureBox.Image = Properties.Resources.friend4; break;
                    case 64: pictureBox.Image = Properties.Resources.friend4; break;
                    case 62: pictureBox.Image = Properties.Resources.friend2; break;
                    case 60: pictureBox.Image = Properties.Resources.friend2; break;
                    case 58: pictureBox.Image = Properties.Resources.friend2; break;
                    case 56: pictureBox.Image = Properties.Resources.friend2; break;
                    case 54: pictureBox.Image = Properties.Resources.friend2; break;
                    default: pictureBox.Image = null;break;

                }
                ++index;
            }
        }

        private void SkipToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.panelChessman.Parent = this.pictureBoxChessPanel;
        }
    }
}
