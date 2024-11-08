using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// Un scriptableObject qui contient les données d'un produits dans la boutique
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary

// les nom donées lorsqu'on créer le fichier ou lorsqu'on le survole dans la barre de menu 
[CreateAssetMenu(fileName = "Objet", menuName = "ObjetBoutique")]

public class SOObjets : ScriptableObject
{

    
    [Header("Les information")]
    [SerializeField] string _nom = "Nom du produits"; // nom du produit
    [SerializeField] Sprite _imgProduits; // image du produit
    [SerializeField] [Range(0,200) ]int _prix; // prix du produits
    [SerializeField][Range(1, 5)] int _niveauRequis = 1; // niveau requis pour achter le produit
    [SerializeField] [TextArea] string _description; // Description du produits


    [Header("Les conditions des produits")]
    [SerializeField] [Tooltip("Cet objet donne-t-il droit au doubleSaut")] bool _peutDoubleSaut; // Donne la joueur la capacité de double sauter
    [SerializeField] [Tooltip("Cet objet donne-t-il droit de augmenter le mana")] bool _peutAugmenterMana ; // Donne la joueur la capacité d'augmenter le nombre total de son mana
    [SerializeField] [Tooltip("Cet objet utilise les joyaux vert ?")] bool _venduAvecJoyauxVert; // Utilise des joyeaux Vert comme argent
    [SerializeField] [Tooltip("Cet objet utilise les joyaux bleu ?")] bool _venduAvecJoyauxBleu ; // Utilise des joyeaux Bleu comme argent
    [SerializeField] [Tooltip("Cet objet améliore-t-il les projectiles?")] bool _amelioreProjectile ; // Améliore les projectiles


    /// <summary>
    /// Les getters et setters des variables
    /// </summary>
    public string nom { get => _nom; set =>_nom = value; }
    public Sprite imgProduit { get => _imgProduits; set =>_imgProduits = value; } 
    public int prix {get => _prix; set => prix = value;}  
    public int niveauRequis {get => _niveauRequis; set => _niveauRequis = Mathf.Clamp(value,0,int.MaxValue);}
    public string description{ get => _description; set => description = value; }
    public bool peutDoubleSaut{get => _peutDoubleSaut; set => _peutDoubleSaut = value; }
    public bool peutAugmenterMana{get => _peutAugmenterMana; set => _peutAugmenterMana = value; }
    public bool venduAvecJoyauxBleu{get => _venduAvecJoyauxBleu; set => _venduAvecJoyauxBleu = value; }
    public bool venduAvecJoyauxVert{get => _venduAvecJoyauxVert; set => _venduAvecJoyauxVert = value; }
    public bool amelioreProjectile{get => _amelioreProjectile; set => _amelioreProjectile = value; }

}
