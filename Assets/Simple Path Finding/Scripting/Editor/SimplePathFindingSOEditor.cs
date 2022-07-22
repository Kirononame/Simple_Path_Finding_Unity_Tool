using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SimplePathFinding
{
    [CustomEditor(typeof(SimplePathFindingSO), true)]
    public class SimplePathFindingSOEditor : Editor
    {
        private SimplePathFindingSO pathFindingInspected;

        private GUIStyle headerLabelStyle;
        
        private bool guiStylesInitialize = false;
        

        private void OnEnable()
        {
            pathFindingInspected = target as SimplePathFindingSO;

            SceneView.duringSceneGui += OnToolGUI;
            pathFindingInspected.Selected = true;

        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnToolGUI;
            pathFindingInspected.Selected = false;
        }

        public override void OnInspectorGUI()
        {
            
            if(!guiStylesInitialize)
                InitializeGuiStyles();

            EditorGUILayout.LabelField("Path Finding", headerLabelStyle);

            EditorGUILayout.Space();
            
            pathFindingInspected.Graph = EditorGUILayout.ObjectField(pathFindingInspected.Graph,typeof(GraphSO), true) as GraphSO;
            
            EditorGUILayout.Space();
            
            MarkAllDirty();

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
        
        public  void OnToolGUI(SceneView sceneView)
        {
            if(pathFindingInspected.ShowAllGraphInScene)
            {
                if(pathFindingInspected.Graph != null)
                    pathFindingInspected.Graph.DrawGraphInScene();
            }
        }

        private void MarkAllDirty()
        {
            EditorUtility.SetDirty(target);
        }
        
    }

}