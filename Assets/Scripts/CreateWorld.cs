using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateWorld : MonoBehaviour {

	//Parameters of the game

	//Dimension of the world
	public int SIZE_X;
	public int SIZE_Y;
	public int SIZE_Z;

	//Max and min of hill number
	public int MIN_HILL;
	public int MAX_HILL;

	//Size max and min of the hills
	public float MIN_HEIGHT_HILL;
	public float MIN_SIZE_HILL;
	public float MAX_SIZE_HILL;

	public int COLUMNS = 3;
	public int ROWS = 3;

	//Template Gameobject to generate the gorund of each hill
	public GameObject Ground;

	//Sounds for colision and explosion
	public AudioClip audioColission;
	public AudioClip audioExplosion;

	//Time to generate new Red Box
	public float TIME_CHANGE = 10;

	//Value of destruct one cube
	public int POINTS_CUBE = 100;

	//Objects to set the differents texts
	public Text scoreText;
	public Text finalScore;
	public Text timeText;
	public GameObject canvasScore;
	public GameObject canvasTime;
	public GameObject canvasGameOver;

	//Total game time
	public float totalTime = 60f;

	//References of the ship
	public GameObject ship;
	
 	//Started time of differents events
	float iniTime;
	float timeRedBox;

	//Current Red Boxes
	GameObject[] activeCubes;

	//Current score
	int score;

	//The game is alive
	bool alive = false;


	//Function to detect when start the game, after main menu
	void OnLevelWasLoaded(int level) {
		iniTime = timeRedBox = Time.time;
		score = 0;
		alive = true;
	}


	// Use this for initialization
	void Start () {
		int countHill = Random.Range(MIN_HILL, MAX_HILL);

		activeCubes = new GameObject[COLUMNS*ROWS];
		bool[] cuad = new bool[COLUMNS*ROWS];

		//Obtain random segments where generate one hill
		int cont = 0;
		while (cont<countHill) {
			int numCuad = Random.Range(0, COLUMNS*ROWS);
			if (!cuad[numCuad]) {
				cuad[numCuad] = true;
				cont++;
			}
		}

		//Generate a hill in the segments marked before 
		for (int j=0; j<COLUMNS; j++) {
			for (int k=0; k<ROWS; k++) {
				int numCuad = j*ROWS+k;
				if (cuad[numCuad]) {
					doCuad(numCuad);
				}
			}
		}

		//Mark the first red boxes
		markCube();

	/*
		Descomment if we want to test just the scene 2

		iniTime = timeRedBox = Time.time;
		score = 0;
		alive = true;
		
	*/
	}

	/**
	 *	doCuad() 
	 *  Generate the hill of one segment
	 *  Obtain random size and height of the hill
	 */
	void doCuad(int numCuad) {
		float size_cuad_x = SIZE_X/COLUMNS;
		float size_cuad_z = SIZE_Z/ROWS;
	
		float size = Random.Range(MIN_SIZE_HILL, MAX_SIZE_HILL);
		float height = Random.Range(MIN_HEIGHT_HILL, size);

		float xi = numCuad%COLUMNS*size_cuad_x + size_cuad_x/2 - size/2;
		float zi = numCuad/COLUMNS*size_cuad_z + size_cuad_z/2 - size/2;
		
		doHill(numCuad,xi,0.35f,zi,size,height,size);
	
	}

	/**
	 *  doHill()
	 *  Receiving the parameters of the hill, draw the cubes to generate it.
	 * 	Just put the external cube of the hills.
	 */
	void doHill(int numCuad, float xi, float yi, float zi, float width, float height, float depth) {
		float halfWidth = SIZE_X / 2f;
		float halfDepth = SIZE_Z / 2f;

		GameObject goHill = new GameObject(string.Concat("Hill-",numCuad));
		goHill.transform.parent = transform;

		// Make a mountain
		for (float y = 0f, start = 0f; y < height; y+=0.1f, start+=0.05f) {
			for (float x = start; x <= width-start; x+=0.1f) {	
				for (float z = start; z <= depth-start; z+=0.1f) {
					if (x==start || z==start || x+0.1f>=width-start || z+0.1f>=depth-start) {
						Vector3 p;
						p.x = xi + x - halfWidth;
						p.y = yi + y;
						p.z = zi + z - halfDepth;					

						drawCube(goHill,p,(int)(p.x + halfWidth)*10, (int)(p.y*10), (int)(p.z + halfDepth)*10);						   					
					}				
				}
			}
		}
	}

	/**
	 *  drawCube()
	 *  With the posision generate a cube using the Ground as template
	 *  Also attach to the parent, that was the gameobject hill
	 */
	void drawCube(GameObject parent,Vector3 pos,int x, int y, int z) {
		GameObject newCube = GameObject.Instantiate<GameObject>(Ground);
		newCube.name = string.Format("Ground{0}{1}{2}",x,y,z);
		newCube.transform.position = pos;
		newCube.transform.parent = parent.transform;
	}

	/**
	 *  markCube()
	 *  Function to destroy the current Red Boxes and obtain the new ones.
	 *  To obtain the new ones, for each hill get a random cube and set red box.
	 *  A red box have a rigidbody with gravity to make more realistic and a CubeColision script
	 *  to detect the collsion of the bullets
	 */
	void markCube() {
		for(int i=0; i<activeCubes.Length; i++) {
			if (activeCubes[i]!=null) {
				GameObject oldCube = activeCubes[i];
				Destroy(oldCube);
			}		
		}

		foreach (Transform childHill in transform) {
			int numHill = System.Int32.Parse(childHill.name.Split('-')[1]);
			if (childHill.childCount==0)
				Destroy(childHill.gameObject);
			else {			
				int numChild = Random.Range(0,childHill.childCount);
				GameObject myGameObject = childHill.GetChild(numChild).gameObject;
				Rigidbody gameObjectsRigidBody = myGameObject.AddComponent<Rigidbody>(); // Add the rigidbody.
				gameObjectsRigidBody.mass = 1;
				
				myGameObject.GetComponent<Renderer>().material.color = Color.red;

				CubeCollision cc = myGameObject.AddComponent<CubeCollision>();
				cc.audioColission = audioColission;
				cc.audioExplosion = audioExplosion;
				cc.Parent = this;
				
				activeCubes[numHill] = myGameObject;
			}
		}

		if (transform.childCount==0) Debug.Log("Game Over");
	}

	/**
	 *  destroyCube()
	 *  Funciton called for the CubeCollision script when a box is detroyed
	 *  When it happens add the point of the one cube to the score.
	 */
	public void destroyCube(GameObject parent) {
		score += POINTS_CUBE;
		scoreText.text = string.Format("Score: {0:D5}",score);
	}

	/**
	 *  GameOver()
	 *  In the end of the game, hide the time and score canvas and show the gameover panel.
	 *  Also destroy the ship and cahnge the active camara to the main one.
	 */
	public void gameOver() {
		GameObject maincamera = GameObject.FindGameObjectWithTag("MainCamera");
		maincamera.GetComponent<Camera>().enabled = true;
		Debug.Log("Game Over");
		Destroy(ship);

		finalScore.text = string.Format("Final Score: {0:D5}",score);
		canvasTime.SetActive(false);
		canvasScore.SetActive(false);
		canvasGameOver.SetActive(true);
	}

	// Update is called once per frame
	// Updating the time of the timecanvas and detect when it finished.
	// Also manage the time of the red boxes and called to markCube to generate the new ones.
	void Update () {
		if (alive) {
			float currentTime = Time.time-iniTime;
			float rest = totalTime-currentTime;

			if (rest<1 && rest>=0) rest = 0;

			if (rest >= 0) {
				timeText.text = string.Format("Time: {0:F0}s",rest);
				if (Time.time-timeRedBox > TIME_CHANGE) {
					timeRedBox = Time.time;
					markCube();
				}
			} else { //GAME OVER
				alive = false;
				gameOver();
			}
		}
	}
}
