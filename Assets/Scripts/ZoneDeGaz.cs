using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  #synthese Christian Allan Reolo
///  Gère l'interaction entre perso et le gaz
/// </summary>
public class ZoneDeGaz : MonoBehaviour
{

    /// <summary>
    ///  Envoyer du dommage au perso lorsqu'il rentre en contanct avec le champignon
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Perso>()!=null)
        {
            // Si le perso est dans le gaz, retire la vie graduellement
            Niveau.instance.donnePerso.estDansGas = true;
            StartCoroutine(RetirerVieAuFilDuTemps());
            Niveau.instance.ActiverEtatGaz();

        }
    }

    /// <summary>
    ///  Quand le perso sort du gaz, arrête de retirer la vie
    ///  Disparaitre le fond rose qui indique le perso est dans le gaz
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Perso>()!=null)
        {

            Niveau.instance.donnePerso.estDansGas = false;
            StopCoroutine(RetirerVieAuFilDuTemps());
            Niveau.instance.DesactiverEtatGaz();
        }
    }

    /// <summary>
    ///  Graduellement retire la vie du perso quand il est dans le gaz
    /// </summary>
    /// <returns></returns> <summary>
    private IEnumerator RetirerVieAuFilDuTemps()
    {
        while(Niveau.instance.donnePerso.estDansGas == true)
        {
            Niveau.instance.evenenment.perdreVie.Invoke();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
