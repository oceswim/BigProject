import json
import os
from groundTruthHandling.GroundTruthConversion import start_the_conversion
import shutil
import streamlit as st


def get_image_index(name):
    names = name.split('_')
    ind = names[6].split('.')[0]
    return int(ind)

@st.cache
def convert_ground_truth(theDir,projectName):
    di = ("{}GroundTruthImages/".format(theDir))
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

