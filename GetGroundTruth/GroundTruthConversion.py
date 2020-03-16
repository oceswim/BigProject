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


def detect_super_category(id):
    super_cat_id = 0
    super_cat_name = ""
    if 1 <= id <= 4:
        super_cat_id = 1
        super_cat_name = "Car"
    elif 5 <= id <= 8:
        super_cat_id = 2
        super_cat_name = "Van"
    elif 9 <= id <= 12:
        super_cat_id = 3
        super_cat_name = "Sports Car"
    elif 13 <= id <= 16:
        super_cat_id = 4
        super_cat_name = "Bus"
    elif 17 <= id <= 20:
        super_cat_id = 5
        super_cat_name = "Small aircraft"
    elif 21 <= id <= 24:
        super_cat_id = 6
        super_cat_name = "Jet"
    elif 25 <= id <= 28:
        super_cat_id = 7
        super_cat_name = "Helicopter"
    elif 29 <= id <= 32:
        super_cat_id = 8
        super_cat_name = "Larger aircraft"
    elif id >= 33:
        super_cat_id = 9
        super_cat_name = "Background"

    return super_cat_id, super_cat_name


# car van sports car bus small aircraft jet  helicopter larger aircraft

def start_the_conversion(image_path, img_id):
    mask_image = Image.open(image_path)
    # Define which colors match which categories in the images
    audi_id, bmwz4_id, mini_id, golf_id, \
    ambulance_id, kangoo_id, minivan_id, combi_id, \
    dodge_id, ferrari_id, mcLaren_id, prosche_id, \
    city_bus_id, london_bus_id, school_bus_id, tourist_bus_id, \
    breguet_id, cessnal_id, hawker_id, learjet_id, \
    b2_spirit_id, eurofighter_id, f35_id, mig29_id, \
    ah64d_id, huey_id, ka50_id, mi24_id, \
    a380_id, a757_id, b747_id, concord_id, \
    floor_id,background_id = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
                     28, 29, 30, 31, 32, 33,34]
    category_ids = {
        '(4, 221, 137)': audi_id,
        '(178, 88, 154)': bmwz4_id,
        '(164, 92, 3)': mini_id,
        '(48, 81, 88)': golf_id,
        '(52, 205, 154)': ambulance_id,
        '(148, 192, 194)': kangoo_id,
        '(148, 129, 73)': minivan_id,
        '(16, 204, 144)': combi_id,
        '(16, 13, 27)': dodge_id,
        '(180, 0, 209)': ferrari_id,
        '(48, 29, 73)': mcLaren_id,
        '(144, 128, 27)': prosche_id,
        '(20, 140, 209)': city_bus_id,
        '(144, 129, 129)': london_bus_id,
        '(20, 13, 90)': school_bus_id,
        '(176, 0, 10)': tourist_bus_id,
        '(150, 25, 65)': breguet_id,
        '(18, 212, 0)': cessnal_id,
        '(150, 24, 203)': hawker_id,
        '(18, 21, 153)': learjet_id,
        '(178, 128, 19)': b2_spirit_id,
        '(54, 13, 82)': eurofighter_id,
        '(54, 140, 216)': f35_id,
        '(178, 129, 136)': mig29_id,
        '(150, 0, 2)': ah64d_id,
        '(54, 157, 25)': huey_id,
        '(178, 145, 203)': ka50_id,
        '(54, 220, 146)': mi24_id,
        '(180, 11, 202)': a380_id,
        '(18, 71, 218)': a757_id,
        '(48, 135, 1)': b747_id,
        '(150, 75, 10)': concord_id,
        '(218, 23, 74)': floor_id,
        '(128, 128, 128)': background_id,

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
        print("color: {}".format(color))
        category_id = category_ids[color]
        # for the color value
        color_value = color
        # for the class
        class_name = detect_class(category_id)
        super_category_id, super_class_name = detect_super_category(category_id)

        annotation = create_sub_mask_annotation(sub_mask, image_id, category_id, class_name, super_category_id,
                                                super_class_name, annotation_id,
                                                is_crowd, color_value)
        annotations.append(annotation)
        annotation_id += 1

    return annotations
