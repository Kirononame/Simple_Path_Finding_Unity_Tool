using System.Collections.Generic;
using System.Linq;
using System.Threading;

using UnityEngine;

namespace SimplePathFinding
{
    public class SimplePathFindingAgent : MonoBehaviour
    {
        public float speed = 10f;
        public SimplePathFindingSO simplePathFindingSO;

        private Vector3 startPos;
        private Vector3 destination;
        private List<Node> path;

        private Thread pathFindingThread = null; // You can use threads in unity just not monobeahviours 

        private bool move;

        void Awake()
        {
            move = false;
            simplePathFindingSO.CreateGraph(); // Will create graph as we don't save the graph to load it
        }
        
        public void getPath()
        {
            path = simplePathFindingSO.GetPath(startPos, destination); // list of nodes as the path

            if(path.Count > 0)
            {
                move = true;
            }
            else
            {
                move = false;
            }

        }

        public void SetDestination(Vector3 destination)
        {
            
            this.startPos = transform.position;
            this.destination = destination;
            move = false;
            
            if(pathFindingThread == null)
            {
                pathFindingThread = new Thread(getPath);
                pathFindingThread.Start();
            }
            else if(pathFindingThread.IsAlive)
            {
                pathFindingThread.Abort();
                pathFindingThread = new Thread(getPath);
                pathFindingThread.Start();
            }
            else
            {
                pathFindingThread = new Thread(getPath);
                pathFindingThread.Start();
            }
            
        }

        void Update()
        {
            if(move)
            {
                if(path.Count > 0)
                {
                    Node way = path.Last<Node>();

                    if(Vector3.Distance(way.Position, transform.position) > 1.3) // I tried more than one distance and this worked for me
                    {
                        Vector3 dir = (way.Position - transform.position).normalized;
                        dir.y = 0f;
                        transform.Translate(dir * Time.deltaTime * speed);
                    }
                    else
                    {
                        path.RemoveAt(path.Count - 1);
                    }
                }
                
                else
                {
                    move = false;
                }
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if(path != null)
            {
                for(int i = 0; i < path.Count; i++)
                {
                    Gizmos.color = Color.blue;

                    if(i == 0)
                        Gizmos.color = Color.green;
                    
                    if(i == path.Count-1)
                        Gizmos.color = Color.red;
                    
                    path[i].DrawNodeAsSphere(simplePathFindingSO.GetNodeSize()/4);

                }
            }
        }
#endif

    }
}
