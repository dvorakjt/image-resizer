# Image Resizer

Image Resizer is a desktop application for resizing images to use in responsive images to improve web application performance. 

Image Resizer is currently under development and not yet fully stable, but it does currently support resizing, compressing, and reformatting images on MacOS and Windows. 

Note that to run the project on Mac, lines 84 - 90 and lines 92 - 98 of the .csproj file must be commented back in so that the native libvips binaries upon which NetVips depends are included in the build. Unfortunately, the inclusion of these NativeReference tags, which is the approach recommended by the NetVips author, has some negative consequences such as confusing Intellisense in JetBrains rider. I will be looking into a more elegant way of managing these dependencies, but for the time being this is the only method that I have found to be effective. Conversely, on Windows, these lines MUST remain commented out.

## Overview

The general idea behind Image Resizer is to take a large image, potentially in a less efficient file format like PNG, and convert it into a bunch of smaller images in more efficient formats.

Available formats are AVIF (typically the most efficient format), WebP (still very efficient but a little bit more widely supported) and JPEG (the most efficient format that has universal support).

The widths of the smaller files correspond to what you might include in the srcset attribute of a responsive image. As such, you can define these widths using three strategies: densities, widths, and media queries. 

### Densities

When using the densities strategy to define the image widths that Image Resizer should generate, you provide a base width, and select some densities, ranging from 1x to 4x. These reflect the DPI of the user's device. 1x corresponds to the base width you entered. This width is then multiplied by each of the selected densities and a correspondingly-sized image is generated. Finally, you enter a default width which is used as the fallback in the event that none of the densities you selected matches that of the user's device.

### Widths

When using widths mode, you specify screen widths that act as thresholds at which to display a different image width. This corresponds to the use of the sizes attribute in conjunction with the srcset attribute of a responsive image. You can select from two different threshold strategies: max-widths and min-widths. When max-widths is selected, the max-width media query is used in the sizes attribute. Therefore, the default image width should be larger than all of the other provided widths. When min-widths is selected, the opposite is true.

### Media Queries

The media queries strategy is the most powerful and flexible. With this strategy, you define exactly the media queries you want, allowing you to query many different media features to determine what image to display when. This is useful for things like accounting for both highly responsive page layouts and the user's device DPI. 