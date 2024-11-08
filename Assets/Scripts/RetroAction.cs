using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// classe qui affiche les gameobjects récupérer
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary

public class RetroAction : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _champ; // texte qui va apparaitre dans le rétroaction

    /// <summary>
    /// Afficher le joyaux récupérer
    /// </summary>
    /// <param name="texte"> le nom </param>
    public void AfficherTexte(string texte)
    {
        _champ.text = texte;
    }


    /// <summary>
    /// Détruire le gameObject a la fin de son animation
    /// </summary>
    public void Detruire()
    {
        Destroy(gameObject);
    }
}
