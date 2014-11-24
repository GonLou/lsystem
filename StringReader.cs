using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringReader : StringCreator {

	float angle;
	float length;
	List<Vector3> coords = new List<Vector3>();

	// Use this for initialization
	public StringReader () {
		this.angle = 90.0f;
		this.length = 0.10f;
	}

	public StringReader (float angle, float length) {
		this.angle = angle;
		this.length = length;
	}

	/// destructor
	~StringReader() {
	}

	public void setCoords(float x, float y) {
		this.coords.Add(new Vector3(x, y, 0));
	}

	public void setAngle(float angle) {
		this.angle = angle;
	}

	public void setLength(float length) {
		this.length = length;
	}
	
	public float getAngle() {
		return this.angle;
	}
	
	public float getLength() {
		return this.length;
	}

	public void printCoords() {
		foreach (Vector3 aPart in this.coords)
		{
			Debug.Log(aPart);
		}
	}

	public int getCoordsCount() {
		return this.coords.Count;
	}

	public Vector3 getCoords(int index) {
		return this.coords[index];
	}
	
	/// <summary>
	///  sets two pairs of vectors. The begin point and the end point
	/// </summary>
	/// <param name="x">Receives x position</param>
	/// <param name="y">Receives y position</param>
	public void doubleSetCoords(float x, float y) {
		if (this.coords.Count > 0) {
			this.coords.Add(this.coords[this.coords.Count-1]);
		} else {
			this.coords.Add(new Vector3(0.0f, 0.0f, 0.0f));
		}
		this.coords.Add(new Vector3(this.coords[this.coords.Count-1].x + x, this.coords[this.coords.Count-1].y + y, 0.0f)); 
	}
	
	/// <summary>
	///  interprets the full axiom into vectors
	/// -F+F-F-F+F
	///   _
	/// _| |_
	/// 
	/// </summary>
	/// <param name="axiom">Receives the full axiom</param>
	public void Read(string axiom) {
		float swap_angle_x = 90.0f;
		float swap_angle_y = 90.0f;
		//Debug.Log("pos: -1"+" x >> "+swap_angle_x);
		//Debug.Log("pos: -1"+" y >> "+swap_angle_y);

		for (int str_pos = 0; str_pos < axiom.Length; str_pos++) {
			if (axiom.Substring(str_pos, 1) == "F") {
				doubleSetCoords(this.length * Mathf.Cos (AngleToRadians (swap_angle_x)), 
				                this.length * Mathf.Sin (AngleToRadians (swap_angle_y)));
				//Debug.Log(this.length * swap_angle_x);
				//Debug.Log(this.length * swap_angle_y);
			} else if (axiom.Substring(str_pos, 1) == "+") {
				swap_angle_x += this.angle;
				swap_angle_y += this.angle;
			} else if (axiom.Substring(str_pos, 1) == "-") {
				swap_angle_x -= this.angle;
				swap_angle_y -= this.angle;
			}
//			Debug.Log("axiom: "+axiom.Substring(str_pos, 1));
//
//			Debug.Log("pos: "+str_pos+" x >> "+swap_angle_x);
//			Debug.Log("pos: "+str_pos+" y >> "+swap_angle_y);
		}
	}

	/// <summary>
	///  returns an angle value in radians
	/// </summary>
	/// <param name="angle">Receives an angle</param>
	private float AngleToRadians (float angle)
	{
		return angle * (Mathf.PI / 180);
	}
	
}
