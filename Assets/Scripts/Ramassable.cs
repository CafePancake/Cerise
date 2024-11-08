//#tp3 Carl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// classe pour tous les objets qui pourront etre "ramasse" par le joueur
/// </summary>
public class Ramassable : MonoBehaviour
{
    protected bool _estRamasse; //bool si le joueur a ramass/ l<objet (collision)

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Perso>()!=null) //si collision avec joueur
        {
            if(!_estRamasse) 
            {
                Ramasser(); 
                //Debug.Log("Ramasser"); //si pas ramass/, ramasse
            }
        }
    }

    /// <summary>
    /// fonction qui sera override par les objets a ramasser pour avoir des effets individuels
    /// </summary>
    protected virtual void Ramasser()
    {
        _estRamasse = true; //objet est ramasse
        //Debug.Log("Objet ramasse:"+this.name); //dire quoi on a ramasse dans la console
    }
}
