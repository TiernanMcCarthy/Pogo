using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour
{
    [SerializeField] private float volumeModifier = 1.0f;

    [SerializeField] private float minimumPlayMagnitude=1.0f;

    [SerializeField] private SoundContainer soundSourceContainer;

    [SerializeField] private AudioSource audioSource;

    private bool manageSound = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        soundSourceContainer.ManageAudioSource(audioSource);
    }
    public void PlayDestructionSound()
    {
        audioSource.clip = soundSourceContainer.GetDestructionSound();
        audioSource.volume=1*volumeModifier*soundSourceContainer.destructionMultiplier;
        audioSource.Play();
        manageSound = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(!manageSound)
        {
            return;
        }
        if (soundSourceContainer == null)
        {
            return;
        }
        float playerModifier = 1;
        bool angularSuccess=false;
        if(collision.rigidbody!=null)
        {
            if (collision.rigidbody.gameObject == PlayerManagement.player.gameObject)
            {
                playerModifier = 0.3f;
            }
            if(collision.rigidbody.angularVelocity.magnitude > minimumPlayMagnitude * playerModifier)
            {
                angularSuccess = true;
            }
        }

        //Determine if the object is moving fast enough or rotating fast enough to provoke a sound
        //A player has a much lower threshold for sound
        if(collision.relativeVelocity.magnitude>minimumPlayMagnitude*playerModifier || angularSuccess) 
        {
            //fetch a generic hit sound from the scriptable object library
            audioSource.clip=soundSourceContainer.GetGenericSound();

            //volume is modified by object speed and rotation
            float speedModifier = 1.0f + collision.relativeVelocity.magnitude / ((minimumPlayMagnitude * 3) + 0.001f);

            if (angularSuccess)
            {
                speedModifier += collision.rigidbody.angularVelocity.magnitude / ((minimumPlayMagnitude * 3) + 0.001f);
            }

            //play sound with modifiers
            audioSource.volume =1 *volumeModifier * soundSourceContainer.genericMultiplier;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!manageSound)
        {
            return;
        }
        if (soundSourceContainer == null)
        {
            return;
        }

        audioSource.clip = soundSourceContainer.GetGenericSound();
        audioSource.volume = 1 * volumeModifier * soundSourceContainer.genericMultiplier;
    }
}
