using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// #tp3 Carl
/// Augmenter le nombre de joyaux ramassés
/// Ramasser un joyaux
/// </summary>

public class Joyaux : Ramassable
{
    [SerializeField] RetroAction _retroAction; // #tp3 Christian Allan Reolo Avoir accès à la rétroaction
    [SerializeField] SOPerso _donneesPerso; // Avori accès au données du personnage
    [SerializeField] Light2D _lurmiereFleur; // #Tp4 Christian Allan Reolo Avoir accès au lumière du fleur
    [SerializeField] TypeJoyau _typeJoyau; // #Tp5 Carl type du joyaux pour différencier les types (avant basé sur nom du object)
    public TypeJoyau typeJoyau=>_typeJoyau; // #Tp5 Carl type du joyaux pour différencier les types (avant basé sur nom du object)


    // #Tp4 Christian Allan Reolo 
    // Variable du décalage de la rétroaction
    float _decalage = 1.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _estRamasse=false;
        _lurmiereFleur.enabled = true;
    }
    protected override void Ramasser()
    {
        base.Ramasser(); //fait la fonction de base herite
        _donneesPerso.IncrementerJoyaux(this); // #tp3 Carl Augmenter le nombre de joyaux ramassés


        _lurmiereFleur.enabled = false; // #Tp4 Christian Allan Reolo Désactiver la lumière de la fleur

        // #tp4 Permete de Positioner la retroaction en haut du gameObject
        Vector3 position = new Vector3(transform.position.x, transform.position.y + _decalage, transform.position.z);

        // #Tp4 Christian Allan Reolo Afficher la rétroaction
        RetroAction retro = Instantiate(_retroAction, position , Quaternion.identity,transform.parent);

        // #Tp4 Christian Allan Reolo Afficher le nom du joyaux dans la rétroaction
        retro.AfficherTexte("+1 fleur " + _typeJoyau );

        Destroy(gameObject);
    }

public enum TypeJoyau
    {
    bleue,
    verte,
    }
    
}