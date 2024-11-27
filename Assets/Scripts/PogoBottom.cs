using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PogoBottom : MonoBehaviour
{

    [SerializeField] private PogoController parent;

    [SerializeField] private float jumpInterval = 0.2f;

    private float lastJump = 0;

    [SerializeField] private float floatHeight;

    [SerializeField] private float springStrength;
    [SerializeField] private float springDamper;


    private void ManageJump()
    {
        RaycastHit hit;

        // Cast ray down to detect the terrain or surface below the player
        if (Physics.Raycast(transform.position, transform.up, out hit, floatHeight))
        {
           // Vector3 velocity = parent.

            // Get the distance between the player and the surface
            float currentHeight = hit.distance;

            // Calculate the height difference from the desired floatHeight
            float heightDifference = currentHeight - floatHeight;
        }
    }
}
    
