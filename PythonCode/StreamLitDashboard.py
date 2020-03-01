import streamlit as st

#side bar where we can select the different part
st.header("Oceane Peretti - CI7520 - Assignment 2")
st.write("Theme chosen : image classification")
st.sidebar.header("Page selection")
st.sidebar.text("Please select which page to view")

#page selection box
pages = ["Page 1","Page 2","Page 3","Page 4"]
pageSelected = st.sidebar.selectbox("Page number",options=pages)
if pageSelected == "Page 1":
    st.title("Load and overview of the data")
    st.write("1) Upload of the dataset:")
    st.file_uploader(label="")
    #add visualisation
elif pageSelected == "Page 2":
    st.title("Deployment of pre-trained models")
    existingModel = ["Model1", "Model2", "Model3"]
    st.write("1) Select a pre-trained model")
    modelSelected = st.radio("", options=existingModel)
    message = ("You selected: {}".format(modelSelected))
    st.success(message)
    st.write("2) Upload your dataset:")
    st.file_uploader(label="")
    # add deploy button + visualisation
elif pageSelected == "Page 3":
    st.title("Training and Deployment of existing DNNs")
    existingDNNs = ["YOLO", "SDD", "VGG"]
    st.write("1) Select an existing DNN")
    dnnSelected = st.radio("", options=existingDNNs)
    message = ("You selected: {}".format(dnnSelected))
    st.success(message)
    st.write("1)Upload your dataset:")
    st.file_uploader(label="")
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
    #add train and deploy buttons + visualisation