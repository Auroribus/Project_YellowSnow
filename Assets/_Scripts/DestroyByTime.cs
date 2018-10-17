using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    public float destroy_after_seconds;

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyAfterSeconds());
	}
	
	private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(destroy_after_seconds);

        Destroy(gameObject);
    }
}
