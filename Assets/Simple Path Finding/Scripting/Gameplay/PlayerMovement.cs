using UnityEngine;

namespace SimplePathFinding
{
    public class PlayerMovement : MonoBehaviour
    {

        SimplePathFindingAgent playerAgent;
        
        void Start()
        {
            playerAgent = GetComponent<SimplePathFindingAgent>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3? nullablePos = MouseToWorldPosition();

                if(nullablePos.HasValue)
                {
                    Vector3 pos = (Vector3)MouseToWorldPosition();
                    playerAgent.SetDestination(pos);
                }

            }
        }

        Vector3? MouseToWorldPosition()
        {
            
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(r, out hit))
            {

                Vector3 worldPosition = hit.point;
                return worldPosition;
            }

            return null;

        }
    }

}
