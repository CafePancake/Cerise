using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Classe qui est en charge de la récupération des tuiles du tilemap
/// de l'objet CarteTuiles et les envoyer au TilemapNiveau
/// Gère la opacité des CarteTuiles 
/// auteurs du code: Carl Dumais et Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary>

public class CarteTuiles : MonoBehaviour
{
    // Probalité d'appparition des CarteTuiles
    [SerializeField] [Range(0,100)] int _probabilite;

    

    // Récupère le tilemap du CarteTuile
    Tilemap _tilemap;
    

    /// <summary>
    /// Dès l'ouverture du jeu, récupère les tuiles du tilemap 
    /// de l'objets CarteTuile et les envoyer au TilemapNiveau
    /// </summary>
    void Awake()
    {
        Tilemap _tilemap = GetComponent<Tilemap>();

        // Récupère les limite du tilemap 
        // en taille de cellule
        BoundsInt bounds = _tilemap.cellBounds;

        // Si la pige aléatoire est plus petit ou égale
        // à la probabilité du carte tuile
        if(Random.Range(0, 100) <= _probabilite)
        {   
            // récupère les valeur entiers en hauter du CarteTuile  (axe y)
            for (int y= bounds.yMin; y < bounds.yMax; y++)
            {
                // récupère les valeur entiers en largeur du CarteTuile  (axe x)
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {   
                    // Positionement d'un tuile selon
                    // les valeur entier récupéré
                    Vector3Int pos = new Vector3Int (x,y);

                    // Récupère la tuile selon sa position
                    TileBase tuile = _tilemap.GetTile(pos);

                    Vector3 TileMapPos = transform.position;

                if ( tuile != null)
                {
                    // Appel la fonction publique TransfererTuile du script Jardin
                    // pour appliquer les tuiles du parcelle dans le tailemap du jardin

                    Niveau.instance.TransfererTuile(pos,tuile,TileMapPos);
                }

  
                }
            }
        }

        /// Désactiver le CarteTuile
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Permet de visualiser la transparance des CarteTuiles
    /// hors du jeu
    /// </summary>
    void OnDrawGizmos()
    {
        MontrerProbabilite();
    }

    /// <summary>
    /// Jouer avec l'opacité des CarteTuiles 
    /// selon leur probabilité
    /// </summary>
    void MontrerProbabilite()
    {
        // Si le jeu est fermer
        if (Application.isPlaying == false)
        {
            Tilemap _tilemap = GetComponent<Tilemap>();

            // Affecte l'opacité du CarteTuile selon sa propabilité
            _tilemap.color = new Color(1f,1f,1f,(float)_probabilite/100);
        }
    }

    

}
