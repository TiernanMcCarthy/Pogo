using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraModeTrigger : MonoBehaviour
{
    public enum CameraMode
    {
        Cinematic,
        Gameplay
    }

    public UnityEvent OnSwitch;

    [SerializeField] private CameraMode mode;


    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.attachedRigidbody.gameObject == PlayerManagement.player.gameObject)
            {
                if(mode == CameraMode.Cinematic)
                {
                    PlayerManagement.instance.SetCinematicLookat();
                    OnSwitch.Invoke();
                }
                else
                {
                    PlayerManagement.instance.SetNormalLookat();
                    OnSwitch.Invoke();
                }
            }
        }
    }

}
