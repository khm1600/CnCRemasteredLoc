Read the `.loc` files found in one of Command & Conquer Remastered Collection's `.meg` files.

The file format begins with a uint32 specifying the amount of entries, followed by a table of entry infos then entry content, finally entry tags which are strctured as follows:

```cs
// header
UInt32 entryCount;

// entry info
UInt32 unknown;
UInt32 entryLength; // in characters
UInt32 entryTagLength; // in characters

// entry content
char[] entry; // UTF-16 LE

// entry tags
char[] tag; // UTF-8 or ACSII, too little info to determine
```

All fields are little-endian