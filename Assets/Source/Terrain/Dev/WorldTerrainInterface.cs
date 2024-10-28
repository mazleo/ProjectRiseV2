using ProjectRise.ProceduralGeneration.External;
using ProjectRise.World.External;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectRise.Terrain.Dev
{
    public class WorldTerrainInterface : MonoBehaviour
    {
        private TerrainModel _terrainModel;
        private GameWorldModel _gameWorldModel;

        private void Start()
        {
            _gameWorldModel = new GameWorldModel();
            _terrainModel = TerrainModel.GetBuilder(_gameWorldModel).Build();
            TransformBaseHeightModel(_terrainModel, _gameWorldModel);
            ITerrainMeshGenerator meshGenerator = new WorldTerrainMeshGenerator(
                _terrainModel,
                _gameWorldModel
            );
            Mesh[] meshes = meshGenerator.Generate( /*mesh=*/
                null
            );
            GetComponent<MeshFilter>().mesh = meshes[0];
            for (int m = 1; m < meshes.Length; m++)
            {
                GameObject newMeshGameObject = new GameObject();
                newMeshGameObject.AddComponent<MeshRenderer>();
                newMeshGameObject.AddComponent<MeshFilter>();
                newMeshGameObject.GetComponent<MeshRenderer>().material = gameObject
                    .GetComponent<MeshRenderer>()
                    .material;
                newMeshGameObject.transform.SetParent(gameObject.transform);
                newMeshGameObject.GetComponent<MeshFilter>().mesh = meshes[m];
            }
        }

        private void TransformBaseHeightModel(
            TerrainModel terrainModel,
            GameWorldModel gameWorldModel
        )
        {
            PerlinModel model = PerlinModel
                .GetBuilder()
                .Lacunarity(0.03F)
                .XOffset(Random.Range(0, 10000))
                .ZOffset(Random.Range(0, 10000))
                .Build();
            float volumetricTileSize =
                (terrainModel.TileSize * gameWorldModel.VolumeCameraDimensions.Value.x)
                / gameWorldModel.Width;
            for (int b = 0; b < terrainModel.HorizontalTiles * terrainModel.VerticalTiles; b++)
            {
                float x = (b % terrainModel.HorizontalTiles) * model.Lacunarity + model.XOffset;
                float z =
                    Mathf.Floor(b / terrainModel.HorizontalTiles) * model.Lacunarity
                    + model.ZOffset;
                float y =
                    Mathf.Floor(Mathf.PerlinNoise(x, z) / volumetricTileSize) * volumetricTileSize;
                terrainModel.BaseHeightModel[b] = y;
            }
        }
    }
}
