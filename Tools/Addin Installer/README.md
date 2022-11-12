# SolidWorks Add-in Installer
This tool helps you to install and uninstall SolidWorks add-ins using RegAsm. Use this during development to quickly register different add-ins or different versions. 

NOTE: This application is designed to run on x64 machines and x64 installs of SolidWorks by default

# How to register an add-in when building an installer
Do not use this tool or RegAsm in an installer when you roll out an add-in product. In that case, you need to register your add-in by adding Windows Registry keys yourself. Every installer tool has its own way of adding registry keys.

Add these steps to your installer:
- Register your public classes:
  - This may include your add-in, your plugin, task pane host and task pane window.
  - Make sure to mark your classes as 64-bit, if your installer has a setting for that, or they end up in the wrong part of the registry.
  - The classes are registered in HKEY_CLASSES_ROOT by their name, and also in HKEY_CLASSES_ROOT\CLSID by their GUID
- Add a key in HKEY_LOCAL_MACHINE\SOFTWARE\SolidWorks\AddIns with the GUID of your add-in in curly braces
  - For example "{37C3FB94-2FE0-47DC-A555-1D4291D0A20D}"
- Add these values to the key you created in the previous step:
  - Description - is required
  - Title - is required
  - Icon path - is optional - contains the full path to the icon you want to show in the list of add-ins within SolidWorks. You can even set a path for each icon size: https://help.solidworks.com/2022/english/api/sldworksapiprogguide/overview/Add-in_Icons.htm