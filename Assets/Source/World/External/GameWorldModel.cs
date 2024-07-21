using System;

namespace ProjectRise.World.External
{
    /// <summary>
    /// A Game world model.
    ///
    /// Provides information regarding the game world itself.
    /// </summary>
    public class GameWorldModel
    {
        public const float DefaultWidth = 700F;
        public const float DefaultLength = 500F;
        public const float DefaultHeight = 350F;
        public const float DefaultWaterLevel = 200F;

        public float Width;
        public float Length;
        public float Height;
        public float WaterLevel;

        public GameWorldModel(
            float width = DefaultWidth,
            float length = DefaultLength,
            float height = DefaultHeight,
            float waterLevel = DefaultWaterLevel
        )
        {
            ThrowIfInvalid(width, length, height, waterLevel);
            Width = width;
            Length = length;
            Height = height;
            WaterLevel = waterLevel;
        }

        private void ThrowIfInvalid(float width, float length, float height, float waterLevel)
        {
            if (
                IsNonPositive(width)
                || IsNonPositive(length)
                || IsNonPositive(height)
                || IsNonPositive(waterLevel)
            )
                throw new ArgumentException("GameWorldModel values cannot be negative or 0.");
            if (waterLevel >= height)
                throw new ArgumentException(
                    "Water level should be less than GameWorldModel height."
                );
        }

        private bool IsNonPositive(float value)
        {
            return value <= 0;
        }
    }
}
