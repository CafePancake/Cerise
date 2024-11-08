using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// classe qui controle l'intéractivité du cle
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan ALln Reolo
/// </summary

public class Cle : MonoBehaviour
{

    [Header("Les Scriptable Objects")]
    [SerializeField] SOPerso _donnnePerso; // Récupère les donnés relier au personnage
    [SerializeField] SOEvents _evenements; // ScriptableObject contenant les événements

    [Header("Les Sprites")]
    [SerializeField] Sprite _cleRecuperer; // Image du clé récuprer

    [Header("Les Lumières")]
    [SerializeField] Light2D _lurmiereCle; // #Tp4 Avoir accès à la lurmière du clé


    SpriteRenderer _sr; // Le SpriteRenderer du clé

    AudioSource _audioSource; // #Tp4 Le AudioSource du clé

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>(); // #Tp4 Récupère le AudioSource du clé
        _lurmiereCle.enabled = true; 
    }


    /// <summary>
    /// Controlle la récupération du clé avec le personnage
    /// </summary>
    /// <param name="other"> le gameObject qui intéragit avec la clé < </param>
    void OnTriggerEnter2D (Collider2D other)
    {
        // Si le tag de l'objet qui touche la porte est le Player
        if (other.gameObject.GetComponent<Perso>()!=null)
        {
            // Si le personnage a déjà récupéré la clé
            if (_donnnePerso.acle)
            {
                return;
            }
            _donnnePerso.VerifierCle(true); // Augmente le nombre de clé récupérer
            _sr.sprite = _cleRecuperer; // Change l'image de la clé

            // #Tp4 Joue le son de la clé
            Niveau.instance.donneSonsGenerateur.JouerSon(_audioSource, Niveau.instance.donneSonsGenerateur.sonClef);
            _lurmiereCle.enabled = false; // #Tp4 Désactiver la lumière de la clé
            _evenements.activierEventCLef.Invoke(); // #Tp4 Active l'événement de la clé
        }
    }
}
