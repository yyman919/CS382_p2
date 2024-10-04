using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other){
        //When the trigger is hit in the cube goal object
        //Checks to see if it was the Projectile
        Projectile proj = other.GetComponent<Projectile>();
        if(proj != null){
            //Set goat met to true
            Goal.goalMet = true;
            //Set the color higher
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.75f;
            mat.color = c;

        }
    }

}
