import json
import os
from GroundTruthConversion import start_the_conversion

def get_Image_index(name):
    names = name.split('_')
    ind = names[6].split('.')[0]
    return int(ind)


directory = "Projects/Python97/GroundTruthImages/"
names = os.listdir(directory)
finalAnnotations = []

for val in names:
    ind = (get_Image_index(val))
    annotationInd = ind - 1
    imagePath = directory + val
    name = "mask_{}".format(ind)
    print("name: {} path: {}".format(name, imagePath))
    finalAnnotations.insert(annotationInd, start_the_conversion(imagePath, ind))

with open('GroundTruthVersion2.json', 'w') as f:
    json.dumps(finalAnnotations, f, ensure_ascii=False)