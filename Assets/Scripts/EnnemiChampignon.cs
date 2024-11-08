using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
///  #Synthese Christian Allan Reolo
///  Gère l'interaction entre perso et le champignon
/// </summary>

public class EnnemiChampignon : Ennemi
{
    [SerializeField] Image _imageGas; // Fond rose qui apparait quand le perso est dans le gaz
    [SerializeField] GameObject _zoneDeGaz; // Collider de l'ennemi


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Calcule la distance entre le perso et l'ennemi
        float distance = Vector2.Distance(transform.position, Niveau.instance.persoModele.transform.position);

        // Si la distance entre le perso et l'ennemi est inférieure à 2 et que le perso n'est pas invisible
        if (distance < 2 && !Niveau.instance.donnePerso.estInvisible)
        {
            StopCoroutine(_coroutPatrouille);
            _anim.SetTrigger("Exploser");
            StartCoroutine(JouerGaz());

        }

    }

    /// <summary>
    ///  Apparaitre le gaz à la position de l'énnemi et détruire l'ennemi
    /// </summary>
    IEnumerator JouerGaz()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(_zoneDeGaz, transform.position+Vector3.down/2, Quaternion.identity);
        Niveau.instance.donneSonsGenerateur.JouerSon(Niveau.instance.systemDeSons, Niveau.instance.donneSonsGenerateur.sonExplosion);
        Destroy(gameObject);
    }
}
