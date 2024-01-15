using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       GameObject leftWall = new GameObject();
       leftWall.name = "Left Wall";
       leftWall.AddComponent<Wall>();
       leftWall.transform.position = this.transform.position;
       leftWall.transform.parent = this.transform;

       GameObject rightWall = new GameObject();
       rightWall.name = "Right Wall";
       rightWall.AddComponent<Wall>();
       rightWall.transform.position = new Vector3(this.transform.position.x,
                                                  this.transform.position.y,
                                                  this.transform.position.z + 100);
       rightWall.transform.parent = this.transform;


       GameObject frontWall = new GameObject();
       frontWall.name = "Front Wall";
       frontWall.AddComponent<Wall>();
       frontWall.transform.position = new Vector3(this.transform.position.x + 49,
                                                  this.transform.position.y,
                                                  this.transform.position.z + 50);
       frontWall.transform.Rotate(0, 90f, 0, Space.Self);
       frontWall.transform.parent = this.transform;      

       GameObject backWall = new GameObject();
       backWall.name = "Back Wall";
       backWall.AddComponent<Wall>();
       backWall.transform.position = new Vector3(this.transform.position.x - 49,
                                                  this.transform.position.y,
                                                  this.transform.position.z + 50);
       backWall.transform.Rotate(0, 90f, 0, Space.Self);
       backWall.transform.parent = this.transform;   
    
       GameObject floor = new GameObject();
       floor.name = "Floor";
       floor.AddComponent<Wall>();
       floor.transform.position = new Vector3(this.transform.position.x,
                                              this.transform.position.y - 20,
                                              this.transform.position.z + 50);
       floor.transform.Rotate(90f, 0 ,0, Space.Self);
       floor.transform.localScale = new Vector3(this.transform.localScale.x,
                                                this.transform.localScale.y + 3,
                                                this.transform.localScale.z);
       floor.transform.parent = this.transform;
    }

}
