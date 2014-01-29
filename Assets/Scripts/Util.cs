using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Line {
	public Vector3 start, end;
	public Color color;
	public float time;
	public Line( Vector3 _start, Vector3 _end, Color _color, float _time ) {
		start = _start;
		end = _end;
		color = _color;
		time = _time;
	}
	public bool Expired() {
		time -= Time.deltaTime;
		return time <= 0f;
	}
}

public static class Util {
	//from http://www.bensilvis.com/?p=500
	public static void AutoResize(int screenWidth = 1280, int screenHeight = 800) //960,600
	{
		Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
	}

	public static Rect PlaceholderToGUIRect( GameObject placeholder ) {
		Transform t = placeholder.transform;
		float w = Screen.width * t.localScale.x;
		float h = Screen.height * t.localScale.y;
		float x = Screen.width * t.position.x - w / 2;
		float y = Screen.height * ( 1 - t.position.y ) - h / 2;
		Rect rect = new Rect( x, y, w, h );
		return rect;
	}

	public static Rect GetResizedRect(Rect rect)
	{
		Vector2 position = GUI.matrix.MultiplyVector(new Vector2(rect.x, rect.y));
		Vector2 size = GUI.matrix.MultiplyVector(new Vector2(rect.width, rect.height));
		
		return new Rect(position.x, position.y, size.x, size.y);
	}

	public static List<Line> lines;
	public static void DrawLine( Vector3 _start, Vector3 _end, Color _color, float _time = 0f ) {
		lines.Add( new Line( _start, _end, _color, _time ) );
	}

	//from http://answers.unity3d.com/questions/163864/test-if-point-is-in-collider.html
	public static bool IsInside ( Collider test, Vector3 point)
	{
		Vector3 center;
		Vector3 direction;
		Ray ray;
		RaycastHit hitInfo;
		bool hit;
		
		// Use collider bounds to get the center of the collider. May be inaccurate
		// for some colliders (i.e. MeshCollider with a 'plane' mesh)
		center = test.bounds.center;
		
		// Cast a ray from point to center
		direction = center - point;
		ray = new Ray(point, direction);
		hit = test.Raycast(ray, out hitInfo, direction.magnitude);
		
		// If we hit the collider, point is outside. So we return !hit
		return !hit;
	}

	public static Color SetAlpha( Color color, float alpha ) {
		Color c = color;
		c.a = alpha;
		return c;
	}

	public static IEnumerator FadeOut( SpriteRenderer sprite, float onTime, float fadeTime ) {
		yield return new WaitForSeconds( onTime );
		for( float t = 0f; t <= fadeTime; t += Time.deltaTime ) {
			sprite.material.color = Util.SetAlpha( sprite.material.color, Mathf.Lerp ( 1f, 0f, t/fadeTime ) );
			yield return 0;
		}
		GameObject.Destroy ( sprite.gameObject );
	}




	/*** EXTENSIONS ***/
	public static void Shuffle<T>(this List<T> list)  
	{  
		System.Random rng = new System.Random();  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	public static void Shuffle<T>(this T[] arr)  
	{  
		System.Random rng = new System.Random();  
		int n = arr.Length;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = arr[k];  
			arr[k] = arr[n];  
			arr[n] = value;  
		}  
	}

	public static void Each<T>( this IEnumerable<T> ie, Action<T, int> action )
	{
		int i = 0;
		foreach ( T e in ie ) action( e, i++ );
	}
	//EXAMPLE USAGE:
//	List<string> strings = new List<string>();
//	strings.Each( ( str, n ) =>
//	{
//		// hooray
//	} );

}