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
		for(int i = 0; i < N; i++)
		{
			GameObject p = new GameObject();
			p.AddComponent<particlesClass>();
			particlesClass part = p.GetComponent<particlesClass>();
			float x = Random.Range(-7.0f, 7.0f);
			float y = Random.Range(10f, 14.5f);
			float z = Random.Range(-7.0f, 7.0f);
			part.p = new Vector3(x, y, z);
			part.forces = Vector3.zero;
			part.forces.x = Random.Range(-5.0f, 5.0f);
			part.forces.z = Random.Range(-5.0f, 5.0f);
			part.r = Random.Range(0.2f, 0.5f);
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
		if (Input.GetKey("a"))
		{
			cam.transform.RotateAround(cam.transform.position, Vector3.up, -50.0f * Time.deltaTime);
		}
		if (Input.GetKey("d"))
		{
			cam.transform.RotateAround(cam.transform.position, Vector3.up, 50.0f * Time.deltaTime);
		}
		if (Input.GetKeyDown("s"))
		{
			RadarOn = !RadarOn;
		}

		bool flag = false;
		for(int i = 0;i < N; i++)
		{
			flag = false;
			particlesClass p = particles[i].GetComponent<particlesClass>();
			if (RadarOn)
			{
				RadarVFC(p);
			}else
			{
				p.sph.GetComponent<MeshRenderer>().enabled = true;
			}
			
			Color originalColor = p.color;
			for (int j = 0; j < N; j++)
			{
				if(j != i)
				{
					particlesClass c = particles[j].GetComponent<particlesClass>();
					if (p.CheckCollision(c))
					{
						p.sph.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 0f, 0f));
						c.sph.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 0f, 0f));
						flag = true;
					}
				}
				
			}
			if (!flag)
			{
				//return original color
				p.sph.GetComponent<MeshRenderer>().material.SetColor("_Color", originalColor);
			}
		}
        
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
