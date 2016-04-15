using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyController
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Auto)]
        static extern IntPtr GetWindowCaption(IntPtr hwnd, StringBuilder lpString, int maxCount);
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Height = 21;
            this.Top = 0;
        }
        private void Skip()
        {
            IntPtr hwnd = FindWindow("SpotifyMainWindow", null);

            PostMessage(hwnd, 0x319, IntPtr.Zero, new IntPtr(0xB0000L));
        }
        private string GetPlaying()
        {
            IntPtr hwnd = FindWindow("SpotifyMainWindow", null);
            StringBuilder sb = new StringBuilder(256);
            GetWindowCaption(hwnd, sb, 256);
            return sb.ToString();
            
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            
            if (e.Button == MouseButtons.Left)
            {
                int WM_NCLBUTTONDOWN = 0xA1;
                int HT_CAPTION = 0x2;
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Skip();
        }

        private bool trackchanged = false;
        private int waitsincechange = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = GetPlaying();

            if (previoustext != GetPlaying())
            {
                //track changed
                label1.Left = 22;
                trackchanged = true;
                scrollingText.Enabled = false;
            }

            if (trackchanged)
            {
                waitsincechange++;

                if (waitsincechange > 3)
                {
                    waitsincechange = 0;
                    trackchanged = false;
                    scrollingText.Enabled = true;
                }
            }
            

            previoustext = GetPlaying();
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            this.Opacity = .6;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private string previoustext = "";
        private void scrollingText_Tick(object sender, EventArgs e)
        {

            if (label1.Text == "Spotify")
            {
                label1.Left = 22;
            }
            else
            {
                label1.Left = label1.Left - 1;
                if (label1.Left <= 0 - label1.Width)
                {
                    label1.Left = this.Width;
                }

            }
        }
    }

}
