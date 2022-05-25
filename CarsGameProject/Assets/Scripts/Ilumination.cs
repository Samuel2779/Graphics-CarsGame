using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ilumination : MonoBehaviour
{
	Dictionary<string, Vector3> vectors;
	Dictionary<string, float> scalars;
	// Start is called before the first frame update
	void Start()
    {
		vectors = new Dictionary<string, Vector3>();
		scalars = new Dictionary<string, float>();

		vectors.Add("ka", new Vector3(0.1f, 0.1f, 0)); // kar kag kab
		vectors.Add("ks", new Vector3(0.4f, 0.4f, 0)); // ksr ksg ksb
		vectors.Add("kd", new Vector3(1, 1, 0)); //kdr kdg kdb
		vectors.Add("Ia", new Vector3(1, 1, 1)); 
		vectors.Add("Id", new Vector3(1, 1, 1)); 
		vectors.Add("Is", new Vector3(1, 1, 1));

		scalars.Add("ALPHA", 50);

		vectors.Add("LIGHT", new Vector3(5, 5, 0));
		vectors.Add("CAMERA", new Vector3(10, 7, 0));
		vectors.Add("PoI", new Vector3(3.5f, 3, 0));
		vectors.Add("CC", new Vector3(3, 3, 0));

		vectors.Add("n", vectors["PoI"] - vectors["CC"]);
		vectors.Add("nUnit", Math.Normalize(vectors["n"]));

		vectors.Add("I", vectors["LIGHT"] - vectors["PoI"]);
		vectors.Add("IUnit", Math.Normalize(vectors["I"]));

		vectors.Add("v", vectors["CAMERA"] - vectors["PoI"]);
		vectors.Add("vUnit", Math.Normalize(vectors["v"]));


		vectors.Add("r", Math.Reflect(vectors["n"], vectors["I"]));
		vectors.Add("rUnit", Math.Normalize(vectors["r"]));

		scalars.Add("nuDotLu", Math.Dot( vectors["nUnit"] , vectors["IUnit"]));
		scalars.Add("ruDotLu", Math.Dot(vectors["rUnit"], vectors["vUnit"]));
		scalars.Add("ruDotVuAlpha", Mathf.Pow(scalars["ruDotLu"], scalars["ALPHA"]));
		Vector3 color = Vector3.zero;
		color.x = vectors["ka"].x * vectors["Ia"].x +
					vectors["kd"].x * vectors["Id"].x * scalars["nuDotLu"] +
					vectors["ks"].x * vectors["Is"].x * scalars["ruDotVuAlpha"];

		color.y = vectors["ka"].y * vectors["Ia"].y +
					vectors["kd"].y * vectors["Id"].y * scalars["nuDotLu"] +
					vectors["ks"].y * vectors["Is"].y * scalars["ruDotVuAlpha"];

		color.z = vectors["ka"].z * vectors["Ia"].z +
					vectors["kd"].z * vectors["Id"].z * scalars["nuDotLu"] +
					vectors["ks"].z * vectors["Is"].z * scalars["ruDotVuAlpha"];

		vectors.Add("COLORS", color);

		foreach (KeyValuePair<string, Vector3> pair in vectors)
			Debug.Log(pair.Key + "= " + pair.Value.ToString("F5"));

		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 1, 0));
		cube.transform.localPosition = vectors["CC"];




	}

	// Update is called once per frame
	void Update()
    {
		Debug.DrawLine(vectors["PoI"], vectors["PoI"] + vectors["n"], Color.black);
		Debug.DrawLine(vectors["PoI"], vectors["PoI"] + vectors["I"], Color.blue);
		Debug.DrawLine(vectors["PoI"], vectors["PoI"] + vectors["v"], Color.cyan);
		Debug.DrawLine(vectors["PoI"], vectors["PoI"] + vectors["r"], Color.green);


	}
}
