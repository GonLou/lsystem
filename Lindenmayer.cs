///////////////////////////////////////////////////////////////////
/// 
/// author:
/// Goncalo Lourenco
/// 
/// 
/// <summary>
/// Main Class for L-Systems
/// </summary>
/// 
///////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
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

	public Text inter_number;
	public Text record_number_text;
	public Slider inter_slider;
	public Slider record_slider;
	public InputField rule_input;


	// Unity default start
	void Start ()
	{
		camera.backgroundColor = Color.gray;

		interaction = 3;
		alphabet = "F";
		rule = "F+F-F-F+F";
		axiom = "-F";
		angle = 45.0f;
		unit_size = 0.1f;
		toggleSaveFile = false;

		camera_position = Camera.main.transform.position;

		GameObject temp;
		temp = GameObject.Find("Slider Interactions");
		if (temp != null) {
			// Get the Slider Component
			inter_slider = temp.GetComponent<Slider>();
			
			inter_slider.value = interaction;
			inter_slider.minValue = 1;
			inter_slider.maxValue = 10;
		}

		temp = GameObject.Find("Slider Record");
		if (temp != null) {
			// Get the Slider Component
			record_slider = temp.GetComponent<Slider>();
			
			record_slider.value = 0;
			record_slider.minValue = 1;
			record_slider.maxValue = (int) TotalRecords();
		}

		temp = GameObject.Find("InputField Rule");
		if (temp != null) { 
			rule_input = temp.GetComponent<InputField>();
			rule_input.text = rule;
		}

	}


	// Unity default update
	void Update ()
	{
		GameObject temp;
		temp = GameObject.Find("Slider Interactions");
		if (temp != null) {
			// Get the Slider Component
			inter_slider = temp.GetComponent<Slider>();
			if ( inter_slider.value != interaction ) {
				interaction = (int) inter_slider.value;
				CreateFullAxiom();
				Axiom2Vectors();
				drawEnable = true;
			}
		}
		
		temp = GameObject.Find("Slider Record");
		if (temp != null) {
			// Get the Slider Component
			record_slider = temp.GetComponent<Slider>();
			if ( (int) record_slider.value != (record_number+1) ) {
				record_number = (int) record_slider.value;
				LoadFile(record_number);
			}
			Debug.Log((int) record_slider.value);
		}

		temp = GameObject.Find("InputField Rule");
		if (temp != null) { 
			rule_input = temp.GetComponent<InputField>();
			if (rule_input.text != rule) {
				rule = rule_input.text;
				CreateFullAxiom();
				Axiom2Vectors();
				drawEnable = true;
				UpdateCamera((myStringRead.getMinCoordsX()/2), 
				             (myStringRead.getMaxCoordsY()/2) );
			}
		}

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
					Camera.main.orthographicSize -= 0.1f;
			}

			if ( Input.GetKey(KeyCode.X) ) {							// zoom out
				if (Camera.main.orthographicSize < 10)
					Camera.main.orthographicSize += 0.1f;
			}

			if ( Input.GetKey(KeyCode.R) ) {			// +
				BiggerRecord();
				LoadFile(record_number);
				DisableRendererWithDelay();
			}
			
			if ( Input.GetKey(KeyCode.T) ) {		// -
				SmallerRecord();
				LoadFile(record_number);
				DisableRendererWithDelay();
			}

			UpdateCamera(camera_position.x,camera_position.y);
		}

		if ( Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1) ) LoadFile(0);
		if ( Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2) ) LoadFile(1);
		if ( Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3) ) LoadFile(2);
		if ( Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4) ) LoadFile(3);
		if ( Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5) ) LoadFile(4);
		if ( Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6) ) LoadFile(5);
		if ( Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7) ) LoadFile(6);
		if ( Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8) ) LoadFile(7);

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

		GameObject temp;
		temp = GameObject.Find("Text Interactions");
		if (temp != null) {
			inter_number = temp.GetComponent<Text>();
			inter_number.text = interaction.ToString() + " interactions";
		}
		
		temp = GameObject.Find("Slider Interactions");
		if (temp != null) {
			// Get the Slider Component
			inter_slider = temp.GetComponent<Slider>();
			
			inter_slider.value = interaction;
		}
		
		temp = GameObject.Find("InputField Rule");
		if (temp != null) { 
			rule_input = temp.GetComponent<InputField>();
			rule_input.text = rule;
		}
	}


	/// <summary>
	/// transforms the full axiom into vectors
	/// </summary>
	public void Axiom2Vectors() {
		myStringRead = new StringReader(angle, unit_size);
		myStringRead.Read(myString.Start());
		UpdateCamera(myStringRead.getMinCoordsX(),myStringRead.getMinCoordsY());
		//myStringRead.printCoords();
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


	/// <summary>
	/// Verifies if the minimum record was reached
	/// </summary>
	/// <returns>The record.</returns>
	public int SmallerRecord() {
		record_number--;
		if (record_number < 0) record_number = 0;
		return record_number;
	}


	/// <summary>
	/// Verifies if the maximum record was reached
	/// </summary>
	/// <returns>The record.</returns>
	public int BiggerRecord() {
		StringFile IOFile = new StringFile();
		record_number++;
		if (record_number > IOFile.getTotalRecords()) record_number = IOFile.getTotalRecords();
		return record_number;
	}


	/// <summary>
	/// load from file
	/// </summary>
	/// <param name="example_num">Example_num.</param>
	public void LoadFile(int example_num) {
		example_num++;
		GameObject temp = GameObject.Find("Text Number of Record");
		if (temp != null) {
			record_number_text = temp.GetComponent<Text>();
			record_number_text.text = example_num.ToString();
		}	
		temp = GameObject.Find("Slider Record");
		if (temp != null) {
			record_slider = temp.GetComponent<Slider>();
			record_slider.value = example_num;
		}
		example_num--;

		const int group_line = 13;
		int group_begin = example_num * group_line;
		int group_end = group_begin + group_line - 2;
		int internal_counter = 0;

		int counter = 0;
		string line;

		record_number = example_num;
		drawEnable = false;

		// Read the file and display it line by line.
		System.IO.StreamReader file = new System.IO.StreamReader(@"StringFile.txt");
		while((line = file.ReadLine()) != null && counter != 32000)
		{
			if (counter >= group_begin && counter <= group_end) {

				switch(internal_counter) {
				case 1: // symbol
					alphabet = line;
 					break;
				case 3: // rule
					rule = line;
					break;
				case 5: // axiom
					axiom = line;
					break;
				case 7: // interaction
					if (line != null) int.TryParse(line, out interaction);
					break;
				case 9: // angle
					if (line != null) float.TryParse(line, out angle);
					break;
				case 11: // unit size
					if (line != null) float.TryParse(line, out unit_size);
					counter = 31999;
					break;
				}
				internal_counter++;

			}

			counter++;
		}

		myString = new StringCreator(alphabet, rule, axiom, interaction, angle, unit_size);
		file.Close();

		CreateFullAxiom();
		Axiom2Vectors();
		drawEnable = true;
		UpdateCamera((myStringRead.getMinCoordsX()/2), 
		             (myStringRead.getMaxCoordsY()/2) );
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


	/// <summary>
	/// Returns the total number of records
	/// </summary>
	/// <returns>The total records number.</returns>
	public int TotalRecords() {
		StringFile IOFile = new StringFile();
		return IOFile.getTotalRecords();
	}
	
}
