using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Timers;
using System.Windows.Forms;

namespace MobileExpress
{
    public partial class ArticleForm : Form
    {
        private static SerialPort currentPort = new SerialPort();
        private static System.Timers.Timer aTimer;
        private delegate void updateDelegate(string txt);

        List<Article> Articles = new List<Article>();
        List<Marque> Marques = new List<Marque>();
        List<Modele> Modeles = new List<Modele>();
        Article Article;
        StockAction Action = StockAction.Achat;
        public ArticleForm(StockAction action, Article article, List<Article> articles, List<Marque> marques, List<Modele> modeles)
        {
            InitializeComponent();

            try
            {
                Modeles.Clear();
                Modeles.AddRange(modeles);
                Marques.Clear();
                Marques.AddRange(marques);
                Articles.Clear();
                Articles.AddRange(articles);

                Article = article;
                Action = action;
                if (Action == StockAction.Ajout)
                {
                    labelArticleId.Text = (Articles.OrderByDescending(x => x.Id).First().Id + 1).ToString();
                }
                if (Action == StockAction.MiseAJour && article != null)
                {
                    labelArticleId.Text = article.Id.ToString();
                    textBoxUPC.Text = article.UPC;
                    textBoxEAN.Text = article.EAN;
                    textBoxGTIN.Text = article.GTIN;
                    textBoxISBN.Text = article.ISBN;
                    textBoxMarque.Text = Marques.FirstOrDefault(x => x.Id == (article.MarqueId ?? 0))?.Name ?? string.Empty;
                    textBoxModele.Text = Modeles.FirstOrDefault(x => x.Id == (article.ModeleId ?? 0) && x.MarqueId == (article.MarqueId ?? 0))?.Name ?? string.Empty;
                    textBoxProduit.Text = article.Name;
                    textBoxPrix.Text = article.Price.ToString();
                }
                if (Action == StockAction.Achat)
                {
                    buttonStockDelete.Visible = false;
                    buttonStockDelete.Enabled = false;
                }

                if (currentPort.IsOpen)
                    currentPort.Close();

                currentPort.PortName = "COM3";
                currentPort.BaudRate = 19200;
                currentPort.ReadTimeout = 1000;
                currentPort.Parity = Parity.None;
                currentPort.StopBits = StopBits.One;
                currentPort.DataBits = 8;

                aTimer = new System.Timers.Timer(1000);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        public (StockAction, Article, List<Article>, List<Marque>, List<Modele>) GetResult()
        {
            return (Action, Article, Articles, Marques, Modeles);
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (!currentPort.IsOpen)
            {
                currentPort.Open();
                System.Threading.Thread.Sleep(100); /// for recieve all data from scaner to buffer
                currentPort.DiscardInBuffer();      /// clear buffer          
            }
            try
            {
                string strFromPort = currentPort.ReadExisting().Replace("\r", "").Replace("\n", "");
                textBoxUPC.BeginInvoke(new updateDelegate(updateTextBox), strFromPort);
            }
            catch { }
        }
        private void updateTextBox(string txt)
        {
            if (txt.Length != 0)
            {
                aTimer.Stop();
                aTimer.Dispose();
                currentPort.Close();
                bool result = GetDataFromDB(txt);
                if (!result)
                    GetDataFromURL(txt.Replace("\r", "").Replace("\n", ""));
            }
        }
        private async void GetDataFromURL(string barcode)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($@"https://api.upcitemdb.com/prod/trial/lookup?upc={barcode}"),
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Root test = JsonConvert.DeserializeObject<Root>(body);
                    if (test != null && test.total > 0)
                    {
                        textBoxUPC.Text = test.items.First().upc;
                        textBoxEAN.Text = test.items.First().ean;
                        textBoxGTIN.Text = test.items.First().gtin;
                        textBoxISBN.Text = test.items.First().isbn;
                        textBoxMarque.Text = string.IsNullOrWhiteSpace(test.items.First().brand) ? textBoxMarque.Text : test.items.First().brand;
                        textBoxModele.Text = string.IsNullOrWhiteSpace(test.items.First().model) ? textBoxModele.Text : test.items.First().model;
                        textBoxProduit.Text = string.IsNullOrWhiteSpace(test.items.First().title) ? textBoxProduit.Text : test.items.First().title;
                        if (currentPort.IsOpen)
                            currentPort.Close();
                    }
                    else if (test != null)
                    {
                        textBoxUPC.Text = barcode;
                        MessageBox.Show("Aucune information a été trouvée. Veuillez renseigner le reste des informations.", "Alerte", MessageBoxButtons.OK);
                        if (currentPort.IsOpen)
                            currentPort.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
                if (currentPort.IsOpen)
                    currentPort.Close();
                currentPort.Open();
            }
        }
        private bool GetDataFromDB(string barcode)
        {
            try
            {
                Article article = Articles.FirstOrDefault(x =>
                string.Compare(barcode, x.UPC) == 0 ||
                string.Compare(barcode, x.EAN) == 0 ||
                string.Compare(barcode, x.GTIN) == 0 ||
                string.Compare(barcode, x.ISBN) == 0);

                if (article == null && Action == StockAction.Achat)
                {
                    DialogResult result = MessageBox.Show("L'article n'est pas connu du système, voulez-vous l'ajouter ?", "Alerte", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Action = StockAction.AchatAjout;
                        labelArticleId.Text = (Articles.OrderByDescending(x => x.Id).First().Id + 1).ToString();
                        textBoxQuantity.Text = string.IsNullOrWhiteSpace(textBoxQuantity.Text) ? "1" : textBoxQuantity.Text;
                    }
                }
                else if (article == null)
                    return false;

                if (article != null && Action == StockAction.Ajout)
                {
                    Action = StockAction.MiseAJour;
                    labelArticleId.Text = article.Id.ToString();
                    textBoxQuantity.Text = "99";
                }
                if (Action == StockAction.Achat)
                {
                    labelArticleId.Text = article.Id.ToString();
                }

                if (Action == StockAction.Achat || Action == StockAction.Ajout)
                {
                    textBoxPrix.Text = article.Price.ToString();
                }

                if (Action == StockAction.MiseAJour)
                {
                    textBoxQuantity.Text = article.Quantity.ToString();
                }
                else if (Action == StockAction.AchatAjout)
                    return false;

                textBoxUPC.Text = article.UPC;
                textBoxEAN.Text = article.EAN;
                textBoxGTIN.Text = article.GTIN;
                textBoxISBN.Text = article.ISBN;
                textBoxMarque.Text = Marques.First(x => x.Id == article.MarqueId).Name;
                textBoxModele.Text = Modeles.First(x => x.Id == article.ModeleId && x.MarqueId == article.MarqueId).Name;
                textBoxProduit.Text = article.Name;
                if (currentPort.IsOpen)
                    currentPort.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
                if (currentPort.IsOpen)
                    currentPort.Close();
                currentPort.Open();
                return false;
            }
        }

        private void buttonValidate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxUPC.Text) &&
                    string.IsNullOrWhiteSpace(textBoxEAN.Text) &&
                    string.IsNullOrWhiteSpace(textBoxGTIN.Text) &&
                    string.IsNullOrWhiteSpace(textBoxISBN.Text))
                {
                    MessageBox.Show("Veuillez renseigner un UPC, un EAN, un GTIN et/ou un ISBN.", "Alerte", MessageBoxButtons.OK);
                    return;
                }
                if (string.IsNullOrWhiteSpace(textBoxPrix.Text) ||
                    string.IsNullOrWhiteSpace(textBoxQuantity.Text))
                {
                    MessageBox.Show("Veuillez renseigner un prix et une quantité.", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                string marqueName = textBoxMarque.Text;
                Marque marque = Marques.FirstOrDefault(x => string.Compare(x.Name.ToLower(), marqueName.ToLower()) == 0);
                if (marque == null)
                {
                    marque = new Marque(Marques.OrderByDescending(x => x.Id).First().Id + 1, char.ToUpper(marqueName[0]) + marqueName.Substring(1));
                    Marques.Add(marque);
                    Tools.WriteLineTofile($"{marque.Id};{marque.Name}", Paths.MarquesDSPath, true);
                }
                string modeleName = textBoxModele.Text;
                Modele modele = Modeles.FirstOrDefault(x => x.Name.ToLower() == modeleName.ToLower() && x.MarqueId == marque.Id);
                if (modele == null && !string.IsNullOrWhiteSpace(modeleName))
                {
                    modele = new Modele(Modeles.OrderByDescending(x => x.Id).First().Id + 1, marque.Id, char.ToUpper(modeleName[0]) + modeleName.Substring(1));
                    Modeles.Add(modele);
                    Tools.WriteLineTofile($"{modele.Id};{modele.MarqueId};{modele.Name}", Paths.ModelesDSPath, true);
                }

                string upc = textBoxUPC.Text;
                string ean = textBoxEAN.Text;
                string gtin = textBoxGTIN.Text;
                string isbn = textBoxISBN.Text;
                string name = textBoxProduit.Text;
                int? marqueId = marque?.Id;
                int? modeleId = modele?.Id;
                decimal prix = decimal.Parse(textBoxPrix.Text);
                int quantity = int.Parse(textBoxQuantity.Text);
                int id = int.Parse(labelArticleId.Text);

                Article = new Article()
                {
                    Id = id,
                    UPC = upc,
                    EAN = ean,
                    GTIN = gtin,
                    ISBN = isbn,
                    Name = name,
                    MarqueId = marqueId,
                    ModeleId = modeleId,
                    Price = prix,
                    Quantity = quantity,
                };
                if (Action == StockAction.Achat)
                {
                    Articles.ForEach(x =>
                    {
                        if (x.Id == Article.Id)
                        {
                            x.Quantity -= Article.Quantity;
                        }
                    });
                }
                else if (Action == StockAction.MiseAJour)
                {
                    Articles.ForEach(x =>
                    {
                        if (x.Id == Article.Id)
                        {
                            x.UPC = Article.UPC;
                            x.EAN = Article.EAN;
                            x.GTIN = Article.GTIN;
                            x.ISBN = Article.ISBN;
                            x.Name = Article.Name;
                            x.MarqueId = Article.MarqueId;
                            x.ModeleId = Article.ModeleId;
                            x.Price = Article.Price;
                            x.Quantity = Article.Quantity;
                        }
                    });
                }
                else if (Action == StockAction.AchatAjout)
                {
                    Articles.Add(Article);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void buttonStockDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Action == StockAction.MiseAJour && Article != null)
                {
                    DialogResult dialogResult = MessageBox.Show($"Êtes-vous sûr(e) de vouloir supprimer l'article {Article.Name}.", "Alerte", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Action = StockAction.Suppression;
                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                    }
                }
                else if (Article != null)
                {
                    MessageBox.Show($"Il vous est impossible de supprimer l'article {Article.Name} du stock car vous n'êtes pas dans le bon contexte.", "Alerte", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
    }
}