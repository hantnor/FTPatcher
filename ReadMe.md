By https://value.gay for hantnor
Copyright 2024 ValueFactory https://shader.gay

Batch files, showing proof of purchase, and TOS concerns are a thing of the past. With **FTPatcher**, you can include an in-editor pop-up window to easily patch edits to your model! No Unity experience necessary for the recipient.

### Note
This is listed primarily for the use of **face tracking**, but this can be used for any model edits! Use this to your heart's content. :)

# Set-up:
 1. Create patch using hdiffz.exe: https://github.com/sisong/HDiffPatch 
 - Run the following line in a command line: 
 ```hdiffz.exe "NONFTOriginal.fbx" "FTEdited.fbx" patch.bin``` 
 - Both FBX files should be in the same directory as the hdiffz.exe executable. 
    - The 'NONFTOriginal.FBX' file is the original FBX that was edited to have the FT blendshapes. 
    - The 'FTEdited.FBX' file the edited FBX that has the FT blendshapes. This is the FBX that will be produced when the original FBX is patched.
    - 'patch.bin' is the file path to the patch binary.
 2. Set up the package folder for the model you're making the patch for.
 3. Configure the Orchestrator script to work with the FBX. You can search for '@Config' in the file to find all the places where things need to be adjusted for the model.

## Terms and Licensing
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
associated documentation files (the â€œSoftwareâ€), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute,
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or
substantial portions of the Software.

THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.