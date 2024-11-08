using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// #tp4 Chrisitan Allan Reolo
/// Un script qui permet d'afficher le tableau de bonuse
/// Permet de compter le score et de afficher les éléments en étape
/// </summary>

public class TableauBonuse : MonoBehaviour
{

    [Header("Les Scriptable Objects: ")]
    [SerializeField] SOEvents _evenement; // #Tp4 Chrisitan Allan Reolo Avoir accès au liste d'événement
    [SerializeField] SOPerso _donnePerso; // #Tp4 Chrisitan Allan Reolo Récupère les donnés relier au personnage

    [Header("Les champs de textes: ")]
    [SerializeField] TextMeshProUGUI _champNbTemps; // #tp4 Chrisitan Allan Reolo Champ pour afficher le nombre de temps
    [SerializeField] TextMeshProUGUI _champTempsScore; // #tp4 Chrisitan Allan Reolo Champ pour afficher le score du temps
    [SerializeField] TextMeshProUGUI _champNbPointage; // #tp4 Chrisitan Allan Reolo Champ pour afficher le score du pointage
    [SerializeField] TextMeshProUGUI _champScoreTotal; // #tp4 Chrisitan Allan Reolo Champ pour afficher le score Total

    [Header("Bouton: ")]
    [SerializeField] Button _btnContinuer; // #Tp4 Chrisitan Allan Reolo Bouton pour continuer

    [Header("Canvas: ")]
    [SerializeField] Canvas _canvas; // #Tp4 Chrisitan Allan Reolo Récupère le canvas

    // #Tp4 Chrisitan Allan Reolo Variable pour compter le temps
    int _nbTemps = 0;
    
    void Start()
    {
        _btnContinuer.enabled = false; // #Tp4 Chrisitan Allan Reolo Désactive le bouton continuer
        _evenement.apparaitreTableauBonuse.AddListener(MiseAjour); // #Tp4 Chrisitan Allan Reolo Ajoute un écouteur pour l'événement

        
    }

    /// <summary>
    /// #tp4 Chrisitan Allan Reolo
    /// Fonction qui permet de mettre à jour les données
    /// </summary>
    void MiseAjour()
    {
        _canvas.enabled = true; 
        Invoke("CompeterScore", 0.5f); // #Tp4 Chrisitan Allan Reolo Commence le timer pour afficher les données
    }


    /// <summary>
    /// #pt4 Chrisitan Allan Reolo
    /// Fonction qui permet de débuter le compteur du score 
    /// </summary>
    void CompeterScore()
    {
        StartCoroutine(CompterTemps());
    }

    /// <summary>
    /// #tp4 Chrisitan Allan Reolo
    /// Coroutine qui permet de compter le pointage du temps
    /// et de l'afficher en étape
    /// </summary>
    /// <returns></returns>
    private IEnumerator  CompterTemps()
    {
        while(_champNbTemps.color.a < 1)
        {
            _champNbTemps.color = new Color(1, 1, 1, _champNbTemps.color.a + 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        while(_nbTemps != _donnePerso.nbTempsJeu )
        {
            _nbTemps++;
            _champNbTemps.text = $"{_nbTemps}s * 0.5";
            MiseAjour();
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        //Debug.Log("Temps fini");
        
        while(_champTempsScore.color.a < 1)
        {
            //Debug.Log("Temps fini");
            _champTempsScore.color = new Color(1, 1,1, _champTempsScore.color.a + 0.1f);
            int scoreTemp = (int)(_donnePerso.nbTempsJeu * 0.5);
            _champTempsScore.text = $"{scoreTemp}pts";
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(CompterPointage());

    }

    /// <summary>
    /// #tp4 Chrisitan Allan Reolo
    /// Coroutine qui permet d'afficher le pointage du champignon blanc
    /// </summary>
    /// <returns></returns>
    private IEnumerator CompterPointage()
    {
        _champNbPointage.text = _donnePerso.nbPoints + "pts";

        while(_champNbPointage.color.a < 1)
        {
            _champNbPointage.color = new Color(1, 1, 1, _champNbPointage.color.a + 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(CompterScoreTotal());
    }


    /// <summary>
    /// #tp4 Chrisitan Allan Reolo
    /// Calcul le score total du temp et pointage et l'afficher
    /// </summary>
    /// <returns></returns>
    private IEnumerator CompterScoreTotal()
    {
        _donnePerso.scoreTotal = _donnePerso.nbPoints + (int)(_donnePerso.nbTempsJeu * 0.5);
        _champScoreTotal.text = $"{_donnePerso.scoreTotal}pts";

        while(_champScoreTotal.color.a < 1)
        {
            _champScoreTotal.color = new Color(1, 1, 1, _champScoreTotal.color.a + 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        _btnContinuer.enabled = true;
    }

    /// <summary>
    /// #tp4 Chrisitan Allan Reolo
    /// Désactive les event listener
    /// </summary>
    void OnDestroy()
    {
        _evenement.apparaitreTableauBonuse.RemoveListener(MiseAjour); 
    }



}
