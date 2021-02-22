using System.Windows.Forms;

namespace DstlbxAutoUpdater
{
    public partial class frmMessageOK : Form
    {
        public frmMessageOK()
        {
            InitializeComponent();
        }
        public string Message
        {
            get => lblMessage.Text;
            set => lblMessage.Text = value;
        }
    }
}
