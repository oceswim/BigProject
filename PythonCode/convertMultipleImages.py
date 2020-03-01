import json
import os
import PIL
from forBigProject.groundTruthConversion import start_the_conversion

directory = "/Users/Oceswil83/Desktop/COURSFAC/BigProject/FristTry/Images/Python1/GroundTruthImages/"
aList = os.listdir(directory)
number_files = len(aList)
print(number_files)

finalAnnotations = []
for i in range(0, number_files):
    imageName = "Img{}.png".format(i + 1)
    imagePath = directory + imageName
    # plant_book_mask_image = PIL.Image.open('/path/to/images/plant_book_mask.png')
    name = "mask_{}".format(i)
    print("name: {} path: {}".format(name, imagePath))
    image_id = i+1
    finalAnnotations.append(start_the_conversion(imagePath, image_id))

with open('JSON.txt', 'w') as f:
    json.dump(finalAnnotations, f, ensure_ascii=False)