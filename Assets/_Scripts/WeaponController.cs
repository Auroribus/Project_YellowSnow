using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public float fire_rate;
    public float damage;
    public float magic_cost;

    public GameObject projectile_prefab;

    public WeaponName weapon_name;
    public MagicType magic_type;

    public GameObject temp_explosion;
    
   [System.NonSerialized] public List<GameObject> Projectiles = new List<GameObject>();
    
    private LineRenderer line;

    private void Awake()
    {
        switch(weapon_name)
        {
            case WeaponName.Solarbeam:
                line = GetComponent<LineRenderer>();
                break;
        }
    }

    private void Update()
    {
        switch(weapon_name)
        {            
            case WeaponName.Solshard:
                CleanProjectilesList();
                break;
        }
    }

    public void Shoot(Transform spawn_transform)
    {
        switch(weapon_name)
        {
            case WeaponName.Solshard:
                Projectile(spawn_transform);        
                break;
            case WeaponName.Solarbeam:
                Laser(spawn_transform);
                break;
        }
    }

    private void CleanProjectilesList()
    {
        Projectiles.RemoveAll(item => item == null);
    }

    private void Projectile(Transform spawn_transform)
    {
        Projectiles.Add(Instantiate(projectile_prefab, spawn_transform.position, spawn_transform.rotation));
        Projectiles[Projectiles.Count - 1].GetComponent<ProjectileController>().projectile_damage = damage;
    }

    private void Laser(Transform spawn_transform)
    {
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;

        RaycastHit2D hit = Physics2D.Raycast(spawn_transform.position, dir, 20f);

        //If something was hit.
        if (hit.collider != null)
        {
            //Display the point in world space where the ray hit the collider's surface.
            Debug.Log(spawn_transform.position + " / " + hit.point);
            Instantiate(temp_explosion, hit.transform.position, Quaternion.identity);
            
            line.SetPosition(0, new Vector3(spawn_transform.position.x, spawn_transform.position.y, -10f));
            line.SetPosition(1, new Vector3(hit.transform.position.x, hit.transform.position.y, -10f));
        }
    }
}
