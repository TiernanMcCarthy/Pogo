using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZoneBehaviour
{
    public class CinematicCamera : ZoneBehaviour
    {
        [SerializeField] private Transform cameraViewpoint;

        [SerializeField] private bool cameraControlDuringTraverse = false;

        [SerializeField] private float cameraLength;

        [SerializeField] private float exitTime;

        public void ActivateCamera()
        {
            if (!cameraControlDuringTraverse)
            {
                PlayerManagement.instance.UseCinematicCamera(cameraViewpoint.position, cameraViewpoint.forward, cameraLength);
            }
            else
            {
                PlayerManagement.instance.UseCinematicCamera(cameraViewpoint.position, cameraViewpoint.forward);
            }
        }

        protected override void FirstEntry(ColliderRigidbodyReference rigid)
        {
            if (rigid.rigid.gameObject == PlayerManagement.player.gameObject)
            {
                ActivateCamera();
            }
        }

        protected override void OnRigidBodyRemoved(ColliderRigidbodyReference rigid)
        {
            if(cameraControlDuringTraverse)
            {
                StartCoroutine(RemoveCamera());
            }
        }

        private IEnumerator RemoveCamera()
        {
            yield return new WaitForSeconds(exitTime);
            PlayerManagement.instance.DisableCinematicCamera();
        }

    }
}
