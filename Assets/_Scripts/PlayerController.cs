using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D r_body;

    public float input_speed;
    public float shield_rotation_speed;
    private float horizontal_input, vertical_input;

    private Transform shield, spawnpoint_projectiles; //shield_sprite

    public GameObject active_weapon;
    public GameObject weapon_prefab;
    private WeaponController weapon_control;
    public List<GameObject> Equiped_weapons = new List<GameObject>();

    public int PlayerScore = 0;

    public float Healthpoints = 10;

    public float solar_regenrate = 5;
    public float lunar_regenrate = 10;

    public float active_lunar = 100;
    public float active_solar = 100;
    public float max_lunar = 100;
    public float max_solar = 100;
    
    private float fire_counter = 0;

    private Text lunar_text, solar_text;
    private Text health_text;

	// Use this for initialization
	void Start () {
        r_body = GetComponent<Rigidbody2D>();
        shield = transform.Find("Shield");
        //shield_sprite = shield.Find("Sprite");
        spawnpoint_projectiles = shield.Find("Spawnpoint Projectiles");

        active_weapon = Instantiate(Equiped_weapons[0], transform.position,Quaternion.identity,transform);
        weapon_control = active_weapon.GetComponent<WeaponController>();

        lunar_text = GameController.instance.LunarMana;
        solar_text = GameController.instance.SolarMana;
        health_text = GameController.instance.Health;
        
	}
	
	// Update is called once per frame
	void Update () {

        if(Healthpoints <= 0)
        {
            GameController.instance.RestartGame();
        }
        
        horizontal_input = Input.GetAxis("Horizontal");
        vertical_input = Input.GetAxis("Vertical");

        r_body.velocity = new Vector2(horizontal_input, vertical_input) * input_speed;

        Attack();
	}

    private void FixedUpdate()
    {
        RotateShieldWithMouse();
        PlayerText();
        RegenerateMana();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (weapon_control.magic_cost < active_solar)
            {
                active_solar -= weapon_control.magic_cost;
                
                fire_counter = 0;
                weapon_control.Shoot(spawnpoint_projectiles);
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (weapon_control.magic_cost < active_solar)
            {
                fire_counter += Time.deltaTime;

                if (fire_counter > weapon_control.fire_rate)
                {
                    active_solar -= weapon_control.magic_cost;

                    fire_counter = 0;
                    weapon_control.Shoot(spawnpoint_projectiles);
                }
            }
        }
    }

    private void PlayerText()
    {
        lunar_text.text = "Lunar: " + Mathf.Floor(active_lunar) + "/" + max_lunar;
        solar_text.text = "Solar: " + Mathf.Floor(active_solar) + "/" + max_solar;
        health_text.text = "HP: " + Healthpoints;
    }

    private void RotateShieldWithMouse()
    {
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        shield.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private void RegenerateMana()
    {
        if (active_solar < max_solar && active_solar != 0)
        {
            active_solar += Time.deltaTime * solar_regenrate;
        }

        if(active_lunar < max_lunar && active_lunar != 0)
        {
            active_lunar += Time.deltaTime * lunar_regenrate;
        }
    }
}
