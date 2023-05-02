using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 0;
    private Vector3 _moveDirection;
    private BombManager _bomb;
    [SerializeField] private GameObject _pausePanel = null;
    private CharacterController _controller;
    private Animator anim;
    private bool _isRunnning = true;

    private GameManager _gameManager;
    private void Start()
    {
        _bomb = FindObjectOfType<BombManager>();
        
        anim = GetComponentInChildren<Animator>();
        _gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        _controller = GetComponent<CharacterController>();
        
        if (!_gameManager.isTuto)
        {
            if(_pausePanel != null)
                _pausePanel.SetActive(false);
            Respawn();
        }
    }
    
    
    public void Respawn()
    {
        if (_gameManager.isTuto)
            return;
        
        Vector3 position = _gameManager.GetNewSpawnPoint();
        position = new Vector3(position.x, transform.position.y, position.z);
        
        _controller.enabled = false;
        transform.position = new Vector3(position.x, transform.position.y, position.z);
        _moveDirection = position;

        _bomb._exploded = false;
        _controller.enabled = true;
    }
    
    private void PauseGame()
    {
        _isRunnning = false;
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
       
    }
    
    public void ContinueGame()
    {
        _isRunnning = true;
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_bomb != null && !_bomb._exploded)
        {
            if (!_gameManager.inMenu)
            {
                Move();
            }
                
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && !_gameManager.isTuto && (!_gameManager.inMenu || (_gameManager.inMenu && !_isRunnning)))
        {
            if (_isRunnning) 
            {
                PauseGame();
                _gameManager.inMenu = true;
            }
            else 
            {
                ContinueGame();
                _gameManager.inMenu = false;
            } 
            
        }
        
    }


    private void Idle()
    {
       anim.SetFloat("Blend",0);
    }
    
    private void Run()
    {
       anim.SetFloat("Blend",1);
    }

    private void Move()
    {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");


        _moveDirection = new Vector3(moveX, 0, moveZ);
        _moveDirection = Vector3.ClampMagnitude(_moveDirection, 1f);
        _moveDirection *= walkSpeed;
        
        _controller.Move(_moveDirection * Time.deltaTime);
        if (_moveDirection != Vector3.zero)
        {
            Run();
            transform.forward = _moveDirection;
        }
        else
        {
            Idle();
        }
        
    }


    public void StopMovement()
    {
        _controller.velocity.Set(0,0,0);
        Idle();
    }
}
