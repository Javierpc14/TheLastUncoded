using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float delay = 1.5f;
    // Update is called once per frame
    public void PlayGame()
    {
        StartCoroutine(LoadSceneDelay(1));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneDelay(int index)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(index);
    }
}

