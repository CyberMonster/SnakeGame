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
    public partial class SettingsForm : Form
    {
        private SnakeMainProcessor Game;
        private ProgramThreads ThreadsManager;
        private List<System.Drawing.Color> Colors;
        private List<bool> SettingsList;
        private System.Drawing.Font GameFont;
        public SettingsForm()//System.Threading.Tasks.Task Task_)
        {
            this.ThreadsManager = new ProgramThreads();
            this.Colors = new List<Color>();
            //this.ThreadsManager.AddTask(Task_);
            InitializeComponent();
            this.SetColors();
            this.openFileDialog1.InitialDirectory = /*System.Environment.CurrentDirectory*/ @"C:\Users\Олег\Documents\Visual Studio 2017\Projects\SnakeGame\" + @"PrevPicture.bmp";
            this.openFileDialog1.FileName = this.openFileDialog1.InitialDirectory;
            openFileDialog1_FileOk(this.Game, new CancelEventArgs());
        }

        private void SetColors()
        {
            this.button2.BackColor = System.Drawing.Color.White;
            this.button3.BackColor = System.Drawing.Color.Orchid;
            this.button4.BackColor = System.Drawing.Color.Black;
            this.button5.BackColor = System.Drawing.Color.Tan;
            this.button6.BackColor = System.Drawing.Color.Red;
            this.button9.BackColor = System.Drawing.Color.Maroon;
            this.button10.BackColor = System.Drawing.Color.White;
        }

        private void SetSettings()
        {
            this.SettingsList = new List<bool>();
            this.SettingsList.Add(this.checkBox1.Checked);
            this.SettingsList.Add(this.checkBox2.Checked);
            this.SettingsList.Add(this.checkBox3.Checked);
            this.SettingsList.Add(this.checkBox4.Checked);
            this.SettingsList.Add(this.checkBox5.Checked);
            this.SettingsList.Add(this.checkBox6.Checked);
            this.SettingsList.Add(this.checkBox7.Checked);
            this.SettingsList.Add(this.checkBox8.Checked);
            this.SettingsList.Add(this.checkBox9.Checked);
            this.SettingsList.Add(this.checkBox10.Checked);
            //this.SettingsList.Add(this.checkBox11.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ReloadDemoPicture();
            var GamingFormTask = new System.Threading.Tasks.Task(() =>
            {
                var Context = new ApplicationContext(new GamingForm(ref this.ThreadsManager, this.Colors, this.SettingsList, this.GameFont, (int)this.numericUpDown1.Value)).MainForm;
                Application.Run(Context);
            });
            this.ThreadsManager.AddTask(GamingFormTask);
            GamingFormTask.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ThreadsManager.KillMainThread();
            this.ThreadsManager.SettingsClosed = 0;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.ReloadDemoPicture();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.button2.BackColor = GetColorFromUser();
            this.ReloadDemoPicture();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.button3.BackColor = GetColorFromUser();
            this.ReloadDemoPicture();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.button4.BackColor = GetColorFromUser();
            this.ReloadDemoPicture();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.button5.BackColor = GetColorFromUser();
            this.ReloadDemoPicture();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.button6.BackColor = GetColorFromUser();
            this.ReloadDemoPicture();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.button9.BackColor = GetColorFromUser();
            this.ReloadDemoPicture();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.button10.BackColor = GetColorFromUser();
            this.ReloadDemoPicture();
        }

        private System.Drawing.Color GetColorFromUser()
        {
            this.colorDialog1.ShowDialog();
            return this.colorDialog1.Color;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.fontDialog1.ShowDialog();
            this.GameFont = this.fontDialog1.Font;
            this.ReloadDemoPicture();
        }

        private void ReloadDemoPicture()
        {
            this.Colors = new List<System.Drawing.Color>();
            this.Colors.Add(this.button2.BackColor);
            this.Colors.Add(this.button3.BackColor);
            this.Colors.Add(this.button4.BackColor);
            this.Colors.Add(this.button5.BackColor);
            this.Colors.Add(this.button6.BackColor);
            this.Colors.Add(this.button9.BackColor);
            this.Colors.Add(this.button10.BackColor);
            this.SetSettings();
            this.Game = new SnakeMainProcessor(this.pictureBox1.Height, this.pictureBox1.Width, this.Colors.ToArray(), this.SettingsList, this.GameFont, this.openFileDialog1.FileName);
            this.pictureBox1.Image = this.Game.MessageStartContext(this.Game.Height, this.Width);
        }
    }
}
