**Law-Game Project: Bullet Trajectory Simulation and Shooter Position Detection **
# Forward Scenario: Bullet Trajectory Simulation
The forward scenario simulates bullet trajectories from different shooter positions. We use a bullet asset, a sphere with small dimensions and gold color, which has a Sphere Collider and Collision Detection set to Continuous.

Bullet Script
The Bullet.cs script is attached to the bullet asset. On collision with other objects in the scene, the script instantiates bullet holes. The bullet hole object appears as follows:
![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Forward%20Scenario/Bullet_Prefab.jpg)

, inside the script we instantiate bullet holes on collision to other existing objects in the scene. The bullet hole object:

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Forward%20Scenario/BulletHole.png). 

Shooting Script
The ShootingR.cs script visualizes the bullet trajectory using raycasting. This script is attached to a Glock G22 pistol asset, which includes a Sprite Renderer and a Line Renderer.

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Forward%20Scenario/Sprite%26LineRenderer.jpg)


![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Forward%20Scenario/ShootingR.cs__Inspector.jpg)


Configuration
Bullet: The bullet object with the Bullet.cs script attached.
Cursor: The target showing the bullet hole position on shooting.
Shoot Point: The transform position from which the gun shoots the bullet.
Line Visual: The Line Renderer of the Glock G22.
Line Segment: Set to 10.
Flight Time: Set to 1.
Bullet Hole Prefab: Set to the bullet hole object.
Play Mode
In play mode, moving the cursor shows the possible trajectory. Shooting visualizes the trajectory, and intersections of multiple trajectories indicate the shooter’s position.

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Forward%20Scenario/Csi.gif)

Modifications
We followed a tutorial (Romi Fauzi) with significant changes:

Removed gravity from the bullet.
Changed the trajectory equation to a straight line.
# Equations of Motion
The simplified equations of motion, considering no gravity or air resistance, are:

\[ V_{xz} = \frac{xz(t)}{t} \]

\[ V_y = \frac{y(t)}{t} \]

where \( xz(t) \) and \( y(t) \) are the distances traveled in the \( xz \) and \( y \) directions, respectively.


# Backward Scenario: Shooter Position Detection
The backward scenario aims to locate the possible shooter position based on bullet hole images and/or cartridges in the scene.

Ground Truth Images
65° Azimuth Angle: 

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/65.png)

25° Azimuth Angle 

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/25.png)

These images are imported as 2D sprites in Unity assets and placed in the crime scene.

 We set the *SurfaceVector.cs* script on our **primary** bullethole object, **meaning the one bullet hole that will definitely exist on our scene** and the *Info.cs* in case we have an Extra bullet hole in the scene. On the inspector for the primary bullet, the following information will be required after we attach the *SurfaceVector.cs* script to the object

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/SurfaceVector.cs__Inspector.jpg)

Azimuth Angle, Rotation, and Directionality: Extracted from the JSON file after bullet detection.
Other Bullet Hole Transform: Transform of the secondary bullet hole.
Cone Visualization: A cone created in Blender for visualization.

Play Mode
In play mode, the surface vectors of the bullet holes appear, indicating potential intersections. Enable Gizmos to view these in the scene.

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/CrimeScene.jpg)

Replace vectors with cones to add uncertainty, defined by a threshold parameter.

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/CrimeSceneCones.jpg)

If we have a second (extra) bullet hole, we have attached the *Info.cs* script, we similarly include azimuth angle, rotation and directionality  these information is extracted from the JSON file, after we have applied the bullet detection algorithm.
(There is a new direction parameter that indicates the direction of the surface vector of the extra bullet hole based on the attributes we have inserted. The code on the *Info.cs* script is in the Awake function, as we wish to run and get the new direction of our surface vector before we look for intersection from the  *SurfaceVector.cs*  attached on the primary bullet hole. The *Info.cs* script looks like :

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/Info.cs_Inspector.jpg)

Cartridges
If no extra bullet holes are present, input the cartridge transform to look for intersections,
we include a serialized field where we input the transform of the cartridges and we look for intersection
 
![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/CrimeSceneCones_Cartridges.jpg)

We can have more than one blender cone as input for different visualization and we can serialize the threshold parameter for intersection

Intersection Code
The code checks for vector intersections using geometric analysis, as described in this Stack Overflow discussion.
https://stackoverflow.com/questions/59449628/check-when-two-vector3-lines-intersect-unity3d Intersection code


Virtual Analysis
The process transfers real trajectory rods used by LEAs to the virtual analysis world, applying geometric analysis algorithms.
![image](https://github.com/user-attachments/assets/ec5dd8f4-69d3-4c48-9aeb-8f918b070ddf)
This implementation allows us to accurately simulate bullet trajectories and deduce shooter positions in a crime scene.

