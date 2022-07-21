using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneChanger : MonoBehaviour
{

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            LoadingScene.LoadScene("InGame");
        }
    }
}
