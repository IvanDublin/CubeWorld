# CubeWorld - Red Box
Assignment - Game Engine 1 - DIT

Introduction 
My game have 2 scenes:
  - The first one is the main menu, now just have a button to start the game but 
    here would be instructions of the game and a ranking.
  - The second one is the game, the world is generated procedural and the ship use 
    something seen during the subject such as rigidbodies, colliders and so on. 
NOTE: In the scene2, after scene1, the light doesn't work but if we lunch just the scene2 it works.

Instructions
The main goal is destruct the maximum number of red boxes generated in each hill and 
get the maximum score for them. To do that, we move the ship for the world using:
  - 'a': left
  - 'd': right
  - 'w': forward
  - 's': back
  - Mouse: change the point of view
  - Space: shoot the bullets
To destroy each red box we have to shoot each one 3 times.

Components
Main controller (CreateWorld): Is in charge of generate the world. the procedure is the following:
      - The terrain is split in 9 segments, a grid of 3x3.
      - Randomly we get wich of this segment will have a hill.
      - For each segment with hill, create a gameobject hill and obtain the size and heigh of the hill randomly.
      - To construct the hill we put just the cubes in the exterior of the hill to have better performance.
      - Each cube is associate with a gameobject hill that have created it.
  After do that, to get the red boxes we have the function markCube:
      - First of all, delete the current red boxes.
      - Obtain for each gameobject hill a cube and mark as red. Put a rigidbody and attach a collsion script.
  Also, is in charge of manage the time of the game and update the score.

Shoot controller (Shooter): controller attached to the ship to manage the shoots and create each bullet.
      
Script collision (CubeCollision, ShipCollision):  I have two script to manage the collision of the cubes and the ship. 

Movement controller(MovementBullet, ShipController): I have two script to manage the movement of the ship and the bullet.
  Also, the bullet controller have the detection of colision with other objects. When it happens, the speed changes to 0 and
  change the rigidbody to have gravity. 
  
Sound: all the game have some sound to improve it. Have sounds in:
      - Propulsion: when move the ship
      - Shoot: when the ship create a bullet and shoot it
      - Collision of each object
      - Explosion: when one object is destroyed
    
