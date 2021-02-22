using System.Windows.Forms;

namespace DstlbxAutoUpdater
{
    class CustomMessageBox
    {

        public static DialogResult ShowMessage(string message, string caption, MessageBoxButtons button, MessageBoxIcon icon)
        {
            DialogResult dlgResult = DialogResult.None;

            switch (button)
            {
                case MessageBoxButtons.OK:
                    using (frmMessageOK msgOK = new frmMessageOK())
                    {
                        //Change text, caption & icon
                        msgOK.Text = caption;
                        msgOK.Message = message;

                        dlgResult = msgOK.ShowDialog();

                        msgOK.Activate();
                        msgOK.Focus();
                    }
                    break;
                case MessageBoxButtons.YesNo:
                    using (frmMessageYesNo msgYesNo = new frmMessageYesNo())
                    {
                        //Change text, caption & icon
                        msgYesNo.Text = caption;
                        msgYesNo.Message = message;

                        dlgResult = msgYesNo.ShowDialog();

                        msgYesNo.Activate();
                        msgYesNo.Focus();
                    }
                    break;

            }
            return dlgResult;
        }
    }
}
