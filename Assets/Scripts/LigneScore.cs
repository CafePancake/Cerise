using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//#tp4 Carl
/// <summary>
/// classe qui s'occupe des lignes du tableau individuelles, recevoir l'info, affichage, envoi pour sauvegarde
/// </summary>
public class LigneScore : MonoBehaviour
{
    [SerializeField] TMP_InputField _txtNom; //Input field pour le nom du joueur
    [SerializeField] TextMeshProUGUI _txtScore; //zone de texte du score
    [SerializeField] TextMeshProUGUI _txtPlaceHolder; //zone de texte du placeholder
    [SerializeField] SOSauvegarde _donneeSauvegarde; //SO qui contient la liste des scores et de la logique relie a la liste
    [SerializeField] NomScore _contenuLigne; //variable qui contient le nom et le score relie a la ligne
    [SerializeField] Button _btnEnregistrer; //bouton pour enregistrer de la ligne (chaque ligne a un bouton, innvis sauf si elle du joueur)
    [SerializeField] Image _fondLigne; //image de fond de la ligne


    void Start()
    {
        _fondLigne = GetComponent<Image>();
    }

    /// <summary>
    /// Lit la variable NomScore recue et affiche les valeurs
    /// </summary>
    /// <param name="donnee">Variable du score relie a la ligne recue de TableauScore</param>
    public void LireScore(NomScore donnee)
    {
        _contenuLigne = donnee;
        _txtNom.text = _contenuLigne.nom;
        _txtScore.text = "Score: " + _contenuLigne.score;
    }

    /// <summary>
    /// fonction qui met la ligne en evidence et permet modification du nom quand correspond au score du joueur
    /// </summary>
    public void SelectionnerLigne()
    {
        _txtNom.interactable = true;
        _fondLigne.enabled = true;
        _btnEnregistrer.interactable = true;
        _btnEnregistrer.image.enabled = true;
        _btnEnregistrer.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
    }

    /// <summary>
    /// fonction qui change le placeholder du texte pour le nom
    /// </summary>
    /// <param name="texte">string recu que l<on veut afficher en placeholder</param>
    public void ChangerPlaceholder(string texte)
    {
        _txtPlaceHolder.text = texte;
    }

    /// <summary>
    /// fonction qui change le nom dans la liste des scores pour celui inscrit avant de sauvegarder la liste
    /// </summary>
    /// <param name="pos">position de la ligne sur le tableau</param>
    public void Sauvegarder(int pos)
    {
        if (_txtNom.text == "") _txtNom.text = "Invité(e)";  /// si le nom est laisse vide, cela pourrait causer des problemes car le code se sert en partie du nom vide pour trouver la ligne du joueur, donc un nom vide est converti en invite(e) avant de sauvegarder
        _donneeSauvegarde.EcrireNom(pos, _txtNom.text);
        // _donneeSauvegarde.EcrireFichier();
    }
}
