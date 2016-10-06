using UnityEngine;
using System.Collections;

public abstract class EquipmentController {
    private PrefabRepository prefabRepo;
    private int ammo;

    public EquipmentController(PrefabRepository prefabRepo) {
        this.prefabRepo = prefabRepo;
        ammo = StartingAmmo;
    }

    /// <summary>
    /// Represents the current ammunition count of the equipment. A value of -1 signifies infinite ammunition.
    /// </summary>
    public int Ammo {
        get {
            return ammo;
        }
    }

    /// <summary>
    /// Equipment-controller-specific value for the equipment's name.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Equipment-controller-specific value for the initial ammunition count of the equipment.
    /// </summary>
    public abstract int StartingAmmo { get; }

    /// <summary>
    /// Equipment-controller-specific value for the maximum amount of ammunition the equipment can have.
    /// </summary>
    public abstract int MaxAmmo { get; }

    /// <summary>
    /// Increases the ammunition of the equipment by the specified amount, capping at the maximum ammunition allowed for the equipment.
    /// </summary>
    /// <param name="amount">The amount of ammunition to gain.</param>
    public void AddAmmo(int amount) {
        ammo += amount;
        if (ammo > MaxAmmo)
            ammo = MaxAmmo;
    }

    /// <summary>
    /// Attempts to reduce the ammunition of the equipment by the specified amount. Returns true if the reduction was successful, or returns false if
    /// the current ammount of ammunition is not sufficient to reduce the specified amount from.
    /// </summary>
    /// <param name="amount">The amount of ammunition to lose.</param>
    /// <returns>True if the reduction was successful, false if the reduction could not occur.</returns>
    public bool ReduceAmmo(int amount) {
        if (ammo - amount >= 0) {
            ammo -= amount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns true if the equipment has enough ammunition to be used, including if the equipment currently has infinite ammunition.
    /// </summary>
    /// <param name="ammoCost">The ammunition cost associated with using the equipment.</param>
    /// <returns>True if the equipment can be used.</returns>
    public bool CanFire(int ammoCost) {
        return ammo == -1 || ammo - ammoCost >= 0;
    }

    /// <summary>
    /// Returns the PrefabRepository reference this class is linked to.
    /// </summary>
    /// <returns></returns>
    protected PrefabRepository GetPrefabRepository() {
        return prefabRepo;
    }

    /// <summary>
    /// Equipment-controller-specific process to run when the equipment's primary action is attempted.
    /// </summary>
    public abstract void OnPrimaryAction(Entity owner, Vector3 targetLocation, bool downAction);

    /// <summary>
    /// Equipment-controller-specific process to run when the equipment's primary secondary is attempted.
    /// </summary>
    public abstract void OnSecondaryAction(Entity owner, Vector3 targetLocation, bool downAction);

    /// <summary>
    /// Equipment-controller-specific process to run when the equipment's remote action is attempted.
    /// </summary>
    public abstract void OnRemoteAction(Entity owner, Vector3 targetLocation, bool downAction);
}
