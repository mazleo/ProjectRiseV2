using System;
using ProjectRise.ProceduralGeneration.External;
using ProjectRise.World.External;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectRise.Terrain.Transformer.TectonicPlate.Dev
{
    internal class TectonicPlateInterface : MonoBehaviour
    {
        [SerializeField] private bool update = false;
        
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
            PerlinModel model = PerlinModel.GetBuilder().Lacunarity(0.013F).XOffset(Random.Range(100, 99999)).ZOffset(Random.Range(100, 99999)).Build();
            Mesh mesh = MeshGenerator.GenerateMesh(_terrainModel);
            GetComponent<MeshFilter>().mesh = mesh;
            Texture2D texture = TextureGenerator.GeneratePlateTextureFromModel(model, _terrainModel);
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
        }
    }
}