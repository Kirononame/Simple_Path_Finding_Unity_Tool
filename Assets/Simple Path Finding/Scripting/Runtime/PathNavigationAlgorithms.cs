using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SimplePathFinding
{
    /// <summary>
    /// Static Class <c>PathNavigation</c> Contains algorithms to find the optimal path in a graph.
    /// </summary>
    public static class PathNavigationAlgo
    {
        public enum AvailableAlgorithms
        {
            ASTAR,
        }

        /// <summary>
        /// Static Method <c>Solve_AStar</c>
        /// That solves a graph using AStart algorithm.
        /// </summary>
        /// <param name="startNode">The start node in the graph.</param>
        /// <param name="enode">The End Node in the graph (The End point Node).</param>
        /// <returns>
        /// A List of the nodes that it takes to reach the destination.
        /// </returns>
        public static List<Node> Solve_AStar(Node startNode, Node enode)
        {

            Node endNode = enode;

            Node currentNode = startNode;

            startNode.LocalGoal = 0f;
            startNode.GlobalGoal = Vector3.Distance(startNode.Position, endNode.Position);

            List<Node> notTestedNodes = new List<Node>();
            notTestedNodes.Add(startNode);

            while (notTestedNodes.Count != 0 && currentNode != endNode)
            {
                notTestedNodes.OrderBy(o=>o.GlobalGoal).ToList();
                

                while (notTestedNodes.Count != 0 && notTestedNodes.First<Node>().Visited)
                    notTestedNodes.RemoveAt(0);
                
                if (notTestedNodes.Count == 0)
                    break;
                

                currentNode = notTestedNodes.First<Node>();
                currentNode.Visited = true;

                foreach (Node nodeNeighbour in currentNode.GetNeighbours())
                {
                    
                    if (!nodeNeighbour.Visited && !nodeNeighbour.Obstacle)
                        notTestedNodes.Add(nodeNeighbour);

                    
                    float possiblyLowerGoal = currentNode.LocalGoal + Vector3.Distance(currentNode.Position, nodeNeighbour.Position);

                    if (possiblyLowerGoal < nodeNeighbour.LocalGoal && !nodeNeighbour.Obstacle)
                    {
                        nodeNeighbour.Parent = currentNode;
                        nodeNeighbour.LocalGoal = possiblyLowerGoal;

                        nodeNeighbour.GlobalGoal = nodeNeighbour.LocalGoal + Vector3.Distance(nodeNeighbour.Position, endNode.Position);
                    }
                }
            }
            
            List<Node> nodePath = new List<Node>();
            Node n = enode;
                
            while(n.Parent != null)
            {
                nodePath.Add(n);
                n = n.Parent;
            }
            
            return nodePath;
        
        }
    }
}