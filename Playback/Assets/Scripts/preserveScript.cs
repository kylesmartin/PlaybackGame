using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class that controls game soundtrack
public class preserveScript : MonoBehaviour
{
    private void Awake()
    {
        // destroy any duplicates of the soundtrack
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        // dont destroy soundtrack object between scenes
        DontDestroyOnLoad(this.gameObject);

    }
}
