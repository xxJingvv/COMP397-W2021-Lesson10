using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    public RectTransform rectTransform;

    public Vector2 offScreenPosition;
    public Vector2 onScreenPosition;

    [Range(0.1f, 10.0f)] 
    public float speed = 1.0f;
    public float timer = 0.0f;
    public bool isOnScreen = false;

    [Header("Player Settings")]
    public PlayerBehaviour player;
    public CameraController playerCamera;

    public Pauseable pausable;

    [Header("Scene Data")]
    public SceneDataSO sceneData;

    public GameObject gameStateLabel;

    // Start is called before the first frame update
    void Start()
    {
        pausable = FindObjectOfType<Pauseable>();
        player = FindObjectOfType<PlayerBehaviour>();
        playerCamera = FindObjectOfType<CameraController>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = offScreenPosition;
        timer = 0.0f;
        
        LoadFromPlayerPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Tab))
        // {
        //     ToggleControlPanel();
        // }

        if (isOnScreen)
        {
            MoveControlPanelDown();
        }
        else
        {
            MoveControlPanelUp();
        }

        gameStateLabel.SetActive(pausable.isGamePaused);
    }

    void ToggleControlPanel() {
        isOnScreen = !isOnScreen;
        timer = 0.0f;

        if (isOnScreen)
        {
            //Cursor.lockState = CursorLockMode.None;
            playerCamera.enabled = false;
        }
        else
        {
            //Cursor.lockState = CursorLockMode.Locked;
            playerCamera.enabled = true;
        }
    }

    private void MoveControlPanelDown()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(offScreenPosition, onScreenPosition, timer);
        if (timer < 1.0f)
        {
            timer += Time.deltaTime * speed;
        }
    }

    private void MoveControlPanelUp()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(onScreenPosition, offScreenPosition, timer);
        if (timer < 1.0f)
        {
            timer += Time.deltaTime * speed;
        }

        if (pausable.isGamePaused)
        {
           pausable.TogglePause();
        }
    }

    public void OnControlButtonPressed() {
        ToggleControlPanel();
    }

    public void OnLoadButtonPressed() {
        LoadFromPlayerPrefs();

        player.controller.enabled = false;
        player.transform.position = sceneData.playerPosition;
        player.transform.rotation = sceneData.playerRotation;
        player.controller.enabled = true;

        player.health = sceneData.playerHealth;
        player.healthBar.SetHealth(sceneData.playerHealth);
    }

    public void OnSaveButtonPressed() {
        sceneData.playerPosition = player.transform.position;
        sceneData.playerRotation = player.transform.rotation;
        sceneData.playerHealth = player.health;   

        SaveToPlayerPrefs();
    } 

    public void SaveToPlayerPrefs() {
        // Serialize and save our data to Player Preferences dictionary / db (ideal)
        // PlayerPrefs.SetString("playerData", JsonUtility.ToJson(sceneData)); 

        // (what we need to do)
        PlayerPrefs.SetFloat("player.TransformX", sceneData.playerPosition.x);
        PlayerPrefs.SetFloat("player.TransformY", sceneData.playerPosition.x);
        PlayerPrefs.SetFloat("player.TransformZ", sceneData.playerPosition.x);

        PlayerPrefs.SetFloat("player.RotationX", sceneData.playerRotation.x);
        PlayerPrefs.SetFloat("player.RotationY", sceneData.playerRotation.y);
        PlayerPrefs.SetFloat("player.RotationZ", sceneData.playerRotation.z);
        PlayerPrefs.SetFloat("player.RotationW", sceneData.playerRotation.w);

        PlayerPrefs.SetInt("playerHealth", sceneData.playerHealth);
    }

    public void LoadFromPlayerPrefs() {

        // Deserialize from player data (ideal)
        // var sceneDataJSONString = PlayerPrefs.GetString("playerData");
        // JsonUtility.FromJsonOverwrite(sceneDataJSONString, sceneData);

        // what will only work)
        sceneData.playerPosition.x = PlayerPrefs.GetFloat("player.TransformX");
        sceneData.playerPosition.y = PlayerPrefs.GetFloat("player.TransformY");
        sceneData.playerPosition.z = PlayerPrefs.GetFloat("player.TransformZ");

        sceneData.playerRotation.x = PlayerPrefs.GetFloat("player.RotationX");
        sceneData.playerRotation.y = PlayerPrefs.GetFloat("player.RotationY");
        sceneData.playerRotation.z = PlayerPrefs.GetFloat("player.RotationZ");
        sceneData.playerRotation.w = PlayerPrefs.GetFloat("player.RotationW");

        sceneData.playerHealth = PlayerPrefs.GetInt("playerHealth");
    }
}
