
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;

namespace ChessBoardFigure
{
    public partial class Form1 : Form
    {
        private Rectangle[,] Mas = new Rectangle[8, 8];
        private Rectangle selectedRect1 = Rectangle.Empty;
        private Label coord1Label;
        private Point previousClickPoint = Point.Empty;

        private ComboBox pieceComboBox;
        private string selectedPieceName = null;
       
        public Form1()
        {
            InitializeComponent();
            InitializeRectangles();
            this.MouseClick += Form1_MouseClick;

            coord1Label = new Label()
            {
                Width = 100,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 15),
                Text = $"Cell 1: {GetCellCoordinate(selectedRect1)}",
                Location = new Point(10, 370)
            };
            Controls.Add(coord1Label);
            
            pieceComboBox = new ComboBox
            {
                Width = 100,
                Height = 30,
                Font = new Font("Arial", 12),
                Location = new Point(10, 400)
            };

            pieceComboBox.Items.AddRange(new string[] { "Пешка", "Конь", "Слон", "Ладья", "Ферзь", "Король" });
            Controls.Add(pieceComboBox);

        }
        private const int tileSize = 40;
        private const int gridSize = 8;

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void InitializeRectangles()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle rect = new Rectangle(i * 40, j * 40, 40, 40);
                    Mas[i, j] = rect;
                }
            }
        }
        private void Form1_Paint_1(object sender, PaintEventArgs e)
        {
            bool isBlack = false;
            Pen pen = new Pen(Color.Black);
            Brush brush = new SolidBrush(Color.Black);
            Rectangle rect = new Rectangle();
            Graphics gfx = e.Graphics;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (isBlack)
                    {
                        rect = new Rectangle(i * 40, j * 40, 40, 40);
                        gfx.DrawRectangle(pen, rect);
                        gfx.FillRectangle(brush, rect);
                        isBlack = false;
                    }
                    else
                        isBlack = true;
                }
                isBlack = !isBlack;
            }
            // Добавляем буквенную и цифровую разметку (координаты)
            for (int row = 0; row < gridSize; row++)
            {
                var letterLabel = new Label
                {
                    Width = tileSize,
                    Height = 45,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 10),
                    Text = ((char)('A' + row)).ToString(),
                    Location = new Point(tileSize * row, tileSize * gridSize)
                };
                Controls.Add(letterLabel);

                var numberLabel = new Label
                {
                    Width = 45,
                    Height = tileSize,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 10),
                    Text = (gridSize - row).ToString(),
                    Location = new Point(tileSize * gridSize, tileSize * row)
                };
                Controls.Add(numberLabel);

            }

            coord1Label.Text = selectedRect1.IsEmpty ? string.Empty : $"Cell 1: {GetCellCoordinate(selectedRect1)}";

            if (!string.IsNullOrEmpty(selectedPieceName))
            {
                using (Font font = new Font("Arial", 10, FontStyle.Bold))
                using (Brush pieceBrush = new SolidBrush(Color.Red))
                {
                    e.Graphics.DrawString(selectedPieceName, font, pieceBrush, selectedRect1.Left, selectedRect1.Top);
                }
            }

        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Point clickPoint = e.Location;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (Mas[i, j].Contains(clickPoint))
                    {
                            selectedRect1 = Mas[i, j];
                           
                            if (pieceComboBox.SelectedIndex>=0)
                            {
                                selectedPieceName = pieceComboBox.SelectedItem.ToString();  
                               PlaceChessPiece(selectedPieceName,selectedRect1);
                            }
                            UpdateCoordinateLabels();
                            
                        Invalidate();
                        return;
                    }
                }
            }
            //  если не кликнули на клетку то сброс состояния
            ResetSelection();
            Invalidate();
        }
        private void PlaceChessPiece(string pieceName, Rectangle cell)
        {
            selectedPieceName = pieceName;
            Invalidate();
        }
        private void UpdateCoordinateLabels()
        {
            if (!selectedRect1.IsEmpty)
            {
                coord1Label.Text = $"Cell 1: {GetCellCoordinate(selectedRect1)}";

            }
            else
            {
                coord1Label.Text = string.Empty;
            }
          
        }
        private void ResetSelection()
        {
            selectedRect1 = Rectangle.Empty;
            UpdateCoordinateLabels();
        }
        private string GetCellCoordinate(Rectangle rect)
        {
            int row = rect.Top / tileSize;
            int col = rect.Left / tileSize;
            return $"{(char)('A' + col)}{gridSize - row}";
        }
    }
}