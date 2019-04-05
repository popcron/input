# Input

## Installing

## Examples
Checking if both the start and select buttons on any controllers are pressed:
```cs
bool eval = Controls.GetStart() || Controls.GetSelect();
```

Checking for input using the Map
```cs
bool jump = Controls.GetButtonDown("Jump");
```
