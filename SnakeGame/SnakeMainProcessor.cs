using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SnakeGame
{
    public class SnakeMainProcessor
    {
        public System.Drawing.Pen SnakePen;
        public System.Drawing.Pen SnakeHeadPen;
        public System.Drawing.Pen FoodPen;
        public System.Drawing.Brush Border;
        public System.Drawing.Brush BackGround;
        public System.Drawing.Brush WallPen;

        public System.Drawing.Brush TextPen;
        public System.Drawing.Font TextFont;

        private string LevelPath;

        System.Drawing.Point Food;
        public List<System.Drawing.Point> Points = new List<System.Drawing.Point>();
        public List<System.Drawing.Point> Walls = new List<System.Drawing.Point>();
        System.Random RGenerator = new Random();

        private System.Drawing.Size MainWindowSize;

        public int MapSelector = 1;
        public int Score = 0;
        public int MovePosition = 2;
        public int Height = 100;
        public int Width = 100;
        public int Multipiller = 40;
        public int BorderSize = 10;

        public bool IsGameStarted = false;

        public SnakeMainProcessor(int Height, int Width, string FilePath = @"D:\Games\test.bmp")
        {
            this.LevelPath = FilePath;

            this.InitFunction();

            this.SnakePen = new Pen(System.Drawing.Color.White);
            this.SnakeHeadPen = new Pen(System.Drawing.Color.DarkOrchid);
            this.FoodPen = new Pen(System.Drawing.Color.Red);
            this.Border = System.Drawing.Brushes.SlateGray;
            this.BackGround = System.Drawing.Brushes.Tan;
            this.WallPen = System.Drawing.Brushes.Black;
            this.TextPen = System.Drawing.Brushes.White;
            this.TextFont = new System.Drawing.Font(FontFamily.GenericSerif, 16, FontStyle.Regular);
        }
        private void InitFunction()
        {
            this.MovePosition = 2;
            this.Points.Clear();
            this.Walls.Clear();
            this.MainWindowSize = new Size(Width, Height);
            if (this.LevelPath != "")
            {
                Bitmap LevelBMP = new Bitmap(this.LevelPath);
                this.Height = LevelBMP.Height * this.Multipiller;
                this.Width = LevelBMP.Width * this.Multipiller;
                for (var i = 0; i < LevelBMP.Width; ++i)
                {
                    for (var j = 0; j < LevelBMP.Height; ++j)
                    {
                        if (LevelBMP.GetPixel(i, j).GetBrightness() <= 0.5)
                        {
                            this.Walls.Add(new Point(i, j));
                        }
                        else
                        {
                            //var a = LevelBMP.GetPixel(i, j).R;
                            if (LevelBMP.GetPixel(i, j).R > 128 && LevelBMP.GetPixel(i, j).R < 238)
                            {
                                this.Points.Add(new Point(i, j));
                            }
                        }
                    }
                }
            }
            else
            {
                this.Height = ((Height - this.BorderSize * 2) / Multipiller) * this.Multipiller;
                this.Width = ((Width - this.BorderSize * 2) / Multipiller) * this.Multipiller;
            }

            /*for (var i = 0; i < 18; ++i)
            {
                this.Points.Add(new Point(i + 3, 3));
            }*/

            this.GenerateFood();
        }
        public System.Drawing.Bitmap MakePicture()
        {
            Bitmap Map = new Bitmap(this.Width + this.BorderSize * 2 + 1, this.Height + this.BorderSize * 2 + 1);
            using (Graphics Graphic = Graphics.FromImage(Map))
            {
                Graphic.SmoothingMode = SmoothingMode.None;
                Graphic.CompositingQuality = CompositingQuality.HighQuality;
                Graphic.InterpolationMode = InterpolationMode.Default;

                Graphic.FillRectangle(this.Border, 0, 0, this.Width + this.BorderSize * 2 + 1, this.Height + this.BorderSize * 2 + 1);
                Graphic.FillRectangle(this.BackGround, this.BorderSize, this.BorderSize, this.Width + 1, this.Height + 1);
                Graphic.FillRectangle(new System.Drawing.SolidBrush(this.FoodPen.Color), this.Food.X * this.Multipiller + this.BorderSize + 2, this.Food.Y * this.Multipiller + this.BorderSize + 2, this.Multipiller - 4, this.Multipiller - 4);

                foreach (var Item in this.Points.Take(this.Points.Count() - 1))
                {
                    Graphic.FillRectangle(new System.Drawing.SolidBrush(this.SnakePen.Color), this.BorderSize + Item.X * this.Multipiller + 1, this.BorderSize + Item.Y * this.Multipiller + 1, this.Multipiller - 2, this.Multipiller - 2);
                }

                Graphic.FillRectangle(new System.Drawing.SolidBrush(this.SnakeHeadPen.Color), this.BorderSize + this.Points.Last().X * this.Multipiller + 1, this.BorderSize + this.Points.Last().Y * this.Multipiller + 1, this.Multipiller - 2, this.Multipiller - 2);

                foreach (var Item in this.Walls)
                {
                    Graphic.FillRectangle(this.WallPen, this.BorderSize + Item.X * this.Multipiller + 1, this.BorderSize + Item.Y * this.Multipiller + 1, this.Multipiller - 2, this.Multipiller - 2);
                }
            }
            return Map;
        }
        public System.Drawing.Bitmap Tick()
        {
            if (this.IsGameStarted)
            {
                int DelIndex = this.Points.FindIndex(x => x == this.Points.Last());

                if (DelIndex != this.Points.Count() - 1)
                {
                    this.Points.RemoveRange(0, DelIndex + 1);
                    this.Score -= 10;
                }

                this.Points.Add(new Point(this.Points.Last().X + (((this.MovePosition - 1) & 1) == 1 ? this.MovePosition - 1 : 0), this.Points.Last().Y + ((this.MovePosition & 1) == 1 ? this.MovePosition : 0)));
                var Buffer = this.Points.Last();
                //Buffer.Y += Buffer.X > this.Width / this.Multipiller - 1 ? 1 : 0;
                Buffer.X = Buffer.X > this.Width / this.Multipiller - 1 ? 0 : Buffer.X;
                Buffer.X = Buffer.X < 0 ? this.Width / this.Multipiller - 1 : Buffer.X;
                Buffer.Y = Buffer.Y > this.Height / this.Multipiller - 1 ? 0 : Buffer.Y;
                Buffer.Y = Buffer.Y < 0 ? this.Height / this.Multipiller - 1 : Buffer.Y;
                this.Points[this.Points.Count() - 1] = Buffer;

                if (this.Walls.Where(x => x == this.Points.Last()).Count() != 0)
                {
                    return this.MessageLooseContext(this.MainWindowSize.Height, MainWindowSize.Width);
                    //System.Threading.Thread.CurrentThread.Abort();
                }

                if (this.Points.Last() != this.Food)
                {
                    this.Points.RemoveAt(0);
                }
                else
                {
                    this.Score += 1;
                    GenerateFood();
                }
                return this.MakePicture();
            }
            else
            {
                return null;
            }
        }
        public void GenerateFood()
        {
            do
            {
                this.Food.X = RGenerator.Next(0, this.Width / Multipiller);
                this.Food.Y = RGenerator.Next(0, this.Height / Multipiller);
            }
            while (this.Points.Where(x => x == this.Food).Count() != 0 || this.Walls.Where(x => x == this.Food).Count() != 0);
        }
        public void MoveVector(int Key)
        {
            if (Key >= -1 && Key <= 2 && System.Math.Abs(this.MovePosition + 2 - (Key + 2)) != 2)
            {
                this.MovePosition = Key;
            }
        }
        public Bitmap MessageStartContext(int Heigth, int Width)
        {
            Bitmap Message = this.MakePicture(); //new Bitmap(Width, Height);
            using (Graphics Graphic = Graphics.FromImage(Message))
            {
                if (!this.IsGameStarted)
                {
                    var MessageParametrs = System.Windows.Forms.TextRenderer.MeasureText("Press Any Key For Start", this.TextFont);
                    Graphic.DrawString("Press Any Key For Start", this.TextFont, this.TextPen, (Message.Width - MessageParametrs.Width) / 2, (Message.Height - MessageParametrs.Height) / 2);
                }
            }
            return Message;
        }
        public Bitmap MessageLooseContext(int Heigth, int Width)
        {
            IsGameStarted = false;
            InitFunction();
            Bitmap Message = this.MakePicture(); //new Bitmap(Width, Height);
            using (Graphics Graphic = Graphics.FromImage(Message))
            {
                IsGameStarted = false;
                var MessageParametrs = System.Windows.Forms.TextRenderer.MeasureText("You loose! Your score: " + this.Score.ToString(), this.TextFont);
                Graphic.DrawString("You loose! Your score: " + this.Score.ToString(), this.TextFont, this.TextPen, (Message.Width - MessageParametrs.Width) / 2, (Message.Height - MessageParametrs.Height) / 2);
            }
            this.Score = 0;
            return Message;
        }
        public Bitmap MessagePauseContext(int Heigth, int Width)
        {
            IsGameStarted = false;
            Bitmap Message = this.MakePicture(); //new Bitmap(Width, Height);
            using (Graphics Graphic = Graphics.FromImage(Message))
            {
                IsGameStarted = false;
                var MessageParametrs = System.Windows.Forms.TextRenderer.MeasureText("Press Any Key To Continue", this.TextFont);
                Graphic.DrawString("Press Any Key To Continue", this.TextFont, this.TextPen, (Message.Width - MessageParametrs.Width) / 2, (Message.Height - MessageParametrs.Height) / 2);
            }
            return Message;
        }
    }
}