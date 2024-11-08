using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

/// <summary>
/// classe en chage des mouvements du personnage
/// auteurs du code: Carl Dumais et Christian Allan Reolo 
/// auteur des commentaires: Christan ALln Reolo
/// </summary>
public class Perso : DetecterSol
{

    [Header("Les Scriptable Objects")]
    [SerializeField] SOPerso _donnesPerso; // #tp3 Christitan avoir accès au données du personnage
    [SerializeField] SOEvents _evenements; // #tp3 Christitan avoir accès list d'événements


    [Header("Les Particules")]
    [SerializeField] Particule _particuleMarche; // #tp3 particule de marche
    [SerializeField] Particule _particuleVitesse; // #tp3 particule devitesse
    [SerializeField] Particule _particuleInvisibiliter;


    [Header("Les Variables")]
    [SerializeField] float _vitesse = 5f; //vitesse de déplacement du personnage
    [SerializeField] float _forceSaut = 75f; //force du saut
    [SerializeField] int _nbFrameMax = 10; //nombre de frames max ou la force du saut s'exerce
    [SerializeField] bool _directionEstInverse=false;

    [Header("Les Projectiles")]
    [SerializeField] GameObject _projectile; //#tp3 Carl prefab projectile tire par le personnage
    

    [Header("Les lumieres")]	
    [SerializeField] Light2D _lumiere; //#tp3 Carl lumiere du personnage

    [Header("Position du loup")]	
    [SerializeField] Transform _posLoup; //#tp3 Carl lumiere du personnage

    /// <summary>
    ///  Liste de variable bool
    /// </summary>
    bool _veutSauter; //si le joueur appuie sur la barre d'espace
    bool _veutTirer; //#tp3 Carl Si le joueur appui sur souris gauche
    bool _peutTirer=true; //#tp3 Carl si le perso peut tirer
    bool _estEnBonuseVitesse; //# tp3 Chrisitan si le perso est en bonuse vitesse
    bool _peutDoubleSauter = false; //si le personnage est dans une position où il a droit à double sauter
    bool _peutSauter = false; //si le personnage est dans une position où il a droit à double sauter
    bool _estInvisible; //#tp3 Carl bool si le bonus invisible est actif ou non
    public bool estinvisible => _estInvisible;
    bool _peutBouger = true; // #tp4 Christian Allan reolo indique si le personnage peut marcher
    protected bool _peutJouerSonAtteri = false; // #tp4 Chrisitan Allan reolo indique si le personnage a atteri pour jouer le son d'atterissage

    
    /// <summary>
    ///  Liste de variable float
    /// </summary>
    float _cadenceTirNormale = 0.5f; //#tp3 Carl delais avant que le perso tire a nouveau
    float _cadenceTirRapide = 0.2f; //#tp3 Carl delais avant que le perso tire a nouveau quand amelioration projectile de la boutique est acheté
    float _tempsInvisibilite = 10f; //#tp3 Carl temps que le joueur est invisible quand bonus invisibilite (augmente si le joueur rammae bonus invis quand deja invis)
    float _intensiteInvis=0.2f;
    float _coutTirer = 5f; //#tp3 Carl delais avant que le perso tire a nouveau quand amelioration projectile de la boutique est acheté


    int _nbFrameRestant = 0; //nb de frames qui reste avant que le saut arrête d'ajouter de la force
    float _axeHorizontal; //direction du personnage déterminée par le input du joueur

    private Coroutine _macoroutine;
    
    Rigidbody2D _rb; //rigidbody de l'object
    SpriteRenderer _sr; //sprite renderer de l'object
    AudioSource _audioSource; // #tp4 source audio du perso
    Animator _anim; // #tp4 animator du perso
    float mana;
    
    void Awake()
    {
        // _particuleVitesse.SetActive(false);
        _rb = GetComponent<Rigidbody2D>(); //va chercher les components pour les relier aux variables
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>(); // #tp4 va chercher le source audio du perso
        Debug.Log(_donnesPerso.aDeDoubleSaut);
        mana = _donnesPerso.nbMana;
        _evenements.graviteBasse.AddListener(BaisserGravite);
        _evenements.directionInverse.AddListener(InverserDirection);
        _evenements.ActiverBonusVitesse.AddListener(ActiverVitesse);
        _evenements.ActiverBonusInvis.AddListener(ActiverBonusInvis);
        _evenements.activerEventLoup.AddListener(ActiverPersoLurmiere);
        _evenements.attraperPerso.AddListener(AttraperPerso);
        _evenements.desactiverMarchePerso.AddListener(DesactiverPerso);

    }

    /// <summary>
    /// override la fonction héritée de la classe détectersol
    /// </summary>
    override protected void FixedUpdate()
    {
        base.FixedUpdate(); //exécute le fixedupdate de base de detectersol

        if (_peutBouger)
        {
            //déplace le perso horizontalement selon les inputs en réglant la vélocité
            _rb.velocity = new Vector2(_axeHorizontal * _vitesse, _rb.velocity.y); 
            MarcherAnimation(_rb.velocity.x);
            GererSauter();
        }
    }

    void Update()
    {
         if(_veutTirer&&_peutTirer)
            {
               
                mana-=_coutTirer;
                if( mana>0)
                {
                    Tirer();
                    Niveau.instance.donneSonsGenerateur.JouerSon(Niveau.instance.systemDeSons, Niveau.instance.donneSonsGenerateur.sonProjectille); 
                }
                else Debug.Log("Pas assez de mana");
                _evenements.depenserMana.Invoke();
            }  


        // #tp4 Chrisitan Allan reolo indique si le personnage marche ou non
        bool estEnMarche = _axeHorizontal != 0;

        // #tp4 Chrisitan jouer son marche lorseque le joueur marche au sol
        if (estEnMarche && !_audioSource.isPlaying) 
        {
            JouerSonMarcher(true);
        }
        // #tp4 Chrisitan arreter le son marche lorseque le joueur ne marche pas ou n'est pas au sol
        else if (!estEnMarche || !_estAuSol) 
        {
            JouerSonMarcher(false);
        }

        

    }

    /// <summary>
    /// #tp3 Chrisitan Allan reolo 
    /// Gère le saut du personnage
    /// </summary>
    private void GererSauter()
    {
        if (_veutSauter && _peutSauter)
        {
            float fractionForce = (float)_nbFrameRestant / _nbFrameMax; //fraction force fait en sorte que le saut devient moins puissant avec le temps dépendament du ration framemax et framerestant

            _peutJouerSonAtteri = true; // #tp4 le joueur peut jouer le son d'atterissage

            Vector2 vecteurForce = fractionForce * _forceSaut * new Vector2(0, 21); //augmenter _forceSaut n'a pas les effets attendus force pas assez grande pour lever le perso, solution présentement: changer vector.up avec vector 0,18

            if (_nbFrameRestant > 0) _nbFrameRestant--; //perds une framerestant chaque fixedupdate(50 frames)

            _rb.AddForce(vecteurForce); //ajoute la force pour faire sauter le perso
        }
        else if (_estAuSol)
        {
            if (_peutJouerSonAtteri) // #tp4 si le joueur a atteri
            {
                // #tp4 Chrisitan jouer son atterissage lorseque le joueur touche le sol
                Niveau.instance.donneSonsGenerateur.JouerSon(_audioSource, Niveau.instance.donneSonsGenerateur.sonAterrissage);
                _peutJouerSonAtteri = false;
                // Debug.Log("Atteri");
            }
            _nbFrameRestant = _nbFrameMax; //le framerestant se réinitialise quand le joueur touche le sol
            _peutDoubleSauter = true; //le joueur regagne la capacité de double saut
            _peutSauter = true; //le joueur regagne la capacité de sauter
        }
        // #tp4 Chrisitan jouer son d'atterissage lorseque le joueur touche le sol
        else
        {
            _peutJouerSonAtteri = true;
            if(!_donnesPerso.aDeDoubleSaut) _peutSauter = false; //le joueur ne peut plus sauter
           
        }

        SauterAnimation(_estAuSol);
    }

    /// <summary>
    /// fonction qui s'active quand il y a un input de mouvement, gère la direction
    /// </summary>
    /// <param name="value">ce que le bouton appuyé retourne comme valeur</param>
    void OnMove(InputValue value)
    {
         _axeHorizontal = _directionEstInverse?-value.Get<Vector2>().x:value.Get<Vector2>().x;
        if (_axeHorizontal < 0) _sr.flipX = true;
        else if (_axeHorizontal > 0) _sr.flipX = false; //retourne le perso en fontion de la direction du déplacement
        ActiverTypeParticule(_axeHorizontal);
    }

    /// <summary>
    /// fonction qui s'active quand on appuie ou relache le bouton ded saut (barre espace)
    /// </summary>
    /// <param name="value">valeur retourné par le boutono saut</param>
    void OnJump(InputValue value)
    {
        _veutSauter = value.isPressed;

        //si appui pour sauter en étant dans les airs, peut doublesaut et a débloqué le pouvoir
        if (_veutSauter && !_estAuSol && _peutDoubleSauter && _donnesPerso.aDeDoubleSaut && _peutSauter) 
        {
            _rb.velocity = new Vector2(_axeHorizontal * _vitesse, 0); //reset de vélocité en y pour un mouvement plus prévisible (moins dégeu)
            _nbFrameRestant = _nbFrameMax; //redonne des frames pour sauter
            _peutDoubleSauter = false; //ne peut plus double saut (jusqu'a toucher le sol)
        }

        // #tp4 Chrisitan jouer son de saut lorseque le joueur saute
        if (_veutSauter && _estAuSol)
        {
            Niveau.instance.donneSonsGenerateur.JouerSon(_audioSource, Niveau.instance.donneSonsGenerateur.sonSaut);
        }

    }

    /// <summary>
    ///  #tp3 Carl fonction qui s'active quand on appuie sur le bouton de tir
    /// </summary>
    /// <param name="value"> valeur pour savoir si le joueur appuie sur le touche </param>
    void OnFire(InputValue value)
    {
        _veutTirer=value.isPressed;


        if(_peutTirer)StartCoroutine(Recharger());
        if(!_veutTirer)StopCoroutine(Recharger());

    }

    /// <summary>
    ///  #tp3 Carl fonction qui tire un projectile
    /// </summary>
    void Tirer()
    {
        Instantiate(_projectile, transform.position, Quaternion.identity);
        _peutTirer=false;
    }
   
   /// <summary>
   ///  #tp3 Carl coroutine qui controle le rythme des projectile tiré
   /// </summary>
   /// <returns></returns>
    IEnumerator Recharger()
    {
        while(_veutTirer)
        {
            if(!_donnesPerso.aProjectilesAmeliores)
            {
                yield return new WaitForSeconds(_cadenceTirNormale);  
            }
            else yield return new WaitForSeconds(_cadenceTirRapide);
 
            _peutTirer = true;
            Debug.Log("recharge");
        }
    }

    /// <summary>
    /// #tp3 Chrisitan Allan reolo activer l'effet du bonuse vitesse
    /// </summary>
    void ActiverVitesse()
    {
        StartCoroutine(ChangerVitesse(10f));
        _estEnBonuseVitesse = true;
    }

    /// <summary>
    /// #tp3 Chrisitan Allan reolo coroutine qui change la vitesse du personnage pour un certain temps
    /// </summary>
    /// <param name="secondes"> duration de l'effet du bonuse </param>
    /// <returns></returns>
    private IEnumerator ChangerVitesse(float secondes)
    {
        _vitesse = 10f;

        yield return new WaitForSeconds(secondes);
        _vitesse = 5f;
        _estEnBonuseVitesse = false;
    }

    /// <summary>
    /// #tp3 Carl activer l'effet du bonuse invisibilité
    /// </summary>
     public void ActiverBonusInvis()
    {
        if(!_donnesPerso.estInvisible)StartCoroutine(DevenirInvisible());
    }

    /// <summary>
    /// #tp3 Carl coroutine qui rend le personnage invisible pour un certain temps
    /// et change l'opacity du sprite renderer
    /// </summary>
    /// <returns></returns>
    private IEnumerator DevenirInvisible()
    {
        _donnesPerso.estInvisible = true;
        _sr.color = new Color(1f,1f,1f,_intensiteInvis);
 
        yield return new WaitForSeconds(_tempsInvisibilite);
        _sr.color = new Color(1f,1f,1f,1f);
        _donnesPerso.estInvisible = false;
    }

    /// <summary>
    ///  Changer la particule du perso lorsqu'il marche
    /// </summary>
    /// <param name="_estEnmarche"> état si le perso marche ou non</param>
    void ActiverTypeParticule( float direction)
    {
        if ( direction != 0 )
        {
            if (_estEnBonuseVitesse == true)
            {
                _particuleMarche.ArreterParticule();
                _particuleVitesse.DemarerMarcheParticule(direction);
            }
            else if (_donnesPerso.estInvisible)
            {
                _particuleMarche.ArreterParticule();
                _particuleInvisibiliter.DemarerMarcheParticule(direction);
            }
            else
            {
                _particuleInvisibiliter.ArreterParticule();
                _particuleVitesse.ArreterParticule();
                _particuleMarche.DemarerMarcheParticule(direction);
            }
        }
        else if ( direction == 0)
        {
            _particuleMarche.ArreterParticule();
        }
    }

    void DesactiverPerso()
    {
        _peutBouger = false;
        _anim.SetFloat("vitesseX",0);
        _anim.SetBool("estAuSol",true);
        Niveau.instance.donneSonsGenerateur.FermerAudioSource(_audioSource);
        Debug.Log("Desactiver perso");
    }

    void MarcherAnimation( float direction )
    {
        if(direction == 0 || !_peutBouger)
        {
            _anim.SetFloat("vitesseX", 0);
            _anim.SetBool("estAuSol", true);
        }
        else
        {
            _anim.SetFloat("vitesseX", direction);
            _anim.SetBool("estAuSol", true);
        }
    }
    void SauterAnimation( bool estAuSol)
    {
        if (estAuSol == false)
        {
            _anim.SetBool("estAuSol",false);
            _anim.SetFloat("vitesseY",_rb.velocity.y);
        }
        else
        {
            _anim.SetBool("Effector",false);
            _anim.SetBool("estAuSol",true);
        }

    }

    void JouerEffectorAnimation()
    {
        _anim.SetBool("Effector",true);
    }


    /// <summary>
    /// #tp4 Chrisitan Allan reolo jouer le son de marche lorseque le joueur marche
    /// </summary>
    /// <param name="estEnMarche"> bool permettant de savoir si le perso bouge ou non</param>
    private void JouerSonMarcher( bool estEnMarche)
    {
        // #tp4 Chrisitan jouer son marche lorseque le joueur marche au sol
        if ( estEnMarche && _estAuSol)
        {
            Niveau.instance.donneSonsGenerateur.JouerSonLoop(_audioSource, true);
        }
        // #tp4 Chrisitan arreter le son marche lorseque le joueur ne marche pas
        else 
        {
            Niveau.instance.donneSonsGenerateur.JouerSonLoop(_audioSource, false); 
        }
    }

    /// <summary>
    /// Augmenter l'intensité de la lumière du personnage
    /// </summary>
    void ActiverPersoLurmiere()
    {
        Debug.Log("Activer lumiere");
        StartCoroutine(ChangerVolLumiere( 5f, 1.23f));
    }

    /// <summary>
    /// Manipuler l'intensité de la lumière du personnage
    /// </summary>
    /// <param name="duree">Le temp de transition vers le cible désiré</param>
    /// <param name="intCible">La valeur qu'on veut atteidre</param>
    /// <returns></returns>
    private IEnumerator ChangerVolLumiere( float duree, float intCible)
    {
        float intDebut = _lumiere.intensity;
        float tempsEcoule = 0f;
        while(tempsEcoule < duree)
        {
            tempsEcoule += Time.deltaTime;
            float fraction = tempsEcoule/duree;
            _lumiere.intensity = Mathf.Lerp(intDebut, intCible, fraction);
            yield return null;
        }
    }

    /// <summary>
    /// Bouger le personnage vers la position du loup
    /// quand il se fait attrapé
    /// </summary>
    void AttraperPerso()
    {
        StartCoroutine(CapturerPerso());
        StartCoroutine(ChangerVolLumiere( 1f, 0f));
        _rb.gravityScale = 0;
    }

    /// <summary>
    /// Permet de bouger le personnage vers la position du loup
    /// </summary>
    /// <returns></returns>
    private IEnumerator CapturerPerso( )
    {
        Vector3 posDebut = transform.position;
        float tempsEcouler = 0f;
        float duree = 2.5f;

        while(tempsEcouler < duree)
        {
            tempsEcouler += Time.deltaTime;
            float fraction = tempsEcouler / duree;
            transform.position = Vector3.Lerp(posDebut, _posLoup.position, fraction);
            yield return null;
        }

        transform.position = _posLoup.position;

    }

    /// <summary>
    ///  #Synthese Christian Allan Reolo
    ///  Gère l'intéraction entre le perse et les autres objets 
    ///  n'ayant pas leur 'isTrigger' cochée 
    /// </summary>
    /// <param name="other"> le game onject qui rentre en colision avec le perso</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        // #tp4 Chrisitan jouer son d'atterissage lorseque le joueur est en l'air et touche le effector
        if(other.gameObject.GetComponent<Effector>()!=null && !_estAuSol)
        {
            Niveau.instance.donneSonsGenerateur.JouerSon(_audioSource, Niveau.instance.donneSonsGenerateur.sonEffector);
            JouerEffectorAnimation();
        }
        else if(other.gameObject.GetComponent<Ennemi>()!=null)
        {
            Niveau.instance.donneSonsGenerateur.JouerSon(_audioSource, Niveau.instance.donneSonsGenerateur.sonDouleur);
            Niveau.instance.evenenment.perdreVie.Invoke();
        }
    }

    /// <summary>
    /// #Synthese Christian Allan Reolo
    ///  Gère l'intéraction entre le perse et les autres objets 
    ///  nayant leur 'isTrigger' coché
    /// </summary>
    /// <param name="other"> le game onject qui rentre en colision avec le perso </param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<ZoneDeGaz>()!=null)
        {
            Niveau.instance.donneSonsGenerateur.JouerSon(_audioSource, Niveau.instance.donneSonsGenerateur.sonDouleur);
        }
    }

    void BaisserGravite()
    {
        _rb.gravityScale/=2f;
    }
    void InverserDirection()
    {
        _directionEstInverse=true;
    }





    /// <summary>
    /// #tp3 Chrisitan Allan reolo 
    /// Enlève les event listeners lorsque 
    /// le script est détruit ou quitte la scène
    /// </summary>
    void OnDestroy()
    {
        _evenements.ActiverBonusVitesse.RemoveAllListeners();
        _evenements.ActiverBonusInvis.RemoveAllListeners();
        _evenements.activerEventLoup.RemoveAllListeners();
        _evenements.attraperPerso.RemoveAllListeners();
        _evenements.desactiverMarchePerso.RemoveAllListeners();
        _evenements.directionInverse.RemoveAllListeners();
        _evenements.graviteBasse.RemoveAllListeners();
    }
}