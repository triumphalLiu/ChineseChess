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

        public void Reset()
        {
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
        }

        public int getChessman(int i, int j)
        {
            return chessMan[i][j];
        }

        public void setChessman(int oldi, int oldj, int i, int j)
        {
            chessMan[i][j] = chessMan[oldi][oldj];
            chessMan[oldi][oldj] = 0;
        }
    }
}
