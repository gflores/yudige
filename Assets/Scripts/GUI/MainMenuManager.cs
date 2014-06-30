using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
	static public MainMenuManager instance;
	void Awake(){
		instance = this;
	}
	
	public void ContinueGame()
	{
		MasterGameManager.is_in_tutorial = false;
		MasterGameManager.want_new_game = false;
		Application.LoadLevel("main");
	}
	public void StartNewGame()
	{
		MasterGameManager.is_in_tutorial = true;
		MasterGameManager.want_new_game = true;
		Application.LoadLevel("main");
	}
	public void StartNewGameSkipIntro()
	{
		MasterGameManager.is_in_tutorial = false;
		MasterGameManager.want_new_game = true;
		Application.LoadLevel("main");
	}

	public void ExitGame()
	{
		Application.Quit();
	}
	
	void Update()
	{
		/*
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				ContinueGame();
			}
			else if (Input.GetKeyDown(KeyCode.Z))
			{
				StartNewGame();
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
				StartNewGameSkipIntro();
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				Application.Quit();
			}
		}
		*/

	}

}