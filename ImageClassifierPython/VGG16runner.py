import urllib
from datetime import time
from shutil import copyfile
import os
import random
import keras
import tensorflow
from keras import Sequential, Model, optimizers
from keras.callbacks import ModelCheckpoint, EarlyStopping, TensorBoard
from keras.layers import Flatten, Dense, np, GlobalAveragePooling2D, Dropout
from keras.preprocessing.image import load_img, ImageDataGenerator
from keras.preprocessing.image import img_to_array
from keras.applications.vgg16 import preprocess_input
from keras.applications.vgg16 import decode_predictions
from keras.applications.vgg16 import VGG16


# SSL certificate workaround

def vgg_pred(image1,trained):
    # load the model
    if trained is False:
        model = VGG16()
    else:
        base_model = VGG16(include_top=False, weights=None)
        x = base_model.output
        x = Dense(128)(x)
        x = GlobalAveragePooling2D()(x)
        x = Dropout(0.2)(x)
        predictions = Dense(5, activation='softmax')(x)
        model = Model(inputs=base_model.input, outputs=predictions)

        model.load_weights("models_trained_preTrained/vgg/vgg_fine_tuned_model.h5")

    # load an image from file
    image = load_img(image1, target_size=(224, 224))
    # convert the image pixels to a numpy array
    image = img_to_array(image)
    # reshape data for the model
    image = image.reshape((1, image.shape[0], image.shape[1], image.shape[2]))
    # prepare the image for the VGG model
    image = preprocess_input(image)
    # predict the probability across all output classes
    yhat = model.predict(image)
    # convert the probabilities to class labels
    if trained is True:
        return yhat
    else:
        label = decode_predictions(yhat)
        # retrieve the most likely result, e.g. highest probability
        label = label[0][0]
        # print the classification
        return label


def vgg_train(pathVali,pathTrain):

    train_path = pathTrain
    valid_path = pathVali
    #test_path = pathImg
    #class_Names = ['aeroplane', 'bicycle', 'bus', 'car', 'motor']
    train_batches = ImageDataGenerator(rescale=1./255,
                                       shear_range= .2,
                                       zoom_range = .2,
                                       horizontal_flip=True)
    train_generator= train_batches.flow_from_directory(train_path, target_size=(224, 224),
                                                             class_mode='categorical',
                                                             batch_size=10)
    valid_batches = ImageDataGenerator(rescale=1./255)
    valid_generator= valid_batches.flow_from_directory(valid_path, target_size=(224, 224),
                                                             class_mode='categorical',
                                                             batch_size=5)
    #test_batches = ImageDataGenerator().flow_from_directory(test_path, target_size=(224, 224), classes=class_Names,
    #                                                       batch_size=10)
    hist = []
    loop = 0
    while loop < 2:
        if loop == 0:
            base_model = VGG16(include_top=False,weights='imagenet')
        else :
            base_model = VGG16(include_top=False, weights=None)
        i = 0
        for layer in base_model.layers:
            layer.trainable = False
            i = i + 1
            print(i, layer.name)

        x = base_model.output
        x = Dense(128, activation='softmax')(x)
        x = GlobalAveragePooling2D()(x)
        x = Dropout(0.2)(x)
        predictions = Dense(5, activation='softmax')(x)

        tensorboard = TensorBoard(log_dir="logs/{}".format(time()))
        filepath = 'models_trained_preTrained/vgg/vgg_fine_tuned_model.h5'
        checkpoint = ModelCheckpoint(filepath, monitor='val_loss', verbose=1, save_best_only=True, save_weights_only=False,
                                     mode='auto', period=1)
        callbacks_list = [checkpoint, tensorboard]

        model = Model(inputs=base_model.input, outputs=predictions)
        if loop == 1:
            model.load_weights("models_trained_preTrained/vgg/vgg_fine_tuned_model.h5")
        model.compile(loss="categorical_crossentropy", optimizer=optimizers.SGD(lr=0.001, momentum=0.9),
                      metrics=["accuracy"])

        hist.append(model.fit_generator(
            train_generator,
            steps_per_epoch=10,
            epochs=5,
            callbacks=callbacks_list,
            validation_data=valid_generator,
            validation_steps=20
        ))
        loop += 1
    return hist
