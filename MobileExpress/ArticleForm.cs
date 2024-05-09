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
        private delegate void updateDelegate(string txt);

        List<Article> Articles = new List<Article>();
        List<Marque> Marques = new List<Marque>();
        List<Modele> Modeles = new List<Modele>();
        string CodeReference = null;
        Article Article;
        StockAction Action = StockAction.Achat;
        public ArticleForm(
            StockAction action
            , List<Article> articles
            , List<Marque> marques
            , List<Modele> modeles
            , string codeReference = null
            , Article article = null)
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
                CodeReference = codeReference;
                Action = action;

                if (Action == StockAction.Ajout)
                {
                    labelArticleId.Text = (Articles.OrderByDescending(x => x.Id).First().Id + 1).ToString();
                    textBoxCodeRef.Text = article?.CodeReference ?? CodeReference ?? string.Empty;
                    textBoxQuantity.Text = "1";

                    textBoxMarque.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    textBoxMarque.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBoxMarque.AutoCompleteCustomSource.AddRange(marques.Select(m => m.Name).ToArray());
                    textBoxMarque.TextChanged += TextBoxMarque_TextChanged;

                    textBoxModele.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    textBoxModele.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBoxModele.AutoCompleteCustomSource.AddRange(Modeles.Select(m => m.Name).ToArray());

                    buttonStockDelete.Visible = false;
                    buttonStockDelete.Enabled = false;
                }

                if (Action == StockAction.Achat)
                {
                    labelArticleId.Text = article.Id.ToString();
                    textBoxCodeRef.Text = article.CodeReference;
                    textBoxMarque.Text = Marques.FirstOrDefault(x => x.Id == article.MarqueId)?.Name ?? string.Empty;
                    textBoxModele.Text = Modeles.FirstOrDefault(x => article.MarqueId != null && x.Id == article.ModeleId)?.Name ?? string.Empty;
                    textBoxProduit.Text = article.Produit;
                    textBoxPrix.Text = article.Price.ToString();
                    textBoxQuantity.Text = "1";

                    textBoxMarque.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    textBoxMarque.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBoxMarque.AutoCompleteCustomSource.AddRange(marques.Select(m => m.Name).ToArray());
                    textBoxMarque.TextChanged += TextBoxMarque_TextChanged;

                    textBoxModele.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    textBoxModele.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBoxModele.AutoCompleteCustomSource.AddRange(Modeles.Select(m => m.Name).ToArray());

                    buttonStockDelete.Visible = false;
                    buttonStockDelete.Enabled = false;
                }

                if (Action == StockAction.MiseAJour && article != null)
                {
                    labelArticleId.Text = article.Id.ToString();
                    textBoxCodeRef.Text = article.CodeReference;

                    textBoxMarque.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    textBoxMarque.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBoxMarque.AutoCompleteCustomSource.AddRange(marques.Select(m => m.Name).ToArray());
                    // Pré-sélection de la marque par défaut
                    textBoxMarque.Text = Marques.FirstOrDefault(x => x.Id == article.MarqueId)?.Name ?? string.Empty;
                    textBoxMarque.TextChanged += TextBoxMarque_TextChanged;

                    // Pré-sélection du modèle par défaut
                    textBoxModele.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    textBoxModele.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    if (article.MarqueId == null)
                    {
                        textBoxModele.AutoCompleteCustomSource.AddRange(Modeles.Select(m => m.Name).ToArray());
                    }
                    else if (article.ModeleId == null)
                    {
                        textBoxModele.AutoCompleteCustomSource.AddRange(Modeles.Where(x => x.MarqueId == article.MarqueId).Select(m => m.Name).ToArray());
                    }
                    else
                    {
                        textBoxModele.AutoCompleteCustomSource.AddRange(Modeles.Where(x => x.MarqueId == article.MarqueId).Select(m => m.Name).ToArray());
                        textBoxModele.Text = Modeles.FirstOrDefault(x => x.Id == article.ModeleId.Value && x.MarqueId == article.MarqueId.Value)?.Name ?? string.Empty;
                    }

                    textBoxProduit.Text = article.Produit;
                    textBoxPrix.Text = article.Price.ToString();
                    textBoxQuantity.Text = article.Quantity.ToString();

                    buttonStockDelete.Visible = true;
                    buttonStockDelete.Enabled = true;
                }
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
        private void buttonValidate_Click(object sender, EventArgs e)
        {
            try
            {
                string codeRef = textBoxCodeRef.Text.Trim();
                string produit = textBoxProduit.Text.Trim();
                string prixText = textBoxPrix.Text.Trim();
                string quantityText = textBoxQuantity.Text.Trim();
                string productText = textBoxProduit.Text.Trim();

                if (string.IsNullOrWhiteSpace(codeRef))
                {
                    MessageBox.Show("Veuillez renseigner un code de référence.", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(prixText))
                {
                    MessageBox.Show("Veuillez renseigner un prix.", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(quantityText))
                {
                    MessageBox.Show("Veuillez renseigner une quantité.", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                string marqueName = textBoxMarque.Text.Trim();
                Marque marque = null;
                if (!string.IsNullOrWhiteSpace(marqueName))
                {
                    marque = Marques.FirstOrDefault(x =>
                        string.Compare(x.Name.Trim().ToLowerInvariant(), marqueName.Trim().ToLowerInvariant(), true) == 0);
                    if (marque == null)
                    {
                        (Marque marqueTmp, List<Marque> marques) = Services.AddNewMarque(marqueName, Marques);
                        marque = marqueTmp;
                        Marques = marques;
                        ;
                    }
                }

                string modeleName = textBoxModele.Text.Trim();
                Modele modele = null;
                if (marque != null && !string.IsNullOrWhiteSpace(modeleName))
                {
                    modele = Modeles.FirstOrDefault(x =>
                        string.Compare(x.Name.Trim().ToLowerInvariant(), modeleName.Trim().ToLowerInvariant(), true) == 0 &&
                        x.MarqueId == marque.Id);
                    if (modele == null)
                    {
                        (Modele modeleTmp, List<Modele> modeles) = Services.AddNewModele(modeleName, marque.Id, Modeles);
                        modele = modeleTmp;
                        Modeles = modeles;
                        ;
                    }
                }

                int id = int.Parse(labelArticleId.Text);
                decimal prix = decimal.Parse(prixText);
                int quantity = int.Parse(quantityText);

                Article existingArticle = Articles.FirstOrDefault(x => x.Id == id);
                if (existingArticle == null)
                {
                    (Article articleTmp, List<Marque> marques, List<Modele> modeles, List<Article> articles) = Services.AddNewArticle(
                        marqueName
                        , modeleName
                        , codeRef
                        , productText
                        , prixText
                        , Marques
                        , Modeles
                        , Articles);
                    Article = articleTmp;
                    Articles = articles;
                    ;
                }
                else
                {
                    Article = existingArticle;
                    Article.Produit = produit;
                    Article.MarqueId = marque?.Id;
                    Article.ModeleId = modele?.Id;
                    Article.Price = prix;
                    Article.Quantity = quantity;
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
                    DialogResult dialogResult = MessageBox.Show($"Êtes-vous sûr(e) de vouloir supprimer l'article {Article.Produit}.", "Alerte", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Action = StockAction.Suppression;
                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                    }
                }
                else if (Article != null)
                {
                    MessageBox.Show($"Il vous est impossible de supprimer l'article {Article.Produit} du stock car vous n'êtes pas dans le bon contexte.", "Alerte", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void TextBoxMarque_TextChanged(object sender, EventArgs e)
        {
            string marqueName = textBoxMarque.Text.Trim().ToLower();

            int? selectedMarqueId = Marques.FirstOrDefault(x => x.Name.Trim().ToLower() == marqueName)?.Id;

            if (selectedMarqueId.HasValue)
            {
                string[] modeles = Modeles.Where(m => m.Id == selectedMarqueId.Value)?.Select(x => x.Name).ToArray();

                if (modeles != null && modeles.Any())
                {
                    textBoxModele.AutoCompleteCustomSource.Clear();
                    textBoxModele.AutoCompleteCustomSource.AddRange(modeles);
                }
                else
                {
                    textBoxModele.AutoCompleteCustomSource.Clear();
                    textBoxModele.AutoCompleteCustomSource.AddRange(Modeles.Select(x => x.Name).ToArray());
                }
            }
        }
    }
}