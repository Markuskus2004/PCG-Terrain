using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Road : MonoBehaviour
{
    float depth = 1f;

    float pavementWidth = 0.5f;
    float pavementHeight = 0.2f;

    float laneWidth = 2f;
    float laneHeight = 0.1f;

    float roadMarkingWidth = 0.05f;
    float roadMarkingHeight = 0.1f;

    float carPositionZOffset = 5f;

    [SerializeField]
    private GameObject carPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject leftRoad = new GameObject();
        leftRoad.name = "Left Road";
        leftRoad.transform.position = new Vector3(
           this.transform.position.x + 13.16f,
           this.transform.position.y,
           this.transform.position.z);
        leftRoad.transform.parent = this.transform;
        CreateRoadSegment(leftRoad);
        leftRoad.transform.localScale = new Vector3(1f, 1f, 30f);

        GameObject rightRoad = new GameObject();
        rightRoad.name = "Right Road";
        rightRoad.transform.position = new Vector3(
            this.transform.position.x + 96.25f,
            this.transform.position.y,
            this.transform.position.z);
        rightRoad.transform.parent = this.transform;
        CreateRoadSegment(rightRoad);
        rightRoad.transform.localScale = new Vector3(1f, 1f, 30f);

        GameObject topRoad = new GameObject();
        topRoad.name = "Top Road";
        topRoad.transform.position = new Vector3(
            this.transform.position.x + 59.2f,
            this.transform.position.y,
            this.transform.position.z + 40);
        topRoad.transform.parent = this.transform;
        CreateRoadSegment(topRoad);
        topRoad.transform.localScale = new Vector3(1f, 1f, 46.5f);
        topRoad.transform.Rotate(0f, 90f, 0f, Space.Self);

        GameObject bottomRoad = new GameObject();
        bottomRoad.name = "Bottom Road";
        bottomRoad.transform.position = new Vector3(
            this.transform.position.x + 59.2f,
            this.transform.position.y,
            this.transform.position.z - 40);
        bottomRoad.transform.parent = this.transform;
        CreateRoadSegment(bottomRoad);
        bottomRoad.transform.localScale = new Vector3(1f, 1f, 46.5f);
        bottomRoad.transform.Rotate(0f, -90f, 0f, Space.Self);

        if (carPrefab != null){

            float randomZPosition = Random.Range
            ((-leftRoad.transform.localScale.z + carPositionZOffset),
             (leftRoad.transform.localScale.z - carPositionZOffset)); 

            Vector3 carPosition = new Vector3(
                leftRoad.transform.position.x + pavementWidth + laneWidth,
                leftRoad.transform.position.y,
                randomZPosition 
            );

            GameObject car = Instantiate(carPrefab,
                                        carPosition,
                                        Quaternion.identity);
            car.name = "Car";
            car.transform.parent = this.transform;
            Debug.Log(car.transform.position);
        }
    }


    private void CreateRoadSegment(GameObject parentRoad){

        Material pavementMaterial = new Material(Shader.Find("Specular"));
        pavementMaterial.color = Color.grey;

        Material laneMaterial = new Material(Shader.Find("Specular"));
        laneMaterial.color = Color.black;

        Material roadMarkingMaterial = new Material(Shader.Find("Specular"));
        roadMarkingMaterial.color = Color.white;

        List<Material> pavementMaterialList = new List<Material>();
        pavementMaterialList.Add(pavementMaterial);

        List<Material> laneMaterialList = new List<Material>();
        laneMaterialList.Add(laneMaterial);

        List<Material> roadMarkingMaterialList = new List<Material>();
        roadMarkingMaterialList.Add(roadMarkingMaterial);


        //left pavement
        GameObject leftPavement = new GameObject();
        leftPavement.name = "Left Pavement";
        leftPavement.AddComponent<RoadSegment>();
        leftPavement.GetComponent<RoadSegment>().SetSize(
                new Vector3(pavementWidth,pavementHeight, depth));
        leftPavement.GetComponent<RoadSegment>().
            UpdateMaterialsList(pavementMaterialList);
        leftPavement.transform.position = parentRoad.transform.position;
        leftPavement.transform.parent = parentRoad.transform;

        //left lane
        GameObject leftLane = new GameObject();
        leftLane.name = "Left Lane";
        leftLane.AddComponent<RoadSegment>();
        leftLane.GetComponent<RoadSegment>().SetSize(
            new Vector3(laneWidth, laneHeight, depth));
        leftLane.GetComponent<RoadSegment>().UpdateMaterialsList(
            laneMaterialList);
        leftLane.transform.position = new Vector3(
            leftPavement.transform.position.x + pavementWidth + laneWidth,
            leftPavement.transform.position.y,
            leftPavement.transform.position.z);
        leftLane.transform.parent = parentRoad.transform;
        
        //white road marking
        GameObject roadMark = new GameObject();
        roadMark.name = "Road Mark";
        roadMark.AddComponent<RoadSegment>();
        roadMark.GetComponent<RoadSegment>().SetSize(
            new Vector3(roadMarkingWidth, roadMarkingHeight, depth));
        roadMark.GetComponent<RoadSegment>().UpdateMaterialsList(
            roadMarkingMaterialList);
        roadMark.transform.position = new Vector3(
            leftLane.transform.position.x + laneWidth + roadMarkingWidth,
            leftLane.transform.position.y,
            leftLane.transform.position.z);
        roadMark.transform.parent = parentRoad.transform;

        //right lane
        GameObject rightLane = new GameObject();
        rightLane.name = "Right Lane";
        rightLane.AddComponent<RoadSegment>();
        rightLane.GetComponent<RoadSegment>().SetSize(
            new Vector3(laneWidth, laneHeight, depth));
        rightLane.GetComponent<RoadSegment>().UpdateMaterialsList(
            laneMaterialList);
        rightLane.transform.position = new Vector3(
            roadMark.transform.position.x + roadMarkingWidth + laneWidth,
            roadMark.transform.position.y,
            roadMark.transform.position.z);
        rightLane.transform.parent = parentRoad.transform;

        //right pavement
        GameObject rightPavement = new GameObject();
        rightPavement.name = "Right Pavement";
        rightPavement.AddComponent<RoadSegment>();
        rightPavement.GetComponent<RoadSegment>().SetSize(
            new Vector3(pavementWidth, pavementHeight, depth));
        rightPavement.GetComponent<RoadSegment>().UpdateMaterialsList(
            pavementMaterialList);
        rightPavement.transform.position = new Vector3(
            rightLane.transform.position.x + laneWidth + pavementWidth,
            rightLane.transform.position.y,
            rightLane.transform.position.z);
        rightPavement.transform.parent = parentRoad.transform;
    }


}