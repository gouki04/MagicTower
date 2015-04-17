using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public CoroutineController Controller;

	private SafeCoroutine m_InnerCoroutine;
	private SafeCoroutine m_OuterCoroutine;

	// Use this for initialization
	void Start () {
//		m_InnerCoroutine = Controller.StartSafeCoroutine(InnerExampleCoroutine());
		m_OuterCoroutine = Controller.StartSafeCoroutine(OuterExampleCoroutine());
	}

	private bool finishOuterCoroutine = false;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
			finishOuterCoroutine = true;
        }
	}

	IEnumerator OuterExampleCoroutine()
	{
		m_InnerCoroutine = Controller.StartSafeCoroutine(InnerExampleCoroutine());
		yield return m_InnerCoroutine;

		while (finishOuterCoroutine) {
			yield return null;
		}
	}

	private float timer = 0.0f;
    IEnumerator InnerExampleCoroutine()
	{
		while (true)
		{
			timer = timer + Time.deltaTime;
			yield return (null);
		}
	}

	void OnGUI()
	{
		GUILayout.Label("Coroutine Inner: " + m_InnerCoroutine.State);
		GUILayout.Label("Coroutine Outer: " + m_OuterCoroutine.State);
		GUILayout.Label("time: " + timer);

		CoroutineGUI("Coroutine Inner: ", m_InnerCoroutine);
		CoroutineGUI("Coroutine Outer: ", m_OuterCoroutine);
	}

	static void CoroutineGUI(string a_Name, SafeCoroutine a_SafeCoroutine)
	{
		GUILayout.BeginHorizontal();
		GUI.enabled = a_SafeCoroutine.IsPause;
		if (GUILayout.Button("Resume " + a_Name)) {
			a_SafeCoroutine.Resume();
		}
		GUI.enabled = a_SafeCoroutine.CanPause;
		if (GUILayout.Button("Pause " + a_Name)) {
			a_SafeCoroutine.Pause();
		}
		GUI.enabled = a_SafeCoroutine.IsRunning || a_SafeCoroutine.IsPause;
		if (GUILayout.Button("Stop " + a_Name)) {
			a_SafeCoroutine.Stop();
		}
		GUILayout.EndHorizontal();
	}













}
