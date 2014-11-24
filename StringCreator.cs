///////////////////////////////////////////////////////////////////
/// 
/// author:
/// Goncalo Lourenco
/// 
/// 
/// <summary>
/// String creator for L-Systems
/// </summary>
/// 
///////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringCreator {

	Dictionary<string, string> symbol_and_rule = new Dictionary<string, string>();
	string axiom;
	int interaction;
	float angle;

	/// default constructor
	public StringCreator() {
		this.symbol_and_rule.Add("F", "F+F-F-F+F");
		this.axiom = "-F";
		this.interaction = 4;
		this.angle = 90.0f;
	}

	/// constructor with two parameters
	/// <param name="symbol">Used to specify the symbol.</param>
	/// <param name="rule">Used to specify the rule.</param>
	public StringCreator(string symbol, string rule) {
		this.symbol_and_rule.Add(symbol, rule);
	}

	/// constructor with four parameters
	/// <param name="symbol">Used to specify the symbol.</param>
	/// <param name="rule">Used to specify the rule.</param>
	/// <param name="axiom">Used to specify the axiom.</param>
	/// <param name="angle">Used to specify the angle.</param>
	public StringCreator(string symbol, string rule, string axiom, int interaction, float angle) {
		this.symbol_and_rule.Add(symbol, rule);
		this.axiom = axiom;
		this.interaction = interaction;
		this.angle = angle;
	}

	/// destructor
	~StringCreator() {
	}

	/// constructor with one parameter
	/// <param name="interaction">The number of interactions for the cycle</param>
	/// returns the final string
	public string Start() {
		int cycle = 1;
		string final_string = this.axiom;
		string swap_string = "";

		while(cycle <= this.interaction) {
			for (int str_pos=0; str_pos < final_string.Length; str_pos++){
				string temp = "";
				// try to find symbol in dictionary & apply the rule
				if(this.symbol_and_rule.TryGetValue(final_string.Substring(str_pos, 1), out temp))
					swap_string += temp;
				else // did not find it just add symbol 
					swap_string += final_string.Substring(str_pos, 1);
			}
			final_string = swap_string;
			swap_string = "";
			cycle++;			
		};

		return final_string;
	}
		
}
