using System;
using System.Collections.Generic;

namespace ProjectRise.Terrain
{
    /// <summary>
    /// Holds the terrain transformers, ensuring correct
    /// ordering of all transformers.
    /// </summary>
    internal class TerrainTransformerCollection
    {
        internal readonly List<string> TransformerCollection = new List<string>();
        internal readonly Dictionary<string, ITerrainModelTransformer> TransformerMap =
            new Dictionary<string, ITerrainModelTransformer>();
        private int _transformersCount;

        /// <summary>
        /// Register a transformer into the collection.
        /// </summary>
        /// <param name="index">The index to register to.</param>
        /// <param name="id">The transformer ID.</param>
        /// <param name="transformer">The transformer.</param>
        internal void Register(int index, string id, ITerrainModelTransformer transformer)
        {
            ThrowOnInvalidRegister(index, id, transformer);
            TransformerCollection.Insert(index, id);
            TransformerMap.Add(id, transformer);
            _transformersCount++;
        }

        /// <summary>
        /// Gets a single transformer by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The target transformer.</returns>
        /// <exception cref="ArgumentException">Throws when the ID is invalid.</exception>
        internal ITerrainModelTransformer Get(string id)
        {
            if (!TransformerMap.ContainsKey(id))
                throw new ArgumentException(
                    $"Attempting to retrieve transformer with ID {id} that does not exist."
                );

            return TransformerMap[id];
        }

        /// <summary>
        /// Gets all transformers in a List, with correct
        /// ordering.
        /// </summary>
        /// <returns></returns>
        internal List<ITerrainModelTransformer> GetAll()
        {
            List<ITerrainModelTransformer> transformers = new List<ITerrainModelTransformer>();
            foreach (string id in TransformerCollection)
            {
                transformers.Add(TransformerMap[id]);
            }
            return transformers;
        }

        internal int Count()
        {
            return _transformersCount;
        }

        internal bool IsEmpty()
        {
            return _transformersCount == 0;
        }

        private void ThrowOnInvalidRegister(
            int index,
            string id,
            ITerrainModelTransformer transformer
        )
        {
            if (index != _transformersCount)
                throw new ArgumentException(
                    $"Transformers must be registered in order. The next transformer index is {_transformersCount}."
                );
            if (TransformerMap.ContainsKey(id))
                throw new ArgumentException($"The transformer ID {id} is already registered.");
            if (id.Equals(""))
                throw new ArgumentException("Transformer ID is invalid.");
            if (transformer == null)
                throw new ArgumentNullException("Transformer cannot be null.");
        }
    }
}
