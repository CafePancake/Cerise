//#tp5 Carl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Bonbons", menuName = "Bonbons")]
/// <summary>
/// #synthese Carl
/// so qui contient le nombre de bonbons qui ont des effets et les textes de descriptions d<effets des bonbons
/// </summary>
public class SOBonbons : ScriptableObject

{
    int _nbBonbonsBol = 10;
    public int nbBonbonsBol=>_nbBonbonsBol;

    string[] _texteEffets= new string[]
    {
    "C'est la fête dans la forêt magique!",
    "Il fait étrangement sombre... ",
    "Cerise se sent plus légère?",
    "Tout semble aller plus vite. ",
    "Les champignons envahissent la forêt.",
    "Les bruits de la forêt sont un peu plus étranges",
    "Cerise se sent perdue.",
    "Un gentil bonbon encourage Cerise depuis le fond de son estomac.",
    "Un méchant bonbon rapelle à Cerise qu'elle est une bonne à rien.",
    "Le Grand Vilain Loup est attiré par l'odeur de bonbons...",
    };
    public string[] texteEffets  { get => _texteEffets; set => _texteEffets = value;}
}