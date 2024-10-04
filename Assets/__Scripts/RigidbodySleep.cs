using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]//Ensures that any GameObject this script is attached to has Rigidbody component
public class RigidbodySleep : MonoBehaviour
{
    private int sleepCountdown = 4;
    private Rigidbody rigid;

    void Awake(){
        rigid = GetComponent<Rigidbody>();//Expensive so only call once on awake for each wall and then cache result
    }

    void FixedUpdate(){
        if(sleepCountdown > 0){
            rigid.Sleep();
            sleepCountdown--;//stablize the castle telling object not to move by decrementing
        }
    }



}
