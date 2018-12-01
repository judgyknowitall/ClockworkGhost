using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour {

	public List<LevelDescriptor> levels;

	public List<Pair> tileSetProto;
	Dictionary<TileType, GameObject> tileSet = new Dictionary<TileType, GameObject>();

	
	public GameObject test;
	public GameObject tile;
	public GameObject startTile;
	public GameObject specialTile;

	public int roomSize = 1;
	public float scale {
		get{return roomSize + 1; }
	}
	

	IEnumerator<LevelDescriptor> levelEnumerator;	//public Dictionary<TileType, GameObject> tileSet = new Dictionary<TileType, GameObject>();


	void Start () {
		foreach (var p in tileSetProto){
			tileSet[p.fst] = p.snd;
		}

		levelEnumerator = levels.GetEnumerator();
		levelEnumerator.MoveNext();

		var graph = GenerateGraph(25, 7);
		testGraph(graph.root);

		var start = Instantiate(startTile, transform);
		start.transform.position = special[0].position;

		for (var i = 1; i < special.Count(); i++){
			var spec = Instantiate(specialTile, transform);
			spec.transform.position = special[i].position;
		}
	}

	void testGraph(Node root){
		if (root == null) return;

		var room = new Room(roomSize, tileSet, transform, root);
		
		if (root.up != null){
			var bridge = Instantiate(tile, room.floor[0,0].transform);
			bridge.transform.position = root.position + Vector2.up * scale * 0.5f;
			bridge.gameObject.name = "To Up";
		}
		if (root.down != null){
			var bridge = Instantiate(tile, room.floor[0,0].transform);
			bridge.transform.position = root.position + Vector2.down * scale * 0.5f;
			bridge.gameObject.name = "To Down";
		}
		if (root.left != null){
			var bridge = Instantiate(tile, room.floor[0,0].transform);
			bridge.transform.position = root.position + Vector2.left * scale * 0.5f;
			bridge.gameObject.name = "To Left";
		}
		if (root.right != null){
			var bridge = Instantiate(tile, room.floor[0,0].transform);
			bridge.transform.position = root.position + Vector2.right * scale * 0.5f;
			bridge.gameObject.name = "To Right";
		}
		
		testGraph(root.up);
		testGraph(root.down);
		testGraph(root.left);
		testGraph(root.right);
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

	enum GraphDirections{UP, DOWN, LEFT, RIGHT}

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
						case 0:
							pos = floor[0, j].transform.position + (Vector3)(Vector2.left * 0.6f);
							break;
						case 1:
							pos = floor[floor.GetLength(0) - 1, j].transform.position + (Vector3)(Vector2.right * 0.6f);
							break;
						case 2:
							pos = floor[j, 0].transform.position + (Vector3)(Vector2.down * 0.6f);
							break;
						case 3:
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