using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
	[Export] public Vector3 cameraOffset;
	public CharacterBody3D target;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		target = (CharacterBody3D)GetParent().GetNode("poupee_01");
		
		var playerPosition = target.Position;
		var cameraPosition = Position;
		cameraOffset = cameraPosition - playerPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var playerPosition = target.Position;
		// on positionne la camera par rapport à la poupée
		Position = playerPosition + cameraOffset;
	}
}
