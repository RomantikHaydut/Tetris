using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField, Range(0.1f, 2f)] public float gameSpeed;
    public float gameTime;
    [SerializeField] float diffultyTimer = 45f; // every this time game is getting more difficlut;

    public const int gridSizeX = 10;
    public const int gridSizeY = 20;

    public int GridSizeX = gridSizeX;
    public int GridSizeY = gridSizeY;

    public bool[,] Grid = new bool[gridSizeX, gridSizeY];

    #region Preview
    public bool isOpenTest;

    [SerializeField] SpriteRenderer displayShape;
    private SpriteRenderer[,] displayGrid = new SpriteRenderer[gridSizeX, gridSizeY];

    public List<ShapeController> objectList = new List<ShapeController>();

    public TMP_Text scoreText;
    public TMP_Text bestScoreText;
    public static int score;
    public int bestScore;

    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public TMP_Text gameSpeedAndTimeText; // Delete this.

    //Settings
    public int difficulty; // 0 easy , 1 medium , 2 hard
    public int resolation; // 0 -> 1920x1080 , 1-> 1440x900 , 2-> 1024x768


    public void DisplayPreview()
    {
        if (!isOpenTest)
        {
            return;
        }

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                var active = Grid[i, j];
                var sprite = displayGrid[i, j];

                sprite.color = active ? Color.red : Color.green;
            }
        }
    }

    #endregion

    private void Awake()
    {
        score = 0;
        gameTime = 0;
        DisplayScore(0);
        DisplayBestScore();
        Instance = this;

        if (isOpenTest)
        {
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    var sprite = Instantiate(displayShape, new Vector3(i, j, 0), Quaternion.identity,transform);
                    displayGrid[i, j] = sprite;

                }
            }

            DisplayPreview();
        }


        ResolationAndDifficulty();
        GameSpeed();
    }

    private void Start()
    {
        SoundManager.Instance.StartMainSoundtrack();
        gameSpeedAndTimeText.text = "Difficulty : " + (1 - gameSpeed) * 100 + "\n Next Difficulty : " + (45 - (int)gameTime); // Delete this.
    }

    private void Update()
    {
        if (SpawnManager.gameStarted)
        {
            // Increase game speed.
            gameSpeedAndTimeText.text = "Difficulty : " + (1 - gameSpeed)*100 + "\n Next Difficulty : " + (45 - (int)gameTime); // Delete this.
            gameTime += Time.deltaTime;
            if ((int)gameTime % diffultyTimer == 0 && gameTime >= diffultyTimer)
            {
                gameSpeed *= 0.7f;
                StartCoroutine(SoundManager.Instance.SpeedMainSoundTrack(0.15f));
                gameTime = 0;
            }
        }
    }

    public bool isInside(List<Vector2> coordinateList)
    {
        if (coordinateList == null)
        {
            return false;
        }
        foreach (var coordinate in coordinateList)
        {
            int x = Mathf.RoundToInt(coordinate.x);
            int y = Mathf.RoundToInt(coordinate.y);
            if (x < 0 || x >= gridSizeX)
            {
                // Horizontal out.
                return false;

            }

            if (y < 0 || y > gridSizeY)
            {
                // Vertical out.
                return false;
            }

            if (Grid[x, y])
            {
                // Hit something.
                if (y >= 15)
                {
                    if (!SpawnManager.gameOver)
                    {
                        GameOver();
                    }
                    
                }
                return false;
            }
        }

        return true;
    }

    public bool IsRowFull(int index)
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            if (!Grid[i,index])
            {
                return false;
            }
        }

        return true;
    }

    public void RemovePiecesController()
    {
        for (int i = 0; i < gridSizeY; i++)
        {
            var isFull = IsRowFull(i);
            if (isFull)
            {
                var willDestroy =new List<Transform>();
                
                foreach (var myBlock in objectList)
                {
                    foreach (var piece in myBlock.listPiece)
                    {
                        int y = Mathf.RoundToInt(piece.position.y);
                        if (y == i)
                        {
                            willDestroy.Add(piece);
                            DisplayScore(1);
                        }
                        else if (y > i)
                        {
                            var position = piece.position;
                            position.y--;
                            piece.position = position;
                        }
                    }

                    foreach (var item in willDestroy)
                    {
                        myBlock.listPiece.Remove(item);
                        Destroy(item.gameObject);
                        SoundManager.Instance.ExplosionSound();
                    }
                }

                // Change data.
                for (int j = 0; j < gridSizeX; j++)
                {
                    Grid[j, i] = false;
                }

                for (int j = i + 1; j < gridSizeY; j++)
                {
                    for (int k = 0; k < gridSizeX; k++)
                    {
                        Grid[k, j - 1] = Grid[k, j];
                    }
                }

                //Call again.
                RemovePiecesController();
                return;
            }
        }
    }

    public void DisplayScore(int amount)
    {

        score += amount;
        scoreText.text = "Score :" + score;
        if (score > bestScore)
        {
            bestScore = score;
            ScoreManager.Instance.BestScoreRecord(bestScore);
            DisplayBestScore();
        }
  
    }

    public void DisplayBestScore()
    {
        bestScoreText.text = "Best Score : " + ScoreManager.bestScore;
    }

    public void GameOver()
    {
        //Game Over...
        SoundManager.Instance.GameOverSound();
        gameOverPanel.SetActive(true);
        gameOverText.text= "Game Over... \nYour Score is :" + score;

        if (ScoreManager.bestScore > DataManager.Instance.bestScore)
        {
            DataManager.Instance.SaveScore(ScoreManager.bestScore);
        }
        
        SpawnManager.gameOver = true;
    }

    void ResolationAndDifficulty()
    {
        if (SettingManager.Instance.difficulty_Easy)
        {
            difficulty = 0;
        }
        else if (SettingManager.Instance.difficulty_Medium)
        {
            difficulty = 1;
        }
        else if (SettingManager.Instance.difficulty_Hard)
        {
            difficulty = 2;
        }

        if (SettingManager.Instance.resolation_1920)
        {
            resolation = 0;
        }
        else if (SettingManager.Instance.resolation_1440)
        {
            resolation = 1;
        }
        else if (SettingManager.Instance.resolation_1024)
        {
            resolation = 2;
        }

    }

    void GameSpeed()
    {
        if (difficulty==0)
        {
            gameSpeed = 1;
        }
        else if (difficulty ==1)
        {
            gameSpeed = 0.7f;
        }
        else if (difficulty==2)
        {
            gameSpeed = 0.3f;
        }
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }
    
}
