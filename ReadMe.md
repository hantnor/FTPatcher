Batch files, showing proof of purchase, and TOS concerns are a thing of the past. With **FTPatcher**, you can include an in-editor pop-up window to easily patch edits to your model! No Unity experience necessary for the recipient.

### Note
This is listed primarily for the use of **face tracking**, but this can be used for any model edits, such as body modification, revamps, and other mesh edits. Use this to your heart's content. :)

# Set-up:
 1. Create a patch file using **hdiffz.exe**: https://github.com/sisong/HDiffPatch
    - In a command prompt, use ```cd``` to route to the directory where **hdiffz.exe** is located.
    - Run the following line in a command line:
 ```hdiffz.exe "NONFTOriginal.fbx" "FTEdited.fbx" patch.bin``` 
    - Both FBX files should be in the same directory as the hdiffz.exe executable. 
       - The 'NONFTOriginal.FBX' file is the original FBX that was edited to have the FT blendshapes. 
       - The 'FTEdited.FBX' file the edited FBX that has the FT blendshapes. This is the FBX that will be produced when the original FBX is patched.
       - 'patch.bin' is the file path to the patch binary.
 2. Set up the package folder for the model you're making the patch for.
    - In this template, the directory is ```FTPatcher/FTRootFolder/```. You can change this if you'd like. **Make sure you update Orchestrator.cs with the proper directory changes!**
 3. Configure the Orchestrator script to work with the FBX. You can search for '@Config' in the file to find all the places where things need to be adjusted for the model.