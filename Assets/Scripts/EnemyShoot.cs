using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private float timer;
    [SerializeField] private float shootdelay;
    [SerializeField] private bool isAiming;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform FirePoint;
    private Transform _player;

    void Start()
    {
        timer = shootdelay;
        _player = FindObjectOfType<TopDownCharacterController>().gameObject.transform;
    }
    void Update()
    {
        
        DelayShoot();
    }

    private void DelayShoot()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Shoot();
            timer = shootdelay;
        }
    }

    private void Shoot()
    {
        Quaternion bulletrotation = FirePoint.rotation;
        if (isAiming)
        {
            Vector2 direction = _player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bulletrotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        Instantiate(BulletPrefab, FirePoint.position, bulletrotation);
    }
}
