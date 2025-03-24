using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private CinemachineFreeLook cameraBrain;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Camera cinematicCamera;

    private GameObject belowPogoTarget;
    private Transform pogoLookat;

    [SerializeField] private float pogoTargetDistance = 5.0f;

    [SerializeField] private TMP_Text textBox;
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

        cameraBrain= FindObjectOfType<CinemachineFreeLook>();
        player=FindObjectOfType<PogoController>();
        belowPogoTarget = new GameObject();
        belowPogoTarget.name = "Camera Target";

        pogoLookat = cameraBrain.GetRig(0).LookAt;

        DontDestroyOnLoad(belowPogoTarget);
    }

    public void SetCameraPosition(Vector3 position)
    {
        cameraBrain.ForceCameraPosition(position, Quaternion.LookRotation(player.transform.forward));
        cameraBrain.transform.position = position;
    }

    public void SetCinematicLookat()
    {
        cameraBrain.LookAt = belowPogoTarget.transform;
        cameraBrain.GetRig(0).LookAt = belowPogoTarget.transform;
        cameraBrain.GetRig(1).LookAt = belowPogoTarget.transform;
        cameraBrain.GetRig(2).LookAt = belowPogoTarget.transform;
    }

    public void SetNormalLookat()
    {
        cameraBrain.LookAt = pogoLookat;
        cameraBrain.GetRig(0).LookAt = pogoLookat;
        cameraBrain.GetRig(1).LookAt = pogoLookat;
        cameraBrain.GetRig(2).LookAt = pogoLookat;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        belowPogoTarget.transform.position = player.transform.position + player.GetGravity().normalized * pogoTargetDistance;
    }

    public void UpdateText(string text, float length)
    {
        textBox.text = text;
        StartCoroutine(HideText(length));
    }

    IEnumerator HideText(float length)
    {
        yield return new WaitForSeconds(length);
        textBox.text = "";
    }
    /// <summary>
    /// Cinematic Camera Control is managed here, a length specifies how long this lasts
    /// </summary>
    /// <param name="position"></param>
    /// <param name="forward"></param>
    /// <param name="length"></param>
    public void UseCinematicCamera(Vector3 position, Vector3 forward, float length)
    {
        player.UseCamera(cinematicCamera);
        mainCamera.enabled = false;
        cinematicCamera.enabled= true;
        cinematicCamera.transform.position = position;
        cinematicCamera.transform.forward = forward;
        StartCoroutine(CameraCountdown(length));
    }

    /// <summary>
    /// Cinematic Camera Control is managed by another object, it is your responsibility to relinuqish the camera
    /// </summary>
    /// <param name="position"></param>
    /// <param name="forward"></param>
    public void UseCinematicCamera(Vector3 position, Vector3 forward)
    {
        player.UseCamera(cinematicCamera);
        mainCamera.enabled = false;
        cinematicCamera.enabled = true;
        cinematicCamera.transform.position = position;
        cinematicCamera.transform.forward = forward;
    }

    public void DisableCinematicCamera()
    {
        cinematicCamera.enabled = false;
        mainCamera.enabled = true;
        player.UseCamera(mainCamera);
    }

    IEnumerator CameraCountdown(float length)
    {
        yield return new WaitForSeconds(length);
        cinematicCamera.enabled = false;
        mainCamera.enabled = true;
        player.UseCamera(mainCamera);
    }
}
