using ProjectRise.World.External;
using UnityEngine;

namespace ProjectRise.Terrain
{
    /// <summary>
    /// Manager of the terrain's unity mesh.
    /// </summary>
    /// <typeparam name="TA">Arguments required to generate a mesh.</typeparam>
    internal interface ITerrainMeshManager<TA>
    {
        /// <summary>
        /// Provides the unity mesh. Creates and caches it if it doesn't
        /// exist already.
        /// </summary>
        /// <returns>The unity mesh.</returns>
        internal Mesh GetMesh();

        /// <summary>
        /// Provides the unity mesh. Creates and caches it if it doesn't
        /// already. This method takes in arguments for the mesh which
        /// may of may have not changed since the mesh was last
        /// generated. The args will be passed or assigned to the mesh
        /// generator.
        /// </summary>
        /// <param name="args">Arguments for creating the mesh if needed.</param>
        /// <returns>The unity mesh.</returns>
        internal Mesh GetMesh(TA args);
    }
}
