using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Final Assignment, Particles

//Aldo ponce de la cruz A01651119
//Ignacio Alvarado Reyes A01656149
// Samuel Kareem Cueto Gonzalez A01656120	
//Angel Heredia Vazquez A01650574



public class particlesClass : MonoBehaviour
{
	public float mass;
	public float g;         // gravedad
	public float r;         // radio
	public float rc;        // Restitution Coefficient (elastic=1, inelastic = 0)
	public Vector3 p;       // position
	public Vector3 forces;
	public Color color;
	public Vector3 a;       // acceleration
	public float dragUp;
	public float dragDown;

	public Vector3 drag;
	public Vector3 prev;       // previous position
	Vector3 temp;       // temporal position
	public GameObject sph;     // game object for the particle

	// Start is called before the first frame update
	void Start()
	{
		sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sph.transform.position = p;
		sph.transform.localScale = new Vector3(r * 2, r * 2, r * 2);
		float cr = Random.Range(0.0f, 1.0f);
		float cg = Random.Range(0.0f, 1.0f);
		float cb = Random.Range(0.0f, 1.0f);
		color = new Color(cr, cg, cb);
		sph.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
		prev = p;
		a = Vector3.zero;
		drag.y = 1;
	}

	void Verlet(float dt)
	{
		temp = p;                           // save p temporarily
		a = forces / mass;                  // a = F/m
		p = 2 * p - prev + (a * dt * dt);   // Verlet
		prev = temp;                        // restore previous position
		sph.transform.position = p;
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.frameCount > 20)
		{
			forces.y += -g * mass * Time.deltaTime;
			if (p.y > prev.y) drag = -forces * dragUp;
			else if (p.y < prev.y) drag = -forces * dragDown;
			else drag = Vector3.zero;
			forces += drag;
			CheckFloor();
			CheckWalls();
			Verlet(0.03f);
		}
	}

	void CheckFloor()
	{
		if (p.y < r)
		{
			forces.y = -forces.y * rc;
			//Debug.Log("F=" + forces.ToString("F5"));
			float diff = prev.y - p.y;
			p.y = r;
			prev.y = r - diff;
		}
	}
	void CheckWalls()
	{
		if(p.x > -112f - r)
		{
			forces.x = -forces.x * rc;
			float diff = prev.x - p.x;
			p.x = -112f - r;
			prev.x = -112f - r - diff;
		}
		
		else if (p.x < -133f + r)
		{
			forces.x = -forces.x * rc;
			float diff = prev.x - p.x;
			p.x = -131f + r;
			prev.x = -131f + r - diff;
		}
		if (p.z > 13f - r)
		{
			forces.z = -forces.z * rc;
			float diff = prev.z - p.z;
			p.z = 13f - r;
			prev.z = 13f - r - diff;
		}
		else if (p.z < 3f  - r)
		{
			forces.z = -forces.z * rc;
			float diff = prev.z - p.z;
			p.z = 3f - r;
			prev.z = 3f - r - diff;
		}
		if (p.y > 15f - r)
		{
			forces.y = -forces.y * rc;
			//Debug.Log("F=" + forces.ToString("F5"));
			float diff = prev.y - p.y;
			p.y = 15f - r;
			prev.y = 15f - r - diff;
		}
		
	}

	public void UpdateForces(Vector3 dir)
	{
		forces = -forces * rc;
		Vector3 diff = prev - p;
		//update Positions
		p.x = prev.x - r;
		p.y = prev.y - r;
		p.z = prev.z - r;

		prev.x  = prev.x - diff.x;
		prev.y = prev.y - diff.y;
		prev.z = prev.z - diff.z;


	}

	public bool CheckCollision(particlesClass other)
	{
		float dx = other.p.x - p.x;
		float dy = other.p.y - p.y;
		float dz = other.p.z - p.z;

		float sumR = other.r + r;
		sumR *= sumR;

		return sumR > (dx * dx + dy * dy + dz * dz);
	}
}