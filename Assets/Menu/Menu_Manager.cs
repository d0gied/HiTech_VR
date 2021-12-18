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
    public void load_scene()
    {
        SceneManager.LoadScene("terrain");
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
