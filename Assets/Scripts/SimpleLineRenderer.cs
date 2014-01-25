using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleLineRenderer : MonoBehaviour {
	private Material lineMaterial;

	void Start() {
		Util.lines = new List<Line>();
	}

	void CreateLineMaterial()
	{
		if( !lineMaterial ) {
			lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
			                            "SubShader { Pass { " +
			                            " Blend SrcAlpha OneMinusSrcAlpha " +
			                            " ZWrite Off Cull Off Fog { Mode Off } " +
			                            " BindChannels {" +
			                            " Bind \"vertex\", vertex Bind \"color\", color }" +
			                            "} } }" );
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;}
	}

	void OnPostRender()
	{
		CreateLineMaterial();
		lineMaterial.SetPass( 0 );
		GL.Begin( GL.LINES );
		foreach( Line l in Util.lines ) {
			GL.Color( l.color );
			GL.Vertex3 ( l.start.x, l.start.y, l.start.z );
			GL.Vertex3 ( l.end.x, l.end.y, l.end.z );
		}
		GL.End();

		for ( int i = Util.lines.Count - 1; i >= 0; i-- ) {
			Line line = Util.lines[ i ];
			if ( line.Expired() ) {
				Util.lines.RemoveAt( i );
			}
		}
	}
}
