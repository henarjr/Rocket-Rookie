using UnityEngine;

public class ObstaclesMovments : MonoBehaviour
{
    public float minSize = 0.4f;
    public float maxSize = 0.8f;
    public float maxSpeed = 400f;
    public float minSpeed = 150f;
    public float maxSpinSpeed = 1f;

    public GameObject bounceEffectPrefab;
    Rigidbody2D rb;
    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        transform.localScale = new Vector3(randomSize, randomSize, 1);    

        rb = GetComponent<Rigidbody2D>();

        Vector2 randomDirection = Random.insideUnitCircle;
        rb.AddForce(randomDirection * randomSpeed);

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float impactSpeed = rb.linearVelocity.magnitude;

        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);

        float effectScale = Mathf.Clamp(impactSpeed / 5f, 0.5f, 2f);
        bounceEffect.transform.localScale = Vector3.one * effectScale;

        // Destroy the effect after 1 second
        Destroy(bounceEffect, 1f);
    }
}
