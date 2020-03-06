import os

mainPath = os.environ['HOME']
project = '/Desktop/COURSFAC/PythonBuild/FixedBuild.app/Contents/Projects'
print(os.listdir(mainPath+""+project))
