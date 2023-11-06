# Bullet-hole-Detection
BulletDetection1image/Detectortest.py use it while having as input one image and the JSON file for the mask of the bullethole
BulletDetection1image/DetectortestwithouJSON.py use it while having as input one image and without JSON file for the mask of the bullethole
BulletDetectionAllimages/DetectortestAll.py uses it while having all images and all their respective JSON files

![alt text]( https://github.com/theocharistr/Bullet-hole-Detection/blob/master/BulletDetection1image/bullet%20hole%20car%20198.png)


Preprocess the image, Convert to gray, blur, (apply threshold if needed), canny, dilate, and finally erosion

Find Contours, find contours with  length size >5 (a requirement for the ellipse),  and keep only the child contours (and not the parent contour, meaning external)

Check if the polygon convex created from the Json annotation file contains the center of the ellipse and if the contour dectected< contour exists

Check if the dimensions (width, length) are not zero save in JSON file the ellipse detected number, width, length, rotation of ellipse, impact angle, and azimuth 

Crop the detected ellipse, apply threshold, and Closing (dilation followed by erosion is typically used to close small gaps and holes within the background of a binary image).

Keep the contour with the bigger area and check if its center is left or right from the cropped ellipse

This way we define the directionality of the bullet which we save in JSON file

![alt text](https://github.com/theocharistr/Bullet-hole-Detection/blob/master/BulletDetectionAllimages/directionality.jpg)

Thus we have the azimuth angle and we define the elevation angle based on the rotation and the directionality of the bullet, and we can create a surface vector to locate the potential position of the shooter 

![alt text](https://github.com/theocharistr/Bullet-hole-Detection/blob/master/BulletDetection1image/Detected_bullet%20hole%20car%20198.jpg)

Two more examples for

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/25.png)

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/Detected_25.jpg)

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/65.png)

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/Detected_65.jpg)

If we have more than one bullet hole, they are saved as a list in the JSON file, with their corresponding contour detection.
