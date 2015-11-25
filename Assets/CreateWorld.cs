using UnityEngine;
using System.Collections;

public class CreateWorld : MonoBehaviour {

	public int SIZE_X;
	public int SIZE_Y;
	public int SIZE_Z;

	public int MIN_HILL;
	public int MAX_HILL;
	public int MIN_HEIGHT_HILL;
	public int MIN_SIZE_HILL;
	public int MAX_SIZE_HILL;
	
	public bool[,,] world;

	public GameObject Ground;
	public GameObject Water;
	public GameObject Sand;

	// Use this for initialization
	void Start () {

		world = new bool[SIZE_X,SIZE_Y,SIZE_Z];

		doHill(0,0,0,SIZE_X-8,4,SIZE_Z-8);
		putWater(2);

		int countHill = Random.Range(MIN_HILL, MAX_HILL);

		for (int i=0; i<countHill; i++) {
			int size = Random.Range(MIN_SIZE_HILL, MAX_SIZE_HILL);
			int height = Random.Range(MIN_HEIGHT_HILL, size);
			int xi = Random.Range((-SIZE_X+5) / 2 + size, (SIZE_X-5) / 2 - size);
			int zi = Random.Range((-SIZE_Z+5) / 2 + size, (SIZE_Z-5) / 2 - size);

			doHill(xi,4,zi,size,height,size);
		}


	}

	void doHill(int xi, int yi, int zi, int width, int height, int depth) {
		int halfWidth = width / 2;
		int halfDepth = depth / 2;

		// Make a mountain
		for (int y = 0; y < height; y++) {
			for (int x = y; x < width-y; x++) {			
				if (y==height-1 || x==y || x==width-y-1) {
					for (int z = y; z < depth-y; z++) {
						//bool draw = true;
						/*if (x==y || z==y || x==width-y || z==depth-y) {
						draw = Random.Range(0, 4)!=0;
					}*/
						Vector3 p;
						p.x = xi + x - halfWidth;
						p.y = yi+y;
						p.z = zi + z - halfDepth;
						
						if (!world[(int)p.x + SIZE_X/2, (int)p.y, (int)p.z + SIZE_Z/2])
							drawCube(p);						   
					}
				} else {
					int z1, z2;
					z1 = y;
					z2 = depth-y-1;

					Vector3 p;
					p.x = xi + x - halfWidth;
					p.y = yi+y;
					p.z = zi + z1 - halfDepth;

					if (!world[(int)p.x + SIZE_X/2, (int)p.y, (int)p.z + SIZE_Z/2])
						drawCube(p);	

					p.z = zi + z2 - halfDepth;
					if (!world[(int)p.x + SIZE_X/2, (int)p.y, (int)p.z + SIZE_Z/2])
						drawCube(p);	
				}

				for (int z = y; z < depth-y; z++) {
					Vector3 p;
					p.x = xi + x - halfWidth;
					p.y = yi+y;
					p.z = zi + z - halfDepth;
					world[(int)p.x + SIZE_X/2, (int)p.y, (int)p.z + SIZE_Z/2] = true;
				}
			}
		}
	}

	void drawCube(Vector3 pos) {
		GameObject newCube = GameObject.Instantiate<GameObject>(Ground);
		newCube.transform.position = pos;
		newCube.transform.parent = transform;
	}

	void putWater(int deep) {
		int halfWidth = SIZE_X / 2;
		int halfDepth = SIZE_Z / 2;
		for (int y = 0; y < deep; y++) {
			for (int x = 0; x < SIZE_X; x++) {
				for (int z = 0; z < SIZE_Z; z++) {
					if (!world[x,y,z]) {
						GameObject newCube = GameObject.Instantiate<GameObject>(Water);
						Vector3 p = newCube.transform.position;
						p.x = x - halfWidth;
						p.y = y;
						p.z = z - halfDepth;
						newCube.transform.position = p;
						newCube.transform.parent = transform;
						
						world[x,y,z] = true;
					}
				}
			}
		}

	}

	// Update is called once per frame
	void Update () {
	
	}
}
