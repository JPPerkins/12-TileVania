using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour {

	[SerializeField] float LevelLoadDelay = 2f;
	[SerializeField] float LevelExitSlowMoFactor = 0.2f;
	private void OnTriggerEnter2D(Collider2D other) {
		StartCoroutine(LoadNextLevel()); // start coroutine
	}

	private IEnumerator LoadNextLevel() {
		Time.timeScale = LevelExitSlowMoFactor;
		yield return new WaitForSecondsRealtime(LevelLoadDelay); //yield with a delay
		
		var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex + 1);
	}
}
