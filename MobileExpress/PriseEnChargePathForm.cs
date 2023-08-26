using System.Windows.Forms;

namespace MobileExpress
{
    public partial class PriseEnChargePathForm : Form
    {
        public PriseEnChargePathForm(string path)
        {
            InitializeComponent();

            textBox1.Text = path;
        }
    }
}
