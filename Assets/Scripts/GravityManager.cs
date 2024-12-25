using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple manager for objects to fetch and manage gravity from
/// </summary>
public class GravityManager : MonoBehaviour
{
    /// <summary>
    /// Gravity can be managed by a gravity manager instead of the normal physics, this lets us manipulate gravity in silly ways
    /// </summary>
    public static Vector3 sceneGravity;

    public static GravityManager instance;

    public float rotateSpeed;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
            GameObject dontDestroyObject = gameObject.GetDeepestParent();
            DontDestroyOnLoad(dontDestroyObject);
        }
        else
        {
            Destroy(gameObject);
        }

        sceneGravity = Physics.gravity;

    }

    private void FixedUpdate()
    {
        // Define the scene "up" direction (target up direction)
        Vector3 sceneUp = -PlayerManagement.player.GetGravity();

        // Get the current "up" direction of the object
        Vector3 currentUp = transform.up;

        // Calculate the target rotation based on aligning the current "up" with the scene "up"
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, sceneUp);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        // Optionally: Check if the object is aligned and stop additional processing if needed
        if (Vector3.Angle(currentUp, sceneUp) < 0.1f)
        {
            // Stop further adjustments (optional logic can be added here)
            transform.rotation = targetRotation;
        }
    }
}
