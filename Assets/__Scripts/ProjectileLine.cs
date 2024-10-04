using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileLine : MonoBehaviour
{   
    static List<ProjectileLine> PROJ_LINES = new List<ProjectileLine>();//Shared by all instances of ProjectileLine class (Store a reference for every ProjectileLine Scripts)
    private const float DIM_MULT = 0.75f;

    //Underscore to remind private
    private LineRenderer _line;
    private bool _drawing = true;
    private Projectile _projectile;


    void Start()
    {
        _line = GetComponent<LineRenderer>(); //get a reference to the LineRender and assign 1st pos
        _line.positionCount = 1;
        _line.SetPosition(0, transform.position);
        
        //Allows us to search hierarchy from this gameobject to look for components (attach to Projectile to follow flight)
        _projectile = GetComponentInParent<Projectile>();
        ADD_LINE(this);//Add to the ProjectileLine
        
    }

    private void OnDestroy(){
        //Removes ProjectileLine when GameObject destroyed
        PROJ_LINES.Remove(this);
    }

    static void ADD_LINE(ProjectileLine newLine){
        Color col;
        //Iterate over all the old lines and dim them
        foreach(ProjectileLine pl in PROJ_LINES){
            col = pl._line.startColor;
            col = col * DIM_MULT;
            pl._line.startColor = pl._line.endColor = col;
        }
        PROJ_LINES.Add(newLine);
    }

    void FixedUpdate(){
        if(_drawing){
            _line.positionCount++;//Each frame the Projectileline exisits increases the posCount (follow behind the projectile as it flies)
            _line.SetPosition(_line.positionCount-1, transform.position);
            if(_projectile != null){
                if(!_projectile.awake){//Will keep generating line till this is false
                    _drawing = false;
                    _projectile = null;
                }
            }
        }
    }

}
