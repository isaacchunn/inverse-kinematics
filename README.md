# Bigaboyzs Framework

Sample game made using Unity 2019.4.1f.

## Members

As of now, the current members are...

| Member Name          | Role                                                           |
|----------------------|----------------------------------------------------------------|
| Isaac Chun           |-                                                               |
| Wong Zheng Yu        |-                                                               |
| Samantha Sim         |-                                                               |
| Liong Wen Xuan       |-                                                               |
| Alson Ang            |-                                                               |
| Sheikh Muhd Zubair   |-                                                               |

> [!WARNING]
> This project is just a test project for improving ourselves, this is in no way polished!

### Code Example
```cs
//Sample code for learning how to code
if(!goodAtCoding)
    becomeGood();   //Call function on becoming good.
```

### Variables Conventions
Variables shall be named in camelCase eg:
```cs
int healthValue = 14;
float interestingVariableToSayTheLeast = 999f;
```

Functions and enums shall be named in PascalCase at all times eg:
```
public void DeductHealth(int damageTaken);

public enum Type
{
    One,
    Two,
    Three
}
```


# Source Control
### Branches
In order to facilitate easier segmentation of development and ensuring that a ready to use version of an application is available at all times, we will be using a branching model inspired by Git Flow.
Branch names should be kept short and succinct in PascalCase and should conform to the format listed below.

### master

Should be a branch with the latest working build for normal projects but smaller projects can use this as the main development branch. Varies across projects. As such, this branch is usually protected. Subject to Maintainers.
Most recent projects will have this branch protected from modifications by non-maintainers.
Merge Requests should be done to merge code from Feature or Integration branches into this branch.

### Feature-*
Branches that are used to implement features and are the main development branches.
E.g. Feature-Networking, Feature-NewUIOverhaul, etc.

### Integration-*
Branches that are used to integrate changes into master or other Feature branches.
E.g. Integration-Networking, Integration-NewUIOverhaul, etc
