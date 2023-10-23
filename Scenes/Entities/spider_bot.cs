using Godot;
using System;

public partial class spider_bot : CharacterBody3D {

	[Export]
	private float moveSpeed = 5.0f;
	[Export]
	private float turnSpeed = 1.0f;

	[Export]
	private Node3D target;

	private Vector3 currentDirection;

	public override void _Process (double delta) {
		//Vector3 direction
	}

	private NavigationAgent3D navAgent;

	//private Vector3 _movementTargetPosition = new Vector3(-3.0f, 0.0f, 2.0f);

	private Vector3 movementTarget;

	public Vector3 MovementTarget {
		get { return navAgent.TargetPosition; }
		set { navAgent.TargetPosition = value; }
	}

	public override void _Ready () {
		base._Ready();

		currentDirection = new Vector3();

		navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

		// These values need to be adjusted for the actor's speed
		// and the navigation layout.
		navAgent.PathDesiredDistance = 0.5f;
		navAgent.TargetDesiredDistance = 0.5f;

		// Make sure to not await during _Ready.
		//Callable.From(ActorSetup).CallDeferred();
	}

	public override void _PhysicsProcess (double delta) {
		base._PhysicsProcess(delta);

		//Vector3 currentAgentPosition = GlobalTransform.Origin;
		//Vector3 nextPathPosition = navAgent.GetNextPathPosition();

		Vector3 targetDirection = (NoY(target.GlobalPosition) - NoY(GlobalPosition)).Normalized();

		//Vector3 newLookDirection = currentDirection.Lerp(targetDirection, turnSpeed * (float)delta);

		//currentDirection = newLookDirection;

		LookAt(target.GlobalPosition);
		
		//float angle = direction.AngleTo(target.GlobalPosition);

		//RotateObjectLocal(Vector3.Up, Math.Min(angle, turnSpeed) * (float)delta);

		Vector3 newVelocity = targetDirection * moveSpeed;

		Velocity = newVelocity;

		MoveAndSlide();
	}

	private static Vector3 NoY (Vector3 input) {
		return new Vector3(input.X, 0f, input.Z);
	}
}