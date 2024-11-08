//#tp3 Carl

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;


/// <summary>
/// classe en charge de l<activateur pour les bonus
/// </summary>
public class Activateur : MonoBehaviour
{
    [Header("Les Scriptable Objects")]
    [SerializeField] SOEvents _evenements; //scriptable object qui contient des unityevents
    // [SerializeField] LayerMask _layerMask; //layermask pour que le raycast ne detecte que le perso

    [Header("Les Particules")]
    [SerializeField] ParticleSystem _particulesIntact; //#tp3 Carl particlesystem qui joue de base
    [SerializeField] ParticleSystem _particulesEcrase; //#tp3 Carl particleSystem qui joue quand le joueur active l<activateur

    [Header("Les Sprites")]
    [SerializeField] Sprite _spriteEcrase; //sprite du champignon quand active par perse

    [Header("Les Lumières")]
    [SerializeField] Light2D _lumiere; //lumiere de l'avtivateur

    
    SpriteRenderer _sr; //spriterenderer de l<object

    bool _estEcrase = false; //boul si objet est active ou pas


    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if(!_estEcrase)ActiverActivateur();
    }

/// <summary>
/// fonction qui s<active quand le joueur ecrase l<activateur
/// </summary>
    private void ActiverActivateur()
    {
        _evenements.activateurOn.Invoke(); //appelle l<event dans scriptableobject
        _sr.sprite = _spriteEcrase; 
        _estEcrase=true;
        _lumiere.enabled = false;
        _particulesIntact.Stop(); //#tp3 Carl arrete les particules de base (intact) et joue les particules ecrasement
        _particulesEcrase.Play();
    }
}


