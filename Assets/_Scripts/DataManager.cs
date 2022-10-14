using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public int bestScore;

    public static DataManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [System.Serializable]
    public class SaveData
    {
        public int m_bestScore;
    }

    public void SaveScore(int newBestScore)
    {
        SaveData data = new SaveData();
        data.m_bestScore = newBestScore;

        string json = JsonUtility.ToJson(data);

        string path = Application.persistentDataPath+"savefile.json";
        File.WriteAllText(path, json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.m_bestScore;
        }
    }
}
