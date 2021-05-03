using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    // Hreyfing
    public float speed = 4;
    
    // Líf
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public Transform respawnPosition;
    public ParticleSystem hitParticle;
    
    // Skot
    public GameObject projectilePrefab;

    // Hljóð
    public AudioClip hitSound;
    public AudioClip shootingSound;
    
    // Líf
    public int health
    {
        get { return currentHealth; }  // Skilar núverandi lífi
    }
    
    // Hreyfing
    Rigidbody2D rigidbody2d;
    Vector2 currentInput;
    
    // Líf
    int currentHealth;
    float invincibleTimer;
    bool isInvincible;
   
    // Animation
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    
    // Hljóð
    AudioSource audioSource;
    
    void Start()
    {

        rigidbody2d = GetComponent<Rigidbody2D>();  // Næ í rigidbody leikmannsins
                
        invincibleTimer = -1.0f;  // Tími ódauðleika
        currentHealth = maxHealth;  // Set líf leikmanns sem hámarks líf
        
        animator = GetComponent<Animator>();  // Kalla í hlutinn sem sér um animation
        
        audioSource = GetComponent<AudioSource>();  // ´Kalla í hlutinn sem sér um hljóð
    }

    void Update()
    {
        // Líf
        if (isInvincible)  // Ef að ruby er óðaudleg því það var meitt hana
        {
            invincibleTimer -= Time.deltaTime;  // Mínusa deltatime frá tímanum sem ég stillti sem 2 sekúndur svo það er eins og að telja niður
            if (invincibleTimer < 0)  // Þegar tíminn verður minni en núll læt ég ódauðleikann á false svo ruby getur misst líf aftur
                isInvincible = false;
        }

        // Hreyfing
        float horizontal = Input.GetAxis("Horizontal");  // Næ í input frá notenda á x og y ásnum
        float vertical = Input.GetAxis("Vertical");
                
        Vector2 move = new Vector2(horizontal, vertical);  // Uppfæri staðsetningu ruby frá input notenda
        
        // Kíki hvort að ruby er kyrr með því að gá hvort að x ásinn sé mjög nálægt núll eins og 0.0000001
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);  // Ef Ruby er ekki kyrr læt ég hana horfa í rétta átt
            lookDirection.Normalize();  // Nota normalize til að láta lendina á 1
        }

        currentInput = move;


        // Animation

        animator.SetFloat("Look X", lookDirection.x);  // Sendi breytum í animator ný gildi
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // Skot

        if (Input.GetKeyDown(KeyCode.C))  // Ef notandi ýtir á C kalla ég á klasa sem skýtur skotinu
            LaunchProjectile();
        
        // Samskipti
        if (Input.GetKeyDown(KeyCode.X))  // Ef notandi ýtir á X
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, 1 << LayerMask.NameToLayer("NPC"));  // Held utan um hvort að það sem að Ruby horfir á sé froskurinn
            if (hit.collider != null)  // Ef að Ruby er að horfa á eitthvað
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();  // Kíki hvort það sem Ruby er að horfa á sé froskurinn
                if (character != null)  // Ef að hún er að horfa á froskinn
                {
                    character.DisplayDialog();  // Kalla í klasann hjá froskinum
                }  
            }
        }
 
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;  // Næ í stöðu rigidbodysins
        
        position = position + currentInput * speed * Time.deltaTime;  // Bæti við útreikningum til að fá lokastöðu ruby
        
        rigidbody2d.MovePosition(position);  // Færi ruby
    }

    // Klasi sem breytir lífi ruby og tekur inn fjölda til að draga frá lífi hennar
    public void ChangeHealth(int amount)
    {
        if (amount < 0)  // Ef að talan er mini en núll svo hægt sé að draga frá lífinu
        {
            if (isInvincible)  // Kíki fyrst hvort að ruby sé ódauðleg og skila þá engu
                return;

            isInvincible = true;  // Ef að hún er ekki ódauðleg læt ég hana verða það því að hún er að fara að missa líf
            invincibleTimer = timeInvincible;  // Endurstilli tímann sem hún er ódauðleg

            animator.SetTrigger("Hit");  // Segji animator að spila animation af henni meiða sig
            audioSource.PlayOneShot(hitSound);  // Spila hljóð

            Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);  // Læt smá sprengingu á staðsetningu hennar til að sýna að hún meiddi sig
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);  // Bæti við núverandi líf breytunni sem kemur með klasanum

        if (currentHealth == 0)  // Kíki hvort að lífið eru orðin 0
            Respawn();  // Læt þá ruby respawna

        UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);  // Uppfæri líf línunni í leiknum
    }

    void Respawn()
    {
        ChangeHealth(maxHealth);  // Endurstilli líf leikmannsins þegar að hann deyr
        // transform.position = respawnPosition.position; // Læt leikmanninn byrja upp á nýtt
        SceneManager.LoadScene(2);  // Skipti á leik lokið senuna
    }
    
    // Skot
    void LaunchProjectile()
    {
        // Geri afrit af skotinu
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        // Næ í skotið
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);  // Skýt skotinu áfram þar sem að Ruby er að horfa
        
        animator.SetTrigger("Launch");  // Spila skjótu animation
        audioSource.PlayOneShot(shootingSound);  // Spila skjótu hljóð
    }
    
    // hljóð
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);  // Spilar hljóð af inntaki klasans
    }
}
