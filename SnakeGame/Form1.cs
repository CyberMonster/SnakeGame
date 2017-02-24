using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        public Random Rnd_Gen;
        public SnakeMainProcessor GameField;
        public Timer GlobalTimer;
        private bool CanMove = false;
        private Control KeyDownControl;
        public Form1()
        {
            InitializeComponent();
            Rnd_Gen = new Random();
            this.KeyDownControl = new Control();
            this.KeyDownControl.KeyDown += KeyDownEventHandler;
            this.pictureBox1.Controls.Add(this.KeyDownControl);
            this.GlobalTimer = new Timer();
            this.GlobalTimer.Tick += GlobalTimerElapse;
            this.GlobalTimer.Interval = 150;
            this.pictureBox1.Focus();
            this.GlobalTimer.Stop();
            this.GameField = new SnakeMainProcessor(this.pictureBox1.Height, this.pictureBox1.Width);
            this.pictureBox1.Image = this.GameField.MessageStartContext(this.pictureBox1.Height, this.pictureBox1.Width);
            this.GlobalTimer.Start();
        }

        public void GlobalTimerElapse(object sender, EventArgs e)
        {
            if (this.GameField.IsGameStarted)
            {
                this.CanMove = true;
                //KeyGenerator();
                this.pictureBox1.Image = this.GameField.Tick();
                this.Text = "Ваш счет: " + this.GameField.Score.ToString();
            }
        }

        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.P)
            {
                this.pictureBox1.Image = this.GameField.MessagePauseContext(this.pictureBox1.Height, this.pictureBox1.Width);
                //this.GameField.IsGameStarted = false;
            }
            else
            {
                if (CanMove)
                {
                    this.CanMove = false;
                    this.GameField.MoveVector(e.KeyValue - 74);
                }
                this.GameField.IsGameStarted = true;
            }
        }
        private void KeyGenerator()
        {
            this.CanMove = false;
            this.GameField.MoveVector(this.Rnd_Gen.Next(-1, 3));
        }
    }
}