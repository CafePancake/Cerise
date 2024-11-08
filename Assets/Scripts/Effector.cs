using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
//#tp3 Carl
/// <summary>
/// classe qui sert a generer des particules quand on entre en collision avec l<objet (pour l<objet effector (trampoline))
/// </summary>
public class Effector : MonoBehaviour
{
    [SerializeField] ParticleSystem _particulesBoing;
    [SerializeField] Light2D _lurmiereEffector;
    [SerializeField] AudioSource _audioSource;

    void Start()
    {
        _lurmiereEffector.enabled = true;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        _particulesBoing.Play(); //fait des jolies particules qaund on touche :)
    }
}
