import os
import streamlit as st
from PIL import Image
from yolo_code import set_yolo_param, generate_training_txt
from VGG16runner import vgg_pred
from groundTruthHandling.multipleImageConversion import convert_ground_truth
from trainingYolo import _train
from VGG16runner import vgg_train
import matplotlib.pyplot as plt


def run_yolo(mdl, listOfPaths, jsonp):
    for x in listOfPaths:
        print(x)
        ima = Image.open(x)
        result, text, objects = set_yolo_param(x, ima, mdl,jsonp)  # getting the image processed with yolo_folder and the f1score
    return result,text,objects

@st.cache
def show_yolo(r,t,o):
    for x, y, z in r, o, t:
        st.image(x, use_column_width=True,
                 caption="Objects found: {} and F1 score: {}".format(y, z))


@st.cache
def show_vgg(hi):
    plt.plot(hi.history["acc"])
    plt.plot(hi.history['val_acc'])
    plt.plot(hi.history['loss'])
    plt.plot(hi.history['val_loss'])
    plt.title("model accuracy")
    plt.ylabel("Accuracy")
    plt.xlabel("Epoch")
    plt.legend(["Accuracy", "Validation Accuracy", "loss", "Validation Loss"])
    return plt


@st.cache
def load_images(theDir):
    paths = []
    for entry in os.listdir(theDir):
        if os.path.isfile(os.path.join(theDir, entry)) and entry != ".DS_Store":
            name = os.path.join(theDir, entry)
            paths.append(name)
    return paths


@st.cache
def get_images(paths):
    imageList = []
    for name in paths:
        if name != ".DS_Store":
            img = Image.open(name)
            imageList.append(img)
    return imageList


def display_folders(path):
    fldrs = [""]
    for entry in os.listdir(path):
        if not os.path.isfile(entry):
            fldrs.append(entry)
    return fldrs


# side bar where we can select the different part

pages = ["","Page 1", "Page 2", "Page 3", "Page 4"]

st.sidebar.title("Oceane Peretti - CI7520 - Assignment 2")
st.sidebar.header("Theme chosen : image classification")
st.sidebar.subheader("Data folder selection:")

basepath = st.sidebar.text_input("Where are your projects located?")

trainedY = False

if basepath:
    if os.path.isdir(basepath):
        folders = display_folders(basepath)
        if len(folders) > 0:
            folderSelected = st.sidebar.selectbox("Which Project to load?", options=folders)
            st.sidebar.header("Page selection")
            st.sidebar.text("Please select which page to view")
            if folderSelected:
                with st.spinner('Loading your data and converting ground truth...(It will take a while)'):
                    imagePathNormal = basepath + "/" + folderSelected + "/NormalImages"
                    imagePathGt = basepath + "/" + folderSelected + "/"
                    listOfPathsNormal = load_images(imagePathNormal)
                    NormalImages = get_images(listOfPathsNormal)
                    convert_ground_truth(imagePathGt, folderSelected)

                # page selection box
                pageSelected = st.sidebar.selectbox("Page number", options=pages)
                if pageSelected == "Page 1":
                    st.title("Overview of the data")
                    st.subheader("The following images were loaded:")
                    st.image(NormalImages, width=150)
                    # add visualisation
                elif pageSelected == "Page 2":
                    st.title("Deployment of pre-trained models")
                    existingModel = ["", "YOLO", "VGG16"]
                    st.write("1) Select a pre-trained model")
                    modelSelected = st.selectbox("", options=existingModel)
                    message = ("You selected: {}".format(modelSelected))
                    st.success(message)
                    if modelSelected == 'YOLO':
                        jsonPath = basepath + "/" + folderSelected + "/" + folderSelected + ".json"
                        modelP = "models_trained_preTrained/yolo/yolo.h5"
                        results, texts, objects = run_yolo(modelP, listOfPathsNormal)
                        show_yolo(results,texts,objects)
                    elif modelSelected == 'VGG16':
                        for s in listOfPathsNormal:
                            image = Image.open(s)
                            imageName = s.split("/")[11]
                            st.write("Classifying ", imageName)
                            trained = False
                            label = vgg_pred(s,trained)
                            if trained is False:
                                st.image(image, use_column_width=True, caption='%s (%.2f%%)' % (label[1], label[2] * 100))
                            else:
                                st.image(image, use_column_width=True, caption='%s' % label)
                    elif modelSelected == 'retinanet':
                        st.write("To come")
                    # add deploy button + visualisation
                elif pageSelected == "Page 3":
                    st.title("Training and Deployment of existing DNNs")
                    existingDNNs = ["", "YOLO", "VGG16"]
                    st.write("1) Select an existing DNN")
                    dnnSelected = st.selectbox("", options=existingDNNs)
                    if dnnSelected == 'YOLO':
                        jsonPath = basepath + "/" + folderSelected + "/" + folderSelected + ".json"
                        generate_training_txt(imagePathNormal, jsonPath)
                        trainYolo = st.button("Train Yolo")
                        loadYolo = st.button("Deploy Yolo")
                        trainAndLoad = st.button("Train and deploy Yolo")
                        if trainAndLoad:
                            modelPath = "models_trained_preTrained/yolo/trainedModel.h5"
                            _train(modelPath)
                            jsonPath = basepath + "/" + folderSelected + "/" + folderSelected + ".json"
                            modelP = basepath + "/" + folderSelected + "/trainedModel.h5"
                            with st.spinner("Yolo is being trained"):
                                res, obj, txt = run_yolo(modelP, listOfPathsNormal, jsonPath)
                                show_yolo(res,obj,txt)
                        if trainYolo:
                            modelPath = "models_trained_preTrained/yolo/trainedModel.h5"
                            _train(modelPath)
                        if loadYolo:
                            jsonPath = basepath + "/" + folderSelected + "/" + folderSelected + ".json"
                            modelP = "models_trained_preTrained/yolo/trainedModel.h5"
                            if os.path.isfile(modelP):
                                res, obj, txt = run_yolo(modelP, listOfPathsNormal, jsonPath)
                                show_yolo(res,obj,txt)
                            else:
                                st.warning("Train yolo first")
                    elif dnnSelected == "VGG16":
                        trainVgg = st.button("Train VGG")
                        loadVgg = st.button("Deploy VGG")
                        trainAndLoad = st.button("Train and Deploy VGG16")
                        if trainAndLoad:
                            hist = vgg_train("vgg_splitData/valid","vgg_splitData/train")
                            for i in hist:
                                pl = show_vgg(i)
                                st.pyplot(pl)
                            for s in listOfPathsNormal:
                                image = Image.open(s)
                                imageName = s.split("/")[11]
                                st.write("Classifying ", imageName)
                                label = vgg_pred(s, True)
                                st.image(image, use_column_width=True,
                                         caption='%s (%.2f%%)' % (label[1], label[2] * 100))
                        if trainVgg:
                            hist = vgg_train("vgg_splitData/valid", "vgg_splitData/train")
                            for i in hist:
                                pl = show_vgg(i)
                                st.pyplot(pl)
                        if loadVgg:
                            modelPath = "models_trained_preTrained/vgg/vgg_fine_tuned_model.h5"
                            if os.path.isfile(modelPath):
                                for s in listOfPathsNormal:
                                    image = Image.open(s)
                                    imageName = s.split("/")[11]
                                    st.write("Classifying ", imageName)
                                    label = vgg_pred(s,True)
                                    st.image(image, use_column_width=True,
                                             caption='%s (%.2f%%)' % (label[1], label[2] * 100))
                            else:
                                st.warning("Train vgg first")


        else:
            st.sidebar.warning("Please enter a proper path for your folders")
    else:
        st.error("Something is wrong with the path entered.")
