using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// Un scriptableObject qui contient les données du perso
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary

// les nom donées lorsqu'on créer le fichier ou lorsqu'on le survole dans la barre de menu
[CreateAssetMenu(fileName = "MaPerso", menuName = "Perso")]

public class SOPerso : ScriptableObject

{

    [Header("Les Scriptable Objects: ")]
    [SerializeField] SOEvents _evenements;


    [Header("Les valeurs initiale: ")]
    [SerializeField] [Range (1,5)] int _niveauIn = 1; // Valeur du niveau initial
    [SerializeField] [Range (0,500)] int _nbJoyauxBleuIn = 0; // Valeur du nombre de noyaux Bleu récupérer initialement 
    [SerializeField] [Range (0,500)] int _nbJoyauxVertIn = 0; // Valeur du nombre de noyaux Vert récupérer initialement 
    [SerializeField] [Range (0,200)] int _nbTempsJeuIn = 200; //  #Tp4 Chrisitan Alan nombre de temps au jeu initialement
    [SerializeField] int _nbPointsIn = 0; // Valeur du nombre point ammaser initialement 
    [SerializeField] int _scoreTotalIn = 0; // Valeur du nombre point ammaser initialement
    [SerializeField] float _nbManaIn = 100f; // # tp4 Valeur du nombre de mana initiallement
    [SerializeField] float _nbVieIn = 100f; // # Synthese Valeur du nombre de mana initiallement
    [SerializeField] bool _aDeDoubleSautIn = false; // Valeur initial de la capacité de double sauté
    [SerializeField] bool _aAugmenterManaIn = false; // état si on est capable de double sauté
    [SerializeField] bool _acleIn = false; // état initale si la joeur a la clé
    [SerializeField] bool _aProjectilesAmelioresIn=false;
    [SerializeField] bool[] _aBonbonIn = new bool[] {false, false, false, false, false, false, false, false, false, false};



    [Header("Les valeurs: ")]
    [SerializeField] [Range (1,5)] int _niveau = 1; // niveau que le joueur se retrouve a présent
    [SerializeField] [Range (0,500)] int _nbJoyauxBleu = 0; // nombre de noyaux Bleu récupérer
    [SerializeField] [Range (0,500)] int _nbJoyauxVert = 0; // nombre de noyaux Vert récupérer
    [SerializeField] [Range (0,200)] int _nbTempsJeu = 200; // #Tp4 Chrisitan Aalan nombre de temps de jeu
    [SerializeField] int _nbPoints = 0; // nombre de points récupérer
    [SerializeField] int _scoreTotal = 0; // nombre de points récupérer
    [SerializeField] float _nbMana = 100f; // nombre de Mana restant
    [SerializeField] float _nbVie = 100f; // # Synthese Valeur du nombre de mana initiallement
    [SerializeField] bool _aDeDoubleSaut = false; // état si on est capable de double sauté
    [SerializeField] bool _aAugmenterMana = false; // état si on est capable d'augmenter mana
    [SerializeField] bool _acle = false; // état si la joueur a la clé
    [SerializeField] bool _aProjectilesAmeliores=false;
    [SerializeField] bool _esterminerNiveau = false; // état si le joueur est terminer une niveau sans mourir
    [SerializeField] bool[] _aBonbon = new bool[] {false, false, false, false, false, false, false, false, false, false};


    [Header("Condition ")]
    [SerializeField] bool _estDansGas = false; 
    [SerializeField] bool _estInvisible = false; 
    

    /// <summary>
    /// List des variable publique qu'on peut utilser dans d'autre script 
    /// </summary>
    public int nbJoyauxBleu { get => _nbJoyauxBleu; set => _nbJoyauxBleu = Mathf.Clamp(value,1,int.MaxValue); }
    public int nbJoyauxVert { get => _nbJoyauxVert; set => _nbJoyauxVert = Mathf.Clamp(value,1,int.MaxValue);}
    public int nbTempsJeu { get => _nbTempsJeu; set => _nbTempsJeu = Mathf.Clamp(value,0,200);}
    public int niveau { get => _niveau; set => _niveau = Mathf.Clamp(value,1,int.MaxValue); }
    public int scoreTotal { get => _scoreTotal; set => _scoreTotal = Mathf.Clamp(value,1,int.MaxValue); }
    public int nbPoints { get => _nbPoints; set => _nbPoints = Mathf.Clamp(value,1,int.MaxValue);}
    public float nbMana { get => _nbMana; set => _nbMana = Mathf.Clamp(value,0,int.MaxValue);}
    public float nbManaIn { get => _nbManaIn; set => _nbManaIn = Mathf.Clamp(value,0,int.MaxValue);}
    public float nbVie { get => _nbVie; set => _nbVie = Mathf.Clamp(value,0,int.MaxValue);}
    public bool aDeDoubleSaut { get => _aDeDoubleSaut; set => _aDeDoubleSaut = value;}
    public bool aAugmenterMana  { get => _aAugmenterMana; set => _aAugmenterMana = value;}
    public bool acle  { get => _acle; set => _acle = value;}
    public bool aProjectilesAmeliores { get => _aProjectilesAmeliores; set => _aProjectilesAmeliores = value;}
    public bool estDansGas { get => _estDansGas; set => _estDansGas = value; }
    public bool estInvisible { get => _estInvisible; set => _estInvisible = value; }
    public bool[] aBonbon { get => _aBonbon; set => _aBonbon = value;}
    public bool esterminerNiveau { get => _esterminerNiveau; set => _esterminerNiveau = value; }
    


    // Gère le changement d'infomration relier au personage 
    UnityEvent _evenenemntMiseAJour = new UnityEvent();
    public UnityEvent evenementMiseAjour => _evenenemntMiseAJour;

    /// <summary>
    ///  Changer les valeurs des varible publique a leurs valeurs initialles
    /// </summary>
    public void Renitialiser()
    {
        _nbMana = _nbManaIn;
        _nbVie = _nbVieIn;
        _niveau = _niveauIn;
        _nbJoyauxBleu = _nbJoyauxBleuIn;
        _nbJoyauxVert = _nbJoyauxVertIn;
        _nbTempsJeu = _nbTempsJeuIn;
        _nbPoints = _nbPointsIn;
        _scoreTotal = _scoreTotalIn;
        _acle = _acleIn;
        _aDeDoubleSaut = _aDeDoubleSautIn;
        _aAugmenterMana = _aAugmenterManaIn;
        _aProjectilesAmeliores = _aProjectilesAmelioresIn;
        _estDansGas = false;
        _estInvisible = false;
        _esterminerNiveau = false;
        _aBonbon = _aBonbonIn;
    }

    /// <summary>
    /// #Tp4 Chrisitan Allan
    /// Désactive les améliorations achetées et restaure les valeurs initiales
    /// du pointage, du mana, des joyaux.
    /// </summary>
    public void RenitialiserAffichage()
    {
       _aDeDoubleSaut = _aDeDoubleSautIn;
       _aAugmenterMana = _aAugmenterManaIn;
       _aProjectilesAmeliores = _aProjectilesAmelioresIn;
       _nbTempsJeu = _nbTempsJeuIn;
       _nbMana = _nbManaIn;
    }

    /// <summary>
    /// Permet de modifer les variables relier au person lorsqu'on achète une produits
    /// </summary>
    /// <param name="_donneObjets"> ScriptebleObjets du produit acheter </param>
    public void AcheterProduit(SOObjets _donneObjets)
    {

        if(_donneObjets.venduAvecJoyauxBleu &&_donneObjets.venduAvecJoyauxVert) //#tp3 carl
        {
            Debug.Log("conditionfonctionnedoubleprix)");
            if(_nbJoyauxBleu>=_donneObjets.prix&&_nbJoyauxVert>=_donneObjets.prix)
            {
                if(_donneObjets.amelioreProjectile==true) aProjectilesAmeliores = true;
                _nbJoyauxBleu-=_donneObjets.prix;
                _nbJoyauxVert-=_donneObjets.prix;
                Debug.Log("Projectiles ameliores? : "+_donneObjets.amelioreProjectile);
            }
        }
        //#tp3 Christitan 

        if (_donneObjets.venduAvecJoyauxVert) //#tp3 Christitan 
        {
            if(_nbJoyauxVert>=_donneObjets.prix)
            {
                if(_donneObjets.peutDoubleSaut == true )
                {
                    _nbJoyauxVert-=_donneObjets.prix;
                    aDeDoubleSaut = true;
                    Debug.Log("Double saut" + aDeDoubleSaut );
                } 
            }

        }
        if( _donneObjets.venduAvecJoyauxBleu) 
        {
            if(_nbJoyauxBleu>=_donneObjets.prix)
            {
                if(_donneObjets.peutAugmenterMana == true)
                {
                    _nbJoyauxBleu-=_donneObjets.prix;
                    nbMana = _nbManaIn * 2;
                    aAugmenterMana = true;
                    Debug.Log("Augmenter mana" + aAugmenterMana );
                } 
            }
            
        }

        _evenements.actualiserInfosBoutique.Invoke();
    }

       public void IncrementerJoyaux(Joyaux Joyau)
    {
        if(Joyau.typeJoyau==Joyaux.TypeJoyau.bleue)
        {
            _nbJoyauxBleu++;
        }
        else if(Joyau.typeJoyau==Joyaux.TypeJoyau.verte)
        {
            _nbJoyauxVert++;
        }
 
        evenementMiseAjour.Invoke();
    }
    public void AugmenterPoints(int points)
    {
        _nbPoints+=points;
        evenementMiseAjour.Invoke();
    }

    /// <summary>
    /// Véridier si la jouer a la clé
    /// </summary>
    public void VerifierCle(bool etatCLe)
    {
        if (etatCLe)
        {
            acle = true;
            evenementMiseAjour.Invoke();

        }
        else
        {
            acle = false;
            evenementMiseAjour.Invoke();
        }
        Debug.Log("cle pris");
    }

    void OnValidate() //#tp3 Carl
    {
        _evenements.actualiserInfosBoutique.Invoke();
    }
 
    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    void OnDestroy()
    {
        // Debug.Log("quittereinitialise");
        Renitialiser();
    }


}
