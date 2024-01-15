using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public int numberOfBuildings = 10; // Set the minimum number of buildings to 10
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
        // Create a new material for the building
        Material buildingMaterial = new Material(Shader.Find("Standard"));
        buildingMaterial.color = Random.ColorHSV(); // Random color for each building

        GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Ensure the building is above the 1Y and 1X axis
        position.y = Mathf.Max(position.y, 1f);
        position.x = Mathf.Max(position.x, 1f);

        building.transform.position = position;

        // Make the buildings bigger
        float buildingScale = Random.Range(4f, 20f); // You can adjust the range for the building size
        building.transform.localScale = new Vector3(buildingScale, buildingScale, buildingScale);

        building.GetComponent<Renderer>().material = buildingMaterial;

        // You can customize the building rotation or other properties here
    }
}
