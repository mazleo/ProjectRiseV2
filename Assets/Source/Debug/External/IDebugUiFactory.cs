using System.Collections.Generic;
using UnityEngine;

namespace ProjectRise.Debug.External
{
    /// <summary>
    /// Factory that creates UI elements from overlay models.
    /// </summary>
    public interface IDebugUiFactory
    {
        /// <summary>
        /// Creates the UI elements from the overlay model.
        /// The elements that are directly referenced by the
        /// list are the root GameObjects that should be
        /// under the root overlay GameObject.
        /// </summary>
        /// <returns>
        /// A list containing the root UI elements.
        /// </returns>
        public List<GameObject> Create();
    }
}
