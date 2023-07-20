﻿using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Sandbox;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
public partial class Arcade : GameManager
{
	public Arcade()
	{
	}

	public override void Spawn()
	{
		Log.Info( "Spawn()" );
		base.Spawn();
	}

	public override void ClientSpawn()
	{
		Log.Info( "ClientSpawn()" );
		base.ClientSpawn();

		Game.RootPanel = new Hud();
		_ = new SceneScreen() { Position = Vector3.Up * 128f };
	}


	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var pawn = new Pawn();
		client.Pawn = pawn;

		pawn.Position = Vector3.Up * 128f; // temp
	}
}
