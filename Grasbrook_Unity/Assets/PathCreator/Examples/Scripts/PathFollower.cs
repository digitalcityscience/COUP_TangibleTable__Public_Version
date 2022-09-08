using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        //This two Variables need to be public because they get values 
        //from other scripts
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        
        private GameObject pool;
        private GameObject pathOne;
        private GameObject pathTwo;
        private GameObject pathThree;
        float distanceTravelled;
        public float speed = 10;

        [SerializeField]
        int randomPath;



        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }

            pool = GameObject.Find("CarPool");
            pathOne = GameObject.Find("PathSmallOne");
            pathTwo = GameObject.Find("PathSmallTwo");
            pathThree = GameObject.Find("PathSmallThree");
            randomPath = Random.Range(0, 3);
        }

        void Update()
        {
            speed = GlobalVariable.GlobalNoiseCarSpeed;
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
            else
            {
                if(randomPath == 0)
                {
                    pathCreator = pathOne.GetComponent<PathCreator>();
                }
                else if(randomPath == 1)
                {
                    pathCreator = pathTwo.GetComponent<PathCreator>();
                }
                else
                {
                    pathCreator = pathThree.GetComponent<PathCreator>();
                }
                
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}