using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#region enums

public enum RoomType
{
    start,
    general
}

public enum MagicType
{
    Solar,
    Lunar
}

public enum WeaponName
{
    Solshard,
    Solarbeam
}

public enum ProjectileType
{
    one,
    two,
    three
}

public enum PortalType
{
    crossroad,
    boss,
    treasure
}

#endregion

public class GameController : MonoBehaviour {

    public static GameController instance;

    public Text FPS;
    public Text Rooms;
    public Text Projectiles;
    public Text SolarMana;
    public Text LunarMana;
    public Text Health;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        FPS = GameObject.Find("FPS").GetComponent<Text>();
        Rooms = GameObject.Find("Rooms").GetComponent<Text>();
        SolarMana = GameObject.Find("Solar").GetComponent<Text>();
        LunarMana = GameObject.Find("Lunar").GetComponent<Text>();
        Health = GameObject.Find("Health").GetComponent<Text>();
    }

    float deltaTime, fps;

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        FPS.text = "FPS: " + fps.ToString("F0");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
