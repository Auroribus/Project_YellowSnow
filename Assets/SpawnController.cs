using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    [System.NonSerialized] public int amount_of_enemies;
    public List<GameObject> enemy_types_small = new List<GameObject>();
    public List<GameObject> enemy_types_medium = new List<GameObject>();
    public List<GameObject> enemy_types_boss = new List<GameObject>();

    [System.NonSerialized] public List<Vector2> spawn_points = new List<Vector2>();
    [System.NonSerialized] public List<GameObject> active_enemies = new List<GameObject>();

    public bool has_spawned = false;

    private RoomCreator rc;

    private void Awake()
    {
        rc = GetComponent<RoomCreator>();
    }

    void Update () {
		if(active_enemies.Count > 0)
        {
            CleanupList();
        }
        else if(active_enemies.Count == 0 && has_spawned)
        {
            //no more enemies in room
            if (!rc.room_cleared)
            {
                rc.room_cleared = true;

                Invoke("CollectAllScoreDrops", 1f);
            }
        }
	}

    public void SpawnEnemies()
    {
        has_spawned = true;

        for(int amount = 0; amount < amount_of_enemies; amount++)
        {
            active_enemies.Add(Instantiate(enemy_types_small[0], spawn_points[amount], Quaternion.identity));
        }
    }

    private void CleanupList()
    {
        active_enemies.RemoveAll(item => item == null);
    }

    private void CollectAllScoreDrops()
    {
        GameObject[] score_drops = GameObject.FindGameObjectsWithTag("ScoreDrop");
        foreach (GameObject g in score_drops)
        {
            g.GetComponent<ScoreDrop>().position_set = true;
        }
    }
}
