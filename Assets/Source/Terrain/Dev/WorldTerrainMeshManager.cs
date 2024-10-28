using ProjectRise.World.External;
using UnityEngine;
using Void = ProjectRise.Utils.Void;

namespace ProjectRise.Terrain
{
    internal class WorldTerrainMeshManager : ITerrainMeshManager<Void>
    {
        private TerrainModel _terrainModel;
        private GameWorldModel _gameWorldModel;

        private Mesh _terrainMesh;

        internal WorldTerrainMeshManager(TerrainModel terrainModel, GameWorldModel gameWorldModel)
        {
            _terrainModel = terrainModel;
            _gameWorldModel = gameWorldModel;
        }

        Mesh ITerrainMeshManager<Void>.GetMesh()
        {
            throw new System.NotImplementedException();
        }

        Mesh ITerrainMeshManager<Void>.GetMesh(Void args)
        {
            throw new System.NotImplementedException();
        }
    }
}
