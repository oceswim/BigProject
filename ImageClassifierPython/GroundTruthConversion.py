import json
from PIL import Image
from SubMaskAnnotation import create_sub_mask_annotation
from SubMaskCreator import create_sub_masks

def detect_class(class_id):
    if class_id == 1:
        class_n = "Airbus"
    elif class_id == 2:
        class_n = "Bicycle"
    elif class_id == 3:
        class_n = "Bus"
    elif class_id == 4:
        class_n = "Car"
    elif class_id == 5:
        class_n = "Jet"
    elif class_id == 6:
        class_n = "Motor"
    else:
        class_n = "Not an object"
    return class_n


def start_the_conversion(image_path, img_id):
    mask_image = Image.open(image_path)
    # Define which colors match which categories in the images
    airbus_id, bicycle_id, bus_id, car_id, jet_id, motor_id, background_id, floor_id = [1, 2, 3, 4, 5, 6, 7, 8]
    category_ids = {
        '(4, 221, 137)': airbus_id,
        '(164, 92, 3)': bicycle_id,
        '(160, 80, 211)': bus_id,
        '(48, 81, 88)': car_id,
        '(52, 205, 154)': jet_id,
        '(148, 192, 194)': motor_id,
        '(128, 128, 128)': background_id,
        '(0, 200, 144)': floor_id
    }

    is_crowd = 0
    # These ids will be automatically increased as we go
    annotation_id = 1
    image_id = img_id
    # Create the annotations
    annotations = []

    sub_masks = create_sub_masks(mask_image)
    for color, sub_mask in sub_masks.items():
        # if color != '(0, 200, 144)' and color != '(128, 128, 128)':
        category_id = category_ids[color]
        # for the color value
        color_value = color
        # for the class
        class_name = detect_class(category_id)

        annotation = create_sub_mask_annotation(sub_mask, image_id, category_id, annotation_id,
                                                is_crowd, color_value, class_name)
        annotations.append(annotation)
        annotation_id += 1

    print(json.dumps(annotations))
    return annotations
