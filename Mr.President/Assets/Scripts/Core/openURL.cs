using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openURL : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void open()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSc-VutxSq-YT0s9wIROBqXQDo1SJtT5Glse_zCJZ0ZqFVdlfA/viewform?usp=sf_link");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
