 
# Forward Scenario
# Backward Scenario
We add the images of 

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/65.png)

and

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/25.png)

and select the images as 2D Sprite in Unity assets and we can insert them inside the crime scene. We set the SurfaceVector.cs script on our primary bullethole, meaning the one bullet hole that will definitely exist on our scene and the Info.cs in case we have an Extra bullet hole in the scene. On the inspector for the primary bullet, the following information will be required after we attach the script to the object

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/65_inspector.jpg)

we include azimuth angle, rotation and directionality these information are extracted from the JSON file after we have applied the bullet detection algorithm. We also input the transform of the other bullet hole we want to detect if there is an intersection in this case 25 transform. We also input the cone we want to use for visualization, we have created the cone using blender.

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/CrimeScene.jpg)

We can see if the surface vectors of the bullet holes intersect in the scene and we can replace them with cones to add some uncertainty, in the code the uncertainty is defined with the threshold parameter

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/CrimeSceneCones.jpg)

If we have a second (extra) bullet hole, we have attached the Info.cs script, we similarly include azimuth angle, rotation and directionality  these information is extracted from the JSON file, after we have applied the bullet detection algorithm.
(There is a new direction parameter that indicates the direction of the surface vector of the extra bullet hole based on the attributes we have inserted. The code on the Info.cs script is in the Awake function, as we wish to run and get the new direction of our surface vector before we look for intersection from the SurfaceVector.cs attached on the primary bullet hole.

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/25_inspector.jpg)

 In case we don't have another bullet hole, but we have cartridges, we include a serialized field where we input the transform of the cartridges and we look for intersection
 
![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Scripts/Backward%20Scenario/CrimeSceneCones_Cartridges.jpg)

We can have more than one blender cone as input for different visualization and we can serialize the threshold parameter for intersection
