using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameSettings gameSettings;

    [HideInInspector]
    public Vector3 screenPosition;

    [SerializeField]
    private Transform rings;
    private Transform player;

    private AudioSource gameAudio;

    [SerializeField]
    private AudioClip createAudio;
    [SerializeField]
    private AudioClip scoreAudio;
    [SerializeField]
    private AudioClip deathAudio;

    [HideInInspector]
    public int ringsCount = 0;

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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameAudio = this.GetComponent<AudioSource>();
        player.position = new Vector3(0, -screenPosition.y + 0.64f, 0);

        createRingsIntervalTimer = gameSettings.createTimer;
        currentCreateRingsIntervalTimer = createRingsIntervalTimer;
        createRingsIntervalTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {

        gameSettings.gameSpeed += Time.deltaTime / 30;

        createRingsIntervalTimer -= gameSettings.gameSpeed * Time.deltaTime;
        if (createRingsIntervalTimer <= 0) {
            Instantiate(rings, new Vector3(0, screenPosition.y + 0.32f, 0), Quaternion.identity);
            gameAudio.clip = createAudio;
            gameAudio.Play();
            createRingsIntervalTimer = currentCreateRingsIntervalTimer;
        }
    }
}
