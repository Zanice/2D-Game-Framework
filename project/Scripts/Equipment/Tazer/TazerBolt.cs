using UnityEngine;
using System.Collections;
using System;

public class TazerBolt : Projectile {
    private readonly int DAMAGE = 10;

    public override float Range {
        get {
            return 0.0f;
        }
    }

    public override float Speed {
        get {
            return 5.0f;
        }
    }

    public override bool CheckObstacle() {
        CoordinatePair currentCoordinates = GridRepository.Instance.GridCoordinatesFromPosition(transform.position);
        Tile currentTile = GridRepository.Instance.GetTileAt(currentCoordinates.x, currentCoordinates.y);

        return currentTile.Type.Obstacle == TileType.TileType_Obstacle.WALL;
    }

    public override void ActDuringFlight() {
        //TODO: REMOVE SELF FROM HIT
        LinkedList<Entity> entitiesHit = DetermineEntitiesHit();
        if (entitiesHit.Size != 0)
            OnEntitiesHit(entitiesHit);
    }

    public override void OnEntitiesHit(LinkedList<Entity> entitiesHit) {
        foreach (Entity entity in entitiesHit) {
            entity.TakeDamage(DAMAGE);
        }

        GameObject.Destroy(gameObject);
    }
}
