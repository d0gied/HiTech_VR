using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Manager : MonoBehaviour
{
    public GameObject settings;
    public GameObject slider;
    public GameObject audio_sys;
    public GameObject loading_screen;
    public Slider loadingBar;
    public GameObject gameObjects;

    public void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            audio_sys.GetComponent<AudioSource>().volume = 1f;
        }
        else
        {
            audio_sys.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
        }
    }
    public void load_scene()
    {
        PlayerPrefs.SetFloat("volume", audio_sys.GetComponent<AudioSource>().volume);
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);
        loading_screen.SetActive(true);
        gameObjects.SetActive(false);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
    }

    public void Value_Changed()
    {
        audio_sys.GetComponent<AudioSource>().volume = (slider.transform.position.x + 0.52f) / 1.04f;
    }

    public void Set_Active_Canvas()
    {
        settings.SetActive(!settings.activeSelf);
    }
}