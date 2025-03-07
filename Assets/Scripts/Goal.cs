using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    // Trigger method to detect collisions with the goal
    private void OnTriggerEnter(Collider other)
    {
        // Attempt to get the BallLifetime component from the colliding object
        BallLifetime ballLifetime = other.GetComponent<BallLifetime>();

        // If the colliding object has a BallLifetime component, handle the goal logic
        if (ballLifetime != null)
        {
            goalMet = true;

            // Change the material's color to indicate the goal was met
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.75f; // Adjust alpha for transparency
            mat.color = c;

            Debug.Log("Goal met!");
        }
    }
}
