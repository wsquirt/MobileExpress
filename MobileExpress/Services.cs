using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Xceed.Document.NET;
using Xceed.Words.NET;
using static MobileExpress.FormMobileExpress;
using Color = System.Drawing.Color;
using Task = System.Threading.Tasks.Task;
using TextBox = System.Windows.Forms.TextBox;

namespace MobileExpress
{
    public static class Services
    {
        #region Document Word
        public static string TemplateFacturePath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Template facture.docx";
        public static string TemplateRecuPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Template prise en charge.docx";
        public static void GenerateDocx(
            int type,
            string logo,
            string customerName,
            string customerPhone,
            string customerEmail,
            string takeOverDate,
            string takeOverNumber,
            decimal? accompte,
            decimal paid,
            string modeDePaiement,
            List<MEData> mEDatas,
            string path,
            string title,
            Dictionary<int, string> repairTypeNames,
            Dictionary<int, string> unlockTypeNames,
            Dictionary<int, string> articles,
            Dictionary<int, string> marques,
            Dictionary<int, string> modeles)
        {
            try
            {
                Func<string, string, string, string> func = (tmpmarque, tmpmodele, tmpimei) =>
                {
                    return
                        !string.IsNullOrWhiteSpace(tmpmarque) && !string.IsNullOrWhiteSpace(tmpimei) ?
                            $"{tmpmarque} {tmpmodele} (IMEI:{tmpimei})" :
                        string.IsNullOrWhiteSpace(tmpmarque) && !string.IsNullOrWhiteSpace(tmpimei) ?
                            $"(IMEI:{tmpimei})" : $"{tmpmarque} {tmpmodele}";
                };

                decimal total = 0;
                int priseEnChargeId = 0;
                int invoiceId = 0;
                string articlesHtml = string.Empty;
                List<DocxArticle> docxArticles = new List<DocxArticle>();
                foreach (MEData mEData in mEDatas)
                {
                    if (priseEnChargeId == 0)
                    {
                        priseEnChargeId = mEData.TakeOverId;
                    }
                    if (type == 1 && invoiceId == 0 && mEData.InvoiceId.HasValue)
                    {
                        invoiceId = mEData.InvoiceId.Value;
                    }
                    if (type == 0 && mEData.ArticleId.HasValue)
                    {
                        paid -= (mEData.Price.Value * mEData.Quantity + (mEData.Quantity * (mEData.Remise ?? 0) * -1));
                        continue;
                    }

                    string panne = type == 0 ? (mEData.RepairTypeId.HasValue ? repairTypeNames[mEData.RepairTypeId.Value] : unlockTypeNames[mEData.UnlockTypeId.Value]) : articles[mEData.ArticleId.Value];
                    string marquename = mEData.MarqueId.HasValue ? marques[mEData.MarqueId.Value] : string.Empty;
                    string modelename = mEData.ModeleId.HasValue ? modeles[mEData.ModeleId.Value] : string.Empty;
                    string imei = mEData.IMEI;

                    if (mEData.Verification)
                    {
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = func(marquename, modelename, imei),
                            Panne = panne,
                            Quantity = mEData.Quantity.ToString(),
                            Price = string.Empty,
                            Total = string.Empty,
                        });
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = func(marquename, modelename, imei),
                            Panne = "Vérification",
                            Quantity = mEData.Quantity.ToString(),
                            Price = mEData.Price.Value.ToString(),
                            Total = (mEData.Quantity * mEData.Price.Value).ToString(),
                        });
                        total += (mEData.Quantity * mEData.Price.Value);
                    }
                    else
                    {
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = func(marquename, modelename, imei),
                            Panne = panne,
                            Quantity = mEData.Quantity.ToString(),
                            Price = mEData.Price.ToString(),
                            Total = (mEData.Quantity * mEData.Price.Value).ToString(),
                        });
                        total += (mEData.Quantity * mEData.Price.Value);
                    }
                    if (type == 1 && mEData.Remise.HasValue)
                    {
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = func(marquename, modelename, imei),
                            Panne = "Remise",
                            Quantity = mEData.Quantity.ToString(),
                            Price = (mEData.Remise.Value * -1).ToString(),
                            Total = (mEData.Quantity * mEData.Remise.Value * -1).ToString(),
                        });
                        total += (mEData.Quantity * mEData.Remise.Value * -1);
                    }
                    if (type == 1 && mEData.Garantie.HasValue)
                    {
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = func(marquename, modelename, imei),
                            Panne = $"Garantie {$"{mEData.Garantie.Value.ToString()} mois"}{((mEData.OptionGarantie) ? " (Hors casse et oxydation)" : string.Empty)}",
                            Quantity = mEData.Quantity.ToString(),
                            Price = string.Empty,
                            Total = string.Empty,
                        });
                    }
                }

                string directoryPath = type == 0 ? Paths.PriseEnChargeDirectory : Paths.FactureDirectory; // Remplacez par le chemin de votre répertoire
                                                                                                          // Modèle de fichier recherché
                string priseEnChargeSearchPattern = $@"PriseEnCharge_{(priseEnChargeId < 10 ? $"0{priseEnChargeId}" : $"{priseEnChargeId}")}_*.docx";
                string factureSearchPattern = $@"Facture_{(invoiceId < 10 ? $"0{invoiceId}" : $"{invoiceId}")}_PriseEnCharge_{(priseEnChargeId < 10 ? $"0{priseEnChargeId}" : $"{priseEnChargeId}")}_*.docx";
                string searchPattern = type == 0 ? priseEnChargeSearchPattern : factureSearchPattern;
                // Vérifiez si des fichiers correspondent au modèle dans le répertoire
                string[] files = Directory.GetFiles(directoryPath, searchPattern);
                foreach (var filePath in files)
                {
                    // Supprimez chaque fichier trouvé
                    File.Delete(filePath);
                }

                Task task = new Task(delegate {
                    CreateDocx(new DocxData()
                    {
                        Path = path,
                        Type = type,
                        Logo = logo,
                        Date = takeOverDate,
                        Number = takeOverNumber,
                        CustomerName = customerName,
                        CustomerPhone = customerPhone,
                        CustomerEmail = customerEmail,
                        Total = (total == decimal.Zero ? string.Empty : $"{total}"),
                        Accompte = $"{(accompte ?? decimal.Zero)}",
                        Paid = paid.ToString(),
                        ResteDu = (total - paid - (accompte ?? decimal.Zero)).ToString(),
                        ModeDePaiement = modeDePaiement,
                        Articles = docxArticles,
                    });
                });
                task.Start();
                task.Wait();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private static void CreateDocx(DocxData docxData)
        {
            try
            {
                // Charger le modèle de facture .docx
                var doc = DocX.Load(docxData.Type == 0 ? TemplateRecuPath : TemplateFacturePath);
                // Créer une instance de StringReplaceTextOptions pour spécifier comment vous souhaitez effectuer le remplacement
                var replaceOptions = new StringReplaceTextOptions
                {
                    EscapeRegEx = true, // Échapper les expressions régulières (si nécessaire)
                    NewValue = string.Empty, // La nouvelle valeur de remplacement (vide pour l'instant)
                    SearchValue = string.Empty, // La valeur à rechercher (vide pour l'instant)
                    UseRegExSubstitutions = false, // Utiliser des substitutions d'expressions régulières (si nécessaire)
                    NewFormatting = null // Format à appliquer aux valeurs de remplacement
                };
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.Date;
                replaceOptions.SearchValue = "{{date}}";
                doc.ReplaceText(replaceOptions);
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.Number;
                replaceOptions.SearchValue = "{{number}}";
                doc.ReplaceText(replaceOptions);
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.CustomerName;
                replaceOptions.SearchValue = "{{customerName}}";
                doc.ReplaceText(replaceOptions);
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.CustomerPhone;
                replaceOptions.SearchValue = "{{customerPhone}}";
                doc.ReplaceText(replaceOptions);
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.CustomerEmail;
                replaceOptions.SearchValue = "{{customerEmail}}";
                doc.ReplaceText(replaceOptions);
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.Total;
                replaceOptions.SearchValue = "{{total}}";
                doc.ReplaceText(replaceOptions);
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.Accompte;
                replaceOptions.SearchValue = "{{accompte}}";
                doc.ReplaceText(replaceOptions);
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.Paid;
                replaceOptions.SearchValue = "{{paid}}";
                doc.ReplaceText(replaceOptions);
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.ResteDu;
                replaceOptions.SearchValue = "{{resteDu}}";
                doc.ReplaceText(replaceOptions);
                // Effectuer le remplacement
                replaceOptions.NewValue = docxData.ModeDePaiement;
                replaceOptions.SearchValue = "{{modeDePaiement}}";
                doc.ReplaceText(replaceOptions);

                // Récupérer la table à partir du modèle (en supposant qu'elle soit la première table dans le document)
                var table1 = doc.Tables[5];
                // Remplir la table avec les données des articles
                foreach (var article in docxData.Articles)
                {
                    var row1 = table1.InsertRow();
                    row1.Cells[0].Paragraphs.First().Append(article.Name);
                    row1.Cells[0].Paragraphs.First().FontSize(docxData.Type == 0 ? 8 : 12);
                    row1.Cells[1].Paragraphs.First().Append(article.Panne);
                    row1.Cells[1].Paragraphs.First().FontSize(docxData.Type == 0 ? 8 : 12);
                    row1.Cells[2].Paragraphs.First().Append(article.Quantity);
                    row1.Cells[2].Paragraphs.First().FontSize(docxData.Type == 0 ? 8 : 12);
                    row1.Cells[3].Paragraphs.First().Append(article.Price);
                    row1.Cells[3].Paragraphs.First().FontSize(docxData.Type == 0 ? 8 : 12);
                    row1.Cells[4].Paragraphs.First().Append(article.Total);
                    row1.Cells[4].Paragraphs.First().FontSize(docxData.Type == 0 ? 8 : 12);
                }
                if (docxData.Type == 0)
                {
                    var table2 = doc.Tables[12];
                    // Remplir la table avec les données des articles
                    foreach (var article in docxData.Articles)
                    {
                        var row2 = table2.InsertRow();
                        row2.Cells[0].Paragraphs.First().Append(article.Name);
                        row2.Cells[0].Paragraphs.First().FontSize(8);
                        row2.Cells[1].Paragraphs.First().Append(article.Panne);
                        row2.Cells[1].Paragraphs.First().FontSize(8);
                        row2.Cells[2].Paragraphs.First().Append(article.Quantity);
                        row2.Cells[2].Paragraphs.First().FontSize(8);
                        row2.Cells[3].Paragraphs.First().Append(article.Price);
                        row2.Cells[3].Paragraphs.First().FontSize(8);
                        row2.Cells[4].Paragraphs.First().Append(article.Total);
                        row2.Cells[4].Paragraphs.First().FontSize(8);
                    }
                }

                // Enregistrez le document modifié
                doc.SaveAs(docxData.Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        #endregion
        #region Prise en charge
        #region Réparation
        public static DataGridViewTextBoxColumn TakeOverRepairIdColumn = new DataGridViewTextBoxColumn
        {
            Name = "id",
            ValueType = typeof(int),
            Visible = false,
            ReadOnly = true,
        };
        public static DataGridViewTextBoxColumn TakeOverRepairMarqueColumn = new DataGridViewTextBoxColumn
        {
            HeaderText = "Marque",
            Name = "marque",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverRepairModeleColumn = new DataGridViewTextBoxColumn
        {
            HeaderText = "Modèle",
            Name = "modele",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverRepairTypeColumn = new DataGridViewTextBoxColumn
        {
            HeaderText = "Panne",
            Name = "type",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverRepairIMEIColumn = new DataGridViewTextBoxColumn()
        {
            HeaderText = "IMEI",
            Name = "imei",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverRepairPriceColumn = new DataGridViewTextBoxColumn()
        {
            HeaderText = "Prix",
            Name = "price",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(decimal),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewCheckBoxColumn TakeOverRepairGarantieColumn = new DataGridViewCheckBoxColumn()
        {
            HeaderText = "Garantie",
            Name = "garantie",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewButtonColumn TakeOverRepairDeleteColumn = new DataGridViewButtonColumn()
        {
            HeaderText = "Action",
            Name = "delete",
            Text = "Supprimer",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            Visible = true,
            ReadOnly = false,
        };
        #endregion
        #region Dépannage
        public static DataGridViewTextBoxColumn TakeOverUnlockIdColumn = new DataGridViewTextBoxColumn
        {
            Name = "id",
            ValueType = typeof(int),
            Visible = false,
            ReadOnly = true,
        };
        public static DataGridViewTextBoxColumn TakeOverUnlockMarqueColumn = new DataGridViewTextBoxColumn
        {
            HeaderText = "Marque",
            Name = "marque",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverUnlockModeleColumn = new DataGridViewTextBoxColumn
        {
            HeaderText = "Modèle",
            Name = "modele",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverUnlockTypeColumn = new DataGridViewTextBoxColumn
        {
            HeaderText = "Déblocage",
            Name = "type",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverUnlockIMEIColumn = new DataGridViewTextBoxColumn()
        {
            HeaderText = "IMEI",
            Name = "imei",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverUnlockPriceColumn = new DataGridViewTextBoxColumn()
        {
            HeaderText = "Prix",
            Name = "price",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(decimal),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewCheckBoxColumn TakeOverUnlockGarantieColumn = new DataGridViewCheckBoxColumn()
        {
            HeaderText = "Garantie",
            Name = "garantie",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewButtonColumn TakeOverUnlockDeleteColumn = new DataGridViewButtonColumn()
        {
            HeaderText = "Action",
            Name = "delete",
            Text = "Supprimer",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            Visible = true,
            ReadOnly = false,
        };
        #endregion
        #region Achat
        public static DataGridViewTextBoxColumn TakeOverAchatIdColumn = new DataGridViewTextBoxColumn
        {
            Name = "id",
            ValueType = typeof(int),
            Visible = false,
            ReadOnly = true,
        };
        public static DataGridViewTextBoxColumn TakeOverAchatArticleTColumn = new DataGridViewTextBoxColumn
        {
            Name = "article",
            HeaderText = "Article",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewComboBoxColumn TakeOverAchatArticleCColumn = new DataGridViewComboBoxColumn
        {
            Name = "article",
            HeaderText = "Article",
            //DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
            //DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            ReadOnly = false,
            //DisplayMember = "id",
            //ValueMember = "name",
        };
        public static DataGridViewTextBoxColumn TakeOverAchatQuantityColumn = new DataGridViewTextBoxColumn()
        {
            HeaderText = "Quantité",
            Name = "quantity",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(int),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverAchatPriceColumn = new DataGridViewTextBoxColumn()
        {
            HeaderText = "Prix",
            Name = "price",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(decimal),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewCheckBoxColumn TakeOverAchatGarantieColumn = new DataGridViewCheckBoxColumn()
        {
            HeaderText = "Garantie",
            Name = "garantie",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewCheckBoxColumn TakeOverAchatRemiseColumn = new DataGridViewCheckBoxColumn()
        {
            HeaderText = "Remise",
            Name = "remise",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewButtonColumn TakeOverAchatDeleteColumn = new DataGridViewButtonColumn()
        {
            HeaderText = "Action",
            Name = "delete",
            Text = "Supprimer",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            Visible = true,
            ReadOnly = false,
        };
        #region Scanner
        #region Scanner
        public static bool IsScannerOK = true;
        private static SerialPort CurrentPort = new SerialPort();
        private static System.Timers.Timer ATimer;
        public delegate void updateDelegate(string txt);
        static Form FormCible;
        static updateTextBoxScannerBarcodeDelegate MethodeToExecute;
        #endregion
        public static T GetPropertyValue<T>(object obj, string propName)
        {
            return (T)obj.GetType().GetProperty(propName).GetValue(obj, null);
        }
        public static void InitializeScanner<T>(
            T form,
            updateTextBoxScannerBarcodeDelegate methodeToExecute
        ) where T : Form
        {
            try
            {
                CloseScanner();

                CurrentPort.PortName = "COM3";
                CurrentPort.BaudRate = 19200;
                CurrentPort.ReadTimeout = 1000;
                CurrentPort.Parity = Parity.None;
                CurrentPort.StopBits = StopBits.One;
                CurrentPort.DataBits = 8;

                FormCible = form;

                ATimer = new System.Timers.Timer(1000);
                MethodeToExecute = methodeToExecute;
                ATimer.Elapsed -= TimedEvent;
                ATimer.Elapsed += TimedEvent;
                ATimer.AutoReset = true;
                ATimer.Enabled = true;
            }
            catch (Exception ex)
            {
                IsScannerOK = false;
                MessageBox.Show($"Le scanner est indisponible. Erreur à remonter : {ex.StackTrace}", "Alerte", MessageBoxButtons.OK);
            }
        }
        private static void TimedEvent(object sender, ElapsedEventArgs e)
        {
            OnTimedEvent(sender, e, FormCible, MethodeToExecute);
        }
        private static void OnTimedEvent<T>(object sender, ElapsedEventArgs e, T form, updateTextBoxScannerBarcodeDelegate methodeToExecute) where T : Form
        {
            try
            {
                if (IsScannerOK && !CurrentPort.IsOpen)
                {
                    CurrentPort.Open();
                    System.Threading.Thread.Sleep(100); /// for recieve all data from scaner to buffer
                    CurrentPort.DiscardInBuffer();      /// clear buffer          
                }
                if (IsScannerOK)
                {
                    var textBoxScanner = form.GetType().GetProperty("ScannerBarcode")?.GetValue(form, null);

                    if (textBoxScanner == null)
                        return;
                    TextBox textBox = (TextBox)textBoxScanner;

                    string strFromPort = CurrentPort.ReadExisting().Replace("\r", "").Replace("\n", "");

                    if (string.IsNullOrWhiteSpace(strFromPort))
                        return;

                    // Exécute la méthode spécifiée lorsque le scanner est décclenché
                    textBox.BeginInvoke(methodeToExecute, strFromPort);

                    //Services.CloseScanner();
                    //Services.OpenScanner();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                if (string.Compare(ex.Message, "L'accès au port 'COM3' est refusé.") == 0)
                    return;
                MessageBox.Show("L'accès au scanner est refusé.", "Alerte", MessageBoxButtons.OK);
            }
            catch (IOException ex)
            {
                if (string.Compare(ex.Message, "L'opération d'entrée/sortie a été abandonnée en raison de l'arrêt d’un thread ou à la demande d'une application.\r\n") == 0)
                    return;
                MessageBox.Show("Le scanner est en cours de démarrage.", "Alerte", MessageBoxButtons.OK);
            }
            catch (InvalidOperationException)
            {
                ;
            }
            catch (Exception ex)
            {
                if (string.Compare(ex.Message, "Le port 'COM3' n'existe pas.") == 0)
                    return;
                if (string.Compare(ex.Message, "Le délai d'attente de l'opération a expiré.") == 0)
                    return;
                MessageBox.Show("Le scanner est indisponible.", "Alerte", MessageBoxButtons.OK);
            }
        }
        public static void OpenScanner()
        {
            if (IsScannerOK && !CurrentPort.IsOpen)
            {
                CurrentPort.Open();
                System.Threading.Thread.Sleep(100); /// for recieve all data from scaner to buffer
                CurrentPort.DiscardInBuffer();      /// clear buffer          
            }
        }
        public static void CloseScanner()
        {
            if (CurrentPort != null && CurrentPort.IsOpen)
                CurrentPort.Close();
        }
        #endregion
        #endregion
        #endregion
        #region Relation client
        #endregion
        #region Stock
        #endregion

        // Méthode générique pour obtenir la valeur d'une propriété à partir d'un objet
        public static object GetPropertyValue(object obj, string propertyName)
        {
            foreach (var prop in propertyName.Split('.'))
            {
                if (obj == null)
                    return null;

                PropertyInfo propInfo = obj.GetType().GetProperty(prop);
                if (propInfo == null)
                    return null;

                obj = propInfo.GetValue(obj);
            }

            return obj;
        }
        public static Marque GetMarqueFromBarcodeScanner(List<Marque> marques, string marqueName = null)
        {
            Marque marque = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(marqueName) && (marques?.Any() ?? false))
                {
                    marque = marques.FirstOrDefault(x =>
                        x.Name.Replace(" ", "").Trim().ToLowerInvariant().Contains(
                            marqueName.Replace(" ", "").Trim().ToLowerInvariant()));
                }

                if (marque == null)
                {
                    marque = new Marque(
                        marques.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1,
                        marqueName);
                    marques.Add(marque);
                    Tools.WriteLineTofile($"{marque.Id};{marque.Name}", Paths.MarquesDSPath, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur lors de la détection de la marque est survenue. Erreur à remonter : {ex.StackTrace}", "Erreur", MessageBoxButtons.OK);
            }

            return marque;
        }
        public static Modele GetModeleFromBarcodeScanner(List<Modele> modeles, string modeleName, int marqueId)
        {
            Modele modele = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(modeleName) && (modeles?.Any() ?? false))
                {
                    modele = modeles.FirstOrDefault(x =>
                        x.MarqueId == marqueId && x.Name.Replace(" ", "").Trim().ToLowerInvariant().Contains(
                            modeleName.Replace(" ", "").Trim().ToLowerInvariant()));
                }

                if (modele == null)
                {
                    modele = new Modele(modeles.Count(), marqueId, modeleName);
                    modeles.Add(modele);
                    Tools.WriteLineTofile($"{modele.Id};{modele.MarqueId};{modele.Name}", Paths.ModelesDSPath, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur lors de la détection du modele est survenue. Erreur à remonter : {ex.StackTrace}", "Erreur", MessageBoxButtons.OK);
            }

            return modele;
        }
        public static async Task<Article> GetBarcodeDataFromUrlAsync(string barcode, List<Marque> MarquesDS, List<Modele> ModelesDS, List<TakeOverType> TakeOverTypesDS)
        {
            Article articleTmp = null;
            try
            {
                articleTmp = new Article();

                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($@"https://api.upcitemdb.com/prod/trial/lookup?upc={barcode}"),
                };
                var response = await client.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                Root test = JsonConvert.DeserializeObject<Root>(body);
                if (test != null && test.total > 0)
                {
                    articleTmp.CodeReference = test.items.First().upc;
                    if (string.IsNullOrWhiteSpace(articleTmp.CodeReference))
                        articleTmp.CodeReference = test.items.First().ean;
                    if (string.IsNullOrWhiteSpace(articleTmp.CodeReference))
                        articleTmp.CodeReference = test.items.First().isbn;
                    if (string.IsNullOrWhiteSpace(articleTmp.CodeReference))
                        articleTmp.CodeReference = test.items.First().gtin;

                    articleTmp.MarqueId = Services.GetMarqueFromBarcodeScanner(MarquesDS, test.items.First().brand).Id;
                    if (articleTmp.MarqueId != null)
                        articleTmp.ModeleId = Services.GetModeleFromBarcodeScanner(ModelesDS, test.items.First().model, articleTmp.MarqueId.Value).Id;
                    if (!string.IsNullOrWhiteSpace(test.items.First().title))
                    {
                        articleTmp.Produit = TakeOverTypesDS.First(x => x.Id == 2).Articles.FirstOrDefault(x =>
                            x.Produit.Replace(" ", "").ToLower(System.Globalization.CultureInfo.InvariantCulture)
                            .Contains(test.items.First().title.Replace(" ", "").ToLower(System.Globalization.CultureInfo.InvariantCulture)))?.Produit;
                        if (!string.IsNullOrWhiteSpace(articleTmp.Produit))
                        {
                            articleTmp.Produit = TakeOverTypesDS.First(x => x.Id == 2).Articles.FirstOrDefault(x =>
                                x.Produit.Replace(" ", "").ToLower(System.Globalization.CultureInfo.InvariantCulture)
                                .Contains(test.items.First().title.Replace(" ", "").ToLower(System.Globalization.CultureInfo.InvariantCulture)))?.Produit;
                        }
                    }
                }
                else
                {
                    articleTmp = null;
                    MessageBox.Show("Aucune information a été trouvée. Veuillez renseigner le reste des informations.", "Alerte", MessageBoxButtons.OK);
                    Services.CloseScanner();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
                Services.CloseScanner();
                Services.OpenScanner();
            }
            return articleTmp;
        }
        public static Article GetBarcodeDataFromDB(string barcode, List<TakeOverType> TakeOverTypesDS)
        {
            Article articleTmp = null;
            try
            {
                articleTmp = TakeOverTypesDS
                    .First(x => x.Id == 2).Articles
                    .FirstOrDefault(x => string.Compare(barcode, x.CodeReference) == 0);

                CloseScanner();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
                CloseScanner();
                OpenScanner();
            }
            return articleTmp;
        }

        public static bool CheckArticleInDataGridView(string codeReference, DataGridView dataGridView, int colonneCible)
        {
            bool isArticleInDataGridView = false;
            try
            {
                int columnIndex = colonneCible;
                if (dataGridView.Columns[columnIndex] is DataGridViewComboBoxColumn comboBoxColumn)
                {
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        string dataInRow = row.Cells[columnIndex].Value as string;
                        if (string.IsNullOrWhiteSpace(dataInRow))
                            break;
                        // Comparez le texte avec le DisplayText de l'élément
                        isArticleInDataGridView = dataInRow.ToLowerInvariant().Contains(codeReference.ToLowerInvariant());
                        if (isArticleInDataGridView)
                            break;
                    }
                }
                else if (dataGridView.Columns[columnIndex] is DataGridViewTextBoxColumn textBoxColumn)
                {
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        string dataInRow = row.Cells[columnIndex].Value as string;
                        if (string.IsNullOrWhiteSpace(dataInRow))
                            continue;
                        // Comparez le texte avec le DisplayText de l'élément
                        isArticleInDataGridView = dataInRow.Trim().ToLowerInvariant().Contains(codeReference.Trim().ToLowerInvariant());
                        if (isArticleInDataGridView)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
            return isArticleInDataGridView;
        }
        public static bool CheckArticleInStock(string codeReference, List<Article> articles)
        {
            try
            {
                return articles.Any(x => x.DisplayText.Contains(codeReference));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
            return false;
        }

        public static (Marque, List<Marque> marques) AddNewMarque(string marqueName, List<Marque> marques)
        {
            int id = marques.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;
            string name = char.ToUpper(marqueName[0]) + marqueName.Substring(1);
            Marque newMarque = new Marque(id, name);
            marques.Add(newMarque);
            Tools.WriteLineTofile($"{newMarque.Id};{newMarque.Name}", Paths.MarquesDSPath, true);
            return (newMarque, marques);
        }
        public static (Modele, List<Modele> modeles) AddNewModele(string modeleName, int marqueId, List<Modele> modeles)
        {
            int id = modeles.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;
            int mId = marqueId;
            string name = char.ToUpper(modeleName[0]) + modeleName.Substring(1);
            Modele newModele = new Modele(id, mId, name);
            modeles.Add(newModele);
            Tools.WriteLineTofile($"{newModele.Id};{newModele.MarqueId};{newModele.Name}", Paths.ModelesDSPath, true);
            return (newModele, modeles);
        }
        public static (Article, List<Marque>, List<Modele>, List<Article>) AddNewArticle(
            string marqueName
            , string modeleName
            , string codeReference
            , string articleName
            , string price
            , List<Marque> marques
            , List<Modele> modeles
            , List<Article> articles)
        {
            Marque marque = null;
            Modele modele = null;
            string displayTextMarque = string.Empty;
            string displayTextModele = string.Empty;

            if (!string.IsNullOrWhiteSpace(marqueName))
            {
                marque = marques.FirstOrDefault(x => string.Compare(x.Name.Trim().ToLowerInvariant(), marqueName.Trim().ToLowerInvariant()) == 0);
                if (marque == null)
                {
                    (marque, marques) = AddNewMarque(marqueName, marques);
                }
                if (marque != null)
                {
                    displayTextMarque = $" - {marque.Name}";
                }
            }
            if (marque != null && !string.IsNullOrWhiteSpace(modeleName))
            {
                modele = modeles.FirstOrDefault(x => string.Compare(x.Name.Trim().ToLowerInvariant(), modeleName.Trim().ToLowerInvariant()) == 0 && x.MarqueId == marque.Id);
                if (modele == null)
                {
                    (modele, modeles) = AddNewModele(modeleName, marque.Id, modeles);
                }
                if (modele != null)
                {
                    displayTextModele = $" {modele.Name}";
                }
            }

            Article newArticle = new Article()
            {
                Id = articles.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1,
                CodeReference = codeReference,
                MarqueId = marque.Id,
                ModeleId = modele?.Id,
                Produit = articleName,
                Price = Decimal.Parse(price),
                Quantity = 99,
                DisplayText =
                    $"{articleName}" + displayTextMarque + displayTextModele + $" - {codeReference}",
            };
            articles.Add(newArticle);

            Tools.WriteLineTofile($"{newArticle.Id};{newArticle.MarqueId};{newArticle.ModeleId};{newArticle.Produit};{newArticle.Price};99;{newArticle.CodeReference}", Paths.StockDSPath, true);
            return (newArticle, marques, modeles, articles);
        }
    }
}