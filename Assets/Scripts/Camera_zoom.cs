using UnityEngine;

public class Camera_zoom : MonoBehaviour
{
	public static float mouse;

	public Texture2D mainTex;
	public Texture2D background;
	public float mouseMax = 10;
	public float mouseMin = 0.1f;
	public Camera _camera;
	public float maxFOV = 60;
	public float minFOV = 30;

	private float zoomLevel;
	private bool zoomStart;
	private bool zoom;

	void Start()
	{
		mouse = mouseMax;
	}

	void Update()
	{
		if (Input.GetMouseButton(1))
		{
			zoom = true;
			if (!zoomStart)
			{
				// стартовые, зум и чувствительность мыши, после включения прицела
				zoomStart = true;
				zoomLevel = maxFOV - 20;
				mouse -= 3.32f;
			}
		}
		else
		{
			zoomStart = false;
			zoom = false;
			zoomLevel = maxFOV;
			mouse = mouseMax;
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (zoomLevel > minFOV)
			{
				mouse -= 0.83f; // шаг, регулировки чувствительности мышки
				zoomLevel -= 5; // шаг, регулировки зума
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			if (zoomLevel < maxFOV)
			{
				mouse += 0.83f;
				zoomLevel += 5;
			}
		}

		mouse = Mathf.Clamp(mouse, mouseMin, mouseMax);
		zoomLevel = Mathf.Clamp(zoomLevel, minFOV, maxFOV);
		_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, zoomLevel, 10 * Time.deltaTime);
	}

	void OnGUI()
	{
		if (zoom)
		{
			GUI.depth = 999;
			int hor = Screen.width;
			int ver = Screen.height;
			GUI.DrawTexture(new Rect((hor - ver) / 2, 0, ver, ver), mainTex);
			GUI.DrawTexture(new Rect((hor / 2) + (ver / 2), 0, hor / 2, ver), background);
			GUI.DrawTexture(new Rect(0, 0, (hor / 2) - (ver / 2), ver), background);
		}
	}
}
