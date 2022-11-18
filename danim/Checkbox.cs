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
            UpdateVisual();
            OnClickEvent += CheckBox_OnClickEvent;
        }

        private void CheckBox_OnClickEvent(object sender, ConsoleWindow.OnClickEventArgs e)
        {
            Checked = !Checked;
            UpdateVisual();
            CheckboxChange?.Invoke(this);
        }

        private void UpdateVisual()
        {
            if (Checked == true)
            {
                SetText(Text.Substring(2));
                SetText("[*]" + Text);
            }
            else
            {
                SetText(Text.Substring(3));
                SetText("[]" + Text);
            }
        }

        public delegate void OnCheckboxChange(CheckBox checkBox);

        public event OnCheckboxChange CheckboxChange;
    }
}
