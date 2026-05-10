using Godot;
using System;

public partial class poupee_01 : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 7f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	
	private void _on_area_3d_body_entered(Node3D body)
	{
		if(body.Name == "poupee_02" || body.Name == "poupee_03" || body.Name == "poupee_04")
		{
			GD.Print("Ennemi !");
			body.QueueFree();
		}		
	}
	
	private void _on_area_3d_area_entered(Area3D area)
	{
		if(area.Name =="Fin")
		{
			GD.Print("Fin du niveau");
		}		
		if(area.Name =="Vide")
		{
			GD.Print("Tombé !");
		}	
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = new Vector3(-inputDir.X, 0, inputDir.Y).Normalized();
		
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.Z * Speed;
			velocity.Z = direction.X * Speed;
			
			// Rotation stable
			float angle = Mathf.Atan2(direction.Z, direction.X);
			Rotation = new Vector3(0, angle, 0);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
