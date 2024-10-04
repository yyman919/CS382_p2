using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//For text
using UnityEngine.SceneManagement;

public enum GameMode{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitShots;
    public Vector3 castlePos; //Place to put castles
    public Vector3 flagPolePos;
    public GameObject[] castles; //Array of Castles
    
    [Header("Dynamic")]
    public int level; //Current level
    public int levelMax; //Num of levels
    public int shotsTaken;
    public GameObject castle; //Current Castle
    public GameObject flagPole;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //FollowCam mode



    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        flagPole = Instantiate<GameObject>(flagPole);
        flagPole.transform.position = flagPolePos;
        StartLevel();
    }

    void StartLevel(){
        //Get rid of old castle
        if(castle != null){
            Destroy(castle);
        }

        //Destroy old projectiles
        Projectile.DESTROY_PROJECTILES();
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        

        //Reset the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI(){ //Data to be input into the GUI
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();
        //Check for level end
        if((mode == GameMode.playing) && Goal.goalMet){
            mode = GameMode.levelEnd; //Stop checking level

            FollowCam.SWITCH_VIEW(FollowCam.eView.both);//Zoom out when player hits the goal
            //Start next leve in 2s
            Invoke("NextLevel", 2f);
        }
        
    }

    void NextLevel(){
        level++;
        if(level == levelMax){
            level = 0;
            shotsTaken = 0;
            SceneManager.LoadScene( "_Game_Over" );
        }
        StartLevel();
    }

    static public void SHOT_FIRED(){
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE(){
        return S.castle;
    }
    
}
