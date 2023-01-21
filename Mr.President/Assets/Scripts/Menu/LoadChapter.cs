using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadChapter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadChapter(string Chapter)
    {
        PlayerPrefs.SetString("Chapter",Chapter);
        MenuManager.instance.loading.SetBool("start", true);
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene() 
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainScene");
    }

}
