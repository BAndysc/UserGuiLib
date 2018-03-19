using System.Windows.Forms;
using UserGuiLib.ExampleCommon.ColorWheel;
using UserGuiLib.ExampleCommon.TreeExample;

namespace UserGuiLib.WinFormsExample
{
    public partial class Example : Form
    {
        public Example()
        {
            InitializeComponent();

            userGuiControl1.AddComponent(new ColorPickerExample());
            userGuiControl2.AddComponent(new TreeExample());
        }
    }
}
