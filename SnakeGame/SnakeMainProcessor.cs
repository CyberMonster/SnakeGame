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

        System.Drawing.Point Food;
        public List<System.Drawing.Point> Points = new List<System.Drawing.Point>();
        System.Random RGenerator = new Random();

        public int MovePosition = 2;
        public int Height = 50;
        public int Width = 50;
        public int Multipiller = 10;
        public int BorderSize = 20;

        public SnakeMainProcessor(int Height, int Width)
        {
            this.Height = ((Height - this.BorderSize * 2) / Multipiller) * this.Multipiller;
            this.Width = ((Width - this.BorderSize * 2) / Multipiller) * this.Multipiller;
            this.SnakePen = new Pen(System.Drawing.Color.White);
            this.SnakeHeadPen = new Pen(System.Drawing.Color.DarkOrchid);
            this.FoodPen = new Pen(System.Drawing.Color.Red);
            this.Border = System.Drawing.Brushes.SlateGray;
            this.BackGround = System.Drawing.Brushes.Tan;
            for (var i = 0; i < 3; ++i)
            {
                this.Points.Add(new Point(i, 0));
            }
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
                Graphic.DrawRectangle(this.FoodPen, this.Food.X * this.Multipiller + this.BorderSize + 2, this.Food.Y * this.Multipiller + this.BorderSize + 2, this.Multipiller - 4, this.Multipiller - 4);
                for (var i = 0; i < this.Width; i += this.Multipiller)
                {
                    for (var j = 0; j < this.Height; j += this.Multipiller)
                    {
                        if (this.Points.Where(x => x.X == (i / this.Multipiller) && x.Y == (j / this.Multipiller)).Count() > 0)
                        {
                            Graphic.DrawRectangle(this.SnakeHeadPen, i + this.BorderSize + 1, j + this.BorderSize + 1, Multipiller - 2, Multipiller - 2);
                        }
                    }
                }
            }
            return Map;
        }
        public System.Drawing.Bitmap Tick()
        {
            this.Points.Add(new Point(this.Points.Last().X + (((this.MovePosition - 1) & 1) == 1 ? this.MovePosition - 1 : 0), this.Points.Last().Y + ((this.MovePosition & 1) == 1 ? this.MovePosition : 0)));
            var Buffer = this.Points.Last();
            Buffer.X = Buffer.X > this.Width / this.Multipiller - 1 ? 0 : Buffer.X;
            Buffer.X = Buffer.X < 0 ? this.Width / this.Multipiller - 1 : Buffer.X;
            Buffer.Y = Buffer.Y > this.Height / this.Multipiller - 1 ? 0 : Buffer.Y;
            Buffer.Y = Buffer.Y < 0 ? this.Height / this.Multipiller - 1 : Buffer.Y;
            this.Points[this.Points.Count() - 1] = Buffer;

            int DelIndex = this.Points.FindIndex(x => x == this.Points.Last());

            if (DelIndex != this.Points.Count() - 1)
            {
                this.Points.RemoveRange(0, DelIndex);
            }

            if (this.Points.Last() != this.Food)
            {
                this.Points.RemoveAt(0);
            }
            else
            {
                GenerateFood();
            }
            return this.MakePicture();
        }
        public void GenerateFood()
        {
            do
            {
                this.Food.X = RGenerator.Next(0, this.Width / Multipiller);
                this.Food.Y = RGenerator.Next(0, this.Height / Multipiller);
            }
            while (this.Points.Where(x => x == this.Food).Count() != 0);
        }
        public void MoveVector(int Key)
        {
            if (Key >= -1 && Key <= 2 && System.Math.Abs(this.MovePosition + 2 - (Key + 2)) != 2)
            {
                this.MovePosition = Key;
            }
        }
    }
}