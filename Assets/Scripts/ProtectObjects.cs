using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectObjects : MonoBehaviour
{

    [SerializeField] private List<GameObject> protectedObjects= new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in protectedObjects)
        {
            obj.transform.parent = null;
            DontDestroyOnLoad(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
