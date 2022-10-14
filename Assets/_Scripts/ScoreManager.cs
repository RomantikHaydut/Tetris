using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public static int bestScore;
    public TMP_Text bestScoreText;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DataManager.Instance.LoadScore();
            bestScore = DataManager.Instance.bestScore;
           // DisplayMenuBestScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*public void DisplayMenuBestScore()
    {
        bestScoreText.text = "YOUR BEST SCORE : " + bestScore;
    }
    */


    public void BestScoreRecord(int newBestScore)
    {
        if (newBestScore > bestScore)
        {
            bestScore = newBestScore;
        }
    }

    private void OnApplicationQuit()
    {
        if (bestScore > DataManager.Instance.bestScore)
        {
            DataManager.Instance.SaveScore(bestScore);
        }
    }
}
