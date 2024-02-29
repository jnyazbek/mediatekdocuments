using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using MediaTekDocuments.Utils;


namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// // méthode HTTP pour Update
        private const string PUT = "PUT";
        /// <summary>
        //méthode HTTP pour DROP
        /// </summary>
        private const string DELETE = "DELETE";
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre");
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon");
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public");
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre");
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd");
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue");
            return lesRevues;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            
            string jsonIdDocument = idDocument;
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET,uriApi+ "exemplaire/" + jsonIdDocument);
            return lesExemplaires;
        }

        /// <summary>
        /// Retourne les commandes des documents
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets CommandeDocument</returns>

        public List<CommandeDocument> GetCommandesDocument(string idDocument)
        {
            Console.WriteLine("the id document passed in Access.getcommandedocument is "+ idDocument);
            String jsonIdDocument = convertToJson("id", idDocument);
            List<CommandeDocument> lesCommandesDocument = TraitementRecup<CommandeDocument>(GET, uriApi+"commandedocument/livre/"+idDocument);
            Console.WriteLine("GetCommandesDocument utilisé, the request is "+ uriApi + "commandedocument/livre/" + idDocument);
            return lesCommandesDocument;
        }
        /// <summary>
        /// Retourne les commandes de DVD
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandesDVD(string idDocument)
        {
            Console.WriteLine("the id document passed in Access.getcommandedocument is " + idDocument);
            String jsonIdDocument = convertToJson("id", idDocument);
            List<CommandeDocument> lesCommandesDocument = TraitementRecup<CommandeDocument>(GET, uriApi + "commandedocument/dvd/" + idDocument);
            Console.WriteLine("GetCommandesDocument utilisé, the request is " + uriApi + "commandedocument/dvd/" + idDocument);
            return lesCommandesDocument;
        }
        /// <summary>
        /// Retourne les commandes de revues
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<Abonnement> GetCommandesRevue(string idDocument)
        {
            Console.WriteLine("the id document passed in Access.getcommandedocument is " + idDocument);
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Abonnement> lesCommandesDocument = TraitementRecup<Abonnement>(GET, uriApi + "abonnement/revue/" + idDocument);
            Console.WriteLine("GetCommandesRevue utilisé, the request is " + uriApi + "abonnement/revue/" + idDocument);
            return lesCommandesDocument;
        }
        /// <summary>
        /// Retourne tout les suivis
        /// </summary>
        /// <returns></returns>
        public List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi");
            return lesSuivis;
        }
        /// <summary>
        /// Retourne toutes les commandes
        /// </summary>
        /// <returns></returns>
        public List<Commande> GetAllCommandes()
        {
            List<Commande> commandes = TraitementRecup<Commande>(GET, "commande");
            Console.WriteLine("GetAllCommandes utilisé"); 
            return commandes;
        }
       
        /// <summary>
        /// Retourne toutes les commandes de document
        /// </summary>
        /// <returns></returns>
        public List<CommandeDocument> GetAllCommandesDocument()
        {
            List<CommandeDocument> commandes = TraitementRecup<CommandeDocument>(GET, "commandedocument");
            Console.WriteLine("GetAllCommandesDocument utilisé");
            return commandes;
        }
        /// <summary>
        /// retourne toutes les commandes de revues
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAllCommandesRevue()
        {
            List<Abonnement> commandesRevues = TraitementRecup<Abonnement>(GET, "abonnement");
            Console.WriteLine("GetAllCommandesRevues utilisé");
            return commandesRevues;
        }
        /// <summary>
        /// ajoute un nouveau suivi
        /// </summary>
        /// <param name="suivi"></param>
        /// <returns></returns>
        public bool AddSuivi(Suivi suivi)
        {
            try
            {
                var suiviData = new
                {
                    id = suivi.Id,
                    libelle = suivi.Libelle

                };
                string jsonCreerSuivi = JsonConvert.SerializeObject(suiviData);
                List<Suivi> liste = TraitementRecup<Suivi>(POST, uriApi+"suivi" , jsonCreerSuivi);

                return (liste != null && liste.Count > 0);
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Erreur lors de l'ajout du suivi: " + ex.Message);
                return false;
            }

            

        }
        /// <summary>
        /// ajoute une nouvelle commande document
        /// </summary>
        /// <param name="commandeDocument"></param>
        /// <returns></returns>
        public bool AddCommandeDocument(CommandeDocument commandeDocument) 
        {
           
          
            CommandeDocument commande = commandeDocument;
            Suivi suivi = commandeDocument.Suivi;
            Console.WriteLine("addcommande document verification " + suivi.Libelle);

            try
            {
                var commandeData1 = new
                {
                    id = commande.Id,
                    dateCommande = commande.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    montant = commandeDocument.Montant

                };
                // Création de l'objet à envoyer
                var commandeData = new
                {
                    id = commande.Id,
                    nbExemplaire = commande.NbExemplaire,
                    idLivreDvd = commande.IdLivreDvd,
                    date = commande.Date.ToString("yyyy-MM-dd HH:mm:ss"), // Assurez-vous du format de date attendu par l'API
                    montant = commande.Montant,
                    idsuivi = commande.Suivi.Id
                };

                var suiviData = new
                {
                    id = suivi.Id,
                    libelle = suivi.Libelle
                    
                };
                
                string jsonCreerCommande = JsonConvert.SerializeObject(commandeData1);
                // Sérialisation de l'objet en JSON
                string jsonCreerCommandeDocument = JsonConvert.SerializeObject(commandeData);
                Console.WriteLine("jsonCreerCommandeDocument: " + jsonCreerCommandeDocument);

                string jsonCreerSuivi = JsonConvert.SerializeObject(suiviData);
                Console.WriteLine("jsonCreerSuivi: " + jsonCreerSuivi);

                // Envoi de la requête POST
                List<CommandeDocument> listeSuivi = TraitementRecup<CommandeDocument>(POST, uriApi + "suivi" , jsonCreerSuivi);
                List<CommandeDocument> listeCommande = TraitementRecup<CommandeDocument>(POST, uriApi + "commande" , jsonCreerCommande);
                List<CommandeDocument> listeCommandeDocument = TraitementRecup<CommandeDocument>(POST, uriApi+"commandedocument" , jsonCreerCommandeDocument);

                // Vérifiez ici le succès de l'opération (peut-être en vérifiant la taille de la liste ou d'autres indicateurs)
                return (listeCommandeDocument != null && listeCommandeDocument
                    .Count > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout d'une commande document: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// ajoute une nouvelle commande de revue
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public bool AddCommandeRevue(Abonnement commande)
        {

            try
            {
                var aboData = new
                {
                    id = commande.Id,
                    dateFinAbonnement = commande.DateFinAbonnement.ToString("yyyy-MM-dd HH:mm:ss"),
                    idRevue = commande.IdRevue

                };
                // Création de l'objet à envoyer
                var commandeData = new
                {
                    id = commande.Id,
                    dateCommande = commande.Date.ToString("yyyy-MM-dd HH:mm:ss"), // Assurez-vous du format de date attendu par l'API
                    montant = commande.Montant,
                   
                };

               

                string jsonCreerAbo = JsonConvert.SerializeObject(aboData);
                // Sérialisation de l'objet en JSON
                string jsonCreerCommande = JsonConvert.SerializeObject(commandeData);
                Console.WriteLine("jsonCreerCommandeDocument: " + jsonCreerAbo);

               

                // Envoi de la requête POST
                List<Commande> listeCommande = TraitementRecup<Commande>(POST, uriApi + "commande", jsonCreerCommande);
                List<Abonnement> listeCommandesRevue = TraitementRecup<Abonnement>(POST, uriApi + "abonnement", jsonCreerAbo);

                // Vérifiez ici le succès de l'opération (peut-être en vérifiant la taille de la liste ou d'autres indicateurs)
                return (listeCommandesRevue != null && listeCommande
                    .Count > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout d'une commande document: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// met a jour l'étape de suivi
        /// </summary>
        /// <param name="numSuivi"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public bool ChangeSuivi(string numSuivi, string step)
        {
            try
            {
                var SuiviData = new
                {

                    id = numSuivi,
                    libelle = step
                };
                string jsonUpdateSuivi = JsonConvert.SerializeObject(SuiviData);
                List<Suivi> suivis = TraitementRecup<Suivi>(PUT, uriApi + "suivi", jsonUpdateSuivi);

                return (suivis != null && suivis
                       .Count > 0);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Erreur lors de la modification du suivi: " + ex.Message);
                return false;
            }
           
        }
        /// <summary>
        /// supprime une commande de document
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public bool DeleteCommandeDocument(CommandeDocument commande)
        {
            try
            {
                var commandeData1 = new
                {
                    id = commande.Id,
                    

                };
                // Création de l'objet à envoyer
                var commandeData = new
                {
                    id = commande.Id,
                   
                };

                var suiviData = new
                {
                    id = commande.Suivi.Id,
                    libelle = commande.Suivi.Libelle

                };
                string jsonDeleteCommande = JsonConvert.SerializeObject(commandeData1);
                // Sérialisation de l'objet en JSON
                string jsonDeleteCommandeDocument = JsonConvert.SerializeObject(commandeData);


                string jsonDeleteSuivi = JsonConvert.SerializeObject(suiviData);


                
                Console.WriteLine("Requette : " + uriApi + "commandedocument/" + commande.Id);
                List<CommandeDocument> listeCommandeDocument = TraitementRecup<CommandeDocument>(DELETE, uriApi + "commandedocument/" + commande.Id);
                
                List<CommandeDocument> listeSuivi = TraitementRecup<CommandeDocument>(DELETE, uriApi + "suivi/"+commande.Suivi.Id);
                List<CommandeDocument> listeCommande = TraitementRecup<CommandeDocument>(DELETE, uriApi + "commande/"+ commande.Id);

                if (listeCommandeDocument != null && listeCommandeDocument.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout d'une commande document: " + ex.Message);
                return false;
            }
            
        }
        /// <summary>
        /// supprime une commande de revue
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public bool DeleteCommandeRevue(Abonnement commande)
        {
            try
            {
                var aboData = new
                {
                    id = commande.Id,


                };
                // Création de l'objet à envoyer
                var commandeData = new
                {

                    id = commande.Id,

                };

                
                string jsonDeleteCommande = JsonConvert.SerializeObject(commandeData);
                // Sérialisation de l'objet en JSON
                string jsonDeleteCommandeDocument = JsonConvert.SerializeObject(aboData);


                Console.WriteLine("Requette : " + uriApi + "commande" +
                    "/" + commande.Id);
                List<Abonnement> listeAbonnement = TraitementRecup<Abonnement>(DELETE, uriApi + "abonnement/" + commande.Id);

                List<Commande> listeCommande = TraitementRecup<Commande>(DELETE, uriApi + "commande/" + commande.Id);

                if (listeAbonnement != null && listeAbonnement.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout d'une commande document: " + ex.Message);
                return false;
            }

        }

        public List<Utilisateur> GetUtilisateur(string id)
        {
            Console.WriteLine("the id user passed in Access.getutilisateur is " + id);
            //String jsonIdDocument = convertToJson("id", id);
            List<Utilisateur> lesUtilisateurs = TraitementRecup<Utilisateur>(GET, uriApi + "utilisateur/" + id);
            Console.WriteLine("GetCommandesRevue utilisé, the request is " + uriApi + "utilisateur/" + id);
            return lesUtilisateurs;
        }

        public List<Utilisateur> GetAllUtilisateurs()
        {
            List<Utilisateur> lesUtilisateurs = TraitementRecup<Utilisateur>(GET, "utilisateur");
            return lesUtilisateurs;
        }

          



        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message)
        {
            List<T> liste = new List<T>();
            try
            {
                
                  

                JObject retour = api.RecupDistant(methode, message);
                Console.WriteLine("Réponse brute de l'API :incoming");
                Console.WriteLine("Réponse brute de l'API : " + retour);

                // extraction du code retourné
                String code = (String)retour["code"];
                Console.WriteLine("le code reçu est " + code);
                
                if (code.Equals("200"))
                {
                    DateTest dateTest = new DateTest();
                 //parametre de deserialisation
                    var settings = new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter> { new CustomDateTimeConverter(), new CustomBooleanJsonConverter() }
                    };
                    //recuperation de la string
                    String resultString = JsonConvert.SerializeObject(retour["result"]);
                    String resultString2 = JsonConvert.SerializeObject(dateTest.jsonTest);
                    // Deserialize the JSON string into a list of objects using the defined settings
                    liste = JsonConvert.DeserializeObject<List<T>>(resultString, settings);
                    Console.WriteLine(resultString2 + "and resultstring is "+ resultString);
                }
            
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// overload
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message, string jsonBody)
        {
            List<T> liste = new List<T>();
            try
            {

                Console.WriteLine("OVERLOAD AVEC JSONBODY DE TRAITEMENT REUCP and jsonbody == " + jsonBody); 
                JObject retour = api.RecupDistant(methode, message, jsonBody);

                Console.WriteLine("Réponse brute de l'API : " + retour);

                // extraction du code retourné
                String code = (String)retour["code"];
                Console.WriteLine("le code reçu est " + code);
                if (code.Equals("200"))
                {
                    DateTest dateTest = new DateTest();
                    
                    var settings = new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter> { new CustomDateTimeConverter(), new CustomBooleanJsonConverter() }
                    };
                    
                    String resultString = JsonConvert.SerializeObject(retour["result"]);
                    String resultString2 = JsonConvert.SerializeObject(dateTest.jsonTest);
                    
                    liste = JsonConvert.DeserializeObject<List<T>>(resultString, settings);
                    Console.WriteLine(resultString2 + "and resultstring is " + resultString);
                }

                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + e.Message);
                Environment.Exit(0);
            }
            return liste;
        }


        /// <summary>+
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss"; 
            }
        }


        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

        

    }
}