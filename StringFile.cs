﻿///////////////////////////////////////////////////////////////////
/// 
/// author:
/// Goncalo Lourenco
/// 
/// 
/// <summary>
/// File I/O operations for L-Systems
/// </summary>
/// 
///////////////////////////////////////////////////////////////////
 
using UnityEngine;
using System.Collections;
using System.IO;

public class StringFile {

	private int group_line = 13;

	// Use this for initialization
	public StringFile () {
	}

	/// destructor
	~StringFile() {
	}
	
	/// <summary>
	/// To get the total number of records
	/// </summary>
	/// <returns>The total records.</returns>
	public int getTotalRecords() {
		int counter = 0;
		string line;
		
		System.IO.StreamReader file = new System.IO.StreamReader(@"StringFile.txt");
		while((line = file.ReadLine()) != null)
		{
			counter++;
		}
		file.Close();
		return (int)(counter/group_line);
	}

	/// <summary>
	/// Write into the file
	/// </summary>
	/// <param name="symbol">Symbol.</param>
	/// <param name="rule">Rule.</param>
	/// <param name="axiom">Axiom.</param>
	/// <param name="interaction">Interaction.</param>
	/// <param name="angle">Angle.</param>
	/// <param name="unit_size">Unit_size.</param>
	public void write(string symbol, string rule, string axiom, int interaction, float angle, float unit_size) {
		if (!System.IO.File.Exists(@"StringFile.txt")) {  // if files does not exist create one
			string[] allLines = { 	"symbol:", symbol, 
									"rule:", rule,
									"axiom:", axiom,
									"interaction:", interaction.ToString(),
									"angle:", angle.ToString(),
									"unit size:", unit_size.ToString(),
								""};
			//System.IO.File.WriteAllLines(@"StringFile.txt", allLines);
		}
		else { // append to the end of the file
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"StringFile.txt", true)) {
				file.WriteLine("symbol:");
				file.WriteLine(symbol);
				file.WriteLine("rule:");
				file.WriteLine(rule);
				file.WriteLine("axiom:");
				file.WriteLine(axiom);
				file.WriteLine("interaction:");
				file.WriteLine(interaction.ToString());
				file.WriteLine("angle:");
				file.WriteLine(angle.ToString());
				file.WriteLine("unit size:");
				file.WriteLine(unit_size.ToString());
				file.WriteLine();
			}
		}			
	}

}