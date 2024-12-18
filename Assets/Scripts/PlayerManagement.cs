using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject GetDeepestParent(this GameObject gameObject)
    {
        GameObject deepestParent = gameObject;
        while(true)
        {
            if (deepestParent.transform.parent==null)
            {
                return deepestParent;
            }

            deepestParent = deepestParent.transform.parent.gameObject;
        }
    }


}

public class PlayerManagement : MonoBehaviour
{
    public static PogoController player;

    public static PlayerManagement instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            GameObject dontDestroyObject = gameObject.GetDeepestParent();
            DontDestroyOnLoad(dontDestroyObject);
        }
        else
        {
            Destroy(gameObject);
        }

        player=FindObjectOfType<PogoController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
