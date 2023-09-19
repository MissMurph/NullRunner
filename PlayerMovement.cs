using Godot;
using System;

public partial class PlayerMovement : RigidBody3D {

	[Export]
	private float acceleration = 6f;

	private Vector2 moveInput;

	private Node3D head;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		head = GetNode<Node3D>("Head");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

	}

	public override void _PhysicsProcess (double delta) {
		base._PhysicsProcess(delta);

		moveInput = Input.GetVector("move_left", "move_right", "move_backward", "move_forward");

		//Vector3 direction = head.GlobalRotation.
	}
}