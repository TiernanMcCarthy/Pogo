using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundLooper : MonoBehaviour
{
    [SerializeField] private List<AudioClip> mAudioClipList;

    private AudioSource mAudioSource;

    int mAudioClipIndex = -1;


    // Start is called before the first frame update
    void Start()
    {
        mAudioSource= GetComponent<AudioSource>();
        mAudioSource.loop=false;
        mAudioSource.priority = 999;
    }

    private void CheckPlayStatus()
    {
        if(!mAudioSource.isPlaying)
        {
            mAudioClipIndex++;
            if(mAudioClipIndex >= mAudioClipList.Count)
            {
                mAudioClipIndex = 0;
            }
            mAudioSource.clip = mAudioClipList[mAudioClipIndex];
            mAudioSource.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckPlayStatus();
    }
}
