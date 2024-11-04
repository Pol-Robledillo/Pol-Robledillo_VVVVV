using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject startFlag;
    public GameObject endFlag;
    public GameObject character;
    public bool gameFinished = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (Character.character == null)
            {
                character = GameObject.Find("Player");
            }
            else
            {
                character = Character.character.gameObject;
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isPaused = false;
        Time.timeScale = 1;

        character.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        character.GetComponent<Character>().gravityChanged = false;

        startFlag = GameObject.Find("StartFlag");
        endFlag = GameObject.Find("EndFlag");

        if (character.GetComponent<Character>().advanced)
        {
            character.GetComponent<Character>().spawn = new Vector2(startFlag.transform.position.x + 2f, startFlag.transform.position.y);
        }
        else
        {
            character.GetComponent<Character>().spawn = new Vector2(endFlag.transform.position.x - 2f, endFlag.transform.position.y);
        }
        character.transform.position = character.GetComponent<Character>().spawn;
    }
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        TogglePauseCheckKey();
    }
    private void TogglePauseCheckKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameFinished)
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        if (!gameFinished)
        {
            pauseMenu.SetActive(!pauseMenu.gameObject.activeSelf);
        }
        isPaused = !isPaused;
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
        TogglePause();
    }
    public void Exit()
    {
        Application.Quit();
    }
}
