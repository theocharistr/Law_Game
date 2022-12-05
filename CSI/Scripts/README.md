# Bullet-hole-Detection
![alt text]( https://github.com/theocharistr/Bullet-hole-Detection/blob/master/BulletDetection1image/bullet%20hole%20car%20198.png)


Preprocess the image, Convert to gray, blur, (apply threshold if needed), canny, dilate and finally erosion

Find Contours, find contours with  lenght size >5 ( requirement for ellipse),  keep only the child contours (and not parent contour, meaning external)

Check if the polygon convex created from the Json annotation file contains the center of ellipse and if the contour dectected< contour exist

Check if the dimensions (width, lenght) are not zero save in Json file the ellipse detected number, width, lenght,rotation of ellipse, impact angle and azimuth 

Crop the detected ellipse, apply threshold ,keep the contour with the bigger area and check if its center is left or right from the cropped ellipse

This way we define the directionality of the bullet which we save in Json file

![alt text](https://github.com/theocharistr/Bullet-hole-Detection/blob/master/BulletDetectionAllimages/directionality.jpg)

Thus we have the azimuth angle and we define the elevation angle based the rotation and the directionality of the bullet, and we can create a surface vector to locate potential position of the shooter 

![alt text](https://github.com/theocharistr/Bullet-hole-Detection/blob/master/BulletDetection1image/Detected_bullet%20hole%20car%20198.jpg)
