using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public bool isTuto = false;
    public bool isPossibleToExplodeInTuto = false;
    public bool inMenu = false;

    [SerializeField] private TextMeshProUGUI _chrono;
    [SerializeField] private TextMeshProUGUI _countRespawn;
    [SerializeField] private TextMeshProUGUI _scoreDirect;
    [SerializeField] private TextMeshProUGUI _scoreLoseScreen;
    [SerializeField] private TextMeshProUGUI _highScore;
    [SerializeField] private int _timeExplosionModel = 60;
    private int _timeBeforeExplosion;
    [SerializeField] private int _timeRespawnModel = 3;
    private int _timeBeforeRespawn;
    private int _timeBeforeMainMenu = 3;
    [SerializeField] private GameObject _win = null;
    [SerializeField] private GameObject _loseGO = null;
    [SerializeField] private GameObject _chronoGO = null;
    
    
    [SerializeField] private List<GameObject> SpawnPoints;
    private GameObject lastSpawnPoint = null;

    public int score = 0;
    public int highscore;

    private BombManager _bomb;
    private PlayerController _player;

    private bool _secondDone = true;
    private bool _winActivate = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _bomb = FindObjectOfType<BombManager>();

        _win.SetActive(false);
        
        if (!isTuto)
        {
            _loseGO.SetActive(false);
            _chronoGO.SetActive(true);
        
            highscore = PlayerPrefs.GetInt("Highscore");
            _highScore.text = highscore.ToString();
            _timeBeforeExplosion = _timeExplosionModel;
            _timeBeforeRespawn = _timeRespawnModel;
        
            _chrono.text = _timeBeforeExplosion.ToString();
            _countRespawn.text = _timeBeforeRespawn.ToString();
        }
    }
    
    // Update is called once per frame
    void Update()
    {

        if (isTuto)
        {
            if (_timeBeforeMainMenu == 0)
            {
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
            else
            {
                if (_secondDone)
                {
                    if (_winActivate && isPossibleToExplodeInTuto)
                    {
                        CountBeforeMainMenu();
                    }
                }
            }
            
            return;   
        }

        if (_secondDone)
        {
            Choronometre();
            if (_winActivate  && _timeBeforeRespawn >= 0)
            {
                CountBeforeRespawn();
            }
        }
        
        if (_timeBeforeExplosion == 0)
        {
            _bomb.Explosion();
            _timeBeforeExplosion = -1;
        }
        
        if (_timeBeforeRespawn == 0)
        {
            _player.Respawn();

            score++;

            _win.SetActive(false);
            _winActivate = false;
            _chronoGO.SetActive(true);


            _timeBeforeExplosion = _timeExplosionModel;
            _timeBeforeRespawn = _timeRespawnModel;

            _chrono.text = _timeBeforeExplosion.ToString();
            _countRespawn.text = _timeBeforeRespawn.ToString();
            _scoreDirect.text = score.ToString();
            _timeBeforeRespawn = _timeRespawnModel;


            inMenu = false;
        }
    }

    public void Win()
    {
        _win.SetActive(true);
        _winActivate = true;
        _chronoGO.SetActive(false);

        _player.StopMovement();

        inMenu = true;
    }

    public void Lose()
    {

        _scoreLoseScreen.text = score.ToString();
        _win.SetActive(false);
        _loseGO.SetActive(true);
        _chronoGO.SetActive(false);
        
        inMenu = true;

        if (score > highscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            _highScore.text = score.ToString();
        }
}

    public void EndTuto()
    {
        _win.SetActive(true);
        _winActivate = true;
        isPossibleToExplodeInTuto = true;
    }

    void Choronometre()
    {
        if (_timeBeforeExplosion < 10)
        {
            _chrono.text = "0"+_timeBeforeExplosion;
        }
        else
        {
            _chrono.text = _timeBeforeExplosion.ToString();
        }
        
        
        StartCoroutine(CountChrono());
    }
    
    void CountBeforeRespawn()
    {
        _countRespawn.text = _timeBeforeRespawn.ToString();
        
        StartCoroutine(CountRespawn());
    }
    
    void CountBeforeMainMenu()
    {
        StartCoroutine(CountMainMenu());
    }

    IEnumerator CountChrono()
    {
        _secondDone = false;
        yield return new WaitForSeconds(1);
        _timeBeforeExplosion -= 1;
        _secondDone = true;
    }
    
    IEnumerator CountRespawn()
    {
        _secondDone = false;
        yield return new WaitForSeconds(1);
        _timeBeforeRespawn -= 1;
        _secondDone = true;
    }
    
    IEnumerator CountMainMenu()
    {
        _secondDone = false;
        yield return new WaitForSeconds(1);
        _timeBeforeMainMenu -= 1;
        _secondDone = true;
    }
    

    public Vector3 GetNewSpawnPoint()
    {
        GameObject spawnPoint;

        if (lastSpawnPoint != null)
        {
            IEnumerable<GameObject> temp = SpawnPoints.Where(e => e != lastSpawnPoint);

            spawnPoint = temp.ElementAt(Random.Range(0, temp.Count()));
        }
        else
        {
            spawnPoint = SpawnPoints.ElementAt(Random.Range(0, SpawnPoints.Count()));
        }
        
        lastSpawnPoint = spawnPoint;

        return spawnPoint.transform.position;
    } 
}
