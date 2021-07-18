# SolidWorks API
This repo started out as a fork of [SolidDna](https://github.com/angelsix/solidworks-api) by AngelSix. SolidDna is a great framework to build SOLIDWORKS add-ins, because it acts as a wrapper around the core SOLIDWORKS API. If a SOLIDWORKS API topic is hard to understand or otherwise annoying, we create a more user-friendly version for it.

Because SolidDna wasn't actively maintained and our proposed changes were effectively ignored, we eventually decided to fork it, apply our improvements and publish the results.

We will be focusing on these topics:

- DONE: Make SolidDna able to run multiple add-ins at the same time.
- DONE: Fix bugs.
- Keep expanding SolidDna so we have to use less and less of the core SOLIDWORKS API.

To do that, we'll probably:

- DONE: Remove the ability to run in a separate app domain.
- DONE: Remove the single add-in from the IOC 
- Merge the add-in and plug-in types.


# Getting Started
Here are some videos by AngelSix on how to get started with developing your own SOLIDWORKS add-ins with C# and SolidDna.

https://www.youtube.com/c/angelsix

https://www.youtube.com/playlist?list=PLrW43fNmjaQVMN1-lsB29ECnHRlA4ebYn
