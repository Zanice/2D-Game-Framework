using UnityEngine;
using System.Collections;
using System;

public class KnifeEffect : ConeEffect {
    private readonly int DAMAGE = 10;

    public KnifeEffect(Entity owner, Vector2 position, Vector2 direction) : base(owner, position, direction) {
        ;
    }

    public override float Range {
        get {
            return 1.0f;
        }
    }

    public override int AngleInDegrees {
        get {
            return 90;
        }
    }

    public override void OnEntitiesHit(LinkedList<Entity> entitiesHit) {
        //TODO: REMOVE SELF FROM HIT
        if (entitiesHit.Size > 0) {
            Entity hitEntity = entitiesHit.GetFirst();
            hitEntity.TakeDamage(DAMAGE);
        }
    }
}
