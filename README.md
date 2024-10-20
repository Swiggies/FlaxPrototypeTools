# Prototype Tools for Flax Engine

This plugin adds some very basic tools to help you blockout a level in the Flax Engine. Every actor is easily placeable in a scene with easily changeable parameters for each different object.

Now finally has SDF generation! You can see how lighting will look while you blockout your level!

## THIS SHOULD ONLY BE USED TO PROTOTYPE LEVELS
Since all the geometry is procedural it gets created at runtime and can slow down games. This is much more true with SDF generation. You have been warned.

Currently it has:
- Cubes
- Cylinders
- Ramps
- Linear Stairs

![image](https://github.com/Swiggies/FlaxPrototypeTools/assets/38583668/a2433b5d-14a2-4afa-a5fd-61af6bc13532)
![flaxprototype](https://user-images.githubusercontent.com/38583668/216321377-95a5c424-c61f-4d03-9464-99ead2f84a74.jpg)

I plan on adding more as they are needed but I felt that these were the basics of getting any basic level blocked out.

## Installation

1. Clone this repo into `<game-project>\Plugins\FlaxPrototypeTools`

2. Add reference to the PrototypeTools project in your game by modyfying your games project file (`<game-project>.flaxproj`) as follows:


```
...
"References": [
    {
        "Name": "$(EnginePath)/Flax.flaxproj"
    },
    {
        "Name": "$(ProjectPath)/Plugins/FlaxPrototypeTools/PrototypeTools.flaxproj"
    }
]
```

3. Restart/Start Editor

4. Try it out! Drag in a prototype actor from the toolbox.
