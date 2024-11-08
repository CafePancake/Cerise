using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//#tp4 Carl
/// <summary>
/// Classe en charge de la scene tableau d<honneur, affichage et coordone la lecture/enregistrement des donnees de scores
/// </summary>
public class TableauScores : MonoBehaviour
{
    [SerializeField] SOSauvegarde _donneeSauvegarde; //SO qui contient la liste des scores et ded la logique relatif a ceux cis
    [SerializeField] SOPerso _donneesPerso; //SO qui contient la liste des scores et ded la logique relatif a ceux cis
    [SerializeField] LigneScore[] _lignesScores; //array de scripts relies aux lignes du tableau
    [SerializeField] LigneScore _ligneSelectionnee; //script qui serait celui de la ligne du joueur si il se retrouve sur le tableau
    NomScore _scoreJoueur;
    NomScore _scoreVide = new NomScore{nom = "Vide", score = 0};
    int _lignePos; //position de la ligne du joueur si a lieu
    string _placeholderEcrire = "[Cliquer pour écrire]"; //texte placeholder quand il est possible que le joueur ecrive son nom sur une ligne du tableau

    void Start()
    {
        _scoreJoueur =  new NomScore{nom="", score=_donneesPerso.scoreTotal};
        // _donneeSauvegarde.ReinitialiserScores();
        _donneeSauvegarde.AjouterScore(_scoreJoueur);
        _donneeSauvegarde.ClasserScores();
        _donneeSauvegarde.EcrireFichier();
        _donneeSauvegarde.LireFichier();
        AfficherScores();
    }

    /// <summary>
    /// Cette fonction demande a chaque ligne du tableau de lire le nom et score associe, 
    /// si l<info d<une ligne du tableau correspond a celle du joueur elle sera mise de l<avant,
    /// </summary>
    void AfficherScores()
    {
        for (int i = 0; i < _lignesScores.Length; i++) //pour chaque ligne du tableau
        {
            if (_donneeSauvegarde.lesScores.Count - 1 < i) _donneeSauvegarde.AjouterScore(_scoreVide); //si il n<y a pas encore assez de scores, creer un score vide

            _lignesScores[i].LireScore(_donneeSauvegarde.lesScores[i]); //demande a la ligne de lire score associe

            if (_donneeSauvegarde.lesScores[i]==_scoreJoueur) //si c<est le score du joueur, met en evidence
            {
                Debug.Log("Ligne du joueur "+i);
                _lignePos = i;
                Debug.Log(_lignePos < _donneeSauvegarde.nbLignestableau);
                Debug.Log(_lignePos +","+ _donneeSauvegarde.nbLignestableau);
                if (_lignePos < _donneeSauvegarde.nbLignestableau) //seulement si le score est sur le tableau
                // (tableau a 6 places, mais pour comparer le code prend en compte les 6 du tableau +1 score du joueur, donc un total de 7 positions)
                {

                    _ligneSelectionnee = _lignesScores[i];
                    _lignesScores[i].SelectionnerLigne(); //selectione la ligne (mise de l<avant et permet modif)
                    _lignesScores[i].ChangerPlaceholder(_placeholderEcrire);
                    Debug.Log("La ligne "+ i+" est select");
                }
                Debug.Log("Ligne du joueur n'est pas select");
            }
        }
    }

    /// <summary>
    /// fonction pour enregistrer les donnees de la ligne du joueur (seulement nom qui change) envoit la position de la ligne sur le tableau
    /// </summary>
    public void Enregister()
    {
         for (int i = 0; i < _lignesScores.Length; i++)
        {
            _lignesScores[i].Sauvegarder(i);
        }
        _donneeSauvegarde.EcrireFichier();

        // _ligneSelectionnee.Sauvegarder(_lignePos)
    }

    void OnApplicationQuit()
    {
        // if (_lignePos < _donneeSauvegarde.nbLignestableau) Enregister(); //enregistre si le joueur quitte la scene pour x raison
        Enregister();
        _donneesPerso.Renitialiser();
    }

    void OnDestroy()
    {
        // if (_lignePos < _donneeSauvegarde.nbLignestableau)  //enregistre si le joueur quitte la scene pour x raison
        Enregister();
        _donneesPerso.Renitialiser();
    }
}
