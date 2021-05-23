using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    // Map
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    

    private PlayerMovement controls;
    private bool isMoving;
    private Vector3 origPos, targetPos;
    [SerializeField] private float timeToMove = 0.2f;
    [SerializeField] public int maxMoveSpaces;
    public bool canMove;

    private void Awake()
    {
        controls = new PlayerMovement();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxMoveSpaces = 8;
    }

    private void FixedUpdate()
    {
        
        {
            controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());

        }
    }

    private void Move(Vector2 direction)
    {

        if (CanMove(direction) && !isMoving)
            // transform.position += (Vector3)direction;
            StartCoroutine(MovePlayer ((Vector3) direction));
       

    }
    
    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
            return false;
        return true;

    }

    // This section relates to moving player and limiting movement
    private IEnumerator MovePlayer(Vector3 direction)
    {
        if (maxMoveSpaces > 0)
        {
            isMoving = true;
            float elapsedTime = 0;

            origPos = transform.position;
            targetPos = origPos + direction;
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPos;
            isMoving = false;
            maxMoveSpaces--;
        }
        
    }
}
