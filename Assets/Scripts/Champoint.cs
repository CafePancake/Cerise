using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
//#tp3 Carl
public class Champoint : Ramassable
{
    [SerializeField] SOPerso _donneesPerso;
    [SerializeField] RetroAction _retroAction; // #tp3 Christian Allan Reolo Avoir accès à la rétroaction


    // #Tp4 Christian Allan Reolo 
    // Variable du décalage de la rétroaction
    float _decalage = 1.5f;

    protected override void Ramasser()
    {
        base.Ramasser(); //fait la fonction de base herite
        _donneesPerso.AugmenterPoints(10); // #tp3 Carl Augmenter le nombre de champignons ramassés

        // #tp4 Christian Allan R0olo Permete de Positioner la retroaction en haut du gameObject
        Vector3 position = new Vector3(transform.position.x, transform.position.y + _decalage, transform.position.z);

        // #Tp4 Christian Allan Reolo Afficher la rétroaction
        RetroAction retro = Instantiate(_retroAction, position, Quaternion.identity,transform.parent);

        // #Tp4 Christian Allan Reolo Afficher le nom du champignon dans la rétroaction
        retro.AfficherTexte("+1 " + name );
        
        Destroy(gameObject);
    }
}
