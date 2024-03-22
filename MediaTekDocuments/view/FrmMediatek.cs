using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Shapes;
using System.Windows.Documents;
using Newtonsoft.Json.Bson;
using System.Diagnostics;
using System.Text;
using System.Net;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private int service;
        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek(int service)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            this.service = service;
            AuthorizationCheck();
            RemplirComboBoxSuivi();
           
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres;

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    Console.WriteLine("btnLivresNumRecherche activated");
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Parutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocument = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocument);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = System.IO.Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }


        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        #endregion

        #region Onglet CommandesLivres
        private readonly BindingSource bindingSourceCommandesLivre = new BindingSource();
        private List<CommandeDocument> lesCommandesDocument = new List<CommandeDocument>();
        bool canUpdateSuivi = false;
        /// <summary>
        /// Ouverture de l'onglet Commandes de livres :
        /// appel des méthodes pour remplir le datagrid des commandes de livre et du combo "suivi"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandesLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();

            // Remplir la ComboBox
            RemplirComboBoxSuivi();

            // Sélectionner automatiquement le premier élément
            comboBoxSuivi.SelectedIndex = 0;
        }

        /// <summary>
        /// recherche les information sur le document sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRechercheNumDoc_Click(object sender, EventArgs e)
        {
            string numdoc = textBoxNumDoc.Text;
            Console.WriteLine("button recherche num doc " + numdoc);
            if (numdoc != null)
            {
                lesCommandesDocument = controller.GetCommandesDocuments(numdoc);
                foreach (CommandeDocument commande in lesCommandesDocument)
                {
                    Console.WriteLine("ID: " + commande.Id + ", Nb Exemplaires: " + commande.NbExemplaire + ", ID Livre/DVD: " + commande.IdLivreDvd + ", ID Suivi: " + commande.IdSuivi + ", Date: " + commande.Date + ", Montant: " + commande.Montant);
                }

                RemplirCommandesLivresListe(lesCommandesDocument);
            }


            if (!textBoxNumDoc.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(numdoc));
                if (livre != null)
                {
                    AfficheLivreCommandeInfos(livre);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Remplit la datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesDocument">Liste des commandes d'un document</param>
        private void RemplirCommandesLivresListe(List<CommandeDocument> lesCommandesDocument)
        {
            if (lesCommandesDocument != null)
            {
                bindingSourceCommandesLivre.DataSource = lesCommandesDocument;
                dataGridViewLivres.DataSource = bindingSourceCommandesLivre;
                foreach (CommandeDocument commande in lesCommandesDocument)
                {
                    Console.WriteLine(" remplircommandeslivres liste id " + commande.Id + "date " + commande.Date + "libelle " + commande.Suivi.Libelle);
                }
                dataGridViewLivres.Columns["id"].Visible = true;
                dataGridViewLivres.Columns["idLivreDvd"].Visible = false;
                dataGridViewLivres.Columns["idSuivi"].Visible = false;
                dataGridViewLivres.Columns["Suivi"].Visible = false;
                dataGridViewLivres.Columns["Libelle"].Visible = false;
                dataGridViewLivres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridViewLivres.Columns["date"].DisplayIndex = 4;
                dataGridViewLivres.Columns["montant"].DisplayIndex = 1;
                dataGridViewLivres.Columns["id"].DisplayIndex = 0;
                dataGridViewLivres.Columns[4].HeaderCell.Value = "Date de commande";
                dataGridViewLivres.Columns[1].HeaderCell.Value = "Nombre d'exemplaires";
                
                dataGridViewLivres.Columns["LibelleSuivi"].HeaderText = "Suivi";


            }
            else
            {
                bindingSourceCommandesLivre.DataSource = null;
            }
        }
        /// <summary>
        /// efface les champs d'information sur le document lorsque le numéro est changé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNumDoc_TextChanged(object sender, EventArgs e)
        {

            textBoxTitre.Text = "";
            textBoxAuteur.Text = "";
            textBoxCollection.Text = "";
            textBoxGenre.Text = "";
            textBoxPublic.Text = "";
            textBoxRayon.Text = "";
            RemplirCommandesLivresListe(null);
            comboBoxSuivi.SelectedIndex = 0;
            numeroSuivi.Text = null;
            
        }
        /// <summary>
        /// afficher les information sur le livre recherché
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheLivreCommandeInfos(Livre livre)
        {
            textBoxTitre.Text = livre.Titre;
            textBoxAuteur.Text = livre.Auteur;
            textBoxCollection.Text = livre.Collection;
            textBoxGenre.Text = livre.Genre;
            textBoxPublic.Text = livre.Public;
            textBoxRayon.Text = livre.Rayon;

            string image = livre.Image;
            try
            {
                pictureBoxLivre.Image = Image.FromFile(image);
            }
            catch
            {
                pictureBoxLivre.Image = null;
            }


        }


        /// <summary>
        /// créé un nouveau document
        /// </summary>
        /// <returns></returns>
        private CommandeDocument CreateCommandeDocument()
        {
            string numcommande = GetLastNumCommandes();
            string numlivre = textBoxAddNumLivre.Text;
            int nbExemplaire = Convert.ToInt32(textBoxAddNbEx.Text);
            double montant = Convert.ToDouble(textBoxAddMontant.Text);
            DateTime date = dateTimePickerAdd.Value;
            Suivi suivi = CreateSuivi();
            if (numcommande != "" && numlivre != "" && nbExemplaire != null && date != null && suivi != null)
            {
                CommandeDocument commande = new CommandeDocument(numcommande, date, montant, nbExemplaire, numlivre, suivi.Id, suivi.Libelle);

                return commande;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// créé un nouveau suvi
        /// </summary>
        /// <returns></returns>
        private Suivi CreateSuivi()
        {
            List<Suivi> lesSuivi = controller.GetAllSuivi();
            int idSuivi = 0;
            string Libelle = "En cours";
            foreach (Suivi suivi in lesSuivi)
            {

                if (Convert.ToInt32(suivi.Id) > idSuivi)
                {
                    idSuivi = Convert.ToInt32(suivi.Id);
                }

            }
            string newId = (idSuivi + 1).ToString();
            Suivi newSuivi = new Suivi(newId, Libelle);
            Console.WriteLine("create suivi id " + newId);
            return newSuivi;
        }
        /// <summary>
        /// récupère le dernier numéro de commande et retourne ce nombre +1
        /// </summary>
        /// <returns></returns>
        private string GetLastNumCommandes()
        {
            List<Commande> commandes = controller.GetAllCommandes();
            int numCommande = 0;
            foreach (Commande commande in commandes)
            {
                if (Convert.ToInt32(commande.Id) > numCommande)
                {
                    numCommande = Convert.ToInt32(commande.Id);
                }
            }
            string newNumCommande = (numCommande + 1).ToString();
            return newNumCommande;

        }
        /// <summary>
        /// ajoute une nouvelle commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddCom_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBoxAddNumLivre.Text) &&
                !string.IsNullOrEmpty(textBoxAddMontant.Text) &&
                !string.IsNullOrEmpty(textBoxAddNbEx.Text))

            {
                bool livreExsite = false;
                string numlivre = null;
                foreach (Livre livre in lesLivres)
                {
                    if (textBoxAddNumLivre.Text == livre.Id)
                    {
                        livreExsite = true;
                        numlivre = livre.Id;
                        break;
                    }
                }
                if (livreExsite == true)
                {
                    Console.WriteLine("clik validé xxxxxxxxxxxxxxxxxxxx");
                    CommandeDocument commande = CreateCommandeDocument();

                    controller.AddCommandeDocument(commande);
                    
                    textBoxNumDoc.Text = numlivre;
                    buttonRechercheNumDoc_Click(buttonRechercheNumDoc, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Livre non exsitant.");
                }

            }
            else
            {
                MessageBox.Show("Veuillez remplir toutes les informations sur la commande.");

            }


        }
        //permet de suivre le numero de commande selectionné
        string selectedCommandeDocument = null;
        /// <summary>
        /// permet de selectionnner une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewLivres_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Accéder à la ligne où le clic a eu lieu
                DataGridViewRow row = dataGridViewLivres.Rows[e.RowIndex];


                string idcommande = row.Cells[3].Value.ToString();
                selectedCommandeDocument = idcommande;
                numeroSuivi.Text = idcommande;
                comboBoxSuivi.SelectedIndex = 2;
                try
                {
                    string libelle = row.Cells[8].Value.ToString();
                    switch (libelle)
                    {
                        case "En cours":
                            comboBoxSuivi.SelectedIndex = 0;
                            break;
                        case "Relancée":
                            comboBoxSuivi.SelectedIndex = 1;
                            break;
                        case "Livrée":
                            comboBoxSuivi.SelectedIndex = 2;
                            break;
                        case "Réglée":
                            comboBoxSuivi.SelectedIndex = 3;
                            break;
                    }
                    Console.WriteLine("Vous avez cliqué sur la commande : " + idcommande);
                    Console.WriteLine("Le suivi est  : " + libelle);
                }
                catch
                {
                    Console.WriteLine("erreur de selected value");
                }

            }

        }
        /// <summary>
        /// permet la modification de l'étape de suivi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxSuivi_DropDown(object sender, EventArgs e)
        {
            canUpdateSuivi = true;
        }
        /// <summary>
        /// Change l'etape de suivi dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxSuivi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (numeroSuivi.Text != null && numeroSuivi.Text != "" && canUpdateSuivi == true)
            {
                string numSuivi = numeroSuivi.Text;


                string libelle = comboBoxSuivi.Text;
                controller.ChangeSuivi(numSuivi, libelle);
                Console.WriteLine("The combobox selected suivi is " + libelle);
                canUpdateSuivi = false;

                foreach (CommandeDocument commande in lesCommandesDocument)
                {
                    if (commande.Id == numSuivi)
                    {
                        commande.Suivi.Libelle = libelle;
                        Console.WriteLine("commade document modifiée " + commande.Id);
                        break;
                    }

                }
                bindingSourceCommandesLivre.DataSource = lesCommandesDocument;
                bindingSourceCommandesLivre.ResetBindings(false);

                dataGridViewLivres.DataSource = bindingSourceCommandesLivre;
            }
        }

        /// <summary>
        /// rempli les différents options de suivi
        /// </summary>
        private void RemplirComboBoxSuivi()
        {

            comboBoxSuivi.Items.Clear();
            comboBoxSuivi.Items.Add("En cours");
            comboBoxSuivi.Items.Add("Relancée");
            comboBoxSuivi.Items.Add("Livrée");
            comboBoxSuivi.Items.Add("Réglée");
            Console.WriteLine("RemplirComboBoxSuivi executed. Items count: " + comboBoxSuivi.Items.Count);

        }
        /// <summary>
        /// prépare l'onglet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMediatek_Load(object sender, EventArgs e)
        {
            RemplirComboBoxSuivi();
            RemplirComboBoxSuiviDVD();
            comboBoxSuivi.SelectedIndex = 0; // Sélectionne le premier élément
            comboBoxSuiviDVD.SelectedIndex = 0;
        }
        /// <summary>
        /// supprime une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelCom_Click(object sender, EventArgs e)
        {
            foreach (CommandeDocument commande in lesCommandesDocument)
            {
                Console.WriteLine("seleted commande id is " + selectedCommandeDocument + " and this command id is " + commande.Id);
                if (commande.Id == selectedCommandeDocument)
                {
                    Console.WriteLine("Delete commande numùber " + commande.Id);

                    controller.DeleteCommandeDocument(commande);
                    lesCommandesDocument.Remove(commande);
                    bindingSourceCommandesLivre.DataSource = lesCommandesDocument;
                    bindingSourceCommandesLivre.ResetBindings(false);

                    dataGridViewLivres.DataSource = bindingSourceCommandesLivre;
                    numeroSuivi.Text = null;
                    selectedCommandeDocument = null;


                    break;
                }
            }


        }
        /// <summary>
        /// seul les chiffres 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxAddNumLivre_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }

        private void textBoxAddNbEx_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }

        private void textBoxAddMontant_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }

        private void textBoxNumDoc_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }

        bool orderLivreASC = true;
        private void dataGridViewLivres_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {


            string titreColonne = dataGridViewLivres.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Id":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesDocument.OrderBy(o => o.Id).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesDocument.OrderByDescending(o => o.Id).ToList();
                        orderLivreASC = true;
                    }
                    break;
                case "Date de commande":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesDocument.OrderBy(o => o.Date).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesDocument.OrderByDescending(o => o.Date).ToList();
                        orderLivreASC = true;
                    }
                    break;
                case "Montant":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesDocument.OrderBy(o => o.Montant).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesDocument.OrderByDescending(o => o.Montant).ToList();
                        orderLivreASC = true;
                    }
                    break;
                case "NbExemplaire":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesDocument.OrderBy(o => o.NbExemplaire).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesDocument.OrderByDescending(o => o.NbExemplaire).ToList();
                        orderLivreASC = true;
                    }
                    break;

            }

            RemplirCommandesLivresListe(sortedList);
        }

        #endregion

        #region Onglet CommandesDVD

        private readonly BindingSource bindingSourceCommandesDVD = new BindingSource();
        private List<CommandeDocument> lesCommandesDVD = new List<CommandeDocument>();
        bool canUpdateSuiviDVD = false;

        private void tabCommandeDVD_Enter(object sender, EventArgs e)
        {
            
            lesDvd = controller.GetAllDvd();
            foreach (var dvd in lesDvd)
            {
                Console.Write("dvd ajouté id n° "+dvd.Id);
            }
            // Remplir la ComboBox
            RemplirComboBoxSuiviDVD();

            // Sélectionner automatiquement le premier élément
            comboBoxSuivi.SelectedIndex = 0;
            lesCommandesDVD = controller.GetAllCommandesDocument();
        }

        
        /// <summary>
        /// recherche  les information sur le dvd selectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRechercheDVD_Click(object sender, EventArgs e)
        {
            string numdoc = textBoxNumDVD.Text;
            Console.WriteLine("button recherche num dvd " + numdoc);
            if (numdoc != null)
            {
                lesCommandesDVD = controller.GetCommandesDVD(numdoc);
                foreach (CommandeDocument commande in lesCommandesDVD)
                {
                    Console.WriteLine("ID: " + commande.Id + ", Nb Exemplaires: " + commande.NbExemplaire + ", ID Livre/DVD: " + commande.IdLivreDvd + ", ID Suivi: " + commande.IdSuivi + ", Date: " + commande.Date + ", Montant: " + commande.Montant);
                }

                RemplirCommandesDVDListe(lesCommandesDVD);
            }


            if (!textBoxNumDVD.Text.Equals(""))
            {
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(numdoc));
                if (dvd != null)
                {
                    AfficheDVDCommandeInfos(dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Remplit la datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesDocument">Liste des commandes d'un document</param>
        private void RemplirCommandesDVDListe(List<CommandeDocument> lesCommandesDVD)
        {
            if (lesCommandesDVD != null)
            {
                bindingSourceCommandesDVD.DataSource = lesCommandesDVD;
                dataGridViewDVD.DataSource = bindingSourceCommandesDVD;
                foreach (CommandeDocument commande in lesCommandesDVD)
                {
                    Console.WriteLine(" remplircommandeslivres liste id " + commande.Id + "date " + commande.Date + "libelle " + commande.Suivi.Libelle);
                }
                dataGridViewDVD.Columns["id"].Visible = true;
                dataGridViewDVD.Columns["idLivreDvd"].Visible = false;
                dataGridViewDVD.Columns["idSuivi"].Visible = false;
                dataGridViewDVD.Columns["Suivi"].Visible = false;
                dataGridViewDVD.Columns["Libelle"].Visible = false;
                dataGridViewDVD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridViewDVD.Columns["date"].DisplayIndex = 4;
                dataGridViewDVD.Columns["montant"].DisplayIndex = 1;
                dataGridViewDVD.Columns["id"].DisplayIndex = 0;
                dataGridViewDVD.Columns[4].HeaderCell.Value = "Date de commande";
                dataGridViewDVD.Columns[1].HeaderCell.Value = "Nombre d'exemplaires";
                dataGridViewDVD.Columns["LibelleSuivi"].HeaderText = "Suivi";


            }
            else
            {
                bindingSourceCommandesDVD.DataSource = null;
            }
        }
        /// <summary>
        /// affifche les information sur le dvd
        /// </summary>
        /// <param name="dvd"></param>
        private void AfficheDVDCommandeInfos(Dvd dvd)
        {
            textBoxTitreDVD.Text = dvd.Titre;
            textBoxRealDVD.Text = dvd.Realisateur;
            textBoxDureeDVD.Text = dvd.Duree.ToString();
            textBoxGenreDVD.Text = dvd.Genre;
            textBoxPublicDVD.Text = dvd.Public;
            textBoxRayonDVD.Text = dvd.Rayon;
            textBoxSynopsisDVD.Text = dvd.Synopsis;

            string image = dvd.Image;

            try
            {
                pictureBoxCouvertureDVD.Image = Image.FromFile(image);
            }
            catch
            {
                pictureBoxCouvertureDVD.Image = null;
            }


        }
        /// <summary>
        /// ajoute les étapes de suivi
        /// </summary>
        private void RemplirComboBoxSuiviDVD()
        {

            comboBoxSuiviDVD.Items.Clear();
            comboBoxSuiviDVD.Items.Add("En cours");
            comboBoxSuiviDVD.Items.Add("Relancée");
            comboBoxSuiviDVD.Items.Add("Livrée");
            comboBoxSuiviDVD.Items.Add("Réglée");
            Console.WriteLine("RemplirComboBoxSuivi executed. Items count: " + comboBoxSuivi.Items.Count);

        }
        /// <summary>
        /// change l'étape de suivi dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxSuiviDVD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (numeroSuiviDVD.Text != null && numeroSuiviDVD.Text != "" && canUpdateSuivi == true)
            {
                string numSuivi = numeroSuiviDVD.Text;

                string libelle = comboBoxSuiviDVD.Text;
                controller.ChangeSuivi(numSuivi, libelle);
                Console.WriteLine("The combobox selected suivi is " + libelle);
                canUpdateSuivi = false;

                foreach (CommandeDocument commande in lesCommandesDVD)
                {
                    if (commande.Id == numSuivi)
                    {
                        commande.Suivi.Libelle = libelle;
                        Console.WriteLine("commade document modifiée " + commande.Id);
                        break;
                    }

                }
                bindingSourceCommandesDVD.DataSource = lesCommandesDVD;
                bindingSourceCommandesDVD.ResetBindings(false);

                dataGridViewDVD.DataSource = bindingSourceCommandesDVD;
            } 
        }

        /// <summary>
        /// permet la selection de la commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewDVD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Accéder à la ligne où le clic a eu lieu
                DataGridViewRow row = dataGridViewDVD.Rows[e.RowIndex];


                string idcommande = row.Cells[3].Value.ToString();
                selectedCommandeDocument = idcommande;
                Console.WriteLine("selected id is " + idcommande);
                numeroSuiviDVD.Text = idcommande;
                
                try
                {
                    string libelle = row.Cells[8].Value.ToString();
                    switch (libelle)
                    {
                        case "En cours":
                            comboBoxSuiviDVD.SelectedIndex = 0;
                            break;
                        case "Relancée":
                            comboBoxSuiviDVD.SelectedIndex = 1;
                            break;
                        case "Livrée":
                            comboBoxSuiviDVD.SelectedIndex = 2;
                            break;
                        case "Réglée":
                            comboBoxSuiviDVD.SelectedIndex = 3;
                            break;
                    }
                    Console.WriteLine("Vous avez cliqué sur la commande : " + idcommande);
                    Console.WriteLine("Le suivi est  : " + libelle);
                }
                catch
                {
                    Console.WriteLine("erreur de selected value");
                }

            }
        }
        /// <summary>
        ///autorise le changement de l'étape de suivi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxSuiviDVD_DropDown(object sender, EventArgs e)
        {
            canUpdateSuivi = true;
        }
        /// <summary>
        /// efface les champs d'information lors d'un changement d'id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNumDVD_TextChanged(object sender, EventArgs e)
        {

            textBoxTitreDVD.Text = "";
            textBoxRealDVD.Text = "";
            textBoxDureeDVD.Text = "";
            textBoxGenreDVD.Text = "";
            textBoxPublicDVD.Text = "";
            textBoxRayonDVD.Text = "";
            RemplirCommandesDVDListe(null);
            comboBoxSuiviDVD.SelectedIndex = 0;
            numeroSuiviDVD.Text = null;

        }
        /// <summary>
        /// créée une commande de dvd
        /// </summary>
        /// <returns></returns>
        private CommandeDocument CreateCommandeDVD()
        {
            string numcommande = GetLastNumCommandes();
            string numlivre = textBoxAddNumDVD.Text;
            int nbExemplaire = Convert.ToInt32(textBoxAddNbExDVD.Text);
            double montant = Convert.ToDouble(textBoxAddMontantDVD.Text);
            DateTime date = dateTimePickerAddDVD.Value;
            Suivi suivi = CreateSuivi();
            if (numcommande != "" && numlivre != "" && nbExemplaire != null && date != null && suivi != null)
            {
                CommandeDocument commande = new CommandeDocument(numcommande, date, montant, nbExemplaire, numlivre, suivi.Id, suivi.Libelle);

                return commande;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// ajoute la commandé créée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddCommandeDVD_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxAddNumDVD.Text) &&
                !string.IsNullOrEmpty(textBoxAddMontantDVD.Text) &&
                !string.IsNullOrEmpty(textBoxAddNbExDVD.Text))

            {
                bool dvdExsite = false;
                string numDVD = null;
                foreach (Dvd dvd in lesDvd)
                {
                    if (textBoxAddNumDVD.Text == dvd.Id)
                    {
                        dvdExsite = true;
                        numDVD= dvd.Id;
                        break;
                    }
                }
                if (dvdExsite == true)
                {
                    Console.WriteLine("clik validé xxxxxxxxxxxxxxxxxxxx");
                    CommandeDocument commande = CreateCommandeDVD();

                    controller.AddCommandeDocument(commande);
                    
                    textBoxNumDVD.Text = numDVD;
                    buttonRechercheDVD_Click(buttonRechercheDVD, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Livre non exsitant.");
                }

            }
            else
            {
                MessageBox.Show("Veuillez remplir toutes les informations sur la commande.");

            }
        }
        /// <summary>
        /// supprime une commande dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelCommandeDVD_Click(object sender, EventArgs e)
        {
            foreach (CommandeDocument commande in lesCommandesDVD)
            {
                Console.WriteLine("seleted commande id is " + selectedCommandeDocument + " and this command id is " + commande.Id);
                if (commande.Id == selectedCommandeDocument)
                {
                    Console.WriteLine("Delete commande numùber " + commande.Id);

                    controller.DeleteCommandeDocument(commande);
                    lesCommandesDVD.Remove(commande);
                    bindingSourceCommandesDVD.DataSource = lesCommandesDVD;
                    bindingSourceCommandesDVD.ResetBindings(false);

                    dataGridViewDVD.DataSource = bindingSourceCommandesDVD;
                    numeroSuiviDVD.Text = null;
                    selectedCommandeDocument = null;


                    break;
                }
            }
        }
        /// <summary>
        /// seul les chiffres peuvent êtres utilisé dans ce champs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNumDVD_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }
        /// <summary>
        /// seul les chiffres peuvent êtres utilisé dans ce champs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxAddNumDVD_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }
        /// <summary>
        /// seul les chiffres peuvent êtres utilisé dans ce champs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxAddNbExDVD_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }
        /// <summary>
        /// seul les chiffres peuvent êtres utilisé dans ce champs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxAddMontantDVD_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }
        /// <summary>
        /// permet le tri par colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewDVD_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            
            string titreColonne = dataGridViewDVD.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Id":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesDVD.OrderBy(o => o.Id).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesDVD.OrderByDescending(o => o.Id).ToList();
                        orderLivreASC = true;
                    }
                    break;
                case "Date de commande":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesDVD.OrderBy(o => o.Date).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesDVD.OrderByDescending(o => o.Date).ToList();
                        orderLivreASC = true;
                    }
                    break;
                case "Montant":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesDVD.OrderBy(o => o.Montant).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesDVD.OrderByDescending(o => o.Montant).ToList();
                        orderLivreASC = true;
                    }
                    break;
                case "NbExemplaire":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesDVD.OrderBy(o => o.NbExemplaire).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesDVD.OrderByDescending(o => o.NbExemplaire).ToList();
                        orderLivreASC = true;
                    }
                    break;
              
               
            }

                RemplirCommandesDVDListe(sortedList);


        }

        #endregion

        #region Onglet CommandesRevue

        private readonly BindingSource bindingSourceCommandesRevue = new BindingSource();
        private List<Abonnement> lesCommandesRevues = new List<Abonnement>();

        private void tabCommandesRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();


            lesCommandesRevues = controller.GetAllCommandesRevue();

            DisplayFinAbo();
        }

        
        /// <summary>
        /// recherche et affiche les information sur la revue séléctionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRechercheRevue_Click(object sender, EventArgs e)
        {
            string numdoc = textBoxNumRevue.Text;
            Console.WriteLine("button recherche num dvd " + numdoc);
            if (numdoc != null)
            {
                lesCommandesRevues = controller.GetCommandesRevue(numdoc);
                foreach (Abonnement commande in lesCommandesRevues)
                {
                    Console.WriteLine("ID: " + commande.Id  + ", ID Revue: " + commande.IdRevue  + ", Date: " + commande.Date + ", Montant: " + commande.Montant);
                }

                RemplirCommandesRevueListe(lesCommandesRevues);
            }


            if (!textBoxNumRevue.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(numdoc));
                if (revue != null)
                {
                    AfficheRevueCommandeInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }
        /// <summary>
        /// affiche les commandes correspondant a la revue selectionnée
        /// </summary>
        /// <param name="lesCommandesRevue"></param>
        private void RemplirCommandesRevueListe(List<Abonnement> lesCommandesRevue)
        {
            if (lesCommandesRevue != null)
            {
                bindingSourceCommandesRevue.DataSource = lesCommandesRevue;
                dataGridViewRevue.DataSource = bindingSourceCommandesRevue;
                foreach (Abonnement commande in lesCommandesRevue)
                {
                    Console.WriteLine(" remplircommandeslivres liste id " + commande.Id + "date " + commande.Date + "libelle " + commande.Suivi.Libelle);
                }

                dataGridViewRevue.Columns["id"].Visible = false;
                dataGridViewRevue.Columns["idRevue"].Visible = false;
                dataGridViewRevue.Columns["titre"].Visible = false;
                dataGridViewRevue.Columns["suivi"].Visible = false;
                dataGridViewRevue.Columns["LibelleSuivi"].Visible = false;
                dataGridViewRevue.Columns["Libelle"].Visible = false;
                dataGridViewRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridViewRevue.Columns["date"].DisplayIndex = 0;
                dataGridViewRevue.Columns["montant"].DisplayIndex = 1;
                dataGridViewRevue.Columns[4].HeaderCell.Value = "Date de commande";
                dataGridViewRevue.Columns[0].HeaderCell.Value = "Date de fin d'abonnement";



            }
            else
            {
                bindingSourceCommandesRevue.DataSource = null;
            }
        }
        /// <summary>
        /// remplis les champs d'information sur la revue
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheRevueCommandeInfos(Revue revue)
        {
            textBoxTitreRevue.Text = revue.Titre;
            textBoxPeriodeRevue.Text = revue.Periodicite;
            textBoxDelaisRevue.Text = revue.DelaiMiseADispo.ToString();
            textBoxGenreRevue.Text = revue.Genre;
            textBoxPublicRevue.Text = revue.Public;
            textBoxRayonRevue.Text = revue.Rayon;



        }

        
        /// <summary>
        /// permet la selection d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewRevue_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                // Accéder à la ligne où le clic a eu lieu
                DataGridViewRow row = dataGridViewRevue.Rows[e.RowIndex];


                string idcommande = row.Cells[3].Value.ToString();
                selectedCommandeDocument = idcommande;
                Console.WriteLine("selected id is " + idcommande);
                
                try
                {
                   
                    Console.WriteLine("Vous avez cliqué sur la commande : " + idcommande);
                    
                }
                catch
                {
                    Console.WriteLine("erreur de selected value");
                }

            }
        }
        /// <summary>
        /// efface les champs lors d'un changement de numéro de revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNumRevue_TextChanged(object sender, EventArgs e)
        {

            textBoxTitreRevue.Text = "";
            textBoxPeriodeRevue.Text = "";
            textBoxDelaisRevue.Text = "";
            textBoxGenreRevue.Text = "";
            textBoxPublicRevue.Text = "";
            textBoxRayonRevue.Text = "";
            RemplirCommandesRevueListe(null);
            
        }

        
        /// <summary>
        /// créée un abonnement à une revue
        /// </summary>
        /// <returns></returns>
        private Abonnement CreateCommandeRevue()
        {
            string numcommande = GetLastNumCommandes();
            string numlivre = textBoxNumRevueAdd.Text;
            double montant = Convert.ToDouble(textBoxMontantRevueAdd.Text);
            DateTime date = dateTimePickerCommandeRevue.Value;
            DateTime dateFinAbo = dateTimePickerFinAbo.Value;
            if (numcommande != "" && numlivre != "" && date != null && dateFinAbo != null && montant != null)
            {
                Abonnement commande = new Abonnement(numcommande,date,montant,dateFinAbo,numlivre);

                return commande;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// ajoute la commande créée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddCommandeRevue_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxNumRevueAdd.Text) &&
              !string.IsNullOrEmpty(textBoxMontantRevueAdd.Text) &&
              dateTimePickerCommandeRevue.Value < dateTimePickerFinAbo.Value)

            {
                bool revueExsite = false;
                string numrevue = null;
                foreach (Revue revue in lesRevues)
                {
                    if (textBoxNumRevueAdd.Text == revue.Id)
                    {
                        revueExsite = true;
                        numrevue = revue.Id;
                        break;
                    }
                }
                if (revueExsite == true)
                {
                    Console.WriteLine("clik validé xxxxxxxxxxxxxxxxxxxx");
                    Abonnement commande = CreateCommandeRevue();

                    controller.AddCommandeRevue(commande);
                    
                    textBoxNumRevue.Text = numrevue;
                    buttonRechercheRevue_Click(buttonRechercheRevue, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Revue non exsitante.");
                }

            }
            else
            {
                MessageBox.Show("Veuillez remplir toutes les informations sur la commande.");

            }
        }

        /// <summary>
        /// supprime la commande séléctionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteCommandeRevue_Click(object sender, EventArgs e)
        {

            foreach (Abonnement commande in lesCommandesRevues)
            {
                Console.WriteLine("seleted commande id is " + selectedCommandeDocument + " and this command id is " + commande.Id);
                if (commande.Id == selectedCommandeDocument)
                {
                    bool canDelete = ExemplaireCheck(commande);

                    if (canDelete)
                    {
                        Console.WriteLine("Delete commande numùber " + commande.Id);

                        controller.DeleteCommandeRevue(commande);
                        lesCommandesRevues.Remove(commande);
                        bindingSourceCommandesRevue.DataSource = lesCommandesRevues;
                        bindingSourceCommandesRevue.ResetBindings(false);

                        dataGridViewRevue.DataSource = bindingSourceCommandesRevue;
                        selectedCommandeDocument = null;

                    }
                    else
                    {
                        Console.WriteLine("Impossible de supprimer " + commande.Id);
                        MessageBox.Show("Impossible de supprimer un abonnement avec des exemplaires");
                    }


                    break;
                }
            }
        }
        /// <summary>
        /// vérifie les parution sur une periode d'abonnement
        /// </summary>
        /// <param name="dateCommande"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="dateParution"></param>
        /// <returns></returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return (DateTime.Compare(dateCommande, dateParution) < 0 && DateTime.Compare(dateParution, dateFinAbonnement) < 0);
        }
        /// <summary>
        /// vérifie si l'abonnement est en lien avec des exemplaires dans la bdd
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns></returns>
        private bool ExemplaireCheck(Abonnement abonnement)
        {
            List<Exemplaire> lesExemplairesAbonnement = controller.GetExemplairesRevue(abonnement.IdRevue);
             bool datedeparution = false;
            foreach (Exemplaire exemplaire in lesExemplairesAbonnement.Where(exemplaires => ParutionDansAbonnement(abonnement.Date, abonnement.DateFinAbonnement, exemplaires.DateAchat)))
            {
                datedeparution = true;

            }
            return !datedeparution;
        }

        /// <summary>
        /// vérifie les abonnement qui finissent d'ici 30j
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> abonnementsEnFin()
        {
            List<Abonnement> lesAbos = controller.GetAllCommandesRevue();

            // Filtrer les abonnements dont la date de fin est à moins de 30 jours
            List<Abonnement> abonnementsProchesDeFin = lesAbos.Where(abonnement =>
            abonnement.DateFinAbonnement <= DateTime.Now.AddDays(30) && abonnement.DateFinAbonnement >= DateTime.Now).ToList();
            return abonnementsProchesDeFin;
        }
        /// <summary>
        /// Ouvre une pop up avertissement 
        /// </summary>
        public void DisplayFinAbo()
        {
            List<Abonnement> lesAbosEnFin = abonnementsEnFin();
            List<Abonnement> lesAbosEnFinTri = lesAbosEnFin.OrderBy(a => a.DateFinAbonnement).ToList();
            List<(string Titre, DateTime DateFinAbonnement)> titresEtDates = new List<(string, DateTime)>();

            foreach (Abonnement abonnement in lesAbosEnFinTri)
            {
                foreach (Revue revue in lesRevues)
                {
                    if (revue.Id == abonnement.IdRevue)
                    {
                        titresEtDates.Add((revue.Titre, abonnement.DateFinAbonnement));
                    }
                }
            }

            StringBuilder message = new StringBuilder();
            message.AppendLine("Les abonnements suivants arrivent à échéance :");
            foreach (var (Titre, DateFinAbonnement) in titresEtDates)
            {
                message.AppendLine($"{Titre} - Fin le {DateFinAbonnement:dd/MM/yyyy}");
            }

            // Afficher le message dans une MessageBox
            MessageBox.Show(message.ToString(), "Alerte Abonnements en Fin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Seul des chiffres sont utilisables sur ce champ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNumRevue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }
        /// <summary>
        /// Seul des chiffres sont utilisables sur ce champ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNumRevueAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }
        /// <summary>
        /// Seul des chiffres sont utilisables sur ce champ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMontantRevueAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Autoriser seulement les chiffres, la touche Backspace et la virgule (pour les nombres décimaux)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true; // Rejeter le caractère
            }

            // Pour autoriser un seul point décimal
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true; // Rejeter le caractère si un point décimal est déjà présent
            }
        }
        /// <summary>
        /// Seul des chiffres sont utilisables sur ce champ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewRevue_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dataGridViewRevue.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Id":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesRevues.OrderBy(o => o.Id).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesRevues.OrderByDescending(o => o.Id).ToList();
                        orderLivreASC = true;
                    }
                    break;
                case "Date de commande":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesRevues.OrderBy(o => o.Date).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesRevues.OrderByDescending(o => o.Date).ToList();
                        orderLivreASC = true;
                    }
                    break;
                case "Montant":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesRevues.OrderBy(o => o.Montant).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesRevues.OrderByDescending(o => o.Montant).ToList();
                        orderLivreASC = true;
                    }
                    break;
                case "Date de fin d'abonnement":
                    if (orderLivreASC == true)
                    {
                        sortedList = lesCommandesRevues.OrderBy(o => o.DateFinAbonnement).ToList();
                        orderLivreASC = false;
                    }
                    else
                    {
                        sortedList = lesCommandesRevues.OrderByDescending(o => o.DateFinAbonnement).ToList();
                        orderLivreASC = true;
                    }
                    break;
               

            }

            RemplirCommandesRevueListe(sortedList);
        }

        #endregion

        
       /// <summary>
       /// vérifie les autorisations de l'utilisateur et adapte les onglets disponibles
       /// </summary>
       public void  AuthorizationCheck()
       {
            switch (service)
            {
                case 1:

                    break;
                case 2:
                    foreach(TabPage tab in frmOnglets.TabPages)
                    {
                        Console.WriteLine("tab found name is " + tab.Name);

                    }
                    frmOnglets.TabPages.Remove(tabPage1);
                    frmOnglets.TabPages.Remove(tabCommandeDVD);
                    frmOnglets.TabPages.Remove(tabCommandesRevue);
                    groupBox7.Enabled = false;
                    break; 
                case 3:
                    MessageBox.Show("Vous n'avez aucune autorisation");
                    
                    Environment.Exit(0);
                    break;
            }
       }

    }
}
    




