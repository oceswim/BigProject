import numpy as np

from keras.applications.vgg16 import VGG16
from keras.optimizers import Adam
from keras.layers import Dense
from keras import Sequential, metrics
from keras_preprocessing.image import ImageDataGenerator
from sklearn.metrics import confusion_matrix
import matplotlib.pyplot as plt


def train(pathImg):
    train_path = "vgg_splitData/train"
    valid_path = "vgg_splitData/valid"
    test_path = pathImg
    class_Names = ['aeroplane', 'bicycle', 'bus', 'car', 'motor']
    train_batches = ImageDataGenerator().flow_from_directory(train_path, target_size=(224, 224), classes=class_Names,
                                                             batch_size=10)
    valid_batches = ImageDataGenerator().flow_from_directory(valid_path, target_size=(224, 224), classes=class_Names,
                                                             batch_size=5)
    test_batches = ImageDataGenerator().flow_from_directory(test_path, target_size=(224, 224), classes = class_Names,
                                                            batch_size=10)

    vgg16_model = VGG16()
    model = Sequential()
    for layer in vgg16_model.layers[:-1]:
        model.add(layer)

    for layer in model.layers:
        layer.trainable = False

    # add layer output to predict on 5 classes instead of initial 1000
    model.add(Dense(5, activation='softmax'))

    model.summary()

    model.compile(Adam(lr=.0001), loss='categorical_crossentropy', metrics=['accuracy'])
    model.fit_generator(train_batches, steps_per_epoch=4, validation_data=valid_batches, validation_steps=4, epochs=5,
                        verbose=2)

    test_imgs, test_labels = next(test_batches)
    test_labels = test_labels[:, 0]
    predictions = model.predict_generator(test_batches, steps=1, verbose=0)
    cm = confusion_matrix(test_labels, np.round(predictions[:, 0]))
    fig = plt.figure()
    plt.matshow(cm)
    plt.title('Problem 1: Confusion Matrix Digit Recognition')
    plt.colorbar()
    plt.ylabel('True Label')
    plt.xlabel('Predicated Label')
    fig.savefig('confusion_matrix.jpg')
