using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zergling : MonoBehaviour {

    public Camera gameCamera;
    public Animator animator;

    public bool move;

    public Vector3 targetPos;
    public Vector3 destinationPos;

    public Cell currentCell;

    List<Cell> movePath;

    public int frameCount;
    public float speed;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        move = false;
        frameCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (move)
        {
            RotateToTarget();
            MoveFromAStarPath();
        }

        switch (frameCount++)
        {
            case 0: speed = 2; break;
            case 1: speed = 8; break;
            case 2: speed = 9; break;
            case 3: speed = 5; break;
            case 4: speed = 6; break;
            case 5: speed = 7; break;
            case 6: speed = 2; break;
        }

        if (frameCount >= 6)
            frameCount = 0;

        if (Input.GetMouseButtonDown(1))
        {
            //Vector3 worldMousePos = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            
            //targetPos = worldMousePos;

            
        }
	}

    // Current Path
    private Cell currentTarget;
    // path Count
    public int pathCount;


    private void MoveFromAStarPath()
    {
        MoveToTarget();
    }

    private void ChangeTarget()
    {
        pathCount--;

        if (pathCount < 0)
        {
            move = false;

            return;
        }

        currentTarget = movePath[pathCount];
        targetPos = currentTarget.transform.position;
    }

    public void Move(Cell targetCell, Vector3 touchPos)
    {
        movePath = AStarManager.Instance.GetAStarPath(currentCell, targetCell);

        pathCount = movePath.Count;
        currentTarget = movePath[movePath.Count - 1];
        targetPos = currentTarget.transform.position;
        destinationPos = touchPos;
        move = true;
    }

    public bool MoveToTarget()
    {
        targetPos.z = transform.position.z; 
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed * 0.5F);

        if (transform.position == targetPos)
        {
            ChangeTarget();
            return true;
        }
        return false;
    }

    private void RotateToTarget()
    {
        Vector3 v = destinationPos - transform.position;

        float rotation = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        //Debug.Log(pathCount + " : " + rotation);
        if (rotation > 90)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            rotation *= -1;
            rotation += 180;
        }
        else if (rotation <= -90)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            rotation *= -1;
            rotation -= 180;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        // Animator - Rotation Value
        if (rotation > 70 && rotation <= 90)
        {
            animator.SetInteger("Rotation", 0);
        }
        else if (rotation > 50 && rotation <= 70)
        {
            animator.SetInteger("Rotation", 1);
        }
        else if (rotation > 30 && rotation <= 50)
        {
            animator.SetInteger("Rotation", 2);
        }
        else if (rotation > 10 && rotation <= 30)
        {
            animator.SetInteger("Rotation", 3);
        }
        else if (rotation > -10 && rotation <= 10)
        {
            animator.SetInteger("Rotation", 4);
        }
        else if (rotation > -30 && rotation <= -10)
        {
            animator.SetInteger("Rotation", 5);
        }
        else if (rotation > -50 && rotation <= -30)
        {
            animator.SetInteger("Rotation", 6);
        }
        else if (rotation > -70 && rotation <= -50)
        {
            animator.SetInteger("Rotation", 7);
        }
        else if (rotation > -90 && rotation <= -70)
        {
            animator.SetInteger("Rotation", 8);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("D");
        transform.LookAt(Input.mousePosition);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cell")
        {
            currentCell = collision.GetComponent<Cell>();
            //collision.GetComponent<Cell>().isZergling = true;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Cell")
    //    {
    //        collision.GetComponent<Cell>().isZergling = false;
    //    }
    //}
}
