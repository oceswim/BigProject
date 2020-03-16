import json
import os
import platform

from GroundTruthConversion import start_the_conversion
import shutil


def get_image_index(name):
    names = name.split('_')
    ind = names[6].split('.')[0]
    return int(ind)


def convert_ground_truth(theDir,projectName):
    slash = "/"
    if platform.system() == "Windows":
        slash = "\""
    di = ("{}GroundTruthImages{}".format(theDir,slash))
    names = os.listdir(di)
    finalAnnotations = []
    jsonName = projectName+'.json'
    for val in names:
        if val != '.DS_Store':
            ind = (get_image_index(val))
            annotationInd = ind - 1
            imagePath = di + val
            name = "mask_{}".format(ind)
            print("name: {} path: {}".format(name, imagePath))
            finalAnnotations.insert(annotationInd, start_the_conversion(imagePath, ind))
    with open(jsonName, 'w') as f:
        json.dump(finalAnnotations, f, ensure_ascii=False)

    shutil.move(jsonName, theDir)


convert_ground_truth("/Users/Oceswil83/Desktop/p1/","p1")