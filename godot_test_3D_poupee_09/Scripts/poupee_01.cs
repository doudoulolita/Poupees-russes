using Godot;
using System;

public partial class poupee_01 : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 7f;
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	
	public int nbPoints = 0;
	public int nbPoupees = 0;
	public int nbFleurs = 0;
	
	Label cpt;
	Label cpt2;
	Label cpt3;
	

	public override void _Ready()
	{
		cpt = GetParent().GetNode("GUI/Compteur") as Label;
		cpt2 = GetParent().GetNode("GUI/Compteur2") as Label;
		cpt3 = GetParent().GetNode("GUI/Compteur3") as Label;
		
		cpt.Text = "Votre score est de 0"; // initialisation
		cpt2.Text = "Nombre de poupées attrapées : "; // initialisation
		cpt3.Text = "Nombre de fleurs touchées :"; // initialisation
		

	}
	
	private void _on_area_3d_body_entered(Node3D body)
	{	
		var globalScript = GetNode<Global>("/root/Global");
		

		for(int i=0; i<5; i++)
		{
			if(body.Name == "poupee_0" + i)
			{	
				body.QueueFree();
				GD.Print("Nombre de points : ");
				
				nbPoupees = globalScript.IncreaseCount(nbPoupees);
				globalScript.PrintCount(nbPoupees);
				cpt2.Text = "Nombre de poupées attrapées : " + nbPoupees;
			}
		}
		
			for(int i=0; i<7; i++)
		{
			if(body.Name == "fleur_0" + i)
			{	
				GD.Print("Nombre de points : ");
				cpt3.Text = "Nombre de fleurs touchées : ";
				nbFleurs = globalScript.IncreaseCount(nbFleurs);
				globalScript.PrintCount(nbFleurs);
				cpt3.Text = "Nombre de fleurs touchées : " + nbFleurs;
			}
			
			nbPoints = nbPoupees - nbFleurs;	
			globalScript.PrintCount(nbPoints);
			
			if(nbPoints>0) 
			{
				cpt.Text = "Vous avez gagné " + nbPoints + " point(s)";
			}
			if(nbPoints<0) 
			{
				cpt.Text = "Vous avez perdu " + Math.Abs(nbPoints) + " point(s)";
			}	
			if(nbPoints==0)
			{
				cpt.Text = "Votre score est de 0";
			}
		}	
	}
	
	private void _on_area_3d_area_entered(Area3D area)
	{
		
		if(area.Name =="Fin")
		{
			GD.Print("Fin du niveau");
			
			cpt.Text = "Fin du niveau. ";
			
			if(nbPoints>0) 
			{
				cpt.Text += "Vous avez gagné " + nbPoints + " point(s). Bravo ! ";
			}
			if(nbPoints<0) 
			{
				cpt.Text += "Vous avez perdu " + Math.Abs(nbPoints) + " point(s). Dommage !";
			}	
			if(nbPoints==0)
			{
				cpt.Text += "Votre score est de 0. Dommage !";
			}

		}		
		if(area.Name =="Vide")
		{
			GD.Print("Tombé !");
			cpt.Text = "Tombé !";
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
