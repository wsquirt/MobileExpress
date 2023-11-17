using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace MobileExpress
{
    public static class Tools
    {
        public static int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
        public static decimal? ToNullableDecimal(string s)
        {
            decimal i;
            if (decimal.TryParse(s, out i)) return i;
            return null;
        }
        public static string GetEnumDescriptionFromEnum<T>(T enumValue) where T : Enum
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();
        }
        public static T GetEnumFromDescription<T>(string description) where T : Enum
        {
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (GetEnumDescriptionFromEnum(enumValue) == description)
                {
                    return enumValue;
                }
            }

            throw new ArgumentException("Enum value not found for the given description.", nameof(description));
        }
        public static string GetStringFromCustomer(Customer customer)
        {
            string customerName;

            if (!string.IsNullOrWhiteSpace(customer.LastName))
            {
                if (!string.IsNullOrWhiteSpace(customer.FirstName))
                {
                    if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
                    {
                        if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                        {
                            customerName = $"{customer.FirstName} {customer.LastName} - {customer.PhoneNumber} - {customer.EmailAddress}";
                        }
                        else
                        {
                            customerName = $"{customer.FirstName} {customer.LastName} - {customer.PhoneNumber}";
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                    {
                        customerName = $"{customer.FirstName} {customer.LastName} - {customer.EmailAddress}";
                    }
                    else
                    {
                        customerName = $"{customer.FirstName} {customer.LastName}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
                {
                    if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                    {
                        customerName = $"{customer.LastName} - {customer.PhoneNumber} - {customer.EmailAddress}";
                    }
                    else
                    {
                        customerName = $"{customer.LastName} - {customer.PhoneNumber}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                {
                    customerName = $"{customer.LastName} - {customer.EmailAddress}";
                }
                else
                {
                    customerName = $"{customer.LastName}";
                }
            }
            else if (!string.IsNullOrWhiteSpace(customer.FirstName))
            {
                if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
                {
                    if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                    {
                        customerName = $"{customer.FirstName} - {customer.PhoneNumber} - {customer.EmailAddress}";
                    }
                    else
                    {
                        customerName = $"{customer.FirstName} - {customer.PhoneNumber}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                {
                    customerName = $"{customer.FirstName} - {customer.EmailAddress}";
                }
                else
                {
                    customerName = $"{customer.FirstName}";
                }
            }
            else if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
            {
                if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                {
                    customerName = $"{customer.PhoneNumber} - {customer.EmailAddress}";
                }
                else
                {
                    customerName = $"{customer.PhoneNumber}";
                }
            }
            else
            {
                customerName = $"{customer.EmailAddress}";
            }

            return customerName;
        }

        public static List<string> GetDataFromFile(string path)
        {
            try
            {
                List<string> data = new List<string>();
                using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        data.Add(line);
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        public static bool RewriteDataToFile(List<string> data, string path, bool toAppend)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, append: toAppend, Encoding.UTF8))
                {
                    if (writer == null)
                        MessageBox.Show($"L'écrire dans le fichier {path} a échoué.");

                    foreach (string line in data)
                    {
                        writer.WriteLine(line);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        public static bool WriteLineTofile(string line, string path, bool toAppend)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path: path, append: toAppend, Encoding.UTF8))
                {
                    writer.WriteLine(line);
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Une erreur est survenue ! " + e.Message);
                return false;
            }
        }

        public static (bool, bool, bool) GetBoolFromPaymentMode(PaymentMode paymentMode)
        {
            return (
                paymentMode == PaymentMode.CB ? (true, false, false) :
                paymentMode == PaymentMode.ESP ? (false, true, false) :
                paymentMode == PaymentMode.CBESP ? (true, true, false) :
                paymentMode == PaymentMode.VIR ? (false, false, true) :
                paymentMode == PaymentMode.CBVIR ? (true, false, true) :
                paymentMode == PaymentMode.ESPVIR ? (false, true, true) :
                paymentMode == PaymentMode.CBESPVIR ? (true, true, true) :
                (false, false, false)
            );
        }
        public static PaymentMode GetPaymentModeFromBool(bool cb, bool espece, bool virement)
        {
            return
                cb && espece && virement ? PaymentMode.CBESPVIR :
                cb && espece ? PaymentMode.CBESP :
                cb && virement ? PaymentMode.CBVIR :
                cb ? PaymentMode.CB :
                espece && virement ? PaymentMode.ESPVIR :
                espece ? PaymentMode.ESP :
                virement ? PaymentMode.VIR :
                PaymentMode.NONE;
        }
    }
}
