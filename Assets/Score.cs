using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static Score instance;

    public float new_value;
    private float old_value;

    public float add_speed = .5f;

    Text score;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        score = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateScore();
	}

    private void UpdateScore()
    {
        if(old_value != new_value)
        {
            old_value += add_speed;
            score.text = old_value.ToString("00000");
        }
        else if(old_value >= new_value)
        {
            old_value = new_value;
            score.text = old_value.ToString("00000");
        }
    }
}
