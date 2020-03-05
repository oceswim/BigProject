# import the necessary packages
from collections import namedtuple
import numpy as np
import cv2


def get_boxes(pred, gt, imageName):
    sizePred = len(pred)
    sizeGt = len(gt)
    iouList = []
    truePos = 0;
    falsePos = 0;
    falseNeg = 0;
    if sizePred < sizeGt:
        print("no")
        i = 0
        for s in gt:
            for t in pred:
                result = bb_intersection_over_union(s, t)
                iouList.append(result)
        for n in iouList:
            if n >= 0.5:
                truePos += 1
            elif n < 0.5:
                falsePos += 1
        falseNeg = sizeGt - sizePred
        print(truePos)
        print(falsePos)
        print(falseNeg)
        precisionResults = precisionCalculation(truePos, falsePos)
        recallResult = recallCalculation(truePos, falseNeg)
        f1 = get_F1(precisionResults, recallResult)
        print("f1 is: %.2f" % f1)
    elif sizePred == sizeGt:
        print("wow")


def precisionCalculation(Tp, Fp):
    precision = float(Tp / (Tp + Fp))
    print("Precision: ", precision)
    return precision


def recallCalculation(Tp, Fn):
    recall = float(Tp / (Tp + Fn))
    print("Recall: ", recall)
    return recall


def get_F1(precisionVal, recallVal):
    f1 = float(2 * (recallVal * precisionVal) / (recallVal + precisionVal))
    return f1


def bb_intersection_over_union(boxA, boxB):
    # determine the (x, y)-coordinates of the intersection rectangle

    xA = max(boxA[0], boxB[0])
    yA = max(boxA[1], boxB[1])
    xB = min(boxA[2], boxB[2])
    yB = min(boxA[3], boxB[3])

    # compute the area of intersection rectangle
    interArea = max(0, (xB - xA + 1)) * max(0, (yB - yA + 1))
    # compute the area of both the prediction and ground-truth
    # rectangles
    boxAArea = (boxA[2] - boxA[0] + 1) * (boxA[3] - boxA[1] + 1)
    boxBArea = (boxB[2] - boxB[0] + 1) * (boxB[3] - boxB[1] + 1)
    # compute the intersection over union by taking the intersection
    # area and dividing it by the sum of prediction + ground-truth
    # areas - the interesection area
    iou = interArea / float(boxAArea + boxBArea - interArea)

    # return the intersection over union value
    return iou
