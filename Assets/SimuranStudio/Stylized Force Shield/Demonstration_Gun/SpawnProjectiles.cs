using UnityEngine;
using UnityEngine.UI;

public class SpawnProjectiles : MonoBehaviour 
{
	public GameObject		firePoint;

    //	delay between shots
    public float			fireRate		=	1;

	public Text				textfieldName;

	public GameObject		effect;

    //	massive effects
    public GameObject[]		muzzles;

	private int				selectIndex_		= 0;

    //	clamped the mouse or SPACE (turned on shooting)
    private bool			powerOn_;

	private float			timeout_;

	void Start () 
	{
		if (muzzles != null && muzzles.Length > 0)
			effect			=	muzzles[0];

		if (textfieldName != null && muzzles.Length > 0)
            textfieldName.text =	effect.name; 
	}



	void Update () 
	{
        //	length of the beam from the screen
        var maximumLenght	=	1000;

		RaycastHit hit;
		var mousePos	= Input.mousePosition;
		var rayMouse	= Camera.main.ScreenPointToRay(mousePos);

        //	cursor over the object
        if (Physics.Raycast (rayMouse.origin, rayMouse.direction, out hit, maximumLenght)) 
		{
            //	hit.point	-	point of collision of the beam and the object

            transform.rotation	=	Quaternion.LookRotation(hit.point - transform.position);
		}
        //	if the cursor is hovered over a void
        else
        {

		}


		var	powerOn		=	Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        //	switch effect
        if (powerOn_ != powerOn) {
			powerOn_	=	powerOn;

			//Shoot();
		}

		timeout_	-=	Time.deltaTime;

		if (effect != null && powerOn)
		{
			if (timeout_ <= 0)
			{
				timeout_	=	fireRate;
				Shoot();
			}
		}

        if (muzzles.Length > 0)
        {
			if (Input.GetKeyDown(KeyCode.D))
                Next();

            if (Input.GetKeyDown(KeyCode.A))
                Previous();
        }
	}

	public void Shoot () 
	{
		if (effect != null)
			Instantiate (effect, firePoint.transform.position, Quaternion.LookRotation(transform.forward));
	}

    //	switching forward
    public void Next () 
	{
		selectIndex_++;

		if (selectIndex_ > muzzles.Length-1)
			selectIndex_ = 0;

		effect = muzzles [selectIndex_];

		if (textfieldName != null)	
			textfieldName.text = effect.name;
	}

    //	switching back
    public void Previous () 
	{
		selectIndex_--;

		if (selectIndex_ < 0)
			selectIndex_ = muzzles.Length-1;

		effect = muzzles[selectIndex_];

		if (textfieldName != null)	
			textfieldName.text = effect.name;
	}

	//	end
}
