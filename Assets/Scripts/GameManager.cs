using Assets.Scripts;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject HUD;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text highestScoreText;
    [SerializeField] private AudioManager audioManager;

    private int score;
    private int lives;
    private int highestScore;
    public int Score => score;
    public int Lives => lives;
    public static bool gameIsPaused;
    //%userprofile%\AppData\LocalLow\<companyname>\<productname>
    //Example:C:\Users\jtkjm\AppData\LocalLow\PRU212\Asteroids
    private string filePath;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        filePath = Path.Combine(Application.persistentDataPath, "HighestScore.json");
        LoadHighestScore();
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        HUD.SetActive(false);
        mainMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return)) {
            NewGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    public void NewGame()
    {
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

        for (int i = 0; i < asteroids.Length; i++) {
            Destroy(asteroids[i].gameObject);
        }

        gameOverUI.SetActive(false);
        pauseMenu.SetActive(false);

        SetScore(0);
        SetLives(3);
        Respawn();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        if (score > highestScore)
        {
            highestScore = score;
            highestScoreText.text = highestScore.ToString();
            SaveHighestScore();
        }
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }

    public void OnAsteroidDestroyed(Asteroid asteroid)
    {
        explosionEffect.transform.position = asteroid.transform.position;
        explosionEffect.Play();
        audioManager.PlaySFX(audioManager.sfxExplode);

        if (asteroid.size < 0.7f) {
            SetScore(score + 100); // small asteroid
        } else if (asteroid.size < 1.4f) {
            SetScore(score + 50); // medium asteroid
        } else {
            SetScore(score + 25); // large asteroid
        }
    }

    public void OnPlayerDeath(Player player)
    {
        player.gameObject.SetActive(false);

        explosionEffect.transform.position = player.transform.position;
        explosionEffect.Play();
        audioManager.PlaySFX(audioManager.sfxExplode);

        SetLives(lives - 1);

        if (lives <= 0) {
            gameOverUI.SetActive(true);
        } else {
            Invoke(nameof(Respawn), player.respawnDelay);
        }
    }
    public void OnStarCollected()
    {
        audioManager.PlaySFX(audioManager.sfxStarCollect);
        SetScore(score * 2);

    }
    public void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            pauseMenu.SetActive(false);
        }
    }
    public void OnRestartButtonClicked()
    {
        if (lives <= 0)
        {
            NewGame();
        }
    }
    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    public void OnResumeButtonClicked()
    {
        gameIsPaused = !gameIsPaused;
        PauseGame();
    }
    public void OnPlayButtonClicked()
    {
        mainMenu.SetActive(false); 
        gameIsPaused = false;      
        Time.timeScale = 1f;       
        AudioListener.pause = false;
        HUD.SetActive(true);
        NewGame();                 
    }
    private void SaveHighestScore()
    {
        GameData data = new GameData();
        data.HighestScore = highestScore;

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    private void LoadHighestScore()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            GameData data = JsonUtility.FromJson<GameData>(jsonData);
            highestScore = data.HighestScore;
        }
        else
        {
            highestScore = 0;
            SaveHighestScore(); // Create the file with initial data
        }
        highestScoreText.text = highestScore.ToString();
    }

}
