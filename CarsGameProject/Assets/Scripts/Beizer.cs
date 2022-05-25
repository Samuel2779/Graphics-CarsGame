using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Aldo ponce de la cruz A01651119
//Ignacio Alvarado Reyes A01656149
// Samuel Kareem Cueto Gonzalez A01656120	
//Angel Heredia Vazquez A01650574
public class Beizer : MonoBehaviour
{
	List<Vector3> cntrl;
	List<Vector3> travel;
	List<GameObject> sphepres;
	List<GameObject> travelSpheres;
	GameObject Ball;
	List<Vector3[]> original;
	bool flag = false;
	int index;

	Vector3[] ApplyTransformation(Vector3[] verts, Matrix4x4 m)
	{
		int number = verts.Length;
		Vector3[] result = new Vector3[number];


		for (int i = 0; i < number; i++)
		{
			Vector3 v = verts[i];
			Vector4 temp = new Vector4(v.x, v.y, v.z, 1);

			result[i] = m * temp;

		}
		return result;
	}
	// Start is called before the first frame update
	void Start()
	{
		Ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Ball.transform.position = new Vector3(0, 0, 0);
		Ball.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 1, 0));
		original = new List<Vector3[]>();
		original.Add(Ball.GetComponent<MeshFilter>().mesh.vertices);
		travelSpheres = new List<GameObject>();
		sphepres = new List<GameObject>();
		travel = new List<Vector3>();
		cntrl = new List<Vector3>();
		cntrl.Add(new Vector3(0, 0, 0));
		cntrl.Add(new Vector3(1, 5, 0));
		cntrl.Add(new Vector3(5, 10, 0));
		cntrl.Add(new Vector3(7, 8, 0));
		cntrl.Add(new Vector3(10, 0, 0));

		Vector3 test1 = EvalBeizer(cntrl, 0);
		Debug.Log(test1);
		Vector3 test2 = EvalBeizer(cntrl, 1.0f);
		Debug.Log(test2);
		index = 0;
		for (int i = 0; i < cntrl.Count; i++)
		{
			sphepres.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
			sphepres[i].transform.position = cntrl[i];
			sphepres[i].GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0));


		}
		

		for (int i = 0; i < 20; i++)
		{
			travel.Add(EvalBeizer(cntrl, i / 20.0f));
			Debug.Log(travel[i]);
			travelSpheres.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
			travelSpheres[i].transform.position = travel[i];
			travelSpheres[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			travelSpheres[i].GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 0, 1));

		}
		
	}

	// Update is called once per frame
	void Update()
	{
		Ball.GetComponent<MeshFilter>().mesh.vertices = original[0];
		if (!flag) {
			Matrix4x4 trans = Transformations.TranslateM(travel[index].x, travel[index].y, travel[index].z);
			Ball.GetComponent<MeshFilter>().mesh.vertices = ApplyTransformation(original[0], trans);
			index += 1;
			if (index == 20){
				flag = true;
				index -= 1;
			}
		}else
		{
			Matrix4x4 trans = Transformations.TranslateM(travel[index].x, travel[index].y, travel[index].z);
			Ball.GetComponent<MeshFilter>().mesh.vertices = ApplyTransformation(original[0], trans);
			index -= 1;
			if (index <= 0)
			{
				flag = false;
			}
		}
	}
	Vector3 EvalBeizer( List<Vector3> P, float t)
	{
		int n = P.Count;
		Vector3 p = Vector3.zero;
		for(int i = 0; i < n; i++)
		{
			p += Combination(n- 1, i) * Mathf.Pow(1.0f - t, n- 1 - i) *
				Mathf.Pow(t, i) * P[i];
		}
		return p;
	}

	float Combination(int n, int i)
	{
		return (float)(Factorial(n) / (Factorial(i) * Factorial(n - i)));
	}

	int Factorial (int n)
	{
		if (n == 0)
		{
			return 1;
		}
		else
		{
			return n * Factorial(n - 1);
		}
	}
}
