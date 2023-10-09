using Godot;
using System;

public partial class Limb : PhysicalBone3D {

	[Signal]
	public delegate void LimbHitEventHandler (Limb limb, Vector3 direction);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

	}

	public void AttackLimb (Vector3 direction) {
		GD.Print("calling signal");
		EmitSignal(SignalName.LimbHit, this, direction);
	}
}