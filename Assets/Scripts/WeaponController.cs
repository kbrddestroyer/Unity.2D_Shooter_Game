using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Just an informational class

    [Header("Base information")]
    [SerializeField, Range(0f, 60f)] protected float shootingRate;
    [SerializeField, Range(0f, 10f)] protected float damageMultiplier;
    [SerializeField, Range(0f, 45f)] protected float randomAspect;
    [SerializeField, Range(0f, 10f)] protected float weaponReloadDelay;
    [SerializeField, Range(0, 220)] protected int ammoMax;
    [Header("Child objects")]
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform bulletSpawnPoint;
    [SerializeField] protected GameObject muzzleflash;
    [Header("HUD")]
    [SerializeField] protected Texture2D hudImage;
    [Header("Globals")]
    [SerializeField] protected int id;
    public float ShootingRate { get => shootingRate; }
    public float DamageMultiplier { get => damageMultiplier; }
    public float RandomAspect { get => randomAspect; }
    public float WeaponReloadDelay { get => weaponReloadDelay; }
    public int AmmoMax { get => ammoMax; }
    public GameObject BulletPrefab { get => bulletPrefab; }
    public Transform BulletSpawnPoint { get => bulletSpawnPoint; }
    public GameObject Muzzleflash { get => muzzleflash; }
    public Texture2D HUDImage { get => hudImage; }
    public int ID { get => id; }
}
