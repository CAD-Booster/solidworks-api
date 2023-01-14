# SolidWorks Command Manager Icon Generator
This tool combines and resizes your icons into a format that SOLIDWORKS understands.

That task is painstaking doing it manually, so this tool simply accepts a list of images and combines the icons into image files of all possible sizes.

# What it does
SolidWorks Command Manager tabs (for toolbars and Tools menus) need a list of images, called Sprites. Each image must contain all of the icons for that menu group in a particular size. An example:

![icon sprite, each icon being 20x20 pixels](icons20.png)
![icon sprite, each icon being 32x32 pixels](icons32.png)
![icon sprite, each icon being 40x40 pixels](icons40.png)

This tool creates six PNG images, one for each icon size supported by SOLIDWORKS:
- 20x20 pixels
- 32x32 pixels
- 40x40 pixels
- 64x64 pixels
- 96x96 pixels
- 128x128 pixels

# How to use
First, open the solution (.sln) file and build the solution in Release mode. This creates an .exe file in the bin/Release folder.

## Method 1: Drag and drop
1. Select your images. The order in which you select the images matters. The image you select first appears on the left.
1. Drop your images onto the exe.
1. The tool creates six images with the filename icons{size}.png.

## Method 2: Command line
1. Double-click the .exe file.
1. Enter the filename of your first icon and press enter.
1. Keep entering filenames until you're done. Enter an empty filename to go to the next step.
1. Enter a name for your files. It will append the icon size. If you enter "icons" it creates files like "icons20.png". Press enter.
1. The command line closes and creates six images.
