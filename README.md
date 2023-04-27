# How to Run
- Recommended Unity Editor Version: 2022.2.7f1
- In Unity Hub Click Open drop down and Select ``Add Project From Disk`` And select the root folder of this project
- Once Opened there are some demo scene in the ``Assets/Scenes/`` folder which can be found in the 'Project' Tab/Window
- In the Hierarchy tab, all the objects in the scene can be found their and modified
- The shader is only in effect when the ``Play`` button at the center-top is clicked
- Scripts can be found in ``Assets/Scripts/`` 
- The shader code can be found in ``Assets/Shader/``

# Raymarch Cam
- This script is for the camera. It has various Parameter
- ``Raymarch``: The raymarch shader should be here already
- ``Ambient Occlusion``: Toggles AO on and off
- ``Fog On``: Toggles the Fog
- ``Fog Rate``: Controls that amount of fog/how fast objects become foggy
- ``Glow On``: Toggles the Glow Effect

# Raymarch Light
- Objects with this script act as the light for the scene
- ``Light``: This control what type of light it is
- ``Color``: This control the color of the light
- ``Cut Off`: This is an half angle for the spot light 

# Raymarch Shape
- Objects with this script are the object rendered in the scene
- ``Shape``: This controls what type of shape should be rendered
- ``Behaviour``: This controls the boolean operation that is applied on the shape. ``Default`` and ``Blend`` are both Union and ``Wrap`` is just for fun
- `Ambient`: This is the Ambient material of the object
- `Diffuse`: This is the Diffuse material of the object
- `Specular`: This is the Specular material of the object
- `Blend`: Controls how strong/range of the Blend Behaviour
- `Power`: Controls the fractal 'density' amount