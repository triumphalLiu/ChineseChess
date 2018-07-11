using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ChineseChess
{
    public partial class MainForm : Form
    {
        private GameController gameController;
        private Dictionary<int, Image> chessmanImagePair;   //储存int-Image的对应
        private const int RowSum = 10;  //行总数
        private const int ColSum = 9;   //列总数
        private int[] lastStep; //记录上一步的动作，用于撤销操作
        private PictureBox currentChosenPictureBox = null;  //当前选中的棋子
        private Image currentChosenImage = null;    //当前选中的棋子的图像，用于闪动效果的实现
        private System.Timers.Timer flickerTimer;   //定时器，用于闪动效果的实现
        private int[][] moveHelper; //用于储存当前选定的棋子可接触、攻击、移动到的新位置状态

        public MainForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.skipToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Enabled = false;
        }

        //右键菜单-新游戏
        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新游戏的初始化操作
            if (this.panelChessman.Controls.Count == 0)
            {
                gameController = new GameController();

                //如果没有生成过棋盘PictureBox，那么生成10 * 9个
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
                        pictureBox.Click += new EventHandler(ClickChessEvent);
                        pictureBox.MouseEnter += new EventHandler(MouseEnterEvent);
                        pictureBox.MouseLeave += new EventHandler(MouseExitEvent);
                        panelChessman.Controls.Add(pictureBox);
                    }
                }

                flickerTimer = new System.Timers.Timer();
                flickerTimer.Elapsed += new ElapsedEventHandler(TimerPictureBoxFlicker);
                flickerTimer.Interval = 500;
                flickerTimer.AutoReset = true;

                //添加编号和Image对应的字典
                chessmanImagePair = new Dictionary<int, Image>();
                chessmanImagePair.Add(-7, Properties.Resources.enemy7); chessmanImagePair.Add(7, Properties.Resources.friend7);
                chessmanImagePair.Add(-6, Properties.Resources.enemy6); chessmanImagePair.Add(6, Properties.Resources.friend6);
                chessmanImagePair.Add(-5, Properties.Resources.enemy5); chessmanImagePair.Add(5, Properties.Resources.friend5);
                chessmanImagePair.Add(-4, Properties.Resources.enemy4); chessmanImagePair.Add(4, Properties.Resources.friend4);
                chessmanImagePair.Add(-3, Properties.Resources.enemy3); chessmanImagePair.Add(3, Properties.Resources.friend3);
                chessmanImagePair.Add(-2, Properties.Resources.enemy2); chessmanImagePair.Add(2, Properties.Resources.friend2);
                chessmanImagePair.Add(-1, Properties.Resources.enemy1); chessmanImagePair.Add(1, Properties.Resources.friend1);
                chessmanImagePair.Add(0, null);
                chessmanImagePair.Add(10, Properties.Resources.green);  chessmanImagePair.Add(-10, Properties.Resources.red);

                //动态数组申请空间
                lastStep = new int[6];
            }

            //重置游戏，重新摆放棋子
            gameController.Reset();
            this.ResetAllChessman();
            //初始化闪动定时器
            DisableFlickerTimer();
            //允许右键菜单
            this.skipToolStripMenuItem.Enabled = true;
            this.undoToolStripMenuItem.Enabled = true;
            //初始化数据
            SetLastStep(-1, -1, -1, -1, -1, -1);
        }

        //重置所有棋子
        private void ResetAllChessman()
        {
            int index = 0;
            foreach (Control control in this.panelChessman.Controls)
            {
                PictureBox pictureBox = (PictureBox)control;
                pictureBox.Image = chessmanImagePair[gameController.GetChessman(index / ColSum, index % ColSum)];
                index++;
            }
        }

        //移动单个棋子
        private bool ResetAChessman(int lasti, int lastj, int i, int j)
        {
            //游戏是否即将结束
            bool gameWillOver = false;
            if (true == gameController.CanMove(lasti, lastj, i, j) && true == gameController.IsGameOver(i, j))
                gameWillOver = true;
            //移动
            SetLastStep(lasti, lastj, i, j, gameController.GetChessman(lasti, lastj), gameController.GetChessman(i, j));
            gameController.SetChessman(lasti, lastj, i, j);
            PictureBox pictureBox = (PictureBox)this.panelChessman.Controls[i * ColSum + j];
            pictureBox.Image = chessmanImagePair[gameController.GetChessman(i, j)];
            PictureBox oldPictureBox = (PictureBox)this.panelChessman.Controls[lasti * ColSum + lastj];
            oldPictureBox.Image = chessmanImagePair[0];
            if (gameWillOver)
            {
                bool winner = gameController.GameOver();
                currentChosenImage = null;
                DisableFlickerTimer();
                this.skipToolStripMenuItem.Enabled = false;
                this.undoToolStripMenuItem.Enabled = false;
                this.Cursor = Cursors.Default;
                MessageBox.Show("游戏结束！" + (winner == false ? "红" : "黑") + "方胜利");
            }
            return true;
        }

        //更新棋盘
        private void UpdateChessPanel()
        {
            for(int i = 0; i < RowSum; ++i)
                for(int j = 0; j < ColSum; ++j)
                    if(gameController.GetMoveHelper(i, j) == 1)
                        ((PictureBox)this.panelChessman.Controls[i * ColSum + j]).BackgroundImage = chessmanImagePair[-10];
                    else if (gameController.GetMoveHelper(i, j) == -1)
                        ((PictureBox)this.panelChessman.Controls[i * ColSum + j]).BackgroundImage = chessmanImagePair[0];
                    else if (gameController.GetMoveHelper(i, j) == 2)
                        ((PictureBox)this.panelChessman.Controls[i * ColSum + j]).BackgroundImage = chessmanImagePair[10];
        }

        //记录上一步的信息
        private void SetLastStep(int lasti, int lastj, int i, int j, int lastValue, int value)
        {
            lastStep[0] = lasti;
            lastStep[1] = lastj;
            lastStep[2] = i;
            lastStep[3] = j;
            lastStep[4] = lastValue;
            lastStep[5] = value;
        }

        //右键菜单-跳过回合
        private void SkipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveHelper();
            DisableFlickerTimer();
            gameController.SkipTurn();
        }

        //右键菜单-撤销
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastStep[0] == -1)
                return;
            RemoveHelper();
            DisableFlickerTimer();
            gameController.ResetChessman(lastStep[0], lastStep[1], lastStep[2], lastStep[3], lastStep[4], lastStep[5]);
            PictureBox pictureBox = (PictureBox)this.panelChessman.Controls[lastStep[2] * ColSum + lastStep[3]];
            pictureBox.Image = chessmanImagePair[lastStep[5]];
            PictureBox oldPictureBox = (PictureBox)this.panelChessman.Controls[lastStep[0] * ColSum + lastStep[1]];
            oldPictureBox.Image = chessmanImagePair[lastStep[4]];
            SetLastStep(-1, -1, -1, -1, -1, -1);
        }

        //右键菜单-退出
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //鼠标移至棋盘事件
        private void MouseEnterEvent(object sender, EventArgs e)
        {
            //获取当前的PictureBox
            PictureBox pictureBox = (PictureBox)sender;
            //当前没有选中且该点为空
            if(currentChosenPictureBox == null && pictureBox.Image == null)
            {
                this.Cursor = Cursors.No;
            }
            //当前没有选中
            else if(currentChosenPictureBox == null)
            {
                int index = 0;
                foreach (Control control in this.panelChessman.Controls)
                {
                    if (pictureBox == (PictureBox)control)
                        break;
                    index++;
                }
                if (gameController.IsAvaliable(index / ColSum, index % ColSum) == false)
                {
                    this.Cursor = Cursors.No;
                }
                else
                {
                    this.Cursor = Cursors.Hand;
                }
            }
            //当前有选中的
            else
            {
                //如果有选中，先判断是不是同一边的，如果是，指针改成Hand
                //如果有选中，那么尝试移动，先获取这些PictureBox的位置
                PictureBox lastPictureBox = currentChosenPictureBox;
                int index = 0, thisIndex = 0, lastIndex = 0;
                foreach (Control control in this.panelChessman.Controls)
                {
                    if (lastPictureBox == (PictureBox)control)
                        lastIndex = index;
                    if (pictureBox == (PictureBox)control)
                        thisIndex = index;
                    index++;
                }
                //尝试移动，查看结果，但是不实际移动
                /*if ((gameController.GetChessman(lastIndex / ColSum, lastIndex % ColSum) < 0 && gameController.GetChessman(thisIndex / ColSum, thisIndex % ColSum) < 0)
                 || (gameController.GetChessman(lastIndex / ColSum, lastIndex % ColSum) > 0 && gameController.GetChessman(thisIndex / ColSum, thisIndex % ColSum) > 0)
                 || true == gameController.CanMove(lastIndex / ColSum, lastIndex % ColSum, thisIndex / ColSum, thisIndex % ColSum))*/
                if(gameController.GetMoveHelper(thisIndex / ColSum, thisIndex % ColSum) != 0)
                    this.Cursor = Cursors.Hand;
                else
                    this.Cursor = Cursors.No;
            }
        }

        //鼠标移出棋盘事件
        private void MouseExitEvent(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        //鼠标点击棋盘事件
        private void ClickChessEvent(object sender, EventArgs e)
        {
            //获取当前的PictureBox
            PictureBox pictureBox = (PictureBox)sender;
            //判断是否已经有选中
            if (currentChosenPictureBox == null && pictureBox.Image == null)
                return;
            if (currentChosenPictureBox == null)
            {
                int index = 0;
                foreach (Control control in this.panelChessman.Controls)
                {
                    if (pictureBox == (PictureBox)control)
                        break;
                    index++;
                }
                if (gameController.IsAvaliable(index / ColSum, index % ColSum))
                {
                    SetHelper(index / ColSum, index % ColSum);
                    currentChosenPictureBox = pictureBox;
                    flickerTimer.Enabled = true;
                }
            }
            else if (currentChosenPictureBox == pictureBox)
            {
                RemoveHelper();
                DisableFlickerTimer();
            }
            else
            {
                //如果有选中，那么尝试移动，先获取这些PictureBox的位置
                PictureBox lastPictureBox = currentChosenPictureBox;
                int index = 0, thisIndex = 0, lastIndex = 0;
                foreach (Control control in this.panelChessman.Controls)
                {
                    if (lastPictureBox == (PictureBox)control)
                        lastIndex = index;
                    if (pictureBox == (PictureBox)control)
                        thisIndex = index;
                    index++;
                }
                //尝试移动，如果可以移动，那么先改变再禁用；如果不可移动，那么先禁用再改变
                //在移动之前移除提示框，防止被覆盖
                RemoveHelper();
                if (true == gameController.CanMove(lastIndex / ColSum, lastIndex % ColSum, thisIndex / ColSum, thisIndex % ColSum))
                {
                    this.ResetAChessman(lastIndex / ColSum, lastIndex % ColSum, thisIndex / ColSum, thisIndex % ColSum);
                    this.Cursor = Cursors.No;
                    currentChosenPictureBox = pictureBox;
                    DisableFlickerTimer();
                }
                else
                {
                    DisableFlickerTimer();
                    currentChosenPictureBox = null;
                }
            }
        }

        //设置提示框
        private void SetHelper(int i, int j)
        {
            gameController.SetMoveHelper(i, j);
            UpdateChessPanel();
        }

        //移除提示框
        private void RemoveHelper()
        {
            gameController.SetMoveHelper(-1, -1);
            UpdateChessPanel();
        }

        //禁用闪动定时器，没有选取的棋子或是选取的棋子已经完成了本次移动
        private void DisableFlickerTimer()
        {
            if(currentChosenImage != null)
                currentChosenPictureBox.Image = currentChosenImage;
            flickerTimer.Enabled = false;
            currentChosenImage = null;
            currentChosenPictureBox = null;
        }

        //控件闪动事件，在Image和NULL之间交替以达到闪动效果
        private void TimerPictureBoxFlicker(object sender, EventArgs e)
        {
            if (currentChosenPictureBox != null && currentChosenImage == null)
                currentChosenImage = currentChosenPictureBox.Image;
            if (currentChosenPictureBox.Image == null)
                currentChosenPictureBox.Image = currentChosenImage;
            else
                currentChosenPictureBox.Image = null;
        }

        //主框架加载函数
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.panelChessman.Parent = this.pictureBoxChessPanel;
        }
    }
}
