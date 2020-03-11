import numpy as np  # (pip install numpy)
from skimage import measure  # (pip install scikit-image)
from shapely.geometry import Polygon, MultiPolygon  # (pip install Shapely)


def create_sub_mask_annotation(sub_mask, image_id, category_id, annotation_id, is_crowd, rgb_value, class_name):
    # Find contours (boundary lines) around each sub-mask
    # Note: there could be multiple contours if the object
    # is partially occluded. (E.g. an elephant behind a tree)
    contours = measure.find_contours(sub_mask, 0.5, positive_orientation='low')

    segmentations = []
    polygons = []
    for contour in contours:
        # Flip from (row, col) representation to (x, y)
        # and subtract the padding pixel
        for i in range(len(contour)):
            row, col = contour[i]
            contour[i] = (col - 1, row - 1)

        # Make a polygon and simplify it
        poly = Polygon(contour)
        poly = poly.simplify(1.0, preserve_topology=False)
        polygons.append(poly)
        if poly.geom_type == 'Polygon':
            segmentation = np.array(poly.exterior.coords).ravel().tolist()
        else:# some are multipolygon
            segmentation = []
        segmentations.append(segmentation)
    # Combine the polygons to calculate the bounding box and area
    multi_poly = MultiPolygon(polygons)
    if len(multi_poly.bounds) > 0:
        x, y, max_x, max_y = multi_poly.bounds
        width = max_x - x
        height = max_y - y
        bbox = (x, y, width, height)
        area = multi_poly.area
    else:
        x = 0
        y = 0
        width = 0
        height = 0
        bbox = (x, y, width, height)
        area = multi_poly.area

    annotation = {
        'segmentation': segmentations,
        'iscrowd': is_crowd,
        'image_id': image_id,
        'category_id': category_id,
        'object class': class_name,
        'rbg color': rgb_value,
        'id': annotation_id,
        'bbox': bbox,
        'area': area
    }

    return annotation
