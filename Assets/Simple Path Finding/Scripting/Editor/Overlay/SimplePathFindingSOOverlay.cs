using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Overlays;
#endif

namespace SimplePathFinding
{
    
    [Overlay(typeof(SceneView), "Simple Path Finding", true)]
    public class SimplePathFindingEditorOverlay : IMGUIOverlay, ITransientOverlay
    {
        
        private SimplePathFindingSO pathFindingSO;
        public bool visible
        {
            get
            {
                
                if(pathFindingSO == null || !pathFindingSO.Selected)
                {
                    pathFindingSO = Selection.activeObject as SimplePathFindingSO;
                }
                

                if (pathFindingSO != null)
                {
                    if(pathFindingSO.Selected)
                        return true;

                    return false;
                }

                else
                {

                    return false;
                }

            }
        }


        public override void OnGUI()
        {
            if (pathFindingSO != null)
            {
                pathFindingSO.ShowAllGraphInScene = 
                    EditorGUILayout.Toggle("Show all graphs", pathFindingSO.ShowAllGraphInScene);
            }
            
        }
    }
}
