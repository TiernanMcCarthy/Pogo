using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSoundSource : MonoBehaviour
{
    [SerializeField]
    private AudioSource soundSource;

    [SerializeField] private SoundContainer container;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (container != null)
        {
            soundSource.transform.position = collision.ClosestPoint(collision.transform.position);
            soundSource.clip = container.GetGenericSound();
            soundSource.Play();
        }
    }
}
