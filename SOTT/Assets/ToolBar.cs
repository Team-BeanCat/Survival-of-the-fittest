using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class ToolBar : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public Slider reloadProgress;
    public GameObject loadingContainer;

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        StartCoroutine("LoadAsyncronouly");
    }

    IEnumerator LoadAsyncronouly()
    {

        AsyncOperation loading = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex); //Begin Loading
        loadingContainer.SetActive(true); //Show the Loading bar and label

        while (!loading.isDone)
        {
            Debug.Log("Reload progress: " + loading.progress); //Update in console
            reloadProgress.value = loading.progress*100; //Update the slider
            loadingText.SetText("Loading: <color=#00AFFF>" + Mathf.Round(loading.progress*100) + "% </color>"); //Update Label
            yield return null;
        }
    }
}
