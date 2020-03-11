import argparse

import yolo_code
from PIL import Image


def detect_img(imagesPath, modelPath):
    theImages = []
    jsonP=""#please give whole path here
    for img in imagesPath:
        image = Image.open(img)
        r_image = yolo_code.set_yolo_param(img,image,modelPath,jsonP)
        theImages.append(r_image)
        yolo_code.close_session()
    return theImages

