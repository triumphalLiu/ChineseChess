using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseChess
{
    class GameController
    {
        private int[][] chessMan;
        private bool[][] avalChess;
        private bool whosTurn;
        private const int minX = 0, minY = 0, maxX = 8, maxY = 9;

        public void Reset()
        {
            //初始化棋盘
            chessMan = new int[10][];
            for (int i = 0; i < chessMan.Length; ++i)
                chessMan[i] = new int[9];
            for(int i = 0; i < chessMan.Length; ++i)
                for(int j = 0; j < chessMan[i].Length; ++j)
                    switch (i * chessMan[i].Length + j)
                    {
                        case 0:  chessMan[i][j] = -6; break;  case 89: chessMan[i][j] = 6; break;
                        case 1:  chessMan[i][j] = -7; break;  case 88: chessMan[i][j] = 7; break;
                        case 2:  chessMan[i][j] = -5; break;  case 87: chessMan[i][j] = 5; break;
                        case 3:  chessMan[i][j] = -3; break;  case 86: chessMan[i][j] = 3; break;
                        case 4:  chessMan[i][j] = -1; break;  case 85: chessMan[i][j] = 1; break;
                        case 5:  chessMan[i][j] = -3; break;  case 84: chessMan[i][j] = 3; break;
                        case 6:  chessMan[i][j] = -5; break;  case 83: chessMan[i][j] = 5; break;
                        case 7:  chessMan[i][j] = -7; break;  case 82: chessMan[i][j] = 7; break;
                        case 8:  chessMan[i][j] = -6; break;  case 81: chessMan[i][j] = 6; break;
                        case 25: chessMan[i][j] = -4; break;  case 70: chessMan[i][j] = 4; break;
                        case 19: chessMan[i][j] = -4; break;  case 64: chessMan[i][j] = 4; break;
                        case 27: chessMan[i][j] = -2; break;  case 62: chessMan[i][j] = 2; break;
                        case 29: chessMan[i][j] = -2; break;  case 60: chessMan[i][j] = 2; break;
                        case 31: chessMan[i][j] = -2; break;  case 58: chessMan[i][j] = 2; break;
                        case 33: chessMan[i][j] = -2; break;  case 56: chessMan[i][j] = 2; break;
                        case 35: chessMan[i][j] = -2; break;  case 54: chessMan[i][j] = 2; break;
                        default: chessMan[i][j] = 0; break;
                    }
            //初始化先手和可移表
            avalChess = new bool[10][];
            for (int i = 0; i < avalChess.Length; ++i)
                avalChess[i] = new bool[9];
            whosTurn = true;
            TransTurn();
        }

        //获取棋子
        public int GetChessman(int i, int j)
        {
            return chessMan[i][j];
        }
        
        //获取可移动状态
        public bool IsAvaliable(int i, int j)
        {
            return avalChess[i][j];
        }

        //获取路径中棋子个数
        public int GetChessmanCountInPath(int oldi, int oldj, int i, int j)
        {
            int count = 0;
            if(i == oldi)
            {
                int maxer = Math.Max(oldj, j);
                int miner = Math.Min(oldj, j);
                for (int loop = miner + 1; loop < maxer; ++loop)
                    if (chessMan[i][loop] != 0)
                        count++;
            }
            else if(j == oldj)
            {
                int maxer = Math.Max(oldi, i);
                int miner = Math.Min(oldi, i);
                for (int loop = miner + 1; loop < maxer; ++loop)
                    if (chessMan[loop][j] != 0)
                        count++;
            }
            return count;
        }

        public bool CanMove(int oldi, int oldj, int i, int j)
        {
            //是否越界
            if (i < minY || i > maxY || j < minX || j > maxX)
                return false;
            //是否移动到本方棋子上
            if ((chessMan[i][j] > 0 && chessMan[oldi][oldj] > 0) || (chessMan[i][j] < 0 && chessMan[oldi][oldj] < 0))
                return false;
            //是否满足特定棋子的规律
            int type = chessMan[oldi][oldj];
            switch (Math.Abs(type))
            {
                case 0: return false;
                //帅/将 必须在九宫格内，每次只能移动一步
                case 1:
                    //目标仅移动一步
                    if (Math.Abs(i - oldi) + Math.Abs(j - oldj) != 1)
                        return false;
                    //目标在九宫格内
                    if (type < 0 && (i >= 0 && i <= 2 && j >= 3 && j <= 5))
                        return true;
                    else if (type > 0 && (i >= 7 && i <= 9 && j >= 3 && j <= 5))
                        return true;
                    break;
                //兵/卒 只能平移或者向对方移动
                case 2:
                    //目标仅移动一步
                    if (Math.Abs(i - oldi) + Math.Abs(j - oldj) != 1)
                        return false;
                    //目标平移或向对方移动
                    if (type < 0 && (i > oldi || (i == oldi && i >= 5)))
                        return true;
                    if (type > 0 && (i < oldi || (i == oldi && i < 5)))
                        return true;
                    break;
                //仕/士 必须在九宫格内，每次只能斜线一步
                case 3:
                    //目标仅斜线移动一步
                    if (!(Math.Abs(i - oldi) == 1 && Math.Abs(j - oldj) == 1))
                        return false;
                    //目标在九宫格内
                    if (type < 0 && (i >= 0 && i <= 2 && j >= 3 && j <= 5))
                        return true;
                    else if (type > 0 && (i >= 7 && i <= 9 && j >= 3 && j <= 5))
                        return true;
                    break;
                //炮/砲 仅可直线移动，且如果路径中有棋子(有且仅有1个)，必须移动到敌方棋子上
                case 4:
                    //仅可直线移动
                    if (i != oldi && j != oldj)
                        return false;
                    //如果目标为空位，那么路径中不能有棋子，否则必须有1个
                    if (GetChessman(i, j) == 0 && GetChessmanCountInPath(oldi, oldj, i, j) == 0)
                        return true;
                    if (GetChessman(i, j) != 0 && GetChessmanCountInPath(oldi, oldj, i, j) == 1)
                        return true;
                    break;
                //相/象 仅可田字型移动，且不可卡位，且不可过河
                case 5:
                    //不可过河
                    if (type < 0 && i >= 5)
                        return false;
                    if (type > 0 && i < 5)
                        return false;
                    //仅可田字型移动
                    if (!(Math.Abs(i - oldi) == 2 && Math.Abs(j - oldj) == 2))
                        return false;
                    //不可被卡位
                    if (i > oldi && j > oldj && chessMan[oldi + 1][oldj + 1] == 0)
                        return true;
                    if (i > oldi && j < oldj && chessMan[oldi + 1][oldj - 1] == 0)
                        return true;
                    if (i < oldi && j > oldj && chessMan[oldi - 1][oldj + 1] == 0)
                        return true;
                    if (i < oldi && j < oldj && chessMan[oldi - 1][oldj - 1] == 0)
                        return true;
                    break;
                //车/车 仅可直线移动，且路径中不能有棋子
                case 6:
                    //仅可直线移动
                    if (i != oldi && j != oldj)
                        return false;
                    //路径中不能有棋子
                    if (GetChessmanCountInPath(oldi, oldj, i, j) == 0)
                        return true;
                    break;
                //马/马 仅可日字型移动，且不能被卡位
                case 7:
                    //仅可日字型移动
                    if (!((Math.Abs(i - oldi) == 2 && Math.Abs(j - oldj) == 1) || (Math.Abs(i - oldi) == 1 && Math.Abs(j - oldj) == 2)))
                        return false;
                    //不能被卡位
                    if (Math.Abs(i - oldi) == 2)
                    {
                        if (i > oldi && j > oldj && chessMan[oldi + 1][oldj] == 0)
                            return true;
                        if (i > oldi && j < oldj && chessMan[oldi + 1][oldj] == 0)
                            return true;
                        if (i < oldi && j > oldj && chessMan[oldi - 1][oldj] == 0)
                            return true;
                        if (i < oldi && j < oldj && chessMan[oldi - 1][oldj] == 0)
                            return true;
                    }
                    else if(Math.Abs(j - oldj) == 2)
                    {
                        if (i > oldi && j > oldj && chessMan[oldi][oldj + 1] == 0)
                            return true;
                        if (i > oldi && j < oldj && chessMan[oldi][oldj - 1] == 0)
                            return true;
                        if (i < oldi && j > oldj && chessMan[oldi][oldj + 1] == 0)
                            return true;
                        if (i < oldi && j < oldj && chessMan[oldi][oldj - 1] == 0)
                            return true;
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }

        //移动棋子
        public bool SetChessman(int oldi, int oldj, int i, int j)
        {
            if (CanMove(oldi, oldj, i, j) == false) return false;
            chessMan[i][j] = chessMan[oldi][oldj];
            chessMan[oldi][oldj] = 0;
            TransTurn();
            return true;
        }

        //跳过回合，防止TransTurn被滥用
        public void SkipTurn()
        {
            TransTurn();
        }

        //改变回合
        private void TransTurn()
        {
            whosTurn = !whosTurn;
            for (int i = 0; i < avalChess.Length; ++i)
                for (int j = 0; j < avalChess[i].Length; ++j)
                    if (chessMan[i][j] < 0 && whosTurn == false)
                        avalChess[i][j] = true;
                    else if (chessMan[i][j] > 0 && whosTurn == true)
                        avalChess[i][j] = true;
                    else avalChess[i][j] = false;
        }
        
    }
}
