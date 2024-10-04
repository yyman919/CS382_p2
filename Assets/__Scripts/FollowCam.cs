using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static private FollowCam S;
    static public GameObject POI; //Point of Interest Object the camera will be following
    public enum eView {none, slingshot, castle, both};

    [Header("Inscribed")]
    public float easing = 0.05f; //returns half way point
    public Vector2 minXY = Vector2.zero; //Vector2.zero = [0,0]
    public GameObject viewBothGO;

    [Header("Dynamic")]
    public float camZ;
    public eView nextView = eView.slingshot;


    void Awake(){
        S = this;
        camZ = this.transform.position.z;
    }

    void FixedUpdate(){

        //Commented out and no longer used
        /*if(POI == null) return;
        Vector3 destination = POI.transform.position; //Get pos of POI
        */

        //New to implement return to camera when done with on launch
        Vector3 destination = Vector3.zero; //Sets to [0,0,0]
        if(POI != null) {
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if((poiRigid != null) && poiRigid.IsSleeping()){ //Check if projectile is sleeping
                POI = null;
            }
        }

        if(POI != null){
            destination = POI.transform.position;
        }

        //Min X and Y limits for camera to make it not go too far left or down
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing); //linear interpolation = finds a point between both Vector3s passed in
        destination.z = camZ; //Force destination to be far away enough
        transform.position = destination; //Set camera to destination
        Camera.main.orthographicSize = destination.y + 10; //This adds to keep the ground in view
    }

    public void SwitchView(eView newView){
        if(newView == eView.none){
            newView = nextView;
        }
        switch (newView){
            case eView.slingshot:
                POI = null;
                nextView = eView.castle;
                break;
            case eView.castle:
                POI = MissionDemolition.GET_CASTLE();
                nextView = eView.both;
                break;
            case eView.both:
                POI = viewBothGO;
                nextView = eView.slingshot;
                break;
        }
    }

    public void SwitchView(){
        SwitchView(eView.none);
    }

    static public void SWITCH_VIEW(eView newView){
        S.SwitchView(newView);
    }


}
