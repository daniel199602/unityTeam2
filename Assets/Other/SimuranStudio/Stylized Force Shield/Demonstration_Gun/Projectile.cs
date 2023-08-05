using UnityEngine;

public class Projectile : MonoBehaviour
{

	public	float		speed	=	10;

	public	GameObject	muzzle;
	public	GameObject	explosion;

	void Start ()
    {
        if (muzzle != null)
			Instantiate(muzzle, transform.position, Quaternion.LookRotation(transform.forward));
    }


	//
    void Update()
    {
        transform.position	=	transform.position + transform.forward * (speed * Time.deltaTime);
    }

	void OnCollisionEnter (Collision _collider) 
	{
		Destroy(transform.gameObject);

		if (explosion != null)
			Instantiate(explosion, transform.position, Quaternion.LookRotation(transform.forward * -1f));
	}

	//	end
}
