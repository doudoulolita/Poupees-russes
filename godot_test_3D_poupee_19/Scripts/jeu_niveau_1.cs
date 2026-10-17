using Godot;
using System;

public partial class jeu_niveau_1 : Node3D
{
	[Export]
	public PackedScene attak { get; set; }
	
	private void _on_attak_timer_timeout()
	{
		// crée une nouvelle instance de fleur attack dans la scène attak
		attak Attak = attak.Instantiate<attak>();
		
		var attakSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		// donne un offset aléatoire
		attakSpawnLocation.ProgressRatio = GD.Randf();
		
		Vector3 playerPosition = GetNode<poupee_01>("poupee_01").GlobalPosition;
		Attak.Initialize (attakSpawnLocation.GlobalPosition, playerPosition);
		
		// Vector3 ciblePlateforme = Vector3.Zero;
		// Attak.Initialize (attakSpawnLocation.GlobalPosition, ciblePlateforme);
		
		// instancie la fleur Attak dans la scène principale
		AddChild(Attak);
	}
}



