# Input
This is a simple wrapper built on top of the existing unity input system, created in order to add easier support for changing inputs while the game is running and multiple controller support.

## Installing
To install for use in Unity, copy everything from this repository to `<YourNewUnityProject>/Packages/Popcron.Input` folder.

If using 2018.3.x, you can add a new entry to the manifest.json file in your Packages folder:
```json
"com.popcron.input": "https://github.com/popcron/input.git"
```
After that, add a Controls Manager component onto an object if one doesn't already exist.

## Examples
Checking if both the start and select buttons are pressed:
```cs
bool eval = Controls.GetStart() || Controls.GetSelect();
```

Checking for input using a specific controller:
```cs
bool jumpPlayer1 = Controls.GetButtonDown("Jump");
bool jumpPlayer2 = Controls.GetButtonDown("Jump", 1);
bool jumpPlayer3 = Controls.GetButtonDown("Jump", 2);
```
