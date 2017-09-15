using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameSettings gameSettings;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text levelText;

    //[SerializeField]
    //private Text highScoreText;

    public enum GameState {
        Start,
        InProgress,
        Dead,
        Restarting
    }

    public GameState gameState;

    [HideInInspector]
    public bool isScored = false;

    [HideInInspector]
    public bool isDead = false;

    [HideInInspector]
    public Vector3 screenPosition;

    [SerializeField]
    private Transform rings;
    private Transform player;

    private float currentGameSpeed;

    private AudioSource gameAudio;

    [SerializeField]
    private AudioClip createAudio;
    [SerializeField]
    private AudioClip scoreAudio;
    [SerializeField]
    private AudioClip deathAudio;

    [HideInInspector]
    public int ringsCount = 0;

    private float score = 0;

    private float createRingsIntervalTimer = 0;
    private float currentCreateRingsIntervalTimer = 0;

    private void Awake() {
        instance = this;
        screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        if (rings == null) {
            Debug.LogError("No rings reference assigned!");
        }

        ringsCount = Mathf.RoundToInt(Mathf.Abs(screenPosition.x * 2) / 0.64f);
        if (ringsCount % 2 == 1) {
            ringsCount--;
        }
    }

    // Use this for initialization
    void Start () {
        currentGameSpeed = gameSettings.gameSpeed;
        gameState = GameState.Start;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameAudio = this.GetComponent<AudioSource>();
        player.position = new Vector3(0, -screenPosition.y + 0.64f, 0);

        createRingsIntervalTimer = gameSettings.createTimer;
        currentCreateRingsIntervalTimer = createRingsIntervalTimer;
        createRingsIntervalTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        switch (gameState) {
            case GameState.Start:
                gameSettings.gameSpeed = currentGameSpeed;
                gameState = GameState.InProgress;
                break;
            case GameState.InProgress:
                if(isScored == true) {
                    gameAudio.clip = scoreAudio;
                    gameAudio.Play();
                    gameSettings.gameSpeed += 0.2f;
                    score++;
                    isScored = false;
                }
                scoreText.text = (score * Mathf.RoundToInt(gameSettings.gameSpeed)).ToString();
                levelText.text = Mathf.RoundToInt(gameSettings.gameSpeed).ToString();
                /*if (score >= GameData.highScore) {
                    GameData.highScore = Mathf.RoundToInt(score);
                    highScoreText.text = GameData.highScore.ToString();
                }*/
                break;
            case GameState.Dead:
                gameAudio.clip = deathAudio;
                gameAudio.Play();
                gameState = GameState.Restarting;
                
                StartCoroutine(RestartGame(gameSettings.restartTimer));
                break;
            case GameState.Restarting:
                return;
        } 

        createRingsIntervalTimer -= gameSettings.gameSpeed * Time.deltaTime;
        if (createRingsIntervalTimer <= 0) {
            Instantiate(rings, new Vector3(0, screenPosition.y + 0.32f, 0), Quaternion.identity);
            gameAudio.clip = createAudio;
            gameAudio.Play();
            createRingsIntervalTimer = currentCreateRingsIntervalTimer;
        }
    }

    IEnumerator RestartGame(float _second) {
        yield return new WaitForSeconds(_second);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
