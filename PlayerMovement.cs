using Godot;
using System;

public partial class PlayerMovement : RigidBody3D {

	[Export]
	private float acceleration = 10f;

	[Export]
	private float speed = 10f;

	[Export]
	private float maxSpeed = 20f;

	[Export]
	private float baseMovementModifier = 1f;

	private float currentMovementModifier;

	[Export]
	private float airModifier = 0.1f;

	[Export]
	private Vector3 velocity = new();

	[Export]
	private float jumpForce = 10f;

	[Export]
	private float stopSpeed = 0.1f;

	Vector3 direction = new();

	private Vector2 moveInput;

	private Node3D head;

	private RayCast3D feet;

	private bool grounded;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		head = GetNode<Node3D>("Head");
		feet = GetNode<RayCast3D>("Feet");

		currentMovementModifier = baseMovementModifier;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		

		if (feet.IsColliding()) {
			grounded = true;
			currentMovementModifier = baseMovementModifier;
		}
		else {
			grounded = false;
			currentMovementModifier = airModifier;
		}
	}

	public override void _PhysicsProcess (double delta) {
		moveInput = Input.GetVector("move_left", "move_right", "move_backward", "move_forward");

		direction += head.GlobalTransform.Basis.X * moveInput.X;
		direction -= head.GlobalTransform.Basis.Z * moveInput.Y;

		velocity = velocity.Lerp(direction * speed, acceleration * currentMovementModifier * (float)delta);

		ApplyCentralForce(velocity);
	}

	public override void _IntegrateForces (PhysicsDirectBodyState3D state) {
		/*if (moveInput.Length() < 0.2f) {
			float x = state.LinearVelocity.X;
			float y = state.LinearVelocity.Z;

			x = Mathf.Lerp(x, 0, stopSpeed);
			y = Mathf.Lerp(y, 0, stopSpeed);

			state.LinearVelocity = new Vector3(x, state.LinearVelocity.Y, y);
		}*/
	}

	public override void _Input (InputEvent @event) {
		if (@event.IsActionPressed("jump") && grounded) {
			ApplyCentralImpulse(new Vector3(0, jumpForce, 0));
		}
	}
}