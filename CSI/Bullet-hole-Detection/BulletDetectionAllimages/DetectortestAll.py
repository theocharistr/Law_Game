import cv2
import json
import shapely.geometry as sg
from shapely.geometry import Point
import math
import numpy
import collections
import os
from tqdm import tqdm
from PIL import Image, ImageOps



Data_Outputs = {
    "Ellipse Detected Number": [],
    "Width of Ellipse": [],
    "Length of Ellipse": [],
    "Rotation of Ellipse": [],
    "ImpactAngle": [],
    "impactAngleAzimuth": [],
    "Directionality": []
}
data_dir = 'images/'
annotation_dir = 'annotation/'
save_dir = 'outputs'
save_dir_JSON = 'outputsJSON'

os.makedirs(save_dir, exist_ok=True)
os.makedirs(save_dir_JSON, exist_ok=True)
files = os.listdir(annotation_dir)
files.sort()

for file in tqdm(files):
    f = open(os.path.join(annotation_dir, '', file))
    data = json.load(f)
    img_name = data['imagePath']
    image = cv2.imread('images/' + img_name)

    shapes = data["shapes"]
    counterExist = len(data["shapes"])

    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    blur = cv2.GaussianBlur(gray, (13, 13), 0)
    thresh = cv2.threshold(blur, 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)[1]

    canny = cv2.Canny(blur, 120, 255, 1) #blur or thresh

    # Dilate with elliptical shaped kernel
    kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (3, 3))
    dilate = cv2.dilate(canny, kernel, iterations=3)
    erosion = cv2.erode(dilate, kernel, iterations=3)

    # Find contours, filter using contour threshold area, draw ellipse
    ret, thresh50 = cv2.threshold(blur, 50, 255, cv2.THRESH_BINARY)

    # cv2.imshow('thresh50', thresh50)
    # cv2.waitKey()
    cnts = cv2.findContours(erosion, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
    counterDetected = 0

    hierarchy = cnts[1]
    cnts = cnts[0] if len(cnts) == 2 else cnts[1]
    # print('hierarchy',hierarchy)
    for count, c in enumerate(cnts):
        if len(c) >= 5 and hierarchy[0][count][2] == -1:

            B = cv2.contourArea(c)
            ellipse = cv2.fitEllipse(c)
            # Gets width and height of ellipse
            CoE = Point([ellipse[0]])  # Center of Ellipse
            for i in range(len(data["shapes"])):
                polygon = sg.MultiPoint(shapes[i]['points']).convex_hull
                if (polygon.contains(CoE) and counterDetected < counterExist):
                    cv2.ellipse(image, ellipse, (36, 255, 12), 2)
                    # Gets width and height of ellipse
                    widthE = ellipse[1][0]
                    lengthE = ellipse[1][1]
                    # Center of Ellipse
                    CoE = Point([ellipse[0]])
                    centerX = ellipse[0][0]
                    centerY = ellipse[0][1]
                    # Gets rotation of ellipse; same as rotation of contour
                    rotation = ellipse[2]-90

                    counterLeft = 0
                    counterRight = 0
                    if lengthE != 0 and widthE != 0:
                        print("counterDetected Number:", counterDetected)
                        print(lengthE, ": Length of Ellipse,", widthE, ' : Width of Ellipse,', rotation,
                              ' : rotation of Ellipse')
                        ImpactAngle = (widthE / lengthE)
                        area = cv2.contourArea(c)
                        cv2.putText(image, str(counterDetected), (int(centerX), int(centerY)), cv2.FONT_HERSHEY_SIMPLEX, 0.9,
                                    (255, 60, 255), 2)
                        print(ImpactAngle, ": ImpactAngle,")
                        impactAngleAzimuth = math.degrees(math.asin(ImpactAngle))
                        impactAngleVertical = math.degrees(math.acos(ImpactAngle))
                        Data_Outputs['Ellipse Detected Number'].append(counterDetected)
                        Data_Outputs['Width of Ellipse'].append(widthE)
                        Data_Outputs['Length of Ellipse'].append(lengthE)
                        Data_Outputs['Rotation of Ellipse'].append(rotation)
                        Data_Outputs['ImpactAngle'].append(ImpactAngle)
                        Data_Outputs['impactAngleAzimuth'].append(impactAngleAzimuth)

                        print(impactAngleAzimuth, " : impactAngleAzimuth, ", )
                        counterDetected += 1
                        # print("Center x :", centerX, "Center y:", centerY, "")
                        # print(int(centerX - lengthE / 2), ",", int(centerX + lengthE / 2), "and", int(centerY - widthE / 2), ",",
                        #     int(centerY + widthE / 2), "vs", image.shape[1], "and", image.shape[0])
                        ret, thresh15 = cv2.threshold(blur, 50, 255, cv2.THRESH_BINARY )

                        croppedEllipse = thresh15[int(centerY - widthE / 2):int(centerY + widthE / 2),
                                         int(centerX - lengthE / 2):int(centerX + lengthE / 2)]
                        croppedEllipseGray = gray[int(centerY - widthE / 2):int(centerY + widthE / 2),
                                             int(centerX - lengthE / 2):int(centerX + lengthE / 2)]
                        contours1, hierarchy1 = cv2.findContours(croppedEllipse, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
                        cv2.drawContours(croppedEllipse, contours1, -1, (255, 255, 255), 3)
                        counter2 = 0

                        cv2.imshow('Gray croppedEllipse', croppedEllipseGray)
                        cv2.imshow('Thresh croppedEllipse', croppedEllipse)
                        #cv2.waitKey()

                        maxAreaThresholdedContour = 0
                        for count, c in enumerate(contours1):
                            if len(c) >= 5:
                                # compute the center of the contour
                                M = cv2.moments(c)
                                area = cv2.contourArea(c)

                                if (M["m00"] != 0) and area > maxAreaThresholdedContour:
                                    cX = int(M["m10"] / M["m00"])
                                    cY = int(M["m01"] / M["m00"])
                                    # cv2.putText(croppedEllipse, str(counter2), (cX, cY), cv2.FONT_HERSHEY_SIMPLEX, 0.9,
                                    #             (0, 0, 255), 2)
                                    counter2 += 1
                        # cv2.imshow('croppedEllipse', croppedEllipse)
                        # print("cx", cX, "cy", cY)
                        if (cX < croppedEllipse.shape[0] / 2):
                            print("Came from right")
                            directionality = 'right'
                        else:
                            print("Came from left")
                            directionality = 'left'
                        Data_Outputs['Directionality'].append(directionality)

    cv2.imshow('gray', gray)
    cv2.imshow('blur', blur)
    cv2.imshow('canny', canny)
    cv2.imshow('dilate', dilate)
    cv2.imshow('erosion', erosion)
    cv2.imshow('image with contours', image)
    # cv2.waitKey()
    cv2.destroyAllWindows()

    cv2.imwrite(os.path.join(save_dir + '/Detected_' + img_name.split('.')[0] + '.jpg'), image)
    # Serializing json
    json_object = json.dumps(Data_Outputs, indent=4)
    # Writing to sample.json
    filename = os.path.join(save_dir_JSON + '/Detected_' + img_name.split('.')[0] + '.json')
    # Writing to sample.json
    with open(filename, "w") as outfile:
        outfile.write(json_object)

