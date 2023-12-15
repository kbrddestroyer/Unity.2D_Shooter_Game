using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] protected float lifetime;
    [SerializeField, Range(0f, 100f)] protected float baseSpeed;

    protected float damageMultiplier = 1f;
    public float DamageMultiplier { get => damageMultiplier; set { damageMultiplier = value; } }
    protected float passedTime = 0f;

    public void Update()
    {
        transform.position += transform.right * Time.deltaTime * baseSpeed;
        passedTime += Time.deltaTime;
        if (passedTime >= lifetime)
            Destroy(this.gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground") Destroy(this.gameObject);

        IDamagable damagable = collision.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.HP -= damageMultiplier;
            Destroy(this.gameObject);
        }
    }
}
