using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joc.Library
{
    public class Board : Constants
    {
        public object[,] board;
        public int red_left, black_left, red_kings, black_kings;

        public Board()
        {
            board = new object[ROWS, COLS];
            red_left = 12;
            black_left = 12;
            red_kings = 0;
            black_kings = 0;
            create_board();
        }

        public Board(string empty)
        {
            board = new object[ROWS, COLS];
            red_left = 12;
            black_left = 12;
            red_kings = 0;
            black_kings = 0;
            
        }

        public void changePiece(int i, int j, object ob)
        {
            board[i, j] = ob;
        }

        public void create_board()
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    if (col % 2 == (row + 1) % 2)
                    {
                        if (row < 3)
                        {
                            this.board[row, col] = new Piece(row, col, BLACK);
                        }
                        else if (row > 4)
                        {
                            this.board[row, col] = new Piece(row, col, RED);
                        }
                        else
                        {
                            this.board[row, col] = 0;
                        }
                    }
                    else
                        this.board[row, col] = 0;
                }
            }
        }

        // public void draw_square() se face in clasa Game.1 din Monogame

        public object get_piece(int row, int col)
        {
            return board[row, col];
        }

        public void move(object[,] board, Piece piece, int row, int col)
        {
            var temp = board[piece.row, piece.col];
            board[piece.row, piece.col] = board[row, col];
            board[row, col] = temp;
            piece.move(row, col);

            if ((row == ROWS - 1) || (row == 0))
            {
                piece.make_King();
                if (piece.color == RED)  // Rosu sau negru
                {
                    this.red_kings++;
                }
                else
                {
                    black_kings++;
                }

            }
        }

        public void remove(List<Piece> pieces)
        {
            foreach (object piece in pieces)
            {
                Random random = new Random();
                var randomNumber = random.Next(1, 10);
                if (piece is Piece && ((Piece)piece).shield && randomNumber > 4 && randomNumber < 7)
                {
                    ((Piece)piece).shield = false;
                }
                else
                {
                    if (piece is Piece)
                    {
                        if (((Piece)piece).color == RED)
                        {
                            red_left -= 1;
                        }
                        else
                        {
                            black_left -= 1;
                        }
                        board[((Piece)piece).row, ((Piece)piece).col] = 0;
                    }

                }
            }
        }

        public Color winner()
        {
            if (black_left <= 0)
            {
                return BLACK;
            }
            else if (red_left <= 0)
            {
                return RED;
            }
            return BLUE;
        }

        private Dictionary<(int, int), List<Piece>> TraverseLeft(int start, int stop, int step, Color color, int left, List<Piece> skipped = null)
        {
            Dictionary<(int, int), List<Piece>> moves = new Dictionary<(int, int), List<Piece>>();
            List<Piece> last = new List<Piece>();

            for (int r = start; r < stop; r += step)
            {
                if (left < 0)
                    break;

                if (board[r,left] is int)
                {
                    continue;

                }

                Piece current = (Piece)board[r, left];
                if (current == null)
                {
                    if (skipped != null && last.Count == 0)
                        break;
                    else if (skipped != null)
                        moves.Add((r, left), last.Concat(skipped).ToList());
                    else
                        moves.Add((r, left), last);

                    if (last.Count > 0)
                    {
                        int row = (step == -1) ? Math.Max(r - 3, 0) : Math.Min(r + 3, ROWS);
                        moves = moves.Concat(TraverseLeft(r + step, row, step, color, left - 1, last)).ToDictionary(x => x.Key, x => x.Value);
                        moves = moves.Concat(TraverseRight(r + step, row, step, color, left + 1, last)).ToDictionary(x => x.Key, x => x.Value);
                    }
                    break;
                }
                else if (current.color == color)
                {
                    break;
                }
                else
                {
                    last = new List<Piece> { current };
                }

                left--;
            }

            return moves;
        }

        private Dictionary<(int, int), List<Piece>> TraverseRight(int start, int stop, int step, Color color, int right, List<Piece> skipped = null)
        {
            Dictionary<(int, int), List<Piece>> moves = new Dictionary<(int, int), List<Piece>>();
            List<Piece> last = new List<Piece>();

            for (int r = start; r < stop; r += step)
            {
                if (right >= ROWS)
                    break;

                if (board[r, right] is int)
                {
                    continue;
                }

                Piece current = (Piece)board[r, right];
                if (current == null)
                {
                    if (skipped != null && last.Count == 0)
                        break;
                    else if (skipped != null)
                        moves.Add((r, right), last.Concat(skipped).ToList());
                    else
                        moves.Add((r, right), last);

                    if (last.Count > 0)
                    {
                        int row = (step == -1) ? Math.Max(r - 3, 0) : Math.Min(r + 3, ROWS);
                        moves = moves.Concat(TraverseLeft(r + step, row, step, color, right - 1, last)).ToDictionary(x => x.Key, x => x.Value);
                        moves = moves.Concat(TraverseRight(r + step, row, step, color, right + 1, last)).ToDictionary(x => x.Key, x => x.Value);
                    }
                    break;
                }
                else if (current.color == color)
                {
                    break;
                }
                else
                {
                    last = new List<Piece> { current };
                }

                right++;
            }

            return moves;
        }

        public Dictionary<(int, int), List<Piece>> GetValidMoves(Piece piece)
        {
            Dictionary<(int, int), List<Piece>> moves = new Dictionary<(int, int), List<Piece>>();
            int left = piece.col - 1;
            int right = piece.col + 1;
            int row = piece.row;

            if (piece.color == RED || piece.king)
            {
                moves = moves.Concat(TraverseLeft(row - 1, Math.Max(row - 3, -1), -1, piece.color, left)).ToDictionary(x => x.Key, x => x.Value);
                moves = moves.Concat(TraverseRight(row - 1, Math.Max(row - 3, -1), -1, piece.color, right)).ToDictionary(x => x.Key, x => x.Value);
            }
            if (piece.color == BLACK || piece.king)
            {
                moves = moves.Concat(TraverseLeft(row + 1, Math.Min(row + 3, ROWS), 1, piece.color, left)).ToDictionary(x => x.Key, x => x.Value);
                moves = moves.Concat(TraverseRight(row + 1, Math.Min(row + 3, ROWS), 1, piece.color, right)).ToDictionary(x => x.Key, x => x.Value);
            }
            return moves;
        }
    }
}