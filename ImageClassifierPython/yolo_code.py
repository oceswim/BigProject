import colorsys
from timeit import default_timer as timer
import cv2
import numpy as np
from keras import backend as K
from keras.models import load_model
from keras.layers import Input
from PIL import Image, ImageFont, ImageDraw
import json
from yolo_model import yolo_eval, yolo_body, tiny_yolo_body
from yolo_utils import letterbox_image
import os
from keras.utils import multi_gpu_model
import streamlit as st
from CalculateEfficiency import get_boxes


def generate_training_txt(path, jsonPath):
    imagesPaths = os.listdir(path)
    actualPaths = []
    indexes = []
    lines = []
    for i in imagesPaths:
        if i != ".DS_Store":
            actualPaths.append(path + "/" + i)
            indexes.append(get_Image_index(i))
    with open(jsonPath) as json_file:
        data = json.load(json_file)
        for p in data:
            singleLine = []
            index = int(p[0]['image_id']) - 1
            singleLine.append(actualPaths[index])
            for x in p:
                if x['object class'] != "Not an object":
                    for i in x['bbox']:
                        toInt = int(i)
                        toString = str(toInt)
                        singleLine.append(toString)
                    idToString = str(int(x['category_id']))
                    singleLine.append(idToString)
            lines.append(singleLine)
    filePath = "yolo_folder/annot_txt.txt"
    with open(filePath, 'w') as file:
        for s in lines:
            for x in range(0,len(s)):
                file.write(s[x])
                if x == 0 or x % 5 == 0 and x < len(s)-1:
                    file.write(' ')
                elif 0 < x < len(s)-1:
                    file.write(',')
            file.write('\n')


def read_img(img):
    the_bytes = np.asarray(bytearray(img.read()), dtype=np.uint8)
    theImage = cv2.imdecode(the_bytes, 1)
    return theImage


def get_Image_index(name):
    names = name.split('_')
    ind = names[6].split('.')[0]
    return int(ind)


@st.cache
def process_json(predBoundBox, path, imageName):
    index = get_Image_index(imageName)
    groundTruthBoxes = []
    with open(path) as json_file:
        data = json.load(json_file)
        for p in data:
            if p[0]['image_id'] == index:
                for x in p:
                    if x['object class'] != "Not an object":
                        groundTruthBoxes.append(x['bbox'])
    for s in groundTruthBoxes:
        s[2] = s[0] + s[2]
        s[3] = s[1] + s[3]

    print(len(predBoundBox))
    print(len(groundTruthBoxes))
    return get_boxes(predBoundBox, groundTruthBoxes)


def set_yolo_param(imgP,image, model,jsonP):  # model is model path
    model_image_size = (416, 416)
    class_names = _get_class('yolo_folder/txt_files/classes.txt')
    anchors = _get_anchors('yolo_folder/txt_files/yolo-anchors.txt')
    sess = K.get_session()
    boxes, scores, classes, colors, mod, shape = generate(model, anchors, class_names)

    return detect_image(imgP,image, model_image_size, mod, shape, boxes, scores, classes,
                        class_names,
                        sess, colors,jsonP)


def _get_class(classes):
    classes_path = os.path.expanduser(classes)
    with open(classes_path) as f:
        classesName = f.readlines()
    class_final = [c.strip() for c in classesName]
    return class_final


def _get_anchors(anchors):
    anchors_path = os.path.expanduser(anchors)
    with open(anchors_path) as f:
        anchor = f.readline()
    anchorsFinal = [float(x) for x in anchor.split(',')]
    return np.array(anchorsFinal).reshape(-1, 2)


def generate(model, anch, cls, gpu_num=1, score=.3, iou=.45):
    model_path = os.path.expanduser(model)
    assert model_path.endswith('.h5'), 'Keras model or weights must be a .h5 file.'

    # Load model, or construct model and load weights.
    num_anchors = len(anch)
    num_classes = len(cls)
    is_tiny_version = num_anchors == 6  # default setting
    try:
        yolo_model = load_model(model_path, compile=False)
    except:
        yolo_model = tiny_yolo_body(Input(shape=(None, None, 3)), num_anchors // 2, num_classes) \
            if is_tiny_version else yolo_body(Input(shape=(None, None, 3)), num_anchors // 3, num_classes)
        yolo_model.load_weights(model_path)  # make sure model, anchors and classes match
    else:
        assert yolo_model.layers[-1].output_shape[-1] == \
               num_anchors / len(yolo_model.output) * (num_classes + 5), \
            'Mismatch between model and given anchor and class sizes'

    print('{} model, anchors, and classes loaded.'.format(model_path))

    # Generate colors for drawing bounding boxes.
    hsv_tuples = [(x / len(cls), 1., 1.)
                  for x in range(len(cls))]
    colors = list(map(lambda x: colorsys.hsv_to_rgb(*x), hsv_tuples))
    colors = list(
        map(lambda x: (int(x[0] * 255), int(x[1] * 255), int(x[2] * 255)),
            colors))
    np.random.seed(10101)  # Fixed seed for consistent colors across runs.
    np.random.shuffle(colors)  # Shuffle colors to decorrelate adjacent classes.
    np.random.seed(None)  # Reset seed to default.

    # Generate output tensor targets for filtered bounding boxes.
    input_image_shape = K.placeholder(shape=(2,))
    if gpu_num >= 2:
        yolo_model = multi_gpu_model(yolo_model, gpus=gpu_num)
    boxes, scores, classes = yolo_eval(yolo_model.output, anch,
                                       len(cls), input_image_shape,
                                       score_threshold=score, iou_threshold=iou)
    return boxes, scores, classes, colors, yolo_model, input_image_shape


def detect_image(pathImg,image, size, model, shape, boxes, scores, classes, class_names,
                 sess, colors,jsonP):
    start = timer()

    if size != (None, None):
        assert size[0] % 32 == 0, 'Multiples of 32 required'
        assert size[1] % 32 == 0, 'Multiples of 32 required'
        boxed_image = letterbox_image(image, tuple(reversed(size)))
    else:
        new_image_size = (image.width - (image.width % 32),
                          image.height - (image.height % 32))
        boxed_image = letterbox_image(image, new_image_size)
    image_data = np.array(boxed_image, dtype='float32')

    print(image_data.shape)
    image_data /= 255.
    image_data = np.expand_dims(image_data, 0)  # Add batch dimension.

    out_boxes, out_scores, out_classes = sess.run(
        [boxes, scores, classes],
        feed_dict={
            model.input: image_data,
            shape: [image.size[1], image.size[0]],
            K.learning_phase(): 0
        })

    print('Found {} boxes for {}'.format(len(out_boxes), 'img'))

    font = ImageFont.truetype(font='yolo_folder/font/FiraMono-Medium.otf',
                              size=np.floor(3e-2 * image.size[1] + 0.5).astype('int32'))
    thickness = (image.size[0] + image.size[1]) // 300

    names = []
    predBboxes = []
    for i, c in reversed(list(enumerate(out_classes))):
        predicted_class = class_names[c]
        box = out_boxes[i]
        score = out_scores[i]

        label = '{} {:.2f}'.format(predicted_class, score)
        draw = ImageDraw.Draw(image)
        label_size = draw.textsize(label, font)

        top, left, bottom, right = box
        top = max(0, np.floor(top + 0.5).astype('int32'))
        left = max(0, np.floor(left + 0.5).astype('int32'))
        bottom = min(image.size[1], np.floor(bottom + 0.5).astype('int32'))
        right = min(image.size[0], np.floor(right + 0.5).astype('int32'))
        print(label, (left, top),image.size[0], (right, bottom),image.size[1])

        if top - label_size[1] >= 0:
            text_origin = np.array([left, top - label_size[1]])
        else:
            text_origin = np.array([left, top + 1])
        if predicted_class not in names:
            newBbox = []
            newBbox.append(float(left))
            newBbox.append(float(top))
            newBbox.append(float(right))
            newBbox.append(float(bottom))
            predBboxes.append(newBbox)
            names.append(predicted_class)
        # My kingdom for a good redistributable image drawing library.
        for i in range(thickness):
            draw.rectangle(
                [left + i, top + i, right - i, bottom - i],
                outline=colors[c])
        draw.rectangle(
            [tuple(text_origin), tuple(text_origin + label_size)],
            fill=colors[c])
        draw.text(text_origin, label, fill=(0, 0, 0), font=font)
        del draw

    end = timer()
    print(end - start)
    return image , process_json(predBboxes,jsonP,pathImg), names


def close_session():
    K.clear_session()
