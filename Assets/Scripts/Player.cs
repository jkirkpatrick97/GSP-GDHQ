using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, 1.05f, 0);
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private float _damageCoolDown = 0.5f;
    [SerializeField]
    private float _thusterBoostMultiplier = 1.6f;
    [SerializeField]
    private int _ammoCount = 15;


    private float lastDamageTime = 0;
    private int _shieldStrength = 0;

    private SpawnManager _spawnManager;

    private bool _tripleshotEnabled = false;
        
    [SerializeField]
    private int _score;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioClip _noAmmoSoundClip;
    
    private AudioSource _audioSource;

   

    // Start is called before the first frame update
    void Start()
    {
      

        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source om the Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

       

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= _thusterBoostMultiplier;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed /= _thusterBoostMultiplier;
        }
                
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
        }

        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        //transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        /*
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, transform.position.z);
        }
        */
    }

    void FireLaser()
    {            
        _canFire = Time.time + _fireRate;
        if (_tripleshotEnabled == true && _ammoCount > 2)
        {
            Instantiate(_tripleshotPrefab, transform.localPosition + _laserOffset, Quaternion.identity);
            _ammoCount -= 3;
            _uiManager.UpdateAmmo(_ammoCount);
            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();

        }
        else if (_ammoCount > 0)
        {
            Instantiate(_laserPrefab, transform.localPosition + _laserOffset, Quaternion.identity);
            _ammoCount -= 1;
            _uiManager.UpdateAmmo(_ammoCount);
            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = _noAmmoSoundClip;
            _audioSource.Play();
        }
        
    }

    public void Damage()
    {
        if (lastDamageTime <= Time.time - _damageCoolDown)
        {
            if (_shieldStrength > 0)
            {
                _shieldStrength -= 1;
                if(_shieldStrength == 2)
                {
                    _playerShield.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                else
                {
                    _playerShield.GetComponent<SpriteRenderer>().color = Color.red;
                }
                

                if(_shieldStrength < 1)
                {
                    _playerShield.SetActive(false);
                }
                return;
            }

            _lives -= 1;
            _audioSource.clip = _explosionSoundClip;
            _audioSource.Play();
            if (_lives == 2)
            {
                _rightEngine.SetActive(true);
            }
            if (_lives == 1)
            {
                _leftEngine.SetActive(true);
            }

            _uiManager.UpdateLives(_lives);

            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();

                Destroy(this.gameObject);

            }

            lastDamageTime = Time.time;
        }
    }

    
    public void TripleShotActive()
    {
        _tripleshotEnabled = true;
        _ammoCount += 30;
        _uiManager.UpdateAmmo(_ammoCount);
        StartCoroutine(TrippleShotPowerDownRoutine());
    }

    IEnumerator TrippleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleshotEnabled = false;
    }

    public void SpeedBoostActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDown());
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _playerShield.GetComponent<SpriteRenderer>().color = Color.white;
        _playerShield.SetActive(true);
        _shieldStrength = 3;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void IncreaseAmmo()
    {
        _ammoCount += 15;
        _uiManager.UpdateAmmo(15);
    }

    
}
