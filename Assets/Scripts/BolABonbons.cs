using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#synthese Carl
/// <summary>
/// classe qui gere le bol a bonbons de la boutique, chance de conner des effets quand on clique
/// </summary>
public class BolABonbons : MonoBehaviour
{
    [SerializeField] SOPerso _doneesPerso; //So qui contient les donnes relatives au personnage (mana vie points)
    [SerializeField] int _limiteLoup; //repr/sente une chance de d/clancher l<evenement du loup apres un certain nombre de bonbon pris
    [SerializeField] ParticleSystem _particleDroit; //particule system a droite du bol quand on clic
    [SerializeField] ParticleSystem _particleGauche; //mmeme chose mais a gauche
    [SerializeField] AudioSource _audio; //audiosource de l<objet
    [SerializeField] Animator _anim; //animator de l<objet
    [SerializeField] AudioClip _sonPige; //son qui joue quand on clique sur le bol
    int _nbonbonsPris=0; //nombre de bonbons pris par le joueur
    int _limiteLoupHaute=101; //affecte la chance de d/clencher l<evenement loup (fonctionne un peu comme le nombre mx d un dé)
    int _limiteLoupBasse=25; //nombre minimum obtenu par le dé 

    void Awake()
    {
        _audio=GetComponent<AudioSource>();
        _anim=GetComponent<Animator>();
    }

    void OnMouseDown()
    {
         _audio.PlayOneShot(_sonPige);
        _particleDroit.Play();
        _particleGauche.Play();
        _anim.SetTrigger("Pige");
        //Faire indications interaction (son, animation et 3e)
        PrendreBonbon();
    }

    /// <summary>
    /// classe qui decide si le bonbon possede un effet et si l<evenement loup est d/clench/
    /// </summary>
    void PrendreBonbon()
    {
        int numBonbon = Random.Range(0,101);//roule un de a 100 faces
        if(numBonbon<_doneesPerso.aBonbon.Length) //si nb plus petit que nb elements dans liste d<effets possibles
        {
            _doneesPerso.aBonbon[numBonbon] = true; //active l<effet bonbon qui correspond a numBonbon
            // Debug.Log(_doneesPerso.aBonbon[numBonbon]+""+numBonbon);
        }
        _nbonbonsPris+=1; //compte le nombre de bonbons pris (effet ou non)
        _limiteLoup = Random.Range(_limiteLoupBasse,_limiteLoupHaute); //chiffre aleatoire entre limite haute et basse chaque pige
        if(_nbonbonsPris>_limiteLoup)_doneesPerso.aBonbon[9]=true; //si a pris plus de bonbons que la limite aleatoire, active effet bonbon qui declanche le loup
        //cela fait en sorte que le loup ne d/clenche pas aussitot que le joueur depasse un nombre de bonbons mais a des plus en plus de chance de le declancher chaque bonbon de plus a partir d<un certain nombre
    }
}
