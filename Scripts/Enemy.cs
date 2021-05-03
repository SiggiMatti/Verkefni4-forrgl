using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	// Hreyfing
	public float speed;
	public float timeToChange;  
	public bool horizontal; 

	public GameObject smokeParticleEffect;
	public ParticleSystem fixedParticleEffect;

	public AudioClip hitSound;
	public AudioClip fixedSound;
	
	Rigidbody2D rigidbody2d;
	float remainingTimeToChange;
	Vector2 direction = Vector2.right;
	bool repaired = false;
	
	// Animation
	Animator animator;
	
	// Hljóð
	AudioSource audioSource;
	
	void Start ()
	{
		rigidbody2d = GetComponent<Rigidbody2D>();  // Sæki rigidbody óvinins
		remainingTimeToChange = timeToChange;  // Set tímann sem það tekur fyrir óvin að skipta um átt og læt hann í aðra breytu

		direction = horizontal ? Vector2.right : Vector2.down;  // Set upp áttina

		animator = GetComponent<Animator>();  // Næ í animator hlutinn

		audioSource = GetComponent<AudioSource>();  // N´æ í audiosource hlutinn
	}
	
	void Update()
	{
		if(repaired)
			return;
		
		remainingTimeToChange -= Time.deltaTime;  // Tel niður tíma með því að draga frá deltatime við tímann á hverjum ramma

		if (remainingTimeToChange <= 0)  // Ef tíminn fer fyrir neðan 0
		{
			remainingTimeToChange += timeToChange;  // Endurstilli tímann
			direction *= -1;  // Læt óvininn fara í öfuga átt með því að margfalda áttinii með -1
		}

		animator.SetFloat("ForwardX", direction.x);  // Sendi breytunum sem sjá um hvernig óvinurinn er animataður ný gildi
		animator.SetFloat("ForwardY", direction.y);
	}

	void FixedUpdate()
	{
		rigidbody2d.MovePosition(rigidbody2d.position + direction * speed * Time.deltaTime);  // Hér færi ég óvininn sjálfan
	}

	void OnCollisionStay2D(Collision2D other)  // Þegar að eitthhvað snertir óvininn
	{
		if(repaired)
			return;
		
		RubyController controller = other.collider.GetComponent<RubyController>();  // Næ í scriptuna sem er aðeins á og stjórnar ruby
		
		if(controller != null)  // Ef að hluturinn sem snerti óvininn hefur RubyController scriptuna
			controller.ChangeHealth(-1);  // Þá læt ég Ruby missa líf
	}

	public void Fix()
	{
		animator.SetTrigger("Fixed");  // Spila animation þegar að óvinurinn deyr
		repaired = true;  // læt repaired á true svo að allar scriptur hætta að keyra
		
		smokeParticleEffect.SetActive(false);  // Tek í burt reykinn

		Instantiate(fixedParticleEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);  // læt nýtt effect á óvininn

		// Læt þetta á false svo að óvinur gerir ekkert og getur ekki gert neitt við neinn
		rigidbody2d.simulated = false;
		
		audioSource.Stop();  // Stoppa gamla hljóðið og spila ný hljóð
		audioSource.PlayOneShot(hitSound);
		audioSource.PlayOneShot(fixedSound);
	}
}
