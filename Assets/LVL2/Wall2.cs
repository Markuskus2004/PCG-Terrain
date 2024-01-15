using UnityEngine;

public class LandscapeGenerator : MonoBehaviour
{
    public Material mountainMaterial; // Material for mountains
    public Material treeMaterial; // Material for trees
    public int landscapeWidth = 100; // Width of the landscape
    public int landscapeLength = 100; // Length of the landscape
    public int numberOfMountains = 10; // Number of mountains
    public int numberOfTrees = 50; // Number of trees

    void Start()
    {
        GenerateTerrain();
        GenerateMountains();
        GenerateTrees();
    }

    void GenerateTerrain()
    {
        Terrain terrain = gameObject.AddComponent<Terrain>();
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = landscapeWidth + 1;
        terrainData.size = new Vector3(landscapeWidth, 20, landscapeLength);

        terrain.terrainData = terrainData;
    }

    void GenerateMountains()
    {
        for (int i = 0; i < numberOfMountains; i++)
        {
            float x = Random.Range(0, landscapeWidth);
            float z = Random.Range(0, landscapeLength);
            float height = Random.Range(5, 20); // Adjust the height range as needed

            GameObject mountain = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mountain.transform.localScale = new Vector3(1, height, 1);
            mountain.transform.position = new Vector3(x, height / 2f, z);
            mountain.GetComponent<Renderer>().material = mountainMaterial;
        }
    }

    void GenerateTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            float x = Random.Range(0, landscapeWidth);
            float z = Random.Range(0, landscapeLength);
            float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));

            GameObject tree = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tree.transform.localScale = new Vector3(1, Random.Range(5, 10), 1);
            tree.transform.position = new Vector3(x, y + tree.transform.localScale.y / 2f, z);
            tree.GetComponent<Renderer>().material = treeMaterial;
        }
    }
}
