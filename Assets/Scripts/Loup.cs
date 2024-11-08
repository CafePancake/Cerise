using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  #Synthese Christian Allan Reolo
///  Gère l'interaction entre perso et le loup
/// </summary>
public class Loup : MonoBehaviour
{
    [SerializeField] SOEvents _evenements; // Événements du jeu
    [SerializeField] Transform _positionPerso; // Position du personnage
    [SerializeField] Vector2 _vitesse = new Vector2(5.5f,2.5f); // Vitesse du loup
    bool _estActif; // État du loup
    SpriteRenderer _sr; // Sprite du loup
    Rigidbody2D _rb; // Rigidbody du loup
    AudioSource _audio; // AudioSource du loup
    Animator _animator; // Animator du loup
    Vector2 _mouvement; // Direction du loup
    bool _estEnHaut;
    bool _estEnBas;
    bool _peutJouerSon;

    void Start()
    {
        _estActif = false;
        _peutJouerSon = true;
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _evenements.bougerLoup.AddListener(ActiverLoup);
        _evenements.desactiverLoup.AddListener(Desactiverloup);
    
    }
    void Update()
    {
        // Direction du loup
        Vector2 direction = _positionPerso.position - transform.position;

        direction.Normalize();
        _mouvement = direction;

        // Distance entre le loup et le perso
        float distance = Vector2.Distance(transform.position, Niveau.instance.persoModele.transform.position);

        // Jouer le son de grognement si le perso est proche
        if (distance < 10 && _peutJouerSon)
        {
           StartCoroutine(JouerGronement());
            _peutJouerSon = false;
           Debug.Log("Le loup grogne");
        }


        // Flip le sprite du loup en fonction de la direction du perso
        if (direction.x > 0)
        {
            _sr.flipX = false;
        }
        else
        {
            _sr.flipX = true;
        }
    }


    void FixedUpdate()
    {
        // SI le perso à touché le perso, désactiver le mouvement du loup
        if(_estActif)
        {
            BougerLoup(_mouvement);
        }
    }

    /// <summary>
    ///  Bouger le loup vers la position du perso
    /// </summary>
    /// <param name="direction"></param>
    void BougerLoup(Vector2 direction)
    {
        _rb.MovePosition((Vector2)transform.position + (direction * _vitesse * Time.deltaTime));
    }


    /// <summary>
    ///  Activer le loup de commencer a chasé le perso
    /// </summary>
    void ActiverLoup()
    {
        _estActif = true;
        Niveau.instance.donneSonsGenerateur.JouerSon(_audio, Niveau.instance.donneSonsGenerateur.sonLoup);
    }

    /// <summary>
    ///  Quand le loup touche le perso, désactiver le mouvement du perso
    ///  et l'attraper
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Perso>()!=null)
        {
            _estActif = false;
            _evenements.desactiverMarchePerso.Invoke();
            _evenements.attraperPerso.Invoke();
            _animator.SetTrigger("Attraper");
            Niveau.instance.donneSonsGenerateur.JouerSon(_audio, Niveau.instance.donneSonsGenerateur.sonMort);
            Niveau.instance.donneSonsGenerateur.JouerSon(_audio, Niveau.instance.donneSonsGenerateur.sonGronement);
            Niveau.instance.donnePerso.esterminerNiveau = false;
        }
    }

    private IEnumerator JouerGronement()
    {

            Niveau.instance.donneSonsGenerateur.JouerSon(_audio, Niveau.instance.donneSonsGenerateur.sonGronement);
            yield return null;
    }
    

    /// <summary>
    /// Arréter le mouvement du Loup suite de l'attraper le perso
    /// </summary>
    void Desactiverloup()
    {
        _estActif = false;
    }

    /// <summary>
    ///  Désactiver les événements lrsoqu'on change de scène
    /// </summary>
    void OnDestroy()
    {
        _evenements.activerEventLoup.RemoveListener(ActiverLoup);
        _evenements.desactiverLoup.RemoveListener(Desactiverloup);
    }
}
