using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRise.Utils
{
    /// <summary>
    /// Util for Terrain features.
    /// </summary>
    public class TerrainUtil
    {
        public static int[] GetNeighbors(
            int index,
            Func<int, bool> passesCondition,
            int horizontalTiles,
            int mapLength
        )
        {
            ValidateGetNeighborsInputs(index, passesCondition, horizontalTiles, mapLength);
            int r = Mathf.FloorToInt(index / horizontalTiles);
            int c = index % horizontalTiles;
            List<int> neighbors = new List<int>();

            int topLeftIndex = GetOneDimensionalIndex(r + 1, c - 1, horizontalTiles);
            if (IsValidIndex(topLeftIndex, mapLength) && passesCondition(topLeftIndex))
                neighbors.Add(topLeftIndex);
            int topIndex = GetOneDimensionalIndex(r + 1, c, horizontalTiles);
            if (IsValidIndex(topIndex, mapLength) && passesCondition(topIndex))
                neighbors.Add(topIndex);
            int topRightIndex = GetOneDimensionalIndex(r + 1, c + 1, horizontalTiles);
            if (IsValidIndex(topRightIndex, mapLength) && passesCondition(topRightIndex))
                neighbors.Add(topRightIndex);
            int leftIndex = GetOneDimensionalIndex(r, c - 1, horizontalTiles);
            if (IsValidIndex(leftIndex, mapLength) && passesCondition(leftIndex))
                neighbors.Add(leftIndex);
            int rightIndex = GetOneDimensionalIndex(r, c + 1, horizontalTiles);
            if (IsValidIndex(rightIndex, mapLength) && passesCondition(rightIndex))
                neighbors.Add(rightIndex);
            int bottomLeftIndex = GetOneDimensionalIndex(r - 1, c - 1, horizontalTiles);
            if (IsValidIndex(bottomLeftIndex, mapLength) && passesCondition(bottomLeftIndex))
                neighbors.Add(bottomLeftIndex);
            int bottomIndex = GetOneDimensionalIndex(r - 1, c, horizontalTiles);
            if (IsValidIndex(bottomIndex, mapLength) && passesCondition(bottomIndex))
                neighbors.Add(bottomIndex);
            int bottomRightIndex = GetOneDimensionalIndex(r - 1, c + 1, horizontalTiles);
            if (IsValidIndex(bottomRightIndex, mapLength) && passesCondition(bottomRightIndex))
                neighbors.Add(bottomRightIndex);

            return neighbors.ToArray();
        }

        public static bool IsValidIndex(int index, int mapLength)
        {
            return index >= 0 && index < mapLength;
        }

        public static int GetOneDimensionalIndex(int r, int c, int horizontalTiles)
        {
            if (
                IsNegative(r)
                || IsNegative(c)
                || IsNegative(horizontalTiles)
                || c >= horizontalTiles
            )
                return -1;
            return (r * horizontalTiles) + c;
        }

        private static void ValidateGetNeighborsInputs(
            int index,
            Func<int, bool> passesCondition,
            int horizontalTiles,
            int mapLength
        )
        {
            if (IsNegative(index) || IsNegative(horizontalTiles) || IsNegative(mapLength))
                throw new ArgumentException("Inputs should not be negative.");

            if (passesCondition == null)
                throw new ArgumentException("Function should not be null");

            if (index >= mapLength)
                throw new IndexOutOfRangeException();

            if (horizontalTiles > mapLength)
                throw new ArgumentException("horizontalTiles cannot be larger than mapLength");
        }

        private static bool IsNegative(int num)
        {
            return num < 0;
        }
    }
}
