using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            DontDestroyOnLoad(gameObject);
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
