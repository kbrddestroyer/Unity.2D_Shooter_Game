using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WeaponWorldObject : WorldObjectController
{
    protected int ammoLeft;

    public int AmmoLeft { get => ammoLeft; set => ammoLeft = value; }

    protected override void Pickup()
    {
        localPlayer.AttachWeapon(this);
    }
}
