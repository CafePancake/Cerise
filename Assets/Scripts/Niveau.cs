

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Classe gère la génération des salle de façon aléatoire
/// Gère l'intégration de la bordure du niveau
/// auteurs du code: Carl Dumais et Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary>

public class Niveau : MonoBehaviour
{
    
    [Header("Les champs sérialisé relié au niveau: ")]
    [SerializeField] public Tilemap _tileMap;// Tilemap du niveau
    [SerializeField] Salle[] _salleModel; // List des salles différents
    [SerializeField] TileBase _tuileModel;// Tuile de la bordure
    [SerializeField] Vector2Int _nbSalle; //// #tp3 Christian: nombre de salle dans le niveau #tp4 Carl changement de int a vector2int
    public Vector2Int nbSalle => _nbSalle;



    [Header("Les Scriptable Objects: ")]
    [SerializeField] SOPerso _donnePerso; // Donnees du scrpitableObject SOperso
    public SOPerso donnePerso => _donnePerso;
    [SerializeField] SOSonsGenerateur _donneSonsGenerateur; // Donnees du scrpitableObject SOperso
    public SOSonsGenerateur donneSonsGenerateur => _donneSonsGenerateur; 
    [SerializeField] SONavigation _navigation; // Donnees du scrpitableObject SOperso
    public SONavigation navigation => _navigation;
    [SerializeField] SOEvents _evenement; // #pt5 Chrisitan Allan Reolo Donnees du scrpitableObject SOEvents
    public SOEvents evenenment => _evenement;

    [SerializeField] SOBonbons _donneesBonbons; // Donnees par rapport aux bonbons
    


    [Header("Les gameObjects: ")]
    [SerializeField] GameObject _cleModele;// #tp3 Christian: la cle de la prote
    [SerializeField] GameObject _porteModele; // #tp3 Christian: Porte du niveau
    [SerializeField] GameObject _imgBackground;// #tp3 Christian: arriere plan du niveau
    [SerializeField] GameObject _persoModele; // #tp3 Carl prefab pour le trampoline
    [SerializeField] GameObject _panneauBonbon; // #Sytnhese Christian: Ennemi du niveau
    public GameObject persoModele => _persoModele;
    [SerializeField] GameObject[] _ennemiModele; // #Sytnhese Christian: Ennemi du niveau
    [SerializeField] AudioSource SystemDeSons;
    public AudioSource systemDeSons => SystemDeSons;

 


    [Header("Le tableau des gamobjects instantiés: ")]
    [SerializeField] Transform[] _groupesEntites; //#tp3 Carl tableau de transform d<objets empty pour mieux organiser les objets generes (parents/folders)


    [Header("Le confinement du camera: ")]
    [SerializeField]Transform _confinement;

    [Header("Les lurmière: ")]
    [SerializeField] Light2D _lurmiereGlobale; // #Tp5 Avoir accès au fonction de l'activation des particules
    

    [Header("Image: ")]
    [SerializeField] Image _imageNoir; // #Tp5 Avoir accès au fonction de l'activation des particules
    [SerializeField] Image _imageGaz; // #Tp5 Avoir accès au fonction de l'activation des particules

    [Header("Variables reliées au bonbon: ")]
    [SerializeField] TextMeshProUGUI _texteEffetsBonbons; //zone de texte qui affiche les effets au debut de la partie
    [SerializeField] Light2D _lumiereGlobale;
    [SerializeField] float _tempo = 110f; //tempo de la musique (bpm)
    [SerializeField] float _multiplicateurChampimonde = 2f; //multipllie les chances de champignons d<apparaitre par cette variable quand evenement champimonde
    


     [SerializeField] List<MeubleProbabilite> _meubles = new List<MeubleProbabilite>(); //liste des MeubleProbabilite, possede des prefab avec leurs probabilite d<apparaitre

    
    List<Vector2Int> _lesPosLibre = new List<Vector2Int>(); // #tp3 Christian: list des positions de tuiles qui son vide dans le tilemap du niveau
    List<Vector2Int> _lesPosSurRepere = new List<Vector2Int>(); // #tp3 Christian: "list" de position ayant une repere
    List<Vector2Int> _lesPosSol = new List<Vector2Int>(); //#tp3 Carl liste des positions de tuiles du niveau qui sont occupees
    List<Vector2Int> _lesPosObjetValides = new List<Vector2Int>(); //#tp3 Carl liste des positions de tuiles du niveau qui sont occupees

    Vector2Int _tailleSalleAvecBord = Salle.tailleAvecBordure;
    public Vector2Int tailleSalleAvecBord => _tailleSalleAvecBord;
    Vector2Int _tailleNiveauAvecUneBordure;
    Vector2Int _posRep; // #tp3 Christian: Position d'une repere dans le niveau
    Vector2Int _decalage; // #tp3 Christian: Décalage des gameobjectsdans le niveau
    

    Vector2 _decalageObjets = new Vector2(0.5f,1.4f); //#tp3 Carl Décalage des objets pour apparaitre ~au centre des cases au lieu des intersections
    BoundsInt _limitesNiveau; //#tp3 Carl positions tuiles min et max dans le niveau


    float _nbSalleCouvertesParFond = 3f; //#tp4 Carl nombre de salles (en x) qui peuvent etre cououvertes par une instance du fond
    Vector2 _scaleFond= new Vector2(0.9f, 0.85f); //#tp4 Carl ajustement du scle pour le fond pour qu<il rentre dans un 3 par 3


    static Niveau _instance;
    static public Niveau instance => _instance;

    /// <summary>
    /// Dès l'ouverture du jeu, Généré les salles 
    /// dans le tilemap niveau
    /// </summary>
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        // taille d'une salle incluant la bordure d'une côté
        _tailleNiveauAvecUneBordure = _tailleSalleAvecBord - Vector2Int.one;
        
        if(_donnePerso.niveau>=2)_nbSalle.y=3;
        if(_donnePerso.niveau>=2)_nbSalle.x=_donnePerso.niveau; //#tp4 Carl ajuste la taille du niveau selon le niveau
        _panneauBonbon.SetActive(false); // #Tp5 Carl
        _evenement.hautevitesse.AddListener(AccelererTemps);
        _evenement.fete.AddListener(EngagerModeDisco);
        _evenement.noirceur.AddListener(ActiverNoirceur);
        _evenement.champiMonde.AddListener(AjusterProbChampi);
        _evenement.activerEventLoup.AddListener(DiminuerLumiereGlobale); // #Synthese Christian Allan Reolo
        _evenement.attraperPerso.AddListener(ActiverFadeFromBlack); // #Synthese Christian Allan Reolo


    }

    void Start()
    {
        CreerLesSalle();
        CreerLaBordure();
        TrouverPosLibre();
        PlacerImageDeFond();

        MangerBonbons();
        for (int i = 0; i < _meubles.Count; i++)
        {
            // Debug.Log(i);
            GenererMeubles(_meubles[i], _groupesEntites[i]);
        }
    }

    /// <summary>
    /// Créer les salles dans le niveau
    /// et positionne la clé, la porte et le personnage
    /// </summary>
    private void CreerLesSalle()
    {

        // #tp3 postionement la cle, la perso et la porte
        Vector2Int placementCle = new Vector2Int(0, Random.Range(0, _nbSalle.y));

        //Tp4 Carl tentative de convertir nbsalle en vector2Int
        Vector2Int placementPorte = new Vector2Int(_nbSalle.x -1, Random.Range(0, _nbSalle.y)); 

        // #Synthese Christian: positionnement de l'ennemi dans une salle aléatoire
        // Vector2Int placementEnnemi = new Vector2Int(Random.Range(0, _nbSalle.x), Random.Range(0, _nbSalle.y));

        // Vector2Int placementPerso = new Vector2Int(_nbSalle.x-1, placementPorte.y);

        // Générer les salles à l'horizontal
        for (int x = 0; x < _nbSalle.x; x++)
        {
            // Générer les salles au vertical
            for (int y = 0; y < _nbSalle.y; y++)
            {
                Vector2Int placementSalle = new Vector2Int(x, y);
                Vector2Int placementEnnemi = placementSalle ;

                // Positoner une salle à côter d'une salle déja générer 
                Vector2 pos = new Vector2(x * _tailleNiveauAvecUneBordure  .x, y * _tailleNiveauAvecUneBordure  .y);

                // Générer une salle
                Salle salle = Instantiate(_salleModel[Random.Range(0, _salleModel.Length)], pos, Quaternion.identity, transform);
                salle.name = "salle_" + x + "_" + y;
                
                
                PlacerGameObjects(placementCle, placementPorte, placementEnnemi ,placementSalle, salle);
            }
        }

         //#tp4 Carl confinement de camera
        Vector2Int _taille = new Vector2Int(nbSalle.x * tailleSalleAvecBord.x-(_nbSalle.x-1), nbSalle.y * tailleSalleAvecBord.y-(_nbSalle.y-1)); //calculle la taille du niveau en prenant compte des overlapping borders entre les salles
        _confinement.localScale = (Vector2)_taille;
        _confinement.position = new Vector3(-tailleSalleAvecBord.x/2, -tailleSalleAvecBord.y/2); //positione le collider dans coin bas/droite du niveau
    }

    /// <summary>
    /// #tp3 Christian Allan Reolo
    /// Permet de instantiate la clé, la porte et le personnage dans des salle 
    /// </summary>
    /// <param name="placementCle"> positionenemt du cle< /param>
    /// <param name="placementPorte"> positionenemt du porte </param>
    /// <param name="placementPerso"> positionenemt du porte< </param>
    /// <param name="placementSalle"> la salle ou le gameobject va instantiate</param>
    /// <param name="salle"></param>
    private void PlacerGameObjects(Vector2Int placementCle, Vector2Int placementPorte, Vector2Int placementEnnemi ,Vector2Int placementSalle, Salle salle)
    {
        // si la placement de la clé est la même que la placement du salle
        if (placementCle == placementSalle)
        {
            _decalage = Vector2Int.CeilToInt(_tileMap.transform.position);

            // Placer le gameobject par rapport a la position du rerèere du salle
            _posRep = salle.PlacerSurRepere(_cleModele,0,false) - _decalage;

             _lesPosSurRepere.Add(_posRep + Vector2Int.down );
            _lesPosSurRepere.Add(_posRep);

        }
        if (placementPorte == placementSalle)
        {
            // Placer le gameobject par rapport a la position du rerèere du salle
            _posRep = salle.PlacerSurRepere(_porteModele,0,false) - _decalage;
            _lesPosSurRepere.Add(_posRep + Vector2Int.down );
            _lesPosSurRepere.Add(_posRep);

        }
        // if (placementEnnemi == placementSalle)
        // {
        //     // Placer le gameobject par rapport a la position du rerèere du salle
        //     _posRep = salle.PlacerSurRepere(_ennemiModele[0],1, true) - _decalage;
        //     _lesPosSurRepere.Add(_posRep + Vector2Int.down );
        //     _lesPosSurRepere.Add(_posRep);

        // }
    }


    /// <summary>
    /// Créer la bordure du niveau selon le nombre de salle
    /// </summary>
    private void CreerLaBordure()
    {
        // Taille de la niveau selon le nombre de salle en total
        Vector2Int tailleNiveau = new Vector2Int(_nbSalle.x, _nbSalle.y) * _tailleNiveauAvecUneBordure  ;

        // postionement du coin en bas à gauche du niveau 
        Vector2Int min = Vector2Int.zero - _tailleSalleAvecBord/ 2;

        // postionement du coin en haut à droit du niveau 
        Vector2Int max = min + tailleNiveau;

        // récupère les valeur entiers en hauteur du TilemapNveau
        for (int y = min.y; y <= max.y; y++)
        {
            // récupère les valeur entiers en largeur du TilemapNveau
            for (int x = min.x; x <= max.x; x++)
            {
                // Positiontement d'un cellule
                Vector3Int pos = new Vector3Int(x, y);

                // Si la position d'un cellule est au début
                // ou à la fin de la longuer du niveau 
                // sur l'axe des x
                if (x == min.x || x == max.x)
                {
                    _tileMap.SetTile(pos, _tuileModel);
                }

                // Si la position d'un cellule est au début
                // ou à la fin de la longuer du niveau 
                // sur l'axe des y
                else if (y == min.y || y == max.y)
                {
                    _tileMap.SetTile(pos, _tuileModel);
                }
            }
        }
    }

   

    private void TrouverPosLibre()
    {
        BoundsInt bornes = _tileMap.cellBounds;
        _limitesNiveau = bornes; //#tp3 Carl borne enregistree dans une variable pour futur acces

        for (int y = bornes.yMin; y < bornes.yMax; y++)
        {
            for (int x = bornes.xMin; x < bornes.xMax; x++)
            {
                Vector2Int posTuile = new Vector2Int(x,y);
                TileBase tuile = _tileMap.GetTile((Vector3Int)posTuile);
                if (tuile == null)
                {
                    _lesPosLibre.Add(posTuile);
                }
                else _lesPosSol.Add(posTuile); //#tp3 Carl
            }
            _lesPosObjetValides = _lesPosSol; // #tp3 carl
        }

        foreach(Vector2Int pos in _lesPosSurRepere)
        {
            _lesPosLibre.Remove(pos);
            _lesPosObjetValides.Remove(pos);
        }

    }

    public void TransfererTuile( Vector3Int pos, TileBase tuile, Vector3 posTilemap)
    {
        Vector3 decalage = posTilemap - _tileMap.transform.position;

        pos += Vector3Int.FloorToInt(decalage);

        _tileMap.SetTile(pos,tuile);
    }

    /// <summary>
    /// #synthese Carl, remplace la vieille generation hyper longue.
    /// genere des gameobjects dans le niveau selon leur probabilite d<apparaitre
    /// (generation en etapes)
    /// </summary>
    /// <param name="meuble">MeubleProbailite, contient gameobjet et sa probabilite (et string nom)</param>
    /// <param name="groupe">folder ou l<objet sera place dans hierarchie</param>
    void GenererMeubles(MeubleProbabilite meuble, Transform groupe) //#tp5 Carl
    {
        for (int i=0; i<_lesPosObjetValides.Count; i++) //parcoure les pos ou l<on peut mettre un objet
        {
            Vector2Int pos = _lesPosObjetValides[i];
 
            if (_tileMap.GetTile((Vector3Int)pos + Vector3Int.up) == null && pos.y < _limitesNiveau.yMax - 1) //si position au dessus est libre
            {
                if (Random.Range(0f, 100f) <= meuble.probabilite)
                {
                    GameObject instantiation = Instantiate(meuble.objet, (Vector3)((Vector2)pos + _decalageObjets), Quaternion.identity, groupe); //instantie gameobjet a position ajust/e
                    instantiation.name = meuble.nom; //enleve le (clone) du nom, remplace par string nom du MeubleProbabilite
                    _lesPosObjetValides.Remove(pos); //enleve pos des pos ou l<on peut mettre un objet
                }
            }
        }
    }


    /// <summary>
    /// #tp3 Christian
    /// Placer image de fond du niveaux dans la scene niveau
    /// </summary>
    void PlacerImageDeFond() //tp4 Carl instancie plus de fonds si les salles d/passent du fond en x (decalage vers la droite egal au nombre de salle couvertes par un fond pour chawue fond additionel)
    {
        Vector2 pos = _tailleNiveauAvecUneBordure;
        _imgBackground.transform.localScale = _scaleFond;
 
        for (int i = 0; i < Mathf.CeilToInt(_nbSalle.x/_nbSalleCouvertesParFond); i++)
        {
            Instantiate(_imgBackground, pos, Quaternion.identity);
            pos.x += pos.x*_nbSalleCouvertesParFond;
        }
    }


    void DiminuerLumiereGlobale() // #Tp5 Christian Allan Reolo
    {

        StartCoroutine(ChangerVolLumiere(5f, 0.15f ));
    }

    /// <summary>
    /// #Sythense Christian Allan Reolo
    /// Diminuer l'opaciter de la lumiere Global du niveau
    /// </summary>
    /// <param name="duree"> Le temp de transition vers le cible désiré</param>
    /// <param name="intCible"> La valeur qu'on veut atteidre</param>
    /// <returns></returns>
    private IEnumerator ChangerVolLumiere( float duree, float intCible) 
    {
        float intDebut = _lurmiereGlobale.intensity;
        float tempsEcouler = 0f;
        while(tempsEcouler < duree)
        {
            tempsEcouler += Time.deltaTime;
            float fraction = tempsEcouler / duree;
            _lurmiereGlobale.intensity = Mathf.Lerp(intDebut, intCible, fraction);
            yield return null;
        }
    }
    

    /// <summary>
    /// #Synthese Christian Allan Reolo
    ///  Noircir l'écran quand le perso est attrapé
    /// </summary>
    void ActiverFadeFromBlack()
    {
        StartCoroutine(ChangerOpaciter(3f, 1f,_imageNoir));
    }

    /// <summary>
    /// #Synthese Christian Allan Reolo
    /// Simuler une effet de Gaz sur l'écran du jeu
    /// </summary>
    public void ActiverEtatGaz()
    {
        StartCoroutine(ChangerOpaciter(1f, 0.4f,_imageGaz));
    }
    /// <summary>
    /// #Synthese Christian Allan Reolo
    /// Désactiver l'effet de Gaz sur l'écran du jeu
    /// </summary>
    public void DesactiverEtatGaz()
    {
        StartCoroutine(ChangerOpaciter(1f, 0f,_imageGaz));
    }

    /// <summary>
    /// #Synthese Christian Allan Reolo
    ///  Gére l'augmentation et la diminuation de l'opaciter d'une image
    /// </summary>
    /// <param name="duree">Le temp de transition vers le cible désiré</param>
    /// <param name="intCible">La valeur qu'on veut atteidre</param>
    /// <param name="image"> L'image qu'on veut manipuler</param>
    /// <returns></returns>
    private IEnumerator ChangerOpaciter( float duree, float intCible,Image image) // #Tp5 Christian Allan Reolo
    {
        float intDebut = image.color.a;
        float tempsEcouler = 0f;
        while(tempsEcouler < duree)
        {
            tempsEcouler += Time.deltaTime;
            float fraction = tempsEcouler / duree;
            image.color = new Color(image.color.r,image.color.g,image.color.b, Mathf.Lerp(intDebut, intCible, fraction));
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        if(image == _imageNoir) _evenement.apparaitreTableauBonuse.Invoke();
        
    }


    /// <summary>
    /// #synthese Carl
    /// active les events relies aux bonbons selon les effets bonbons que le joueur a et affiche les effets au debut de la partie
    /// </summary>
     void MangerBonbons()
    {
        
        //array de events relies aux bonbons
        UnityEvent[] _evenementsBonbons = new UnityEvent[] { _evenement.fete, _evenement.noirceur, _evenement.graviteBasse, _evenement.hautevitesse, _evenement.champiMonde, _evenement.musiqueEtrange, _evenement.directionInverse, _evenement.messageGentil, _evenement.messageMechant, _evenement.activerEventLoup };
        for (int i = 0; i < _evenementsBonbons.Length; i++) //pour chaque effet
        {
            if (_donnePerso.aBonbon[i]) //si effet en cours
            {
                _panneauBonbon.SetActive(true); // #Tp5 Carl
                _evenementsBonbons[i].Invoke(); //invoke le event
                _texteEffetsBonbons.text += _donneesBonbons.texteEffets[i]+"\n"; //ajoute le texte d<effet retrouv/ dans donnees bonbons (saute une ligne chaque effet)
                // Debug.Log("Cerise a le bonbon" + i);
            }
        }
    }

    /// <summary>
    /// fonction qui active quand certain event bonbon (void .addeventlisteners dans awake)
    /// </summary>
    void AccelererTemps()
    {
        Time.timeScale = 2;
    }

    /// <summary>
    /// fonction qui active quand certain event bonbon (void .addeventlisteners dans awake)
    /// </summary>
    void EngagerModeDisco()
    {
        StartCoroutine(ChangerCouleurLumiere());
    }
 
    /// <summary>
    /// fonction qui active quand certain event bonbon (void .addeventlisteners dans awake)
    /// change le global light pour couleur aleatoire en fonction du rythme de _tempo
    /// </summary>
    /// <returns>return delaye</returns>
    IEnumerator ChangerCouleurLumiere()
    {
        while (true)
        {
            _lumiereGlobale.color = Color.HSVToRGB(Random.Range(0f, 100f) / 100, 1, 1);
            yield return new WaitForSeconds(60 / _tempo);
        }
    }

    /// <summary>
    /// fonction qui active quand certain event bonbon (void .addeventlisteners dans awake)
    /// rend lumiere sombre
    /// </summary>
    void ActiverNoirceur()
    {
        _lumiereGlobale.color = Color.black; //environnement sombre
    }

    /// <summary>
    /// fonction qui active quand certain event bonbon (void .addeventlisteners dans awake)
    /// augmente la probabilite de generer des champignons dans le niveau
    /// </summary>
    void AjusterProbChampi()
    {
        MeubleProbabilite meublecible;
       
        meublecible = _meubles.Find(meuble=>meuble.nom.Contains("activateur"));
        meublecible.probabilite*= _multiplicateurChampimonde;
 
        meublecible = _meubles.Find(meuble=>meuble.nom.Contains("champoint"));
        meublecible.probabilite*= _multiplicateurChampimonde;
 
        meublecible = _meubles.Find(meuble=>meuble.nom.Contains("trampoline"));
        meublecible.probabilite*= _multiplicateurChampimonde;
 
    }
 

    /// <summary>
    /// #tp3 Christian Allan Reolo
    /// Renitialise les valeurs des variables relier au personnage
    /// quand on quite le jeu
    /// </summary>
    void OnApplicationQuit()
    {
        _donnePerso.Renitialiser();
    }

    void OnDestroy()
    {
        _evenement.activerEventLoup.RemoveListener(DiminuerLumiereGlobale); // #Synthese Christian Allan Reolo
        _evenement.attraperPerso.RemoveListener(ActiverFadeFromBlack); // #Synthese Christian Allan Reolo
    }
}
