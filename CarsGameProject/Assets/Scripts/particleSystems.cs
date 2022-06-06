using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Final Assignment, Particles

//Aldo ponce de la cruz A01651119
//Ignacio Alvarado Reyes A01656149
// Samuel Kareem Cueto Gonzalez A01656120	
//Angel Heredia Vazquez A01650574

public class particleSystems : MonoBehaviour
{
	public int N;
	List<GameObject> particles;
	List<GameObject> particlesCarList;
	public List<GameObject> carsMovement = new List<GameObject>();
	public List<GameObject> carParticles = new List<GameObject>();
	public Camera cam;
	float near;
	float far;
	float fwight;
	float fheight;
	bool RadarOn;
    // Start is called before the first frame update
    void Start()
    {
		cam = new Camera();
		cam = Camera.main;
		particles = new List<GameObject>();
		

		//Get particle intanciated in CarMovement, and add its forces, p, mass, etc.
		for (int i = 0; i < N; i++)
		{
			GameObject p = new GameObject();
			p.AddComponent<particlesClass>();
			particlesClass part = p.GetComponent<particlesClass>();
			float x = Random.Range(-113.59f, -130f);
			float y = Random.Range(11f, 12f);
			float z = Random.Range(8f, 12.0f);
			part.p = new Vector3(x, y, z);
			part.forces = Vector3.zero;
			part.forces.x = Random.Range(-5.0f, 5.0f);
			part.forces.z = Random.Range(-5.0f, 5.0f);
			part.r = Random.Range(1.2f, 1.5f);
			part.g = 9.81f;
			part.mass = part.r * 2f;
			part.rc = 0.1f;
			part.dragUp = 0.000001f;
			part.dragDown = 0.1f;
			
			particles.Add(p);
			
		}
		near = cam.nearClipPlane;
		far = cam.farClipPlane;
		fheight = 2.0f * near * Mathf.Tan(cam.fieldOfView* 0.5f * Mathf.Deg2Rad);
		fwight = fheight * cam.aspect;
		RadarOn = true;
		
	}

    // Update is called once per frame
    void Update()
    {
		
		for (int i = 0;i < carParticles.Count; i++)
		{
			float x = carParticles[i].GetComponent<MeshFilter>().mesh.vertices[0].x;
			float y = carParticles[i].GetComponent<MeshFilter>().mesh.vertices[0].y;
			float z = carParticles[i].GetComponent<MeshFilter>().mesh.vertices[0].z;
			Vector3 p = new Vector3(x, y, z);
			for (int j = 0; j < N; j++)
			{
				particlesClass c = particles[j].GetComponent<particlesClass>();
					//Get radious of the sphere colider
				if (CheckCollisionWithCarColider(c,p ,4f))
				{
					CarsMovement car = carsMovement[i].GetComponent<CarsMovement>();
					car.Health = car.Health - 1;
					car.HealthPrinter();
					// update force of c.sph
					Vector3 connectDirection = c.p - p;
					c.UpdateForces(connectDirection);
					c.sph.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 0f, 0f));
					StartCoroutine(DesRenderBall(c));
					


				}
				
				
			}

		}
        
    }
	IEnumerator DesRenderBall(particlesClass c)
	{
		
		yield return new WaitForSeconds(4f);
		c.sph.GetComponent<MeshRenderer>().enabled = false;

	}
	
	private bool CheckCollisionWithCarColider(particlesClass other, Vector3 current, float r)
	{
		float dx = other.p.x - current.x;
		float dy = other.p.y - current.y;
		float dz = other.p.z - current.z;

		float sumR = other.r + r;
		sumR *= sumR;

		return sumR > (dx * dx + dy * dy + dz * dz);
	}

	private void RadarVFC(particlesClass p)
	{
		Vector3 v = p.p - cam.transform.position;
		float d = Math.Magnitude(v);
		fheight = 2.0f * d * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
		fwight = fheight * cam.aspect;

		float dotx = Math.Dot(v, cam.transform.right);
		float doty = Math.Dot(v, cam.transform.up);
		float dotz = Math.Dot(v, cam.transform.forward);

		bool isZ = dotz > cam.nearClipPlane && dotz < cam.farClipPlane;
		bool isX = dotx > (-fwight) / 2 && dotx < fwight / 2;
		bool isY = doty > (- fheight) / 2 && doty < fheight /2 ;

		if (isZ && isX && isY)
		{
			//Render
			p.sph.GetComponent<MeshRenderer>().enabled = true;
		}
		else
		{
			//Not Render
			p.sph.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
