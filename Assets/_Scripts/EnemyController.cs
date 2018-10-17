using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float Healthpoints = 1;
    public float score_value = 10;

    public GameObject enemy_explosion;

    public GameObject Projectile_prefab;    
    public GameObject ScoreDrop_prefab;
    public GameObject EnemyExplosion_prefab;

    public float fire_rate = 1f;
    private float fire_count = 0f;

    List<GameObject> Projectiles = new List<GameObject>();

    private GameObject Player;
    private Transform projectile_spawn;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        projectile_spawn = transform.Find("Spawnpoint Projectiles");
    }

    void Update () {
		if(Healthpoints <= 0)
        {
            DestroyEnemy();
        }
        else
        {
            LookAtPlayer();
            Shoot();
        }
	}

    private void DestroyEnemy()
    {
        float amount_score_drops = score_value / ScoreDrop_prefab.GetComponent<ScoreDrop>().score_value;

        for(int i = 0; i < amount_score_drops; i++)
        {
            Instantiate(ScoreDrop_prefab, transform.position, Quaternion.identity);
        }

        //instance particles
        Instantiate(EnemyExplosion_prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Shoot()
    {
        fire_count += Time.deltaTime;

        if(fire_count >= fire_rate)
        {
            fire_count = 0;
            Projectiles.Add(Instantiate(Projectile_prefab, projectile_spawn.position, transform.rotation));
        }
    }

    private void LookAtPlayer()
    {
        Vector3 dir = Player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }
}
