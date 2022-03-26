using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed;
    public int playerLife;
    private float _gravityModifier;
    private float _xBoundary = 5.8f;
    private Rigidbody _playerRb;
    private float _addGravity;
    private GameManager _manager;

    // Start is called before the first frame update
    void Start()
    {
        _gravityModifier = 7000f;
        _playerRb = GetComponent<Rigidbody>();
        //Physics.gravity *= gravityModifier;
        _manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _manager.playerLifeText.text = "XP: " + playerLife;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.instance.startGame == true)
        {
            Movement();
        }
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        _playerRb.velocity = (Vector3.right * speed * horizontalInput);
        
        if(transform.position.x >= _xBoundary)
        {
            transform.position = new Vector3(_xBoundary, transform.position.y,transform.position.z);
        }

        if(transform.position.x <= -_xBoundary)
        {
            transform.position = new Vector3(-_xBoundary, transform.position.y,transform.position.z);
        }
        _playerRb.AddForce((Vector3.down * _gravityModifier * Time.deltaTime), ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider other)
    {
        transform.position = _manager._reSpawnPos + new Vector3(0, 0.34f, 0);
        playerLife -=1;
        _manager.playerLifeText.text = "XP: " + playerLife;

        if(playerLife <= 0)
        {
            playerLife = 0;
            Destroy(gameObject);
            _manager.GameOver();
        }
    }
    
}
