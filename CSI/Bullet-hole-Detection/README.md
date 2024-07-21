# Bullet hole Detection

**BulletDetection1image/Detectortest.py** processes a single image and requires a related JSON file that contains the mask for the bullet hole.

**BulletDetection1image/DetectortestwithoutJSON.py** processes a single image but does not require a JSON file for the mask of the bullet hole.

**BulletDetectionAllimages/DetectortestAll.py** processes all images in the directory, utilizing their respective JSON files to detect bullet holes.



![alt text]( https://github.com/theocharistr/Bullet-hole-Detection/blob/master/BulletDetection1image/bullet%20hole%20car%20198.png)


Here is a formal description of the image processing and contour analysis procedure:

---

1. **Preprocess the Image:**
   - Convert the image to grayscale.
   - Apply a blur to reduce noise.
   - If necessary, apply a threshold to segment the image.
   - Use Canny edge detection to identify edges.
   - Perform dilation to enhance the edges.
   - Apply erosion to reduce noise and refine the contours.

2. **Find Contours:**
   - Detect contours in the image.
   - Filter contours to keep only those with a length greater than 5, which is required for ellipse fitting.
   - Retain only child contours, excluding parent (external) contours.

3. **Ellipse and Polygon Check:**
   - Check if the ellipse, created from the detected contours, is contained within the polygon defined by the JSON annotation file.
   - Verify that the contour exists and is valid.

4. **Ellipse Dimensions Validation:**
   - Ensure that the dimensions (width and length) of the ellipse are not zero.
   - Save the following details in the JSON file: ellipse number, width, length, rotation, impact angle, and azimuth.

5. **Crop and Process Detected Ellipse:**
   - Crop the area containing the detected ellipse.
   - Apply a threshold to the cropped image.
   - Perform a closing operation (dilation followed by erosion) to close small gaps and holes in the binary image.

6. **Contour Area Analysis:**
   - Identify and retain the contour with the largest area.
   - Determine if the center of this contour is located to the left or right of the cropped ellipse.

7. **Define Bullet Directionality:**
   - Based on the position of the largest contour relative to the cropped ellipse, define the directionality of the bullet.
   - Save the directionality information in the JSON file.

---

![alt text](https://github.com/theocharistr/Bullet-hole-Detection/blob/master/BulletDetectionAllimages/directionality.jpg)

Based on the detected ellipse, we calculate the azimuth angle directly from the provided data. 
Additionally, the elevation angle is determined using the rotation and directionality of the bullet.

With these angles established, we can construct a surface vector to estimate the potential position of the shooter.
![alt text](https://github.com/theocharistr/Bullet-hole-Detection/blob/master/BulletDetection1image/Detected_bullet%20hole%20car%20198.jpg)

Two more examples for

![alt text](https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/25.png)

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/Detected_25.jpg)

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/65.png)

![alt text]( https://github.com/theocharistr/Law_Game/blob/main/CSI/Bullet-hole-Detection/BulletDetection1image/Detected_65.jpg)

When multiple bullet holes are detected, each bullet hole is saved as an entry in a list within the JSON file,
along with its corresponding contour detection data.

