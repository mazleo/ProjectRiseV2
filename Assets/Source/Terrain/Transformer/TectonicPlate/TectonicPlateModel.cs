using System;
using System.Collections.Generic;
using ModestTree;
using ProjectRise.ProceduralGeneration.External;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectRise.Terrain.Transformer.TectonicPlate
{
    /// <summary>
    /// Model for the worlds tectonic plates.
    /// </summary>
    internal class TectonicPlateModel
    {
        private const float PlateThreshold = 0.7F;
        private const int MaxRecursionDepth = 100;

        internal PerlinModel Model;
        internal TerrainModel TerrainModel;

        internal TectonicPlate[] Plates;
        internal int[] PlateMap;

        private int _lastId = -1;
        private PlateType _lastPlateType =
            Random.Range(0F, 1F) > 0.5F ? PlateType.Continental : PlateType.Oceanic;

        internal TectonicPlateModel(PerlinModel model, TerrainModel terrainModel)
        {
            ValidateInputs(model, terrainModel);
            Model = model;
            TerrainModel = terrainModel;
            Generate();
        }

        private void Generate()
        {
            List<TectonicPlate> plates = new List<TectonicPlate>();
            InitializePlateMap();
            bool[] visitedMap = new bool[PlateMap.Length];
            for (int p = 0; p < PlateMap.Length; p++)
            {
                if (GetYSample(p) >= PlateThreshold && !visitedMap[p])
                {
                    _lastId++;
                    plates.Add(new TectonicPlate(_lastId, GetPlateTypeAssignment()));
                    Queue<int> indexQueue = new Queue<int>();
                    indexQueue.Enqueue(p);
                    while (!indexQueue.IsEmpty())
                        SetPlateTypes(_lastId, indexQueue, visitedMap, 0);
                }
            }
            Plates = plates.ToArray();
            ExpandPlates();
        }

        private void ExpandPlates()
        {
            Queue<int> indexQueue = new Queue<int>();
            Queue<int> nextQueue = new Queue<int>();
            for (int q = 0; q < PlateMap.Length; q++)
                if (!IsUnset(q) && HasUnsetNeighbors(q))
                    indexQueue.Enqueue(q);
            while (!indexQueue.IsEmpty() || !nextQueue.IsEmpty())
                ExpandPlatesBfs(indexQueue, nextQueue, 0);
        }

        private void ExpandPlatesBfs(
            Queue<int> indexQueue,
            Queue<int> nextQueue,
            int recursionDepth
        )
        {
            if (
                (indexQueue.IsEmpty() && nextQueue.IsEmpty()) || recursionDepth >= MaxRecursionDepth
            )
                return;

            while (!nextQueue.IsEmpty())
                indexQueue.Enqueue(nextQueue.Dequeue());

            while (!indexQueue.IsEmpty())
            {
                int index = indexQueue.Dequeue();
                int id = PlateMap[index];
                int[] neighbors = GetNeighbors(index, IsUnset);
                for (int n = 0; n < neighbors.Length; n++)
                {
                    PlateMap[neighbors[n]] = id;
                    nextQueue.Enqueue(neighbors[n]);
                }
            }

            recursionDepth++;
            ExpandPlatesBfs(indexQueue, nextQueue, recursionDepth);
        }

        private void InitializePlateMap()
        {
            PlateMap = new int[TerrainModel.BaseHeightModel.Length];
            for (int p = 0; p < PlateMap.Length; p++)
                PlateMap[p] = -1;
        }

        private void SetPlateTypes(
            int id,
            Queue<int> indexQueue,
            bool[] visitedMap,
            int recursionDepth
        )
        {
            if (indexQueue.Count == 0 || recursionDepth >= MaxRecursionDepth)
                return;

            int index = (int)indexQueue.Dequeue();
            PlateMap[index] = id;
            visitedMap[index] = true;

            Func<int, bool> isPlate = neighborIndex =>
                GetYSample(neighborIndex) >= PlateThreshold && !visitedMap[neighborIndex];

            int[] neighbors = GetNeighbors(index, isPlate);
            for (int n = 0; n < neighbors.Length; n++)
            {
                indexQueue.Enqueue(neighbors[n]);
                visitedMap[neighbors[n]] = true;
            }

            recursionDepth++;
            SetPlateTypes(id, indexQueue, visitedMap, recursionDepth);
        }

        private float GetYSample(int index)
        {
            float x = ((index % TerrainModel.HorizontalTiles) * Model.Lacunarity) + Model.XOffset;
            float z =
                (Mathf.Floor(index / TerrainModel.HorizontalTiles) * Model.Lacunarity)
                + Model.ZOffset;
            return Mathf.PerlinNoise(x, z);
        }

        private int[] GetNeighbors(int index, Func<int, bool> passesCondition)
        {
            int r = Mathf.FloorToInt(index / TerrainModel.HorizontalTiles);
            int c = index % TerrainModel.HorizontalTiles;
            List<int> neighbors = new List<int>();

            int topLeftIndex = GetPlateMapIndex(r + 1, c - 1);
            if (IsValidIndex(topLeftIndex) && passesCondition(topLeftIndex))
                neighbors.Add(topLeftIndex);
            int topIndex = GetPlateMapIndex(r + 1, c);
            if (IsValidIndex(topIndex) && passesCondition(topIndex))
                neighbors.Add(topIndex);
            int topRightIndex = GetPlateMapIndex(r + 1, c + 1);
            if (IsValidIndex(topRightIndex) && passesCondition(topRightIndex))
                neighbors.Add(topRightIndex);
            int leftIndex = GetPlateMapIndex(r, c - 1);
            if (IsValidIndex(leftIndex) && passesCondition(leftIndex))
                neighbors.Add(leftIndex);
            int rightIndex = GetPlateMapIndex(r, c + 1);
            if (IsValidIndex(rightIndex) && passesCondition(rightIndex))
                neighbors.Add(rightIndex);
            int bottomLeftIndex = GetPlateMapIndex(r - 1, c - 1);
            if (IsValidIndex(bottomLeftIndex) && passesCondition(bottomLeftIndex))
                neighbors.Add(bottomLeftIndex);
            int bottomIndex = GetPlateMapIndex(r - 1, c);
            if (IsValidIndex(bottomIndex) && passesCondition(bottomIndex))
                neighbors.Add(bottomIndex);
            int bottomRightIndex = GetPlateMapIndex(r - 1, c + 1);
            if (IsValidIndex(bottomRightIndex) && passesCondition(bottomRightIndex))
                neighbors.Add(bottomRightIndex);

            return neighbors.ToArray();
        }

        private bool HasUnsetNeighbors(int index)
        {
            int[] neighbors = GetNeighbors(index, IsUnset);
            return neighbors.Length > 0;
        }

        private bool IsUnset(int index)
        {
            return PlateMap[index] == -1;
        }

        private int GetPlateMapIndex(int r, int c)
        {
            return (r * TerrainModel.HorizontalTiles) + c;
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < PlateMap.Length;
        }

        private PlateType GetPlateTypeAssignment()
        {
            if (_lastPlateType == PlateType.Continental)
            {
                _lastPlateType = PlateType.Oceanic;
                return PlateType.Oceanic;
            }
            _lastPlateType = PlateType.Continental;
            return PlateType.Continental;
        }

        private static void ValidateInputs(PerlinModel perlinModel, TerrainModel terrainModel)
        {
            if (perlinModel == null || terrainModel == null)
                throw new ArgumentException("Inputs should not be null.");
        }
    }
}
