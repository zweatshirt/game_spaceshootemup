using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enemy_4 will start offscreen and then pick a random point on screen to
/// move to. Once it has arrived, it will pick another random point and /// continue until the player has shot it down.
/// </summary>
public class Enemy_4 : Enemy {
    private Vector3 p0, p1; // The two points to interpolate 
    private float timeStart; // Birth time for this Enemy_4 
    private float duration = 4; // Duration of movement


    void Start () {
        // There is already an initial position chosen by Main.SpawnEnemy() 
        // so add it to points as the initial p0 & p1
        p0 = p1 = pos; // a
        InitMovement();
    }
    
    
    void InitMovement() { // b
        p0 = p1; // Set p0 to the old p1
        // Assign a new on-screen location to p1
        float widMinRad = boundCheck.camWidth - boundCheck.radius; 
        float hgtMinRad = boundCheck.camHeight - boundCheck.radius; 
        p1.x = Random.Range( -widMinRad, widMinRad );
        p1.y = Random.Range( -hgtMinRad, hgtMinRad );
        // Reset the time
        timeStart = Time.time;
    }


    public override void Move () { // c
        // This completely overrides Enemy.Move() with a linear interpolation 
        float u = (Time.time-timeStart)/duration;
        if (u>=1) {
            InitMovement();
            u=0;
        }
        u = 1 - Mathf.Pow( 1-u, 2 ); // Apply Ease Out easing to u 
        pos = (1-u)*p0 + u*p1; // Simple linear interpolation
    }
}