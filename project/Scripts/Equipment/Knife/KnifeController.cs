using UnityEngine;
using System.Collections;

public class KnifeController : EquipmentController {
    public KnifeController(PrefabRepository prefabRepo) : base(prefabRepo) {
        ;
    }

    public override string Name {
        get {
            return "Knife";
        }
    }

    public override int StartingAmmo {
        get {
            return -1;
        }
    }

    public override int MaxAmmo {
        get {
            return -1;
        }
    }

    public override void OnPrimaryAction(Entity owner, Vector3 targetLocation, bool downAction) {
        if (downAction) {
            Vector2 position = new Vector2(owner.transform.position.x, owner.transform.position.y);
            Vector3 direction = targetLocation - owner.transform.position;

            KnifeEffect effect = new KnifeEffect(owner, position, direction);
            LinkedList<Entity> entitiesHit = effect.DetermineEntitiesHit();

            entitiesHit.Remove(owner);

            effect.OnEntitiesHit(entitiesHit);
        }
    }

    public override void OnSecondaryAction(Entity owner, Vector3 targetLocation, bool downAction) {
        ;
    }

    public override void OnRemoteAction(Entity owner, Vector3 targetLocation, bool downAction) {
        ;
    }
}
