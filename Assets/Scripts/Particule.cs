using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// Une Class qui gère l'apparition et at arret des particule
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary

public class Particule : MonoBehaviour
{
    
    ParticleSystem _part; // le particuleSystem du gameObject
    ParticleSystem.MainModule _main; // le main module du particleSysteme
    ParticleSystem.ShapeModule _shape; // le shape module du particle system


    void Awake()
    {
        _part = GetComponent<ParticleSystem>();
        _main = _part.main;
        _shape = _part.shape;
        // _part.Pause(); // Arreter la particule dès l'ouverture d'une scène

    }

    /// <summary>
    /// Activer les particule relier a la marche du perso ainsi
    /// que les bonuses
    /// </summary>
    /// <param name="direction">  la valeur de la directions des particule </param>
    public void DemarerMarcheParticule(float direction)
    {
        _part.Play();
        _shape.scale = new Vector3 (1f,direction,1f);
    }

    /// <summary>
    /// Permet d'activer la particule du porte
    /// </summary>
    public void ActiverParticule()
    {
        _part.Play();
    }

    /// <summary>
    /// Arréter les particules
    /// </summary>
    public void ArreterParticule()
    {
        _part.Stop();
    }

    
}
