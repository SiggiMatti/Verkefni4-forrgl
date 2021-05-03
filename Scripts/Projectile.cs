using UnityEngine;
public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;  // Rigidbody klasi
    
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();  // Næ í rigidbody skotsins
    }

    void Update()
    {
        // Eyði kúlunni ef hún fer 1000 units frá upprunalega skotstað
        if(transform.position.magnitude > 1000.0f)
            Destroy(gameObject);
    }

    // Kalla á þetta í RubyController til að skjóta kúlunni
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other) // Þegar að skotið snertir eitthvað
    {
        Enemy e = other.collider.GetComponent<Enemy>();  // Næ í óvin

        if (e != null)  // Ef að kúlan snerti óvin kalla ég á klasa í Enemy sem að sér um allt eftir það
        {
            e.Fix();
        }
        
        Destroy(gameObject);  // Ef að kúlan snerti ekki óvin eyði ég bara kúlunni
    }
}
