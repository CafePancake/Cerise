using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// classe qui désigne la taille du salle
/// Gère la visuel de la bordure
/// auteurs du code: Carl Dumais et Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary>

public class Salle : MonoBehaviour
{

    [SerializeField] Transform[] _tRepere;
    // [SerializeField] Ennemi _scriptEnnemi;

    
    
    
    // taille du salle incluant sa bordure
    static Vector2Int _tailleAvecBordure = new Vector2Int(28,18); //#tp3 Carl changé taille du niveau, ajouté un getter pour taille du niveau sans bordure, changé le nom de certaines variables

    // Permet d'utiliser la valeur du taille du salle dans 
    // un autre script
    static public Vector2Int tailleAvecBordure => _tailleAvecBordure;

    // taille du salle excluant sa bordure
    static Vector2Int _tailleSansBordure = _tailleAvecBordure-Vector2Int.one;
    static public Vector2Int tailleSansBordure => _tailleSansBordure;

    // Dessine la bordure du salle
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, (Vector2)_tailleSansBordure);
    }

    /// <summary>
    /// Créer une objet dans une parcelle
    /// </summary>
    /// <param name="modele"> Une objet que tu veut mettre dans une parcelle</param>
    public Vector2Int PlacerSurRepere(GameObject modele, int index, bool _estEnnemi)
    {
        Vector3 pos = _tRepere[index].position;

        if(_estEnnemi)
        {
            // _scriptEnnemi.destinations.Add(_tRepere[index]);
            // _scriptEnnemi.destinations.Add(_tRepere[2]);
        }
        Instantiate(modele, pos, Quaternion.identity, transform.parent);
        return Vector2Int.FloorToInt(pos) ;
    }
}
