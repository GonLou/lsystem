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
	float unit_size;
	string full_string;

	/// default constructor
	public StringCreator() {
		this.symbol_and_rule.Add("F", "F+F-F-F+F");
		this.axiom = "-F";
		this.interaction = 4;
		this.angle = 90.0f;
		this.full_string = "";
		this.unit_size = 0.1f;
	}
	
	/// constructor with four parameters
	/// <param name="symbol">Used to specify the symbol.</param>
	/// <param name="rule">Used to specify the rule.</param>
	/// <param name="axiom">Used to specify the axiom.</param>
	/// <param name="angle">Used to specify the angle.</param>
	public StringCreator(string symbol, string rule, string axiom, int interaction, float angle, float unit_size) {
		string[] alphabet_func = new string[10];
		string[] rule_func = new string[10];

		// separates the symbols
		symbol = symbol.Replace(" ", "");
		symbol = symbol.Replace(",", "");
		for (int str_pos=0; str_pos < symbol.Length; str_pos++) {
			alphabet_func[str_pos] = symbol.Substring(str_pos, 1);
		}

		// separates the rules
		int count_rules = 0;
		rule = rule.Replace(" ", "");
		rule = rule.Replace("->", "");
		for (int str_pos=1; str_pos < rule.Length; str_pos++) {
			if (rule.Substring(str_pos, 1) == ";") {
				str_pos++;
				count_rules++;
			} else {
				rule_func[count_rules] = rule_func[count_rules] + rule.Substring(str_pos, 1);
			}
		}

		// adds keys and rules to the dictionary
		for (int attr = 0; attr < 10; attr++) 
			if (alphabet_func[attr] != null) 
				this.symbol_and_rule.Add(alphabet_func[attr], rule_func[attr]);

		this.axiom = axiom;
		this.interaction = interaction;
		this.angle = angle;
		this.full_string = "";
		this.unit_size = unit_size;
	}

	/// destructor
	~StringCreator() {
	}

	public void setFullString(string full_string) {
		this.full_string = full_string;
	}

	public void setAngle(float angle) {
		this.angle = angle;
	}

	public void setUnitSize(float unit_size) {
		this.unit_size = unit_size;
	}

	public string getFullString() {
		return this.full_string;
	}

	public float getAngle() {
		return this.angle;
	}

	public float getUnitSize() {
		return this.unit_size;
	}

	/// <summary>
	/// To match the symbols with the rules
	/// </summary>
	/// <returns>The dic value.</returns>
	/// <param name="symbol">Symbol.</param>
	public string getDicValue(string symbol) {
		string value = "";
		if(symbol_and_rule.TryGetValue(symbol, out value))
			return value;
		else
			return value;
	}

	/// <summary>
	/// constructor with one parameter
	/// </summary>
	/// <returns>Returns the final completed string.</returns>
	public string Start() {
		int cycle = 1;
		string final_string = this.axiom;
		string swap_string = "";

		while(cycle <= this.interaction) {
			string temp = "";

			foreach(KeyValuePair<string, string> entry in this.symbol_and_rule)
				if(this.symbol_and_rule.TryGetValue(entry.Key, out temp))
					final_string = final_string.Replace(entry.Key, temp);

			cycle++;			
		};

		setFullString(final_string);
		Debug.Log ( final_string);

		return final_string;
	}
		
}