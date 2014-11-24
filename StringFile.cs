using UnityEngine;
using System.Collections;

public class StringFile {

	// Use this for initialization
	//string symbol, string rule, string axiom, int interaction
	public StringFile () {
	}

	/// destructor
	~StringFile() {
	}

	/// method that write into a file
	public void write(string symbol, string rule, string axiom, int interaction, float angle) {
		if (!System.IO.File.Exists(@"StringFile.txt")) {  // if files does not exist create one
			string[] allLines = { 	"symbol:", symbol, 
									"rule:", rule,
									"axiom:", axiom,
									"interaction:", interaction.ToString(),
									"angle:", angle.ToString(),
								""};
			System.IO.File.WriteAllLines(@"StringFile.txt", allLines);
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
				file.WriteLine();
			}
		}			
	}

}