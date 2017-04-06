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
        private List<System.Windows.Forms.CheckBox> SettingsList;
        private List<System.Windows.Forms.Button> ButtonsList;
        private System.Drawing.Font GameFont;
        public SettingsForm()//System.Threading.Tasks.Task Task_)
        {
            this.ThreadsManager = new ProgramThreads();
            this.Colors = new List<Color>();
            //this.ThreadsManager.AddTask(Task_);
            InitializeComponent();
            this.InitSettings();
            this.InitButtons();
            this.SetColors();
            //var ra = string.Concat(System.Environment.CurrentDirectory.Split(System.IO.Path.DirectorySeparatorChar).Where((z, j) => j < System.Environment.CurrentDirectory.Split(System.IO.Path.DirectorySeparatorChar).Select((x, i) => x == "SnakeGame" ? i : 0).Sum()).Select(x => x + System.IO.Path.DirectorySeparatorChar)) + @"PrevPicture.bmp";
            //this.openFileDialog1.InitialDirectory = string.Concat(System.Environment.CurrentDirectory.Split(System.IO.Path.DirectorySeparatorChar).Where((z, j) => j < System.Environment.CurrentDirectory.Split(System.IO.Path.DirectorySeparatorChar).Select((x, i) => x == "SnakeGame" ? i : 0).Sum()).Select(x => x + System.IO.Path.DirectorySeparatorChar))+ @"PrevPicture.bmp";
            this.openFileDialog1.InitialDirectory = System.Environment.CurrentDirectory + @"PrevPicture.bmp";
            this.openFileDialog1.FileName = this.openFileDialog1.InitialDirectory;
            openFileDialog1_FileOk(this.Game, new CancelEventArgs());
        }

        private void InitButtons()
        {
            this.ButtonsList = new List<Button>();
            this.ButtonsList.Add(this.button1);
            this.ButtonsList.Add(this.button2);
            this.ButtonsList.Add(this.button3);
            this.ButtonsList.Add(this.button4);
            this.ButtonsList.Add(this.button5);
            this.ButtonsList.Add(this.button6);
            this.ButtonsList.Add(this.button7);
            this.ButtonsList.Add(this.button8);
            this.ButtonsList.Add(this.button9);
            this.ButtonsList.Add(this.button10);
            //this.ButtonsList.Add(this.button11);
            //this.ButtonsList.Add(this.button12);
            for (var i = 0; i < this.ButtonsList.Count; ++i)
            {
                if ((i >= 1 && i <= 5) || (i >= 8 && i <= 9))
                {
                    this.ButtonsList[i].Click += ButtonColorClick;
                }
                else
                {
                    this.ButtonsList[i].Click += ButtonClick;
                }
            }
        }

        private void InitSettings()
        {
            this.SettingsList = new List<CheckBox>();
            this.SettingsList.Add(this.checkBox1);
            this.SettingsList.Add(this.checkBox2);
            this.SettingsList.Add(this.checkBox3);
            this.SettingsList.Add(this.checkBox4);
            this.SettingsList.Add(this.checkBox5);
            this.SettingsList.Add(this.checkBox6);
            this.SettingsList.Add(this.checkBox7);
            this.SettingsList.Add(this.checkBox8);
            this.SettingsList.Add(this.checkBox9);
            this.SettingsList.Add(this.checkBox10);
            this.SettingsList.Add(this.checkBox11);
            this.SettingsList.Add(this.checkBox12);
            this.SettingsList.Add(this.checkBox13);
            this.SettingsList.Add(this.checkBox14);
            this.SettingsList.Add(this.checkBox15);
            this.SettingsList.Add(this.checkBox16);
            //this.SettingsList.Add(this.checkBox17);
            //this.SettingsList.Add(this.checkBox18);
            foreach (var Item in this.SettingsList)
            {
                Item.CheckedChanged += CheckedChangedEventHandler;
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            this.ReloadDemoPicture();
        }

        private void ButtonColorClick(object sender, EventArgs e)
        {
            ((System.Windows.Forms.Button)sender).BackColor = this.GetColorFromUser();
            this.ReloadDemoPicture();
        }

        private void CheckedChangedEventHandler(object sender, EventArgs e)
        {
            this.ReloadDemoPicture();
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

        private void button1_Click(object sender, EventArgs e)
        {
            var GamingFormTask = new System.Threading.Tasks.Task(() =>
            {
                var Context = new ApplicationContext(new GamingForm(ref this.ThreadsManager, this.Colors, this.SettingsList.Select(x => x.Checked).ToList(), (int)this.numericUpDown2.Value, this.GameFont, (int)this.numericUpDown1.Value, this.openFileDialog1.FileName)).MainForm;
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
            this.Game = new SnakeMainProcessor(this.pictureBox1.Height, this.pictureBox1.Width, (int)this.numericUpDown2.Value, this.Colors.ToArray(), this.SettingsList.Select(x => x.Checked).ToList(), this.GameFont, this.openFileDialog1.FileName);
            this.pictureBox1.Image = this.Game.MessageStartContext(this.Game.Height, this.Width);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ReloadDemoPicture();
        }
    }
}