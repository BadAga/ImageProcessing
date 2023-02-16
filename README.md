# ImageProcessing
This WPF project is an application with a graphical user interface and the MVC architectural pattern for bitmap processing through Gamma correction.

## Getting started
First, download the WPF project. Then, install the required dependencies and open the project in your chosen IDE. Finally, build the project, and you are ready to begin using the application.

## Features
This WPF project offers the following features:
- Gamma correction for bitmap processing

- Option to execute the implemented algorithm with a C# DLL or assembly DLL

- Selection of the number of threads

- Selection of Gamma exponent

- Execution time indicator

- Easy to use user interface

- Saving corrected image

## Usage

To use the application correctly, first select picture to be corrected via file dialog then the option to execute the implemented algorithm with either a C# DLL or assembly DLL. Next you can choose number of threads processing the image and value of gamma exponent (you don't have to). Default values are consecutively: 2.2 for gamma exponent and a number of processors available to the current process for number of threads ([see how i got this number](https://learn.microsoft.com/en-us/dotnet/api/system.environment.processorcount?view=net-7.0)). Finally, you can apply the gamma correction to the bitmap by clicking <em>Run</em> button and view the corrected image and execution time.

Here's what you'll see after setting up and running the correction: 

![app_view](https://user-images.githubusercontent.com/72341763/219368735-d63e101c-42c6-4c22-b7e3-57286a452510.png)
