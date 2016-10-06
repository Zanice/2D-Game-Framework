using UnityEngine;
using System.Collections;

public class PlayerMotionController : MonoBehaviour {
    private readonly float BASE_SPEED = .1f;
    private readonly float DIAGONAL_MOVEMENT_SCALE = (float) 1 / Mathf.Sqrt(2);

    //private IGridRepository _gridRepository;
    private MovementCollisionManager collisionManager;
    private bool canMove;
    private bool movingLeft;
    private bool movingRight;
    private bool movingUp;
    private bool movingDown;

    void Start() {
        canMove = true;
        movingLeft = movingRight = movingUp = movingDown = false;
        collisionManager = GetComponent<MovementCollisionManager>();
    }

    public void LinkReferences(GridRepository gridRepo) {
        //_gridRepository = gridRepo;
    }

    public bool CanMove {
        get {
            return canMove;
        }
        set {
            canMove = value;
        }
    }

    public bool MovingLeft {
        get {
            return movingLeft;
        }
    }

    public bool MovingRight {
        get {
            return movingRight;
        }
    }

    public bool MovingUp {
        get {
            return movingUp;
        }
    }

    public bool MovingDown {
        get {
            return movingDown;
        }
    }

    void Update() {
        if (canMove) {
            DetermineMovementDirections();
            MoveAlongDirections();
        }
    }

    public void TranslatePlayer(Vector2 positionChange) {
        float prevX = transform.position.x;
        float prevY = transform.position.y;
        float prevZ = transform.position.z;

        transform.position = new Vector3(prevX + positionChange.x, prevY + positionChange.y, prevZ);
    }

    private void DetermineMovementDirections() {
        bool inputLeft = ActiveMoveLeftInput();
        bool inputRight = ActiveMoveRightInput();
        bool inputUp = ActiveMoveUpInput();
        bool inputDown = ActiveMoveDownInput();

        //Determine movement logic along the horizontal axis
        if (inputLeft && inputRight) {
            movingLeft = false;
            movingRight = false;
        }
        else {
            movingLeft = inputLeft;
            movingRight = inputRight;
        }

        //Determine movement logic along the vetical axis
        if (inputUp && inputDown) {
            movingUp = false;
            movingDown = false;
        }
        else {
            movingUp = inputUp;
            movingDown = inputDown;
        }
    }

    private bool ActiveMoveLeftInput() {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }

    private bool ActiveMoveRightInput() {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }

    private bool ActiveMoveUpInput() {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    }

    private bool ActiveMoveDownInput() {
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }

    private void MoveAlongDirections() {
        //Determine the player's magnitude of movement (speed)
        float movementMagnitude = DetermineMovementMagnitude();

        //Determine the axis/axes the player is moving along
        bool horizontalMovement = movingLeft ^ movingRight;
        bool verticalMovement = movingUp ^ movingDown;
        bool diagonalMovement = horizontalMovement && verticalMovement;

        //Clip the movement magnitude for diagonal movement, if applicable
        if (diagonalMovement)
            movementMagnitude *= DIAGONAL_MOVEMENT_SCALE;

        //Determine the movement vector for the player, clipping for possible obstacles
        float magnitudeLeft = movingLeft ? ClipMovementToPossibleObstaclesLeft(movementMagnitude) : 0;
        float magnitudeRight = movingRight ? ClipMovementToPossibleObstaclesRight(movementMagnitude) : 0;
        float magnitudeUp = movingUp ? ClipMovementToPossibleObstaclesAbove(movementMagnitude) : 0;
        float magnitudeDown = movingDown ? ClipMovementToPossibleObstaclesBelow(movementMagnitude) : 0;

        //Construct the final vector and move the player
        Vector2 movement = new Vector2(magnitudeRight - magnitudeLeft, magnitudeUp - magnitudeDown);
        TranslatePlayer(movement);
    }

    private float DetermineMovementMagnitude() {
        return BASE_SPEED;
    }

    private float ClipMovementToPossibleObstaclesLeft(float magnitude) {
        Tile colliderTile;
        TileObstacleBuffer colliderBuffer;
        float collisionEdgeRight;
        float playerEdgeLeft;
        float collisionEdgeAbove;
        float playerEdgeBelow;
        float collisionEdgeBelow;
        float playerEdgeAbove;
        bool crossRight;
        bool crossAbove;
        bool crossBelow;

        for (int tile = -1; tile < 2; tile++) {
            colliderTile = TileAt(-1, tile);
            colliderBuffer = colliderTile.Buffer;
            if (!colliderBuffer.IsDeadBuffer()) {
                //Determine collision edges for obstacle left of player
                collisionEdgeRight = colliderTile.BufferWorldPosition.x + colliderBuffer.Buffer.x;
                playerEdgeLeft = transform.position.x - collisionManager.EntityBuffer - magnitude;

                //Determine collision edges for obstacle below player
                collisionEdgeAbove = colliderTile.BufferWorldPosition.y + colliderBuffer.Buffer.y;
                playerEdgeBelow = transform.position.y - collisionManager.EntityBuffer;

                //Determine collision edges for obstacle above player
                collisionEdgeBelow = colliderTile.BufferWorldPosition.y - colliderBuffer.Buffer.y;
                playerEdgeAbove = transform.position.y + collisionManager.EntityBuffer;

                crossRight = playerEdgeLeft < collisionEdgeRight;
                crossAbove = playerEdgeBelow < collisionEdgeAbove;
                crossBelow = collisionEdgeBelow < playerEdgeAbove;

                if (crossRight && crossAbove && crossBelow)
                    magnitude = Mathf.Clamp(playerEdgeLeft - collisionEdgeRight, 0, magnitude);
            }
        }

        return magnitude;
    }

    private float ClipMovementToPossibleObstaclesRight(float magnitude) {
        Tile colliderTile;
        TileObstacleBuffer colliderBuffer;
        float collisionEdgeLeft;
        float playerEdgeRight;
        float collisionEdgeAbove;
        float playerEdgeBelow;
        float collisionEdgeBelow;
        float playerEdgeAbove;
        bool crossLeft;
        bool crossAbove;
        bool crossBelow;

        for (int tile = -1; tile < 2; tile++) {
            colliderTile = TileAt(1, tile);
            colliderBuffer = colliderTile.Buffer;
            if (!colliderBuffer.IsDeadBuffer()) {
                //Determine collision edges for obstacle left of player
                collisionEdgeLeft = colliderTile.BufferWorldPosition.x - colliderBuffer.Buffer.x;
                playerEdgeRight = transform.position.x + collisionManager.EntityBuffer + magnitude;

                //Determine collision edges for obstacle below player
                collisionEdgeAbove = colliderTile.BufferWorldPosition.y + colliderBuffer.Buffer.y;
                playerEdgeBelow = transform.position.y - collisionManager.EntityBuffer;

                //Determine collision edges for obstacle above player
                collisionEdgeBelow = colliderTile.BufferWorldPosition.y - colliderBuffer.Buffer.y;
                playerEdgeAbove = transform.position.y + collisionManager.EntityBuffer;

                crossLeft = collisionEdgeLeft < playerEdgeRight;
                crossAbove = playerEdgeBelow < collisionEdgeAbove;
                crossBelow = collisionEdgeBelow < playerEdgeAbove;

                if (crossLeft && crossAbove && crossBelow)
                    magnitude = Mathf.Clamp(collisionEdgeLeft - playerEdgeRight, 0, magnitude);
            }
        }

        return magnitude;
    }

    private float ClipMovementToPossibleObstaclesAbove(float magnitude) {
        Tile colliderTile;
        TileObstacleBuffer colliderBuffer;
        float collisionEdgeLeft;
        float playerEdgeRight;
        float collisionEdgeRight;
        float playerEdgeLeft;
        float collisionEdgeBelow;
        float playerEdgeAbove;
        bool crossRight;
        bool crossLeft;
        bool crossBelow;

        for (int tile = -1; tile < 2; tile++) {
            colliderTile = TileAt(tile, 1);
            colliderBuffer = colliderTile.Buffer;
            if (!colliderBuffer.IsDeadBuffer()) {
                //Determine collision edges for obstacle left of player
                collisionEdgeLeft = colliderTile.BufferWorldPosition.x - colliderBuffer.Buffer.x;
                playerEdgeRight = transform.position.x + collisionManager.EntityBuffer;

                collisionEdgeRight = colliderTile.BufferWorldPosition.x + colliderBuffer.Buffer.x;
                playerEdgeLeft = transform.position.x - collisionManager.EntityBuffer;

                //Determine collision edges for obstacle above player
                collisionEdgeBelow = colliderTile.BufferWorldPosition.y - colliderBuffer.Buffer.y;
                playerEdgeAbove = transform.position.y + collisionManager.EntityBuffer + magnitude;

                crossLeft = collisionEdgeLeft < playerEdgeRight;
                crossRight = playerEdgeLeft < collisionEdgeRight;
                crossBelow = collisionEdgeBelow < playerEdgeAbove;

                if (crossBelow && crossLeft && crossRight)
                    magnitude = Mathf.Clamp(collisionEdgeBelow - playerEdgeAbove, 0, magnitude);
            }
        }

        return magnitude;
    }

    private float ClipMovementToPossibleObstaclesBelow(float magnitude) {
        Tile colliderTile;
        TileObstacleBuffer colliderBuffer;
        float collisionEdgeLeft;
        float playerEdgeRight;
        float collisionEdgeRight;
        float playerEdgeLeft;
        float collisionEdgeAbove;
        float playerEdgeBelow;
        bool crossRight;
        bool crossLeft;
        bool crossAbove;

        for (int tile = -1; tile < 2; tile++) {
            colliderTile = TileAt(tile, -1);
            colliderBuffer = colliderTile.Buffer;
            if (!colliderBuffer.IsDeadBuffer()) {
                //Determine collision edges for obstacle left of player
                collisionEdgeLeft = colliderTile.BufferWorldPosition.x - colliderBuffer.Buffer.x;
                playerEdgeRight = transform.position.x + collisionManager.EntityBuffer;

                collisionEdgeRight = colliderTile.BufferWorldPosition.x + colliderBuffer.Buffer.x;
                playerEdgeLeft = transform.position.x - collisionManager.EntityBuffer;

                //Determine collision edges for obstacle below player
                collisionEdgeAbove = colliderTile.BufferWorldPosition.y + colliderBuffer.Buffer.y;
                playerEdgeBelow = transform.position.y - collisionManager.EntityBuffer - magnitude;

                crossLeft = collisionEdgeLeft < playerEdgeRight;
                crossRight = playerEdgeLeft < collisionEdgeRight;
                crossAbove = playerEdgeBelow < collisionEdgeAbove;

                if (crossAbove && crossLeft && crossRight)
                    magnitude = Mathf.Clamp(playerEdgeBelow - collisionEdgeAbove, 0, magnitude);
            }
        }

        return magnitude;
    }

    private Tile TileAt(int relativeX, int relativeY) {
        return collisionManager.DetectionSample[relativeX + 1, relativeY + 1];
    }

    private bool ObstacleAt(int relativeX, int relativeY) {
        return collisionManager.ObstacleSample[relativeX + 1, relativeY + 1];
    }
}
