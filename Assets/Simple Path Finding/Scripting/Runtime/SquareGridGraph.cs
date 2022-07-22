using System;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SimplePathFinding
{
    [Serializable]
    public class SquareGridGraph : Graph
    {

        [HideInInspector]
        [SerializeField]
        private bool initialized = false;

        [HideInInspector]
        [SerializeField]
        private bool created = false;

        [HideInInspector]
        [SerializeField]
        private  float startX = 0f;

        [HideInInspector]
        [SerializeField]
        private float startZ = 0f;

        [HideInInspector]
        [SerializeField]
        private float y = 1.01f;

        [HideInInspector]
        [SerializeField]
        private  int width = 0;

        [HideInInspector]
        [SerializeField]
        private  int height = 0;

        
        [HideInInspector]
        [SerializeField]
        private  float nodeSize = 1f;

        [HideInInspector]
        [SerializeField]
        private int layer = 6;

        private List<Node> nodes;

        private Dictionary<(int, int), Node> nodesDict;

        private int numOfStepsX = 0;
        private int numOfStepsZ = 0;


        public override void Init()
        {
            nodes = new List<Node>();
            nodesDict = new Dictionary<(int, int), Node>();

            created = false;

            initialized = true;
        }

		public override void Destroy()
        {
            startX = 0f;
            startZ = 0f;
            y = 1.01f;

            width = 0;
            height = 0;
            
            nodeSize = 1f;

            numOfStepsX = 0;
            numOfStepsZ = 0;

            created = false;
            initialized = false;
        }
        
	    public override float GetNodeSize()
        {
            return nodeSize;
        }

        //TODO:
        public override void Save()
        {

        }

        //TODO:
        public override void Load()
        {

        }

        public override void Create()
		{

			Init();

			numOfStepsX = Mathf.FloorToInt(width / nodeSize);
			numOfStepsZ = Mathf.FloorToInt(height / nodeSize);

			int layerAsLayerMask;
			layerAsLayerMask = (1 << layer);

			for (int stepZ = 0; stepZ < numOfStepsZ; stepZ++)
			{
				for (int stepX = 0; stepX < numOfStepsX; stepX++)
				{

					Vector3 pos = new Vector3(startX+ stepX*nodeSize + nodeSize/2, y, startZ+ stepZ*nodeSize + nodeSize/2);

					Node n = new Node(pos);
					nodes.Add(n);
					nodesDict.Add((stepX, stepZ), n);

					var hitColliders = Physics.OverlapSphere(pos, nodeSize/2, layerAsLayerMask);
					if(hitColliders.Length > 0)
					{
						n.Obstacle = true;
					}

				}
			}

			for (int stepZ = 0; stepZ < numOfStepsZ; stepZ++)
			{
				for (int stepX = 0; stepX < numOfStepsX; stepX++)
				{
					if(stepX < numOfStepsX-1)
						nodesDict[(stepX,stepZ)].AddNeighbour(nodesDict[(stepX+1,stepZ)]);

					if(stepZ < numOfStepsZ-1)
						nodesDict[(stepX,stepZ)].AddNeighbour(nodesDict[(stepX,stepZ+1)]);

					if(stepX > 0)
						nodesDict[(stepX,stepZ)].AddNeighbour(nodesDict[(stepX-1,stepZ)]);

					if(stepZ > 0)
						nodesDict[(stepX,stepZ)].AddNeighbour(nodesDict[(stepX,stepZ-1)]);

					// /*
					if(stepX < numOfStepsX-1 && stepZ < numOfStepsZ-1)
						nodesDict[(stepX,stepZ)].AddNeighbour(nodesDict[(stepX+1,stepZ+1)]);

					if(stepX > 0 && stepZ < numOfStepsZ-1)
						nodesDict[(stepX,stepZ)].AddNeighbour(nodesDict[(stepX-1,stepZ+1)]);

					if(stepX < numOfStepsX-1 && stepZ > 0)
						nodesDict[(stepX,stepZ)].AddNeighbour(nodesDict[(stepX+1,stepZ-1)]);

					if(stepX > 0 && stepZ > 0)
						nodesDict[(stepX,stepZ)].AddNeighbour(nodesDict[(stepX-1,stepZ-1)]);
					// */
				}
			}

			created = true;
		}

		public override Node PositionToNode(Vector3 pos)
		{
			int numOfStepsX = (int) (width / nodeSize);
			int numOfStepsZ = (int) (height / nodeSize);

			int xx = (int) ( (pos.x - startX) / nodeSize);
			int zz = (int) ( (pos.z - startZ) / nodeSize);

			if(xx < 0)
					xx = 0;

			if(zz < 0)
				zz = 0;

			if(xx > numOfStepsX-1)
				xx = numOfStepsX-1;

			if(zz > numOfStepsZ-1)
				zz = numOfStepsZ-1;

			return nodesDict[(xx,zz)];
		}

		public override void ResetGraphForTraversal()
		{
			foreach (Node node in nodes)
			{
				node.resetNodeForTraversal();
			}
		}


#if UNITY_EDITOR
		
        public override void DrawInScene()
        {

            if(initialized)
            {

                Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

                numOfStepsX = Mathf.FloorToInt(width / nodeSize);
                numOfStepsZ = Mathf.FloorToInt(height / nodeSize);

                if(!created)
                {

                    float nWidth = (numOfStepsX * nodeSize);
                    float nHeight = (numOfStepsZ * nodeSize);
                    
                    
                    for (int z = 0; z <= numOfStepsZ; z++)
                    {
                        Handles.color = Color.white;

                        if(z == 0 || z == numOfStepsZ)
                            Handles.color = Color.black;
                        
                        Handles.DrawLine(new Vector3(startX, y, startZ+z*nodeSize), new Vector3(startX+nWidth, y, startZ+ z*nodeSize));
                    }


                    for (int x = 0; x <= numOfStepsX; x++)
                    {
                        Handles.color = Color.white;

                        if(x == 0 || x == numOfStepsX)
                            Handles.color = Color.black;
                        
                        Handles.DrawLine(new Vector3(startX+x*nodeSize, y, startZ), new Vector3(startX+x*nodeSize, y, startZ+nHeight));
                    }


                }

                else
                {

                    if(nodesDict.Count != 0)
                    {

                        for (int stepZ = 0; stepZ < numOfStepsZ; stepZ++)
                        {
                            for (int stepX = 0; stepX < numOfStepsX; stepX++)
                            {
                                Handles.color =  new Color(0.16f, 0.49f, 0.61f, 1f);

                                if(nodesDict[(stepX,stepZ)].Obstacle)
                                    continue;

                                Vector3 pos = nodesDict[(stepX,stepZ)].Position;
                                Vector3[] verts = 
                                {
                                    new Vector3(pos.x - nodeSize/2, pos.y, pos.z - nodeSize/2),
                                    new Vector3(pos.x - nodeSize/2, pos.y, pos.z + nodeSize/2),
                                    new Vector3(pos.x + nodeSize/2, pos.y, pos.z + nodeSize/2),
                                    new Vector3(pos.x + nodeSize/2, pos.y, pos.z - nodeSize/2)
                                };

                                Color grey = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                                
                                Handles.DrawSolidRectangleWithOutline(verts, grey, Color.white);
                                
                                Handles.color = new Color(0, 0, 0, 1);

                                float nposX = nodesDict[(stepX,stepZ)].Position.x;
                                float nposY = nodesDict[(stepX,stepZ)].Position.y;
                                float nposZ = nodesDict[(stepX,stepZ)].Position.z;

                                if(stepX == numOfStepsX-1 || nodesDict[(stepX+1,stepZ)].Obstacle) 
                                    Handles.DrawLine(new Vector3(nposX+nodeSize/2,
                                                    nposY, nposZ-nodeSize/2),
                                                    new Vector3(nposX+nodeSize/2,
                                                    nposY, nposZ+nodeSize/2));
                        
                                if(stepX == 0 || nodesDict[(stepX-1,stepZ)].Obstacle)
                                    Handles.DrawLine(new Vector3(nposX-nodeSize/2,
                                                    nposY, nposZ-nodeSize/2),
                                                    new Vector3(nposX-nodeSize/2,
                                                    nposY,
                                                    nposZ+nodeSize/2));

                                if(stepZ == numOfStepsZ-1 || nodesDict[(stepX,stepZ+1)].Obstacle)
                                    Handles.DrawLine(new Vector3(nposX-nodeSize/2,
                                                    nposY, nposZ+nodeSize/2),
                                                    new Vector3(nposX+nodeSize/2,
                                                    nposY, nposZ+nodeSize/2));

                                if(stepZ == 0 || nodesDict[(stepX,stepZ-1)].Obstacle)
                                    Handles.DrawLine(new Vector3(nposX-nodeSize/2,
                                                    nposY, nposZ-nodeSize/2),
                                                    new Vector3(nposX+nodeSize/2,
                                                    nposY,
                                                    nposZ-nodeSize/2));
                                
                                
                            }
                        }

                        
                    }
                    else
                    {
                        Debug.Log("Dictionary Destroyed");
                        created = false;
                        initialized = false;
                    }
                }

            }


        }

        public override void DrawInSpectorData()
        {
            
            EditorGUILayout.LabelField("Square Grid Graph");

            startX = EditorGUILayout.FloatField("X", startX);
            startZ = EditorGUILayout.FloatField("Z", startZ);
            y = EditorGUILayout.FloatField("Y", y);

            width = EditorGUILayout.IntField("Width", width);
            height = EditorGUILayout.IntField("Height", height);

            nodeSize = EditorGUILayout.FloatField("Node Size", nodeSize);

            layer = EditorGUILayout.IntField("Layer", layer);


            if (GUILayout.Button("Inititialize Graph"))
            {
                Init();
            }

            if (GUILayout.Button("Bake Graph"))
            {
                Create();
            }

            if (GUILayout.Button("Destroy Graph"))
            {
                Destroy();
            }
            
        }
    
#endif

    }
}
