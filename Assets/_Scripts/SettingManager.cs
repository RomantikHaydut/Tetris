using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    public GameObject settingsPanel;

    public TMP_Dropdown resolation_DropDown;
    public TMP_Dropdown difficulty_DropDown;

    public  bool difficulty_Easy;
    public  bool difficulty_Medium;
    public  bool difficulty_Hard;

    public  bool resolation_1920;
    public  bool resolation_1440;
    public  bool resolation_1024;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            difficulty_Easy = true;
            resolation_1920 = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void ChangeDifficulty()
    {
        if (difficulty_DropDown.value == 0)
        {
            difficulty_Easy = true;
            difficulty_Medium = false;
            difficulty_Hard = false;
        }
        else if (difficulty_DropDown.value == 1)
        {
            difficulty_Easy = false;
            difficulty_Medium = true;
            difficulty_Hard = false;
        }
        else
        {
            difficulty_Easy = false;
            difficulty_Medium = false;
            difficulty_Hard = true;
        }

    }

    public void ChangeResolation()
    {
        if (resolation_DropDown.value == 0)
        {
            resolation_1920 = true;
            resolation_1440 = false;
            resolation_1024 = false;
            Screen.SetResolution(1920, 1080, true);
        }
        else if (resolation_DropDown.value == 1)
        {
            resolation_1920 = false;
            resolation_1440 = true;
            resolation_1024 = false;
            Screen.SetResolution(1440, 900, true);
        }
        else
        {
            resolation_1920 = false;
            resolation_1440 = false;
            resolation_1024 = true;
            Screen.SetResolution(1024, 768, true);
        }

    }

    public void OpenSettingsScene()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsScene()
    {
        settingsPanel.SetActive(false);
    }

}
