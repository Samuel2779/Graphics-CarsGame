using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapsController : MonoBehaviour
{

	public List<GameObject> cars = new List<GameObject>();
	public GameObject textPositions;
	List<CarsMovement> carComponentMovement;
	List<int> lapsCar;
	List<int> currentIndex;
    // Start is called before the first frame update
    void Start()
    {
		carComponentMovement = new List<CarsMovement>();
		lapsCar = new List<int>();
		currentIndex = new List<int>();

		for (int i = 0; i < cars.Count; i++)
		{
			carComponentMovement.Add(cars[i].GetComponent<CarsMovement>());
			lapsCar.Add(0);
			currentIndex.Add(0);
		}
		InitialsPositions();
    }

	private void InitialsPositions()
	{
		textPositions.GetComponent<UnityEngine.UI.Text>().text = "Positions:\n" + carComponentMovement[0].numPlayer +"\n" +
			carComponentMovement[1].numPlayer + "\n" + carComponentMovement[2].numPlayer + "\n" + carComponentMovement[3].numPlayer + "\n";
	}
    // Update is called once per frame
    void Update()
    {
		for (int i = 0; i < cars.Count; i++)
		{
			lapsCar[i] = carComponentMovement[i].indexTotal;
			
		}
		var sorted = lapsCar.Select((x, i) => new KeyValuePair<int, int>(x, i)).OrderBy(x => x.Key).ToList();

		List<int> B = sorted.Select(x => x.Key).ToList();
		List<int> idx = sorted.Select(x => x.Value).ToList();
		UpdatedPositions(idx);

	}
	private void UpdatedPositions(List<int> sortedIndex)
	{
		List<int> carPosition = new List<int>();
		for(int i = 0; i < sortedIndex.Count; i++)
		{
			carPosition.Add(sortedIndex[i] + 1);
		}
		textPositions.GetComponent<UnityEngine.UI.Text>().text = "Positions:\n" + "Frist Place => P" + carPosition[3].ToString() + "\n" +
			"Second Place => P" + carPosition[2].ToString() + "\n" + "Third Place => P" + carPosition[1].ToString() + "\n"
			+ "Fourth Place => P" + carPosition[0].ToString() + "\n";
	}
}
