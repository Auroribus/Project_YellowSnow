using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public Text FPS;
    public Text Rooms;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        FPS = GameObject.Find("FPS").GetComponent<Text>();
        Rooms = GameObject.Find("Rooms").GetComponent<Text>();
    }

    float deltaTime, fps;

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        FPS.text = "FPS: " + fps.ToString("F0");
    }


}
