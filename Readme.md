<img src="logo.webp" width=200 height=200 />

This library provides a set of classes to work with splines.

There are also helper types to aid with geometric calculations.

## Dimensions

The types are available in 1D to 4D versions:

| Dimension | Description                                                                                                                      |
|-----------|----------------------------------------------------------------------------------------------------------------------------------|
| 1D        | Describe the movement of an object along a line (forward-backward movement)                                                      |
| 2D        | Describe the movement of an object in a plane                                                                         |
| 3D        | Describe the movement of an object in space                                                                   |
| 4D        | Describe the movement of an object in space<br/>Additional W-Axis that describe the twisting/rotation along a curve |

## Continuity

When splicing together multiple splines, it is important to consider the continuity of the splines.

This is important when considering visual artifacts, or especially when using splines for robotics control, where higher order continuity is required to ensure smooth motion.

The following table shows the continuity of some common spline types.

| Spline Type        | Continuity | Description                                                         |
|--------------------|------------|---------------------------------------------------------------------|
| Linear Spline      | C0         | continuous function                                                 |
| Bezier Spline      | C0         | specific cases may be higher; ie., cubic Bezier can be C2           |
| Quadratic Spline   | C1         | continuous first derivative                                         |
| Hermite Spline     | C1         | continuous first derivative                                         |
| Catmull-Rom Spline | C1         | continuous first derivative                                         |
| Cardinal Spline    | C1         | continuous first derivative                                         |
| Cubic Spline       | C2         | continuous second derivative                                        |
| B-Spline           | varies     | depends on degree and knot vector, typically C2 for cubic B-splines |
| NURBS              | varies     | depends on degree and knot vector, typically C2 for cubic NURBS     |

## Spline properties

| Spline Type        | Properties                                                                                                                                                                                                                                 |
|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Linear Spline      | Piecewise linear functions<br/>Continuous but not smooth<br/>Does not capture curvature                                                                                                                                                    |
| Bezier Spline      | Defined by control points, typically cubic<br/>Only the first and last control points are interpolated<br/>Smooth and continuous<br/>Local control by control points                                                                                                              |
| Quadratic Spline   | Piecewise quadratic functions<br/>Continuous first derivative<br/>Captures basic curvature                                                                                                                                                                                 |
| Hermite Spline     | Defined using derivatives (tangents) at control points<br/>Piecewise cubic<br/>Continuous first derivative                                                                                                                                                                         |
| Catmull-Rom Spline | Special case of Hermite spline (tangents are automatically calculated)<br/>Interpolates through control points<br/>Smooth and continuous                                                                                                                                           |
| Cardinal Spline    | Similar to Catmull-Rom but with adjustable tension parameter<br/>Continuous and smooth<br/>Interpolates through control points                                                                                                                                                     |
| Cubic Spline       | Piecewise cubic functions<br/>Continuous first and second derivatives<br/>Captures more complex curvature                                                                                                                                                                              |
| B-Spline           | Piecewise polynomial functions<br/>Generalization of cubic splines (can be of any degree)<br/>Local control (change in one part of the spline affects only that part)<br/>Smoothness can be controlled<br/>Knot vector determines the spline<br/>Captures complex curvature |
| NURBS              | Extension of B-splines with rational functions<br/>Can represent both standard analytical shapes and freeform shapes<br/>Allows for varying weights<br/>Very flexible and powerful                                                          |

## Use cases

| Spline Type        | Use Case                                                                                                                  |
|--------------------|---------------------------------------------------------------------------------------------------------------------------|
| Linear Spline      | Simple and fast interpolation<br/>Data visualization with simple trends<br/>Connecting points in a straightforward manner |
| Bezier Spline      | Simple and intuitive curve design<br/>Graphic design and vector graphics<br/>Animation<br/>User interface design |
| Quadratic Spline   | Slightly more accurate than linear for interpolation<br/>Useful when the data trend is relatively simple<br/>Graphics applications |
| Hermite Spline     | Smooth interpolation with control over tangents<br/>Robotics and path planning<br/>Keyframe animation |
| Catmull-Rom Spline | Easy to use and intuitive for curve modeling<br/>Computer graphics<br/>Animation paths and camera movements |
| Cardinal Spline    | Flexible interpolation with tension control<br/>Graphics and data visualization<br/>Path and trajectory planning |
| Cubic Spline       | Smooth interpolation<br/>Engineering and scientific applications<br/>Data fitting where smoothness is required<br/>Computer graphics |
| B-Spline           | Flexible and robust for modeling complex shapes<br/>CAD/CAM applications<br/>Image processing<br/>Animation paths |
| NURBS              | High precision modeling<br/>CAD, CAM, and CAE applications<br/>Complex surfaces in computer graphics<br/>Animation and rendering |

## Helper types

| Folder            | Description                                                                                                                |
|-------------------|----------------------------------------------------------------------------------------------------------------------------|
| Geometric Algebra | Bivector, Trivector, Multivector, etc. for geometric calculations<br/>This types are used in vector algebra                |
| Geometric Shapes  | Ray/Line/Line Segment, Polygons, Planes, circle, etc.<br/>Used for geometric calculations and calculation of intersections |
| Numerics          | Specialized Vector and Matrix types                                                                                        |
| Interpolation     | Types for interpolating data points using splines                                                                          |
| Outlier Handling  | Used to detect and remove outliers from a curve                                                                            |
