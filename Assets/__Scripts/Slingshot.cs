using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]//Set in Unity Inspector Pane (dragged prefab projectile)
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;//Set prefab for ProjectileLine

    [Header("Dynamic")]//Set Dynamically when game running
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    public Transform leftBand;
    public Transform rightBand;
    public LineRenderer leftLine;
    public LineRenderer rightLine;

    public AudioClip launchSound; 
    private AudioSource audioSource;


    void Awake(){
        Transform launchPointTrans = transform.Find("LaunchPoint");//searches for a child of Slingshot named LaunchPoint
        launchPoint = launchPointTrans.gameObject; //gets gameobject associated w/ transform and assigns it to GameObject launchPoint
        launchPoint.SetActive(false); //SetActive shows the halo and that is the range user interacts with (deactivates the gameobject)
        launchPos = launchPointTrans.position;


        //ADDED CONTENT HERE TO GET RUBBER BAND USING LINE RENDERER
        //To get the LineRenderer located in 
        leftLine = leftBand.GetComponent<LineRenderer>();
        rightLine = rightBand.GetComponent<LineRenderer>();

        //Declare how many points I am using to connect the sling shot
        leftLine.positionCount = 2;
        rightLine.positionCount = 2;

        //If not on click sling shot does not appear
        leftLine.enabled = false;
        rightLine.enabled = false;

        //To add slingshot rubber band sound
        audioSource = GetComponent<AudioSource>();
    }

    //Enter and Exit Function of Sphere surronding the slingshot
    void OnMouseEnter(){
        //print("Slingshot: OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit(){
        //print("Slingshot: OnMouseExit()");
        launchPoint.SetActive(false);
    }

    //User clicks in the Collider Component
    void OnMouseDown(){
        aimingMode = true; //Pressed mouse button
        projectile = Instantiate(projectilePrefab) as GameObject;//Created object
        projectile.transform.position = launchPos; //Start at launchPoint
        projectile.GetComponent<Rigidbody>().isKinematic = true; //Set to isKinematic

        //If the mouse clicked down the slingshot rubber band appears
        leftLine.enabled = true;
        rightLine.enabled = true;
    }

    void Update(){
        if(!aimingMode) return;

        //Mouse pos in 2D screen
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D -launchPos; //Find delta from launchPos to mousePos3D

        //Limits mouseDelta to radius of Collider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude){
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //Move projectile to new Pos
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        //Set postion of sling shot line to connect to the leftCylinder & projectile
        leftLine.SetPosition(0, leftBand.position);
        leftLine.SetPosition(1, projectile.transform.position);

        //Set position of slingshot line to connect to rightCylinder & projectile
        rightLine.SetPosition(0, rightBand.position);
        rightLine.SetPosition(1, projectile.transform.position);

        //Mouse Released
        if(Input.GetMouseButtonUp(0)){
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;//once again respond to gravity
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult; //velocity proportional to mouse distance

            //Add Audio Component
            audioSource.PlayOneShot(launchSound);

            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
            FollowCam.POI = projectile; //Calls from FollowCam.cs to set the MainCamera to follow the projectile
            Instantiate<GameObject>(projLinePrefab, projectile.transform);//Accepts a parent transform as the second parameter (ProjectLinePrefab will be created as a child of projectile)
            projectile = null; //does not delete just clears to create another
            MissionDemolition.SHOT_FIRED();

            leftLine.enabled = false;
            rightLine.enabled = false;
        }

    }



}
