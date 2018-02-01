using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PluginTest : MonoBehaviour {

#if UNITY_IOS

	private delegate void intCallback(int result);

	[DllImport("__Internal")]
	private static extern double IOSgetElapsedTime();

	[DllImport("__Internal")]
	private static extern void IOScreateNativeAlert(string[] strings, int stringCount, intCallback callback);

#endif

	// Use this for initialization
	void Start () {

		Debug.Log("Elapsed Time: " + getElapsedTime());
		StartCoroutine(ShowDialog(Random.Range(7,12)));
	}

	IEnumerator ShowDialog(float delayTime)
	{
		Debug.Log("Will show alert after " + delayTime + " seconds");
		if (delayTime > 0)
			yield return new WaitForSeconds(delayTime);
		CreateIOSAlert(new string[] {"Title","Message","DefaultButton","OtherButton"});
	}


	double getElapsedTime()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			return IOSgetElapsedTime();
		Debug.LogWarning("Wrong platform!");
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(intCallback))]
	static void nativeAlertHandler(int result)
	{
		Debug.Log("Unity: clicked button at index: " + result);
	}

	public void CreateIOSAlert(string[] strings)
	{
		if (strings.Length<3)
		{
			Debug.LogError("Alert requires at least 3 strings!");
			return;
		}

		if (Application.platform == RuntimePlatform.IPhonePlayer)
			IOScreateNativeAlert(strings, strings.Length, nativeAlertHandler);
		else
			Debug.LogWarning("Can only display alert on iOS");
		Debug.Log("Alert shown after: " + getElapsedTime() + " seconds");
	}
}
