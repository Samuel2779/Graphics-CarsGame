using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsMovement : MonoBehaviour
{
	List<Vector3> trajectory, curve;
	List<Vector3> movementPoints;
	public GameObject Car;
	GameObject CarCollider;
	public List<Vector3> TrajPoints;
	public List<Vector3> CurvPoints;
	Vector3[] originals, colliderChild;
	Vector3 pos; 
	bool flag = false;
	int index;
	float param;
	Vector3 start;
    Vector3 end;
	public KeyCode keyCode;

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
	Vector3 interpolation(Vector3 A, Vector3 B, float t){
        return A + t * (B-A);
    }
	// Start is called before the first frame update
	void Start()
    {
		CarCollider = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		trajectory = new List<Vector3>();
		curve = new List<Vector3>();
		movementPoints = new List<Vector3>();
		originals = Car.GetComponent<MeshFilter>().mesh.vertices;
		colliderChild = CarCollider.GetComponent<MeshFilter>().mesh.vertices;
		pos = Vector3.zero;
		start = Vector3.zero;
		end = Vector3.zero; 
		param = 0;
		index = 0;

		for (int i = 0; i < TrajPoints.Count; i++)
        {
            Vector3 point = TrajPoints[i];
            trajectory.Add(point);
        }
		for (int i = 0; i < CurvPoints.Count; i++)
		{
			Vector3 point2 = CurvPoints[i];
			curve.Add(point2);
		}


		List<Vector3> temp = new List<Vector3>();

		for (int i = 0; i < trajectory.Count-1; i++)
        {
            for (float x = 0; x < 1; x += 0.01f)
            {
                temp = new List<Vector3>();
                temp.Add(trajectory[i]);
                temp.Add(curve[i]);
                temp.Add(trajectory[i+1]);
                movementPoints.Add(EvalBeizer(temp, x));
            }
        }


	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(keyCode))
        {
        param += 0.0001f;
		start = movementPoints[index];
		end = movementPoints[index + 1];
        pos = interpolation(start,end, param);
		Vector3 prev = interpolation(start,end, param-0.0005f);
		Vector3 dir = pos -prev;
        Vector3 du = dir.normalized;
        float angle = Mathf.Rad2Deg*Mathf.Atan(du.z/du.x);
        Matrix4x4 r = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);
		Matrix4x4 t = Transformations.TranslateM(pos.x, pos.y, pos.z);
		Car.GetComponent<MeshFilter>().mesh.vertices = ApplyTransformation(originals, t * r);
		Matrix4x4 scSp = Transformations.ScaleM(3f, 3f, 3f);
		CarCollider.GetComponent<MeshFilter>().mesh.vertices = ApplyTransformation(colliderChild, t * r * scSp);
		index += 1;
		if (index == movementPoints.Count - 1){
			index = 0;
		}
		}
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

	void printRedDots()
    {
        for (int i = 0; i < movementPoints.Count; i++)
        {
            GameObject red_sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            red_sphere.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            red_sphere.transform.position = movementPoints[i];
        }
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
