using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCover : MonoBehaviour
{
    [Header("Inscribed")]
    public Sprite[] cloudSprites;
    public int numClouds = 40;
    public Vector3 minPos = new Vector3(-20, -5, -5);
    public Vector3 maxPos = new Vector3(300, 40, 5);

    [Tooltip("For scaleRange, x isthe min value, and y is the max value")]
    public Vector2 scaleRange = new Vector2(1, 4);//min and max values in Inspector (for this)

    void Start(){
        Transform parentTrans = this.transform;
        GameObject cloudGO;
        Transform cloudTrans;
        SpriteRenderer sRend;
        float scaleMult;

        for(int i = 0; i < numClouds; i++){
            //Creates a empty GameObject from scratch in script
            cloudGO= new GameObject();
            cloudTrans = cloudGO.transform;
            sRend = cloudGO.AddComponent<SpriteRenderer>(); //allows us to assign one cloud sprite later

            int spriteNum = Random.Range(0, cloudSprites.Length); //randomly pick between the cloud sprites
            sRend.sprite = cloudSprites[spriteNum];

            cloudTrans.position = RandomPos(); //random position in the background
            cloudTrans.SetParent(parentTrans, true);

            scaleMult = Random.Range(scaleRange.x, scaleRange.y);//(for this matches up top)
            cloudTrans.localScale = Vector3.one * scaleMult;
        }
    }

    Vector3 RandomPos(){
        Vector3 pos = new Vector3();
        pos.x = Random.Range(minPos.x, maxPos.x);
        pos.y = Random.Range(minPos.y, maxPos.y);
        pos.x = Random.Range(minPos.z, maxPos.y);
        return pos;
    }

    
    void Update()
    {
        
    }
}
