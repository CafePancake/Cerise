using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#tp3 Carl
/// <summary>
/// classe qui declenche un event quand on ramasse le bonus invis (bonus orange)
/// herite de bonus
/// </summary>
public class BonusInvis : Bonus
{
    protected override void Ramasser()
    {
        base.Ramasser(); //fait la fonction de base herite
        _evenements.ActiverBonusInvis.Invoke();
    }
}
