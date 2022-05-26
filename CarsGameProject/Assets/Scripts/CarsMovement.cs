using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsMovement : MonoBehaviour
{
	List<Vector3> cntrl;
	List<Vector3> travel;
	List<GameObject> sphepres;
	List<GameObject> travelSpheres;
	public GameObject Car;
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
		original = new List<Vector3[]>();
		travelSpheres = new List<GameObject>();
		sphepres = new List<GameObject>();
		travel = new List<Vector3>();
		cntrl = new List<Vector3>();
		//Add point of interest for curves
		Car = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//Transformation.cs

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	Vector3 EvalBeizer(List<Vector3> P, float t)
	{
		int n = P.Count;
		Vector3 p = Vector3.zero;
		for (int i = 0; i < n; i++)
		{
			p += Combination(n - 1, i) * Mathf.Pow(1.0f - t, n - 1 - i) *
				Mathf.Pow(t, i) * P[i];
		}
		return p;
	}
	float Combination(int n, int i)
	{
		return (float)(Factorial(n) / (Factorial(i) * Factorial(n - i)));
	}

	int Factorial(int n)
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
