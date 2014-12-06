///////////////////////////////////////////////////////////////////
/// 
/// author:
/// Goncalo Lourenco
/// 
/// 
/// <summary>
/// Full axiom transformation into vectors
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

	/// Default initialization
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

	private void setCoords(float x, float y) {
		this.coords.Add(new Vector3(x, y, 0));
	}

	private void setCoordsAdd(float x, float y) {
		this.coords.Add(new Vector3(this.coords[this.coords.Count-1].x + x, this.coords[this.coords.Count-1].y + y, 0));
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
	
	private float getLastCoordX() {
		return this.coords[getCoordsCount()-1].x;
	}

	private float getLastCoordY() {
		return this.coords[getCoordsCount()-1].y;
	}

	public void printCoords() {
		foreach (Vector3 aPart in this.coords)
		{
			Debug.Log(aPart);
		}
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
		float swap_angle = 90.0f;
		List<Vector3> last_coords = new List<Vector3>();
		List<float> last_angle = new List<float>();
		int status = 0; // 0 - is no brackets | 1 - opened a bracket | 2 - closed a bracket
		int count_brackets = 0;
		
		setCoords( 0, 0 );
		setCoordsAdd( 0, 0 );
		last_coords.Add( new Vector3(0, 0, 0) );

		last_angle.Add (swap_angle);

		axiom = axiom + " " ;
		
		for (int str_pos = 0; str_pos < axiom.Length; str_pos++) {
			if (axiom.Substring(str_pos, 1) == "F" || 
			    axiom.Substring(str_pos, 1) == "X") {
				switch (status) {
				case 0:
					setCoords( (last_coords[count_brackets]).x, 
					          (last_coords[count_brackets]).y );
					
					setCoordsAdd(	this.length * Mathf.Cos (AngleToRadians (swap_angle)), 
					             	this.length * Mathf.Sin (AngleToRadians (swap_angle)) );

					last_coords.Insert( count_brackets, new Vector3(	getLastCoordX(),
					                    			                	getLastCoordY(),
					                                				    0) );
					break;
				case 1: // open [
					setCoords( (last_coords[count_brackets-1]).x, 
					          (last_coords[count_brackets-1]).y );

					setCoordsAdd(	this.length * Mathf.Cos (AngleToRadians (swap_angle)), 
					             	this.length * Mathf.Sin (AngleToRadians (swap_angle)) );

					last_coords.Insert( count_brackets, new Vector3( getLastCoordX(),
						                                             getLastCoordY(),
					                                                 0) );
					status = 0;
					break;
				}
			} else if (axiom.Substring(str_pos, 1) == "[") {
				status = 1;
				count_brackets ++;	

				last_angle.Add (swap_angle);
				last_coords.Add( new Vector3(                   getLastCoordX(),
				                                                getLastCoordY(),
				                                                0) );
			} else if (axiom.Substring(str_pos, 1) == "]") {
				swap_angle = last_angle[count_brackets];
				last_angle.RemoveAt(count_brackets);
				last_coords.RemoveAt(count_brackets);

				status = 0;
				count_brackets --;
			} else if (axiom.Substring(str_pos, 1) == "+") {
				swap_angle += this.angle;
			} else if (axiom.Substring(str_pos, 1) == "-") {
				swap_angle -= this.angle;
			}
		}
		
		if (getLastCoordX() > max_coords.x)
			max_coords.x = getLastCoordX();
		else if (getLastCoordY() > max_coords.y)
			max_coords.y = getLastCoordY();
		
		if (getLastCoordX() < min_coords.x)
			min_coords.x = getLastCoordX();
		else if (getLastCoordY() < min_coords.y)
			min_coords.y = getLastCoordY();
		
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