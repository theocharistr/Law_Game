# Car Accident Dynamics Script

This script calculates vehicle collision dynamics using various methodologies, such as McHenry's and Prasad's energy deformation methods. It also converts these calculations into equivalent barrier speeds while considering various conditions like road surface status, humidity, vehicle category, and braking efficiency.

## Features

- **Energy Deformation Calculations**: Uses McHenry's and Prasad's methods.
- **Collision Dynamics**: Accounts for oblique collisions.
- **Speed Calculations**: Computes speed from skid marks and equivalent barrier speed.
- **Environment and Vehicle Parameters**: Considers road surface, humidity, vehicle category, and braking efficiency.

## Inspector Setup

Attach the script to the car in the scene and select the desired inputs from the inspector:

![Inspector Setup](https://github.com/theocharistr/Law_Game/blob/main/CAR_ACCIDENT/Car_inspector.png)

For example, deformation measurements of the frontal area:

![Deformation Measurements](https://github.com/theocharistr/Law_Game/blob/main/CAR_ACCIDENT/Deformation_Measurements.jpg)

## Input Parameters

- **Weight**: Vehicle weight in kilograms.
- **Surface Status**: Enum dropdown.
- **Humidity**: Enum dropdown.
- **Speed Range**: Enum dropdown.
- **Area**: Enum dropdown.
- **Vehicle Category**: Enum dropdown.
- **Deformation Width**: Width of deformation in meters.
- **Roadway Slope**: Slope of the roadway in degrees.
- **Collision Angle**: Angle of collision in degrees.
- **Skidmark Length**: Length of skid marks in meters.
- **Crawl**: Distance crawled in meters.
- **Distance Travelled Without Skidmark**: Distance travelled without skid marks in meters.
- **Braking Efficiency**: Braking efficiency percentage. If the vehicle has left four skid marks, the braking efficiency is 100%. Subtract 20% for each front wheel and 30% for each back wheel that did not leave a skid mark.

## Script Breakdown

### Enums Definitions

Defines enums for different categories:
- Area
- Vehicle_category
- SurfaceStatus
- Humidity
- SpeedRange

### Serialized Fields

Stores various parameters used for calculations:
- Vehicle weight
- Surface status
- Humidity
- Speed range
- Deformation width
- Collision angle
- More...

### Start Method

Initializes the script's logic:
- Prints initial setup information.
- Computes energy deformation using McHenry's method.
- Applies a correction factor for oblique collisions.
- Computes energy deformation using Prasad's method.
- Converts energy deformation to equivalent barrier speed.
- Converts speed to different units.
- Computes speed from skid marks using two different methods.
- Calculates vehicle speed at the start of skid marks.

### Energy Deformation Methods

Contains methods to compute energy deformation:
- `energyDeformation`: Uses McHenry's method.
- `prasad`: Uses Prasad's method.

### Equivalent Barrier Speed Method

Converts deformation energy into equivalent barrier speed.

### Friction Coefficient Class

Contains methods for:
- Calculating the friction coefficient.
- Slope corrections.
- Speeds.
- Stopping distances.

### Speed Convert Class

Converts speeds between different units.

