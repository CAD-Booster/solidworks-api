# SolidDna
## A user-friendly framework for SOLIDWORKS add-ins
SolidDna is a great framework to build SOLIDWORKS add-ins because it acts as a wrapper around the core SOLIDWORKS API. If a SOLIDWORKS API topic is hard to understand or otherwise annoying, we create a more user-friendly version for it. 

We'd love your help to keep expanding and improving this project.

# Getting Started

## NuGet Package
[CADBooster.SolidDna on NuGet](https://www.nuget.org/packages/CADBooster.SolidDna)

## AngelSix videos
Here are some videos by AngelSix on how to get started with developing your own SOLIDWORKS add-ins with C# and SolidDna. They are a little dated by now, but we got started this way.

- Channel: https://www.youtube.com/c/angelsix
- Playlist: https://www.youtube.com/playlist?list=PLrW43fNmjaQVMN1-lsB29ECnHRlA4ebYn

## CAD Booster articles
We're writing a series of articles about getting started with the SOLIDWORKS API. We're starting with the absolute basics and are trying to include all weird API behaviors beginners may trip over. Because the API docs will not teach you those tricks.

- [The SOLIDWORKS Object Model + API explained](https://cadbooster.com/the-solidworks-object-model-api-explained-part-1/)
- [SOLIDWORKS API: the basics – SldWorks, ModelDoc2](https://cadbooster.com/solidworks-api-basics-sldworks-modeldoc2/)
- [How to work with Features ](https://cadbooster.com/how-to-work-with-features-in-the-solidworks-api/)
- [Persistent ID and sketch segment ID and more](https://cadbooster.com/persistent-id-sketch-segment-id-in-the-solidworks-api/)
- [All identifiers in the SOLIDWORKS API](https://cadbooster.com/all-identifiers-and-ids-in-the-solidworks-api/)
- [About return values](https://cadbooster.com/about-return-values-in-the-solidworks-api-part-6/)
- [Entities and GetCorresponding](https://cadbooster.com/entities-and-getcorresponding-in-the-solidworks-api/)
- Selections, math and custom properties (coming soon)
- How to create task panes and Property Manager Pages (coming soon)
- How to create your own SOLIDWORKS add-in (coming soon)
- Creating add-ins with SolidDna: the basics (coming soon)

# About this fork
This repository is a fork of [SolidDna](https://github.com/angelsix/solidworks-api) by AngelSix. Because SolidDna wasn't actively maintained and our proposed fixes were effectively ignored, we eventually decided to fork it, apply our improvements and publish the results. 

Since then, we fixed a load of bugs, made SolidDna capable of running multiple add-ins at the same time and strong-name signed the NuGet package. Signing allows multiple versions of SolidDna to be loaded at the same time.

To achieve that, we removed the dependency injection (because all add-ins run in the same thread), removed running in a separate app domain (because it exposed SOLIDWORKS bugs) and removed the reference to the Dna Framework (so we don't have to strong-name sign it).

We will keep expanding SolidDna so you and I have to use less and less of the core SOLIDWORKS API. And we'd love your help. Check out the list of issues (or create one yourself) and send us a pull request.

# About CAD Booster
We build intuitive add-ins for SOLIDWORKS to automate the boring bits of engineering. Our main products are [Drew](https://cadbooster.com/solidworks-add-in/drew/) (create 2D drawings twice as fast) and [Lightning](https://cadbooster.com/solidworks-add-in/lightning-fastener-filter/) (makes working with fasteners fun again). 

We use SolidDna in all of our add-ins. 