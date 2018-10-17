using UnityEngine;

public class ProjectileController : MonoBehaviour {
    
    public float projectile_speed;
    public float projectile_damage;

    public ProjectileType projectile_type = ProjectileType.one;

    public GameObject projectile_explosion;
    public GameObject projectile_muzzle;

    public float knockback_force;

    private void Start()
    {
        switch(projectile_type)
        {
            case ProjectileType.one:
                Instantiate(projectile_muzzle, transform.position, Quaternion.identity);
                break;
        }
    }

    private void Update()
    {
        switch(projectile_type)
        {
            case ProjectileType.one:
                transform.Translate(Vector3.up * projectile_speed * Time.deltaTime);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (projectile_type)
        {
            case ProjectileType.one:
                if (collision.tag == "Wall")
                {
                    DestroyProjectile();
                }
                else if (collision.tag == "Enemy" && transform.tag != "ProjectileEnemy")
                {
                    collision.transform.GetComponent<EnemyController>().Healthpoints -= projectile_damage;
                    DestroyProjectile();
                }
                else if(collision.tag == "Shield" && transform.tag != "Projectile")
                {
                    //knockback attempt
                    //Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();

                    //Vector3 moveDirection = rb.transform.position - transform.position;
                    //collision.GetComponentInParent<Rigidbody2D>().AddForce(moveDirection.normalized * knockback_force);

                    DestroyProjectile();
                }
                else if(collision.tag == "Player" && transform.tag != "Projectile")
                {
                    //knockback attempt
                    //Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();

                    //Vector3 moveDirection = rb.transform.position - transform.position;
                    //collision.GetComponent<Rigidbody2D>().AddForce(moveDirection.normalized * knockback_force, ForceMode2D.Impulse);

                    collision.transform.GetComponent<PlayerController>().Healthpoints -= projectile_damage;
                    DestroyProjectile();
                }
                break;
        }
        
    }

    private void DestroyProjectile()
    {
        Instantiate(projectile_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
