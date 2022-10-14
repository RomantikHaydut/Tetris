using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TMP_Text bestScoreText;
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SoundManager.Instance.StopMainSoundTrack();
        }

        DisplayMenuBestScore();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettingsScene()
    {
        SettingManager.Instance.OpenSettingsScene();
    }

    public void CloseSettingsScene()
    {
        SettingManager.Instance.CloseSettingsScene();
    }

    public void ChangeDifficulty()
    {
        SettingManager.Instance.ChangeDifficulty();
    }

    public void ChangeResolation()
    {
        SettingManager.Instance.ChangeResolation();
    }
    public void DisplayMenuBestScore()
    {
        bestScoreText.text = "YOUR BEST SCORE : " + ScoreManager.bestScore;
    }
}
