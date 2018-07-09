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
        private GameController gameController;
        private Dictionary<int, Image> chessmanImagePair;
        private int RowSum = 10;
        private int ColSum = 9;

        public MainForm()
        {
            InitializeComponent();
            chessmanImagePair = new Dictionary<int, Image>();
            chessmanImagePair.Add(-7, Properties.Resources.enemy7); chessmanImagePair.Add(7, Properties.Resources.friend7);
            chessmanImagePair.Add(-6, Properties.Resources.enemy6); chessmanImagePair.Add(6, Properties.Resources.friend6);
            chessmanImagePair.Add(-5, Properties.Resources.enemy5); chessmanImagePair.Add(5, Properties.Resources.friend5);
            chessmanImagePair.Add(-4, Properties.Resources.enemy4); chessmanImagePair.Add(4, Properties.Resources.friend4);
            chessmanImagePair.Add(-3, Properties.Resources.enemy3); chessmanImagePair.Add(3, Properties.Resources.friend3);
            chessmanImagePair.Add(-2, Properties.Resources.enemy2); chessmanImagePair.Add(2, Properties.Resources.friend2);
            chessmanImagePair.Add(-1, Properties.Resources.enemy1); chessmanImagePair.Add(1, Properties.Resources.friend1);
            chessmanImagePair.Add(0, null);
        }

        private void ResetAllChessman()
        {
            int index = 0;
            foreach (Control control in this.panelChessman.Controls)
            {
                PictureBox pictureBox = (PictureBox)control;
                pictureBox.Image = chessmanImagePair[gameController.getChessman(index / ColSum, index % ColSum)];
                index++;
            }
        }

        private void ResetAChessman(int lasti, int lastj, int i, int j)
        {
            PictureBox pictureBox = (PictureBox)this.panelChessman.Controls[i * ColSum + j];
            pictureBox.Image = chessmanImagePair[gameController.getChessman(i, j)];
            PictureBox oldPictureBox = (PictureBox)this.panelChessman.Controls[lasti * ColSum + lastj];
            oldPictureBox.Image = chessmanImagePair[0];
        }

        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //如果没有生成过棋盘PictureBox，那么生成10 * 9个
            if (this.panelChessman.Controls.Count == 0)
            {
                gameController = new GameController();
                for (int j = 0; j < RowSum; ++j)
                {
                    for (int i = 0; i < ColSum; ++i)
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
            //重置游戏，重新摆放棋子
            gameController.Reset();
            this.ResetAllChessman();
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
