using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// #TP3: Chrisitan Allan Reolo
/// classe qui controle l'intéractivité de la porte
/// auteurs du code: Christian Allan Reolo 
/// auteur des commentaires: Christan Allan Reolo
/// </summary

public class Porte : MonoBehaviour
{
    [Header("Les Scriptable Objects")]
    [SerializeField] SOPerso _donnePerso; // Récupère les donnés relier au personnage
    [SerializeField] SOEvents _evenement; // #Tp4 Avoir accès au liste d'événement


    [Header("Les Lumières")]
    [SerializeField] Light2D _lurmierePorte1; // #Tp4 Avoir accès au fonction de l'activation des particules
    [SerializeField] Light2D _lurmierePorte2; // #Tp4 Avoir accès au fonction de l'activation des particules

    [Header("Les Sprites")]
    [SerializeField] Sprite _porteOuvert; // image de la porte ouvert

    [Header("Les Particules")]
    [SerializeField] Particule _particulePorte; // Avoir accès au fonction de l'activation des particules

    SpriteRenderer _sr; // Le SpriteRenderer de la porte

    AudioSource _audioSource; // #Tp4 Le AudioSource de la porte

    /// <summary>
    /// Permet de d'arrêter le Particule de la porte dès l'ouverture du niveau
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>(); // #Tp4 Récupère le AudioSource de la porte
        _particulePorte.ArreterParticule(); // Arrter le particule
        Niveau.instance.donneSonsGenerateur.JouerSon(_audioSource, Niveau.instance.donneSonsGenerateur.sonPorte);

        //#tp4 Christian Allan Reolo
        // Les lumières de la porte
        _lurmierePorte1.enabled=true;
        _lurmierePorte2.enabled=false;
    }

    /// <summary>
    /// Controlle l'ouverture de la porte avec le personnage
    /// </summary>
    /// <param name="other"> le gameObject qui intéragit avec la porte</param>
     void OnTriggerEnter2D (Collider2D other)
    {
        // Si le tag de l'objet qui touche la porte est le Player
        // et que le Player a récuperé la clé pour la porte
        if ( other.gameObject.GetComponent<Perso>()!=null && _donnePerso.acle)
        {
            if(_sr.sprite == _porteOuvert)
            {
                return;
            }

            _sr.sprite = _porteOuvert; 
            _donnePerso.VerifierCle(false);
            _particulePorte.ActiverParticule(); // Jouer la particule de la porte
            Niveau.instance.donneSonsGenerateur.JouerSon(_audioSource, Niveau.instance.donneSonsGenerateur.sonPorte);
            Niveau.instance.donnePerso.esterminerNiveau = true; // #Tp4 Le joueur a terminer le niveau
            _lurmierePorte2.enabled = true; // #Tp4 Activer la lumière gauche de la porte
            _evenement.arreterTemps.Invoke(); // #Tp4 Arreter le temps 
            _evenement.desactiverLoup.Invoke(); // #synhteseDesactiver le loup
            StartCoroutine(ApparaitrePanneauBonues()); // #Tp4 Apparaitre le tableau de bonuse
            
        }
    }


    /// <summary>
    /// #tp4 Chrisitan Allan Reolo
    /// Coroutine qui permet de faire apparaitre le tableau de bonuse
    /// dans la scène du niveau
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApparaitrePanneauBonues()
    {
        yield return new WaitForSeconds( 5f);
        _evenement.apparaitreTableauBonuse.Invoke(); // #Tp4 Apparaitre le tableau de bonuse
    }


}
