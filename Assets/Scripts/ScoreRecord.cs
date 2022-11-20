using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScoreRecord : MonoBehaviour {

	public Text Name;
	public Text Score;

	private void OnEnable(){
	var textFields = GetComponentsInChildren<Text>();
	Name = textFields.First(f => f.name == "name");
	Score = textFields.First(f => f.name == "score");
	}
	
	public void WriteRecord(string name, string score)
	{
	Name.text = name;
	Score.text = score;
	}



}
