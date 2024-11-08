using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui est en charge de détecter si le personnage est en contact avec le sol
/// auteurs du code: Carl Dumais et Christian Allan Reolo 
/// auteur des commentaires: Carl Dumais
/// </summary>
public class DetecterSol : MonoBehaviour
{
    [SerializeField] float _distanceSol = 0.8f; //distance entre le centre du personnage et le sol
    [SerializeField] LayerMask _layerMask; //un layermask qui permet de détecter les collisions avec le sol en ignorant le reste

    protected bool _estAuSol = false; //indique si le personnage touche au sol


    Vector2 _tailleBoite = new Vector2(0.5f,0.2f); //taille de la boite de détection de collision au sol

    virtual protected void FixedUpdate()
    {
        VerifierSol();
    }

    /// <summary>
    /// fonction qui s'occupe de détecter la collision au sol
    /// </summary>
    private void VerifierSol()
    {
        Vector2 pointDepart = (Vector2)transform.position - new Vector2(0,_distanceSol); //le point de départ de la boite est centre du perso converti en vector2 -distance sol

        Collider2D hit = Physics2D.OverlapBox(pointDepart,_tailleBoite,0,_layerMask); //crée la boite qui détecte les collisions
        _estAuSol = (hit != null); //_est au sol devient true quand collision détectée 


    }

    /// <summary>
    /// fonction qui crée un visuel pour voir la boite de détection
    /// </summary>
    void OnDrawGizmos()
    {
        if(Application.isPlaying == false)VerifierSol(); //si le jeu est en arrêt, faire la détection

        Gizmos.color = _estAuSol ? Color.green : Color.red; //vert quand touche au sol, rouge quand non

        Vector2 pointDepart = (Vector2)transform.position - new Vector2(0,_distanceSol); //même ligne que vérifier sol, possibilité de réduire redondance _pointDepart

        Gizmos.DrawCube(pointDepart,_tailleBoite); //dessine la boite
    }
}
