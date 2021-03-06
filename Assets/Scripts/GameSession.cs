﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {

	[SerializeField] int playerLives = 3;
	[SerializeField] int score = 0;

	private void Awake() {
		int numGameSessions = FindObjectsOfType<GameSession>().Length;
		if (numGameSessions > 1) {
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}

	public void AddToScore(int pointsToAdd) {
		AddToScore += pointsToAdd;
	}

	public void ProcessPlayerDeath() {
		if (playerLives > 1) {
			TakeLife();
		} else {
			ResetGameSession();
		}
	}

	private void ResetGameSession() {
		SceneManager.LoadScene(0);
		Destroy(gameObject);
	}

	private void TakeLife() {
		playerLives--;
		var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex);
	}
}
