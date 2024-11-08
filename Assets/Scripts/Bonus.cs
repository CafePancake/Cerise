//#tp3 Carl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// classe des bonus (troncs avec champi) !!!h/rite de classe ramassable
/// </summary>
public class Bonus : Ramassable
{

    [Header("Les Scriptable Objects")]
    [SerializeField] protected SOEvents _evenements; //SO qui contient les evenemets


    [Header("Les Sprites")]
    [SerializeField] Sprite _spriteActif; //sprite quand le bonus est active
    [SerializeField] Sprite _spriteInactif; //sprite quand bonus n<est pas active

    [Header("Les Lurmieres")]
    [SerializeField] Light2D _lumiere; //lumiere du bonus


    SpriteRenderer _sr; //sprite renderer de l<objet
    
    void Start()
    {
        _estRamasse = true;
        _lumiere.enabled = false;
        _sr = GetComponent<SpriteRenderer>();
        _evenements.activateurOn.AddListener(PousserChampignons); //ajoute un listener pour le event (quand event activateurOn est appelle execute pousserchampignons)
    }
    
    /// <summary>
    /// pousserchampignon change le sprite et indique que le bonus n<est pas ramasse
    /// </summary>
    void PousserChampignons()
    {
        _estRamasse = false;
        _sr.sprite = _spriteActif;
        _lumiere.enabled = true;
        StartCoroutine(EnleverChampignons());
    }
    IEnumerator EnleverChampignons()
    {
        yield return new WaitForSeconds(10f);
        _estRamasse = true;
        _lumiere.enabled = false;
        _sr.sprite=_spriteInactif;
    }

    /// <summary>
    /// ovveride de ramasser de classe ramassable
    /// </summary>
    protected override void Ramasser()
    {
        base.Ramasser(); //fait la fonction de base herite
        _sr.sprite = _spriteInactif; //change le sprite a celui inactif
        _lumiere.enabled = false;

    //attention, ce code est commun aux deux bonus, faire une variante pour les deux (DONC!! peut etre 2script qui heritent de bonus!!!)

    }

}
