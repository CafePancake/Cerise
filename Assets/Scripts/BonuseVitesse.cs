using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// classe qui gère l'appele de l'activation de l'effet bonuse
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary

public class BonuseVitesse : Bonus
{
    [SerializeField] SOEvents _evenementsBonuse; // Avoir accès au list d'événements

    protected override void Ramasser()
    {
        base.Ramasser(); //fait la fonction de base herite

        // appele l'événement pour activer l'effet de la bonuse vitesse
        _evenementsBonuse.ActiverBonusVitesse.Invoke(); 
    }
}
