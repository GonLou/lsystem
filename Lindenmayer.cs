using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lindenmayer : MonoBehaviour
{

	public Material mat;
	public StringReader myStringRead;
	public StringCreator myString;

	[Range(1, 10)]
	public int interaction;
	public string alphabet;
	public string rule;
	public string axiom;
	[Range(0, 360)]
	public float angle;
	[Range(0, 1)]
	public float unit_size;

	public bool toggleSaveFile;

	public bool drawEnable = false;

	public int record_number;

	public Vector3 camera_position;

	// Unity default start
	void Start ()
	{
		camera.backgroundColor = Color.gray;

		interaction = 3;
		alphabet = "F";
		rule = "F+F-F-F+F";
		axiom = "-F";
		angle = 90.0f;
		unit_size = 0.1f;
		toggleSaveFile = false;

		camera_position = Camera.main.transform.position;
	}


	// Unity default update
	void Update ()
	{
		if (drawEnable) {
			if ( Input.GetKey(KeyCode.UpArrow) )				// camera move up
				camera_position.y += 0.1f;

			if ( Input.GetKey(KeyCode.DownArrow) ) 				// camera move down
				camera_position.y -= 0.1f;

			if ( Input.GetKey(KeyCode.RightArrow) )             // camera move right
				camera_position.x += 0.1f;

			if ( Input.GetKey(KeyCode.LeftArrow) )				// camera move left
				camera_position.x -= 0.1f;

			if ( Input.GetKey(KeyCode.Z) ) {							// zoom in
				if (Camera.main.orthographicSize > 1)
					Camera.main.orthographicSize--;
			}

			if ( Input.GetKey(KeyCode.X) ) {							// zoom out
				if (Camera.main.orthographicSize < 10)
					Camera.main.orthographicSize++;
			}


			UpdateCamera(camera_position.x,camera_position.y);
		}

		if ( Input.GetKeyDown(KeyCode.Keypad1) ) LoadFile(0);
		if ( Input.GetKeyDown(KeyCode.Keypad2) ) LoadFile(1);
		if ( Input.GetKeyDown(KeyCode.Keypad3) ) LoadFile(2);
		if ( Input.GetKeyDown(KeyCode.Keypad4) ) LoadFile(3);
		if ( Input.GetKeyDown(KeyCode.Keypad5) ) LoadFile(4);
		if ( Input.GetKeyDown(KeyCode.Keypad6) ) LoadFile(5);
		if ( Input.GetKeyDown(KeyCode.Keypad7) ) LoadFile(6);
		if ( Input.GetKeyDown(KeyCode.Keypad8) ) LoadFile(7);

	}


	/// <summary>
	/// manipulates the camera position
	/// </summary>
	/// <param name="cx">Cx.</param>
	/// <param name="cy">Cy.</param>
	public void UpdateCamera(float cx, float cy) {
		camera_position.x = cx;
		camera_position.y = cy;
		Camera.main.transform.position = camera_position;
	}


	/// <summary>
	/// creates the full axiom from alphabet and rules
	/// </summary>
	public void CreateFullAxiom() {
		myString = new StringCreator(alphabet, rule, axiom, interaction, angle, unit_size);
		//myStringRead = new StringReader(angle, unit_size);
		//myStringRead.Read(myString.Start());
	}


	/// <summary>
	/// transforms the full axiom into vectors
	/// </summary>
	public void Axiom2Vectors() {
		myStringRead = new StringReader(angle, unit_size);
		myStringRead.Read(myString.Start());
//		UpdateCamera(myStringRead.getMaxCoordsX()-myStringRead.getMinCoordsX(),
//		             myStringRead.getMaxCoordsY()-myStringRead.getMinCoordsY());
		UpdateCamera(myStringRead.getMinCoordsX(),myStringRead.getMinCoordsY());
	}
	
	
	/// <summary>
	/// save into file
	/// </summary>
	public void SaveFile() {
		string tmp_full_axiom = myString.getFullString();
		string tmp_alphabet = "";
		string tmp_rule = "";
		//Debug.Log ("HERE "+tmp_full_axiom);

		for (int str_pos=0; str_pos < tmp_full_axiom.Length; str_pos++){
			string single_symbol = tmp_full_axiom.Substring(str_pos, 1);
			string value_retrieved = myString.getDicValue(single_symbol);

			//Debug.Log ("R: "+single_symbol);
			if ( (single_symbol == "+"  ||
			      single_symbol == "-"  ||
			      single_symbol == "["  ||
			      single_symbol == "]") ) {
			} else {
			    if (IsNewValue(tmp_alphabet, single_symbol)) {
					//Debug.Log ("in");
					tmp_alphabet += single_symbol + ", ";
					tmp_rule += single_symbol + "->" + value_retrieved + "; ";
				}
			}
		}

		StringFile IOFile = new StringFile();
		IOFile.write(tmp_alphabet, tmp_rule, axiom, interaction, angle, unit_size);
	}

	public int SmallerRecord() {
		record_number--;
		if (record_number < 0) record_number = 0;
		return record_number;
	}
	public int BiggerRecord() {
		StringFile IOFile = new StringFile();
		record_number++;
		if (record_number > IOFile.getTotalRecords()) record_number = IOFile.getTotalRecords();
		return record_number;
	}

	/// <summary>
	/// load from file
	/// </summary>
	public void LoadFile(int example_num) {
		const int group_line = 13;
		int group_begin = example_num * group_line;
		int group_end = group_begin + group_line - 2;

		int counter = 0;
		string line;
		
		record_number = example_num;
		drawEnable = false;

		// Read the file and display it line by line.
		System.IO.StreamReader file = new System.IO.StreamReader(@"StringFile.txt");
		while((line = file.ReadLine()) != null)
		{
			if (counter >= group_begin && counter <= group_end)
				Debug.Log (line);

			counter++;
		}
		//Debug.Log ("group_begin"+group_begin);
		//Debug.Log ("group_end"+group_end);
		//myString = new StringCreator(alphabet, rule, axiom, interaction, angle, unit_size);
		file.Close();
	}


	/// <summary>
	/// To find values in strings
	/// </summary>
	/// <returns><c>true</c> if this instance is new value the specified in_alphabet in_symbol; otherwise, <c>false</c>.</returns>
	/// <param name="in_alphabet">In_alphabet.</param>
	/// <param name="in_symbol">In_symbol.</param>
	private bool IsNewValue(string in_alphabet, string in_symbol) {
		for (int str_pos_func=0; str_pos_func < in_alphabet.Length; str_pos_func++) {
			if (in_alphabet.Substring(str_pos_func, 1) == in_symbol) return false;
		}
		return true;
	}

	
	/// <summary>
	/// This is where the lines are drawn
	/// </summary>
	void OnPostRender ()
	{
		if (drawEnable) {
			int list_total = myStringRead.getCoordsCount();

			LineMaterial();

			GL.PushMatrix ();
			mat.SetPass (0);
			GL.Begin (GL.LINES);
			GL.Color (Color.white);

			for (int i = 0; i < list_total; i++)
			{
				GL.Vertex ((myStringRead.getCoords(i)));
			}

			GL.End ();
			GL.PopMatrix ();	
		}
	}


	/// <summary>
	/// Applies a material to the line to be rendered
	/// </summary>
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
