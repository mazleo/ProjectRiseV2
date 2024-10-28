using System;
using System.Collections.Generic;
using ProjectRise.World.External;
using UnityEngine;

namespace ProjectRise.Terrain
{
    internal class WorldTerrainMeshGenerator : ITerrainMeshGenerator
    {
        private const int NumVerticesFace = 6;

        private TerrainModel _terrainModel;
        private GameWorldModel _gameWorldModel;

        internal WorldTerrainMeshGenerator(TerrainModel terrainModel, GameWorldModel gameWorldModel)
        {
            _terrainModel = terrainModel;
            _gameWorldModel = gameWorldModel;
        }

        Mesh[] ITerrainMeshGenerator.Generate(Mesh mesh)
        {
            List<Mesh> meshes = new List<Mesh>();
            meshes.Add(GenerateTopFaceMesh());
            meshes.Add(GenerateNorthFaceMesh());
            meshes.Add(GenerateSouthFaceMesh());
            meshes.Add(GenerateEastFaceMesh());
            meshes.Add(GenerateWestFaceMesh());
            return meshes.ToArray();
        }

        private Mesh GenerateWestFaceMesh()
        {
            List<Vector3> vertices = new List<Vector3>();
            float xOffset = _gameWorldModel.VolumeCameraDimensions.Value.x / 2;
            float zOffset = _gameWorldModel.VolumeCameraDimensions.Value.z / 2;
            int xTiles = (int)(_gameWorldModel.Width / _terrainModel.TileSize);
            float volumetricTileSize =
                (_terrainModel.TileSize * _gameWorldModel.VolumeCameraDimensions.Value.x)
                / _gameWorldModel.Width;
            for (int b = 0; b < _terrainModel.BaseHeightModel.Length; b++)
            {
                int row = Mathf.FloorToInt(b / xTiles);
                int column = b % xTiles;
                int rightColumn = column + 1;
                if (rightColumn >= xTiles)
                    continue;
                int rightBaseHeightIndex = (row * xTiles) + rightColumn;
                float baseHeight = GetBaseHeight(b);
                float rightBaseHeight = GetBaseHeight(rightBaseHeightIndex);
                if (rightBaseHeight > baseHeight)
                {
                    float x = rightColumn * volumetricTileSize - xOffset;
                    float y = baseHeight;
                    float z = (row + 1) * volumetricTileSize - zOffset;
                    Vector3[] westFaceVertices = GenerateWestFace(
                        new Vector3(x, y, z),
                        volumetricTileSize,
                        rightBaseHeight
                    );
                    vertices.AddRange(westFaceVertices);
                }
            }
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = GenerateTriangles(mesh.vertices);
            return mesh;
        }

        private Mesh GenerateEastFaceMesh()
        {
            List<Vector3> vertices = new List<Vector3>();
            float xOffset = _gameWorldModel.VolumeCameraDimensions.Value.x / 2;
            float zOffset = _gameWorldModel.VolumeCameraDimensions.Value.z / 2;
            int xTiles = (int)(_gameWorldModel.Width / _terrainModel.TileSize);
            float volumetricTileSize =
                (_terrainModel.TileSize * _gameWorldModel.VolumeCameraDimensions.Value.x)
                / _gameWorldModel.Width;
            for (int b = 0; b < _terrainModel.BaseHeightModel.Length; b++)
            {
                int row = Mathf.FloorToInt(b / xTiles);
                int column = b % xTiles;
                int rightColumn = column + 1;
                if (rightColumn >= xTiles)
                    continue;
                int rightBaseHeightIndex = (row * xTiles) + rightColumn;
                float baseHeight = GetBaseHeight(b);
                float rightBaseHeight = GetBaseHeight(rightBaseHeightIndex);
                if (baseHeight > rightBaseHeight)
                {
                    float x = rightColumn * volumetricTileSize - xOffset;
                    float y = rightBaseHeight;
                    float z = row * volumetricTileSize - zOffset;
                    Vector3[] eastFaceVertices = GenerateEastFace(
                        new Vector3(x, y, z),
                        volumetricTileSize,
                        baseHeight
                    );
                    vertices.AddRange(eastFaceVertices);
                }
            }
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = GenerateTriangles(mesh.vertices);
            return mesh;
        }

        private Mesh GenerateNorthFaceMesh()
        {
            List<Vector3> vertices = new List<Vector3>();
            float xOffset = _gameWorldModel.VolumeCameraDimensions.Value.x / 2;
            float zOffset = _gameWorldModel.VolumeCameraDimensions.Value.z / 2;
            int xTiles = (int)(_gameWorldModel.Width / _terrainModel.TileSize);
            int zTiles = (int)(_gameWorldModel.Length / _terrainModel.TileSize);
            float volumetricTileSize =
                (_terrainModel.TileSize * _gameWorldModel.VolumeCameraDimensions.Value.x)
                / _gameWorldModel.Width;
            for (int b = 0; b < _terrainModel.BaseHeightModel.Length; b++)
            {
                int row = Mathf.FloorToInt(b / xTiles);
                int column = b % xTiles;
                int aboveRow = row + 1;
                if (aboveRow >= zTiles)
                    break;
                int aboveBaseHeightIndex = (aboveRow * xTiles) + column;
                float baseHeight = GetBaseHeight(b);
                float aboveBaseHeight = GetBaseHeight(aboveBaseHeightIndex);
                if (baseHeight > aboveBaseHeight)
                {
                    float x = (column + 1) * volumetricTileSize - xOffset;
                    float y = aboveBaseHeight;
                    float z = (row + 1) * volumetricTileSize - zOffset;
                    Vector3[] northFaceVertices = GenerateNorthFace(
                        new Vector3(x, y, z),
                        volumetricTileSize,
                        baseHeight
                    );
                    vertices.AddRange(northFaceVertices);
                }
            }
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = GenerateTriangles(mesh.vertices);
            return mesh;
        }

        private Mesh GenerateSouthFaceMesh()
        {
            List<Vector3> vertices = new List<Vector3>();
            float xOffset = _gameWorldModel.VolumeCameraDimensions.Value.x / 2;
            float zOffset = _gameWorldModel.VolumeCameraDimensions.Value.z / 2;
            int xTiles = (int)(_gameWorldModel.Width / _terrainModel.TileSize);
            int zTiles = (int)(_gameWorldModel.Length / _terrainModel.TileSize);
            float volumetricTileSize =
                (_terrainModel.TileSize * _gameWorldModel.VolumeCameraDimensions.Value.x)
                / _gameWorldModel.Width;
            for (int b = 0; b < _terrainModel.BaseHeightModel.Length; b++)
            {
                int row = Mathf.FloorToInt(b / xTiles);
                int column = b % xTiles;
                int aboveRow = row + 1;
                if (aboveRow >= zTiles)
                    break;
                int aboveBaseHeightIndex = (aboveRow * xTiles) + column;
                float baseHeight = GetBaseHeight(b);
                float aboveBaseHeight = GetBaseHeight(aboveBaseHeightIndex);
                if (aboveBaseHeight > baseHeight)
                {
                    float x = column * volumetricTileSize - xOffset;
                    float y = baseHeight;
                    float z = (row + 1) * volumetricTileSize - zOffset;
                    Vector3[] southFaceVertices = GenerateSouthFace(
                        new Vector3(x, y, z),
                        volumetricTileSize,
                        aboveBaseHeight
                    );
                    vertices.AddRange(southFaceVertices);
                }
            }
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = GenerateTriangles(mesh.vertices);
            return mesh;
        }

        private Mesh GenerateTopFaceMesh()
        {
            Vector3[] vertices = new Vector3[
                _terrainModel.BaseHeightModel.Length * NumVerticesFace
            ];
            float xOffset = _gameWorldModel.VolumeCameraDimensions.Value.x / 2;
            float zOffset = _gameWorldModel.VolumeCameraDimensions.Value.z / 2;
            int xTiles = (int)(_gameWorldModel.Width / _terrainModel.TileSize);
            float volumetricTileSize =
                (_terrainModel.TileSize * _gameWorldModel.VolumeCameraDimensions.Value.x)
                / _gameWorldModel.Width;
            for (int i = 0; i < _terrainModel.BaseHeightModel.Length; i++)
            {
                int row = Mathf.FloorToInt(i / xTiles);
                int column = i % xTiles;
                float x = column * volumetricTileSize - xOffset;
                float y = GetBaseHeight(i);
                float z = row * volumetricTileSize - zOffset;
                Vector3 originVector = new Vector3(x, y, z);
                Vector3[] topFaceVertices = GenerateTopFace(originVector, volumetricTileSize);
                Array.Copy(
                    topFaceVertices, /* sourceIndex */
                    0,
                    vertices, /*destinationIndex*/
                    i * NumVerticesFace, /*length*/
                    NumVerticesFace
                );
            }
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = GenerateTriangles(mesh.vertices);
            return mesh;
        }

        private int[] GenerateTriangles(Vector3[] vertices)
        {
            int[] triangles = new int[vertices.Length];
            for (int v = 0; v < vertices.Length; v++)
                triangles[v] = v;
            return triangles;
        }

        private Vector3[] GenerateTopFace(Vector3 originVertex, float tileSize)
        {
            Vector3[] vertices = new Vector3[NumVerticesFace];
            vertices[0] = originVertex;
            vertices[1] = new Vector3(originVertex.x, originVertex.y, originVertex.z + tileSize);
            vertices[2] = new Vector3(
                originVertex.x + tileSize,
                originVertex.y,
                originVertex.z + tileSize
            );
            vertices[3] = new Vector3(originVertex.x, originVertex.y, originVertex.z);
            vertices[4] = new Vector3(
                originVertex.x + tileSize,
                originVertex.y,
                originVertex.z + tileSize
            );
            vertices[5] = new Vector3(originVertex.x + tileSize, originVertex.y, originVertex.z);
            return vertices;
        }

        private Vector3[] GenerateSouthFace(Vector3 originVertex, float tileSize, float highY)
        {
            Vector3[] vertices = new Vector3[NumVerticesFace];
            vertices[0] = originVertex;
            vertices[1] = new Vector3(originVertex.x, highY, originVertex.z);
            vertices[2] = new Vector3(originVertex.x + tileSize, highY, originVertex.z);
            vertices[3] = new Vector3(originVertex.x, originVertex.y, originVertex.z);
            vertices[4] = new Vector3(originVertex.x + tileSize, highY, originVertex.z);
            vertices[5] = new Vector3(originVertex.x + tileSize, originVertex.y, originVertex.z);
            return vertices;
        }

        private Vector3[] GenerateEastFace(Vector3 originVertex, float tileSize, float highY)
        {
            Vector3[] vertices = new Vector3[NumVerticesFace];
            vertices[0] = originVertex;
            vertices[1] = new Vector3(originVertex.x, highY, originVertex.z);
            vertices[2] = new Vector3(originVertex.x, highY, originVertex.z + tileSize);
            vertices[3] = new Vector3(originVertex.x, originVertex.y, originVertex.z);
            vertices[4] = new Vector3(originVertex.x, highY, originVertex.z + tileSize);
            vertices[5] = new Vector3(originVertex.x, originVertex.y, originVertex.z + tileSize);
            return vertices;
        }

        private Vector3[] GenerateWestFace(Vector3 originVertex, float tileSize, float highY)
        {
            Vector3[] vertices = new Vector3[NumVerticesFace];
            vertices[0] = originVertex;
            vertices[1] = new Vector3(originVertex.x, highY, originVertex.z);
            vertices[2] = new Vector3(originVertex.x, highY, originVertex.z - tileSize);
            vertices[3] = new Vector3(originVertex.x, originVertex.y, originVertex.z);
            vertices[4] = new Vector3(originVertex.x, highY, originVertex.z - tileSize);
            vertices[5] = new Vector3(originVertex.x, originVertex.y, originVertex.z - tileSize);
            return vertices;
        }

        private Vector3[] GenerateNorthFace(Vector3 originVertex, float tileSize, float highY)
        {
            Vector3[] vertices = new Vector3[NumVerticesFace];
            vertices[0] = originVertex;
            vertices[1] = new Vector3(originVertex.x, highY, originVertex.z);
            vertices[2] = new Vector3(originVertex.x - tileSize, highY, originVertex.z);
            vertices[3] = new Vector3(originVertex.x, originVertex.y, originVertex.z);
            vertices[4] = new Vector3(originVertex.x - tileSize, highY, originVertex.z);
            vertices[5] = new Vector3(originVertex.x - tileSize, originVertex.y, originVertex.z);
            return vertices;
        }

        private float GetBaseHeight(int b)
        {
            float volumetricTileSize =
                (_terrainModel.TileSize * _gameWorldModel.VolumeCameraDimensions.Value.x)
                / _gameWorldModel.Width;
            if (volumetricTileSize >= 1)
                return Mathf.Floor(_terrainModel.BaseHeightModel[b] / volumetricTileSize)
                    * volumetricTileSize;
            else
            {
                float numTiles = Mathf.Floor(
                    (
                        _terrainModel.BaseHeightModel[b]
                        * _gameWorldModel.VolumeCameraDimensions.Value.x
                    ) / volumetricTileSize
                );
                return numTiles * volumetricTileSize;
            }
        }
    }
}
