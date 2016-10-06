using UnityEngine;
using System.Collections;
using System;

public class TazerController : EquipmentController {
    public TazerController(PrefabRepository prefabRepo) : base(prefabRepo) {
        ;
    }

    public override string Name {
        get {
            return "Tazer";
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
            Vector3 direction = targetLocation - owner.transform.position;

            GameObject tazerBoltPrefab = GetPrefabRepository().PrefabOfTazerBolt();
            GameObject tazerBolt = GameObject.Instantiate(tazerBoltPrefab, owner.transform.FindChild("Aim Point").position, Quaternion.identity) as GameObject;
            tazerBolt.GetComponent<TazerBolt>().StartFlight(owner.GetComponent<Player>(), direction);
        }
    }

    public override void OnSecondaryAction(Entity owner, Vector3 targetLocation, bool downAction) {
        ;
    }

    public override void OnRemoteAction(Entity owner, Vector3 targetLocation, bool downAction) {
        ;
    }
}
