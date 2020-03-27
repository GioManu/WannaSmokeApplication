using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannaSmoke.Client
{
    public class PopUpNotifierSettings
    {
        public int Duration { get; set; }
        public string TitleText { get; set; }
        public EventHandler OnCloseEvent { get; set; }
        public Size Size { get; set; }
    }
}
