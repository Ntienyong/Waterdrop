using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject[] platforms;
    public GameObject player;
    public Transform platformParent;
    private Vector3 _playerStartPos;
    private Vector3 _spawnPos;
    private Vector3 _randomStartPos;
    public Vector3 _reSpawnPos;
    public static GameManager instance = null;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerLifeText;
    public TextMeshProUGUI gameoverText;
    public TextMeshProUGUI countDown;
    public Button restartButton;
    public PlayerController _controlPlayer;
    private float _startSpawnTime = 1.0f;
    private float _contSpawnTime = 1.0f;
    private float _yStart = 4.5f;
    private float _xSpawnPosRange = 5.3f;
    private float _ySpawnPos = 5.5f;
    public int _countDown = 4;
    public bool startGame;
    public bool gameover;
    public bool gameActive;
    public int score;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        gameActive = true;
        StartCoroutine(StartGametimer());
        StartGameMechanics();
    }

    // Update is called once per frame
    void Update()
    {
        _reSpawnPos = platformParent.GetChild(2).transform.position;
        
        PauseGame();
    }

    void StartGameMechanics()
    {
        for (int i = 0; i < 4; i++)
        {
            _randomStartPos = new Vector3(Random.Range(-5.3f, 5.3f), _yStart, 0);
            Instantiate(platforms[0], _randomStartPos, transform.rotation, platformParent);
            _yStart -= 2.0f;
            if (i == 2)
            {
                _playerStartPos = _randomStartPos + Vector3.up;

            }

        }
        InvokeRepeating("SpawnPlatforms", _startSpawnTime, _contSpawnTime);
        StartCoroutine("SpawnBadPlatforms");
        Instantiate(player, _playerStartPos, transform.rotation);
        Debug.Log("Player spawn");
    }

    void SpawnPlatforms()
    {
        if (startGame == true)
        {
            _spawnPos = new Vector3(Random.Range(-_xSpawnPosRange, _xSpawnPosRange), -_ySpawnPos, 0);

            Instantiate(platforms[0], _spawnPos, platforms[0].transform.rotation, platformParent);
        }
    }

    public IEnumerator SpawnBadPlatforms()
    {
        _spawnPos = new Vector3(Random.Range(-_xSpawnPosRange, _xSpawnPosRange), -_ySpawnPos, 0);
        Instantiate(platforms[1], _spawnPos, platforms[1].transform.rotation);
        yield return new WaitForSeconds(Random.Range(2.0f, 15.0f));
        StartCoroutine("SpawnBadPlatforms");
    }

    public IEnumerator StartGametimer()
    {
        while (_countDown > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _countDown--;
            countDown.text = " " + _countDown + " ";
            if (_countDown == 0)
            {
                countDown.gameObject.SetActive(false);
                startGame = true;
                StartCoroutine(CountScore());
            }
        }
    }

    public IEnumerator CountScore()
    {
        yield return new WaitForSeconds(0.05f);
        score++;
        scoreText.text = "Score: " + score;

        if(score > 1000 && !gameover)
        {
            Time.timeScale = 1.2f;
        }

        if(score > 2000 && !gameover)
        {
            Time.timeScale = 1.5f;
        }

        if(score > 3000 && !gameover)
        {
            Time.timeScale = 2.0f;
        }
        StartCoroutine(CountScore());
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameoverText.gameObject.SetActive(true);
        gameover = true;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Waterdrop");
        // Application.LoadLevel("Waterdrop");
    }

    void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.Space) && gameActive)
        {
            Debug.Log("Paused game");
            Time.timeScale = 0;
            gameActive = false;
        }

        else if(Input.GetKeyDown(KeyCode.Space) && !gameActive)
        {
            Debug.Log("Unpaused game");
            Time.timeScale = 1;
            gameActive = true;
        }
    }
}
