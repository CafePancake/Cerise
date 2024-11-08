//#tp3 Carl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "Evenements", menuName = "Nouveau groupe evenements")]
/// <summary>
/// SO qui contient des evenements!
/// </summary>
public class SOEvents : ScriptableObject

{
    UnityEvent _activateurOn = new UnityEvent(); //l>evenement de l<activateur
    public UnityEvent activateurOn => _activateurOn; //et son accesseur

    UnityEvent _activerBonuseVitesse = new UnityEvent(); //#tp3 Christian event pour bonus vitesse
    public UnityEvent ActiverBonusVitesse => _activerBonuseVitesse; 

    UnityEvent _activerDoubleSaut = new UnityEvent(); //#tp3 Christian event pour activer double saut
    public UnityEvent activerDoubleSaut => _activerDoubleSaut;

    UnityEvent _DepenserMana = new UnityEvent(); //#tp3 Christian event pour réduire mana
    public UnityEvent depenserMana => _DepenserMana;

    UnityEvent _activerBonusInvis = new UnityEvent(); //#tp3 Carl event pour bonus invisibilite
    public UnityEvent ActiverBonusInvis => _activerBonusInvis;
 
    UnityEvent _actualiserInfosBoutique = new UnityEvent(); //#tp3 Carl event pour actualiser les infos nb de joyaux dans le boutique
    public UnityEvent actualiserInfosBoutique => _actualiserInfosBoutique;

    UnityEvent _arreterTemps = new UnityEvent(); //#tp4 Christian Allan event pour arreter le temps
    public UnityEvent arreterTemps => _arreterTemps;

    UnityEvent _apparaitreTableauBonuse = new UnityEvent(); //#tp4 Christian Allan event pour apparaitre le tableau de bonuse
    public UnityEvent apparaitreTableauBonuse => _apparaitreTableauBonuse;

    UnityEvent _desactiverMarchePerso = new UnityEvent(); //#tp4 Christian Allan event pour desactiver le player input
    public UnityEvent desactiverMarchePerso => _desactiverMarchePerso;

    UnityEvent _activierEventCLef = new UnityEvent(); //#tp4 Christian Allan event pour activer l'event de la cle
    public UnityEvent activierEventCLef => _activierEventCLef;

    UnityEvent _activerEventLoup = new UnityEvent(); //#tp4 Christian Allan event pour activer l'event du loup
    public UnityEvent activerEventLoup => _activerEventLoup;

    UnityEvent _attraperPerso = new UnityEvent(); //#Synthese Christian Allan event pour attraper le personnage
    public UnityEvent attraperPerso => _attraperPerso;

    UnityEvent _depenserVie = new UnityEvent(); //#Synthese Christian Allan event pour depenser la vie
    public UnityEvent perdreVie => _depenserVie;

    UnityEvent _bougerLoup = new UnityEvent(); //#Synthese Christian Allan event pour augmenter la vitesse du loup
    public UnityEvent bougerLoup => _bougerLoup;

    UnityEvent _desactiverLoup = new UnityEvent(); //#Synthese Christian Allan event pour desactiver le loup
    public UnityEvent desactiverLoup => _desactiverLoup;

    //||||||||||||||||||||||||||||||||||||||||||||||||||||||| EVENEMENTS BONBONS #tp5 Carl
     UnityEvent _fete = new UnityEvent(); //unityevent qui change la couleur d'un carré semitransparent devant la caméra
     public UnityEvent fete => _fete;
 
     UnityEvent _noirceur = new UnityEvent(); //unityevent qui déclenche seulement la noirceur
     public UnityEvent noirceur => _noirceur;
 
     UnityEvent _graviteBasse = new UnityEvent(); //unityevent qui baisse la gravité qui affecte le personnage joeur
     public UnityEvent graviteBasse => _graviteBasse;
 
     UnityEvent _hauteVitesse = new UnityEvent(); //unityevent qui augmente la vitesse du joueur et des ennemis
     public UnityEvent hautevitesse => _hauteVitesse;
 
     UnityEvent _champiMonde = new UnityEvent(); //unityevent qui augmente la quantité de champignons
     public UnityEvent champiMonde => _champiMonde;
 
     UnityEvent _musiqueEtrange = new UnityEvent(); //unityevent qui change le pitch de la musique
     public UnityEvent musiqueEtrange => _musiqueEtrange;
 
     UnityEvent _directionInverse = new UnityEvent(); //unityevent qui inverse les inputs de mouvement horizontaux
     public UnityEvent directionInverse => _directionInverse;
 
     UnityEvent _messageGentil = new UnityEvent(); //unityevent qui envois un message reconfortant parmis les message d'effets de bonbons :)
     public UnityEvent messageGentil => _messageGentil;
 
     UnityEvent _messageMechant = new UnityEvent(); //unityevent qui envois un message blessant parmis les messages d'effets de bonbons :(
     public UnityEvent messageMechant => _messageMechant;
    
}
