using ProjectRise.ProceduralGeneration.External;
using ProjectRise.Terrain.Transformer.Alpine;
using ProjectRise.Terrain.Transformer.Landform;
using ProjectRise.World.External;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectRise.Terrain.Transformer.TectonicPlate.Dev
{
    /// <summary>
    /// landform game object for development purposes.
    /// </summary>
    internal class AlpineFormationInterface : MonoBehaviour
    {
        [SerializeField]
        private bool update;

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

            LandFormationModel landFormationModel = new LandFormationModel(
                tectonicPlateModel,
                _terrainModel,
                _gameWorldModel
            );

            AlpineFormationModel alpineFormationModel = new AlpineFormationModel(
                tectonicPlateModel,
                _terrainModel,
                _gameWorldModel
            );

            TransformTerrainModel(landFormationModel, alpineFormationModel);
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
            float snowThreshold =
                _gameWorldModel.VolumeCameraDimensions.Value.y
                - (
                    ((1F - (_gameWorldModel.LandformHeight / _gameWorldModel.Height)) * 0.2F)
                    * _gameWorldModel.VolumeCameraDimensions.Value.y
                );
            float alpineThreshold =
                _gameWorldModel.VolumeCameraDimensions.Value.y
                - (
                    ((1F - (_gameWorldModel.LandformHeight / _gameWorldModel.Height)) * 0.8F)
                    * _gameWorldModel.VolumeCameraDimensions.Value.y
                );
            float landformThreshold =
                (_gameWorldModel.LandformHeight / _gameWorldModel.Height)
                * _gameWorldModel.VolumeCameraDimensions.Value.y;
            material.SetFloat("_SnowThreshold", snowThreshold);
            material.SetFloat("_AlpineThreshold", alpineThreshold);
            material.SetFloat("_LandformThreshold", landformThreshold);
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

        private void TransformTerrainModel(
            LandFormationModel landFormationModel,
            AlpineFormationModel alpineFormationModel
        )
        {
            for (int b = 0; b < _terrainModel.BaseHeightModel.Length; b++)
            {
                float volumetricTileSize =
                    (_terrainModel.TileSize * _gameWorldModel.VolumeCameraDimensions.Value.x)
                    / _gameWorldModel.Width;
                float alpineSlopeIncrease =
                    (
                        (_gameWorldModel.Height - _gameWorldModel.LandformHeight)
                        / _gameWorldModel.Height
                    )
                    * alpineFormationModel.HeightModel[b]
                    * 0.6F;
                float baseHeight =
                    (
                        Mathf.Floor(landFormationModel.HeightModel[b] / volumetricTileSize)
                        * volumetricTileSize
                    ) + alpineSlopeIncrease;
                _terrainModel.BaseHeightModel[b] = baseHeight;
            }
        }
    }
}
