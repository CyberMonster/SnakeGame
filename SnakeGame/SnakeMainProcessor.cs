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
        public System.Drawing.Pen WallPen;

        public System.Drawing.Brush TextPen;
        public System.Drawing.Font TextFont;

        private string LevelPath;
        private int DelIndex;

        System.Drawing.Point Food;
        public List<System.Drawing.Point> Points = new List<System.Drawing.Point>();
        public List<System.Drawing.Point> Walls = new List<System.Drawing.Point>();
        public List<bool> MainSettings;
        System.Random RGenerator = new Random();

        private System.Drawing.Size MainWindowSize;
        private System.Drawing.Bitmap LevelBMP;

        public int MapSelector = 1;
        public int Score = 0;
        public int MovePosition = 2;
        public int Height = 100;
        public int Width = 100;
        public int Multipiller = 40;
        public int BorderSize = 10;

        private System.Drawing.Point PointBuffer;
        private System.Drawing.Size MessageSize;

        public bool IsGameStarted = false;

        public SnakeMainProcessor(int Height, int Width, System.Drawing.Color [] Colors, List<bool> Settings, System.Drawing.Font TextFont, string FilePath = @"C:\Users\Олег\Documents\Visual Studio 2017\Projects\SnakeGame\Anton.bm")
        {
            this.MainWindowSize = new Size(Width, Height);
            if (TextFont == null)
            {
                TextFont = new System.Drawing.Font(FontFamily.GenericSerif, 16, FontStyle.Regular);
            }

            if (Settings == null)
            {
                this.MainSettings = new List<bool>();
                for (var i = 0; i < 10; ++i)
                {
                    this.MainSettings.Add(true);
                }
            }
            else
            {
                this.MainSettings = Settings;
            }

            this.LevelPath = FilePath;

            if (Colors == null)
            {
                this.SnakePen = new Pen(System.Drawing.Color.White);
                this.SnakeHeadPen = new Pen(System.Drawing.Color.DarkOrchid);
                this.WallPen = new Pen(System.Drawing.Color.Black);
                this.BackGround = new System.Drawing.SolidBrush(System.Drawing.Color.Tan);
                this.FoodPen = new Pen(System.Drawing.Color.Red);
                this.Border = new System.Drawing.SolidBrush(System.Drawing.Color.SlateGray);
                this.TextPen = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            }
            else
            {
                this.SnakePen = new Pen(Colors[0]);
                this.SnakeHeadPen = new Pen(Colors[1]);
                this.WallPen = new Pen(Colors[2]);
                this.BackGround = new System.Drawing.SolidBrush(Colors[3]);
                this.FoodPen = new Pen(Colors[4]);
                this.Border = new System.Drawing.SolidBrush(Colors[5]);
                this.TextPen = new System.Drawing.SolidBrush(Colors[6]);
            }

            this.TextFont = TextFont;

            this.InitFunction();
        }
        private void InitFunction()
        {
            this.MovePosition = 2;
            this.Points.Clear();
            this.Walls.Clear();
            //this.MainWindowSize = new Size(Width, Height);
            if (this.LevelPath != "" && System.IO.File.Exists(this.LevelPath))
            {
                this.LevelBMP = new Bitmap(this.LevelPath);
                this.Height = this.LevelBMP.Height * this.Multipiller;
                this.Width = this.LevelBMP.Width * this.Multipiller;
                for (var i = 0; i < LevelBMP.Width; ++i)
                {
                    for (var j = 0; j < this.LevelBMP.Height; ++j)
                    {
                        if (this.LevelBMP.GetPixel(i, j).GetBrightness() <= 0.5)
                        {
                            this.Walls.Add(new Point(i, j));
                        }
                        else
                        {
                            //var a = LevelBMP.GetPixel(i, j).R;
                            if (this.LevelBMP.GetPixel(i, j).R > 128 && this.LevelBMP.GetPixel(i, j).R < 238)
                            {
                                this.Points.Add(new Point(i, j));
                            }
                        }
                    }
                }
            }
            else
            {
                this.Height = ((this.MainWindowSize.Height - this.BorderSize * 2) / this.Multipiller) * this.Multipiller;
                this.Width = ((this.MainWindowSize.Width - this.BorderSize * 2) / this.Multipiller) * this.Multipiller;
                for (var i = 0; i < 3; ++i)
                {
                    this.Points.Add(new Point(i, 0));
                }
            }

            this.GenerateFood();
        }
        public System.Drawing.Bitmap MakePicture()
        {
            this.LevelBMP = new Bitmap(this.Width + this.BorderSize * 2 + 1, this.Height + this.BorderSize * 2 + 1);
            using (Graphics Graphic = Graphics.FromImage(this.LevelBMP))
            {
                Graphic.SmoothingMode = SmoothingMode.None;
                Graphic.CompositingQuality = CompositingQuality.HighQuality;
                Graphic.InterpolationMode = InterpolationMode.Default;

                if (this.MainSettings[9])
                {
                    Graphic.FillRectangle(this.Border, 0, 0, this.Width + this.BorderSize * 2 + 1, this.Height + this.BorderSize * 2 + 1);
                }
                Graphic.FillRectangle(this.BackGround, this.BorderSize, this.BorderSize, this.Width + 1, this.Height + 1);

                this.SDraw(Graphic, this.MainSettings[7], this.MainSettings[6], this.FoodPen, this.Food.X * this.Multipiller + this.BorderSize + 2, this.Food.Y * this.Multipiller + this.BorderSize + 2, this.Multipiller - 4, this.Multipiller - 4);

                foreach (var Item in this.Points.Take(this.Points.Count() - 1))
                {
                    this.SDraw(Graphic, this.MainSettings[7], this.MainSettings[3], this.SnakePen, this.BorderSize + Item.X * this.Multipiller + 1, this.BorderSize + Item.Y * this.Multipiller + 1, this.Multipiller - 2, this.Multipiller - 2);
                }

                this.SDraw(Graphic, this.MainSettings[7], this.MainSettings[4], this.SnakeHeadPen, this.BorderSize + this.Points.Last().X * this.Multipiller + 1, this.BorderSize + this.Points.Last().Y * this.Multipiller + 1, this.Multipiller - 2, this.Multipiller - 2);

                foreach (var Item in this.Walls)
                {
                    this.SDraw(Graphic, this.MainSettings[7], this.MainSettings[5], this.WallPen, this.BorderSize + Item.X * this.Multipiller + 1, this.BorderSize + Item.Y * this.Multipiller + 1, this.Multipiller - 2, this.Multipiller - 2);
                }
            }
            return this.LevelBMP;
        }
        public System.Drawing.Bitmap Tick()
        {
            if (this.IsGameStarted)
            {
                this.DelIndex = this.Points.FindIndex(x => x == this.Points.Last());

                if (this.DelIndex != this.Points.Count() - 1)
                {
                    if (!this.MainSettings[0])
                    {
                        this.Points.RemoveRange(0, this.DelIndex + 1);
                        this.Score -= 10;
                    }
                    else
                    {
                        return this.MessageLooseContext(this.MainWindowSize.Height, MainWindowSize.Width);
                    }
                }

                this.Points.Add(new Point(this.Points.Last().X + (((this.MovePosition - 1) & 1) == 1 ? this.MovePosition - 1 : 0), this.Points.Last().Y + ((this.MovePosition & 1) == 1 ? this.MovePosition : 0)));
                this.PointBuffer = this.Points.Last();
                //this.PointBuffer.Y += this.PointBuffer.X > this.Width / this.Multipiller - 1 ? 1 : 0;
                this.PointBuffer.X = this.PointBuffer.X > this.Width / this.Multipiller - 1 ? 0 : this.PointBuffer.X;
                this.PointBuffer.X = this.PointBuffer.X < 0 ? this.Width / this.Multipiller - 1 : this.PointBuffer.X;
                this.PointBuffer.Y = this.PointBuffer.Y > this.Height / this.Multipiller - 1 ? 0 : this.PointBuffer.Y;
                this.PointBuffer.Y = this.PointBuffer.Y < 0 ? this.Height / this.Multipiller - 1 : this.PointBuffer.Y;

                this.Points[this.Points.Count() - 1] = this.PointBuffer;

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
                    this.GenerateFood();
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
            this.LevelBMP = this.MakePicture(); //new Bitmap(Width, Height);
            using (Graphics Graphic = Graphics.FromImage(this.LevelBMP))
            {
                if (!this.IsGameStarted)
                {
                    this.MessageSize = System.Windows.Forms.TextRenderer.MeasureText("Press Any Key For Start", this.TextFont);
                    Graphic.DrawString("Press Any Key For Start", this.TextFont, this.TextPen, (this.LevelBMP.Width - this.MessageSize.Width) / 2, (this.LevelBMP.Height - this.MessageSize.Height) / 2);
                }
            }
            return this.LevelBMP;
        }
        public Bitmap MessageLooseContext(int Heigth, int Width)
        {
            this.IsGameStarted = false;
            this.InitFunction();
            this.LevelBMP = this.MakePicture(); //new Bitmap(Width, Height);
            using (Graphics Graphic = Graphics.FromImage(this.LevelBMP))
            {
                this.IsGameStarted = false;
                this.MessageSize = System.Windows.Forms.TextRenderer.MeasureText("You loose! Your score: " + this.Score.ToString(), this.TextFont);
                Graphic.DrawString("You loose! Your score: " + this.Score.ToString(), this.TextFont, this.TextPen, (this.LevelBMP.Width - this.MessageSize.Width) / 2, (this.LevelBMP.Height - this.MessageSize.Height) / 2);
            }
            this.Score = 0;
            return this.LevelBMP;
        }
        public Bitmap MessagePauseContext(int Heigth, int Width)
        {
            this.IsGameStarted = false;
            this.LevelBMP = this.MakePicture(); //new Bitmap(Width, Height);
            using (Graphics Graphic = Graphics.FromImage(this.LevelBMP))
            {
                this.IsGameStarted = false;
                this.MessageSize = System.Windows.Forms.TextRenderer.MeasureText("Press Any Key To Continue", this.TextFont);
                Graphic.DrawString("Press Any Key To Continue", this.TextFont, this.TextPen, (this.LevelBMP.Width - this.MessageSize.Width) / 2, (this.LevelBMP.Height - this.MessageSize.Height) / 2);
            }
            return this.LevelBMP;
        }
        private void SDraw(System.Drawing.Graphics Gr, bool b1, bool b2, Pen Pn, int x, int y, int w, int h)
        {
            if (b1)
            {
                if (b2)
                {
                    Gr.FillEllipse(new System.Drawing.SolidBrush(Pn.Color), x, y, w, h); 
                }
                else
                {
                    Gr.FillRectangle(new System.Drawing.SolidBrush(Pn.Color), x, y, w, h);
                }
            }
            else
            {
                if (b2)
                {
                    Gr.DrawEllipse(Pn, x, y, w, h); 
                }
                else
                {
                    Gr.DrawRectangle(Pn, x, y, w, h);
                }
            }
        }
        private void SDraw(System.Drawing.Graphics Gr, bool b, Pen Pn, int x, int y, int w, int h)
        {
            if (b)
            {
                Gr.FillRectangle(new System.Drawing.SolidBrush(Pn.Color), x, y, w, h);
            }
            else
            {
                Gr.DrawRectangle(Pn, x, y, w, h);
            }
        }
        private void SDrawS(System.Drawing.Graphics Gr, bool b, Pen Pn, int x, int y, int w, int h)
        {
            if (b)
            {
                Gr.FillEllipse(new System.Drawing.SolidBrush(Pn.Color), x, y, w, h);
            }
            else
            {
                Gr.DrawEllipse(Pn, x, y, w, h);
            }
        }
    }
}