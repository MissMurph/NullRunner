using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Body : Node3D {

	private Skeleton3D rig;

	//private Queue<Limb>

	public override void _Ready() {
		rig = GetNode<Skeleton3D>("Armature/GeneralSkeleton");

		foreach (Node limbNode in GetNode("Armature/GeneralSkeleton").GetChildren(false)) {
			if (limbNode is Limb limb) {
				//GD.Print("connecting signal");
				limb.Connect(Limb.SignalName.LimbHit, Callable.From<Limb, Vector3>(OnLimbHit));
			}
		}
	}

	public override void _Process(double delta) {

	}

	private void OnLimbHit (Limb limb, Vector3 direction) {
		GD.Print(rig.GetBoneName(limb.GetBoneId()));

		Array<StringName> array = new Array<StringName>() { new StringName(rig.GetBoneName(limb.GetBoneId())) };

		//rig.PhysicalBonesStartSimulation(array);

		limb.JointType = PhysicalBone3D.JointTypeEnum.None;

		//GetTree().Root.AddChild(limb);

		rig.PhysicalBonesStartSimulation(array);

		limb.ApplyCentralImpulse(direction * 5f);

		
		

		limb.Disconnect(Limb.SignalName.LimbHit, Callable.From<Limb, Vector3>(OnLimbHit));

		//rig.PhysicalBonesStopSimulation();
	}
}