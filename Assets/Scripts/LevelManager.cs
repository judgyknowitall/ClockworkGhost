﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour {

	public List<LevelDescriptor> levels;

	public List<Pair> tileSetProto;
	Dictionary<TileType, GameObject> tileSet = new Dictionary<TileType, GameObject>();

	public int roomSize = 1;
	public float scale {
		get{return roomSize + 1; }
	}	

	IEnumerator<LevelDescriptor> levelEnumerator;


	void Start () {
		foreach (var p in tileSetProto){
			tileSet[p.fst] = p.snd;
		}

		levelEnumerator = levels.GetEnumerator();
		levelEnumerator.MoveNext();

		var graph = GenerateGraph(25, 7);
		testGraph(graph.root);
		BuildAllHallways(graph.root);
	}

	void testGraph(Node root){
		if (root == null) return;

		var room = new Room(roomSize, tileSet, transform, root);
		root.room = room;
		
		testGraph(root.up);
		testGraph(root.down);
		testGraph(root.left);
		testGraph(root.right);
	}

	void BuildAllHallways(Node root){
		if (root == null) return;

		if (root.up != null){
			MakeHallway(root, root.up, GraphDirections.UP);
		}
		if (root.down != null){
			MakeHallway(root, root.down, GraphDirections.DOWN);
		}
		if (root.left != null){
			MakeHallway(root, root.left, GraphDirections.LEFT);
		}
		if (root.right != null){
			MakeHallway(root, root.right, GraphDirections.RIGHT);
		}

		BuildAllHallways(root.up);
		BuildAllHallways(root.down);
		BuildAllHallways(root.left);
		BuildAllHallways(root.right);
	}

	void MakeHallway(Node start, Node end, GraphDirections dir){
		var hallwayPos = Random.Range(0, start.room.walls.GetLength(1));
		var posStart = start.room.walls[(int)dir, hallwayPos].transform.position;
		Destroy(start.room.walls[(int)dir, hallwayPos]);
		start.room.walls[(int)dir, hallwayPos] = Instantiate(tileSet[TileType.FLOOR], transform);
		start.room.walls[(int)dir, hallwayPos].transform.position = posStart;

		var posEnd = end.room.walls[(int)dir.opposite(), hallwayPos].transform.position;
		Destroy(end.room.walls[(int)dir.opposite(), hallwayPos]);
		end.room.walls[(int)dir.opposite(), hallwayPos] = Instantiate(tileSet[TileType.FLOOR], transform);
		end.room.walls[(int)dir.opposite(), hallwayPos].transform.position = posEnd;

		for (var i = posStart; (i - posStart).sqrMagnitude <= (posEnd - posStart).sqrMagnitude; i += (Vector3)(dir.ToVector2() * 0.6f)){
			var hallFloorTile = Instantiate(tileSet[TileType.FLOOR], transform);
			hallFloorTile.transform.position = i;

			foreach (var j in dir.orthoganal()){
				var pos = i + (Vector3)(j.ToVector2() * 0.6f);
				var wall = Instantiate(tileSet[TileType.WALL], transform);
				wall.transform.position = pos;
			}
		}

	}

	#region Level Generation
	public void GenerateLevel(){ }

	List<Node> special = new List<Node>();

	private Graph GenerateGraph(uint length, uint complexity){ 
		var output = new Graph{
			root = new Node{
				position = Vector2.zero, 
				right = new Node{
					position = Vector2.zero + Vector2.right * scale
				}
			},
			nodes = new HashSet<Node>()
		};

		var current = output.root.right;
		for (var i = 0; i < complexity; i++){
			for (var j = 0; j < length; j++){
				var dir = (GraphDirections)Random.Range(0, (int)System.Enum.GetValues(typeof(GraphDirections)).Length);
				Vector2 newPos;
				Node[] nodeAtPos;
				switch (dir){
					case GraphDirections.UP:
						newPos = current.position + Vector2.up * scale;
						nodeAtPos = 
							(from node in output.nodes
							where node.position == newPos
							select node).ToArray();
						if (current.up == null && nodeAtPos.Length == 0){
							var node = new Node{position=newPos};
							output.nodes.Add(node);
							current.up = node;
							current = current.up;
						}
						else if (nodeAtPos.Length > 0){
							current = nodeAtPos[0];
						}else{
							current = current.up;

						}
						break;
					case GraphDirections.DOWN:
						newPos = current.position + Vector2.down * scale;
						nodeAtPos = 
							(from node in output.nodes
							where node.position == newPos
							select node).ToArray();
						if (current.down == null && nodeAtPos.Length == 0){
							var node = new Node{position=newPos};
							output.nodes.Add(node);
							current.down = node;
							current = current.down;
						}
						else if (nodeAtPos.Length > 0){
							current = nodeAtPos[0];
						}else{

							current = current.down;
						}
						break;

					case GraphDirections.LEFT:
						newPos = current.position + Vector2.left * scale;
						nodeAtPos = 
							(from node in output.nodes
							where node.position == newPos
							select node).ToArray();
						if (current.left == null && nodeAtPos.Length == 0){
							var node = new Node{position=newPos};
							output.nodes.Add(node);
							current.left = node;
							current = current.left;
						}
						else if (nodeAtPos.Length > 0){
							current = nodeAtPos[0];
						}else{
							current = current.left;
						}
						break;
					case GraphDirections.RIGHT:

						newPos = current.position + Vector2.right * scale;
						nodeAtPos = 
							(from node in output.nodes
							where node.position == newPos
							select node).ToArray();
						if (current.right == null && nodeAtPos.Length == 0){
							var node = new Node{position=newPos};
							output.nodes.Add(node);
							current.right = node;
							current = current.right;
						}
						else if (nodeAtPos.Length > 0){
							current = nodeAtPos[0];
						}else{
							current = current.right;
						}
						break;
				}
			}
			special.Add(current);
			current = output.root.right;
		}
		return output;
	}

	public enum GraphDirections{UP = 3, DOWN = 2, LEFT = 0, RIGHT = 1}

	class Graph{
		public Node root;
		public HashSet<Node> nodes;
	}

	class Node{
		public Vector2 position;
		public Room room;
		public Node up;
		public Node down;
		public Node left;
		public Node right;
	}

	#endregion

	[System.Serializable]
	public struct LevelDescriptor{
		public uint minDifficulty;
		public uint maxDifficulty;
		public uint length;
		public uint complexity;
		public Lore lore;
		public List<Spawner> spawners; 
	}

	class Room{
		public GameObject[,] floor;
		public GameObject[,] walls;
		public GameObject[] corners;

		public Room(int size, Dictionary<TileType, GameObject> tileSet, Transform transform, Node root){
			floor = new GameObject[size, size];
			walls = new GameObject[4, size];
			corners = new GameObject[4];

			for (var i = 0; i < floor.GetLength(0); i++){
				for (var j = 0; j < floor.GetLength(1); j++){
					floor[i,j] = Instantiate(tileSet[TileType.FLOOR], transform);
					var x = i - floor.GetLength(0) / 2f + 0.5f;
					var y = j - floor.GetLength(1) / 2f + 0.5f;
					var pos = root.position + new Vector2(x * 0.6f, y * 0.6f);
					floor[i,j].transform.position = pos;
				}
			}
			for (var i = 0; i < 4; i++){
				for (var j = 0; j < walls.GetLength(1); j++){
					walls[i, j] = Instantiate(tileSet[TileType.WALL], transform);
					Vector2 pos = Vector2.zero;

					switch(i){
						case 0: // Left Side
							pos = floor[0, j].transform.position + (Vector3)(Vector2.left * 0.6f);
							break;
						case 1: // Right Side
							pos = floor[floor.GetLength(0) - 1, j].transform.position + (Vector3)(Vector2.right * 0.6f);
							break;
						case 2: // Down Side
							pos = floor[j, 0].transform.position + (Vector3)(Vector2.down * 0.6f);
							break;
						case 3: // Up Side
							pos = floor[j, floor.GetLength(1) - 1].transform.position + (Vector3)(Vector2.up * 0.6f);
							break;
					}

					walls[i, j].transform.position = pos;
				}
			}

			corners[0] = Instantiate(tileSet[TileType.CORNER], transform);
			corners[1] = Instantiate(tileSet[TileType.CORNER], transform);
			corners[2] = Instantiate(tileSet[TileType.CORNER], transform);
			corners[3] = Instantiate(tileSet[TileType.CORNER], transform);

			corners[0].transform.position = new Vector2(
				(walls[0, 0].transform.position + (Vector3)(Vector2.up * 0.6f)).x,
				(walls[3, 0].transform.position + (Vector3)(Vector2.right * 0.6f)).y);
			corners[1].transform.position = new Vector2(
				(walls[1, 0].transform.position + (Vector3)(Vector2.up * 0.6f)).x,
				(walls[3, walls.GetLength(1) - 1].transform.position + (Vector3)(Vector2.left * 0.6f)).y);
			corners[2].transform.position = new Vector2(
				(walls[0, walls.GetLength(1) - 1].transform.position + (Vector3)(Vector2.down * 0.6f)).x,
				(walls[2, 0].transform.position + (Vector3)(Vector2.left * 0.6f)).y);
			corners[3].transform.position = new Vector2(
				(walls[2, walls.GetLength(1) - 1].transform.position + (Vector3)(Vector2.right * 0.6f)).x,
				(walls[1, 0].transform.position + (Vector3)(Vector2.down * 0.6f)).y);
		}
	}

	public enum TileType{FLOOR, WALL, CORNER}


	[System.Serializable]
	public struct Pair{
		public TileType fst;
		public GameObject snd;
	}
}


static class MyExtensions{
	public static LevelManager.GraphDirections opposite(this LevelManager.GraphDirections dir){
		switch (dir){
			case LevelManager.GraphDirections.UP:
				return LevelManager.GraphDirections.DOWN;
			case LevelManager.GraphDirections.DOWN:
				return LevelManager.GraphDirections.UP;
			case LevelManager.GraphDirections.LEFT:
				return LevelManager.GraphDirections.RIGHT;
			case LevelManager.GraphDirections.RIGHT:
				return LevelManager.GraphDirections.LEFT;
			default:
				return (LevelManager.GraphDirections)0;
		}
	}

	public static Vector2 ToVector2(this LevelManager.GraphDirections dir){
		switch (dir){
			case LevelManager.GraphDirections.UP:
				return Vector2.up;
			case LevelManager.GraphDirections.DOWN:
				return Vector2.down;
			case LevelManager.GraphDirections.LEFT:
				return Vector2.left;
			case LevelManager.GraphDirections.RIGHT:
				return Vector2.right;
			default:
				return Vector2.zero;
		}
	}

	public static LevelManager.GraphDirections[] orthoganal(this LevelManager.GraphDirections dir){
		switch (dir){
			case LevelManager.GraphDirections.UP:
			case LevelManager.GraphDirections.DOWN:
				return new LevelManager.GraphDirections[]{LevelManager.GraphDirections.LEFT, LevelManager.GraphDirections.RIGHT};
			case LevelManager.GraphDirections.LEFT:
			case LevelManager.GraphDirections.RIGHT:
				return new LevelManager.GraphDirections[]{LevelManager.GraphDirections.UP, LevelManager.GraphDirections.DOWN};
			default:
				return null;
		}
	}
}