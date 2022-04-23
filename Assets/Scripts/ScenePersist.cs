using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    private int startingSceneIndex;

    private void Awake()
    {
        int numberOfScenePersists = FindObjectsOfType<ScenePersist>().Length;
        
        if (numberOfScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (startingSceneIndex != currentSceneIndex)
        {
            Destroy(gameObject);
        }
    }
}
