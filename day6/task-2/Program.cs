using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Program
{
	private int[,] lights;
	private List<string> commands;
	private string coordsRegex = @"\d{0,3},\d{0,3}";
	private string commandRegex = @"turn off|turn on|toggle";
	
	public Program(List<string> commands) {
		this.commands = commands;
		this.lights = new int[1000, 1000];
	}

	public void processCommands() {
		Regex coords = new Regex(coordsRegex, RegexOptions.IgnoreCase);
		Regex cmd = new Regex(commandRegex, RegexOptions.IgnoreCase);
		foreach (string command in commands) {
			MatchCollection c = coords.Matches(command);
			int[] start = getCoord(c[0].Groups[0].Captures[0].Value);
			int[] end = getCoord(c[1].Groups[0].Captures[0].Value);
			Match m = cmd.Match(command);
			string opt = m.Groups[0].Captures[0].Value;
			for (int x = start[0]; x <= end[0]; x++) {
				for (int y = start[1]; y <= end[1]; y++) {
					setLight(x, y, opt);
				}
			}
		}
	}

	public int getLightBrightness() {
		int total = 0;
		foreach (int light in this.lights) {
			total += light;
		}
		return total;
	}

	private int[] getCoord(string coordStr) {
		int[] coords = new int[2];
		string[] parts = coordStr.Split(',');
		for (int i = 0; i < parts.Length; i++) {
			coords[i] = Int32.Parse(parts[i]);
		}
		return coords;
	}

	private void setLight(int x, int y, string opt) {
		switch (opt) {
			case "turn on":
				this.lights[x, y] += 1;
				break;
			case "turn off":
				if (this.lights[x, y] > 0) {
					this.lights[x, y] -= 1;
				}
				break;
			case "toggle":
				this.lights[x, y] += 2;;
				break;
		}
	}

	public static void Main(string[] args)
	{
		List<string> lines;
		if (args.Length == 0) {
			lines = readStdIn();
		} else {
			lines = readFile(args[0]);
		}
		Program p = new Program(lines);
		p.processCommands();
		Console.WriteLine("Total brightness: {0}", p.getLightBrightness());
	}

	public static List<string> readStdIn()
	{
		List<string> lines = new List<string>();
		string line;
		while ((line = Console.ReadLine()) != null && line != "") {
			lines.Add(line);
		}
		return lines;
	}

	public static List<string> readFile(string fileName)
	{
		List<string> lineList = new List<string>();
		string[] lines = File.ReadAllLines(fileName);
		foreach (string line in lines) {
			lineList.Add(line);
		}
		return lineList;
	}
}
