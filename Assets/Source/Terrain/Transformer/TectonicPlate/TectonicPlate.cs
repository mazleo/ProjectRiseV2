using System;

namespace ProjectRise.Terrain.Transformer.TectonicPlate
{
    /// <summary>
    /// Represents a single tectonic plate in the world map.
    /// </summary>
    internal class TectonicPlate
    {
        internal int ID;
        internal PlateType Type;

        internal TectonicPlate(int id, PlateType type)
        {
            ValidateId(id);
            ID = id;
            Type = type;
        }

        private void ValidateId(int id)
        {
            if (id < 0)
                throw new ArgumentException("The ID shouldn't be negative.");
        }
    }
}
