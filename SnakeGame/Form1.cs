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
        public SnakeMainProcessor GameField;
        public Timer GlobalTimer;
        private bool CanMove;
        private Control KeyDownControl;
        public Form1()
        {
            InitializeComponent();
            //this.pictureBox1.Focus();
            this.KeyDownControl = new Control();
            this.KeyDownControl.KeyDown += KeyDownEventHandler;
            this.pictureBox1.Controls.Add(this.KeyDownControl);

            this.GameField = new SnakeMainProcessor(this.pictureBox1.Height, this.pictureBox1.Width);
            this.GlobalTimer = new Timer();
            this.GlobalTimer.Interval = 100;
            this.GlobalTimer.Tick += GlobalTimerElapse;
            this.GlobalTimer.Start();
        }
        public void GlobalTimerElapse(object sender, EventArgs e)
        {
            this.CanMove = true;
            this.pictureBox1.Image = this.GameField.Tick();
        }

        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (CanMove)
            {
                this.CanMove = false;
                this.GameField.MoveVector(e.KeyValue - 74);
            }
        }
    }
}