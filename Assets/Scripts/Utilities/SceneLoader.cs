using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public GameObject oldScene;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Outside");

        //oldScene.

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(40);

         
    }
}
