using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using ProjectRise.ProceduralGeneration.External;
using ProjectRise.Terrain.Transformer.TectonicPlate;
using ProjectRise.Utils;
using ProjectRise.World.External;
using Random = UnityEngine.Random;

namespace ProjectRise.Terrain.Transformer.Alpine
{
    internal class AlpineFormationModel
    {
        private const int DefaultMaxRadiusDistance = 3;

        internal float[] HeightModel;
        internal int[] RadiusHeightModel;

        internal TectonicPlateModel TectonicPlateModel;
        internal TerrainModel TerrainModel;
        internal int MaxRadiusDistance;

        internal AlpineFormationModel(
            TectonicPlateModel tectonicPlateModel,
            TerrainModel terrainModel,
            GameWorldModel gameWorldModel,
            int maxRadiusDistance = DefaultMaxRadiusDistance
        )
        {
            TectonicPlateModel = tectonicPlateModel;
            TerrainModel = terrainModel;
            GameWorldModel = gameWorldModel;
            MaxRadiusDistance = maxRadiusDistance;

            HeightModel = new float[TerrainModel.BaseHeightModel.Length];
            RadiusHeightModel = new int[TerrainModel.BaseHeightModel.Length];

            Generate();
        }

        private void Generate()
        {
            GenerateRadiusModel();
            GenerateHeightModel();
        }

        private void GenerateHeightModel()
        {
            for (int h = 0; h < RadiusHeightModel.Length; h++)
                HeightModel[h] = RadiusHeightModel[h] / (float)MaxRadiusDistance;
        }

        private void GenerateRadiusModel()
        {
            for (int p = 0; p < TectonicPlateModel.PlateMap.Length; p++)
                if (IsConvergentEdge(p))
                {
                    Queue<int> indexQueue = new Queue<int>();
                    indexQueue.Enqueue(p);
                    GenerateRadiusModelBfs(
                        MaxRadiusDistance,
                        0,
                        0,
                        indexQueue,
                        new Queue<int>(),
                        new bool[TectonicPlateModel.PlateMap.Length]
                    );
                }
        }

        private void GenerateRadiusModelBfs(
            int radius,
            int stepDownCount,
            int radiusStepDownThreshold,
            Queue<int> indexQueue,
            Queue<int> nextQueue,
            bool[] visitedMap
        )
        {
            if (radius == 0 || (indexQueue.IsEmpty() && nextQueue.IsEmpty()))
                return;

            bool isNextLayer = false;
            if (indexQueue.IsEmpty())
            {
                for (int n = 0; n < nextQueue.Count; n++)
                    indexQueue.Enqueue(nextQueue.Dequeue());
                isNextLayer = true;
            }

            int index = indexQueue.Dequeue();
            if (RadiusHeightModel[index] < radius)
                RadiusHeightModel[index] = radius;
            visitedMap[index] = true;

            Func<int, bool> isAlpineSlope = neighborIndex =>
                !IsConvergentEdge(neighborIndex) && !visitedMap[neighborIndex];
            int[] neighbors = TerrainUtil.GetNeighbors(
                index,
                isAlpineSlope,
                TerrainModel.HorizontalTiles,
                TerrainModel.BaseHeightModel.Length
            );
            for (int n = 0; n < neighbors.Length; n++)
            {
                nextQueue.Enqueue(neighbors[n]);
                visitedMap[neighbors[n]] = true;
            }

            if (isNextLayer)
            {
                if (stepDownCount == radiusStepDownThreshold)
                    radius--;
                if (radius > MaxRadiusDistance)
                    MaxRadiusDistance = radius;
            }

            stepDownCount = stepDownCount == radiusStepDownThreshold ? 0 : stepDownCount + 1;
            GenerateRadiusModelBfs(
                radius,
                stepDownCount,
                radiusStepDownThreshold,
                indexQueue,
                nextQueue,
                visitedMap
            );
        }

        private bool IsConvergentEdge(int index)
        {
            int[] neighbors = TerrainUtil.GetNeighbors(
                index,
                unused => true,
                TerrainModel.HorizontalTiles,
                TerrainModel.BaseHeightModel.Length
            );
            for (int n = 0; n < neighbors.Length; n++)
            {
                int id = TectonicPlateModel.PlateMap[index];
                int neighborId = TectonicPlateModel.PlateMap[neighbors[n]];
                PlateType type = TectonicPlateModel.Plates[id].Type;
                PlateType neighborType = TectonicPlateModel.Plates[neighborId].Type;
                if (id != neighborId && type == neighborType && type == PlateType.Continental)
                    return true;
            }
            return false;
        }
    }
}
