using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#synthese Carl
/// <summary>
/// classe mere qui s<occupe du comportement des ennemis en general (patrouille) les effets propre au tye d<ennemi sont dans des classes respectives
/// </summary>
public class Ennemi : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _sr; //sprite renderer de l<objet
    [SerializeField] protected Animator _anim; //Animator de l<objet
    [SerializeField] protected Rigidbody2D _rb; //rigidbody de l<objet
    // [SerializeField] protected Transform[] _destinations;
    [SerializeField] List<Transform> _destinations = new List<Transform>(); //liste des transforms qui correspondent aux points de patrouille qui deterineront le mouvement de l<ennemi
    public List<Transform> destinations => _destinations; //accesseur pour la liste
    [SerializeField] protected float _dureeDeplacement=1f; //temps disponible pour que l<ennemi se rende au prochain point
    [SerializeField] protected TypeDeplacement _typeDeplacement; //type enum de deplacements (aleatoire, lineaire)
    [SerializeField] protected float _delaiDestinationSuivante=3f; //delais secondes avant prochaine destination de patrouille
    [SerializeField] protected float _delaiProchainSon=10f; //delais entre les sons idle pour pas trop agacer le joueur
    [SerializeField] protected float _delaiDepart=1f; //delais avant que l<ennemi commence patrouille
    [SerializeField] protected int _pointsVie; //points de vie  d el,ennemi
    [SerializeField] protected int _dommageContact; //quantite de dommage cause au contact
    [SerializeField] protected float _toleranceDist = 0.1f; //marge d<erreur distance entre position et point de patrouille
    [SerializeField] protected bool _estMort = false; //bool est-ce que l<ennemi est mort?
    [SerializeField] protected int _donnePoint = 50; //nb de points donnes au joueur quand il tue un ennemi
    protected int _iDest=0; //represente un index dans liste destinations
    protected Vector2 _posIni; //position initiale losque debute une transition entre destinations
    protected float _tempsActuel; //temps passe depuis debut transition
    [SerializeField] protected AudioSource _audio; //audiosource de l<ennmi, le audio source ne s<entend lorsque proche de l<ennemi 
    [SerializeField] protected AudioClip _sonIdle;// son de base qui joue parfois
    [SerializeField] protected AudioClip _sonMourir; //son quand l<ennemi meurt
    [SerializeField] protected AudioClip _sonOuch; //son quand l<ennemi se fait attaquer (projectile du perso)
    protected Coroutine _coroutPatrouille; //coroutine qui gere la patrouille
    protected Coroutine _coroutSons; //corout qui gere les sons idle
    
    void Awake()
    {
        _audio=GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb.MovePosition(_destinations[_iDest].position);
        _coroutPatrouille = StartCoroutine(CoroutinePatrouiller());
        _coroutSons = StartCoroutine(CoroutineSon());
    }

    /// <summary>
    /// coroutine qui gere les deplacements des ennemis
    /// </summary>
    /// <returns>return delaye</returns>
    IEnumerator CoroutinePatrouiller()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delaiDepart);
            Vector2 posDest = ObtenirPosProchaineDestination(); //choisi parmi listepoints de patrouille
            _posIni = transform.position;
            _tempsActuel=0f;
            while(Vector2.Distance(transform.position,posDest)>_toleranceDist) //tant que pas rendu
            {
                yield return new WaitForFixedUpdate(); //chaque fixed update
                BougerLerp(posDest); //bouge un peu vers destiation (plus d<info en bas)
            }
            
            yield return new WaitForSeconds(_delaiDestinationSuivante); //attends avant de bouger encore
        }
    }

    /// <summary>
    /// coroutine qui gere les sons idle
    /// </summary>
    /// <returns>return delaye</returns>
    IEnumerator CoroutineSon()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f,_delaiProchainSon)); //probleme random ambigu entre unityengine et system?? preciser unityengine
            _audio.PlayOneShot(_sonIdle);
        }
    }

    /// <summary>
    /// obtient prochaine dedstination selon type de deplacement
    /// </summary>
    /// <returns>retourne un vecteur 2 qui contient position de la prochaine destination</returns>
    protected Vector2 ObtenirPosProchaineDestination()
    {
        if(_typeDeplacement==TypeDeplacement.Lineaire)
        {
            if(_iDest<_destinations.Count-1)_iDest++; //si lineaire va toujours au prochain point si pas au dedrnier point
            else _iDest=0; //sinon retourne au premier point
        }

        else if(_typeDeplacement==TypeDeplacement.Aleatoire)
        {
            _iDest=UnityEngine.Random.Range(0, _destinations.Count); //si aleatoire, choisi un point au hasard
        }

        else return _destinations[0].position; //si aucun des types, va au premier point (ex si on ajoute type stationaire ou erreur)
        
        Vector2 posProchaineDestination = _destinations[_iDest].position;
        return posProchaineDestination;
    }

    /// <summary>
    /// bouge le personnage un peu chaque fixed update entre les destinations
    /// </summary>
    /// <param name="posDest">vecteur 2, position ou l<on souhaite aller</param>
    protected void BougerLerp(Vector2 posDest)
    {
        _tempsActuel+=Time.fixedDeltaTime; //calcule le temps qui s<ecoule
        float fraction = _tempsActuel/_dureeDeplacement; //fraction entre temps ecoule et temps dispo pour se deplacer
        fraction = Mathf.SmoothStep(0f,1f,fraction); //smoothstep pour adoucir debut et fin mouvement
        Vector2 nouvPos = Vector2.Lerp(_posIni, posDest, fraction); //cacule position cible entre depart, destination et position dans le temps
        _rb.MovePosition(nouvPos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>();
        if (perso!=null) FaireActionCollision(perso); //si collisione avec joueur appelle action propre a ennemi
        else return;
    }

    /// <summary>
    /// fonction qui pourrait activer les actions propre au type d,ennemi dans les classes filles
    /// </summary>
    /// <param name="perso">script perso, appartient au personnage collision/</param>
    protected virtual void FaireActionCollision(Perso perso)
    {

    }

    /// <summary>
    /// fonction qui eleve les vies a l<ennemi
    /// </summary>
    /// <param name="nbVies">nombre de vies elev/es</param>
    public void EnleverVies(int nbVies)
    {
        _pointsVie-=nbVies;
        _audio.PlayOneShot(_sonOuch);
        if(_pointsVie<=0) Mourir();
    }


    /// <summary>
    /// fonction qui active si l<ennemi n<as plus de points de vie APRES avoir recu du dommage
    /// </summary>
    protected void Mourir()
    {
        // _anim.SetBool("estMort", _estMort);
        StopAllCoroutines();
        _audio.PlayOneShot(_sonMourir);
        Destroy(gameObject);
    }
    
    /// <summary>
    /// enum qui contient des types de deplacements
    /// </summary>
    protected enum TypeDeplacement
    {
    Lineaire,
    Aleatoire,
    }
}


