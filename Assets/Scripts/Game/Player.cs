using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _horizontalInput;
    private float _verticalInput;
    [SerializeField]
    private GameObject _lazerPrefab;
    [SerializeField]
    private float _firerate = 0.5f;
    private float _canFire = 0.0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager spawnManager;
    private bool _isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject[] _engine;
    [SerializeField]
    private GameObject _expolsionPrefab;
    [SerializeField]
    private AudioClip _lazerSoundClip;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -3, 0);  
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>(); 
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if(_uiManager == null){
            Debug.Log("UIManager is null");
        }
        if(_audioSource != null){
            _audioSource.clip = _lazerSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
       CalculateMovement();
       FireControl();
    }

    void CalculateMovement()
    {
         _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * _speed * Time.deltaTime * _horizontalInput);
        transform.Translate(Vector3.up * _speed * Time.deltaTime * _verticalInput);
        if(transform.position.y >= 0.0f){
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
        else if(transform.position.y <= -4.0f){
            transform.position = new Vector3(transform.position.x, -4.0f, transform.position.z);
        }
        if(transform.position.x >= 11.0f){
            transform.position = new Vector3(-11.0f, transform.position.y, transform.position.z);
        }
        else if(transform.position.x <= -11.0f){
            transform.position = new Vector3(11.0f, transform.position.y, transform.position.z);
        }
    }

    void FireControl()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);
         Vector3 pos2 = new Vector3(transform.position.x - 0.85f, transform.position.y, transform.position.z);
       if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire){
            _canFire = Time.time + _firerate;
            if(_isTripleShotActive == true){
                Instantiate(_tripleShotPrefab, pos2, Quaternion.identity);  
            }
            else{
                Instantiate(_lazerPrefab, pos, Quaternion.identity);
            }
            _audioSource.Play();
       }
    }

    public void Damage()
    {
        if(_isShieldActive == true){
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _lives--;
        if(_lives == 2){
            _engine[0].SetActive(true);
        }
        if(_lives == 1){
            _engine[1].SetActive(true);
        }
        _uiManager.UpdateLives(_lives);
        if(_lives == 0){
            Destroy(this.gameObject);
            if(spawnManager != null){
                spawnManager.StopSpawning();
            }
            Instantiate(_expolsionPrefab, transform.position, Quaternion.identity);
            _uiManager.GameOverText();
        }
    }

    public void IsTripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoost()
    {
        _speed = 10.0f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 5.0f;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }
}
