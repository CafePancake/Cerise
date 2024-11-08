using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
//#tp4 Carl
/// <summary>
/// SO qui gere la liste des scores et s<occupe de la lecture et ecriture du json pour les scores
/// </summary>
[CreateAssetMenu(menuName = "TIM/Sauvegarde", fileName = "Sauvegarde")]
public class SOSauvegarde : ScriptableObject
{
    [SerializeField] List<NomScore> _lesScores = new List<NomScore>(); //liste qui contient des variables NomScore (contient un string et un int)
    public List<NomScore> lesScores => _lesScores; //accesseur pour la liste
    NomScore _scoreCarl=new NomScore{nom="Carl", score=99};
    NomScore _scoreChristian=new NomScore{nom="Christian", score=99};
    NomScore _scorePascale=new NomScore{nom="Pascale", score=99};
    NomScore _scoreJonathan=new NomScore{nom="Jonathan", score=98};
    NomScore _scoreKathleen=new NomScore{nom="Kathleen", score=98};
    int _entreesMax = 7; //nombre de scores maximum dans la liste
    int _nbLignestableau = 6; //nombre de lignes max dans le tableau qui affichera les scores
    public int nbLignestableau => _nbLignestableau; //accesseur pour ^
    [SerializeField] string _fichier = "scores.tim"; //nom du fichier qui contiendra les scores

    [DllImport("__Internal")]
    private static extern void SynchroniserWebGL();

    /// <summary>
    /// Fonction qui s<ocuupe de lire le json et ecraser les donnes du so asset par ceux du json
    /// </summary>
    public void LireFichier()
    {
        string _fichierEtChemin = Application.persistentDataPath + "/" + _fichier; //automatiquement determine un emplacement pour le fichier ou il est possible d'écrire (permission du systeme)
        if (File.Exists(_fichierEtChemin)) //verifie que le fichier existe!
        {
            string contenu = File.ReadAllText(_fichierEtChemin);
            JsonUtility.FromJsonOverwrite(contenu, this); //ecrase contenu du so asset par contenu du json

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this); //pour que l'objet soit sauvegardé dans le so asset
            UnityEditor.AssetDatabase.SaveAssets();
#endif

            // Debug.Log(contenu);
            // Debug.Log(_lesScores);
        }
        else Debug.LogWarning("Le fichier n'existe pas");
        // Debug.Log(_fichierEtChemin);
    }

    /// <summary>
    /// fonnction qui ecrase les donnes du json par ceux du so asset
    /// </summary>
    public void EcrireFichier()
    {
        string _fichierEtChemin = Application.persistentDataPath + "/" + _fichier; //automatiquement determine un emplacement pour le fichier ou il est possible d'écrire (permission du systeme)
        string contenu = JsonUtility.ToJson(this); //true pour formater le json
        File.WriteAllText(_fichierEtChemin, contenu);

        if (Application.platform == RuntimePlatform.WebGLPlayer) //sauvegarde webgl si sur webgl
        {
            SynchroniserWebGL();
        }
    }

    /// <summary>
    /// ajoute un score dans al liste des scores avec le parametres recu
    /// </summary>
    /// <param name="nom">nom du joueur (est "" si nouveau score)</param>
    /// <param name="score">score du joueur</param>
    public void AjouterScore(NomScore score)
    {
        if (_lesScores.Count < _entreesMax) 
        {
           _lesScores.Add(score);
        }
        
    }

    /// <summary>
    /// classe les scores du plus grand au plus petit (Enleve aussi le plus petit de la liste (posisiton 6 dans liste, donc 7e position))
    /// </summary>
    public void ClasserScores()
    {
        _lesScores.Sort((score1, score2) => score2.score.CompareTo(score1.score));
        if (_lesScores.Count > _nbLignestableau) _lesScores.RemoveAt(_lesScores.Count - 1);
    }

    /// <summary>
    /// change le nom du score dans la liste par le nom inscrit par joueur
    /// </summary>
    /// <param name="pos">position de la ligne associe sur le tableau, qui normalement est aussi la pos dans la liste</param>
    /// <param name="nom">nom inscrit sur la ligne par le joueur</param>
    public void EcrireNom(int pos, string nom)
    {
        _lesScores[pos].nom = nom;
    }

    /// <summary>
    /// R/initialise la liste des scores ![ATTENTION REMPLLACE SCORES]!
    /// </summary>
    public void ReinitialiserScores()
    {
        _lesScores.Clear();
        _lesScores.AddRange(new List<NomScore>{_scoreCarl, _scoreChristian, _scorePascale, _scoreJonathan, _scoreKathleen});
    }
}