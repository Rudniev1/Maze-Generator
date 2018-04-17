using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

namespace Completed
{
    //Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
    public class Player : MonoBehaviour
    {

        public float moveTime = 0.1f;           
        public LayerMask blockingLayer;         

        private BoxCollider2D boxCollider;                    
        private float inverseMoveTime;
        bool movePlayer = false;
        Text infoText;

        int movesCount = 0;
        int StepsByAlgoritm;
        bool calculate;
        

        void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();

            inverseMoveTime = 1f / moveTime;
            infoText = GameObject.FindObjectsOfType<Text>()[1];
            infoText.text = "Ilość poczynionych kroków: 0";
        }

        private void Update()
        {
           
            if(PlayerPrefs.GetFloat("StepsToCount") != 0f && calculate == false)
            {
                StepsByAlgoritm = (int)(PlayerPrefs.GetFloat("StepsToCount") * 4);
                calculate = true;
            }

            int horizontal = 0;     
            int vertical = 0;       

            //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));

            //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            //Check if moving horizontally, if so set vertical to zero.
            if (horizontal != 0)
            {
                vertical = 0;
            }
            //Check if we have a non-zero value for horizontal or vertical
            if ((horizontal != 0 || vertical != 0) && (movePlayer == false))
            {
                RaycastHit2D hit;
                Move(horizontal, vertical,out hit);
                movesCount++;
                infoText.text = "Ilość przebytych kroków: " + movesCount ;
            }

            if(horizontal == 0 && vertical == 0)
            {
                movePlayer = false;
            }
        }

        void SmoothMovement(Vector3 end)
        {

            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            while (sqrRemainingDistance > float.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, end, 99999f);
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            }
        }

        bool Move(int xDir, int yDir, out RaycastHit2D hit)
        {
            movePlayer = true;
            Vector2 start = transform.position;
            Vector2 end = start + new Vector2(xDir, yDir);

            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, blockingLayer);
            boxCollider.enabled = true;

            if (hit.transform == null)
            {
                SmoothMovement(end);
                return true;
            }
            return false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Meta")
            {
                Debug.Log(StepsByAlgoritm);
                infoText.text = "Dotarłeś do celu. Liczba kroków : " + movesCount + "\n Najmniejsza możliwa liczba kroków: " + StepsByAlgoritm;
            }
        }     

    }
}

