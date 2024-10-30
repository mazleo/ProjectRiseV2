using System;
using System.Collections.Generic;
using ModestTree;
using ProjectRise.Terrain.Transformer.TectonicPlate;
using ProjectRise.Utils;
using ProjectRise.World.External;
using UnityEngine;

namespace ProjectRise.Terrain.Transformer.Landform
{
    /// <summary>
    /// Model for the world landform.
    /// </summary>
    internal class LandFormationModel
    {
        private const int MaxRecursionDepth = 100;
        private const int ContinentalDistanceIndex = 0;
        private const int OceanicDistanceIndex = 1;

        internal float[] HeightModel;
        internal float[] DistanceModel;

        internal TectonicPlateModel TectonicPlateModel;
        internal TerrainModel TerrainModel;
        internal GameWorldModel GameWorldModel;

        internal int MaxDistance = Int32.MinValue;
        internal int MinDistance = Int32.MaxValue;

        internal LandFormationModel(
            TectonicPlateModel tectonicPlateModel,
            TerrainModel terrainModel,
            GameWorldModel gameWorldModel
        )
        {
            ValidateInputs(tectonicPlateModel, terrainModel, gameWorldModel);
            TectonicPlateModel = tectonicPlateModel;
            TerrainModel = terrainModel;
            GameWorldModel = gameWorldModel;
            HeightModel = new float[TectonicPlateModel.PlateMap.Length];
            DistanceModel = new float[TectonicPlateModel.PlateMap.Length];
            Generate();
            ValidateDistanceModel();
            ValidateHeightModel();
        }

        private void ValidateInputs(
            TectonicPlateModel tectonicPlateModel,
            TerrainModel terrainModel,
            GameWorldModel gameWorldModel
        )
        {
            if (tectonicPlateModel == null || terrainModel == null || gameWorldModel == null)
                throw new ArgumentException("No inputs should be null.");
        }

        private void ValidateDistanceModel()
        {
            for (int d = 0; d < DistanceModel.Length; d++)
                if (DistanceModel[d] != 0)
                    return;
            throw new Exception("Generated distance model not valid.");
        }

        private void ValidateHeightModel()
        {
            for (int d = 0; d < HeightModel.Length; d++)
                if (HeightModel[d] != 0)
                    return;
            throw new Exception("Generated height model not valid.");
        }

        private void Generate()
        {
            PopulateDistanceModel();
            PopulateHeightModel();
        }

        private void PopulateHeightModel()
        {
            float waterLevelRatio = GameWorldModel.WaterLevel / GameWorldModel.LandformHeight;
            float landformRatio = 1F - waterLevelRatio;
            float continentalStepSize = landformRatio / MaxDistance;
            float oceanicStepSize = waterLevelRatio / Mathf.Abs(MinDistance);
            for (int h = 0; h < HeightModel.Length; h++)
            {
                float height;
                int plateId = TectonicPlateModel.PlateMap[h];
                PlateType plateType = TectonicPlateModel.Plates[plateId].Type;
                if (plateType == PlateType.Continental)
                    height = (DistanceModel[h] * continentalStepSize) + waterLevelRatio;
                else
                    height =
                        (waterLevelRatio - (Mathf.Abs(MinDistance)) * oceanicStepSize)
                        + ((DistanceModel[h] + Mathf.Abs(MinDistance)) * oceanicStepSize);
                HeightModel[h] = height;
            }
        }

        private void PopulateDistanceModel()
        {
            Queue<int> indexQueue = GetDivergentEdges();
            Queue<int> nextQueue = new Queue<int>();
            bool[] visitedMap = new bool[TectonicPlateModel.PlateMap.Length];
            int[] distances = new int[2];
            while (!indexQueue.IsEmpty() || !nextQueue.IsEmpty())
                distances = PopulateDistanceModelBfs(
                    distances,
                    indexQueue,
                    nextQueue,
                    visitedMap, /*recursionDepth=*/
                    0
                );
        }

        private int[] PopulateDistanceModelBfs(
            int[] distances,
            Queue<int> indexQueue,
            Queue<int> nextQueue,
            bool[] visitedMap,
            int recursionDepth
        )
        {
            if (
                (indexQueue.IsEmpty() && nextQueue.IsEmpty()) || recursionDepth >= MaxRecursionDepth
            )
                return distances;

            bool isNextEdge = false;
            if (indexQueue.IsEmpty())
            {
                for (int n = 0; n < nextQueue.Count; n++)
                    indexQueue.Enqueue(nextQueue.Dequeue());
                isNextEdge = true;
            }

            int index = indexQueue.Dequeue();
            int id = TectonicPlateModel.PlateMap[index];
            DistanceModel[index] =
                TectonicPlateModel.Plates[id].Type == PlateType.Continental
                    ? distances[ContinentalDistanceIndex]
                    : distances[OceanicDistanceIndex];
            if (distances[ContinentalDistanceIndex] > MaxDistance)
                MaxDistance = distances[ContinentalDistanceIndex];
            if (distances[OceanicDistanceIndex] < MinDistance)
                MinDistance = distances[OceanicDistanceIndex];

            if (isNextEdge)
            {
                if (TectonicPlateModel.Plates[id].Type == PlateType.Continental)
                    distances[ContinentalDistanceIndex]++;
                else
                    distances[OceanicDistanceIndex]--;
            }

            int[] neighbors = TerrainUtil.GetNeighbors(
                index,
                neighbor => !visitedMap[neighbor],
                TerrainModel.HorizontalTiles,
                TerrainModel.BaseHeightModel.Length
            );
            for (int n = 0; n < neighbors.Length; n++)
            {
                nextQueue.Enqueue(neighbors[n]);
                visitedMap[neighbors[n]] = true;
            }
            return PopulateDistanceModelBfs(
                distances,
                indexQueue,
                nextQueue,
                visitedMap,
                recursionDepth + 1
            );
        }

        private Queue<int> GetDivergentEdges()
        {
            Queue<int> edges = new Queue<int>();
            for (int p = 0; p < TectonicPlateModel.PlateMap.Length; p++)
                if (IsDivergentEdge(p))
                    edges.Enqueue(p);
            return edges;
        }

        private bool IsDivergentEdge(int index)
        {
            int[] neighbors = TerrainUtil.GetNeighbors(
                index,
                unused => true,
                TerrainModel.HorizontalTiles,
                TerrainModel.BaseHeightModel.Length
            );
            int id = TectonicPlateModel.PlateMap[index];
            for (int n = 0; n < neighbors.Length; n++)
            {
                int neighborId = TectonicPlateModel.PlateMap[neighbors[n]];
                if (id != neighborId)
                    if (
                        TectonicPlateModel.Plates[id].Type
                        != TectonicPlateModel.Plates[neighborId].Type
                    )
                        return true;
            }
            return false;
        }
    }
}
