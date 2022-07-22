using System.Collections.Generic;

using UnityEngine;

namespace SimplePathFinding
{
    [CreateAssetMenu(fileName ="Simple Path Finding", menuName ="SimplePathFindingSO/SimplePathFinding", order = 1)]
    public class SimplePathFindingSO : ScriptableObject
    {
        
        [HideInInspector]
        [SerializeReference]
        private GraphSO graphSO; // It is made to have more than one graph in future releases

        public GraphSO Graph
        {
            get
            {
                return graphSO;
            }
            set
            {
                graphSO = value;
            }
        }

#if UNITY_EDITOR
        private bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }

        [SerializeField]
        [HideInInspector]
        private bool showAllGraphInScene = false;

        public bool ShowAllGraphInScene 
        {
            get{ return showAllGraphInScene; }
            set{ showAllGraphInScene = value; }
        }
#endif

        public void CreateGraph()
        {
            graphSO.CreateGraph();
        }

        public List<Node> GetPath(Vector3 start, Vector3 destination)
        {
            Node sNode = graphSO.PositionToNode(start);
            Node eNode = graphSO.PositionToNode(destination);

            graphSO.ResetGraphForTraversal();

            return PathNavigationAlgo.Solve_AStar(sNode, eNode);

        }

        public float GetNodeSize()
        {
            return graphSO.GetNodeSize();
        }


    }
}
