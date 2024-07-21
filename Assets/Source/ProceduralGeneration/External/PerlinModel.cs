using System;

namespace ProjectRise.ProceduralGeneration.External
{
    /// <summary>
    /// A model representing perlin noise.
    /// </summary>
    public class PerlinModel
    {
        public static Builder GetBuilder()
        {
            return new Builder();
        }

        /// <summary>
        /// Controls the frequency of the noise.
        /// </summary>
        public float Lacunarity;

        /// <summary>
        /// Controls the amplitude of the noise.
        /// </summary>
        public float Persistence;

        /// <summary>
        /// Controls where in the noise to sample from the x-axis.
        /// </summary>
        public float XOffset;

        /// <summary>
        /// Controls where in the noise to sample from the z-axis.
        /// </summary>
        public float ZOffset;

        private PerlinModel(float lacunarity, float persistence, float xOffset, float zOffset)
        {
            Lacunarity = lacunarity;
            Persistence = persistence;
            XOffset = xOffset;
            ZOffset = zOffset;
        }

        public class Builder
        {
            private float _lacunarity = 1F;
            private float _persistence = 1F;
            private float _xOffset;
            private float _zOffset;

            internal Builder() { }

            public Builder Lacunarity(float lacunarity)
            {
                ThrowOnInvalid(lacunarity);
                _lacunarity = lacunarity;
                return this;
            }

            public Builder Persistence(float persistence)
            {
                ThrowOnInvalid(persistence);
                _persistence = persistence;
                return this;
            }

            public Builder XOffset(float xOffset)
            {
                ThrowOnInvalid(xOffset);
                _xOffset = xOffset;
                return this;
            }

            public Builder ZOffset(float zOffset)
            {
                ThrowOnInvalid(zOffset);
                _zOffset = zOffset;
                return this;
            }

            public PerlinModel Build()
            {
                return new PerlinModel(_lacunarity, _persistence, _xOffset, _zOffset);
            }

            private void ThrowOnInvalid(float value)
            {
                if (value < 0)
                    throw new ArgumentException("PerlinModel field should not be negative.");
            }
        }
    }
}
