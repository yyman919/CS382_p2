using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [Header("Inscribed")]

    // Speed at which the Barrier moves
    public float        speed = 10f;

    // Distance where Barrier turns around
    public float        upAndDownEdge = 10f;

    // Chance that the Barrier will change directions
    public float        changeDirChance = 0.1f;

    void Update () {
        // Basic Movement
        Vector3 pos = transform.position;
        pos.y += speed * Time.deltaTime;
        transform.position = pos;
        // Changing Direction
        if (pos.y < -upAndDownEdge) {
            speed = Mathf.Abs(speed);   // Move up
        }else if (pos.y > upAndDownEdge) {
            speed = -Mathf.Abs(speed);  // Move down
        }
    }

    void FixedUpdate () {
        // Random direction changes are not time-based due to FixedUpdate()
        if ( Random.value < changeDirChance) {
            speed *= -1;    // Change Direction
        }  
    }
}
