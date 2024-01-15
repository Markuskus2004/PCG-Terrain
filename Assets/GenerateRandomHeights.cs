using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



// Defining serializable classes for terrain textures, trees, and vegetation data
[System.Serializable]
public class TerrainTextureData
{
    public Texture2D terrainTexture;
    public Vector2 tileSize;
    public float minHeight;
    public float maxHeight;
}

[System.Serializable]
public class TreeData
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}

[System.Serializable]
public class VegetationData
{
    public GameObject vegetationMesh;
    public float minHeight;
    public float maxHeight;
}

public class GenerateRandomHeights : MonoBehaviour
{
    // Terrain and terrain data references
    private Terrain terrain;
    private TerrainData terrainData;

    // Variables for random height generation
    [SerializeField]
    [Range(0f, 1f)]
    private float minRandomHeightRange = 0f; // Minimum height range for random terrain

    [SerializeField]
    [Range(0f, 1f)]
    private float maxRandomHeightRange = 0.1f; // Maximum height range for random terrain

    [SerializeField]
    private bool flattenTerrain = true; // Option to flatten terrain on destruction

    // Variables for Perlin noise settings
    [Header("Perlin Noise")]
    [SerializeField]
    private bool perlinNoise = false; // Toggle for using Perlin noise

    [SerializeField]
    private float perlinNoiseWidthScale = 0.01f; // Scale factor for Perlin noise width

    [SerializeField]
    private float perlinNoiseHeightScale = 0.01f; // Scale factor for Perlin noise height

    // Variables for terrain texture data
    [Header("Texture Data")]
    [SerializeField]
    private List<TerrainTextureData> terrainTextureData; // List of terrain textures

    [SerializeField]
    private bool addTerrainTexture = false; // Toggle for adding terrain textures

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f; // Blend offset for texture transition

    // Variables for tree data
    [Header("Tree Data")]
    [SerializeField]
    private List<TreeData> treeData; // List of tree data

    [SerializeField]
    private int maxTrees = 10000; // Maximum number of trees

    [SerializeField]
    private int treeSpacing = 10; // Spacing between trees

    [SerializeField]
    private bool addTrees = false; // Toggle for adding trees

    [SerializeField]
    private int terrainLayerIndex; // Layer index for terrain

    // Variables for vegetation data
    [Header("Vegetation Data")]
    [SerializeField]
    private List<VegetationData> vegetationData; // List of vegetation data

    [SerializeField]
    private int maxVegetations = 100; // Maximum number of vegetations

    [SerializeField]
    private int vegetationSpacing = 10; // Spacing between vegetations

    [SerializeField]
    private bool addVegetation = false; // Toggle for adding vegetation

    // Variables for water settings
    [Header("Water")]
    [SerializeField]
    private GameObject water; // Water prefab

    [SerializeField]
    private float waterHeight = 0.35f; // Height of water relative to terrain

    // Variables for cloud settings
    [Header("Cloud")]
    [SerializeField]
    private GameObject dust; // Cloud/ dust prefab
    [SerializeField]
    private float cloudBaseHeight = 1f; // Base height for clouds
    [SerializeField]
    private int dustSpacing = 250; // Spacing between clouds

    // Variables for rain settings
    [Header("Rain")]
    [SerializeField]
    private GameObject rain; // Rain prefab

    [SerializeField]
    public GameObject cloudPrefab; // Assign your cloud prefab in the inspector
    public int cloudCount = 10; // Number of clouds to spawn
    public Vector3 spawnArea = new Vector3(50, 50, 50); // Define the spawn area size

    [SerializeField]
    public Cubemap skyPrefab; // Assign your cloud prefab in the inspector

    // Start is called before the first frame update
    void Start()
    {
        // Initialize terrain and terrain data
        if (terrain == null)
        {
            terrain = this.GetComponent<Terrain>();
        }

        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }

        GenerateHeights();
        AddTerrainTextures();
        AddTrees();
        AddWater();
        SpawnClouds();
        AddSky();
    }



    // Method to generate random heights for terrain
    void GenerateHeights()
    {
        // Create a height map array based on terrain resolution
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        // Loop through the height map array to assign heights
        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                if (perlinNoise)
                {
                    // Use Perlin noise to generate heights if enabled
                    heightMap[width, height] = Mathf.PerlinNoise(width * perlinNoiseWidthScale, height * perlinNoiseHeightScale);
                }
                else
                {
                    // Otherwise, use random range for heights
                    heightMap[width, height] = UnityEngine.Random.Range(minRandomHeightRange, maxRandomHeightRange);
                }
            }
        }
        // Set the generated heights to the terrain
        terrainData.SetHeights(0, 0, heightMap);
    }

    // Method to flatten the terrain
    void FlattenTerrain()
    {
        // Create a height map array with all zeroes (flat terrain)
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        // Loop through the height map array to assign flat heights
        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                heightMap[width, height] = 0;
            }
        }

        // Set the flat heights to the terrain
        terrainData.SetHeights(0, 0, heightMap);
    }

    // Method to add terrain textures
    private void AddTerrainTextures()
    {
        // Create an array of terrain layers based on the number of textures
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureData.Count];

        // Loop through each texture data to create terrain layers
        for (int i = 0; i < terrainTextureData.Count; i++)
        {
            if (addTerrainTexture)
            {
                // If adding terrain texture is enabled, set texture and tile size
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = terrainTextureData[i].terrainTexture;
                terrainLayers[i].tileSize = terrainTextureData[i].tileSize;
            }
            else
            {
                // Otherwise, create an empty terrain layer
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;
            }
        }

        // Assign the created terrain layers to the terrain data
        terrainData.terrainLayers = terrainLayers;

        // Retrieve the height map from terrain data
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        // Create an alphamap array based on terrain alphamap dimensions
        float[,,] alphamapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        // Loop through the alphamap array to assign texture based on height
        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] alphamap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureData.Count; i++)
                {
                    float heightBegin = terrainTextureData[i].minHeight - terrainTextureBlendOffset;
                    float heightEnd = terrainTextureData[i].maxHeight + terrainTextureBlendOffset;

                    // Assign texture based on height range
                    if (heightMap[width, height] >= heightBegin && heightMap[width, height] <= heightEnd)
                    {
                        alphamap[i] = 1;
                    }
                }

                // Blend the alphamap values
                Blend(alphamap);

                // Assign the blended alphamap values to the alphamap array
                for (int j = 0; j < terrainTextureData.Count; j++)
                {
                    alphamapList[width, height, j] = alphamap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphamapList);

    }

    // Method to blend alphamap values
    private void Blend(float[] alphamap)
    {
        float total = 0;

        for (int i = 0; i < alphamap.Length; i++)
        {
            total += alphamap[i];
        }

        for (int i = 0; i < alphamap.Length; i++)
        {
            alphamap[i] = alphamap[i] / total;
        }
    }

    void SpawnClouds()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                400f,
                Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
            );

            Instantiate(cloudPrefab, transform.position + randomPosition, Quaternion.identity);
        }
    }
    private void AddWater()
    {
        GameObject waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
        waterGameObject.name = "water";
        waterGameObject.transform.position = this.transform.position + new Vector3(terrainData.size.x / 2, waterHeight * terrainData.size.y,
        terrainData.size.z / 2);
        waterGameObject.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }
    private void AddSky()
    {
        Material skyboxMaterial = new Material(Shader.Find("Skybox/Cubemap"));
        skyboxMaterial.SetTexture("_Tex", skyPrefab);
        RenderSettings.skybox = skyboxMaterial;
    }

    // Method to add trees to the terrain
    private void AddTrees()
    {
        // Create an array of tree prototypes based on tree data count
        TreePrototype[] trees = new TreePrototype[treeData.Count];

        // Loop through each tree data to create tree prototypes
        for (int i = 0; i < treeData.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeData[i].treeMesh;
        }

        // Assign the tree prototypes to the terrain data
        terrainData.treePrototypes = trees;

        // Create a list to store tree instances
        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        if (addTrees)
        {
            // If adding trees is enabled, loop through the terrain to place trees
            for (int z = 0; z < terrainData.size.z; z += treeSpacing)
            {
                for (int x = 0; x < terrainData.size.x; x += treeSpacing)
                {
                    for (int treeIndex = 0; treeIndex < trees.Length; treeIndex++)
                    {
                        if (treeInstanceList.Count < maxTrees)
                        {
                            // Calculate the current height and check if it's within the tree height range
                            float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y; // this is going to give us a height value between 0 and 1

                            if (currentHeight >= treeData[treeIndex].minHeight && currentHeight <= treeData[treeIndex].maxHeight)
                            {
                                // Calculate random position for the tree
                                float randomX = (x + UnityEngine.Random.Range(-5.0f, 5.0f)) / terrainData.size.x;

                                float randomZ = (z + UnityEngine.Random.Range(-5.0f, 5.0f)) / terrainData.size.z;

                                Vector3 treePosition = new Vector3(randomX * terrainData.size.x,
                                                                   currentHeight * terrainData.size.y,
                                                                   randomZ * terrainData.size.z) + this.transform.position;

                                RaycastHit raycastHit;

                                int layerMask = 1 << terrainLayerIndex;

                                // Raycast to check if the position is suitable for placing a tree
                                if (Physics.Raycast(treePosition, -Vector3.up, out raycastHit, 100, layerMask) ||
                                    Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                                {
                                    float treeDistance = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                    // Create and configure the tree instance
                                    TreeInstance treeInstance = new TreeInstance();

                                    treeInstance.position = new Vector3(randomX, treeDistance, randomZ);
                                    treeInstance.rotation = UnityEngine.Random.Range(0, 360);
                                    treeInstance.prototypeIndex = treeIndex;
                                    treeInstance.color = Color.white;
                                    treeInstance.lightmapColor = Color.white;
                                    treeInstance.heightScale = 0.95f;
                                    treeInstance.widthScale = 0.95f;

                                    // Add the tree instance to the list
                                    treeInstanceList.Add(treeInstance);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Assign the tree instances to the terrain data
        terrainData.treeInstances = treeInstanceList.ToArray();

    }

    // Method called when the script is destroyed
    private void OnDestroy()
    {
        if (flattenTerrain)
        {
            FlattenTerrain(); // Flatten the terrain if the flattenTerrain flag is true
        }
    }

}