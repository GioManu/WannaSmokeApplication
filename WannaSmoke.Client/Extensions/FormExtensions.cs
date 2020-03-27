using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using WannaSmoke.Client.Forms;

namespace WannaSmoke.Client.Extensions
{
    public static class FormExtensions
    {
        public static void InitTray(this MainForm me,NotifyIcon icon,Dictionary<string,EventHandler> events)
        {
            var contextMenu = new List<MenuItem>();

            foreach(var ev in events)
            {
                contextMenu.Add(new MenuItem(ev.Key, new EventHandler(ev.Value)));
            }

            icon.ContextMenu = new ContextMenu(contextMenu.ToArray());
        }
        public static void InitPopUp(this MainForm me, PopUpNotifierSettings settings)
        {
            me.PopUp = new PopupNotifier();
            me.PopUp.Size = settings.Size;

            me.PopUp.TitleText = settings.TitleText;
            
            me.PopUp.Disappear += settings.OnCloseEvent;
            me.PopUp.Close += settings.OnCloseEvent;
            
            me.PopUp.AnimationDuration = settings.Duration;
            
            me.PopUp.Image = Properties.Resources.mk_guy_128;

            me.PopUp.ImagePadding = new Padding(10);
            me.PopUp.TitlePadding = new Padding(10);
            me.PopUp.ContentPadding = new Padding(10);

            me.PopUp.TitleFont = new Font(new FontFamily("Comic Sans MS"), emSize: float.Parse("16"));

            me.PopUp.ContentFont = new Font(new FontFamily("Arial"),emSize: float.Parse("12"));

            me.PopUp.IsRightToLeft = true;
        }
        public static void InitSocket(this MainForm me, Action<string> ev)
        {
            me.Socket.OnConnected(ev);
            me.Socket.Connect();
        }
    }
}
