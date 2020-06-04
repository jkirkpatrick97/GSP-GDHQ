using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _enemyLaserSpeed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -1, 0) * _enemyLaserSpeed * Time.deltaTime);

        if (transform.position.y < -5f)
        {

            Destroy(transform.parent.gameObject);
            //Destroy(this.gameObject);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.Damage();
            Destroy(transform.parent.gameObject);
            //Destroy(this.gameObject);

        }
    }
}
