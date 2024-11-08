using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #tp4 Christian Allan Reolo
/// Un script qui permet de jouer les musique d'événements
/// Faire un fade in et fade out pour les musiques
/// </summary>

public class Musique : MonoBehaviour
{
    [Header("Les Musiques: ")]
    [SerializeField] AudioSource _base; // Musique pour l'événement de la cle
    [SerializeField] AudioSource _musiqueEventClef; // Musique pour l'événement de la cle
    [SerializeField] AudioSource _musiqueEventLoup; // Musique pour l'événement du loup
    

    [Header("Les Scriptable Objects: ")]
    [SerializeField] SOEvents _evenements; // ScriptableObject contenant les événements

    [Header("Les Valeur Pitch: ")]
    [SerializeField] float _pitchAleaMax = 2f;
    [SerializeField] float _pitchAleaMin = 0.5f;
    [SerializeField] float _dureeTransitionPitch = 3f;
    [SerializeField] float _delaisProchainPitch = 5f;


    /// <summary>
    /// #tp4 Christian Allan Reolo
    /// Assuer que les musiques n'ont pas de volume au début
    /// </summary>
    void Awake()
    {
        _musiqueEventClef.volume = 0f;
        _musiqueEventLoup.volume = 0f;
    }


    /// <summary>
    /// #tp4 Christian Allan Reolo
    /// Activer les fonctions lorsqu'un événement est déclenché
    /// </summary>
    void Start()
    {
        _evenements.activierEventCLef.AddListener(ActiverEvenementCLef);
        _evenements.activerEventLoup.AddListener(ActiverEvenementLoup);
        _evenements.musiqueEtrange.AddListener(RandomiserPitch);
    }


    /// <summary>
    /// #tp4 Christian Allan Reolo
    /// Augmenter la volume de la musique de la clef avec une fade in
    /// </summary>
    void ActiverEvenementCLef()
    {
        StartCoroutine(JouerEffetFade(_musiqueEventClef, 3f, 0.5f));
        Debug.Log("<color=red>Recuperation de la clef: Joue Musique Clef</color>");
        
        // Si la musique du loup est en train de jouer, dimnue sa volume avec un fade out
        if(_musiqueEventLoup.volume > 0)
        {
            StartCoroutine(JouerEffetFade(_musiqueEventLoup, 3f, 0f));
        }
    }


    /// <summary>
    /// #t4p Christian Allan Reolo
    /// Augmenter la volume de la musique du loup avec une fade in
    /// </summary>
    void ActiverEvenementLoup()
    {
        StartCoroutine(JouerEffetFade(_musiqueEventLoup, 3f, 0.5f));
        Debug.Log("<color=green> Il reste seulement la moitier du temps : Joue Musique Loup</color>");

        // Si la musique de la clef est en train de jouer, dimnue sa volume avec un fade out
        if(_musiqueEventClef.volume > 0)
        {
            StartCoroutine(JouerEffetFade(_musiqueEventClef, 3f, 0f));
        }
    }

    /// <summary>
    /// #Synthese Christian Allan Reolo
    /// Coroutine qui permet de faire un effet de fade d'une musique
    /// </summary>
    /// <param name="source">Le audioSource d'un gameobject</param>
    /// <param name="duree"> La durée de transition vers la volCible</param>
    /// <param name="volCible"> La volume qu'o veut atteindre</param>
    /// <returns></returns>
    IEnumerator JouerEffetFade( AudioSource source, float duree, float volCible)
    {
        float volDebut = source.volume;
        float tempsEcouler = 0f;
        while(tempsEcouler < duree)
        {
            tempsEcouler += Time.deltaTime;
            float fraction = tempsEcouler / duree;
            source.volume = Mathf.Lerp(volDebut, volCible, fraction);
            yield return null;
        }

        source.volume = volCible;
    }

    void RandomiserPitch()
    {
        // Debug.Log("eventpitchstart");
        StartCoroutine(ChangerPitch());
    }
    IEnumerator ChangerPitch()
    {
        while (true)
        {
            float pitchDebut = _base.pitch;
            float tempsEcoule = 0f;
            float pitchCible = Random.Range(_pitchAleaMin, _pitchAleaMax);
            while (tempsEcoule < _dureeTransitionPitch)
            {
                tempsEcoule += Time.deltaTime;
                float fraction = tempsEcoule / _dureeTransitionPitch;
                _base.pitch = Mathf.Lerp(pitchDebut, pitchCible, fraction);
                _musiqueEventClef.pitch = Mathf.Lerp(pitchDebut, pitchCible, fraction);
                _musiqueEventLoup.pitch = Mathf.Lerp(pitchDebut, pitchCible, fraction);
                yield return null;
            }
 
            // _base.pitch = pitchCible;
            // _musiqueEventClef.pitch = pitchCible;
            // _musiqueEventLoup.pitch = pitchCible;
            // Debug.Log("pitchchangé");
            yield return new WaitForSeconds(_delaisProchainPitch);
        }
 
    }

    /// <summary>
    /// #tp4 Christian Allan Reolo
    /// Désactiver les events listeners lorsqu'on change de scène
    /// </summary>
    void OnDestroy()
    {
        _evenements.activierEventCLef.RemoveListener(ActiverEvenementCLef);
        _evenements.activerEventLoup.RemoveListener(ActiverEvenementLoup);
        _evenements.activerEventLoup.RemoveAllListeners();
    }
}
