using System.Windows.Forms;

namespace MobileExpress
{
    public partial class InvoicePathForm : Form
    {
        public InvoicePathForm(string path)
        {
            InitializeComponent();

            textBox1.Text = path;
        }
    }
}
