using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public GameObject oldScene;

    public UnityEvent onLoadAction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LoadAsyncScene()
    {
        PlayerManagement.instance.SetCinematicLookat();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Outside");

        //oldScene.

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        onLoadAction.Invoke();

        yield return new WaitForSeconds(15);


        Destroy(oldScene.gameObject);

         
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.attachedRigidbody.gameObject == PlayerManagement.player.gameObject)
            {
                StartCoroutine(LoadAsyncScene());
            }
        }
    }
}
