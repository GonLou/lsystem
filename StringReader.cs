///////////////////////////////////////////////////////////////////
/// 
/// author:
/// Goncalo Lourenco
/// 
/// 
/// <summary>
/// Full axiom tranformed into vectors
/// </summary>
/// 
///////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringReader : StringCreator {

	float angle;
	float length;
	List<Vector3> coords = new List<Vector3>();
	Vector2 min_coords;
	Vector2 max_coords;

	// Use this for default initialization
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

	public int getCoordsCount() {
		return this.coords.Count;
	}

	public float getMinCoordsX() {
		return this.min_coords.x;
	}

	public float getMinCoordsY() {
		return this.min_coords.y;
	}

	public float getMaxCoordsX() {
		return this.max_coords.x;
	}

	public float getMaxCoordsY() {
		return this.max_coords.y;
	}

	public Vector3 getCoords(int index) {
		return this.coords[index];
	}
	
	public float getLastCoordX() {
		return this.coords[getCoordsCount()-1].x;
	}

	public float getLastCoordY() {
		return this.coords[getCoordsCount()-1].y;
	}

	public void printCoords() {
		foreach (Vector3 aPart in this.coords)
		{
			Debug.Log(aPart);
		}
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
		List<Vector3> last_coords = new List<Vector3>();
		int count_brackets = 0;

		for (int str_pos = 0; str_pos < axiom.Length; str_pos++) {
			if (axiom.Substring(str_pos, 1) == "F" || 
			    axiom.Substring(str_pos, 1) == "X") {
				if (count_brackets <= 0) {
					doubleSetCoords(	this.length * Mathf.Cos (AngleToRadians (swap_angle_x)), 
				   		             	this.length * Mathf.Sin (AngleToRadians (swap_angle_y)));
				} else {
					int last_record = last_coords.Count-1;
					if (last_record >= 0) {
						setCoords((last_coords[last_record]).x, 
						          (last_coords[last_record]).y);
						last_coords.RemoveAt(last_record);

						setCoords(	this.length * Mathf.Cos (AngleToRadians (swap_angle_x)), 
						            this.length * Mathf.Sin (AngleToRadians (swap_angle_y)));
					}
				}
				//  X
				//  X→F-[[X]+X]+F[+FX]-X
				//	F→FF
				// this.coords.Add(new Vector3(this.coords[this.coords.Count-1].x + x, 
				//                             this.coords[this.coords.Count-1].y + y, 
			} else if (axiom.Substring(str_pos, 1) == "[") {
				if (getCoordsCount() > 0) 
					last_coords.Add( new Vector3(this.length * Mathf.Cos (AngleToRadians (swap_angle_x)) + getLastCoordX(),
					                             this.length * Mathf.Sin (AngleToRadians (swap_angle_y)) + getLastCoordY(),
					                             0));
				else
					last_coords.Add ( new Vector3(0,0,0));
				count_brackets ++;
			} else if (axiom.Substring(str_pos, 1) == "]") {
				// vai busvar o ultimo
				// faz singlerecords
				// elimina-o da lista
				// mete rectos a true para quando processa n ser o doublesetRecords mas o single
				count_brackets --;
			} else if (axiom.Substring(str_pos, 1) == "+") {
				swap_angle_x += this.angle;
				swap_angle_y += this.angle;
			} else if (axiom.Substring(str_pos, 1) == "-") {
				swap_angle_x -= this.angle;
				swap_angle_y -= this.angle;
			}
		}

		if (getCoordsCount() > 0) {
			if (getLastCoordX() > max_coords.x)
				max_coords.x = getLastCoordX();
			else if (getLastCoordY() > max_coords.y)
				max_coords.y = getLastCoordY();

			if (getLastCoordX() < min_coords.x)
				min_coords.x = getLastCoordX();
			else if (getLastCoordY() < min_coords.y)
				min_coords.y = getLastCoordY();
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
