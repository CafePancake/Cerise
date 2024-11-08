using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// classe qui affiche les donnés relier au perso du le niveau
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary

public class AfficherInformation : MonoBehaviour
{
    [Header("Les Scriptable Objects")]
    [SerializeField] SOPerso _donnePerso; // Récupère les donnés relier au personnage
    [SerializeField] SOEvents _evenement; // Avoir accès au liste d'événement


    [Header("Les champs de textes")]
    [SerializeField] TextMeshProUGUI _champNiveau; // Texte pour le niveau à présent
    [SerializeField] TextMeshProUGUI _champJoyauxVert; // Texte pour le nombre de joyaux Vert
    [SerializeField] TextMeshProUGUI _champJoyauxBleu; // Texte pour le nombre de joyaux Bleu
    [SerializeField] TextMeshProUGUI _champPointage; // Texte pour le nombre de points
    [SerializeField] TextMeshProUGUI _champTemp; // Texte pour le nombre de points
    [SerializeField] TextMeshProUGUI _txtNbMana; // Texte pour le nombre de mana du personnage


    [Header("Les images")]
    [SerializeField] Image _imageMana; // Image de la potion augmenter Mana
    [SerializeField] Image _imageDblSaut; // Image de la potion double saut
    [SerializeField] Image _imageProjectile; // Image de la potion augmentation des projectile
    [SerializeField] Image _imagecle; // Image de la cle
    

    [Header("Les Sliders")]
    [SerializeField] Slider _barreManaSlider; // Slider du barre de mana
    [SerializeField] Slider _barreVieSlider; // Slider du barre de vie
    [SerializeField] Slider _barreDommageSlider; // Slider du barre de dommage
    [SerializeField] Slider _barreDepenserSlider; // Slider du barre de dommage



    float _manaEncours; // Mana restant durant le jeu
    float _vieEncours; // Vie restant durant le jeu
    float _dommage; // Dommage infliger au joueur

    bool _estTerminer = false; // #Tp4 Chrisitan Allan Vérifier si le temps est terminé 




    /// <summary>
    /// Afficher les données initiales du personnage
    /// </summary>
    void Start()
    {
        _champNiveau.text = _donnePerso.niveau + "";
        _champJoyauxVert.text = _donnePerso.nbJoyauxBleu + "";
        _champJoyauxBleu.text = _donnePerso.nbJoyauxVert + "";
        _champPointage.text = _donnePerso.nbPoints.ToString("D6");
        _champTemp.text = _donnePerso.nbTempsJeu + "";
        _manaEncours = _donnePerso.nbMana;
        _vieEncours = _donnePerso.nbVie;
        GenererPotionImage();
        CommencerTimer();

        // Asurer que la valeur du mana en cours reste entre 0 et la valeur maximal de mana
        _manaEncours = Mathf.Clamp(_manaEncours, 0, _donnePerso.nbMana);

        _txtNbMana.text = _manaEncours + " ";

        _estTerminer = false;

        // appeler la fonction MiseAJour si l'évenement evenementMiseAjour est invoker
        _donnePerso.evenementMiseAjour.AddListener(RenitialiserDonnes);

        // appeler la fonction MiseAJourMana si l'évenement depenserMana est invoker
        _evenement.depenserMana.AddListener(ChangerNbMana);

        // #Tp4: Christian Allan Reolo Arreter le temps du jeu
        _evenement.arreterTemps.AddListener(ArreterTemps);
        
        // #Synthese: Christian Allan Reolo Afficher la vie restant
        _evenement.perdreVie.AddListener(ChangerNbVie);
    }

    /// <summary>
    /// Afficher les les images des potions 
    /// </summary>
    private void GenererPotionImage()
    {
        // #Synthese : Christian Allan Reolo Amélioration de l'affichage des potions
        // si le joueur a acheté la potion double saut
        if (_donnePerso.aDeDoubleSaut == true)
        {
            _imageDblSaut.color = new Color(1, 1, 1, 1);
        }

        // si le joueur a acheté la potion augmenter mana
        if (_donnePerso.aAugmenterMana == true)
        {
            _imageMana.color = new Color(1, 1, 1, 1);
        } 

        // si le joueur a acheté la potion améliorer projectil
        if (_donnePerso.aProjectilesAmeliores == true)
        {
            _imageProjectile.color = new Color(1, 1, 1, 1);
        }

    }



    /// <summary>
    /// Afficher les nouvelle donnés du perso
    /// </summary>
    void RenitialiserDonnes()
    {
        _champNiveau. text = _donnePerso.niveau + "";
        _champJoyauxVert. text = _donnePerso.nbJoyauxVert + "";
        _champJoyauxBleu. text = _donnePerso.nbJoyauxBleu + "";
        _champPointage. text = _donnePerso.nbPoints.ToString("D6");
        _champTemp. text = _donnePerso.nbTempsJeu + "";
        _txtNbMana.text = _manaEncours + "";

        // #tp4 : Christian Allan Reolo Amélioration de l'affichage de la clé
        // si la jouer a dans sa posession une clé
        if(_donnePerso.acle)
        {
            _imagecle.color = new Color(1, 1, 1, 1);
        }

    }

    /// <summary>
    /// # Synthese: Christian Allan Reolo
    /// Afficher la nombre de mana restant
    /// </summary>
    void ChangerNbMana()
    {
        // Si la valeur du silder du mana est 0, arête a lire les code suivant
        if(_barreManaSlider.value == 0f)
        {
            return;
        }
        // Si la valeur initial du mana restant est plus grande que la fraction du mana 
        else 
        {
            StartCoroutine(EnleverMana(5f, 1f, _barreManaSlider, _barreDepenserSlider));
            
        }
    }
    
    /// <summary>
    /// #Synthese: Christian Allan Reolo
    /// Afficher la nombre de vie restant
    /// </summary>
    void ChangerNbVie()
    {
        if( _donnePerso.estDansGas == true)
        {
            _dommage = 0.2f;
        }
        else
        {
            _dommage = 1f;
        }

        if(_donnePerso.nbVie == 0f)
        {
            Niveau.instance.donnePerso.esterminerNiveau = false;
            _evenement.apparaitreTableauBonuse.Invoke();
            return;
        }
        else
        {
            StartCoroutine(EnleverVie(_dommage, 1f, _barreVieSlider, _barreDommageSlider ));
        }
    }
    /// <summary>
    ///  #Synthese: Christian Allan Reolo
    ///  Enlever le nomvre total de vie du personnage
    ///  Diminuer la valeur du slider de la barre de vie
    /// </summary>
    /// <param name="dommage"> Nombre de dommage que le joueur recoit</param>
    /// <param name="duree"> nombre de seconde pour la transtion vers la vouvelle valeur du slider</param>
    /// <param name="barrePrincepal"> Slider de la vie du joueur/</param>
    /// <param name="barreSecondaire"> Slider du dommage que le joeur a recu</param>
    /// <returns></returns>
    private IEnumerator EnleverVie( float dommage, float duree,Slider barrePrincepal, Slider barreSecondaire)
    {
        _vieEncours -= dommage;
        float fillDebut = barreSecondaire.value;
        float fillDomage = barreSecondaire.value;
        float mFraction = _vieEncours/_donnePerso.nbVie;
        float tempsEcouler = 0f;

        while(fillDomage > mFraction )
        {
            tempsEcouler += Time.deltaTime;
            float fraction = tempsEcouler / duree;
            barrePrincepal.value = mFraction;
            barreSecondaire.value = Mathf.Lerp(fillDebut, mFraction, fraction);
            fillDomage = barreSecondaire.value;
            yield return null;
        }

    }

    /// <summary>
    ///  #Synthese: Christian Allan Reolo
    ///  Enlever le nomvre total de vmanadu personnage
    ///  Diminuer la valeur du slider de la barre de mana
    /// </summary>
    /// <param name="dommage"> Nombre de dommage que le joueur recoit</param>
    /// <param name="duree"> nombre de seconde pour la transtion vers la vouvelle valeur du slider</param>
    /// <param name="barrePrincepal"> Slider du mana du joueur/</param>
    /// <param name="barreSecondaire"> Slider du mana depenser que le joeur a utiliser</param>
    /// <returns></returns>
    private IEnumerator EnleverMana( float dommage,float duree,Slider barrePrincipal, Slider barreSecondaire)
    {
        _manaEncours -= dommage;
        float FillDebut = barreSecondaire.value;
        float fillDepenser = barreSecondaire.value;
        float mFraction = _manaEncours/_donnePerso.nbMana;
        float tempsEcouler = 0f;
        while( fillDepenser > mFraction )
        {
            tempsEcouler += Time.deltaTime;
            float fraction = tempsEcouler / duree;
            barrePrincipal.value = mFraction;
            barreSecondaire.value = Mathf.Lerp(FillDebut, mFraction, fraction);
            fillDepenser = barreSecondaire.value;
            yield return null;
        }

    }

    /// <summary>
    /// #Tp4: Christian Allan Reolo
    /// Commencer le timer du jeu
    /// </summary>
    void CommencerTimer()
    {
        StartCoroutine(CompterTemps());
    }

    /// <summary>
    /// #Tp4: Christian Allan Reolo
    /// Compter le temps restant du jeu
    /// </summary>
    /// <returns> Attende une seconde pour diminuer le temps </returns>
    private IEnumerator CompterTemps()
    {

        while(_donnePerso.nbTempsJeu > 0 && !_estTerminer)
        {
            yield return new WaitForSeconds(1);
            _donnePerso.nbTempsJeu -= 1;

            if(_donnePerso.nbTempsJeu == 20)
            {
                _evenement.activerEventLoup.Invoke();
            }

            RenitialiserDonnes();
        }

        if(_donnePerso.nbTempsJeu == 0)
        {
           
            _evenement.bougerLoup.Invoke();
        }
    }

    /// <summary>
    /// #Tp4: Christian Allan Reolo
    /// Arrêter le temps du jeu
    /// </summary>
    void ArreterTemps()
    {
        _estTerminer = true;
        _evenement.desactiverMarchePerso.Invoke();
        Debug.Log("Temps écoulé");
    }


    /// <summary>
    /// Détruire les listeners des événement avant de changer de scene
    /// </summary>
    void OnDestroy()
    {
        _donnePerso.evenementMiseAjour.RemoveAllListeners();
        _evenement.depenserMana.RemoveAllListeners();
        _evenement.perdreVie.RemoveAllListeners();
    }

}
