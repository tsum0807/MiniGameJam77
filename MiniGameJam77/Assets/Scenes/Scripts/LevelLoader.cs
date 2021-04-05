using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public Animator fadeInTransition;

    public bool hasFadedIn = false;

    public float transitionTime = 1f;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        LoadNextLevel();
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Screen")
        {
            hasFadedIn = true;
        }
    }

    public void LoadNextLevel()
    {
       StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
      
        SceneManager.LoadScene(levelIndex);

        //fadeInTransition.SetTrigger("Start");
    }
}
