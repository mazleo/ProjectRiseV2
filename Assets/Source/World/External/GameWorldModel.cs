using System;
using UnityEngine;

namespace ProjectRise.World.External
{
    /// <summary>
    /// A Game world model.
    ///
    /// Provides information regarding the game world itself.
    /// </summary>
    public class GameWorldModel
    {
        public const float DefaultWidth = 500F;
        public const float DefaultLength = 500F;
        public const float DefaultHeight = 500F;
        public const float DefaultLandformHeight = 400F;
        public const float DefaultWaterLevel = 350F;
        public Vector3 DefaultVolumeCameraDimensions = new Vector3(0.3F, 0.3F, 0.3F);
        public Vector3 DefaultVolumeCameraPosition = new Vector3(0, 0.15F, 0);

        public float Width;
        public float Length;
        public float Height;
        public float LandformHeight;
        public float WaterLevel;
        public Vector3? VolumeCameraDimensions;
        public Vector3? VolumeCameraPosition;

        public GameWorldModel(
            float width = DefaultWidth,
            float length = DefaultLength,
            float height = DefaultHeight,
            float landformHeight = DefaultLandformHeight,
            float waterLevel = DefaultWaterLevel,
            Vector3? volumeCameraDimensions = null,
            Vector3? volumeCameraPosition = null
        )
        {
            Width = width;
            Length = length;
            Height = height;
            LandformHeight = landformHeight;
            WaterLevel = waterLevel;
            VolumeCameraDimensions =
                volumeCameraDimensions == null
                    ? DefaultVolumeCameraDimensions
                    : volumeCameraDimensions;
            VolumeCameraPosition =
                volumeCameraPosition == null ? DefaultVolumeCameraPosition : volumeCameraPosition;
            ThrowIfInvalid(
                Width,
                Length,
                Height,
                LandformHeight,
                WaterLevel,
                VolumeCameraDimensions,
                VolumeCameraPosition
            );
        }

        private void ThrowIfInvalid(
            float width,
            float length,
            float height,
            float landformHeight,
            float waterLevel,
            Vector3? volumeCameraDimensions,
            Vector3? volumeCameraPosition
        )
        {
            if (
                IsNonPositive(width)
                || IsNonPositive(length)
                || IsNonPositive(height)
                || IsNonPositive(landformHeight)
                || IsNonPositive(waterLevel)
                || !IsVolumeCameraAttrValid(volumeCameraDimensions)
                || !IsVolumeCameraAttrValid(volumeCameraPosition)
            )
                throw new ArgumentException("GameWorldModel values cannot be negative or 0.");
            if (waterLevel >= height)
                throw new ArgumentException(
                    "Water level should be less than GameWorldModel height."
                );
        }

        private bool IsVolumeCameraAttrValid(Vector3? volumeCameraAttr)
        {
            if (
                volumeCameraAttr == null
                || IsNegative(volumeCameraAttr.Value.x)
                || IsNegative(volumeCameraAttr.Value.y)
                || IsNegative(volumeCameraAttr.Value.z)
            )
            {
                return false;
            }
            return true;
        }

        private bool IsNonPositive(float value)
        {
            return value <= 0;
        }

        private bool IsNegative(float value)
        {
            return value < 0;
        }
    }
}
