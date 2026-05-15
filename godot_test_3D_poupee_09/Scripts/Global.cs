using Godot;
using System;


public partial class Global : Node
{
	
		
	public int IncreaseCount(int value)
	{
		return value + 1;
	}
	
	public int DecreaseCount(int value)
	{
		return value + 1;
	}
	
	public void PrintCount(int value)
	{
		GD.Print(value);
	}
	
	
	
}
