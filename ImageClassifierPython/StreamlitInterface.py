import os
import streamlit as st
from PIL import Image
from yoloRunner import run_yolo
from VGG16runner import vgg_pred

def load_images(theDir):
    for entry in os.listdir(theDir):
        if os.path.isfile(os.path.join(theDir, entry)):
            name = os.path.join(theDir, entry)
            img = Image.open(name)
            captio = entry
            st.image(img, width=300, caption=captio)


# side bar where we can select the different part
st.header("Oceane Peretti - CI7520 - Assignment 2")
st.write("Theme chosen : image classification")
st.sidebar.header("Page selection")
st.sidebar.text("Please select which page to view")

# page selection box
pages = ["Page 1", "Page 2", "Page 3", "Page 4"]
pageSelected = st.sidebar.selectbox("Page number", options=pages)
if pageSelected == "Page 1":
    images = []
    st.title("Loading and overview of the data")
    st.write("1) Upload of the dataset:")
    upload = st.radio("Which dataset you want to use", ("Existing", "Your own"))
    if upload == "Existing":
        size = len(os.listdir("Projects/NormalImages"))
        basepath = "Projects/NormalImages"
        if st.button("Load"):
            if (size > 0):
                load_images(basepath)
            else:
                st.warning("Your file seems empty, try again")

    elif upload == "Your own":
        basepath = "Projects" #projects from Unity will be saved here
        folders = []
        for entry in os.listdir(basepath):
            if not os.path.isfile(entry):
                folders.append(entry)
        folderSelected = st.selectbox("Which Project to load?", options=folders)
        if st.button("Select"):
            directory = basepath+"/"+folderSelected+"/NormalImages"
            load_images(directory)

    # add visualisation
elif pageSelected == "Page 2":
    st.title("Deployment of pre-trained models")
    existingModel = ["Yolo", "vgg16", "Model3"]
    st.write("1) Select a pre-trained model")
    modelSelected = st.radio("", options=existingModel)
    message = ("You selected: {}".format(modelSelected))
    st.success(message)
    st.write("2) Upload your dataset:")
    uploaded_img = st.file_uploader(label="")
    if st.button("Submit"):
        if uploaded_img is not None:
            if modelSelected == 'Yolo':
                text = []
                result, text = run_yolo(uploaded_img)
                st.image(result, use_column_width=True, caption="Objects found: {}".format(text))
            elif modelSelected == 'vgg16':
                image = Image.open(uploaded_img)
                st.image(image, use_column_width=True)
                st.write("Classifying...")
                label = vgg_pred(uploaded_img)
                st.write('%s (%.2f%%)' % (label[1], label[2] * 100))
        else:
            st.warning("Something went wrong")

    # add deploy button + visualisation
elif pageSelected == "Page 3":
    st.title("Training and Deployment of existing DNNs")
    existingDNNs = ["YOLO", "SDD", "VGG"]
    st.write("1) Select an existing DNN")
    dnnSelected = st.radio("", options=existingDNNs)
    message = ("You selected: {}".format(dnnSelected))
    st.success(message)
    st.write("1)Upload your dataset:")

    # add train and deploy buttons + visualisation
elif pageSelected == "Page 4":
    st.title("Design/training and deployment of my own DNNs")
    myDNNs = ["My DNN 1", "My DNN 2", "My DNN 3"]
    st.write("1) Select one of the created DNNs")
    myDnnSelected = st.radio("", options=myDNNs)
    message = ("You selected: {}".format(myDnnSelected))
    st.success(message)
    st.write("2) Upload your dataset:")
    st.file_uploader(label="")
    # add train and deploy buttons + visualisation
