using ProjectRise.ProceduralGeneration.External;
using ProjectRise.World.External;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectRise.Terrain.Transformer.TectonicPlate.Dev
{
    /// <summary>
    /// Tectonic plate game object.
    /// </summary>
    internal class TectonicPlateInterface : MonoBehaviour
    {
        [SerializeField]
        private bool update = false;

        private GameWorldModel _gameWorldModel;
        private TerrainModel _terrainModel;

        private void Start()
        {
            _gameWorldModel = new GameWorldModel();
            _terrainModel = TerrainModel.GetBuilder(_gameWorldModel).Build();
            GenerateNewMesh();
        }

        private void Update()
        {
            if (update)
                GenerateNewMesh();
            update = false;
        }

        private void GenerateNewMesh()
        {
            PerlinModel perlinModel = PerlinModel
                .GetBuilder()
                .Lacunarity(0.1F)
                .XOffset(Random.Range(100, 99999))
                .ZOffset(Random.Range(100, 99999))
                .Build();

            TectonicPlateModel tectonicPlateModel = new TectonicPlateModel(
                perlinModel,
                _terrainModel
            );

            TransformTerrainModel(tectonicPlateModel);
            ITerrainMeshGenerator meshGenerator = new WorldTerrainMeshGenerator(
                _terrainModel,
                _gameWorldModel
            );
            Mesh[] meshes = meshGenerator.Generate(null);

            if (gameObject.transform.childCount > 0)
            {
                for (int c = 0; c < gameObject.transform.childCount; c++)
                {
                    GameObject child = gameObject.transform.GetChild(c).gameObject;
                    Destroy(child);
                }
                gameObject.transform.DetachChildren();
            }

            Material material = GetComponent<MeshRenderer>().material;
            GetComponent<MeshFilter>().mesh = meshes[0];

            for (int m = 1; m < meshes.Length; m++)
            {
                GameObject meshGameObject = new GameObject();
                meshGameObject.AddComponent<MeshFilter>();
                meshGameObject.AddComponent<MeshRenderer>();

                meshGameObject.GetComponent<MeshFilter>().mesh = meshes[m];
                meshGameObject.GetComponent<MeshRenderer>().material = material;

                meshGameObject.transform.parent = gameObject.transform;
            }
        }

        private void TransformTerrainModel(TectonicPlateModel tectonicPlateModel)
        {
            for (int b = 0; b < _terrainModel.BaseHeightModel.Length; b++)
            {
                int tectonicPlateId = tectonicPlateModel.PlateMap[b];
                TectonicPlate plate = tectonicPlateModel.Plates[tectonicPlateId];
                float baseHeight;
                if (plate.Type == PlateType.Continental)
                    baseHeight = 1F;
                else
                    baseHeight = 0F;
                _terrainModel.BaseHeightModel[b] = baseHeight;
            }
        }
    }
}
