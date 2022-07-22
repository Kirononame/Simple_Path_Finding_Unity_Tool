using System;
using UnityEngine;
namespace SimplePathFinding
{
    /// <summary>
    /// Abstract Class <c>Node</c> models an abstract graph that will be used as the base class for all kind of graphs.
    /// </summary>

    [Serializable]
    public abstract class Graph 
    {
	    /// <summary>
        /// Initialize Graph Data to be created.
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// Create the graph it will call <c> init <c> before creating the graph.
        /// </summary>
        public abstract void Create();
        /// <summary>
        /// Destroy the graph data and put all data to its initial state.
        /// </summary>
        public abstract void Destroy();
        /// <summary>
        /// Resets graph nodes to its initialized state without changing its obstacle state and neighbours.
        /// Used for resetting node to be traversed again by algorithms without baking the graph.
        /// </summary>
        public abstract void ResetGraphForTraversal();
        /// <summary>
        /// Get the node in this graph that corresponds to a position in the world.
        /// </summary>
        public abstract Node PositionToNode(Vector3 pos);
        /// <summary>
        /// Gets the nodes size in this graph.
        /// </summary>
        public abstract float GetNodeSize();

        /// <summary>
        /// Serrialize the graph data to a file.
        /// </summary>
        public abstract void Save();
        /// <summary>
        /// Deserrialize the graph data to a file.
        /// </summary>
        public abstract void Load();

#if UNITY_EDITOR
        /// <summary>
        /// Draw graph in the scene view.
        /// </summary>
        public abstract void DrawInScene();
        /// <summary>
        /// Draw graph data in the inspector.
        /// </summary>
        public abstract void DrawInSpectorData();
#endif 
    }
}