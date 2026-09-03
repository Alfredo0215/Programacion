using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    public class Helper
    {
        private const float MENU_FONT_NORMAL = 25f;
        private const float MENU_FONT_HOVER = 35f;
        public static class UIHelper
        {
            public static void SetLabelHover(Label label)
            {
                label.Font = new Font(label.Font.FontFamily, MENU_FONT_HOVER, label.Font.Style);
                label.ForeColor = Color.Yellow;

            }
            public static void SetLabelNormal(Label label)
            {
                label.Font = new Font(label.Font.FontFamily, MENU_FONT_NORMAL, label.Font.Style);
                label.ForeColor = Color.Gainsboro;
            }

        }
          
    }
}
