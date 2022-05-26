using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject theCar;
    Vector3[] originals;
    public Vector3 start;
    public Vector3 end;
    Vector3 pos; 
    float param;

    Vector3 interpolation(Vector3 A, Vector3 B, float t){
        return A + t * (B-A);
    }

    // Start is called before the first frame update
    void Start()
    {
        param = 0;
        pos = start;
        originals = theCar.GetComponent<MeshFilter>().mesh.vertices;
        
    }

   Vector3[] ApplyTransformation(Matrix4x4 m,Vector3[] verts)
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

    // Update is called once per frame
    void Update()
    {
        param += 0.0001f;
        pos = interpolation(start,end, param);
        Vector3 prev = interpolation(start,end, param-0.00005f);
        Vector3 dir = pos -prev;
        Vector3 du = dir.normalized;
        float angle = Mathf.Rad2Deg*Mathf.Atan(du.z/du.x);
        Matrix4x4 r = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);

        Matrix4x4 t = Transformations.TranslateM(pos.x, pos.y, pos.z);
        theCar.GetComponent<MeshFilter>().mesh.vertices = ApplyTransformation(t * r, originals);
    }
}
