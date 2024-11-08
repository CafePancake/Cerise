using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MySoundManager", menuName = "SoundManager")]
/// <summary>
///  #tp4 Christian Allan Reolo
///  Un scriptableObject qui contient les données des sons
///  Permet de jouer les sons
/// </summary>
public class SOSonsGenerateur : ScriptableObject
{

    [Header("Liste des sons: ")]
    [SerializeField] private AudioClip _sonMarche; //  Son de marche
    [SerializeField] private AudioClip _sonAterrissage; //  Son d'atterissage
    [SerializeField] private AudioClip _sonSaut; //  Son de saut
    [SerializeField] private AudioClip _sonClef; // Son de clef
    [SerializeField] private AudioClip _sonPorte; // Son de porte
    [SerializeField] private AudioClip _sonLoup; //  Son de loup
    [SerializeField] private AudioClip _sonMort; // Son de mort de cerise
    [SerializeField] private AudioClip _sonDouleur; // Son de dommage de cerise
    [SerializeField] private AudioClip _sonGronement; // Son de gronement du loup
    [SerializeField] private AudioClip _sonEfffector; // Son d'effector
    [SerializeField] private AudioClip _sonExplosion; // Son de bouton
    [SerializeField] private AudioClip _sonProjectille; // Son de victoire


    [Header("Liste valeur pitch: ")]
    [SerializeField] float _pitchMin = 0.5f; // Son d'effector
    [SerializeField] float _pitchMax = 1.5f; // Son d'effector


    // #tp4 Christian Allan Reolo Les getters et setters des sons
    public AudioClip sonMarche { get => _sonMarche; set => _sonMarche = value; }
    public AudioClip sonAterrissage { get => _sonAterrissage; set => _sonAterrissage = value; }
    public AudioClip sonSaut { get => _sonSaut; set => _sonSaut = value; }
    public AudioClip sonClef { get => _sonClef; set => _sonClef = value; }
    public AudioClip sonPorte { get => _sonPorte; set => _sonPorte = value; }
    public AudioClip sonLoup { get => _sonLoup; set => _sonLoup = value; }
    public AudioClip sonEffector { get => _sonEfffector; set => _sonEfffector = value; }
    public AudioClip sonMort { get => _sonMort; set => _sonMort = value; }
    public AudioClip sonGronement { get => _sonGronement; set => _sonGronement = value; }
    public AudioClip sonDouleur { get => _sonDouleur; set => _sonDouleur = value; }
    public AudioClip sonExplosion { get => _sonExplosion; set => _sonExplosion = value; }
    public AudioClip sonProjectille { get => _sonProjectille; set => _sonProjectille = value; }




    /// <summary>
    /// #tp4 Christian Allan Reolo
    /// Fonction qui permet de jouer un son une fois
    /// </summary>
    /// <param name="source"> L'audioSource du gameobject</param>
    /// <param name="son"> l'audioClip qu'on veut jouer</param>
    public void JouerSon(AudioSource source, AudioClip son)
    {
        source.PlayOneShot(son);
        source.pitch = UnityEngine.Random.Range(_pitchMin, _pitchMax);
    }

    /// <summary>
    /// #tp4 Christian Allan Reolo
    /// Fonction qui permet de jouer un son en boucle
    /// </summary>
    /// <param name="source">  L'audioSource du gameobject </param>
    /// <param name="_estLoop"> bool permetant de savoir si le son est loopé ou non </param>
    public void JouerSonLoop(AudioSource source, bool _estLoop)
    {
        if(_estLoop)
        {
            source.loop = true;
            source.Play();

        }
        else
        {
            source.loop = false;
        }
    }

    /// <summary>
    /// #tp4 Christian Allan Reolo
    /// Fonction qui permet de fermer un audioSource d'un gameObject
    /// </summary>
    /// <param name="source"> L'audioSource du gameobject  </param>
    public void FermerAudioSource(AudioSource source)
    {
        source.enabled = false;
    }

}
