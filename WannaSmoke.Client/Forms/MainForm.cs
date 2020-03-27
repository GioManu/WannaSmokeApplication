using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using WannaSmoke.Client.Extensions;
using WannaSmoke.Client.Services.SocketService;

namespace WannaSmoke.Client.Forms
{
    public partial class MainForm : Form
    {
        private Settings Settings;
        public ISocketService Socket;
        public PopupNotifier PopUp;
        public MainForm(Settings settings,ISocketService socket)
        {
            Socket = socket;
            Settings = settings;
            
            InitializeComponent();

            // Tray Initialization
            this.InitTray(notifyIcon1,new Dictionary<string, EventHandler>()
            {
                { "Exit",Exit },
                { "Show",Show },
                { "Go Smoke",SmokeBtn_Click }
            });

            // PopUp Initialization
            this.InitPopUp(new PopUpNotifierSettings
            { 
                TitleText = "Hey Buddy ^_^",
                Duration = 5,
                OnCloseEvent = PopUp_Close,
                Size = new Size(new Point(400, 200))
            });

            // Settings Initialization
            this.InitSettings(settings);

            // Socket Initialization
            this.InitSocket(GoSmoke);

            InitSettings(settings);
        }

        /* Toggle Settings/Button */
        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = groupBox1.Visible ? ChangeButtonPosition(false) : ChangeButtonPosition(true);
        }
        private bool ChangeButtonPosition(bool enable)
        {
            SmokeBtn.Visible = !enable;
            InitSettings(Settings);
            return enable;
        }

        /* Settings Initialization */
        private Settings InitSettings(Settings settings)
        {
            ServerTxt.Text = settings.Config.Server;
            NameTxt.Text = settings.Config.Name;
            PortTxt.Text = settings.Config.Port;
            return settings;
        }

        /* Validate Settings */
        private bool ValidateText(params TextBox[] boxes)
        {
            foreach(var param in boxes)
            {
                if (string.IsNullOrEmpty(param.Text))
                {
                    //Todo Color that textBox
                    return false;
                }
            }
            return true;
        }

        /* TestConnection */
        private void button3_Click_2(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            MessageBox.Show(Socket.TestConnection().ToString());
            this.Cursor = Cursors.Default;
        }

        /* PopUpNotification CallToAction Events */
        private void PopUp_Close(object sender, EventArgs e)
        {
            PopUp.ContentText = string.Empty;
        }
        private void SmokeBtn_Click(object sender, EventArgs e)
        {
            Task.Run(() => Socket.GoSmoke(ShowMessage, GoSmokeError));
        }
        private void Show(object sender, EventArgs e)
        {
            this.Show();
        }
        private void Exit(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        /* Form Control Methods */
        private void MainForm_Load(object sender, EventArgs e) { }
        private void MainForm_OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        /* Call To Action Methods */
        private void GoSmoke(string name)
        {
            playSimpleSound();
            PopUp.ContentText = String.Join("\n", PopUp.ContentText, $"{name} Wants To Smoke");
            PopUp.Popup();
        }
        private void playSimpleSound()
        {
            Stream str = Properties.Resources.mk_guy_sound;
            SoundPlayer rp = new SoundPlayer(str);
            rp.Play();
        }
        private void GoSmokeError(string error)
        {
            PopUp.ContentText = error;
            PopUp.Popup();
        }
        /* MessageBox */
        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /* Save Button Action */
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (ValidateText(ServerTxt, PortTxt))
            {
                Settings.Config.Server = ServerTxt.Text.Trim();
                Settings.Config.Name = NameTxt.Text.Trim();
                Settings.Config.Port = PortTxt.Text.Trim();
                Settings.SaveConfig();
                MessageBox.Show("Saved", caption: "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
