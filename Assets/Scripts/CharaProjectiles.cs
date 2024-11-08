using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// #Carl
/// Classe qui controle les projectiles du personnage
/// </summary>
public class CharaProjectiles : MonoBehaviour
{
    [SerializeField] float _vitesse = 500f; //vitesse du projectile 
    Vector2 _cible; //position vis/e (avec al souris)
    Vector2 _direction; //direction du projectile (sera calcule avec _target et position du perso)
    int _dommageProjetile = 1; //dommage fait par le projectile
    Rigidbody2D _rb; //rigidbody du projectile


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Vector2 posIni = transform.position;
        _cible = Camera.main.ScreenToWorldPoint(Input.mousePosition); //target est la ou la souris pointe
        _direction = _cible - posIni; //direction est la diff entre pos perso et pos cible
        _rb.AddForce(_direction.normalized * _vitesse, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Ennemi ennemi = other.gameObject.GetComponent<Ennemi>();
        if (ennemi == null)
        {
            Destroy(gameObject); //si pas un ennemi, detruire et ne rien faire
            return;
            //possiblement ajouter particule destruction
        }
        ennemi.EnleverVies(_dommageProjetile); //sinon enlever des vies en fonction du nb de dommage du projectile
        Destroy(gameObject);
    }

}

