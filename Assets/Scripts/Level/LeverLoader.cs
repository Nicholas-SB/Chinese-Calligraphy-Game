using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LeverLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    // void Update()
    // {
    //     if(Input.GetMouseButtonDown(0))
    //     {
    //         LoadNextLevel();
    //     }
    // }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel (int levelIndex)
    {
        transition.SetTrigger("isStart");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
