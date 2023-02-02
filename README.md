# Prototype Tools for Flax Engine

This plugin adds some very basic tools to help you blockout a level in the Flax Engine. Every actor is easily placeable in a scene with easily changeable parameters for each different object.

Currently it has:
- Cubes
- Cylinders
- Ramps
- Linear Stairs

![flaxprototype](https://user-images.githubusercontent.com/38583668/216321377-95a5c424-c61f-4d03-9464-99ead2f84a74.jpg)

I plan on adding more as they are needed but I felt that these were the basics of getting any basic level blocked out.

## Installation

1. Clone this repo into `<game-project>\Plugins\PrototypeTools`

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

3. Restart Editor.

4. Try it out!
