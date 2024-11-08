using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fee : Ennemi
{
    override protected void FaireActionCollision(Perso perso)
    {
        //perso.EnleverVies(_dommageContact);
        // ObtenirPosProchaineDestination();
        // _rb.MovePosition(destinations[_iDest].position);
        /// teeporte quand se fait attaquer, trop de confusion, soit ajouter animtion/bruit ou changer

    }
}
