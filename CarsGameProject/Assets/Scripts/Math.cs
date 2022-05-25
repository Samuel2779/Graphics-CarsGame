using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Code a Math3D.cs library, with methods:
//static float Dot(Vector3 a, Vector3 b)
//static Vector3 Cross(Vector3 a, Vector3 b)
//static Vector3 Normalize(Vector3 v)
//static float Angle(Vector3 a, Vector3 b)
//static Vector3 ScalarMult(float alpha, Vector3 v)
//# Application:
//static Vector3 Reflect(Vector3 n, Vector3 v) # Reflect v around n
//# Show this reflection result in Unity

public class Math
{
	public static float Dot(Vector3 a, Vector3 b)
	{
		float res = a.x * b.x + a.y * b.y + a.z * b.z;
		return res;
	}
	public static Vector3 Cross(Vector3 a, Vector3 b)
	{
		Vector3 res = new Vector3(a.y * b.z - a.z - b.y , a.z*b.x-a.x*b.z  , a.x*b.y-a.y*b.x);
		return res;
	}

	public static float Magnitude(Vector3 v)
	{
		return Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
	}
	public static Vector3 Normalize(Vector3 v)
	{
		float mag = Magnitude(v);

		return new Vector3(v.x / mag, v.y / mag, v.z / mag);
	}
	public static float Angle(Vector3 a, Vector3 b)
	{
		Vector3 aU  = Normalize(a);
		Vector3 bU = Normalize(b);
		float dotP = Dot(aU, bU);
		float cos = Mathf.Acos(dotP);
		return Mathf.Rad2Deg * cos;
	}
	public static Vector3 ScalarMult(float alpha, Vector3 v)
	{
		return new Vector3(alpha * v.x, alpha * v.y, alpha * v.z);
	}
	public static Vector3 Reflect(Vector3 n, Vector3 v)
	{
		Vector3 nu = Normalize(n);
		Vector3 uPar = ScalarMult(Dot(nu, v), nu);
		Vector3 uOr = v - uPar;
		Vector3 refl = uPar - uOr;
		return refl;
	}
	public static Vector3 ShericalToCartesian(float i, float a,float r, Vector3 SC)
	{
		float ri = Mathf.Deg2Rad * i;
		float ra = Mathf.Deg2Rad * a;
		Vector3 c = Vector3.zero;
		c.x = SC.x + Mathf.Sin(ri) * Mathf.Sin(ra) * r;
		c.y = SC.y + Mathf.Cos(ri) * r;
		c.z = SC.z + Mathf.Sin(ri) * Mathf.Cos(ra) * r;
		return c;
	}

}
