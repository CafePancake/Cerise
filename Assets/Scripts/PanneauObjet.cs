using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// Une Class qui gère l'affichage des information d'un produits dans un paneau
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary

public class PanneauObjet : MonoBehaviour
{
    [Header("Les Scriptable Objects")]
    [SerializeField] SOObjets _donnesObjet; // Récupère les donnés relier au produit
    [SerializeField] SOPerso _donnePerso; // champ Temporaire lorsque Carl travaille sur la boutique


    [Header("Les conteneur d'information")]
    [SerializeField] TextMeshProUGUI _champNom; // champ dont le nom du produits sera affiché
    [SerializeField] TextMeshProUGUI _champPrix; // champ dont le prix du produits sera affiché
    [SerializeField] TextMeshProUGUI _champDescription;  // champ dont la description du produits sera affiché
    [SerializeField] Image _image; // champ dont l'image du produits sera affiché
    [SerializeField] CanvasGroup _canvasGroup; // permet d'intéragir avec le panneau



    void Start()
    {
        AfficherInforamation();
    }

    /// <summary>
    /// Permet d'afficher l'infomation du produits
    /// </summary>
    void AfficherInforamation()
    {
        _champNom.text = _donnesObjet.nom;
        if(_donnesObjet.venduAvecJoyauxBleu) _champPrix.text = _donnesObjet.prix+" Bleues";
        if(_donnesObjet.venduAvecJoyauxVert) _champPrix.text = _donnesObjet.prix+" Vertes";
        if(_donnesObjet.venduAvecJoyauxBleu&&_donnesObjet.venduAvecJoyauxVert) _champPrix.text= _donnesObjet.prix+" B&V";
        _champDescription.text = _donnesObjet.description;
        _image.sprite = _donnesObjet.imgProduit;
    }

    /// <summary>
    /// Permet de changer les valeurs du personnage
    /// par rapport au infomration du produits lors de l'achat  
    /// </summary>
    public void Acheter()
    {
        Debug.Log("panneauobjet.acheter");
        _donnePerso.AcheterProduit(_donnesObjet);
    }
}
