using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LevelManager : MonoBehaviour {

	public List<LevelDescriptor> levels;
	public GameObject test;

	IEnumerator<LevelDescriptor> levelEnumerator;

	// Use this for initialization
	void Start () {
		levelEnumerator = levels.GetEnumerator();
		var graph = GenerateGraph(10, 3);
		testGraph(graph.root);
	}

	void testGraph(Node root){
		if (root == null) return;
		var thing = Instantiate(test, transform);
		thing.transform.position = root.position;
		testGraph(root.up);
		testGraph(root.down);
		testGraph(root.left);
		testGraph(root.right);
	}

	#region Level Generation
	public void GenerateLevel(){ }

	private Graph GenerateGraph(uint length, uint complexity){ 
		var output = new Graph{
			root = new Node{
				position = Vector2.zero, 
				right = new Node{
					position = Vector2.zero + Vector2.left
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
						newPos = current.position + Vector2.up;
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
						}
						break;
					case GraphDirections.DOWN:
						newPos = current.position + Vector2.down;
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
						}
						break;
					case GraphDirections.LEFT:
						newPos = current.position + Vector2.left;
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
						}
						break;
					case GraphDirections.RIGHT:
						newPos = current.position + Vector2.right;
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
						}
						break;
				}

			}
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
		public Node up;
		public Node down;
		public Node left;
		public Node right;
	}

	#endregion

	[System.Serializable]
	public struct LevelDescriptor{
		uint minDifficulty;
		uint maxDifficulty;
		uint length;
		uint complexity;
		Lore lore;
		List<Spawner> spawners; 
	}
}
