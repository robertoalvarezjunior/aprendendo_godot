using Godot;
using System;

enum PlayerState
{
	Idle,
	Walk,
	Jump
}

public partial class Player : CharacterBody2D
{
	public const float Speed = 100.0f;
	public const float JumpVelocity = -300.0f;
	private AnimatedSprite2D anim;
	private PlayerState state;

	private Vector2 velocity;

	public override void _Ready()
	{
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GoIdle();
	}

	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		switch (state)
		{
			case PlayerState.Idle:
				IdleState();
				break;
			case PlayerState.Walk:
				WalkState();
				break;
			case PlayerState.Jump:
				JumpState();
				break;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void GoIdle()
	{
		state = PlayerState.Idle;
		anim.Play("idle");
	}

	private void GoWalk()
	{
		state = PlayerState.Walk;
		anim.Play("walk");
	}

	private void GoJump()
	{
		state = PlayerState.Jump;
		anim.Play("jump");
		velocity.Y = JumpVelocity;
	}

	private void IdleState()
	{
		Move();
		if (velocity.X != 0)
		{
			GoWalk();
			return;
		}
		if (Input.IsActionJustPressed("up"))
		{
			GoJump();
			return;
		}
	}

	private void WalkState()
	{
		Move();

		if (velocity.X == 0)
		{
			GoIdle();
			return;
		}
		if (Input.IsActionJustPressed("up"))
		{
			GoJump();
			return;
		}
	}

	private void JumpState()
	{
		Move();

		if (IsOnFloor())
		{
			if (velocity.X == 0)
			{
				GoIdle();
				return;
			}
			else
			{
				GoWalk();
				return;
			}
		}
	}

	private void Move()
	{

		float direction = Input.GetAxis("left", "right");

		if (direction != 0)
		{
			velocity.X = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
		}

		if (direction > 0)
		{
			anim.FlipH = false;
		}
		else if (direction < 0)
		{
			anim.FlipH = true;
		}
	}

}
