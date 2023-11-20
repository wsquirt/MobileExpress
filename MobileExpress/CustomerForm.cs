using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MobileExpress
{
    public partial class CustomerForm : Form
    {
        string Lastname;
        string Firstname;
        string Phone;
        string Email;
        Sexe Sexe;
        List<Customer> Customers;
        Customer Customer;
        bool IsUpdate;

        public CustomerForm(Customer customer, List<Customer> customers)
        {
            Lastname = string.Empty;
            Firstname = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Sexe = Sexe.Unknown;

            InitializeComponent();

            Customers = customers;
            Customer = customer;
            IsUpdate = Customer != null;

            foreach (Sexe value in Enum.GetValues(typeof(Sexe)))
            {
                comboBox1.Items.Add(Tools.GetEnumDescriptionFromEnum<Sexe>(value));
            }
            comboBox1.DrawMode = DrawMode.Normal;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedItem = Tools.GetEnumDescriptionFromEnum<Sexe>(Sexe.Unknown);
            if (IsUpdate)
            {
                textBox1.Text = Customer.LastName;
                textBox2.Text = Customer.FirstName;
                textBox3.Text = Customer.PhoneNumber;
                textBox4.Text = Customer.EmailAddress;

                Sexe = Customer.Sexe;
                comboBox1.SelectedItem = Tools.GetEnumDescriptionFromEnum<Sexe>(Sexe);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Lastname = textBox1.Text;
                Firstname = textBox2.Text;
                Phone = textBox3.Text;
                Email = textBox4.Text;

                if (Customers.Any(x =>
                {
                    bool isExistedCustomer =
                        x.PhoneNumber.Contains(Phone) ||
                        (x.LastName.ToLowerInvariant().Contains(Lastname.ToLowerInvariant()) && x.FirstName.ToLowerInvariant().Contains(Firstname.ToLowerInvariant())) ||
                        x.LastName.ToLowerInvariant().Contains(Lastname.ToLowerInvariant()) || x.FirstName.ToLowerInvariant().Contains(Firstname.ToLowerInvariant());
                    return isExistedCustomer;
                }))
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                string sexe = (string)comboBox1.SelectedItem;
                if (!string.IsNullOrWhiteSpace(sexe))
                    Sexe = Tools.GetEnumFromDescription<Sexe>(sexe);

                if (IsUpdate)
                {
                    Customer.LastName = !string.IsNullOrWhiteSpace(Lastname) ? char.ToUpper(Lastname[0]) + Lastname.Substring(1) : string.Empty;
                    Customer.FirstName = !string.IsNullOrWhiteSpace(Firstname) ? char.ToUpper(Firstname[0]) + Firstname.Substring(1) : string.Empty;
                    Customer.PhoneNumber = Phone;
                    Customer.EmailAddress = Email;
                    Customer.Sexe = Sexe;
                }
                else
                {
                    Customer = new Customer();
                    Customer.Id = (Customers.OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1;
                    Customer.LastName = !string.IsNullOrWhiteSpace(Lastname) ? char.ToUpper(Lastname[0]) + Lastname.Substring(1) : string.Empty;
                    Customer.FirstName = !string.IsNullOrWhiteSpace(Firstname) ? char.ToUpper(Firstname[0]) + Firstname.Substring(1) : string.Empty;
                    Customer.PhoneNumber = Phone;
                    Customer.EmailAddress = Email;
                    Customer.Sexe = Sexe;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }

        public (bool, Customer) GetResult()
        {
            return (IsUpdate, Customer);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
