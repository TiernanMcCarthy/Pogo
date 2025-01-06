using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoundContainer", order = 1)]
public class SoundContainer : ScriptableObject
{
    [SerializeField] private List<AudioClip> genericSounds;
    [SerializeField] private List<AudioClip> destructionSounds;

    public float genericMultiplier = 1;
    public float destructionMultiplier  = 1;

    [SerializeField] private float spacialBlend = 1;
    [SerializeField] private float falloffDistance = 50;
    public AudioClip GetGenericSound()
    {
        if (genericSounds != null)
        {
            if(genericSounds.Count > 0)
            {
                AudioClip clip= genericSounds[Random.Range(0, genericSounds.Count)];
                return clip;
            }
        }
        Debug.LogError(string.Format("Generic Sounds of {0} Scriptable Object have not been assigned", name));
        return null;
    }

    public void ManageAudioSource(AudioSource source)
    {
        source.spatialBlend = spacialBlend;
        source.maxDistance = falloffDistance;
    }

    public AudioClip GetDestructionSound()
    {
        if (destructionSounds != null)
        {
            if (destructionSounds.Count > 0)
            {
                AudioClip clip = destructionSounds[Random.Range(0, destructionSounds.Count)];
                return clip;
            }
        }
        Debug.LogError(string.Format("Destruction Sounds of {0} Scriptable Object have not been assigned", name));
        return null;
    }
}
