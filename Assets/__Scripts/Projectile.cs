using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    const int LOOKBACK_COUNT = 10;//maxlen to allow deltas list to reach (const to share amoung all projectiles)
    static List<Projectile> PROJECTILES = new List<Projectile>(); //All projectiles in the game
    [SerializeField]
    private bool _awake = true;

    //Public Get, Private Set Properties
    public bool awake { //Be able to see in constructor and read but not want other scripts to set value
        get{return _awake;}
        private set {_awake = value;}
    }
    
    //To position and distance far from actual pos of projectile to avoid physic hiccups
    private Vector3 prevPos;
    private List<float> deltas = new List<float>();//Stores history of Projectiles move distance
    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        awake = true;
        prevPos = new Vector3(1000,1000,0);
        deltas.Add(1000);

        PROJECTILES.Add(this);
    }

    void FixedUpdate(){
        if(rigid.isKinematic || !awake) return; //Will only execute between when projectile is fired or asleep

        Vector3 deltaV3 = transform.position - prevPos;//Distance between current pos and last pos
        deltas.Add(deltaV3.magnitude);//Store in delta
        prevPos = transform.position;

        while(deltas.Count > LOOKBACK_COUNT){
            deltas.RemoveAt(0);
        }

        float maxDelta = 0;
        foreach(float f in deltas){
            if(f > maxDelta) maxDelta = f;
        }

        if(maxDelta <= Physics.sleepThreshold){
            awake = false;
            rigid.Sleep();
        }
    }

    private void OnDestroy(){
        PROJECTILES.Remove(this);
    }

    static public void DESTROY_PROJECTILES(){
        foreach(Projectile p in PROJECTILES){
            Destroy(p.gameObject);
        }
    }



}
