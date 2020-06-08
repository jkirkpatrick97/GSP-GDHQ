using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Missile : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float _moveSpeed = 1800f;
    private float _speed = 12;

    [SerializeField]
    private float _rotateSpeed = 12000f;

    private Rigidbody2D rb;

    private Transform enemy;

    private float _spawnTime;
    private GameObject[] _enemies;
   
    // Start is called before the first frame update
    void Start()
    {
        _spawnTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = GetClosestEnemy(_enemies);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && target.GetComponent<Enemy>().IsDead() == false)
        {
            rb.velocity = transform.up * _moveSpeed * Time.deltaTime;
            Vector3 targetVector = target.transform.position - transform.position;
            float rotatingIndex = Vector3.Cross(targetVector, transform.up).z;
            rb.angularVelocity = -1 * rotatingIndex * _rotateSpeed * Time.deltaTime;
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        if (Time.time - 1.5  > _spawnTime)
        {
            Destroy(this.gameObject);
        }
    }


    Transform GetClosestEnemy(GameObject[] _enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in _enemies)
        {
            if (potentialTarget.GetComponent<Enemy>().IsDead() == false)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }
        }

        return bestTarget;
    }
}
