using UnityEngine;
using System.Collections;


public class Lindenmayer : MonoBehaviour
{

	public Material mat;
	public StringReader newRead;

	void Start ()
	{
		camera.backgroundColor = Color.gray;

		StringCreator newSeries = new StringCreator("F", "F+F-F-F+F","-F", 4);

		StringFile writeToFile = new StringFile();
		writeToFile.write("F", "F+F-F-F+F","-F", 4, 90.0f);

		newRead = new StringReader();

		Debug.Log (newSeries.Start());

		newRead.Read(newSeries.Start());

		newRead.printCoords();

	}


	void Update ()
	{
		
	}

	public float AngleToRadians (float angle)
	{
		return angle * (Mathf.PI / 180);
	}

	
	
	void OnPostRender ()
	{
		int list_total = newRead.getCoordsCount();

		LineMaterial();

		GL.PushMatrix ();
		mat.SetPass (0);
		GL.Begin (GL.LINES);
		//tempVector();
		GL.Color (Color.white);

		for (int i = 0; i < list_total; i++)
		{
			GL.Vertex ((newRead.getCoords(i)));
			//Debug.Log (">> "+(newRead.getCoords(i)));
		}

		GL.End ();
		GL.PopMatrix ();	
	}

	void tempVector() {
		GL.Color (Color.red);
		
		GL.Vertex (new Vector3 (0, 0, 0));
		GL.Vertex (new Vector3 (0, 1, 0));
		GL.Color (Color.blue);
		float y = 1;
		float x= 1;
		GL.Vertex (new Vector3 (x, y, 0));
		x+=Mathf.Cos (AngleToRadians (0));
		y+=Mathf.Sin(AngleToRadians (0));
		GL.Vertex (new Vector3 (x, y, 0));
		//Debug.Log ("Cos "+Mathf.Cos (AngleToRadians (0)));
		//Debug.Log ("Sin "+Mathf.Sin (AngleToRadians (0)));
		GL.Color (Color.red);
		GL.Vertex (new Vector3 (x, y, 0));
		//		y +=1;
		//		x +=1;
		x+=Mathf.Cos (AngleToRadians (0));
		y+=Mathf.Sin(AngleToRadians (0));
		GL.Vertex (new Vector3 (x, y, 0));
		

	}

	void LineMaterial() {
	                    if (!mat) {
							mat = new Material ("Shader \"Lines/Colored Blended\" {" +
							                    "SubShader { Pass { " +
							                    "    Blend SrcAlpha OneMinusSrcAlpha " +
							                    "    ZWrite Off Cull Off Fog { Mode Off } " +
							                    "    BindChannels {" +
							                    "      Bind \"vertex\", vertex Bind \"color\", color }" +
							                    "} } }");
							mat.hideFlags = HideFlags.HideAndDontSave;
							mat.shader.hideFlags = HideFlags.HideAndDontSave;
	                    }
	}

}
