using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
/// <summary>
/// classe qui s<occupe des informations dans la boutique (nomobre de joyaux)
/// </summary>
public class BoutiqueInfos : MonoBehaviour
{
    [Header("Les Scriptable Objects")]
    [SerializeField] SOEvents _evenements; //So qui contient des unityevents
    [SerializeField] SOPerso _doneesPerso; //SO qui contient des donnees par rapport au personnage


    [Header("Les champs de textes")]
    [SerializeField] TMP_Text _nbJoyaux1; //zone de texte qui affiche le nb de joyaux 1 (fleurs bleues)
    [SerializeField] TMP_Text _nbJoyaux2; //meme pour les joyaux 2 (vertes)

    void Start()
    {
        _evenements.actualiserInfosBoutique.AddListener(ActualiserInfos);
        ActualiserInfos();
        _doneesPerso.RenitialiserAffichage();
    }

    //affiche le nb de joyaux selon le nb dans _doneesPerso
    void ActualiserInfos()
    {
        _nbJoyaux1.text = _doneesPerso.nbJoyauxBleu.ToString();
        _nbJoyaux2.text = _doneesPerso.nbJoyauxVert.ToString();
    }

    void OnApplicationQuit()
    {
        _doneesPerso.Renitialiser();
    }
}
