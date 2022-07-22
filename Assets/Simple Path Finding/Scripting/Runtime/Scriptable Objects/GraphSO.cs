using System;
using UnityEngine;

namespace SimplePathFinding
{
    [CreateAssetMenu(fileName ="Graph", menuName ="SimplePathFindingSO/Graph", order = 1)]
    public class GraphSO : ScriptableObject
    {
        public enum Graphtype
        {
            SQUARE_GRID,
            HEXAGONAL_GRID,

            // Add graph types to be supported
        }

        [HideInInspector]
        [SerializeField]
        private Graphtype graphType;

        
        [HideInInspector]
        [SerializeReference]
        private Graph graph = new SquareGridGraph();

#if UNITY_EDITOR
        [SerializeField]
        [HideInInspector]
        private bool showGraphInScene = false;

        public bool ShowGraphInScene 
        {
            get{ return showGraphInScene; }
            set{ showGraphInScene = value; }
        }
#endif

        public static Graph GetGraph(Graphtype graphType) => graphType switch
        {
            Graphtype.SQUARE_GRID    =>  new SquareGridGraph(),
            _ => throw new ArgumentOutOfRangeException(nameof(graphType), $"Not expected graphType value: {graphType}"),
        };

        public void InitGraph()
        {
            graphType = Graphtype.SQUARE_GRID;
            graph = GetGraph(graphType);
        }

        public void CreateGraph()
        {
            graph.Create();
        }

        public float GetNodeSize()
        {
            return graph.GetNodeSize();
        }
        
        public Node PositionToNode(Vector3 position)
        {
            return graph.PositionToNode(position);
        }
        
        public void ResetGraphForTraversal()
        {
            graph.ResetGraphForTraversal();
        }

        public void DestroyGraph()
        {
            graph.Destroy();
        }


#if UNITY_EDITOR
        public string[] GetGraphsNames()
        {
            return Enum.GetNames(typeof(Graphtype));
        }

        public string GetSelectedGraphName()
        {
            return Enum.GetName(typeof(Graphtype), graphType);
        }

        public void SelectedGraphType(int graphName)
        {
            Graphtype selectedGraphType = (Graphtype) graphName;
            

            graphType = selectedGraphType;
            
            if(graph is not null)
                graph.Destroy();

            try
            { 
                graph = GetGraph(graphType);
            }

            catch (ArgumentOutOfRangeException)
            {
                graph = null;
                throw;
            }
            
        }

        public void DrawGraphInspector()
        {
            if(graph is not null)
                graph.DrawInSpectorData();
        }

        public void DrawGraphInScene()
        {
            if(graph is not null)
                graph.DrawInScene();
        }
#endif
          
    }

}