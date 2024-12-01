using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Managers
{
	public class SceneFlowManager : Singleton<SceneFlowManager>
	{
		[SerializeField]
		LoadingScreen loadingScreenPrefab;

		[SerializeField]
		string firstScene;

		string currentScene;
		Coroutine loadSceneCoroutine;

		float globalMusicVolume = 1f;
		public float GlobalMusicVolume { get => globalMusicVolume; set => globalMusicVolume = value; }
		float globalSfxVolume = 1f;
		public float GlobalSfxVolume { get => globalSfxVolume; set => globalSfxVolume = value; }
		float sensitivity = 10f;
		public float Sensitivity 
		{ 
			get => sensitivity;
			set
			{
				sensitivity = value;
				GameManager.Instance.Player.SetSens(sensitivity);
			} 
		}

		private void Awake()
		{
			LoadScene(firstScene);
		}

		public void LoadScene(string sceneName)
		{
			DOTween.KillAll();
			Time.timeScale = 1f;
			if (loadSceneCoroutine != null) StopCoroutine(loadSceneCoroutine);

			loadSceneCoroutine = StartCoroutine(LoadSceneCoroutine(sceneName));
		}

		private IEnumerator LoadSceneCoroutine(string sceneName)
		{
			LoadingScreen loadingScreen = Instantiate(loadingScreenPrefab);
			yield return loadingScreen.FadeIn();

			if (!string.IsNullOrEmpty(currentScene))
				SceneManager.UnloadSceneAsync(currentScene);

			AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			while (!loadOperation.isDone)
			{
				yield return null;
			}
			currentScene = sceneName;

			yield return new WaitForEndOfFrame();
			yield return loadingScreen.FadeOut();
			Destroy(loadingScreen.gameObject);
			Time.timeScale = 1f;
		}
	}
}
