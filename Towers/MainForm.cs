using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Towers
{
    public partial class MainForm : Form
    {
        
        
        private CellState[] cells = new CellState[] { CellState.White, CellState.White, CellState.White, CellState.Empty, CellState.Black, CellState.Black, CellState.Black };
        private CellState[] winCells = new CellState[] { CellState.Black, CellState.Black, CellState.Black, CellState.Empty, CellState.White, CellState.White, CellState.White };
        public MainForm()
        {
            InitializeComponent();
            messageText.Text = string.Empty;
        }
        private void DrawCell(int cellIndex, Graphics gr)
        {
            var cellRect = GetCellRectangle(cellIndex);
            //gr.DrawRectangle(Pens.Gray, cellRect);

            if (cells[cellIndex] == CellState.White)
            {
                gr.FillEllipse(Brushes.White, cellRect.X + 2, cellRect.Y + 2, cellRect.Width - 4, cellRect.Height - 4);
                gr.DrawEllipse(Pens.Black, cellRect.X + 2, cellRect.Y + 2, cellRect.Width - 4, cellRect.Height - 4);
            }
            else if (cells[cellIndex] == CellState.Black)
            {
                gr.FillEllipse(Brushes.Black, cellRect.X + 2, cellRect.Y + 2, cellRect.Width - 4, cellRect.Height - 4);
            }



        }
        private Rectangle GetCellRectangle(int cellIndex)
        {
            var cellSize = (ClientSize.Width - 40) / 7;
            var x = 5 + (cellIndex * (cellSize + 5));
            var y = (ClientSize.Height - cellSize-messageText.Height) / 2;
            return new Rectangle(x, y, cellSize, cellSize);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                DrawCell(i, e.Graphics);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
        private int GetEmptyCell(int cellIndex)
        {
            if (cells[cellIndex] == CellState.Empty) return -1;
            if (cells[cellIndex] == CellState.White)
            {
                for (int i = 1; i < 3; i++)
                {
                    if (cellIndex + i >= cells.Length) break;
                    if (cells[cellIndex + i] == CellState.Empty) return cellIndex + i;

                }
                return -1;
            }
            for (int i = 1; i < 3; i++)
            {
                if (cellIndex - i < 0) break;
                if (cells[cellIndex - i] == CellState.Empty) return cellIndex - i;

            }
            return -1;
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                if (GetCellRectangle(i).Contains(e.Location))
                {
                    int newIndex = GetEmptyCell(i);
                    if (newIndex != -1)
                    {
                        cells[newIndex] = cells[i];
                        cells[i] = CellState.Empty;
                        Invalidate();
                    }
                    if (!CheckTurns())
                    {
                        if (string.Join(",", cells.Select(s => s.ToString()).ToArray()) == string.Join(",", winCells.Select(s => s.ToString()).ToArray()))
                            messageText.Text = "Победа";
                        else messageText.Text = "Нет ходов";
                    }
                    return;
                }

            }
        }
        private bool CheckTurns() => Enumerable.Range(0, 7).Any(i => GetEmptyCell(i) != -1);
        

        private void OnReset(object sender, EventArgs e)
        {
            cells = new CellState[] { CellState.White, CellState.White, CellState.White, CellState.Empty, CellState.Black, CellState.Black, CellState.Black };
            messageText.Text = string.Empty;
            Invalidate();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Close();
        }
    }

    public enum CellState
    {
        Empty, White, Black
    }
    
}

