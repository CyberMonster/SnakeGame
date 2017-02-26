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
    public partial class GamingForm : Form
    {
        public Random Rnd_Gen;
        public SnakeMainProcessor GameField;
        public Timer GlobalTimer;
        private bool CanMove = true;
        private Control KeyDownControl;
        private ProgramThreads ThreadsManager;
        public GamingForm(ref ProgramThreads ThreadStack, List<System.Drawing.Color> Colors, List<bool> Settings, int Multipiller, System.Drawing.Font GameFont, int Interval, string Path)//int Interval, string MapPath, bool Mode, Color[] Colors)
        {
            this.ThreadsManager = ThreadStack;
            InitializeComponent();
            Rnd_Gen = new Random();
            this.KeyDownControl = new Control();
            this.KeyDownControl.KeyDown += KeyDownEventHandler;
            this.pictureBox1.Controls.Add(this.KeyDownControl);
            this.GlobalTimer = new Timer();
            this.GlobalTimer.Tick += GlobalTimerElapse;
            this.GlobalTimer.Interval = Interval;
            this.pictureBox1.Focus();
            this.GlobalTimer.Stop();
            this.GameField = new SnakeMainProcessor(this.pictureBox1.Height, this.pictureBox1.Width, Multipiller, Colors.ToArray(), Settings, GameFont, Path);
            this.pictureBox1.Image = this.GameField.MessageStartContext(this.pictureBox1.Height, this.pictureBox1.Width);
            this.GlobalTimer.Start();
        }

        public void GlobalTimerElapse(object sender, EventArgs e)
        {
            if (this.GameField.IsGameStarted)
            {
                this.CanMove = true;
                if (this.GameField.KeysStack.Count() != 0)
                {
                    this.GameField.MoveVector(this.GameField.KeysStack.First());
                    this.GameField.KeysStack.RemoveAt(0);
                    if (this.GameField.KeysStack.Count() != 0)
                    {
                        this.CanMove = false;
                    }
                }
                if (this.GameField.MainSettings[15])
                KeyGenerator();
                this.pictureBox1.SuspendLayout();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image = this.GameField.Tick();
                this.pictureBox1.ResumeLayout();
                this.Text = "Ваш счет: " + this.GameField.Score.ToString();
            }
        }

        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                this.pictureBox1.SuspendLayout();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image = this.GameField.MessagePauseContext(this.pictureBox1.Height, this.pictureBox1.Width);
                this.pictureBox1.ResumeLayout();
                //this.GameField.IsGameStarted = false;
            }
            else
            {
                if (CanMove)
                {
                    this.CanMove = false;
                    this.GameField.KeysStack.Add(e.KeyValue - 74);
                }
                else
                {
                    if (this.GameField.MainSettings[2])
                    {
                        this.GameField.KeysStack.Add(e.KeyValue - 74);
                    }
                }
                this.GameField.IsGameStarted = true;
            }
        }
        private void KeyGenerator()
        {
            this.CanMove = false;
            this.GameField.MoveVector(this.Rnd_Gen.Next(-1, 3));
        }

        private void GamingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ThreadsManager.KillMainThread();
        }
    }
}