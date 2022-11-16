using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace danim
{
    public class CheckBox : Component
    {
        public bool Checked { get; set; }

        public CheckBox(string name, Position position, string text, bool @checked) : base(name, position, typeof(CheckBox), "[] " + text, true, ConsoleColor.White)
        {
            Checked = @checked;

            OnClickEvent += CheckBox_OnClickEvent;
        }

        private void CheckBox_OnClickEvent(object sender, ConsoleWindow.OnClickEventArgs e)
        {
            Checked = !Checked;
            if(Checked == true)
            {
                SetText(Text.Substring(2));
                SetText("[*]" + Text);
            }
            else
            {
                SetText(Text.Substring(3));
                SetText("[]" + Text);
            }
            CheckboxChange?.Invoke(this);
        }

        public delegate void OnCheckboxChange(CheckBox checkBox);

        public event OnCheckboxChange CheckboxChange;
    }
}
