using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    private Vector3 moveDir;

    public float moveSpeed = 5f;
    private Rigidbody rb;
    private bool running;
    private bool canMove;

    public List<Vector3> partyWaypoints;
    [SerializeField] private int MaximumPartyWaypoints = 0;

    void Start()
    {
        partyWaypoints = new List<Vector3>();
        rb = GetComponent<Rigidbody>();
    }

    public Vector3 GetWayPoint(int index)
    {
        return partyWaypoints[index];
    }

    public int GetWaypointSize()
    {
        return partyWaypoints.Count;
    }
    
    private void AddWayPoint()
    {
        if(moveDir != Vector3.zero)
        {
            if(partyWaypoints.Count > MaximumPartyWaypoints)
            {
                partyWaypoints.RemoveAt(0);
            }
            partyWaypoints.Add(transform.position);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("BattleScene");
        }
    }
    void Update()
    {
        AddWayPoint();

        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        rb.position += moveDir * (moveSpeed * Time.deltaTime);

    }


}
