using UnityEngine;

public class BuildingGenerator2 : MonoBehaviour
{
    public int numberOfBuildings = 6; 
    public float citySize = 20f;

    void Start()
    {
        GenerateBuildings();
    }

    void GenerateBuildings()
    {
        for (int i = 0; i < numberOfBuildings; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-citySize, citySize), 0f, Random.Range(-citySize, citySize));
            CreateBuilding(randomPosition);
        }
    }

    void CreateBuilding(Vector3 position)
    {
        
        Material buildingMaterial = new Material(Shader.Find("Standard"));
        buildingMaterial.color = Random.ColorHSV(); 

        GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);

        
        position.y = Mathf.Max(position.y, 3f);
        position.x = Mathf.Max(position.x, 3f);

        building.transform.position = position;

        
        float buildingScale = Random.Range(5f, 10f); 
        building.transform.localScale = new Vector3(buildingScale, buildingScale, buildingScale);

        building.GetComponent<Renderer>().material = buildingMaterial;

        
    }
}
