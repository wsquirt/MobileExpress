using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Document.NET;
using Xceed.Words.NET;

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
                Func<string, string, string, string, string> func = (tmpnom, tmpmarque, tmpmodele, tmpimei) =>
                {
                    return
                        !string.IsNullOrWhiteSpace(tmpmarque) && !string.IsNullOrWhiteSpace(tmpimei) ?
                            $"{tmpmarque} {tmpmodele} (IMEI:{tmpimei}) - {tmpnom}" :
                        string.IsNullOrWhiteSpace(tmpmarque) && !string.IsNullOrWhiteSpace(tmpimei) ?
                            $"(IMEI:{tmpimei}) {tmpnom}" :
                        !string.IsNullOrWhiteSpace(tmpmarque) && string.IsNullOrWhiteSpace(tmpimei) ?
                            $"{tmpmarque} {tmpmodele} - {tmpnom}" : $"{tmpnom}";
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

                    string articlename = type == 1 ?
                        (mEData.RepairTypeId.HasValue ? repairTypeNames[mEData.RepairTypeId.Value] :
                        mEData.UnlockTypeId.HasValue ? unlockTypeNames[mEData.UnlockTypeId.Value] :
                        articles[mEData.ArticleId.Value]) :
                        (mEData.RepairTypeId.HasValue ? repairTypeNames[mEData.RepairTypeId.Value] :
                        unlockTypeNames[mEData.UnlockTypeId.Value]);
                    string marquename = mEData.MarqueId.HasValue ? marques[mEData.MarqueId.Value] : string.Empty;
                    string modelename = mEData.ModeleId.HasValue ? modeles[mEData.ModeleId.Value] : string.Empty;
                    string imei = mEData.IMEI;

                    if (mEData.Verification)
                    {
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = func(articlename, marquename, modelename, imei),
                            Quantity = mEData.Quantity.ToString(),
                            Price = string.Empty,
                            Total = string.Empty,
                        });
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = "Vérification - " + func(articlename, marquename, modelename, imei),
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
                            Name = func(articlename, marquename, modelename, imei),
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
                            Name = "Remise - " + func(articlename, marquename, modelename, imei),
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
                            Name = $"Garantie {$"{mEData.Garantie.Value.ToString()} mois"}{((mEData.GarantieOption ?? false) ? " (Hors casse et oxydation)" : string.Empty)} - " + func(articlename, marquename, modelename, imei),
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
                    row1.Cells[0].Paragraphs.First().FontSize(docxData.Type == 0 ? 9 : 12);
                    row1.Cells[1].Paragraphs.First().Append(article.Quantity);
                    row1.Cells[1].Paragraphs.First().FontSize(docxData.Type == 0 ? 9 : 12);
                    row1.Cells[2].Paragraphs.First().Append(article.Price);
                    row1.Cells[2].Paragraphs.First().FontSize(docxData.Type == 0 ? 9 : 12);
                    row1.Cells[3].Paragraphs.First().Append(article.Total);
                    row1.Cells[3].Paragraphs.First().FontSize(docxData.Type == 0 ? 9 : 12);
                }
                if (docxData.Type == 0)
                {
                    var table2 = doc.Tables[12];
                    // Remplir la table avec les données des articles
                    foreach (var article in docxData.Articles)
                    {
                        var row2 = table2.InsertRow();
                        row2.Cells[0].Paragraphs.First().Append(article.Name);
                        row2.Cells[0].Paragraphs.First().FontSize(9);
                        row2.Cells[1].Paragraphs.First().Append(article.Quantity);
                        row2.Cells[1].Paragraphs.First().FontSize(9);
                        row2.Cells[2].Paragraphs.First().Append(article.Price);
                        row2.Cells[2].Paragraphs.First().FontSize(9);
                        row2.Cells[3].Paragraphs.First().Append(article.Total);
                        row2.Cells[3].Paragraphs.First().FontSize(9);
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
        public static DataGridViewTextBoxColumn TakeOverAchatMarqueColumn = new DataGridViewTextBoxColumn
        {
            HeaderText = "Marque",
            Name = "marque",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverAchatModeleColumn = new DataGridViewTextBoxColumn
        {
            HeaderText = "Modèle",
            Name = "modele",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverAchatArticleColumn = new DataGridViewTextBoxColumn
        {
            HeaderText = "Article",
            Name = "article",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(string),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverAchatQuantityColumn = new DataGridViewTextBoxColumn()
        {
            HeaderText = "Quantité",
            Name = "quantity",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(int),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewTextBoxColumn TakeOverAchatPriceColumn = new DataGridViewTextBoxColumn()
        {
            HeaderText = "Prix",
            Name = "price",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
            ValueType = typeof(decimal),
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewCheckBoxColumn TakeOverAchatGarantieColumn = new DataGridViewCheckBoxColumn()
        {
            HeaderText = "Garantie",
            Name = "garantie",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewCheckBoxColumn TakeOverAchatRemiseColumn = new DataGridViewCheckBoxColumn()
        {
            HeaderText = "Remise",
            Name = "remise",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            Visible = true,
            ReadOnly = false,
        };
        public static DataGridViewButtonColumn TakeOverAchatDeleteColumn = new DataGridViewButtonColumn()
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
        #endregion
        #region Relation client
        #endregion
        #region Stock
        #endregion
    }
}