using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable
{
    /*
     *  This should be parent class for every in-game sup-class
     *  Defines basic logic of player object
     */

    [Header("Base player settings")]
    [SerializeField, Range(0f, 25f)] protected float speed;
    [SerializeField, Range(0f, 50f)] protected float sprintSpeed;
    [SerializeField, Range(0f, 10f)] protected float mouseSens;
    [SerializeField, Range(0f, 50f)] protected float jumpForce;
    [SerializeField, Range(0f, 50f)] protected float maxHP;
    [SerializeField, Range(0f, 10f)] protected float raycastCheckRadius;
    [Header("Child controllers")]
    [SerializeField] protected Transform weaponAttachPoint;
    [SerializeField] protected Transform hands;
    [SerializeField] protected WeaponController attachedWeapon;
    [Header("GUI controllers")]
    [SerializeField] protected Animator GUI_FX;
    [SerializeField] protected TMP_Text hpLabel;
    [SerializeField] protected TMP_Text ammoLabel;
    [SerializeField] protected TMP_Text ammoStashLabel;
    [SerializeField] protected TMP_Text armorLabel;
    [SerializeField] protected RawImage weaponHUDImage;
    [Header("Globals")]
    [SerializeField] protected ListSpawnables weaponList;

    // Components
    protected Camera mainCamera;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected AudioSource stepSound;
    // Other
    protected bool canShoot = true;
    protected float shootingTimePassed = 0f;
    protected bool isOnGround = true;
    protected LayerMask groundMask;
    protected int ammo;
    protected int ammoInInventory;

    public int Ammo
    {
        get => ammo;
        set
        {
            ammo = value;
            ammoLabel.text = ammo.ToString();
        }
    }
    public int AmmoInInventory {
        get => ammoInInventory;
        set
        {
            ammoInInventory = value;
            ammoStashLabel.text = ammoInInventory.ToString();
        }
    }

    public bool IsOnGround { get => isOnGround; set
        {
            isOnGround = value;
            animator.SetBool("isJumping", !value);
        }
    }

    protected float hp;

    public float HP { get => hp; set 
        { 
            hp = value; 
            hpLabel.text = value.ToString();
            
            GUI_FX.SetTrigger("hurt");
            mainCamera.GetComponent<Animator>().SetTrigger("shake");

            if (hp <= 0)
            {
                Die();
            }
        } 
    }

    protected Vector3 mousePosition;
    public Vector3 MousePosition { get => mousePosition; }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raycastCheckRadius);
    }

    protected void OnDisable()
    {
        animator.SetFloat("speed", 0f);
        stepSound.enabled = false;
    }

    public void AttachWeapon(WeaponWorldObject weaponWorldObject)
    {
        if (attachedWeapon == null)
        {
            WeaponController weapon = Instantiate(weaponWorldObject.PlayerItemPrefab, hands).GetComponent<WeaponController>();
            attachedWeapon = weapon;
            weaponHUDImage.enabled = true;
            weaponHUDImage.texture = attachedWeapon.HUDImage;
            attachedWeapon.transform.localScale = weaponWorldObject.PlayerItemPrefab.transform.localScale;
            Ammo = weaponWorldObject.AmmoLeft;
            Destroy(weaponWorldObject.gameObject);
            animator.SetBool("hasWeapon", true);
            hands.gameObject.SetActive(true);
        }
    }

    public void DetachWeapon()
    {
        if (attachedWeapon != null)
        {
            WeaponWorldObject weaponWorldObject = (WeaponWorldObject) weaponList.get(attachedWeapon.ID);
            WeaponWorldObject worldObject = Instantiate(weaponWorldObject, transform.position, Quaternion.identity);
            worldObject.AmmoLeft = ammo;
            Ammo = 0;
            Destroy(attachedWeapon.gameObject);
            attachedWeapon = null;
            weaponHUDImage.enabled = false;
            weaponHUDImage.texture = null;
            animator.SetBool("hasWeapon", false);
            hands.gameObject.SetActive(false);
        }
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stepSound = GetComponent<AudioSource>();
        HP = maxHP;
        groundMask = LayerMask.GetMask("Ground");

        if (attachedWeapon != null) Ammo = attachedWeapon.AmmoMax;
    }

    public void Damage()
    {
        GUI_FX.SetTrigger("hurt");
        mainCamera.GetComponent<Animator>().SetTrigger("shake");
        HP -= 1;
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Reload()
    {
        if (attachedWeapon != null) StartCoroutine(ReloadCoroutine());
    }

    protected IEnumerator ReloadCoroutine()
    {
        canShoot = false;
        yield return new WaitForSeconds(attachedWeapon.WeaponReloadDelay);
        int delta = attachedWeapon.AmmoMax - Ammo;
        AmmoInInventory -= delta;
        if (AmmoInInventory < 0)
        {
            delta += AmmoInInventory;
            AmmoInInventory = 0;
        }
        Ammo += delta;
        canShoot = true;
    }

    protected virtual void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, raycastCheckRadius, groundMask);
        IsOnGround = (hit.collider != null);

        float speedX = Input.GetAxis("Horizontal");     // X-axis speed (moving, etc.)

        if (Mathf.Abs(speedX) > 0 && isOnGround) stepSound.enabled = true;
        else stepSound.enabled = false;

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.AddRelativeForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Q)) Damage();
        if (Input.GetKeyDown(KeyCode.G)) DetachWeapon();
        shootingTimePassed += Time.deltaTime;
        if (attachedWeapon != null && canShoot && ammo > 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                attachedWeapon.Muzzleflash.SetActive(true);
            else if (Input.GetKeyUp(KeyCode.Mouse0))
                attachedWeapon.Muzzleflash.SetActive(false);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (shootingTimePassed >= 1f / attachedWeapon.ShootingRate)
                {
                    GameObject ob = Instantiate(attachedWeapon.BulletPrefab, attachedWeapon.BulletSpawnPoint.position, attachedWeapon.BulletSpawnPoint.rotation);
                    ob.transform.Rotate(0f, 0f, Random.Range(-attachedWeapon.RandomAspect, attachedWeapon.RandomAspect));
                    Ammo--;
                    shootingTimePassed = 0f;
                }
            }
        }
        else if (attachedWeapon != null) attachedWeapon.Muzzleflash.SetActive(false);
        if (Input.GetKeyDown(KeyCode.R)) Reload();
        animator.SetBool("isSprinting", Input.GetKey(KeyCode.LeftShift));
        transform.position += new Vector3(speedX, 0, 0) * ((Input.GetKey(KeyCode.LeftShift)) ? sprintSpeed : speed) * Time.deltaTime;   // Main moving
                                                                                                                                        // Angle between cursor and weapon position
        Vector2 weaponScreenPosition = mainCamera.WorldToScreenPoint(weaponAttachPoint.position);
        Vector2 mousePosition = Input.mousePosition;
        mousePosition -= weaponScreenPosition;
        animator.SetFloat("speed", speedX * ((mousePosition.x < 0) ? -1 : 1));
        this.transform.localScale = new Vector3((mousePosition.x < 0) ? -1 : 1, 1, 1);
        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg - ((mousePosition.x < 0) ? 180 : 0);
        if (attachedWeapon != null) attachedWeapon.BulletSpawnPoint.localRotation = Quaternion.Euler(0f, 0f, (mousePosition.x < 0) ? 180 : 0);
        weaponAttachPoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
