using UnityEngine;

namespace ProjectRise.Terrain
{
    /// <summary>
    /// Generates the mesh for the terrain.
    /// This should take care of all mesh factors, like
    /// triangles, submeshes, etc.
    /// </summary>
    internal interface ITerrainMeshGenerator
    {
        /// <summary>
        /// Generates the terrain mesh.
        /// </summary>
        /// <param name="mesh">Mesh object to reuse if any.</param>
        /// <returns>The unity mesh.</returns>
        internal Mesh[] Generate(Mesh mesh);
    }
}
