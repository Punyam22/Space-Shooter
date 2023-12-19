using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if(_anim == null){
            Debug.Log("Animator is NULL");
        }
        if(_audioSource == null){
            Debug.Log("Audio Source is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        float xPos = Random.Range(-9.0f, 9.0f);
        if(transform.position.y <= -5.0f){
            transform.position = new Vector3(xPos, 7.5f, transform.position.z);  
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            Player player = other.transform.GetComponent<Player>();
            if(player != null){
                player.Damage();
            } 
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
        if(other.tag == "Lazer"){
            Destroy(other.gameObject);
            if(_player != null){
                _player.AddScore();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
    }

}
