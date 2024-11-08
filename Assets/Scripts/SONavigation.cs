using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// classe gère la navigation entre les scènes
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan ALln Reolo
/// </summary

// les nom donées lorsqu'on créer le fichier ou lorsqu'on le survole dans la barre de menu 
[CreateAssetMenu(fileName = "MaNavigation", menuName = "Navivation")] 

public class SONavigation : ScriptableObject
{
    [Header("Les Scriptable Objects: ")]
    [SerializeField] SOPerso _donnesPerso; // Récupère les donnés relier au personnage

    /// <summary>
    /// Aller à la scène niveau pour débuter le jeux
    /// </summary>
    public void Jouer()
    {
        _donnesPerso.Renitialiser();
        AllerSceneSuivant();
    }

    /// <summary>
    /// Aller à la scène Boutique est désactiver les améliorations
    /// </summary>
    public void AllerSceneApresNiveau()
    {
        _donnesPerso.RenitialiserAffichage();

        if(_donnesPerso.esterminerNiveau)
        {
            AllerScene("Boutique");
        }
        else
        {
            AllerScene("TableauHonneur");
        }

    }

    /// <summary>
    /// Retourner la scène précédents 
    /// </summary>
    public void RetournerArriere()
    {
        _donnesPerso.niveau++;
        _donnesPerso.evenementMiseAjour.Invoke();
        AllerScenePrecedent();
    }


    /// <summary>
    /// Aller au scene suivant
    /// </summary>
    public void AllerSceneSuivant()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Debug.Log("Scene suivant");
    }

    /// <summary>
    /// Aller au scene Precedent
    /// </summary>
    public void AllerScenePrecedent()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        Debug.Log("Scene precedent");
    }
    
    /// <summary>
    /// Aller a scene spécifique
    /// </summary>
    public void AllerScene( string nomScene)
    {
        SceneManager.LoadScene(nomScene);
    }
}
