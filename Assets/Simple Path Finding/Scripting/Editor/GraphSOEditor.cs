using System;
using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace SimplePathFinding
{
    [CustomEditor(typeof(GraphSO), true)]
    public class GraphSOEditor : Editor
    {
        private const string NO_GRAPHS_WARNING = "There is no Graph associated to this location yet";

        private GraphSO graphInspected; // refrence to the scriptable object to have an editor

        private GUIStyle headerLabelStyle;
        private string[] graphTypeList;
        private bool guiStylesInitialize = false;

        private int selectedGraph;


        private void OnEnable()
        {
            
            graphInspected = target as GraphSO;

            PopulateGraphPicker();
            
            SceneView.duringSceneGui += OnToolGUI;

        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnToolGUI;
            
        }

        public override void OnInspectorGUI()
        {
            
            if(!guiStylesInitialize)
                InitializeGuiStyles();

            EditorGUILayout.LabelField("Graph", headerLabelStyle);

            
            EditorGUILayout.Space();
            DrawGraphTypePicker(); // picker to choose graph

            graphInspected.DrawGraphInspector();

            MarkAllDirty();

        }


        private void DrawGraphTypePicker()
        {
            EditorGUI.BeginChangeCheck();
            selectedGraph = graphTypeList.ToList().IndexOf(graphInspected.GetSelectedGraphName()); // list of graph names squre_grid hexagonal ...

            // should never happen as i added graphs to the enum
            if (selectedGraph < 0)
            {
                EditorGUILayout.HelpBox(NO_GRAPHS_WARNING, MessageType.Warning);
            }

            selectedGraph = EditorGUILayout.Popup("Graph", selectedGraph, graphTypeList);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed selected graph");
                graphInspected.SelectedGraphType(selectedGraph);
            
            }
        }

        private void PopulateGraphPicker()
        {
            graphTypeList = graphInspected.GetGraphsNames();
        }

        private void InitializeGuiStyles()
        {
            headerLabelStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 18,
                fixedHeight = 70.0f
            };
            guiStylesInitialize = true;
            
        }
        
        public void DrawToolWindow(int id)
        {
        
            graphInspected.ShowGraphInScene = 
                EditorGUILayout.Toggle("Show graph", graphInspected.ShowGraphInScene);
        }

        Rect toolWindowRect = new Rect(10, 0, 180f, 0f);
        public  void OnToolGUI(SceneView sceneView)
        {

            toolWindowRect = GUILayout.Window(45, toolWindowRect, DrawToolWindow, "Graph");
            toolWindowRect.y = sceneView.position.height - toolWindowRect.height - 50;

            if(graphInspected.ShowGraphInScene)
                graphInspected.DrawGraphInScene();

        }

        private void MarkAllDirty()
        {
            EditorUtility.SetDirty(target);
        }
            
    }

}