# Input

## Installing

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
