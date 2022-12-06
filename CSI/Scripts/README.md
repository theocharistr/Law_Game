 
# Forward Scenario
The purpose of the forward scenario is to simulate the bullet trajectory in different position of the shooter. We attach the 
*Bullet.cs* to an asset of a bullet we have created (a sphere with small dimensions,and gold color), inside the script we instantiate bullet holes on collision to other existing objects in the scene. The bullet hole object:

![alt text]([BulletHole.png](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Forward%20Scenario/BulletHole.png)). 

We also have *ShootingR.cs* which using raycast visualizes the bullet trajectory, we attach this script to a Glock G22( a pistol asset we download from Unity assets)
we add a sprite renderer and line renderer to the item.

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Forward%20Scenario/Sprite%26LineRenderer.jpg)


![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Forward%20Scenario/ShootingR.cs__Inspector.jpg)


 For the *ShootingR.cs*, we require information such as the Bullet object we have created and attached the script, the Cursor which we set as Target and it shows the position of where the bullet hole will be created if we shoot (right click on play mode), the Shoot Point where we input the Shoot Point transform, which we should manually set to the transform position (of the prefab) from where the gun shoots the bullet. For the Line visual we select the Line Renderer of the Glock G22 we have just created. Line Segement we set to 10 and flight time to 1. For the bullet hole prefab we set it as the target (basically the bullet hole object)
When we go in play mode, we can move the cursor around and see the possible trajectory, if we shoot the trajectory is visualized in the scene and if we have more than one shooting we can suspect that the position of the shooter is the intersection of the cones 

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Forward%20Scenario/Csi.gif)

# Backward Scenario
The purpose of the backward scenario is to locate the possible position of the shooter based on bullet hole image(s)and/or cartridges on the scene.
We add the images of  ground truth of 65 azimuth angle 

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/65.png)

and ground truth of 25 azimuth angle 

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/25.png)

and select the images as 2D Sprite in Unity assets and we can insert them inside the crime scene. We set the *SurfaceVector.cs* script on our **primary** bullethole object, **meaning the one bullet hole that will definitely exist on our scene** and the *Info.cs* in case we have an Extra bullet hole in the scene. On the inspector for the primary bullet, the following information will be required after we attach the *SurfaceVector.cs* script to the object

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/SurfaceVector.cs__Inspector.jpg)

we include azimuth angle, rotation and directionality these information are extracted from the JSON file after we have applied the bullet detection algorithm. We also input the transform of the other bullet hole we want to detect if there is an intersection in this case 25 transform. We also input the cone we want to use for visualization, we have created the cone using blender.
If we select play mode, the surface vectors of the bullet holes (Debug draw lines) will appear, in case it is not we need to enable Gizmos when we switch to scene.

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/CrimeScene.jpg)

We can see if the surface vectors of the bullet holes intersect in the scene and we can replace them with cones to add some uncertainty, in the code the uncertainty is defined with the threshold parameter

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/CrimeSceneCones.jpg)

If we have a second (extra) bullet hole, we have attached the *Info.cs* script, we similarly include azimuth angle, rotation and directionality  these information is extracted from the JSON file, after we have applied the bullet detection algorithm.
(There is a new direction parameter that indicates the direction of the surface vector of the extra bullet hole based on the attributes we have inserted. The code on the *Info.cs* script is in the Awake function, as we wish to run and get the new direction of our surface vector before we look for intersection from the  *SurfaceVector.cs*  attached on the primary bullet hole. The *Info.cs* script looks like :

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/Info.cs_Inspector.jpg)

 In case we don't have another bullet hole, but we have cartridges, we include a serialized field where we input the transform of the cartridges and we look for intersection
 
![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/CrimeSceneCones_Cartridges.jpg)

We can have more than one blender cone as input for different visualization and we can serialize the threshold parameter for intersection
