using System.Collections.Generic;
using UnityEngine;

namespace SimplePathFinding
{
    /// <summary>
    /// Class <c>Node</c> models a node Location in the game scene.
    /// </summary>
    public class Node
    {
        /// <value> Property <c>Visited</c> Needed for the <c>A* Algorithm.</c></value>
        public bool Visited
        { get; set; }

        /// <value> Property <c>GlobalGoal</c> Needed for the <c>A* Algorithm.</c></value>
        public float GlobalGoal
        { get; set; }

        /// <value> Property <c>LocalGoal</c> Needed for the <c>A* Algorithm.</c></value>
        public float LocalGoal
        { get; set; }

        /// <value>Property <c>Obstacle</c> represents the Node's 
        /// if it can be traversed or it is blocked by an obstacle.
        ///</value>
        public bool Obstacle
        { get; set; }

        /// <value> Property <c>Position</c> Represents the position of the Node.</value>
        public Vector3 Position
        { get; set; }

        /// <value> Property <c>Parent</c> Represents the Node Parent of the Node.</value>
        public Node Parent
        { get; set; }

        /// <summary>
        /// Instance variable <c>neighbours</c> represents the node neighbours.
        /// </summary>
        private List<Node> neighbours;

        /// <summary>
        /// Create a Node that presents a location in the game scene that 
        /// the player can reach if is not blocked.
        /// </summary>
        ///  <param name="position">The location of the node in the scene.</param>
        public Node(Vector3 position)
        {
            this.Position = position;
            resetNode();
        }

		/// <summary>
        /// Add a neighbour that is connected directly to this node.
        /// </summary>
        ///  <param name="node">The node object connected to this node.</param>
        public void AddNeighbour(Node node)
        {
            neighbours.Add(node);
        }

        /// <summary>
		/// Get the Node neighbours that are connected directly to this node.
		/// </summary>
		/// <returns>
		/// A List of Node objects for this Node neighbours.
		/// </returns>
		public List<Node> GetNeighbours()
		{
			return neighbours;
		}

        /// <summary>
        /// Resets the node object to its initialized state.
        /// </summary>
        public void resetNode()
        {
            this.LocalGoal = float.MaxValue;
            this.GlobalGoal = float.MaxValue;

            this.Obstacle = false;
            this.Visited = false;

            this.Parent = null;
            this.neighbours = new List<Node>();
        }

        /// <summary>
        /// Resets the node object to its initialized state without changing its obstacle state and neighbours.
        /// Used for resetting node to be traversed again by algorithms without baking the graph
        /// </summary>
        public void resetNodeForTraversal()
        {
            this.LocalGoal = float.MaxValue;
            this.GlobalGoal = float.MaxValue;

            this.Visited = false;

            this.Parent = null;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws the node as a Sphere in the scene view for visual representation
        /// Should be called in the <c>OnDrawGizmos</c>
        /// </summary>
        ///  <param name="radius">The radius of the sphere to be drawn.</param>
        public void DrawNodeAsSphere(float radius)
        {
            Gizmos.DrawSphere(this.Position, radius);
        }
#endif

    }
}    