using System.Windows.Forms;

namespace DstlbxAutoUpdater
{
    public partial class frmMessageYesNo : Form
    {
        public frmMessageYesNo()
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
